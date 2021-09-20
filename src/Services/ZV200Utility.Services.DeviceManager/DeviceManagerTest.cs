using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEnumUtility;
using Microsoft.Extensions.Configuration;
using Notifications.Wpf.Core;
using ZV200Utility.Core.Enums;
using ZV200Utility.Services.DeviceManager.Model;
using ZV200Utility.Services.Notification;
using ZV200Utility.Services.SerialPortScanner;
using ZV200Utility.Services.SerialPortScanner.Enums;
using ZV200Utility.Services.SerialPortScanner.Models;

namespace ZV200Utility.Services.DeviceManager
{
    /// <inheritdoc />
    public class DeviceManagerTest : IDeviceManager
    {
        private readonly IConfiguration _configuration;
        private readonly INotification _notification;
        private readonly Timer _timer;

        private int _countOnTime;

        /// <summary>
        /// Test.
        /// </summary>
        /// <param name="serialPortScanner">1</param>
        /// <param name="configuration">2</param>
        /// <param name="notification">3</param>
        public DeviceManagerTest(
            ISerialPortScanner serialPortScanner,
            IConfiguration configuration,
            INotification notification)
        {
            _configuration = configuration;
            _notification = notification;
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
            await Task.Delay(500);
            SettingDevice = settingDevice;
        }

        private async void OnTimer(object state)
        {
            try
            {
                if (_countOnTime++ > 20)
                {
                    _countOnTime = 0;
                    throw new TimeoutException();
                }
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

            var argsList = Enumerable
                .Range(0, 6)
                .Select((x, index) => new SensorInfoArgs(index, x != 0))
                .ToList();
            RegistersRequested?.Invoke(this, argsList);
        }

        private void SetDefaultValue()
        {
            SettingModbus = new SettingModbusArgs(
                byte.Parse(_configuration["AddressModbusSelected"] ?? "1"),
                SerialPort.GetPortNames().FirstOrDefault(x => x == _configuration["ComPortSelected"]) ?? SerialPort.GetPortNames().First(),
                FastEnum.GetValues<BaudRate>()
                    .FirstOrDefault(x => x.GetEnumMemberValue() == _configuration["BaudRateSelected"]));
        }

        private async void OnSerialPortScannerOnSerialPortChanged(object sender, SerialPortArgs args)
        {
            try
            {
                await Open();
            }
            catch (Exception)
            {
                Close();
            }
        }

        private async Task Open()
        {
            await GetSettingDevice();
            StartTimer();

            StatusConnect = StatusConnect.Connected;
            StatusConnectChanged?.Invoke(this, null!);
        }

        private void Close()
        {
            StopTimer();

            StatusConnect = StatusConnect.Disconnected;
            StatusConnectChanged?.Invoke(this, null!);
        }

        private async Task GetSettingDevice()
        {
            await Task.Delay(500);

            var readRegisterSetting = new ushort[] { 2, 1 };
            var readRegisterSetting1 = new ushort[] { 1, 1 };

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