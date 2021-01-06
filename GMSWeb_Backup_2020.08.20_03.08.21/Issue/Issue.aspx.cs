using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Transactions;
using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Text;
using System.Web.Services;
using AjaxControlToolkit;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace GMSWeb.Issue
{
    public partial class Issue : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Issue");
            LogSession session = base.GetSessionInfo();
            hidCoyID.Value = session.CompanyId.ToString();
            hidUserID.Value = session.UserId.ToString();
            string system = (Request.Params["System"].ToString().Trim() == "G") ? "GMS" : "LMS";
            string type = (Request.Params["Type"].ToString().Trim() == "B") ? "Bugs" : "Changes";
            lblTitle.Text = system + " > " + type ;
            
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetIssue(string system, string type, string IssueID)
        {
            DataSet dsTemp = new DataSet();
            dsTemp.Clear();
            new GMSGeneralDALC().GetIssue(system, type, IssueID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static void InsertIssue(short CoyID, string System, string Type, string ReportedBy, string Description, short CreatedBy)
        {
            new GMSGeneralDALC().InsertIssue(CoyID, System, Type, ReportedBy, Description, CreatedBy);
        }

        [WebMethod]
        public static void UpdateIssue(string IssueID, string System, string Type, string ReportedBy, string Description, string Remarks, string Status, short ModifiedBy)
        {
            new GMSGeneralDALC().UpdateIssue(IssueID, System, Type, ReportedBy, Description, Remarks, Status, ModifiedBy);
        }
    }
}