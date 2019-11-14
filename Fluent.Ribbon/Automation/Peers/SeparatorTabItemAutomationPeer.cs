namespace Fluent.Automation.Peers
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation.Peers;

    /// <summary>
    /// Automation Peer for a separator in the backstage
    /// </summary>
    public class SeparatorTabItemAutomationPeer : HeaderedControlAutomationPeer
    {
        private SeparatorTabItem SeparatorTabItem => (SeparatorTabItem)this.Owner;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public SeparatorTabItemAutomationPeer(SeparatorTabItem separatorTabItem)
            : base(separatorTabItem)
        {
        }

        #region Necessary implementations (from TabItemAutomationPeer)

        // The need can be seen from https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/TabItemAutomationPeer.cs,6e5d1f459704abbe

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            BackstageTabControl backstageTabControl = System.Windows.Controls.ItemsControl.ItemsControlFromItemContainer(this.SeparatorTabItem) as BackstageTabControl;
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
            return "SeparatorTabItem";
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Separator;
        }

        #endregion
    }
}
