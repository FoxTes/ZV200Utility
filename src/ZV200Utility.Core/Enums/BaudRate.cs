using System.ComponentModel;
using System.Runtime.Serialization;
using ZV200Utility.Core.Converters;

namespace ZV200Utility.Core.Enums
{
    /// <summary>
    /// Скорость последовательной передачи данных COM порта.
    /// </summary>
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum BaudRate
    {
        /// <summary>
        /// 2400 бод.
        /// </summary>
        [Description("2400")]
        [EnumMember(Value = "2400")]
        S2400 = 2400,

        /// <summary>
        /// 9600 бод.
        /// </summary>
        [Description("9600")]
        [EnumMember(Value = "9600")]
        S9600 = 9600,

        /// <summary>
        /// 19200 бод.
        /// </summary>
        [Description("19200")]
        [EnumMember(Value = "19200")]
        S19200 = 19200,

        /// <summary>
        /// 38400 бод.
        /// </summary>
        [Description("38400")]
        [EnumMember(Value = "38400")]
        S38400 = 38400,

        /// <summary>
        /// 57600 бод.
        /// </summary>
        [Description("57600")]
        [EnumMember(Value = "57600")]
        S57600 = 57600,

        /// <summary>
        /// 115200 бод.
        /// </summary>
        [Description("115200")]
        [EnumMember(Value = "115200")]
        S115200 = 115200
    }
}
