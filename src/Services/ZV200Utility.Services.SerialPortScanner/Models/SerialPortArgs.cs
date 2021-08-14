using ZV200Utility.Services.SerialPortScanner.Enums;

namespace ZV200Utility.Services.SerialPortScanner.Models
{
    /// <summary>
    /// Предоставляет данные для события <see cref="SerialPortScanner.SerialPortChanged"/>.
    /// </summary>
    public readonly struct SerialPortArgs
    {
        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="SerialPortArgs"/>.
        /// </summary>
        /// <param name="serialPortAction">Действие, вызвавшее событие.</param>
        /// <param name="serialPorts">Коллекция имен последовательных портов.</param>
        public SerialPortArgs(SerialPortAction serialPortAction, string[] serialPorts)
        {
            SerialPortAction = serialPortAction;
            SerialPorts = serialPorts;
        }

        /// <summary>
        /// Возвращает коллекцию имен последовательных портов.
        /// </summary>
        public string[] SerialPorts { get; }

        /// <summary>
        /// Возвращает действие, вызвавшее событие.
        /// </summary>
        public SerialPortAction SerialPortAction { get; }
    }
}