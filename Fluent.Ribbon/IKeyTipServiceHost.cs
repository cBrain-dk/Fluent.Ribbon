// ReSharper disable once CheckNamespace
namespace Fluent;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Provides the host operations required by <see cref="KeyTipService"/>.
/// </summary>
public interface IKeyTipServiceHost
{
    /// <summary>
    /// Gets whether the host is enabled.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Gets whether keyboard focus is within the host.
    /// </summary>
    bool IsKeyboardFocusWithin { get; }

    /// <summary>
    /// Gets whether the host is collapsed.
    /// </summary>
    bool IsCollapsed { get; }

    /// <summary>
    /// Gets whether the host handles key tips.
    /// </summary>
    bool IsKeyTipHandlingEnabled { get; }

    /// <summary>
    /// Returns the control that hosts the key-tip adorner chain.
    /// </summary>
    /// <returns>The host control.</returns>
    Control AsControl();

    /// <summary>
    /// Returns the current key-tip target, such as an open menu or the host itself.
    /// </summary>
    /// <returns>The active key-tip target.</returns>
    FrameworkElement GetKeyTipServiceTarget();

    /// <summary>
    /// Determines whether the service should focus the host for the current target.
    /// </summary>
    /// <param name="keyTipsTarget">The current key-tip target.</param>
    /// <returns>Whether the service should focus the host.</returns>
    bool CanSetFocusFromKeyTipService(FrameworkElement keyTipsTarget);

    /// <summary>
    /// Focuses the element where the key-tip chain starts.
    /// </summary>
    void SetFocusFromKeyTipService();
}
