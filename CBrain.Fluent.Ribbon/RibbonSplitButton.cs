namespace CBrain.Fluent.Ribbon;

using System.Windows;

/// <summary>
/// Provides the cBrain ribbon SplitButton control.
/// </summary>
public class RibbonSplitButton : global::Fluent.SplitButton
{
    /// <inheritdoc />
    public override FrameworkElement CreateQuickAccessItem()
    {
        var item = base.CreateQuickAccessItem();
        global::Fluent.RibbonProperties.SetIsElementInQuickAccessToolBar(item, true);
        return item;
    }

    /// <inheritdoc />
    protected override global::Fluent.SplitButton CreateQuickAccessSplitButton()
    {
        return new RibbonSplitButton();
    }
}
