namespace Fluent.Automation.Peers
{
    using System;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;

    /// <summary>
    /// Based on https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-support-for-the-tab-control-type
    /// </summary>
    public class BackstageTabControlAutomationPeer : System.Windows.Automation.Peers.SelectorAutomationPeer, ISelectionProvider
    {
        private BackstageTabControl BackstageTabControl => (BackstageTabControl)this.Owner;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public BackstageTabControlAutomationPeer(BackstageTabControl owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            switch (item)
            {
                case BackstageTabItem backstageTabItem:
                    return new BackstageTabItemAutomationPeer(backstageTabItem, this);

                case Fluent.Button button:
                    return new ButtonAsItemAutomationPeer(button, this);

                case Fluent.SeparatorTabItem separator:
                    return new SeparatorTabItemAutomationPeer(separator, this);

                default:
                    throw new NotImplementedException($"The control of type: {item.GetType()} was not expected in the backstage, we need an automationpeer for it");
            }
        }

        #region UIAutomation Support

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Tab;
        }

        /// <inheritdoc />
        protected override bool IsContentElementCore()
        {
            return true;
        }

        /// <inheritdoc />
        protected override bool IsControlElementCore()
        {
            return true;
        }

        /// <inheritdoc />
        protected override AutomationOrientation GetOrientationCore()
        {
            return AutomationOrientation.Vertical;
        }

        #endregion

        #region Standard automation fields

        /// <inheritdoc />
        protected override string GetNameCore()
        {
            return AutomationPeerHelper.GetName(this);
        }

        /// <inheritdoc />
        protected override bool IsEnabledCore()
        {
            return AutomationPeerHelper.IsEnabledCore(this);
        }

        /// <inheritdoc />
        protected override string GetAcceleratorKeyCore()
        {
            return AutomationPeerHelper.GetAcceleratorKey(this);
        }

        /// <inheritdoc />
        protected override string GetAccessKeyCore()
        {
            return AutomationPeerHelper.GetAccessKeyAndAcceleratorKey(this);
        }

        /// <inheritdoc />
        protected override string GetHelpTextCore()
        {
            return AutomationPeerHelper.GetHelpText(this);
        }

        #endregion

        #region ISelectionProvider

        bool ISelectionProvider.IsSelectionRequired => true;

        bool ISelectionProvider.CanSelectMultiple => false;

        #endregion
    }
}
