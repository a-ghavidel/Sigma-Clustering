using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace PhylogeneticTree
{
    public partial class Settings : Form 
    {
        private int loadAttemps = 0;
        private Microsoft.VisualBasic.Devices.ComputerInfo compInfo;

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            
            try
            {
                compInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
                this.cmbPriority.Text = Settings1.Default.Priority.ToString();
                this.cmbImageType.Text = Settings1.Default.ImageType.ToString();
                this.chbGapIgnore.Checked = Settings1.Default.Ignore_Gaps;
                this.label9.Enabled = txtNumberOfSequences.Enabled = cbNotification.Checked = Settings1.Default.NotificationEnabled1;
                this.cbEqualLengths.Checked = Settings1.Default.NotificationEnabled2;
                this.txtThreshold.Text = Settings1.Default.Threshold_Size;
                this.chbShowAdditionalinfo.Checked = Settings1.Default.ShowAdditionalInfo;
                if (Settings1.Default.IDLength == "")
                    Settings1.Default.IDLength = "20";
                this.txtIDLength.Text = Settings1.Default.IDLength;
                this.cmbGapExtendPenalty.Text = Settings1.Default.Extend_Gap.ToString() ;
                this.cmbGapOpenPenalty.Text = Settings1.Default.Open_Gap.ToString();
            }
            catch
            {
                if (++loadAttemps <= 3)
                    Settings_Load(sender, e);
                else
                {
                    MessageBox.Show("Failed to load settings! Please try again later.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Dispose();
                }
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            double d;
            int i;
            try
            {
                d = Convert.ToDouble(txtThreshold.Text);
                if (d < 0 || d > 1)
                {
                    MessageBox.Show("Please input a number betwwen 0 and 1 for the variance threshold", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Cluster threshold size should be a positive integer", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                i = Convert.ToInt32(txtPP.Text);
                if (i < 0)
                {
                    MessageBox.Show("Please input a non-negative number for presence percentage", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Please input a non-negative number for presence percentage", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                i = Convert.ToInt32(txtNumberOfSequences.Text);
                if (i <= 0)
                {
                    MessageBox.Show("Please input a non-zero positive number for number of sequences", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            catch
            {
                txtNumberOfSequences.Text = Settings1.Default.NumberOfSequences;
                MessageBox.Show("Number of sequences must be a positive integer", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                i = Convert.ToInt32(txtIDLength.Text);
                if (i <= 0)
                {
                    MessageBox.Show("Please input a non-zero positive number for ID length", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            catch
            {
                txtIDLength.Text = Settings1.Default.IDLength;
                MessageBox.Show("The length of the sequence ID must be a positive integer", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                Settings1.Default.Algorithm = cmbAlgorithm.SelectedItem.ToString();
                Settings1.Default.Open_Gap = cmbGapOpenPenalty.Text;
                Settings1.Default.Extend_Gap = cmbGapExtendPenalty.Text;

                Settings1.Default.CoVariance = rbtnCoVariance.Checked;
                Settings1.Default.Euclidean = rbtnEuclidean.Checked;
                Settings1.Default.Pearson = rbtnPearsonCorrelation.Checked;

                Settings1.Default.Presence_Percentage = txtPP.Text;
                Settings1.Default.Threshold_Size = txtThreshold.Text;
                Settings1.Default.Ignore_Gaps = chbGapIgnore.Checked;

                Settings1.Default.NotificationEnabled1 = cbNotification.Checked;
                Settings1.Default.NotificationEnabled2 = cbEqualLengths.Checked;
                Settings1.Default.NumberOfSequences = txtNumberOfSequences.Text;
                Settings1.Default.Priority = cmbPriority.Text;

                Settings1.Default.IDLength = txtIDLength.Text;
                Settings1.Default.ShowAdditionalInfo = chbShowAdditionalinfo.Checked;
                Settings1.Default.ImageType = cmbImageType.SelectedItem.ToString();

                Settings1.Default.Save();
                this.Dispose();
            }
            catch
            {
                MessageBox.Show("Failed to save your settings! This may happen due to the disc authentication policy", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//end of btnSave

        private void btnDefault_Click(object sender, EventArgs e)
        {
            cmbAlgorithm.SelectedIndex = 1;
            cmbGapOpenPenalty.Text = "-4";
            cmbGapExtendPenalty.Text = "-1";
            rbtnEuclidean.Checked = true;
            txtPP.Text = "25";
            txtThreshold.Text = "0.002";
            chbGapIgnore.Checked = true;
            cbEqualLengths.Checked = cbNotification.Checked = true;
            txtNumberOfSequences.Text = "105";
            cmbPriority.Text = "AboveNormal";
            txtIDLength.Text = "20";
            cmbImageType.SelectedItem = cmbImageType.Items[0];
            chbShowAdditionalinfo.Checked = true;
        }

        private void cmbPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (compInfo.OSFullName.ToLower().Contains("home"))
                MessageBox.Show("Please note that there's only a slight difference between process priorities in Windows 'Home' edition. To reduce run time, I strongly recommend you to use 'Professional' or 'Ultimate' editions.",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                if (cmbPriority.Text == "RealTime" && Settings1.Default.Priority != "RealTime")
                    MessageBox.Show("RealTime priority obtain CPU time with minimal latency but it may prevent all other processes from executing!",
                        "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cbNotification_CheckedChanged(object sender, EventArgs e)
        {
            label9.Enabled = txtNumberOfSequences.Enabled = cbNotification.Checked;
        }

        private void txtThreshold_TextChanged(object sender, EventArgs e)
        {

        }

       

    }
}
