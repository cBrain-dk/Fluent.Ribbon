namespace Fluent.CBrain.Tests.Controls;

using System.Threading;
using NUnit.Framework;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class QuickAccessItemTests
{
    [Test]
    public void CreateQuickAccessItem_RibbonComboBox_ReturnsFreshWrapperWithInheritedSetup()
    {
        // Arrange
        var source = new RibbonComboBox
        {
            IsEditable = true,
            Text = "Quick access text"
        };

        // Act
        var item = source.CreateQuickAccessItem();

        // Assert
        Assert.That(item, Is.TypeOf<RibbonComboBox>());
        Assert.That(item.GetType().Assembly, Is.SameAs(typeof(Fluent.ComboBox).Assembly));
        Assert.That(item, Is.Not.SameAs(source));
        Assert.That(Fluent.RibbonProperties.GetIsElementInQuickAccessToolBar(item), Is.True);

        var comboBox = (RibbonComboBox)item;
        Assert.That(comboBox.IsEditable, Is.True);
        Assert.That(comboBox.Text, Is.EqualTo("Quick access text"));
    }

    [Test]
    public void CreateQuickAccessItem_RibbonSplitButton_ReturnsFreshWrapperWithInheritedSetup()
    {
        // Arrange
        var source = new RibbonSplitButton
        {
            IsCheckable = true
        };

        // Act
        var item = source.CreateQuickAccessItem();

        // Assert
        Assert.That(item, Is.TypeOf<RibbonSplitButton>());
        Assert.That(item.GetType().Assembly, Is.SameAs(typeof(Fluent.SplitButton).Assembly));
        Assert.That(item, Is.Not.SameAs(source));
        Assert.That(Fluent.RibbonProperties.GetIsElementInQuickAccessToolBar(item), Is.True);
        Assert.That(Fluent.RibbonProperties.GetSize(item), Is.EqualTo(Fluent.RibbonControlSize.Small));

        var splitButton = (RibbonSplitButton)item;
        Assert.That(splitButton.CanAddButtonToQuickAccessToolBar, Is.False);
        Assert.That(splitButton.IsCheckable, Is.True);
    }

    [Test]
    public void CreateQuickAccessItem_RibbonGroupBox_ReturnsFreshWrapperWithInheritedSetup()
    {
        // Arrange
        var source = new RibbonGroupBox
        {
            Header = "Actions",
            LauncherText = "More actions",
            IsLauncherVisible = true
        };

        // Act
        var item = source.CreateQuickAccessItem();

        // Assert
        Assert.That(item, Is.TypeOf<RibbonGroupBox>());
        Assert.That(item.GetType().Assembly, Is.SameAs(typeof(Fluent.RibbonGroupBox).Assembly));
        Assert.That(item, Is.Not.SameAs(source));
        Assert.That(Fluent.RibbonProperties.GetIsElementInQuickAccessToolBar(item), Is.True);

        var groupBox = (RibbonGroupBox)item;
        Assert.That(groupBox.State, Is.EqualTo(Fluent.RibbonGroupBoxState.QuickAccess));
        Assert.That(groupBox.Header, Is.EqualTo("Actions"));
        Assert.That(groupBox.LauncherText, Is.EqualTo("More actions"));
        Assert.That(groupBox.IsLauncherVisible, Is.True);
    }
}
