using System;
using ZV200Utility.Services.SerialPortScanner.Models;

namespace ZV200Utility.Services.SerialPortScanner
{
    /// <summary>
    /// Представляет сервис для отслеживания изменений состояния последовательного порта.
    /// </summary>
    public interface ISerialPortScanner
    {
        /// <summary>
        /// Указывает, что произошло изменение доступных последовательных портов.
        /// </summary>
        event EventHandler<SerialPortArgs> SerialPortChanged;

        /// <summary>
        /// Запускает сканирование.
        /// </summary>
        void Start();

        /// <summary>
        /// Останавливает сканирование.
        /// </summary>
        void Stop();
    }
}