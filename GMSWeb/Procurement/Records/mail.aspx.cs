using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace GMSWeb.Procurement.Records
{
    public partial class mail : System.Web.UI.Page
    {
        protected void btn_SendMessage_Click(object sender, EventArgs e)
        {
            SmtpClient smtpClient = new SmtpClient("localhost/GMS3/Procurement/Records/mail.aspx", 25);

            smtpClient.Credentials = new System.Net.NetworkCredential("yvonne.lim@leedennox.com", "leeden123");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mailMessage = new MailMessage(txtFrom.Text, txtTo.Text);
            mailMessage.Subject = txtSubject.Text;
            mailMessage.Body = txtBody.Text;

            try
            {
                smtpClient.Send(mailMessage);
                Label1.Text = "Message sent";
            }
            catch (Exception ex)
            {
                Label1.Text = ex.ToString();
            }
        }
    }
}