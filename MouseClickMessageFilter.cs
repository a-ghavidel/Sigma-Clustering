using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace PhylogeneticTree
{
    class MouseClickMessageFilter : IMessageFilter
    {
        private MouseClickMessageFilter Filter;

        private const int LButtonDown = 0x201;
        private const int LButtonUp = 0x202;
        private const int LButtonDoubleClick = 0x203;
        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case LButtonDown:
                case LButtonUp:
                case LButtonDoubleClick:
                    return true;
            }
            return false;
        }

        public void DisableMouseClicks()
        {
            if (this.Filter == null)
            {
                this.Filter = new MouseClickMessageFilter();
                Application.AddMessageFilter(this.Filter);
            }
        }

        public void EnableMouseClicks()
        {
            if ((this.Filter != null))
            {
                Application.RemoveMessageFilter(this.Filter);
                this.Filter = null;
            }
        }
    }//end of class
}
