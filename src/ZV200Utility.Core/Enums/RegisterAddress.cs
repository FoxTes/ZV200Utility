namespace ZV200Utility.Core.Enums
{
    /// <summary>
    /// Адрес регистра в сообщении MODBUS.
    /// </summary>
    public enum RegisterAddress
    {
        /// <summary>
        /// Адрес устройства в сети MODBUS. Функции: 03h, 06h, 10h.
        /// </summary>
        AddressDevice = 0x01,

        /// <summary>
        /// Скорость устройства для порта. Функции: 03h, 06h, 10h.
        /// </summary>
        Speed = 0x02,

        /// <summary>
        /// Настройки паритета устройства. Функции: 03h, 06h, 10h.
        /// </summary>
        Parity = 0x03,

        /// <summary>
        /// Функция реле. Функции: 03h, 06h, 10h.
        /// </summary>
        RelayFunction = 0x10,

        /// <summary>
        /// Логика реле. Функции: 03h, 06h, 10h.
        /// </summary>
        RelayLogic = 0x11,

        /// <summary>
        /// Функция звуковой сигнализации открытия двери. Функции: 03h, 06h, 10h.
        /// </summary>
        SoundFunction = 0x20,

        /// <summary>
        /// Логика работы дискретного входа. Функции: 03h, 06h, 10h.
        /// </summary>
        InputDiscreteLogic = 0x21,

        /// <summary>
        /// Флаг отсутствия/присутствия напряжения на шине А. Функции: 03h.
        /// </summary>
        BusA = 0x30,

        /// <summary>
        /// Флаг отсутствия/присутствия напряжения на шине B. Функции: 03h.
        /// </summary>
        BusB = 0x31,

        /// <summary>
        /// Флаг отсутствия/присутствия напряжения на шине C. Функции: 03h.
        /// </summary>
        BusC = 0x32,

        /// <summary>
        /// Коллективный статус отсутствия/присутствия напряжения на шинах. Функции: 03h.
        /// </summary>
        BusComplex = 0x33,

        /// <summary>
        /// Флаг состояния реле. Функции: 03h.
        /// </summary>
        RelayStatus = 0x40,

        /// <summary>
        /// Флаг состояния дискретного входа. Функции: 03h.
        /// </summary>
        InputDiscreteStatus = 0x41,

        /// <summary>
        /// Версия МПО.
        /// </summary>
        Version = 0x90,
    }
}