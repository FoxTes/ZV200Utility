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
        /// Периодический вызов принудительного подключения к порту.
        /// </summary>
        event EventHandler<string[]> SerialPortPolled;

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