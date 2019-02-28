using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace Fluent.AutomationPeers
{
  public interface ILabelBy
  {
    AutomationPeer GetLabelByAutomationPeer();
  }
}
