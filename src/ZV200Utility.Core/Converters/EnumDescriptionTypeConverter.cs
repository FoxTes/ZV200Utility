using System;
using System.ComponentModel;

namespace ZV200Utility.Core.Converters
{
    /// <summary>
    /// Конвертер, преобразующий атрибуты, указанные в перечислении, в название.
    /// </summary>
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EnumDescriptionTypeConverter"/>.
        /// </summary>
        /// <param name="type">Тип перечисления.</param>
        public EnumDescriptionTypeConverter(Type type)
            : base(type)
        {
        }

        /// <inheritdoc />
        public override object ConvertTo(
            ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value,
            Type destinationType)
        {
            if (destinationType != typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);

            if (value == null)
                return string.Empty;

            var fi = value.GetType().GetField(value.ToString()!);
            if (fi == null)
                return string.Empty;

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 && !string.IsNullOrEmpty(attributes[0].Description)
                ? attributes[0].Description
                : value.ToString();
        }
    }
}