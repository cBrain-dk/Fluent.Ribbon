using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace Fluent.AutomationPeers
{
    /// <summary>
    /// Accessibility helper for DropDownButtons (based on the behavior of MenuItemAutomationPeer)
    /// https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/MenuItemAutomationPeer.cs
    /// </summary>
    public class DropDownButtonAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, IInvokeProvider
    {
        private DropDownButton DropDownButton => (DropDownButton)Owner;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="owner"></param>
        public DropDownButtonAutomationPeer(DropDownButton owner) : base(owner)
        { }

        protected override bool IsEnabledCore()
        {
            return AutomationPeerHelper.IsEnabledCore(this);
        }

        protected override string GetAcceleratorKeyCore()
        {
            return AutomationPeerHelper.GetAcceleratorKey(this);
        }

        protected override string GetAccessKeyCore()
        {
            return AutomationPeerHelper.GetAccessKeyAndAcceleratorKey(this);
        }

        protected override string GetHelpTextCore()
        {
            return AutomationPeerHelper.GetHelpText(this);
        }

        protected override string GetNameCore()
        {
            return AutomationPeerHelper.GetNameAndHelpText(this);
        }

        /// <summary>
        /// Determine the type of the control for screenreaders
        /// </summary>
        /// <returns></returns>
        protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.Button;

        /// <summary>
        /// Get the class name for the owning Control
        /// </summary>
        /// <returns></returns>
        protected override string GetClassNameCore() => Owner.GetType().Name;

        /// <summary>
        /// Returns the object associated with the patternInterface if it implements the relevant interface
        /// </summary>
        /// <param name="patternInterface"></param>
        /// <returns></returns>
        public override object GetPattern(PatternInterface patternInterface)
        {

            if (patternInterface == PatternInterface.ExpandCollapse
                || patternInterface == PatternInterface.Invoke)

            {
                return this;
            }

            return base.GetPattern(patternInterface);
        }

        /// <summary>
        /// Get the children of the dropdownbutton, special case when the popup is open
        /// </summary>
        /// <returns></returns>
        protected override List<AutomationPeer> GetChildrenCore()
        {
            List<AutomationPeer> children = base.GetChildrenCore();
            if (!IsEnabledCore())
                return children;

            if (((IExpandCollapseProvider)this).ExpandCollapseState == ExpandCollapseState.Expanded)
            {
                ItemCollection items = DropDownButton.Items;
                if (items.Count > 0)
                {
                    children = new List<AutomationPeer>();
                    for (int i = 0; i < items.Count; i++)
                    {
                        UIElement element = DropDownButton.ItemContainerGenerator.ContainerFromIndex(i) as UIElement;
                        if (element != null)
                        {
                            AutomationPeer peer = UIElementAutomationPeer.FromElement(element);
                            if (peer == null)
                                peer = UIElementAutomationPeer.CreatePeerForElement(element);
                            if (peer != null)
                                children.Add(peer);
                        }
                    }
                }
            }

            return children;
        }

        #region IExpandCollapse

        ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState => DropDownButton.IsDropDownOpen ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed;

        void IExpandCollapseProvider.Expand()
        {
            if (!IsEnabled())
                throw new ElementNotEnabledException();

            DropDownButton.IsDropDownOpen = true;
        }

        void IExpandCollapseProvider.Collapse()
        {
            if (!IsEnabled())
                throw new ElementNotEnabledException();

            DropDownButton.IsDropDownOpen = false;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
        {
            RaisePropertyChangedEvent(
                ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty,
                oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed,
                newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
        }

        #endregion

        #region IInvoke

        void IInvokeProvider.Invoke()
        {
            if (!IsEnabled())
                throw new ElementNotEnabledException();

            DropDownButton.IsDropDownOpen = !DropDownButton.IsDropDownOpen;
        }

        #endregion
    }
}
