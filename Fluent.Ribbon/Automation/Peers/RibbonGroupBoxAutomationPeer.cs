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
    /// Automation peer for <see cref="RibbonGroupBox"/>.
    /// </summary>
    public class RibbonGroupBoxAutomationPeer : HeaderedControlAutomationPeer, IExpandCollapseProvider
    {
        private RibbonGroupBox RibbonGroupBox => (RibbonGroupBox)this.Owner;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonGroupBoxAutomationPeer([NotNull] RibbonGroupBox owner)
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
            if (patternInterface == PatternInterface.ExpandCollapse)
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
                ItemCollection items = this.RibbonGroupBox.Items;
                if (items.Count > 0)
                {
                    children = new List<AutomationPeer>();
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (this.RibbonGroupBox.ItemContainerGenerator.ContainerFromIndex(i) is UIElement element)
                        {
                            AutomationPeer peer = FromElement(element);
                            if (peer == null)
                            {
                                peer = CreatePeerForElement(element);
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

        ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState => this.RibbonGroupBox.IsDropDownOpen
            ? ExpandCollapseState.Expanded
            : ExpandCollapseState.Collapsed;

        void IExpandCollapseProvider.Expand()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            this.RibbonGroupBox.IsDropDownOpen = true;
        }

        void IExpandCollapseProvider.Collapse()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            this.RibbonGroupBox.IsDropDownOpen = false;
        }

        [System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.NoInlining)]
        internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
        {
            this.RaisePropertyChangedEvent(
                ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty,
                oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed,
                newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
        }

        #endregion
    }
}
