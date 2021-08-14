using ZV200Utility.Core.Enums;

namespace ZV200Utility.Services.DeviceManager.Model
{
    /// <summary>
    /// Предоставляет данные для настроек прибора.
    /// </summary>
    public readonly struct SettingDeviceArgs
    {
        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="SettingDeviceArgs"/>.
        /// </summary>
        /// <param name="relayFunction">Функция реле.</param>
        /// <param name="relayLogic">Логика реле.</param>
        /// <param name="soundFunction">Функция звуковой сигнализации открытия двери.</param>
        /// <param name="inputDiscreteLogic">Логика работы дискретного входа.</param>
        public SettingDeviceArgs(RelayOperatingMode relayFunction, bool relayLogic, bool soundFunction, bool inputDiscreteLogic)
        {
            RelayFunction = relayFunction;
            RelayLogic = relayLogic;
            SoundFunction = soundFunction;
            InputDiscreteLogic = inputDiscreteLogic;
        }

        /// <summary>
        /// Функция реле.
        /// </summary>
        public RelayOperatingMode RelayFunction { get; }

        /// <summary>
        /// Логика реле.
        /// </summary>
        public bool RelayLogic { get; }

        /// <summary>
        /// Функция звуковой сигнализации открытия двери.
        /// </summary>
        public bool SoundFunction { get; }

        /// <summary>
        /// Логика работы дискретного входа.
        /// </summary>
        public bool InputDiscreteLogic { get; }
    }
}
