namespace Fluent
{
    /// <summary>
    /// Interface to identify the controls that are acting items within selectors
    /// </summary>
    public interface ISelectableItem
    {
        /// <summary>
        /// Determines whether item is currently selected
        /// </summary>
        bool IsSelected { get; set; }
    }
}
