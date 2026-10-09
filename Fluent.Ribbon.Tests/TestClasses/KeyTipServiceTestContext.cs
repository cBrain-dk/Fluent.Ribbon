namespace Fluent.Tests.TestClasses;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fluent.Tests.Helper;
using NUnit.Framework;

internal sealed class KeyTipServiceTestContext : IDisposable
{
    public KeyTipServiceTestContext(Control host, Fluent.KeyTipService service)
    {
        this.Service = service;
        this.OutsideFocusTarget = new TextBox();
        var panel = new StackPanel();
        panel.Children.Add(host);
        panel.Children.Add(this.OutsideFocusTarget);
        this.Window = new Window
        {
            Title = "Fluent keytip service contract tests",
            Width = 640,
            Height = 300,
            ShowActivated = true,
            Content = panel
        };
        try
        {
            this.Window.Show();
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

    public Window Window { get; }

    public TextBox OutsideFocusTarget { get; }

    public Fluent.KeyTipService Service { get; }

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
        try
        {
            this.Service.Detach();
        }
        finally
        {
            this.Window.Close();
            UIHelper.DoEvents();
        }
    }
}
