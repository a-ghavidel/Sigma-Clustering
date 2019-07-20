using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PhylogeneticTree
{
    public partial class Help : Form
    {
        private Boolean doWait = false;
        
        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
            treeView1.Nodes[0].Checked = true;
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\hlp\\pcInfo.txt"))
            {
                    doWait = true;
            }
            else
                {
                    StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\hlp\\pcInfo.txt");
                    if (SystemInformation.ComputerName != sr.ReadLine())
                        doWait = true;
                    sr.Close();
                }
        }

        private void waitHtml()
        {
            webBrowser1.Navigate(new Uri(Directory.GetCurrentDirectory() + "\\hlp\\wait.html"));
        }

        private void load7()
        {
            string cpuCaption = WMI_ProcessorInformation.WMI_Processor_Information.GetCpuCaption();
            cpuCaption += " (" + WMI_ProcessorInformation.WMI_Processor_Information.GetCpuDataWidth() + "-bit)";
            string numberOfCores = WMI_ProcessorInformation.WMI_Processor_Information.GetCpuCores().ToString();
            int PCount = Environment.ProcessorCount;
            OperatingSystem os = Environment.OSVersion;
            Boolean is64os = Environment.Is64BitOperatingSystem;
            Microsoft.VisualBasic.Devices.ComputerInfo compInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
            ulong ram = compInfo.TotalPhysicalMemory;
            ram = ram / (1024 * 1024);//convert to MB
            string osFullName = compInfo.OSFullName;
            try
            {
                StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\hlp\\7.html");
                string html7 = sr.ReadToEnd();
                sr.Close();
                html7 = html7.Replace("xxpnxx", cpuCaption);
                html7 = html7.Replace("xxncxx", numberOfCores);
                html7 = html7.Replace("xxosxx", osFullName + " " + os.ServicePack);
                html7 = html7.Replace("xxpcxx", PCount.ToString());
                if (is64os)
                    html7 = html7.Replace("xxosaxx", "64");
                else
                    html7 = html7.Replace("xxosaxx", "32");
                html7 = html7.Replace("xxramxx", ram.ToString() + " MB");

                StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\hlp\\7_1.html");
                sw.Write(html7);
                sw.Close();
                webBrowser1.Navigate(new Uri(Directory.GetCurrentDirectory() + "\\hlp\\7_1.html"));
                StreamWriter sw2 = new StreamWriter(Directory.GetCurrentDirectory() + "\\hlp\\pcInfo.txt");
                sw2.WriteLine(SystemInformation.ComputerName);
                sw2.Close();
                doWait = false;
            }
            catch
            {
            }

        }//end of load 7.html
        
        private void loadText(string name)
        {
            if (name == "7")
            {
                if (doWait)
                {
                    Thread tid1 = new Thread(new ThreadStart(waitHtml));
                    tid1.Priority = ThreadPriority.Highest;
                    tid1.Start();
                    Thread tid2 = new Thread(new ThreadStart(load7));
                    tid2.Start();
                }
                else
                    if (File.Exists(Directory.GetCurrentDirectory() + "\\hlp\\" + "7_1.html"))
                        webBrowser1.Navigate(new Uri(Directory.GetCurrentDirectory() + "\\hlp\\" + "7_1.html"));
            }
            else
                webBrowser1.Navigate(new Uri(Directory.GetCurrentDirectory() + "\\hlp\\" + name + ".html"));
        }
        
        
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
           loadText(treeView1.SelectedNode.Name);
            
        }
    }
}
