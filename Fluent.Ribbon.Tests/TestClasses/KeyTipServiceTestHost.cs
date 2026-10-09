namespace Fluent.Tests.TestClasses;

using System.Windows;
using System.Windows.Controls;

internal sealed class KeyTipServiceTestHost : ItemsControl, IKeyTipServiceHost
{
    public KeyTipServiceTestHost()
    {
        this.Focusable = true;
    }

    public bool IsCollapsed { get; set; }

    public bool IsKeyTipHandlingEnabled { get; set; } = true;

    public Control AsControl() => this;

    public FrameworkElement GetKeyTipServiceTarget() => this;

    public bool CanSetFocusFromKeyTipService(FrameworkElement keyTipsTarget) => true;

    public void SetFocusFromKeyTipService() => this.Focus();
}
