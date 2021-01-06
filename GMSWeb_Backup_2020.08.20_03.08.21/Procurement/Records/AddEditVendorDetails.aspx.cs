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


namespace GMSWeb.Procurement.Records
{
    public partial class AddEditVendorDetails : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (!Page.IsPostBack)
            {

                if (Request.Params["FORMID"] != null)
                {
                    hidFormID1.Value = Request.Params["FORMID"].ToString();
                    hidVendorID1.Value = Request.Params["FORMID"].ToString();
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
          
            //this.lblEmail1.Text = vendorapplicationform.VendorObject.Email;

            GMSCore.Entity.VendorApplicationForm vendor1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID1.Value));
            if (vendor1 != null)
            {
                txtVendorName.Text = vendor1.VendorObject.CompanyName;
                //txtTypeOfOwnership.Text = vendor1.TypeOfOwnership;
                //hidTypeOfOwnership.Value = vendor1.TypeOfOwnership;
                //txtNatureOfBusiness.Text = vendor1.NatureOfBusiness;

                btnDuplicate.Visible = true;
                //btnSendEmail.Visible = true;
            }
            else {
                btnDuplicate.Visible = false;
                //btnSendEmail.Visible = false;
            }
          
            #endregion
        }
        #endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (string.IsNullOrEmpty(hidFormID1.Value.Trim())) { 

                #region Add new record

                GMSCore.Entity.VendorApplicationForm vendorApplicationForm = new GMSCore.Entity.VendorApplicationForm();

            if (txtVendorName.Text.Trim() == "")
            {
                base.JScriptAlertMsg("Vendor Name cannot be empty.");
                return;
            }
            else
            {
                GMSCore.Entity.Vendor vendor = new SystemDataActivity().RetrieveVendorByVendorName(txtVendorName.Text.Trim());
                if (vendor == null)
                {
                    base.JScriptAlertMsg("Vendor cannot be found in database. Please check your vendor name or create the vendor first");
                    return;
                }
                else
                {
                    vendorApplicationForm.VendorID = vendor.VendorID;
                }
            }

            //GMSCore.Entity.VendorApplicationForm vendorApplicationForm = new GMSCore.Entity.VendorApplicationForm();


            //if (txtTypeOfOwnership.Text.Trim() == vendorApplicationForm.TypeOfOwnership)
            //{
            //    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The record has been submitted before.");

            //    return;
            //}
            //else
            //{

            //    if (txtTypeOfOwnership.Text.Trim() != "")
            //    {
            //        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The form has been submitted.");
            //        return;
            //    }
            //    else
            //    {
            //        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "Please fill in all the fields.");

            //    }
            //}

            vendorApplicationForm.CoyID = session.CompanyId;
            vendorApplicationForm.Save();
            vendorApplicationForm.Resync();
            hidFormID1.Value = vendorApplicationForm.FormID.ToString();
            LoadData();

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
            str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
            str.Append("if (r) {window.location.href = \"../../Procurement/Records/AddEditVendorDetails.aspx\";} else " + " {window.close();} ");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }


            #endregion
        }

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

        

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
            str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
            str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm.aspx\";} else " + " {window.close();} ");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }
        #endregion

        #region SetVendor
        protected void SetVendor(object sender, EventArgs e)
        {
            GMSCore.Entity.Vendor vendor = new SystemDataActivity().RetrieveVendorByVendorName(txtVendorName.Text.Trim());
            GMSCore.Entity.VendorApplicationForm vendor1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID1.Value));
            if (vendor != null)
            {
                txtVendorName.Text = vendor.CompanyName;
                //txtTypeOfOwnership.Text = vendor1.TypeOfOwnership;
                //hidTypeOfOwnership.Value = vendor1.TypeOfOwnership;
                //txtNatureOfBusiness.Text = vendor1.NatureOfBusiness;
            }
            else
            {
                txtVendorName.Text = "";
                //txtTypeOfOwnership.Text = "";
                //hidTypeOfOwnership.Value = "";
                //txtNatureOfBusiness.Text = "";
                base.JScriptAlertMsg("Vendor cannot be found in database. Please check your vendor name or create the vendor first");
                return;
            }
        }
        #endregion

        #region btnDuplicate_Click
        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            //hidCourseSessionID.Value = "";
            //txtDateFrom.Text = "";
            //txtDateFromTime.Text = "";
            //txtDateTo.Text = "";
            //txtDateToTime.Text = "";
            //btnDuplicate.Visible = false;
            //btnAttendee.Visible = false;
        }
        #endregion

        #region btnSendEmail_Click
        protected void btnSendEmail_Click(object sender, EventArgs e)
        {

            LogSession session = base.GetSessionInfo();
            var companyID = session.CompanyId;
            var vendorID = hidVendorID1.Value;
            //var companyName = lblVendorName1.Text;
            //var email = lblEmail1.Text;
            //var linktopass = lnkVendorEvaluation1.Text.Trim();

            //btnDuplicate.Visible = false;
  

            SystemDataActivity sDataActivity = new SystemDataActivity();
            GMSCore.Entity.VendorApplicationForm vendorAppForm = sDataActivity.RetrieveVendorApplicationFormByVendorID(GMSUtil.ToInt(hidVendorID1.Value.Trim()));

            //btnSendEmail.Visible = true;

            //linktopass += vendorAppForm.FormID.ToString();

            //var m = new ResponseModel();

            //try
            //{
            //    new GMSGeneralDALC().SendVendorEmail(companyID, companyName, email, linktopass);
            //}
            //catch (Exception x)
            //{
            //    m.Status = 1;
            //    m.Message = x.Message;
            //}

            //return m;
        }
        #endregion

    }
    #endregion
}
