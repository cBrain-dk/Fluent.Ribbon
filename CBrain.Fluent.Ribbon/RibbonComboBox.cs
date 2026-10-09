namespace CBrain.Fluent.Ribbon;

using System.Windows;

/// <summary>
/// Provides the cBrain ribbon ComboBox control.
/// </summary>
public class RibbonComboBox : global::Fluent.ComboBox
{
    /// <inheritdoc />
    public override FrameworkElement CreateQuickAccessItem()
    {
        var item = base.CreateQuickAccessItem();
        global::Fluent.RibbonProperties.SetIsElementInQuickAccessToolBar(item, true);
        return item;
    }

    /// <inheritdoc />
    protected override global::Fluent.ComboBox CreateQuickAccessComboBox()
    {
        return new RibbonComboBox();
    }
}
