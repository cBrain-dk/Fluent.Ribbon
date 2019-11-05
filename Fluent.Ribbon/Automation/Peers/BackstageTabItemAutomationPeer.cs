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
    public class BackstageTabItemAutomationPeer : FluentTabItemAutomationPeer, ISelectionItemProvider
    {
        private BackstageTabItem BackstageTabItem => (BackstageTabItem)this.Item;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public BackstageTabItemAutomationPeer([NotNull] BackstageTabItem owner, [NotNull] BackstageTabControlAutomationPeer container)
            : base(owner, container)
        {
        }

        #region Necessary implementations (from TabItemAutomationPeer)

        // The need can be seen from https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/TabItemAutomationPeer.cs,6e5d1f459704abbe
        
        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            if (this.BackstageTabItem.IsSelected 
                && this.BackstageTabItem.TabControlParent.SelectedContent is FrameworkElement element)
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
            return nameof(this.BackstageTabItem);
        }

        #endregion
    }
}