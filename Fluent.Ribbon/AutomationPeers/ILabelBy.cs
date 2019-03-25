namespace Fluent.AutomationPeers
{
    using System.Windows.Automation.Peers;

    /// <summary>
    /// Interface for controls where the connected label is part of the screen reader tip
    /// </summary>
    public interface ILabelBy
    {
        /// <summary>
        /// Get the automation peer for a label
        /// </summary>
        /// <returns></returns>
        AutomationPeer GetLabelByAutomationPeer();
    }
}
