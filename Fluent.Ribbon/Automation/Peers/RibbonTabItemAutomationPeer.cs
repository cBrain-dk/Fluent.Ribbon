namespace Fluent.Automation.Peers
{
    using System.Windows.Automation.Peers;
    using JetBrains.Annotations;

    /// <summary>
    /// Base automation peer for <see cref="RibbonTabItemAutomationPeer"/>.
    /// </summary>
    public class RibbonTabItemAutomationPeer : HeaderedControlAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonTabItemAutomationPeer([NotNull] RibbonTabItem owner)
            : base(owner)
        {
        }

        #region UIAutomation Support

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return "RibbonTabItem";
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Tab;
        }

        #endregion
    }
}
