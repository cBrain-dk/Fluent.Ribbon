namespace Fluent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Automation.Peers;

    /// <inheritdoc />
    public class RibbonAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Standard implementation of an automation peer
        /// </summary>
        /// <param name="fe">The Ribbon control this automation peer is attached to</param>
        public RibbonAutomationPeer(FrameworkElement fe)
            : base(fe)
        {
        }

        /// <inheritdoc />
        protected override string GetAccessKeyCore()
        {
            IKeyTipedControl control = this.Owner as IKeyTipedControl;
            if (control != null && control.KeyTip != null)
            {
                return control.KeyTip;
            }

            return base.GetAccessKeyCore();
        }

        /// <inheritdoc/>
        protected override string GetNameCore()
        {
            IKeyTipedControl control = this.Owner as IKeyTipedControl;
            string name = base.GetNameCore();

            return name;
        }
    }
}
