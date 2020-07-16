namespace Fluent
{
    using System.Windows;

    /// <summary>
    /// Interface for injecting custom object to image conversion
    /// </summary>
    public interface ICustomObjectToImagerConverter
    {
        /// <summary>
        /// Convert an object into a FrameworkElement which represents a picture
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        FrameworkElement TryConvert(object value);
    }
}
