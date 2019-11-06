namespace Fluent.Automation.Peers
{
    using System;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;

    /// <summary>
    /// Based on https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-support-for-the-tab-control-type
    /// </summary>
    public class BackstageTabControlAutomationPeer : FluentTabControlAutomationPeer, ISelectionProvider
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public BackstageTabControlAutomationPeer(BackstageTabControl owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            switch (item)
            {
                case BackstageTabItem backstageTabItem:
                    return new BackstageTabItemAutomationPeer(backstageTabItem, this);

                case Fluent.Button button:
                    return new BackstageButtonAsItemAutomationPeer(button, this);

                case Fluent.SeparatorTabItem separator:
                    return new SeparatorTabItemAutomationPeer(separator, this);

                default:
                    throw new NotImplementedException($"The control of type: {item.GetType()} was not expected in the backstage, we need an automationpeer for it");
            }
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return "BackstageTabControlAutomationPeer";
        }
    }
}
