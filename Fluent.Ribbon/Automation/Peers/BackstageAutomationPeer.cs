namespace Fluent.Automation.Peers
{
    using System.Runtime.CompilerServices;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using JetBrains.Annotations;

    /// <summary>
    /// Based on Automation Peer for Menu.cs (https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/MenuAutomationPeer.cs,bb6ceeab103ddb49)
    /// </summary>
    public class BackstageAutomationPeer : HeaderedControlAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public BackstageAutomationPeer([NotNull] Backstage owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return "Backstage";
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Tab;
        }
    }
}
