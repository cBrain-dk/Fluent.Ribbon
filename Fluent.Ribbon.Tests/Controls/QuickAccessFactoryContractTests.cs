namespace Fluent.Tests.Controls;

using NUnit.Framework;

[TestFixture]
public class QuickAccessFactoryContractTests
{
    [Test]
    public void CreateQuickAccessItem_ComboBoxFactoryOverride_UsesDerivedInstanceAndInheritedSetup()
    {
        // Arrange
        var source = new FactoryComboBox { IsEditable = true };

        // Act
        var item = source.CreateQuickAccessItem();

        // Assert
        Assert.That(source.FactoryCalls, Is.EqualTo(1));
        Assert.That(item, Is.SameAs(source.FactoryResult));
        Assert.That(item, Is.Not.SameAs(source));
        Assert.That(source.FactoryResult.IsEditable, Is.True);
    }

    [Test]
    public void CreateQuickAccessItem_SplitButtonFactoryOverride_UsesDerivedInstanceAndInheritedSetup()
    {
        // Arrange
        var source = new FactorySplitButton();

        // Act
        var item = source.CreateQuickAccessItem();

        // Assert
        Assert.That(source.FactoryCalls, Is.EqualTo(1));
        Assert.That(item, Is.SameAs(source.FactoryResult));
        Assert.That(item, Is.Not.SameAs(source));
        Assert.That(source.FactoryResult.CanAddButtonToQuickAccessToolBar, Is.False);
        Assert.That(RibbonProperties.GetSize(source.FactoryResult), Is.EqualTo(RibbonControlSize.Small));
    }

    private sealed class FactoryComboBox : ComboBox
    {
        public DerivedComboBox FactoryResult { get; } = new();

        public int FactoryCalls { get; private set; }

        protected override ComboBox CreateQuickAccessComboBox()
        {
            this.FactoryCalls++;
            return this.FactoryResult;
        }
    }

    private sealed class DerivedComboBox : ComboBox
    {
    }

    private sealed class FactorySplitButton : SplitButton
    {
        public DerivedSplitButton FactoryResult { get; } = new();

        public int FactoryCalls { get; private set; }

        protected override SplitButton CreateQuickAccessSplitButton()
        {
            this.FactoryCalls++;
            return this.FactoryResult;
        }
    }

    private sealed class DerivedSplitButton : SplitButton
    {
    }
}
