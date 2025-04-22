using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Application.Helpers
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the description attribute value for an enum value
        /// </summary>
        public static string GetDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null) return enumValue.ToString();

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : enumValue.ToString();
        }

        /// <summary>
        /// Converts a string to an enum value
        /// </summary>
        public static T ToEnum<T>(this string value) where T : struct
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            return Enum.TryParse<T>(value, true, out var result)
                ? result
                : throw new ArgumentException($"String value '{value}' could not be converted to enum of type {typeof(T).Name}");
        }
    }
}
