using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace Fluent
{
  public interface IToolTipCommand
  {
    object Icon { get; }
    object WhiteIcon { get; }
    object LargeIcon { get; }
    string Title { get; }
    bool? IsVisible { get; }
    string Header { get; }
    string LargeHeader { get; }
    string DisabledText { get; }
    string HelpText { get; }
    string KeyboardShortCut { get; }
    string KeyboardShortCutHumanText { get; }

    string KeyTip { get; }
    Key? Key { get; }
    ModifierKeys? Modifier { get; }
    ModifierKeys? SecondModifier { get; }
  }
}
