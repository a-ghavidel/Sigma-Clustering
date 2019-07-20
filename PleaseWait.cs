using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms;

namespace PhylogeneticTree
{
    public class PleaseWait: IDisposable
    {
        private Form mSplash;
        private Point mLocation;
        private string message = "Please wait...";

        public PleaseWait(Point location)
        {
            mLocation = location;
            Thread t = new Thread(new ThreadStart(workerThread));
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
        public PleaseWait(Point location, string waitingMessage)
        {
            mLocation = location;
            message = waitingMessage;
            Thread t = new Thread(new ThreadStart(workerThread));
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
        public void Dispose()
        {
            mSplash.Invoke(new MethodInvoker(stopThread));
        }
        private void stopThread()
        {
            mSplash.Close();
        }
        private void workerThread()
        {

            mSplash = new Form2();   // Substitute this with your own
            mSplash.Controls[1].Text = message;
            mSplash.StartPosition = FormStartPosition.Manual;
            mSplash.Location = mLocation;
            mSplash.TopMost = true;
            mSplash.ShowDialog();//Application.Run(mSplash);
        }
    }

}
