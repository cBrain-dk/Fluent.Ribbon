namespace Fluent.Automation.Peers
{
    using JetBrains.Annotations;

    /// <inheritdoc />
    public class ButtonAutomationPeer : System.Windows.Automation.Peers.ButtonAutomationPeer
    {
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
    }
}