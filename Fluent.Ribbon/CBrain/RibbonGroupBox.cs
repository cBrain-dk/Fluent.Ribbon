namespace Fluent.CBrain;

using System.Windows;

/// <summary>
/// Provides the cBrain ribbon group box control.
/// </summary>
public class RibbonGroupBox : Fluent.RibbonGroupBox
{
    /// <inheritdoc />
    public override FrameworkElement CreateQuickAccessItem()
    {
        var item = base.CreateQuickAccessItem();
        Fluent.RibbonProperties.SetIsElementInQuickAccessToolBar(item, true);
        return item;
    }

    /// <inheritdoc />
    protected override Fluent.RibbonGroupBox CreateQuickAccessRibbonGroupBox()
    {
        return new RibbonGroupBox();
    }
}
