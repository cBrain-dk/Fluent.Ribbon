namespace Fluent
{
    using System.Windows.Input;

    /// <summary>
    /// Interface for a ViewModel based command
    /// Button henceforth will also refer to other type of controls (checkboxes, menuitems, etc.)
    /// </summary>
    public interface IToolTipCommand
    {
        /// <summary>
        /// Icon to display on the button
        /// </summary>
        object Icon { get; }

        /// <summary>
        /// White icon to display on the button (used in quickaccess items or in high contrast)
        /// </summary>
        object WhiteIcon { get; }

        /// <summary>
        /// Large icon used on buttons (when button is large)
        /// </summary>
        object LargeIcon { get; }

        /// <summary>
        /// Title of screentip for the button
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Whether the corresponding should be visible
        /// </summary>
        bool? IsVisible { get; }

        /// <summary>
        /// Used on the label of the button
        /// </summary>
        string Header { get; }

        /// <summary>
        /// Used on the label of the button if it is large
        /// </summary>
        string LargeHeader { get; }

        /// <summary>
        /// Reason for the button not being active
        /// </summary>
        string DisabledText { get; }

        /// <summary>
        /// The main text of the screentip (comes after <see cref="Title"/>)
        /// </summary>
        string HelpText { get; }

        /// <summary>
        /// String to be interpreted by the computer of what accelerator key sequence activate the command
        /// </summary>
        string KeyboardShortCut { get; }

        /// <summary>
        /// Human readable version of <see cref="KeyboardShortCut"/>
        /// </summary>
        string KeyboardShortCutHumanText { get; }

        /// <summary>
        /// The access key sequence for the command
        /// </summary>
        string KeyTip { get; }

        /// <summary>
        /// The primary accelerator key (f.ex. Ctrl Alt [Del])
        /// </summary>
        Key? Key { get; }

        /// <summary>
        /// The primary modifier on the accelerator key (f.ex. [Ctrl] Alt Del)
        /// </summary>
        ModifierKeys? Modifier { get; }

        /// <summary>
        /// The secondary modifer on the accelerator key (f.ex. Ctrl [Alt] Del)
        /// </summary>
        ModifierKeys? SecondModifier { get; }
    }
}
