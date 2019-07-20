using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace PhylogeneticTree
{
    public partial class CheckUpdate : Form
    {
        public CheckUpdate()
        {
            InitializeComponent();
        }

        private void waiting()
        {
            webBrowser1.Navigate(System.IO.Directory.GetCurrentDirectory() + "\\hlp\\Waiting.html");
        }

        private void check()
        {
            Thread.Sleep(500);
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                webBrowser1.Navigate("http://www.blueweb.ir/phylogeny/Update.aspx?version=2.1");
            else
                webBrowser1.Navigate(System.IO.Directory.GetCurrentDirectory() + "\\hlp\\Error.html");
        }
        
        private void CheckUpdate_Load(object sender, EventArgs e)
        {
            Thread tid1 = new Thread(new ThreadStart(waiting));
            tid1.Priority = ThreadPriority.Highest;
            tid1.Start();
            Thread tid2 = new Thread(new ThreadStart(check));
            tid2.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
