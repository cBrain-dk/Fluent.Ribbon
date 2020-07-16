namespace FluentTest.Icons
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Fluent;

    internal static class CustomImages
    {
        public static readonly CustomTextImage Red = new CustomTextImage("Red", new SolidColorBrush(Colors.Red));
        public static readonly CustomTextImage Blue = new CustomTextImage("Blue", new SolidColorBrush(Colors.Blue));
    }

    internal class CustomTextImage
    {
        public CustomTextImage(string text, Brush background)
        {
            this.Text = text;
            this.Background = background;
        }

        public string Text { get; }

        public Brush Background { get; }
    }

    internal class CustomImageConverter : ICustomObjectToImagerConverter
    {
        public FrameworkElement TryConvert(object value)
        {
            if (value is CustomTextImage image)
            {
                return new TextBlock
                {
                    Text = image.Text,
                    Background = image.Background,
                    Foreground = new SolidColorBrush(Colors.White)
                };
            }

            return null;
        }
    }
}
