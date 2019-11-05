namespace Fluent
{
    /// <summary>
    /// Interface to identify a tab selector
    /// </summary>
    public interface ITabContainer
    {
        /// <summary>
        /// Content currently displaying for the selected item on the container
        /// </summary>
        object SelectedContent { get; }
    }
}
