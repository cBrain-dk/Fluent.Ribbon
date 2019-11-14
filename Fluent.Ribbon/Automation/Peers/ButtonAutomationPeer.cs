namespace Fluent.Automation.Peers
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using Fluent.Internal;
    using JetBrains.Annotations;

    /// <inheritdoc />
    public class ButtonAutomationPeer : System.Windows.Automation.Peers.ButtonAutomationPeer
    {
        private Button Button => (Button)this.Owner;

        /// <summary>Initializes a new instance of the <see cref="T:ButtonAutomationPeer" /> class.</summary>
        /// <param name="owner">The element associated with this automation peer.</param>
        public ButtonAutomationPeer([NotNull] Button owner)
            : base(owner)
        {
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

        /// <inheritdoc />
        protected override string GetNameCore()
        {
            return AutomationPeerHelper.GetNameAndHelpText(this);
        }

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            if (RibbonProperties.GetIsBackstageItem(this.Button))
            {
                BackstageTabControl backstageTabControl = UIHelper.GetParent<BackstageTabControl>(this.Button);
                //If a separator tab item is asked for children, we show the children related to the currently selected tabItem
                if (backstageTabControl?.SelectedContent is FrameworkElement element)
                {
                    List<AutomationPeer> childPeers = new FrameworkElementAutomationPeer(element).GetChildren();
                    if (childPeers != null)
                    {
                        return childPeers;
                    }
                }
            }

            return base.GetChildrenCore();
        }
    }
}