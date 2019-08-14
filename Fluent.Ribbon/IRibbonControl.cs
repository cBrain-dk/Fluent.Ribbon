namespace Fluent
{
    /// <summary>
    /// Base interface for Fluent controls
    /// </summary>
    public interface IRibbonControl : IHeaderedControl, IKeyTipedControl, ILogicalChildSupport, IReadOnlyControl, IIconedControl
    {
        /// <summary>
        /// Gets or sets Size for the element
        /// </summary>
        RibbonControlSize Size { get; set; }

        /// <summary>
        /// Gets or sets SizeDefinition for element
        /// </summary>
        RibbonControlSizeDefinition SizeDefinition { get; set; }
    }
}