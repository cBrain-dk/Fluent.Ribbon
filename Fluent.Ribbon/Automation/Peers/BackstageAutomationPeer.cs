namespace Fluent.Automation.Peers
{
    using System.Runtime.CompilerServices;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using JetBrains.Annotations;

    /// <summary>
    /// Based on Automation Peer for Menu.cs (https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/MenuAutomationPeer.cs,bb6ceeab103ddb49)
    /// </summary>
    public class BackstageAutomationPeer : System.Windows.Automation.Peers.FrameworkElementAutomationPeer, IExpandCollapseProvider, IToggleProvider, IInvokeProvider
    {
        private Backstage Backstage => this.Owner as Backstage;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public BackstageAutomationPeer([NotNull] Backstage owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return "Backstage";
        }

        /// <inheritdoc />
        protected override AutomationPeer GetLabeledByCore()
        {
            return null;
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Menu;
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
        protected override string GetNameCore()
        {
            return AutomationPeerHelper.GetName(this);
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

        #region IInvoke

        void IInvokeProvider.Invoke()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            this.Backstage.IsOpen = !this.Backstage.IsOpen;
        }

        #endregion

        #region IExpandCollapse

        ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState => this.Backstage.IsOpen
            ? ExpandCollapseState.Expanded
            : ExpandCollapseState.Collapsed;

        void IExpandCollapseProvider.Expand()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            this.Backstage.IsOpen = true;
        }

        void IExpandCollapseProvider.Collapse()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            this.Backstage.IsOpen = false;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
        {
            this.RaisePropertyChangedEvent(
                ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty,
                oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed,
                newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
        }

        #endregion

        #region IToggle

        /// <inheritdoc />
        public void Toggle()
        {
            this.Backstage.IsOpen = !this.Backstage.IsOpen;
        }

        /// <inheritdoc />
        public ToggleState ToggleState => ConvertToToggleState(((DropDownButton)this.Owner).IsDropDownOpen);

        private static ToggleState ConvertToToggleState(bool value)
        {
            return value
                ? ToggleState.On
                : ToggleState.Off;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal virtual void RaiseToggleStatePropertyChangedEvent(bool oldValue, bool newValue)
        {
            this.RaisePropertyChangedEvent(TogglePatternIdentifiers.ToggleStateProperty, ConvertToToggleState(oldValue), ConvertToToggleState(newValue));
        }

        #endregion
    }
}
