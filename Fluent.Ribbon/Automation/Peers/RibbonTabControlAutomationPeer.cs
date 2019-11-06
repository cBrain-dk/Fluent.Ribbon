namespace Fluent.Automation.Peers
{
    using System;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Based on https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-support-for-the-tab-control-type
    /// </summary>
    public class RibbonTabControlAutomationPeer : FluentTabControlAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonTabControlAutomationPeer(Selector owner) 
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            switch (item)
            {
                case RibbonTabItem ribbonTabItem:
                    return new RibbonTabItemAutomationPeer(ribbonTabItem, this);

                default:
                    throw new NotImplementedException($"The control of type: {item.GetType()} was not expected in the backstage, we need an automationpeer for it");
            }
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return "RibbonTabControlAutomationPeer";
        }
    }
}
