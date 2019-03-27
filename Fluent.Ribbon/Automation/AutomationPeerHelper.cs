namespace Fluent.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Input;
    using Fluent.Internal;

    /// <summary>
    /// Helper class for using automation peers
    /// </summary>
    public static class AutomationPeerHelper
    {
        #region Methods for JAWS

        /// <summary>
        /// Get name and help text key combined into a readable string
        /// </summary>
        /// <param name="automationPeer">The peer to extract from</param>
        public static string GetNameAndHelpText(FrameworkElementAutomationPeer automationPeer)
        {
            string name = GetName(automationPeer);
            string helpText = GetHelpText(automationPeer);

            if (string.IsNullOrEmpty(name))
            {
                return helpText;
            }

            if (!string.IsNullOrEmpty(helpText) && !string.Equals(name, helpText))
            {
                return $"{name} - {helpText}";
            }

            return name;
        }

        /// <summary>
        /// Get access key and accelerator key combined into a readable string
        /// </summary>
        /// <param name="automationPeer">The peer to extract from</param>
        public static string GetAccessKeyAndAcceleratorKey(FrameworkElementAutomationPeer automationPeer)
        {
            string accessKey = GetAccessKey(automationPeer);
            string acceleratorKey = GetAcceleratorKey(automationPeer);

            if (string.IsNullOrEmpty(accessKey))
            {
                return acceleratorKey;
            }

            if (!string.IsNullOrEmpty(acceleratorKey) && !string.Equals(accessKey, acceleratorKey))
            {
                return $"{accessKey} - {acceleratorKey}";
            }

            return accessKey;
        }
        #endregion

        #region Automation methods

        /// <summary>
        /// Get the enabled state from an automation peer
        /// </summary>
        /// <param name="automationPeer">The peer to extract from</param>
        public static bool IsEnabledCore(FrameworkElementAutomationPeer automationPeer)
        {
            if (automationPeer.Owner is IReadOnlyControl readOnlyControl)
            {
                return !readOnlyControl.IsReadOnly && automationPeer.Owner.IsEnabled;
            }

            return automationPeer.Owner.IsEnabled;
        }

        /// <summary>
        /// Get the accelerator from an automation peer
        /// </summary>
        /// <param name="automationPeer">The peer to extract from</param>
        public static string GetAcceleratorKey(FrameworkElementAutomationPeer automationPeer)
        {
            string acceleratorKey = AutomationProperties.GetAcceleratorKey(automationPeer.Owner);
            if (!string.IsNullOrEmpty(acceleratorKey))
            {
                return acceleratorKey;
            }

            if (automationPeer.Owner is ICommandSource commandSource
                && commandSource.Command is IToolTipCommand toolTipCommand)
            {
                return toolTipCommand.KeyboardShortCutHumanText;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the access key from an automation peer
        /// </summary>
        /// <param name="automationPeer">The peer to extract from</param>
        public static string GetAccessKey(FrameworkElementAutomationPeer automationPeer)
        {
            string accessKey = AutomationProperties.GetAccessKey(automationPeer.Owner);
            if (!string.IsNullOrEmpty(accessKey))
            {
                return accessKey;
            }

            if (automationPeer.Owner is IKeyTipedControl keyTipedControl
                && !string.IsNullOrEmpty(keyTipedControl.KeyTip))
            {
                var parents = UIHelper.GetParents<IKeyTipedControl>(automationPeer.Owner);
                return GetKeyTip(parents, keyTipedControl);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the name from an automation peer
        /// </summary>
        /// <param name="automationPeer">The peer to extract from</param>
        public static string GetName(FrameworkElementAutomationPeer automationPeer)
        {
            string name = AutomationProperties.GetName(automationPeer.Owner);
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }

            if (automationPeer.Owner is IHeaderedControl headeredControl
                    && headeredControl.Header is string header
                    && !string.IsNullOrEmpty(header))
            {
                return header;
            }

            if (automationPeer.Owner is FrameworkElement fe
                && fe.ToolTip is ScreenTip screenTip
                && !string.IsNullOrEmpty(screenTip.Title))
            {
                return screenTip.Title;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the help text from an automation peer
        /// </summary>
        /// <param name="automationPeer">The peer to extract from</param>
        public static string GetHelpText(FrameworkElementAutomationPeer automationPeer)
        {
            string helpText = AutomationProperties.GetHelpText(automationPeer.Owner);
            if (!string.IsNullOrEmpty(helpText))
            {
                return helpText;
            }

            FrameworkElement fe = automationPeer.Owner as FrameworkElement;
            if (fe == null)
            {
                return string.Empty;
            }

            if (fe.ToolTip is string toolTipString)
            {
                return toolTipString;
            }

            if (fe.ToolTip is ScreenTip screenTip)
            {
                string screenTipHelpText = screenTip.Text;

                if (!IsEnabledCore(automationPeer) &&
                    !string.IsNullOrEmpty(screenTip.DisableReason))
                {
                    string disableReason = RibbonLocalization.Current.Localization.ScreenTipDisableReasonHeader
                        + Environment.NewLine + screenTip.DisableReason;

                    screenTipHelpText += Environment.NewLine + disableReason;
                }

                return screenTipHelpText;
            }

            return string.Empty;
        }

        #endregion

        #region Helper methods
        private static string GetKeyTip(List<IKeyTipedControl> keyTipedControls, IKeyTipedControl ribbonControl)
        {
            if (string.IsNullOrEmpty(ribbonControl.KeyTip))
            {
                return null;
            }

            string sKeyTip = "Alt, ";
            foreach (var keyTip in keyTipedControls)
            {
#pragma warning disable SA1119 // Statement must not use unnecessary parenthesis
                if (!(keyTip is RibbonGroupBox ribbonGroupBox) || ribbonGroupBox.State == RibbonGroupBoxState.Collapsed)
#pragma warning restore SA1119 // Statement must not use unnecessary parenthesis
                {
                    sKeyTip += keyTip.KeyTip + "," + " ";
                }
            }

            sKeyTip += ribbonControl.KeyTip;
            return sKeyTip;
        }
        #endregion
    }
}
