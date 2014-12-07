using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Timers;

namespace Rina.StyleResources.Controls
{
    public class DelayTextBox : TextBox
    {
        private Timer delayTimer;
        private Int32 threshold = 1000;
        private Boolean keysPressed = false;
        private Boolean timerElapsed = false;
        private TextChangedEventArgs textChangedEventArgs;

        public delegate void DelayOverHandler();

        public Int32 Delay
        {
            set { this.threshold = value; }
        }

        public DelayTextBox()
        {
            delayTimer = new Timer(this.threshold);
            delayTimer.Elapsed += delayTimerElapsed;
        }

        public void DelayOver()
        {
            OnTextChanged(this.textChangedEventArgs);
        }

        public void delayTimerElapsed(object sender, EventArgs e)
        {
            this.delayTimer.Enabled = false;
            this.timerElapsed = true;
            this.Dispatcher.Invoke(new DelayOverHandler(DelayOver), null);
        }

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (this.delayTimer.Enabled)
            {
                this.delayTimer.Enabled = true;
            }
            else
            {
                this.delayTimer.Enabled = false;
                this.delayTimer.Enabled = true;
            }

            this.keysPressed = true;

            base.OnKeyDown(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (this.timerElapsed || !this.keysPressed)
            {
                this.timerElapsed = false;
                this.keysPressed = false;
                base.OnTextChanged(e);
            }
            else
            {
                this.textChangedEventArgs = e;
            }
        }
    }
}
