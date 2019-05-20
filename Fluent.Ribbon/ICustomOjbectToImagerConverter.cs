namespace Fluent
{
    using System;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// Interface for injecting custom object to image conversion
    /// </summary>
    public interface ICustomOjbectToImagerConverter
    {
        /// <summary>
        /// Convert an object into a FrameworkElement which represents a picture
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns></returns>
        FrameworkElement Convert(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
