namespace Fluent.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    ///     Converts <c>null</c> or empty string to <c>true</c> and not <c>null</c> or empty string to <c>false</c>.
    /// </summary>
    public sealed class IsNullOrEmptyStringConverter : IValueConverter
    {
        private static IsNullOrEmptyStringConverter instance;

        /// <summary>
        ///     A singleton instance for <see cref="IsNullOrEmptyStringConverter" />.
        /// </summary>
        public static IsNullOrEmptyStringConverter Instance => instance ?? (instance = new IsNullOrEmptyStringConverter());

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                return string.IsNullOrEmpty(text);
            }

            return value == null;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}