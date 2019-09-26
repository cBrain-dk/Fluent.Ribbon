namespace Fluent.Automation.Peers
{
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using JetBrains.Annotations;

    /// <summary>
    /// Based on https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-support-for-the-tabitem-control-type
    /// </summary>
    public class BackstageTabItemAutomationPeer : System.Windows.Automation.Peers.SelectorItemAutomationPeer, ISelectionItemProvider
    {
        private BackstageTabItem BackstageTabItem => (BackstageTabItem)this.Item;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public BackstageTabItemAutomationPeer([NotNull] BackstageTabItem owner, [NotNull] BackstageTabControlAutomationPeer container)
            : base(owner, container)
        {
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return "BackstageTabItem";
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.TabItem;
        }

        /// <inheritdoc />
        protected override AutomationPeer GetLabeledByCore()
        {
            return null;
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
        protected override bool IsKeyboardFocusableCore()
        {
            return this.IsEnabled();
        }

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

        #region ISelectionItemProvider

        bool ISelectionItemProvider.IsSelected => this.BackstageTabItem.IsSelected;

        void ISelectionItemProvider.Select()
        {
            if (this.IsEnabled() == false)
            {
                throw new ElementNotEnabledException();
            }

            this.BackstageTabItem.IsSelected = true;
        }

        void ISelectionItemProvider.AddToSelection()
        {
            if (this.IsEnabled() == false)
            {
                throw new ElementNotEnabledException();
            }

            //We do not support multiple selections in the backstage, so overwrite the current selected item
            this.BackstageTabItem.IsSelected = true;
        }

        void ISelectionItemProvider.RemoveFromSelection()
        {
            if (this.IsEnabled() == false)
            {
                throw new ElementNotEnabledException();
            }

            // Do nothing, we always require something to be selected
        }

        #endregion
    }
}