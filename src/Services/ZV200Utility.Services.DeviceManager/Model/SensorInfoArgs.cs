namespace ZV200Utility.Services.DeviceManager.Model
{
    /// <summary>
    /// Предоставляет данные для события <see cref="DeviceManager.RegistersRequested"/>.
    /// </summary>
    public readonly struct SensorInfoArgs
    {
        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="SensorInfoArgs"/>.
        /// </summary>
        /// <param name="number">Порядковый номер.</param>
        /// <param name="status">Статус.</param>
        public SensorInfoArgs(int number, bool status)
        {
            Number = number;
            Status = status;
        }

        /// <summary>
        /// Порядковый номер.
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Статус.
        /// </summary>
        public bool Status { get; }
    }
}
