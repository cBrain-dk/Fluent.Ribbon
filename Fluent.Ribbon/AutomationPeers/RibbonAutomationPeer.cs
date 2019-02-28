using Fluent.AutomationPeers;
using Fluent.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;

namespace Fluent
{
    /// <summary>
    /// Accessibility helper for ribbons
    /// </summary>
    public class RibbonAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fe"></param>
        public RibbonAutomationPeer(FrameworkElement fe) : base(fe)
        { }

        /// <summary>
        /// Provides the name for screen readers
        /// </summary>
        /// <returns></returns>
        protected override string GetNameCore()
        {
            return AutomationPeerHelper.GetName(this);
        }

        /// <summary>
        /// Provides the accesskey for screen readers
        /// </summary>
        /// <returns></returns>
        protected override string GetAccessKeyCore()
        {
            return AutomationPeerHelper.GetAccessKeyAndAcceleratorKey(this);
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }

        protected override string GetAcceleratorKeyCore()
        {
            return AutomationPeerHelper.GetAcceleratorKey(this);
        }
    }
}
