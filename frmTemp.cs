using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PhylogeneticTree
{
    public partial class frmTemp : Form
    {
        public frmTemp()
        {
            InitializeComponent();
        }

        private void frmTemp_Load(object sender, EventArgs e)
        {
            chart1.Series[0].Points.AddXY(1, 3);
            chart1.Series[0].Points.AddXY(4, 5);
            chart1.Series[0].Points.AddXY(6, 8);
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            //clickPosition = pos;
            var results = chart1.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.PlottingArea)
                {
                    try
                    {
                        double xVal = result.ChartArea.AxisX.PixelPositionToValue(pos.X);
                        double yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);

                        MessageBox.Show(xVal.ToString()+yVal.ToString());
                    }
                    catch (Exception)
                    {
                    }

                    //tooltip.Show("X=" + xVal + ", Y=" + yVal, this.chart1, e.Location.X, e.Location.Y - 15);
                }
            }
        }
    }
}
