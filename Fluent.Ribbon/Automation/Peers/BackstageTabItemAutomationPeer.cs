namespace Fluent.Automation.Peers
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using JetBrains.Annotations;

    /// <summary>
    /// Based on https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-support-for-the-tabitem-control-type
    /// </summary>
    public class BackstageTabItemAutomationPeer : FluentTabItemAutomationPeer, ISelectionItemProvider
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public BackstageTabItemAutomationPeer([NotNull] BackstageTabItem owner, [NotNull] BackstageTabControlAutomationPeer container)
            : base(owner, container)
        {
        }

        #region UIAutomation Support

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return nameof(BackstageTabItem);
        }

        #endregion
    }
}