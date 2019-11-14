namespace Fluent.Automation.Peers
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using Fluent.Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Based on Automation Peer for Menu.cs (https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/MenuAutomationPeer.cs,bb6ceeab103ddb49)
    /// </summary>
    public class BackstageAutomationPeer : HeaderedControlAutomationPeer
    {
        private Backstage Backstage => (Backstage)this.Owner;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public BackstageAutomationPeer([NotNull] Backstage owner)
            : base(owner)
        {
        }

        #region Necessary implementations (from TabItemAutomationPeer)

        // The need can be seen from https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/TabItemAutomationPeer.cs,6e5d1f459704abbe

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            RibbonTabControl ribbonTabControl = UIHelper.GetParent<RibbonTabControl>(this.Backstage);
            //If a separator tab item is asked for children, we show the children related to the currently selected tabItem
            if (ribbonTabControl?.SelectedContent is FrameworkElement element)
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
            return "Backstage";
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Tab;
        }

        #endregion
    }
}
