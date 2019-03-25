namespace Fluent.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Markup.Primitives;

    /// <summary>
    /// Helper for getting wpf properties
    /// </summary>
    public static class DependencyObjectHelper
    {
        /// <summary>
        /// Yep
        /// </summary>
        /// <param name="element">Control to get dependency properties from</param>
        /// <returns></returns>
        public static List<DependencyProperty> GetDependencyProperties(object element)
        {
            List<DependencyProperty> properties = new List<DependencyProperty>();
            MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(element);
            if (markupObject != null)
            {
                foreach (MarkupProperty mp in markupObject.Properties)
                {
                    if (mp.DependencyProperty != null)
                    {
                        properties.Add(mp.DependencyProperty);
                    }
                }
            }

            return properties;
        }

        /// <summary>
        /// Yep
        /// </summary>
        /// <param name="element">Control to get attached properties from</param>
        /// <returns></returns>
        public static List<DependencyProperty> GetAttachedProperties(object element)
        {
            List<DependencyProperty> attachedProperties = new List<DependencyProperty>();
            MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(element);
            if (markupObject != null)
            {
                foreach (MarkupProperty mp in markupObject.Properties)
                {
                    if (mp.IsAttached)
                    {
                        attachedProperties.Add(mp.DependencyProperty);
                    }
                }
            }

            return attachedProperties;
        }
    }
}
