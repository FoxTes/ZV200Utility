using ZV200Utility.Core.Enums;

namespace ZV200Utility.Modules.Setting.Models
{
    /// <summary>
    /// Класса, описывающий элемент для режима работы.
    /// </summary>
    public class RelayOperatingItem
    {
        /// <summary>
        /// Имя режима работы.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание режима работы.
        /// </summary>
        public string ToolTipItem { get; set; }

        /// <summary>
        /// Режим работы реле.
        /// </summary>
        public RelayOperatingMode RelayOperatingMode { get; set; }
    }
}