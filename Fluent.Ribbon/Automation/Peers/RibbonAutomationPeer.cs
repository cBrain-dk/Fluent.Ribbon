namespace Fluent.Automation.Peers
{
    using System.Windows;
    using System.Windows.Automation.Peers;

    /// <summary>
    /// Accessibility helper for ribbons
    /// </summary>
    public class RibbonAutomationPeer : HeaderedControlAutomationPeer
    {
        /// <summary>Initializes a new instance of the <see cref="T:RibbonAutomationPeer" /> class.</summary>
        /// <param name="owner">The element associated with this automation peer.</param>
        public RibbonAutomationPeer(Ribbon owner) 
            : base(owner)
        {
        }
    }
}
