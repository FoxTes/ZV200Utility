using System.ComponentModel;
using System.Runtime.Serialization;
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
        [Description("Нет")]
        [EnumMember(Value = "НЕТ")]
        Disable,

        /// <summary>
        /// Сигнализация исправности модуля.
        /// </summary>
        [Description("Режим1")]
        [EnumMember(Value = "НЕТ")]
        Work1,

        /// <summary>
        /// Сигнализация присутствия напряжения хотя бы на одной фазе.
        /// </summary>
        [Description("Режим2")]
        [EnumMember(Value = "НЕТ")]
        Work2,

        /// <summary>
        /// Сигнализация присутствия напряжения на любых двух фазах.
        /// </summary>
        [Description("Режим3")]
        [EnumMember(Value = "НЕТ")]
        Work3,

        /// <summary>
        /// Сигнализация присутствия напряжения на трех фазах.
        /// </summary>
        [Description("Режим4")]
        [EnumMember(Value = "НЕТ")]
        Work4,

        /// <summary>
        /// Сигнализация отсутствия напряжения хотя бы на одной фазе.
        /// </summary>
        [Description("Режим5")]
        [EnumMember(Value = "НЕТ")]
        Work5,

        /// <summary>
        /// Сигнализация отсутствия напряжения на любых двух фазах.
        /// </summary>
        [Description("Режим6")]
        [EnumMember(Value = "НЕТ")]
        Work6,

        /// <summary>
        /// Сигнализация отсутствия напряжения на трех фазах.
        /// </summary>
        [Description("Режим7")]
        [EnumMember(Value = "НЕТ")]
        Work7
    }
}