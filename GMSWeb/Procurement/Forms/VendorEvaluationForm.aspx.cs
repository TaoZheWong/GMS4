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
using System.Collections.Generic;

namespace GMSWeb.Procurement.Forms
{
    public partial class VendorEvaluationForm : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (!Page.IsPostBack)
            {

                if (Request.Params["FORMID"] != null)
                {
                    hidFormID1.Value = Request.Params["FORMID"].ToString();
                    //hidVendorID1.Value = Request.Params["VENDORID"].ToString();
                    //hidFormID2.Value = Request.Params["FORMID"].ToString();
                }
                LoadData();
            }
        }

        #region LoadData
        private void LoadData()
        {
            #region Load By FormID

            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID1.Value));
            this.lblVendorName.Text = vendorapplicationform.VendorObject.CompanyName;
            this.lblEmail.Text = vendorapplicationform.VendorObject.Email;

            GMSCore.Entity.VendorApplicationForm vendor1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID1.Value));
            if (vendor1 != null)
            {
           
                //txtTypeOfOwnership.Text = vendor1.TypeOfOwnership;
                //hidTypeOfOwnership.Value = vendor1.TypeOfOwnership;
                //txtNatureOfBusiness.Text = vendor1.NatureOfBusiness;

              

                //if (txtTypeOfOwnership.Text == "")
                //{
                //    btnDuplicate.Visible = false;
                //    btnSendEmail.Visible = true;
                //}
                //else {
                //    btnDuplicate.Visible = true;
                //    btnSendEmail.Visible = true;

                //}
              
              

                string appPath = HttpRuntime.AppDomainAppVirtualPath;

                lnkVendorEvaluation.Text = lnkVendorEvaluation.NavigateUrl = "localhost" + appPath + "/Procurement/Forms/VendorEvaluationForm1.aspx?FORMID=" + hidFormID1.Value.Trim();
            }
          
            #endregion
        }
        #endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

           
                #region Add new record
                
                GMSCore.Entity.VendorApplicationForm vendorApplicationForm = new GMSCore.Entity.VendorApplicationForm();

           
              //      if (txtTypeOfOwnership.Text.Trim() == vendorApplicationForm.TypeOfOwnership)
              //      {
              //      Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The record has been submitted before.");
                   
              //      return;
              //      }
              //      else
              //      {

              //  if (txtTypeOfOwnership.Text.Trim() != "")
              //  {
              //      Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The form has been submitted.");
              //      return;
              //  }
              //  else
              //  {
              //      Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "Please fill in all the fields.");
                   
              //  }
              //}
            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
            str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
            str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm.aspx\";} else " + " {window.close();} ");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }


        #endregion

        #region btnUpdate_Click
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }

            GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID1.Value.Trim()));

           
                //vendorApplicationForm.TypeOfOwnership = txtTypeOfOwnership.Text.Trim();
                //vendorApplicationForm.NatureOfBusiness = txtNatureOfBusiness.Text.Trim();
                vendorApplicationForm.VEFStatus = "1";
                vendorApplicationForm.ApprovedStatus = "0";

                vendorApplicationForm.Save();
                vendorApplicationForm.Resync();
                hidFormID1.Value = vendorApplicationForm.FormID.ToString();
                LoadData();

            }

            //StringBuilder str = new StringBuilder();
            //str.Append("<script language='javascript'>");
            //str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
            //str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
            //str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm.aspx\";} else " + " {window.close();} ");
            //str.Append("</script>");
            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
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

        //#region btnSendEmail_Click
        //protected void btnSendEmail_Click(object sender, EventArgs e)
        //{

        //    LogSession session = base.GetSessionInfo();
        //    var companyID = session.CompanyId;
        //    var vendorID = hidVendorID1.Value;
        //    var companyName = lblVendorName.Text;
        //    var email = lblEmail.Text;
        //    var linktopass = lnkVendorEvaluation.Text.Trim();

        //    btnDuplicate.Visible = false;
        //    btnSendEmail.Visible = false;


        //    SystemDataActivity sDataActivity = new SystemDataActivity();
        //    GMSCore.Entity.VendorApplicationForm vendorAppForm = sDataActivity.RetrieveVendorApplicationFormByVendorID(GMSUtil.ToInt(hidVendorID1.Value.Trim()));

        //    btnSendEmail.Visible = true;

        //    //linktopass += vendorAppForm.FormID.ToString();

        //    var m = new ResponseModel();

        //    try
        //    {
        //        new GMSGeneralDALC().SendVendorEmail(companyID, companyName, email, linktopass);
        //    }
        //    catch (Exception x)
        //    {
        //        m.Status = 1;
        //        m.Message = x.Message;
        //    }

        //    //return m;
        //}
        //#endregion

        //#region btnDuplicate_Click
        //protected void btnDuplicate_Click(object sender, EventArgs e)
        //{
        //    hidFormID1.Value = "";
        //    //txtDateFrom.Text = "";
        //    //txtDateFromTime.Text = "";
        //    //txtDateTo.Text = "";
        //    //txtDateToTime.Text = "";
        //    btnDuplicate.Visible = false;
        //    //btnAttendee.Visible = false;
        //}
        //#endregion

        //#region btnApproved_Click
        //protected void btnApproved_Click(object sender, EventArgs e)
        //{
        //    LogSession session = base.GetSessionInfo();
        //    if (session == null)
        //    {
        //        Response.Redirect("../../SessionTimeout.htm");
        //        return;
        //    }

        //    GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID2.Value.Trim()));

        //    //vendorApplicationForm.Status = "1";
        //    vendorApplicationForm.VEFStatus = "1";

        //    vendorApplicationForm.Save();
        //    vendorApplicationForm.Resync();
        //    hidFormID2.Value = vendorApplicationForm.FormID.ToString();
        //    LoadData();

        //    StringBuilder str = new StringBuilder();
        //    str.Append("<script language='javascript'>");
        //    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
        //    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
        //    str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm.aspx\";} else " + " {window.close();} ");
        //    str.Append("</script>");
        //    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        //}
        //    #endregion

        //    #region btnRejected_Click
        //protected void btnRejected_Click(object sender, EventArgs e)
        //{
        //    LogSession session = base.GetSessionInfo();
        //    if (session == null)
        //    {
        //        Response.Redirect("../../SessionTimeout.htm");
        //        return;
        //    }

        //    GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID2.Value.Trim()));

        //    //vendorApplicationForm.Status = "2";
        //    vendorApplicationForm.VEFStatus = "2";

        //    vendorApplicationForm.Save();
        //    vendorApplicationForm.Resync();
        //    hidFormID2.Value = vendorApplicationForm.FormID.ToString();
        //    LoadData();

        //    StringBuilder str = new StringBuilder();
        //    str.Append("<script language='javascript'>");
        //    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
        //    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
        //    str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm.aspx\";} else " + " {window.close();} ");
        //    str.Append("</script>");
        //    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);

        //    #endregion

    }


    #endregion
//}
   