namespace Fluent
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interface which denotes the necessary methods and properties required to be used in conjuction with the <see cref="KeyTipService" />
    /// </summary>
    public interface IKeyTipServiceHost
    {
        /// <summary>
        /// Wrapper for IsEnabled on <see cref="Control"/>
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Wrapper for IsKeyboardFocusWithin on <see cref="Control"/>
        /// </summary>
        bool IsKeyboardFocusWithin { get; }

        /// <summary>
        /// Whether the host is currently in a collapsed state
        /// </summary>
        bool IsCollapsed { get; }

        /// <summary>
        /// Whether the host reacts to KeyTips
        /// </summary>
        bool IsKeyTipHandlingEnabled { get; }

        /// <summary>
        /// Wrapper on the interface to return the host as a <see cref="Control"/>
        /// </summary>
        /// <returns></returns>
        Control AsControl();

        /// <summary>
        /// If the host spawns a menu or a popup like it, this new element is the one supposed to get the KeyTip service
        /// Therefore return the element which is currently acting as the active control
        /// </summary>
        /// <returns></returns>
        FrameworkElement GetKeyTipServiceTarget();

        /// <summary>
        /// Determines whether the control is in a state where it should place focus to allow for keytips to be shown
        /// </summary>
        /// <param name="keyTipsTarget">The current element the service is going to be placing keytips upon</param>
        /// <returns></returns>
        bool CanSetFocusFromKeyTipService(FrameworkElement keyTipsTarget);

        /// <summary>
        /// Set focus in the topmost graphical element where the keytipschain will start
        /// </summary>
        void SetFocusFromKeyTipService();
    }
}
