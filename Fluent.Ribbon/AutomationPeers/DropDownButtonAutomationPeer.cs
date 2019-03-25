namespace Fluent.AutomationPeers
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using System.Windows.Controls;

    /// <summary>
    /// Accessibility helper for DropDownButtons (based on the behavior of MenuItemAutomationPeer)
    /// https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/MenuItemAutomationPeer.cs
    /// </summary>
    public class DropDownButtonAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, IInvokeProvider
    {
        private DropDownButton DropDownButton => (DropDownButton)this.Owner;

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="owner">Owning control</param>
        public DropDownButtonAutomationPeer(DropDownButton owner)
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

        /// <summary>
        /// Determine the type of the control for screenreaders
        /// </summary>
        /// <returns></returns>
        protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.Button;

        /// <inheritdoc />
        protected override string GetClassNameCore() => this.Owner.GetType().Name;

        /// <inheritdoc />
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.ExpandCollapse
                || patternInterface == PatternInterface.Invoke)

            {
                return this;
            }

            return base.GetPattern(patternInterface);
        }

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            List<AutomationPeer> children = base.GetChildrenCore();
            if (!this.IsEnabledCore())
            {
                return children;
            }

            if (((IExpandCollapseProvider)this).ExpandCollapseState == ExpandCollapseState.Expanded)
            {
                ItemCollection items = this.DropDownButton.Items;
                if (items.Count > 0)
                {
                    children = new List<AutomationPeer>();
                    for (int i = 0; i < items.Count; i++)
                    {
                        UIElement element = this.DropDownButton.ItemContainerGenerator.ContainerFromIndex(i) as UIElement;
                        if (element != null)
                        {
                            AutomationPeer peer = UIElementAutomationPeer.FromElement(element);
                            if (peer == null)
                            {
                                peer = UIElementAutomationPeer.CreatePeerForElement(element);
                            }

                            if (peer != null)
                            {
                                children.Add(peer);
                            }
                        }
                    }
                }
            }

            return children;
        }

        #region IExpandCollapse

        ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState => this.DropDownButton.IsDropDownOpen 
            ? ExpandCollapseState.Expanded 
            : ExpandCollapseState.Collapsed;

        void IExpandCollapseProvider.Expand()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            this.DropDownButton.IsDropDownOpen = true;
        }

        void IExpandCollapseProvider.Collapse()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            this.DropDownButton.IsDropDownOpen = false;
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

        #region IInvoke

        void IInvokeProvider.Invoke()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            this.DropDownButton.IsDropDownOpen = !this.DropDownButton.IsDropDownOpen;
        }

        #endregion
    }
}
