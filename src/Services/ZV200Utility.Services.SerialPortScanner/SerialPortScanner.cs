using System;
using System.IO.Ports;
using System.Linq;
using System.Management;
using Microsoft.Extensions.Logging;
using ZV200Utility.Services.SerialPortScanner.Enums;
using ZV200Utility.Services.SerialPortScanner.Models;

namespace ZV200Utility.Services.SerialPortScanner
{
    /// <inheritdoc />
    public class SerialPortScanner : ISerialPortScanner
    {
        private readonly ILogger _logger;

        private ManagementEventWatcher _arrival;
        private ManagementEventWatcher _removal;
        private string[] _serialPorts;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SerialPortScanner"/>.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        public SerialPortScanner(ILogger logger)
        {
            _logger = logger;
            _serialPorts = GetAvailableSerialPorts();

            MonitoringDeviceChanges();
        }

        /// <inheritdoc />
        public event EventHandler<SerialPortArgs> SerialPortChanged;

        /// <inheritdoc />
        public void Start()
        {
            _arrival.Start();
            _removal.Start();

            _logger.LogTrace("Остановлено сканирование последовательных портов");
        }

        /// <inheritdoc />
        public void Stop()
        {
            _arrival.Stop();
            _removal.Stop();

            _logger.LogTrace("Запущено сканирование последовательных портов");
        }

        private void MonitoringDeviceChanges()
        {
            try
            {
                var deviceArrivalQuery
                    = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
                var deviceRemovalQuery
                    = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");

                _arrival = new ManagementEventWatcher(deviceArrivalQuery);
                _removal = new ManagementEventWatcher(deviceRemovalQuery);

                _arrival.EventArrived += (o, args) => RaisePortsChangedIfNecessary(SerialPortAction.Add);
                _removal.EventArrived += (sender, eventArgs) => RaisePortsChangedIfNecessary(SerialPortAction.Remove);
            }
            catch (ManagementException ex)
            {
                _logger.LogError("Ошибка при запуске сканирования последовательных портов: {@Ex}", ex);
            }
        }

        private void RaisePortsChangedIfNecessary(SerialPortAction serialPortAction)
        {
            lock (_serialPorts)
            {
                var availableSerialPorts = GetAvailableSerialPorts();
                if (_serialPorts.SequenceEqual(availableSerialPorts))
                    return;

                _serialPorts = availableSerialPorts;
                SerialPortChanged?.Invoke(null, new SerialPortArgs(serialPortAction, _serialPorts));
            }
        }

        private string[] GetAvailableSerialPorts() => SerialPort.GetPortNames();
    }
}