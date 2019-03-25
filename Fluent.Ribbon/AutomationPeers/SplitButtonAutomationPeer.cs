namespace Fluent.AutomationPeers
{
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;

    /// <inheritdoc />
    public class SplitButtonAutomationPeer : DropDownButtonAutomationPeer, IInvokeProvider, IExpandCollapseProvider
    {
        private SplitButton SplitButton => (SplitButton)this.Owner;

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="owner">Owning control</param>
        public SplitButtonAutomationPeer(SplitButton owner) 
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
            => AutomationControlType.SplitButton;

        /// <inheritdoc />
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Invoke)
            {
                return this;
            }

            return base.GetPattern(patternInterface);
        }

        #region IInvokeProvider
        void IInvokeProvider.Invoke()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            this.SplitButton.AutomationButtonClick();
        }
        #endregion
    }
}
