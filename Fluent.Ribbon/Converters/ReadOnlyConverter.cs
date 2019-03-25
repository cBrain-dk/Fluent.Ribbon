namespace Fluent.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converter for calculating enabled state based on a parent control (which may or may not implement IsReadOnly)
    /// </summary>
    public class ReadOnlyConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IRibbonControl c)
            {
                return !c.IsReadOnly;
            }

            if (value is FrameworkElement frameworkElement)
            {
                return frameworkElement.IsEnabled;
            }

            return true;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        #endregion

    }
}
