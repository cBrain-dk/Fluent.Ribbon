namespace Fluent.Automation.Peers
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using JetBrains.Annotations;

    /// <summary>
    /// Based on https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-support-for-the-tabitem-control-type
    /// </summary>
    public abstract class FluentTabItemAutomationPeer : System.Windows.Automation.Peers.SelectorItemAutomationPeer, ISelectionItemProvider
    {
        private ITabItem TabItem => (ITabItem)this.Item;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public FluentTabItemAutomationPeer([NotNull] ITabItem owner, [NotNull] FluentTabControlAutomationPeer container)
            : base(owner, container)
        {
        }

        #region Necessary implementations (from TabItemAutomationPeer)

        // The need can be seen from https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/TabItemAutomationPeer.cs,6e5d1f459704abbe

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            if (this.TabItem.IsSelected
                && this.TabItem.TabControlParent.SelectedContent is FrameworkElement element)
            {
                List<AutomationPeer> childPeers = new FrameworkElementAutomationPeer(element).GetChildren();
                if (childPeers != null)
                {
                    return childPeers;
                }
            }

            return new List<AutomationPeer>(0);
        }

        #endregion

        #region UIAutomation Support

        /// <inheritdoc />
        protected override abstract string GetClassNameCore();

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
            return AutomationPeerHelper.IsEnabledCore(this);
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

        #region ISelectionItemProvider

        bool ISelectionItemProvider.IsSelected => this.TabItem.IsSelected;

        void ISelectionItemProvider.Select()
        {
            if (this.IsEnabled() == false)
            {
                throw new ElementNotEnabledException();
            }

            this.TabItem.IsSelected = true;
        }

        void ISelectionItemProvider.AddToSelection()
        {
            if (this.IsEnabled() == false)
            {
                throw new ElementNotEnabledException();
            }

            //We do not support multiple selections in the backstage, so overwrite the current selected item
            this.TabItem.IsSelected = true;
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