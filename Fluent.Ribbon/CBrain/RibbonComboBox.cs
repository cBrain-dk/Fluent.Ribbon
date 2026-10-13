namespace Fluent.CBrain;

using System.Windows;

/// <summary>
/// Provides the cBrain ribbon ComboBox control.
/// </summary>
public class RibbonComboBox : Fluent.ComboBox
{
    /// <inheritdoc />
    public override FrameworkElement CreateQuickAccessItem()
    {
        var item = base.CreateQuickAccessItem();
        Fluent.RibbonProperties.SetIsElementInQuickAccessToolBar(item, true);
        return item;
    }

    /// <inheritdoc />
    protected override Fluent.ComboBox CreateQuickAccessComboBox()
    {
        return new RibbonComboBox();
    }
}
