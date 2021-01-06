using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using System.Text;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Debtors.Debtors
{
    public partial class AddEditContact : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadSalutationDDL();

               
                if (Request.Params["ContactID"] != null)
                    LoadSingleContactData(Request.Params["ContactID"].ToString());
            }
            
        }


        private void LoadSingleContactData(string hidContactID)
        {
            LogSession session = base.GetSessionInfo();

            if (hidContactID != "")
            {

                GMSCore.Entity.AccountContacts accountContacts = new AccountContactActivity().RetrieveAccountContactByID(session.CompanyId, hidContactID);

                if (accountContacts != null)
                {
                    hidContactID1.Value = hidContactID.ToString();
                    txtFirstName.Text = accountContacts.FirstName;
                    txtLastName.Text = accountContacts.LastName;
                    ddlSalutation.SelectedValue = accountContacts.Salutation;
                    txtDesignation.Text = accountContacts.Designation;
                    txtOfficePhone.Text = accountContacts.OfficePhone;
                    txtMobilePhone.Text = accountContacts.MobilePhone;
                    txtFax.Text = accountContacts.Fax;
                    txtEmail.Text = accountContacts.Email;
                    if (accountContacts.Remarks != null)
                        txtContactRemarks.Text = accountContacts.Remarks;
                    chkIsActive.Checked = (bool)accountContacts.IsActive;

                }


            }
            else
            {

                txtFirstName.Text = "";
                txtLastName.Text = "";
                ddlSalutation.SelectedIndex = -1;
                txtDesignation.Text = "";
                txtOfficePhone.Text = "";
                txtMobilePhone.Text = "";
                txtFax.Text = "";
                txtEmail.Text = "";

                txtContactRemarks.Text = "";
                chkIsActive.Checked = true;
            }

        }

        #region LoadDDL
        private void LoadSalutationDDL()
        {
            DataSet dsSalutation = new DataSet();
            (new GMSGeneralDALC()).GetSalutationSelect(ref dsSalutation);
            ddlSalutation.DataSource = dsSalutation;
            ddlSalutation.DataBind();
        }
        #endregion

        protected void btnSubmitData_Click(object sender, EventArgs e)
        {
            

            LogSession session = base.GetSessionInfo();

            // Request.Params["ContactID"].ToString() != ""

            if (Request.Params["ContactID"] == null)
            {
                #region Add New Record.
                try
                {
                    GMSCore.Entity.AccountContacts accountContacts = new GMSCore.Entity.AccountContacts();

                    if (txtFirstName.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("First Name cannot be empty.");
                        return;
                    }

                    DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(session.CompanyId, (short)DateTime.Now.Year);
                    if (documentNumber == null) //If tbDocumentNumber does not exist
                    {
                        documentNumber = new DocumentNumber();
                        documentNumber.CoyID = session.CompanyId;
                        documentNumber.Year = (short)DateTime.Now.Year;
                        documentNumber.QuotationNo = "0001";
                        documentNumber.ExternalCourseCodePrefix = "E";
                        documentNumber.ExternalCourseCodeNumber = "001";
                        documentNumber.InternalCourseCodePrefix = "I";
                        documentNumber.InternalCourseCodeNumber = "001";
                        documentNumber.OrganizerID = 0;
                        documentNumber.EmployeeCourseRowID = 0;
                        documentNumber.EmployeeID = 0;
                        documentNumber.AttachmentNo = "0001";
                        documentNumber.ProspectNo = "0001";
                        documentNumber.ContactNo = "0001";
                        documentNumber.CommNo = "0001";
                        documentNumber.CommCommentNo = "0001";
                        documentNumber.PurchaseNo = "0001";

                    }

                    accountContacts.CoyID = session.CompanyId;
                    accountContacts.ContactID = documentNumber.ContactNo;
                    accountContacts.AccountCode = Request.Params["AccountCode"].ToString();
                    accountContacts.FirstName = txtFirstName.Text.Trim();
                    accountContacts.LastName = txtLastName.Text.Trim();
                    accountContacts.Salutation = ddlSalutation.SelectedValue;
                    accountContacts.Designation = txtDesignation.Text.Trim();
                    accountContacts.OfficePhone = txtOfficePhone.Text.Trim();
                    accountContacts.MobilePhone = txtMobilePhone.Text.Trim();
                    accountContacts.Fax = txtFax.Text;
                    accountContacts.Email = txtEmail.Text;
                    accountContacts.Remarks = txtContactRemarks.Text.Trim();
                    accountContacts.IsActive = chkIsActive.Checked;
                    accountContacts.CreatedBy = session.UserId;
                    accountContacts.CreatedDate = DateTime.Now;

                    accountContacts.Save();
                    accountContacts.Resync();

                    string nxtStr = ((short)(short.Parse(documentNumber.ContactNo) + 1)).ToString();
                    for (int i = nxtStr.Length; i < documentNumber.ContactNo.Length; i++)
                    {
                        nxtStr = "0" + nxtStr;
                    }
                    documentNumber.ContactNo = nxtStr;
                    documentNumber.Save();  


                    lblContactsMsg.Text = "Record created successfully!<br /><br />";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "parent.parent.window.location.reload();parent.parent.GB_hide();", true);
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Record created successfully!')", true);
                    //LoadContactsData();
                    
                    
                }
                catch (Exception ex)
                {
                    //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
                #endregion
            }
            else
            {
                #region Update Record.
                try
                {

                    GMSCore.Entity.AccountContacts accountContacts = new AccountContactActivity().RetrieveAccountContactByID(session.CompanyId, Request.Params["ContactID"].ToString());
                    
                    if (accountContacts == null)
                    {
                        base.JScriptAlertMsg("This contact cannot be found in database.");
                        return;
                    }
                    if (txtFirstName.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("First Name cannot be empty.");
                        return;
                    }

                    accountContacts.FirstName = txtFirstName.Text.Trim();
                    accountContacts.LastName = txtLastName.Text.Trim();
                    accountContacts.Salutation = ddlSalutation.SelectedValue;
                    accountContacts.Designation = txtDesignation.Text.Trim();
                    accountContacts.OfficePhone = txtOfficePhone.Text.Trim();
                    accountContacts.MobilePhone = txtMobilePhone.Text.Trim();
                    accountContacts.Fax = txtFax.Text;
                    accountContacts.Email = txtEmail.Text;
                    accountContacts.Remarks = txtContactRemarks.Text.Trim();
                    accountContacts.IsActive = chkIsActive.Checked;
                    accountContacts.ModifiedBy = session.UserId;
                    accountContacts.ModifiedDate = DateTime.Now;

                    accountContacts.Save();
                    accountContacts.Resync();
                    lblContactsMsg.Text = "Record modified successfully!<br /><br />";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "parent.parent.window.location.reload();parent.parent.GB_hide();", true);
                    //LoadContactsData();
                   
                }
                catch (Exception ex)
                {
                    //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
                #endregion
            }
        }
       

        
             

        
    }
}
