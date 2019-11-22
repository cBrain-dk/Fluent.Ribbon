namespace Fluent.Automation.Peers
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using JetBrains.Annotations;

    /// <summary>
    /// Based on https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-support-for-the-tabitem-control-type
    /// </summary>
    public class BackstageTabItemAutomationPeer : HeaderedControlAutomationPeer, ISelectionItemProvider
    {
        private BackstageTabItem BackstageTabItem => (BackstageTabItem)this.Owner;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public BackstageTabItemAutomationPeer([NotNull] BackstageTabItem owner)
            : base(owner)
        {
        }

        #region Necessary implementations (from TabItemAutomationPeer)

        // The need can be seen from https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/TabItemAutomationPeer.cs,6e5d1f459704abbe

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            BackstageTabControl backstageTabControl = System.Windows.Controls.ItemsControl.ItemsControlFromItemContainer(this.BackstageTabItem) as BackstageTabControl;
            //If a separator tab item is asked for children, we show the children related to the currently selected tabItem
            if (backstageTabControl?.SelectedContent is FrameworkElement element)
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
        protected override string GetClassNameCore()
        {
            return "BackstageTabItem";
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Tab;
        }

        /// <inheritdoc/>
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.SelectionItem)
            {
                return this;
            }

            return base.GetPattern(patternInterface);
        }

        #region Implement ISelectionItemProvider

        bool ISelectionItemProvider.IsSelected => this.BackstageTabItem.IsSelected;

        IRawElementProviderSimple ISelectionItemProvider.SelectionContainer => null;

        void ISelectionItemProvider.Select()
        {
            this.BackstageTabItem.IsSelected = true;
        }

        void ISelectionItemProvider.AddToSelection()
        {
            this.BackstageTabItem.IsSelected = true;
        }

        void ISelectionItemProvider.RemoveFromSelection()
        {
            this.BackstageTabItem.IsSelected = false;
        }

        #endregion

        #endregion
    }
}