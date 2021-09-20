using System;
using System.IO.Ports;
using System.Threading;
using ZV200Utility.Services.SerialPortScanner.Enums;
using ZV200Utility.Services.SerialPortScanner.Models;

namespace ZV200Utility.Services.SerialPortScanner
{
    /// <inheritdoc />
    public class SerialPortScannerTest : ISerialPortScanner
    {
        private readonly Timer _timer;

        /// <summary>
        /// Test class.
        /// </summary>
        public SerialPortScannerTest()
        {
            _timer = new Timer(OnTimer, 0, Timeout.Infinite, Timeout.Infinite);
        }

        /// <inheritdoc />
        public event EventHandler<SerialPortArgs> SerialPortChanged;

        /// <inheritdoc />
        public event EventHandler<string[]> SerialPortPolled;

        /// <inheritdoc />
        public void Start()
        {
            _timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(15));
        }

        /// <inheritdoc />
        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void OnTimer(object state)
        {
            SerialPortChanged?.Invoke(null, new SerialPortArgs(SerialPortAction.Add, SerialPort.GetPortNames()));
        }
    }
}