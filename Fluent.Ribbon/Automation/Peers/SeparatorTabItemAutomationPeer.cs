namespace Fluent.Automation.Peers
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation.Peers;

    /// <summary>
    /// Based on https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-support-for-the-separator-control-type
    /// </summary>
    public class SeparatorTabItemAutomationPeer : System.Windows.Automation.Peers.ItemAutomationPeer
    {
        private SeparatorTabItem SeparatorTabItem => (SeparatorTabItem)this.Item;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public SeparatorTabItemAutomationPeer(SeparatorTabItem separatorTabItem, BackstageTabControlAutomationPeer backstageTabControlAutomationPeer)
            : base(separatorTabItem, backstageTabControlAutomationPeer)
        {
        }

        #region Necessary implementations (from TabItemAutomationPeer)

        // The need can be seen from https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/TabItemAutomationPeer.cs,6e5d1f459704abbe

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            BackstageTabControl backstageTabControl = System.Windows.Controls.ItemsControl.ItemsControlFromItemContainer(this.SeparatorTabItem) as BackstageTabControl;
            //If a separator tab item is asked for children, we show the children related to the currently selected tabItem
            if (backstageTabControl.SelectedContent is FrameworkElement element)
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

        /// <inheritdoc />
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.SelectionItem)
            {
                return this;
            }

            return base.GetPattern(patternInterface);
        }

        /// <inheritdoc />
        protected override AutomationPeer GetLabeledByCore()
        {
            return null;
        }

        /// <inheritdoc />
        protected override bool IsContentElementCore()
        {
            return false;
        }

        /// <inheritdoc />
        protected override bool IsControlElementCore()
        {
            return true;
        }

        /// <inheritdoc />
        protected override bool IsKeyboardFocusableCore()
        {
            return false;
        }

        #endregion

        #region Standard automation fields

        /// <inheritdoc />
        protected override string GetNameCore()
        {
            return string.Empty;
        }

        /// <inheritdoc />
        protected override bool IsEnabledCore()
        {
            return AutomationPeerHelper.IsEnabledCore(this);
        }

        /// <inheritdoc />
        protected override string GetAcceleratorKeyCore()
        {
            return string.Empty;
        }

        /// <inheritdoc />
        protected override string GetAccessKeyCore()
        {
            return string.Empty;
        }

        /// <inheritdoc />
        protected override string GetHelpTextCore()
        {
            return string.Empty;
        }

        #endregion
    }
}
