﻿namespace Fluent.Automation.Peers
{
    /// <inheritdoc />
    public class MenuItemAutomationPeer : System.Windows.Automation.Peers.MenuItemAutomationPeer
    {
        /// <summary>Initializes a new instance of the <see cref="T:MenuItemAutomationPeer" /> class.</summary>
        /// <param name="owner">The element associated with this automation peer.</param>
        public MenuItemAutomationPeer(MenuItem owner)
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
    }
}
