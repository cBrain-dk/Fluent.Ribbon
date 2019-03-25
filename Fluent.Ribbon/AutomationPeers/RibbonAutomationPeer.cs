namespace Fluent
{
    using System.Windows;
    using System.Windows.Automation.Peers;
    using Fluent.AutomationPeers;

    /// <summary>
    /// Accessibility helper for ribbons
    /// </summary>
    public class RibbonAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="fe">Owning control</param>
        public RibbonAutomationPeer(FrameworkElement fe) 
            : base(fe)
        {
        }

        /// <inheritdoc />
        protected override string GetNameCore()
        {
            return AutomationPeerHelper.GetName(this);
        }

        /// <inheritdoc />
        protected override string GetAccessKeyCore()
        {
            return AutomationPeerHelper.GetAccessKeyAndAcceleratorKey(this);
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }

        /// <inheritdoc />
        protected override string GetAcceleratorKeyCore()
        {
            return AutomationPeerHelper.GetAcceleratorKey(this);
        }
    }
}
