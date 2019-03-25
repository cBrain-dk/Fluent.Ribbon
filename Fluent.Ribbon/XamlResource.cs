namespace Fluent
{
    /// <summary>
    /// Placeholder object for an image defined as a path
    /// </summary>
    public class XamlResource
    {
        /// <summary>
        /// Name of the datatemplate representing the object
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The data loaded from the datatemplate
        /// </summary>
        public XamlResource(string key)
        {
            this.Key = key;
        }
    }
}
