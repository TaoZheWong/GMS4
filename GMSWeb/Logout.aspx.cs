using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;


namespace GMSWeb
{
    public partial class Logout : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //if (!string.IsNullOrEmpty(Request.QueryString["LOGOUT"]) &&
                //    Request.QueryString["LOGOUT"].Trim() == "1")
                //{
                LogSession session = base.GetSessionInfo();
                try
                {
                    AuthKey.DeleteByKey(session.UserId);
                }catch(Exception ex)
                {

                }
                Session.RemoveAll();
                FormsAuthentication.SignOut();
                Response.Redirect("Default.aspx");
                
                    //lblMessage.Text = "You have logged out.";
                //}
            }
        }
    }
}
