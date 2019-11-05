namespace Fluent
{
    /// <summary>
    /// Interface to identify the controls that are acting items within tabcontainers
    /// </summary>
    public interface ITabItem
    {
        /// <summary>
        /// Determines whether item is currently selected
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Selector owning item
        /// </summary>
        ITabContainer TabControlParent { get; }
    }
}
