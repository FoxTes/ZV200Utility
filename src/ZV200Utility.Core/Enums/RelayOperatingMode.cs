using System.ComponentModel;
using System.Runtime.Serialization;
using FastEnumUtility;
using ZV200Utility.Core.Converters;

namespace ZV200Utility.Core.Enums
{
    /// <summary>
    /// Режим работы реле.
    /// </summary>
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum RelayOperatingMode
    {
        /// <summary>
        /// Не определена.
        /// </summary>
        [Label("НEТ")]
        [EnumMember(Value = "Описание режима НЕТ.")]
        Disable,

        /// <summary>
        /// Сигнализация исправности модуля.
        /// </summary>
        [Label("Режим1")]
        [EnumMember(Value = "Описание режима номер 1.")]
        Work1,

        /// <summary>
        /// Сигнализация присутствия напряжения хотя бы на одной фазе.
        /// </summary>
        [Label("Режим2")]
        [EnumMember(Value = "Описание режима номер 2.")]
        Work2,

        /// <summary>
        /// Сигнализация присутствия напряжения на любых двух фазах.
        /// </summary>
        [Label("Режим3")]
        [EnumMember(Value = "Описание режима номер 3.")]
        Work3,

        /// <summary>
        /// Сигнализация присутствия напряжения на трех фазах.
        /// </summary>
        [Label("Режим4")]
        [EnumMember(Value = "Описание режима номер 4.")]
        Work4,

        /// <summary>
        /// Сигнализация отсутствия напряжения хотя бы на одной фазе.
        /// </summary>
        [Label("Режим5")]
        [EnumMember(Value = "Описание режима номер 5.")]
        Work5,

        /// <summary>
        /// Сигнализация отсутствия напряжения на любых двух фазах.
        /// </summary>
        [Label("Режим6")]
        [EnumMember(Value = "Описание режима номер 6.")]
        Work6,

        /// <summary>
        /// Сигнализация отсутствия напряжения на трех фазах.
        /// </summary>
        [Label("Режим7")]
        [EnumMember(Value = "Описание режима номер 7.")]
        Work7
    }
}