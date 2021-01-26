using GMSCore;
using GMSCore.Activity;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMSWeb.HR.Recruitment
{
    public partial class EmailResume : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["FileName"] != null && Request.Params["FileName"].ToString() != "" && 
                Request.Params["ID"] != null && Request.Params["ID"].ToString() != ""&&
                 Request.Params["HRinCharge"] != null && Request.Params["HRinCharge"].ToString() != "")
            {
                string fileName = Decrypt(Request.Params["FileName"].ToString());
                string coyID = Decrypt(Request.Params["ID"].ToString());
            }
        }

        protected void btn_SendMessage_Click(object sender, EventArgs e)
        {
            string linktopass = "https://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS4/HR/Recruitment/ViewPage.aspx?ID=" + Request.Params["ID"].ToString() + "&FileName=" + Request.Params["FileName"].ToString();
            string emailTo = txtTo.Text.ToString();
            string ccTo = txtCC.Text.ToString();
            string hr = Decrypt(Request.Params["HRinCharge"].ToString());
            string subject = txtSubject.Text.ToString();
            try
            {
                new GMSGeneralDALC().SendResumeEmail(emailTo, ccTo, linktopass,hr,subject);
                lblMsg.Text = "Message sent";
                txtTo.Text = "";
                txtCC.Text = "";
                txtSubject.Text = "";
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        private static string Decrypt(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}