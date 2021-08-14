namespace ZV200Utility.Modules.StatusRelays.Models
{
    /// <summary>
    /// Модель, предоставляющая данные о реле.
    /// </summary>
    public readonly struct Phase
    {
        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="Phase"/>.
        /// </summary>
        /// <param name="status">Статус концевика.</param>
        public Phase(bool status)
        {
            Status = status;
        }

        /// <summary>
        /// Статус концевика.
        /// </summary>
        public bool Status { get; }
    }
}