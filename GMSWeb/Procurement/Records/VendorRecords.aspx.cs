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

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Text;
using GMSWeb.CustomCtrl;
using System.Collections.Generic;
using System.Web.Services;

using CrystalDecisions;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace GMSWeb.Procurement.Records
{
    public partial class VendorRecords : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (!Page.IsPostBack)
            {

                if (Request.Params["FORMID"] != null)
                {
                    hidFormID3.Value = Request.Params["FORMID"].ToString();
                    hidFormID4.Value = Request.Params["FORMID"].ToString();
                }
                LoadData();
            }
        }

        string appPath = HttpRuntime.AppDomainAppVirtualPath;

        //string javaScript = "";
        //javaScript = "<script language=\"javascript\" type=\"text/javascript\" src=\"" + appPath + "/scripts/popcalendar.js\"></script>";


        #region LoadData
        private void LoadData()
        {
            #region Load By FormID

            GMSCore.Entity.Vendor vendor = GMSCore.Entity.Vendor.RetrieveByKey(GMSUtil.ToInt(hidVendorID3.Value.Trim()));
            GMSCore.Entity.VendorApplicationForm vendorApp = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID3.Value));
            lblVendorName1.Text = vendorApp.VendorObject.CompanyName;
            lblEmail1.Text = vendorApp.VendorObject.Email;
            lblStatus1.Text = vendorApp.ApprovedStatus;

            lnkVendorEvaluation1.Text = lnkVendorEvaluation1.NavigateUrl = "localhost" + appPath + "/Procurement/Forms/VendorEvaluationForm.aspx?VENDORID=" + hidVendorID3.Value.Trim() + "&FORMID=";

            switch (vendorApp.ApprovedStatus)
            {
                case "1":
                    lblStatus1.Text = "Approved";
                    break;
                case "2":
                    lblStatus1.Text = "Rejected";
                    break;
                default:
                    lblStatus1.Text = "Pending";
                    break;
            }

            GMSCore.Entity.VendorApplicationForm vendor1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            if (vendor1 != null)
            {
                //txtTypeOfOwnership1.Text = vendor1.TypeOfOwnership;
                //hidTypeOfOwnership1.Value = vendor1.TypeOfOwnership;
                //txtNatureOfBusiness1.Text = vendor1.NatureOfBusiness;

            }
            #endregion
        }
        #endregion


        #region btnApproved_Click
        protected void btnApproved_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }

            GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value.Trim()));

            //vendorApplicationForm.Status = "1";
            vendorApplicationForm.ApprovedStatus = "1";

            vendorApplicationForm.Save();
            vendorApplicationForm.Resync();
            hidFormID4.Value = vendorApplicationForm.FormID.ToString();
            LoadData();

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
            str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
            str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm.aspx\";} else " + " {window.close();} ");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }
        #endregion

        #region btnRejected_Click
        protected void btnRejected_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }

            GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value.Trim()));

            //vendorApplicationForm.Status = "2";
            vendorApplicationForm.ApprovedStatus = "2";

            vendorApplicationForm.Save();
            vendorApplicationForm.Resync();
            hidFormID4.Value = vendorApplicationForm.FormID.ToString();
            LoadData();

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
            str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
            str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm.aspx\";} else " + " {window.close();} ");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }
        #endregion

        public class ResponseModel
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public IDictionary<string, object> Params { get; set; }

            public ResponseModel()
            {
                Status = 0; // 0=success; 1=error
                Message = "Success";
            }

            public ResponseModel(int status, string message)
            {
                Status = status;
                Message = message;
            }
        }


    }
}