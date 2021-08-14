namespace ZV200Utility.Services.SerialPortScanner.Enums
{
    /// <summary>
    /// Определяет действие, вызвавшее событие SerialPortChanged.
    /// </summary>
    public enum SerialPortAction
    {
        /// <summary>
        /// Добавлен последовательный порт.
        /// </summary>
        Add,

        /// <summary>
        /// Удален последовательный порт.
        /// </summary>
        Remove
    }
}