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
        [EnumMember(Value = "Не определена.")]
        Disable,

        /// <summary>
        /// Сигнализация исправности модуля.
        /// </summary>
        [Label("Режим 1")]
        [EnumMember(Value = "Сигнализация исправности модуля.")]
        Work1,

        /// <summary>
        /// Сигнализация присутствия напряжения хотя бы на одной фазе.
        /// </summary>
        [Label("Режим 2")]
        [EnumMember(Value = "Сигнализация присутствия напряжения хотя бы на одной фазе.")]
        Work2,

        /// <summary>
        /// Сигнализация присутствия напряжения на любых двух фазах.
        /// </summary>
        [Label("Режим 3")]
        [EnumMember(Value = "Сигнализация присутствия напряжения на любых двух фазах.")]
        Work3,

        /// <summary>
        /// Сигнализация присутствия напряжения на трех фазах.
        /// </summary>
        [Label("Режим 4")]
        [EnumMember(Value = "Сигнализация присутствия напряжения на трех фазах.")]
        Work4,

        /// <summary>
        /// Сигнализация отсутствия напряжения хотя бы на одной фазе.
        /// </summary>
        [Label("Режим 5")]
        [EnumMember(Value = "Сигнализация напряжения хотя бы на одной фазе.")]
        Work5,

        /// <summary>
        /// Сигнализация отсутствия напряжения на любых двух фазах.
        /// </summary>
        [Label("Режим 6")]
        [EnumMember(Value = "Сигнализация отсутствия напряжения на любых двух фазах.")]
        Work6,

        /// <summary>
        /// Сигнализация отсутствия напряжения на трех фазах.
        /// </summary>
        [Label("Режим 7")]
        [EnumMember(Value = "Сигнализация отсутствия напряжения на трех фазах.")]
        Work7
    }
}