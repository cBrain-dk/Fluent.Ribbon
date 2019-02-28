using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace Fluent.AutomationPeers
{
    // Should probably implement IToggleProvider as well, since SplitButton contains a ToggleButton
    public class SplitButtonAutomationPeer : DropDownButtonAutomationPeer, IInvokeProvider, IExpandCollapseProvider
    {
        private SplitButton SplitButton => (SplitButton)Owner;
        public SplitButtonAutomationPeer(SplitButton owner) : base(owner)
        { }

        protected override AutomationControlType GetAutomationControlTypeCore()
            => AutomationControlType.SplitButton;

        override public object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Invoke)
                return this;

            return base.GetPattern(patternInterface);
        }

        #region IInvokeProvider
        void IInvokeProvider.Invoke()
        {
            if (!IsEnabled())
                throw new ElementNotEnabledException();

            SplitButton.InvokeButtonClick();
        }
        #endregion
    }
}
