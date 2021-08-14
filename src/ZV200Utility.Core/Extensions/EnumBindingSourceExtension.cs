using System;
using System.Windows.Markup;

namespace ZV200Utility.Core.Extensions
{
    /// <summary>
    /// Расширение принимающие тип перечисления, которое создает связываемый список значений перечисления.
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _enumType;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EnumBindingSourceExtension"/>.
        /// </summary>
        /// <param name="enumType">Тип перечисления.</param>
        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType;
        }

        /// <summary>
        /// Тип перечисления.
        /// </summary>
        /// <exception cref="ArgumentException">Тип должен являться перечислением.</exception>
        public Type EnumType
        {
            get => _enumType;
            set
            {
                if (value == _enumType)
                    return;
                if (value != null)
                {
                    var enumType = Nullable.GetUnderlyingType(value) ?? value;
                    if (!enumType.IsEnum)
                        throw new ArgumentException("Type must be for an Enum.");
                }

                _enumType = value;
            }
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_enumType == null)
                throw new InvalidOperationException("The EnumType must be specified.");

            // TODO: Перевести на FastEnum.
            var actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
            var enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == _enumType)
                return enumValues;

            var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}