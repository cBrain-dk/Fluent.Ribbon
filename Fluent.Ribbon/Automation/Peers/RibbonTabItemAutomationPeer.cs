namespace Fluent.Automation.Peers
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using JetBrains.Annotations;

    /// <summary>
    /// Base automation peer for <see cref="RibbonTabItemAutomationPeer"/>.
    /// </summary>
    public class RibbonTabItemAutomationPeer : FluentTabItemAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonTabItemAutomationPeer([NotNull] RibbonTabItem owner, RibbonTabControlAutomationPeer ribbonTabControlAutomationPeer)
            : base(owner, ribbonTabControlAutomationPeer)
        {
        }

        #region UIAutomation Support

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return "RibbonTabItem";
        }

        #endregion
    }
}
