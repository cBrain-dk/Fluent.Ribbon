namespace Fluent.Automation.Peers
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using JetBrains.Annotations;

    /// <summary>
    /// Base automation peer for <see cref="RibbonTabItem"/>.
    /// </summary>
    public class RibbonTabItemAutomationPeer : HeaderedControlAutomationPeer
    {
        private RibbonTabItem RibbonTabItem => (RibbonTabItem)this.Owner;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonTabItemAutomationPeer([NotNull] RibbonTabItem owner)
            : base(owner)
        {
        }

        #region Necessary implementations (from TabItemAutomationPeer)

        // The need can be seen from https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/TabItemAutomationPeer.cs,6e5d1f459704abbe

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            RibbonTabControl ribbonTabControl = System.Windows.Controls.ItemsControl.ItemsControlFromItemContainer(this.RibbonTabItem) as RibbonTabControl;
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
            return "RibbonTabItem";
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Tab;
        }

        #endregion
    }
}
