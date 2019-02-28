using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Fluent.Converters
{
    public class ReadOnlyConverter : IValueConverter
    {

        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FrameworkElement frameworkElement = value as FrameworkElement;
            IRibbonControl c = value as IRibbonControl;

            if (c != null)
                return !c.IsReadOnly;
            if(frameworkElement!=null)
                return frameworkElement.IsEnabled;

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
