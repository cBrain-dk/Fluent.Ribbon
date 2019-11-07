﻿namespace Fluent.Automation.Peers
{
    /// <summary>
    /// Wrapper class based on https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Automation/Peers/TabItemWrapperAutomationPeer.cs,73eb2a5b985e1cf0
    /// </summary>
    public class RibbonTabItemWrapperAutomationPeer : System.Windows.Automation.Peers.FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonTabItemWrapperAutomationPeer(RibbonTabItem owner)
            : base(owner)
        {
        }
    }
}