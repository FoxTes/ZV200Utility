using ZV200Utility.Core.Enums;

namespace ZV200Utility.Services.DeviceManager.Model
{
    /// <summary>
    /// Предоставляет данные для настроек подключения к прибору.
    /// </summary>
    public readonly struct SettingModbusArgs
    {
        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="SettingModbusArgs"/>.
        /// </summary>
        /// <param name="addressDevice">Адрес прибора.</param>
        /// <param name="serialPortSelected">Имя последовательно порта.</param>
        /// <param name="baudRate">Скорость последовательного порта.</param>
        public SettingModbusArgs(
            byte addressDevice,
            string serialPortSelected,
            BaudRate baudRate)
        {
            AddressDevice = addressDevice;
            SerialPort = serialPortSelected;
            BaudRate = baudRate;
        }

        /// <summary>
        /// Адрес прибора в сети MODBUS [1-247].
        /// </summary>
        public byte AddressDevice { get; }

        /// <summary>
        /// Имя последовательно порта.
        /// </summary>
        public string SerialPort { get; }

        /// <summary>
        /// Скорость последовательного порта.
        /// </summary>
        public BaudRate BaudRate { get; }
    }
}