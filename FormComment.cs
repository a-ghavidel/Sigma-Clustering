using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;

namespace PhylogeneticTree
{
    public partial class FormComment : Form
    {
        public FormComment()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = false;
            mail.BodyEncoding = Encoding.UTF8;
            mail.From = new MailAddress("sigmaclustering@yahoo.com");
            mail.To.Add("sigmaclustering@yahoo.com"); 
            mail.Subject = "New Comment on S-Clustering";
            mail.Body = "نام: " + txtName.Text + "\n" + txtEmail.Text + "\n" + "موضوع: " + txtSubject.Text + "\n" + "--------------\n" + txtComment.Text;
            // Smtp configuration
            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("sigmaclustering@yahoo.com", "@123456789"); 
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Port = 587; //use 587 for gmail & yahoo          
            client.Host = "smtp.mail.yahoo.com";
            client.EnableSsl = true;

            try
            {
                Point pt = this.Location;
                pt.X = this.Location.X + this.Size.Width / 2 - 94;
                pt.Y = this.Location.Y + this.Size.Height / 2 - 48;
                using (new PleaseWait(pt, "Sending..."))
                    client.Send(mail);
                MessageBox.Show("Thank you. Your message was sent successfully.", "Successfully Was Sent",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Dispose();
            }
            catch(Exception exc)
            {
                MessageBox.Show("There is a problem by your network connection!","ERROR!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        

        private void FormComment_Load(object sender, EventArgs e)
        {
            Point pt = this.Location;
            pt.X = this.Location.X + this.Size.Width / 2 - 94;
            pt.Y = this.Location.Y + this.Size.Height / 2 - 48;
            bool InternetConnection;
            using (new PleaseWait(pt, "Checking Internet.."))
                InternetConnection = WebRequestTest();
            if (!InternetConnection)
            {
                MessageBox.Show("It seems there is a problem with your Internet connection!", "Internet Connection Problem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtComment.Enabled = txtEmail.Enabled = txtName.Enabled = txtSubject.Enabled = btnSend.Enabled = false;
                label1.Text = "لطفا ابتدا به اینترنت متصل شوید و دوباره امتحان کنید";
                label1.ForeColor = Color.Red;
            }
        }

        public static bool WebRequestTest()
        {
            string url = "http://www.google.com";
            try
            {
                WebRequest myRequest = WebRequest.Create(url);
                WebResponse myResponse = myRequest.GetResponse();
            }
            catch (System.Net.WebException)
            {
                return false;
            }
            return true;
        }

    }
}
