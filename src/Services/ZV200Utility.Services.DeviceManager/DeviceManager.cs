using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEnumUtility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NModbus;
using NModbus.Serial;
using Notifications.Wpf.Core;
using ZV200Utility.Core.Enums;
using ZV200Utility.Services.DeviceManager.Extensions;
using ZV200Utility.Services.DeviceManager.Helpers;
using ZV200Utility.Services.DeviceManager.Model;
using ZV200Utility.Services.Notification;
using ZV200Utility.Services.SerialPortScanner;
using static System.IO.Ports.SerialPort;

namespace ZV200Utility.Services.DeviceManager
{
    /// <inheritdoc />
    public class DeviceManager : IDeviceManager
    {
        private readonly IConfiguration _configuration;
        private readonly INotification _notification;
        private readonly SerialPort _serialPort;
        private readonly ModbusFactory _modbusFactory;
        private readonly Timer _timer;

        private SerialPortAdapter _serialPortAdapter;
        private IModbusSerialMaster _modbusSerialMaster;

        private bool _isConnectAdapter;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DeviceManager"/>.
        /// </summary>
        /// <param name="serialPortScanner">Сканер последовательного порта.</param>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="notification">Уведомления.</param>
        /// <param name="logger">Логгер.</param>
        public DeviceManager(
            ISerialPortScanner serialPortScanner,
            IConfiguration configuration,
            INotification notification,
            ILogger logger)
        {
            _configuration = configuration;
            _notification = notification;

            _serialPort = new SerialPort();
            _modbusFactory = new ModbusFactory(logger: new ModbusSerilog(LoggingLevel.Trace, logger));
            _timer = new Timer(OnTimer, 0, Timeout.Infinite, Timeout.Infinite);

            if (GetPortNames().Any())
            {
                SetDefaultValue();

                serialPortScanner.SerialPortPolled += SerialPortScannerOnSerialPortPolled;
                serialPortScanner.Start();
            }
            else
            {
                notification.ShowAsync(
                    "Последовательный порт",
                    "Внимание!\nПрограмма не обнаружила в система COM порты. Подключение к прибору не будет осуществлено.",
                    NotificationType.Error);
            }
        }

        /// <inheritdoc />
        public event EventHandler<IReadOnlyList<SensorInfoArgs>> RegistersRequested;

        /// <inheritdoc />
        public event EventHandler StatusConnectChanged;

        /// <inheritdoc />
        public StatusConnect StatusConnect { get; private set; } = StatusConnect.Disconnected;

        /// <inheritdoc />
        public SettingModbusArgs SettingModbus { get; set; }

        /// <inheritdoc />
        public SettingDeviceArgs SettingDevice { get; private set; }

        /// <inheritdoc />
        public async Task SetSettingDevice(SettingDeviceArgs settingDevice)
        {
            var buffer = new ushort[2];

            buffer[0] = (ushort)settingDevice.RelayFunction;
            buffer[1] = Convert.ToUInt16(settingDevice.RelayLogic);

            await _modbusSerialMaster.WriteMultipleRegistersAsync(
                SettingModbus.AddressDevice,
                (ushort)RegisterAddress.RelayFunction,
                buffer);

            buffer[0] = Convert.ToUInt16(settingDevice.SoundFunction);
            buffer[1] = Convert.ToUInt16(settingDevice.InputDiscreteLogic);

            await _modbusSerialMaster.WriteMultipleRegistersAsync(
                SettingModbus.AddressDevice,
                (ushort)RegisterAddress.SoundFunction,
                buffer);

            SettingDevice = new SettingDeviceArgs(
                settingDevice.RelayFunction,
                settingDevice.RelayLogic,
                settingDevice.SoundFunction,
                settingDevice.InputDiscreteLogic);
        }

        private async void OnTimer(object state)
        {
            var buffer = new ushort[6];

            try
            {
                buffer = await _modbusSerialMaster.ReadHoldingRegisterRanges(
                    SettingModbus.AddressDevice, RegisterAddress.BusA, RegisterAddress.InputDiscreteStatus);
            }
            catch (TimeoutException)
            {
                _isConnectAdapter = true;
            }
            catch (InvalidOperationException)
            {
                Close();
                await _notification.ShowAsync(
                    "Подключение",
                    $"Не удалось установить соединение с прибором.\nПорт {SettingModbus.SerialPort} был закрыт.",
                    NotificationType.Warning);
            }
            catch (Exception ex)
            {
                Close();
                await _notification.ShowAsync(
                    "Подключение",
                    $"Не удалось прочитать регистры.\n{ex.Message}",
                    NotificationType.Warning);
            }

            var argsList = buffer
                .Select((x, index) => new SensorInfoArgs(index, x != 0))
                .ToList();
            RegistersRequested?.Invoke(this, argsList);
        }

        private void SetDefaultValue()
        {
            SettingModbus = new SettingModbusArgs(
                byte.Parse(_configuration["AddressModbusSelected"] ?? "1"),
                GetPortNames().FirstOrDefault(x => x == _configuration["ComPortSelected"]) ?? GetPortNames().First(),
                FastEnum.GetValues<BaudRate>().FirstOrDefault(x =>
                    x.GetEnumMemberValue() == _configuration["BaudRateSelected"]));
        }

        private async void SerialPortScannerOnSerialPortPolled(object sender, string[] args)
        {
            if (StatusConnect != StatusConnect.Disconnected)
                return;
            if (!args.Contains(SettingModbus.SerialPort))
                return;

            try
            {
                await Open();
            }
            catch (TimeoutException)
            {
                _isConnectAdapter = true;
            }
            catch (Exception)
            {
                Close();
            }
        }

        private async Task Open()
        {
            if (!_isConnectAdapter)
            {
                _serialPort.PortName = SettingModbus.SerialPort;
                _serialPort.BaudRate = int.Parse(SettingModbus.BaudRate.GetEnumMemberValue()!);
                _serialPort.Open();

                _serialPortAdapter = new SerialPortAdapter(_serialPort)
                {
                    ReadTimeout = 125,
                    WriteTimeout = 125
                };
                _modbusSerialMaster = _modbusFactory.CreateRtuMaster(_serialPortAdapter);
            }

            await GetSettingDevice();
            StartTimer();

            StatusConnect = StatusConnect.Connected;
            StatusConnectChanged?.Invoke(this, null!);
        }

        private void Close()
        {
            _modbusSerialMaster?.Dispose();
            _serialPortAdapter?.Dispose();
            _serialPort.Close();
            _isConnectAdapter = false;
            StopTimer();

            StatusConnect = StatusConnect.Disconnected;
            StatusConnectChanged?.Invoke(this, null!);
        }

        private async Task GetSettingDevice()
        {
            var buffer = await _modbusSerialMaster.ReadHoldingRegisterRanges(
                SettingModbus.AddressDevice, RegisterAddress.RelayFunction, RegisterAddress.InputDiscreteLogic);

            SettingDevice = new SettingDeviceArgs(
                (RelayOperatingMode)buffer[0],
                buffer[1] != 0,
                buffer[2] != 0,
                buffer[3] != 0);
        }

        private void StartTimer()
        {
            _timer.Change(TimeSpan.FromSeconds(int.Parse(_configuration["DelayPeriod"] ?? "1")),
                TimeSpan.FromSeconds(int.Parse(_configuration["PollingPeriod"] ?? "1")));
        }

        private void StopTimer()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}