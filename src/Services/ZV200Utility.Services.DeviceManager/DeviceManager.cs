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
using ZV200Utility.Services.DeviceManager.Helpers;
using ZV200Utility.Services.DeviceManager.Model;
using ZV200Utility.Services.Notification;
using ZV200Utility.Services.SerialPortScanner;
using ZV200Utility.Services.SerialPortScanner.Enums;
using ZV200Utility.Services.SerialPortScanner.Models;
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

            SetDefaultValue();
            serialPortScanner.SerialPortChanged += OnSerialPortScannerOnSerialPortChanged;
            serialPortScanner.Start();
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
            var dataWrite = new[]
            {
                (ushort)settingDevice.RelayFunction,
                Convert.ToUInt16(settingDevice.RelayLogic),
            };
            await _modbusSerialMaster.WriteMultipleRegistersAsync(
                SettingModbus.AddressDevice,
                (ushort)RegisterAddress.RelayFunction,
                dataWrite);

            var dataWrite1 = new[]
            {
                Convert.ToUInt16(settingDevice.SoundFunction),
                Convert.ToUInt16(settingDevice.InputDiscreteLogic)
            };
            await _modbusSerialMaster.WriteMultipleRegistersAsync(
                SettingModbus.AddressDevice,
                (ushort)RegisterAddress.SoundFunction,
                dataWrite1);

            SettingDevice = settingDevice;
        }

        private async void OnTimer(object state)
        {
            var readRegisterStatusBus = new ushort[3];
            var readRegisterStatusOther = new ushort[2];

            try
            {
                readRegisterStatusBus = await _modbusSerialMaster.ReadHoldingRegistersAsync(
                    SettingModbus.AddressDevice,
                    (ushort)RegisterAddress.BusA,
                    3);

                readRegisterStatusOther = await _modbusSerialMaster.ReadHoldingRegistersAsync(
                    SettingModbus.AddressDevice,
                    (ushort)RegisterAddress.RelayStatus,
                    2);
            }
            catch (TimeoutException)
            {
                Close();
                await _notification.ShowAsync(
                    "Подключение",
                    "Устройство не отвечает на чтение регистров.",
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

            var argsList = readRegisterStatusBus
                .Concat(readRegisterStatusOther)
                .Select((x, index) => new SensorInfoArgs(index, x != 0))
                .ToList();
            RegistersRequested?.Invoke(this, argsList);
        }

        private void SetDefaultValue()
        {
            SettingModbus = new SettingModbusArgs(
                byte.Parse(_configuration["AddressModbusSelected"] ?? "1"),
                GetPortNames().FirstOrDefault(x => x == _configuration["ComPortSelected"]) ?? GetPortNames().First(),
                FastEnum.GetValues<BaudRate>()
                    .FirstOrDefault(x => x.GetEnumMemberValue() == _configuration["BaudRateSelected"]));
        }

        private async void OnSerialPortScannerOnSerialPortChanged(object sender, SerialPortArgs args)
        {
            if (args.SerialPortAction != SerialPortAction.Add || StatusConnect != StatusConnect.Disconnected)
                return;
            if (!args.SerialPorts.Contains(SettingModbus.SerialPort))
                return;

            try
            {
                await _notification.ShowAsync(
                    "Подключение",
                    "Попытка подключения к прибору.",
                    NotificationType.Information);
                await Open();
                await _notification.ShowAsync(
                    "Подключение",
                    "Установлено подключение с прибором.",
                    NotificationType.Information);
            }
            catch (TimeoutException)
            {
                Close();
                await _notification.ShowAsync(
                    "Подключение",
                    "Не удалось установить соединение с прибором.\nУстройство не отвечает на чтение регистров.",
                    NotificationType.Warning);
            }
            catch (Exception ex)
            {
                Close();
                await _notification.ShowAsync(
                    "Подключение",
                    $"Не удалось установить соединение с прибором.\n{ex.Message}",
                    NotificationType.Warning);
            }
        }

        private async Task Open()
        {
            _serialPort.PortName = SettingModbus.SerialPort;
            _serialPort.BaudRate = int.Parse(SettingModbus.BaudRate.GetEnumMemberValue()!);
            _serialPort.Open();

            _serialPortAdapter = new SerialPortAdapter(_serialPort)
            {
                ReadTimeout = 1000,
                WriteTimeout = 1000
            };
            _modbusSerialMaster = _modbusFactory.CreateRtuMaster(_serialPortAdapter);

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
            StopTimer();

            StatusConnect = StatusConnect.Disconnected;
            StatusConnectChanged?.Invoke(this, null!);
        }

        private async Task GetSettingDevice()
        {
            var readRegisterSetting = await _modbusSerialMaster.ReadHoldingRegistersAsync(
                SettingModbus.AddressDevice,
                (ushort)RegisterAddress.RelayFunction,
                2);

            var readRegisterSetting1 = await _modbusSerialMaster.ReadHoldingRegistersAsync(
                SettingModbus.AddressDevice,
                (ushort)RegisterAddress.RelayFunction,
                2);

            SettingDevice = new SettingDeviceArgs(
                FastEnum
                    .GetValues<RelayOperatingMode>()
                    .FirstOrDefault(x => x == (RelayOperatingMode)readRegisterSetting[0]),
                readRegisterSetting[1] != 0,
                readRegisterSetting1[0] != 0,
                readRegisterSetting1[1] != 0);
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