using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent.AutomationPeers
{
    public class CheckBoxAutomationPeer : System.Windows.Automation.Peers.CheckBoxAutomationPeer
    {
        public CheckBoxAutomationPeer(CheckBox owner):base(owner)
        {
        }

        protected override bool IsEnabledCore()
        {
            return AutomationPeerHelper.IsEnabledCore(this);
        }

        protected override string GetAcceleratorKeyCore()
        {
            return AutomationPeerHelper.GetAcceleratorKey(this);
        }

        protected override string GetAccessKeyCore()
        {
            return AutomationPeerHelper.GetAccessKeyAndAcceleratorKey(this);
        }

        protected override string GetHelpTextCore()
        {
            return AutomationPeerHelper.GetHelpText(this);
        }

        protected override string GetNameCore()
        {
            return AutomationPeerHelper.GetNameAndHelpText(this);
        }
    }
}
