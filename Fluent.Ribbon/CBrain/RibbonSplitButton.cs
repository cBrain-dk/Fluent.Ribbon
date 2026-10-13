namespace Fluent.CBrain;

using System.Windows;

/// <summary>
/// Provides the cBrain ribbon SplitButton control.
/// </summary>
public class RibbonSplitButton : Fluent.SplitButton
{
    /// <inheritdoc />
    public override FrameworkElement CreateQuickAccessItem()
    {
        var item = base.CreateQuickAccessItem();
        Fluent.RibbonProperties.SetIsElementInQuickAccessToolBar(item, true);
        return item;
    }

    /// <inheritdoc />
    protected override Fluent.SplitButton CreateQuickAccessSplitButton()
    {
        return new RibbonSplitButton();
    }
}
