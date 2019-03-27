namespace Fluent.Automation.Peers
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using System.Windows.Controls;
    using JetBrains.Annotations;

    /// <summary>
    /// Automation peer for <see cref="DropDownButton"/>.
    /// </summary>
    public class DropDownButtonAutomationPeer : HeaderedControlAutomationPeer, IExpandCollapseProvider, IToggleProvider, IInvokeProvider
    {
        private DropDownButton DropDownButton => (DropDownButton)this.Owner;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public DropDownButtonAutomationPeer([NotNull] DropDownButton owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.Button;

        /// <inheritdoc />
        protected override string GetClassNameCore() => this.Owner.GetType().Name;

        /// <inheritdoc />
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.ExpandCollapse
                || patternInterface == PatternInterface.Invoke
                || patternInterface == PatternInterface.Toggle)

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

        #region IToggle

        /// <inheritdoc />
        public void Toggle()
        {
            ((DropDownButton)this.Owner).IsDropDownOpen = !((DropDownButton)this.Owner).IsDropDownOpen;
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