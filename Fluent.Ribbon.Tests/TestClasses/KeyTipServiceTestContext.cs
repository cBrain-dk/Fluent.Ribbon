namespace Fluent.Tests.TestClasses;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fluent.Tests.Helper;
using NUnit.Framework;

internal sealed class KeyTipServiceTestContext : IDisposable
{
    public KeyTipServiceTestContext(Control host, KeyTipService service)
    {
        this.Service = service;
        this.OutsideFocusTarget = new TextBox();
        var panel = new StackPanel();
        panel.Children.Add(host);
        panel.Children.Add(this.OutsideFocusTarget);
        this.Window = new TestRibbonWindow(panel);
        try
        {
            this.Window.Activate();
            this.Window.UpdateLayout();
            UIHelper.DoEvents();
            Assert.That(this.Window.IsActive, Is.True, "Keytip tests require an active WPF window.");
            Assert.That(this.OutsideFocusTarget.Focus(), Is.True);
            Assert.That(Keyboard.FocusedElement, Is.SameAs(this.OutsideFocusTarget));
            this.Service.Attach();
        }
        catch
        {
            this.Dispose();
            throw;
        }
    }

    public TestRibbonWindow Window { get; }

    public TextBox OutsideFocusTarget { get; }

    public KeyTipService Service { get; }

    public void PressKey(Key key)
    {
        var source = PresentationSource.FromVisual(this.Window);
        this.Window.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice, source, 0, key)
        {
            RoutedEvent = Keyboard.PreviewKeyDownEvent
        });
        this.Window.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice, source, 0, key)
        {
            RoutedEvent = Keyboard.KeyUpEvent
        });
        UIHelper.DoEvents();
    }

    public void Dispose()
    {
        this.Service.Detach();
        this.Window.Dispose();
    }
}
