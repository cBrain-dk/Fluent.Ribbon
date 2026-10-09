namespace Fluent.Tests.Services;

using System.Windows.Input;
using Fluent.Tests.TestClasses;
using FluentTest.Commanding;
using NUnit.Framework;

[TestFixture]
public class KeyTipServiceHostContractTests
{
    [Test]
    public void Attach_DirectChildItemsControlHost_AltShowsKeyTipsAndFocusesHost()
    {
        // Arrange
        var host = new KeyTipServiceTestHost();
        host.Items.Add(new Button { KeyTip = "A" });
        using var context = new KeyTipServiceTestContext(host, new KeyTipService(host));

        // Act
        context.PressKey(Key.LeftAlt);

        // Assert
        Assert.That(context.Service.AreAnyKeyTipsVisible, Is.True);
        Assert.That(host.IsKeyboardFocusWithin, Is.True);
    }

    [TestCase(Key.Escape)]
    [TestCase(Key.LeftAlt)]
    public void KeyTips_Cancelled_ReturnsFocusOutsideHost(Key cancellationKey)
    {
        // Arrange
        var host = new KeyTipServiceTestHost();
        host.Items.Add(new Button { KeyTip = "A" });
        using var context = new KeyTipServiceTestContext(host, new KeyTipService(host));
        context.PressKey(Key.LeftAlt);
        Assert.That(context.Service.AreAnyKeyTipsVisible, Is.True);
        Assert.That(host.IsKeyboardFocusWithin, Is.True);

        // Act
        context.PressKey(cancellationKey);

        // Assert
        Assert.That(context.Service.AreAnyKeyTipsVisible, Is.False);
        Assert.That(Keyboard.FocusedElement, Is.SameAs(context.OutsideFocusTarget));
    }

    [Test]
    public void KeyTips_MatchingDirectChild_ExecutesCommandOnceAndReturnsFocusOutsideHost()
    {
        // Arrange
        var executions = 0;
        var host = new KeyTipServiceTestHost();
        host.Items.Add(new Button
        {
            KeyTip = "A",
            Command = new RelayCommand(() => executions++)
        });
        using var context = new KeyTipServiceTestContext(host, new KeyTipService(host));
        context.PressKey(Key.LeftAlt);
        Assert.That(context.Service.AreAnyKeyTipsVisible, Is.True);
        Assert.That(host.IsKeyboardFocusWithin, Is.True);

        // Act
        context.PressKey(Key.A);

        // Assert
        Assert.That(executions, Is.EqualTo(1));
        Assert.That(context.Service.AreAnyKeyTipsVisible, Is.False);
        Assert.That(Keyboard.FocusedElement, Is.SameAs(context.OutsideFocusTarget));
    }

    [Test]
    public void KeyTips_UnmatchedKeyInHostRoot_DismissesAndReturnsFocusOutsideHost()
    {
        // Arrange
        var host = new KeyTipServiceTestHost();
        host.Items.Add(new Button { KeyTip = "A" });
        using var context = new KeyTipServiceTestContext(host, new KeyTipService(host));
        context.PressKey(Key.LeftAlt);
        Assert.That(context.Service.AreAnyKeyTipsVisible, Is.True);

        // Act
        context.PressKey(Key.Q);

        // Assert
        Assert.That(context.Service.AreAnyKeyTipsVisible, Is.False);
        Assert.That(Keyboard.FocusedElement, Is.SameAs(context.OutsideFocusTarget));
    }

    [Test]
    public void Detach_ThenAttachAgain_OnlyAttachedServiceRespondsToAlt()
    {
        // Arrange
        var host = new KeyTipServiceTestHost();
        host.Items.Add(new Button { KeyTip = "A" });
        using var context = new KeyTipServiceTestContext(host, new KeyTipService(host));
        context.Service.Detach();
        context.PressKey(Key.LeftAlt);
        Assert.That(context.Service.AreAnyKeyTipsVisible, Is.False);

        // Act
        context.Service.Attach();
        context.PressKey(Key.LeftAlt);

        // Assert
        Assert.That(context.Service.AreAnyKeyTipsVisible, Is.True);
    }

    [Test]
    public void Attach_RibbonConstructor_AltStillShowsAndEscapeDismissesKeyTips()
    {
        // Arrange
        var ribbon = new Ribbon { Menu = new Backstage { KeyTip = "B" } };
        // Disable only Ribbon's built-in service so the public constructor is tested independently.
        ribbon.KeyTipKeys.Clear();
        using var context = new KeyTipServiceTestContext(ribbon, new KeyTipService(ribbon));
        context.PressKey(Key.LeftAlt);
        Assert.That(context.Service.AreAnyKeyTipsVisible, Is.True);

        // Act
        context.PressKey(Key.Escape);

        // Assert
        Assert.That(context.Service.AreAnyKeyTipsVisible, Is.False);
        Assert.That(Keyboard.FocusedElement, Is.SameAs(context.OutsideFocusTarget));
    }
}
