namespace Fluent.Automation.Peers
{
    using System;
    using System.Windows.Automation.Peers;
    using JetBrains.Annotations;

    /// <summary>
    /// Automation peer for <see cref="Fluent.ScreenTip" />.
    /// </summary>
    public class ScreenTipAutomationPeer : ToolTipAutomationPeer
    {
        private ScreenTip ScreenTip => (ScreenTip)this.Owner;

        /// <summary>
        ///     Creates a new instance.
        /// </summary>
        public ScreenTipAutomationPeer([NotNull] ScreenTip owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override string GetNameCore()
        {
            string name = base.GetNameCore();

            if (string.IsNullOrEmpty(name))
            {
                name = this.ScreenTip.Title;
            }

            return name;
        }

        /// <inheritdoc />
        protected override string GetHelpTextCore()
        {
            string helpText = base.GetHelpTextCore();

            if (string.IsNullOrEmpty(helpText))
            {
                string screenTipHelpText = this.ScreenTip.Text;

                if (!string.IsNullOrEmpty(this.ScreenTip.DisableReason))
                {
                    string disableReason = RibbonLocalization.Current.Localization.ScreenTipDisableReasonHeader
                        + Environment.NewLine + this.ScreenTip.DisableReason;

                    screenTipHelpText += Environment.NewLine + disableReason;
                }

                if (this.ScreenTip.HelpTopic != null)
                {
                    screenTipHelpText += Environment.NewLine + RibbonLocalization.Current.Localization.ScreenTipF1LabelHeader;
                }

                return screenTipHelpText;
            }

            return helpText;
        }

        /// <inheritdoc />
        protected override bool IsContentElementCore()
        {
            return true;
        }
    }
}