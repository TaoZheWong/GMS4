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
    public partial class AddEditPurchase : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Sales");

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            94);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!IsPostBack)
            {
                LoadIndustryDDL();
                if (Request.Params["AccountCode"] != null)
                {
                    hidAccountCode.Value = Request.Params["AccountCode"].ToString().Trim();
                }
                if (Request.Params["PURCHASEID"] != null)
                {
                    hidPurchaseID.Value = Request.Params["PURCHASEID"].ToString();

                    LoadData();
                }

            }

            string javaScript =
@"
<script language=""javascript"" type=""text/javascript"" src=""/GMS/scripts/popcalendar.js""></script>
";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);




        }

        #region LoadDDL
        private void LoadIndustryDDL()
        {
            LogSession session = base.GetSessionInfo();

            SystemDataActivity sDataActivity = new SystemDataActivity();
            // fill in currency dropdown list
            IList<GMSCore.Entity.AccountIndustry> lstAccIndustry = null;

            lstAccIndustry = sDataActivity.RetrieveAllIndustry();
            ddlIndustry.DataSource = lstAccIndustry;
            ddlIndustry.DataBind();


            DataSet dsTemp = new DataSet();
            (new QuotationDataDALC()).GetAllUOMByCoyIDSelect(session.CompanyId, ref dsTemp);
            ddlUOM.DataSource = dsTemp;
            ddlUOM.DataValueField = "UOM";
            ddlUOM.DataTextField = "UOM";
            ddlUOM.DataBind();
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            if (hidPurchaseID.Value != "")
            {
                GMSCore.Entity.AccountPurchases accountPurchase = GMSCore.Entity.AccountPurchases.RetrieveByKey(hidPurchaseID.Value.Trim());
                
                if (accountPurchase != null)
                {
                    txtSupplier.Text = accountPurchase.Supplier;                   
                    ddlIndustry.SelectedIndex = GMSUtil.ToShort(accountPurchase.IndustryID) - 1;
                    txtProductGroup.Text = accountPurchase.ProductGroup;
                    txtProductName.Text = accountPurchase.ProductName;
                    ddlUOM.SelectedValue = accountPurchase.UOM.ToString();
                    txtQuantity.Text = accountPurchase.Qty.ToString();
                    if (accountPurchase.ContractEndDate != null)
                        contractEndDate.Text = accountPurchase.ContractEndDate.ToString("dd/MM/yyyy");                        
                    if (accountPurchase.Remarks != null)
                        txtRemarks.Text = accountPurchase.Remarks;                 

                }
                


            }

        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            LogSession session = base.GetSessionInfo();

            if (string.IsNullOrEmpty(hidPurchaseID.Value.Trim()))
            {
                #region Add New Record.
                try
                {
                    GMSCore.Entity.AccountPurchases accountPurchase = new GMSCore.Entity.AccountPurchases();

                    if (txtSupplier.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("Supplier Name cannot be empty.");
                        return;
                    }
                    accountPurchase.CoyID = session.CompanyId;
                    accountPurchase.AccountCode = this.hidAccountCode.Value.Trim();
                    accountPurchase.Supplier = txtSupplier.Text.Trim();
                    accountPurchase.IndustryID = ddlIndustry.SelectedValue; 
                    accountPurchase.ProductGroup = txtProductGroup.Text.Trim();
                    accountPurchase.ProductName = txtProductName.Text.Trim();
                    accountPurchase.UOM = ddlUOM.SelectedValue;
                    accountPurchase.Qty = GMSUtil.ToShort(txtQuantity.Text);

                    if (contractEndDate.Text.Trim() == "")
                        accountPurchase.ContractEndDate = GMSCoreBase.DEFAULT_NO_DATE;

                    else if (GMSUtil.ToDate(contractEndDate.Text.Trim()) != GMSCoreBase.DEFAULT_NO_DATE)
                        accountPurchase.ContractEndDate = GMSUtil.ToDate(contractEndDate.Text.Trim());

                    accountPurchase.Remarks = txtRemarks.Text.Trim();                   
                    accountPurchase.CreatedBy = session.UserId;
                    accountPurchase.CreatedDate = DateTime.Now;

                    accountPurchase.Save();
                    accountPurchase.Resync();
                    hidPurchaseID.Value = accountPurchase.PurchaseID.ToString();
                    LoadData();
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record added successfully! Add another one?';");
                    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
                    str.Append("if (r) {window.location.href = \"../../Debtors/Debtors/AddEditPurchase.aspx?AccountCode=" + this.hidAccountCode.Value.Trim() + "\";}");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
                #endregion
            }
            else
            {
                #region Update Record.
                try
                {
                    GMSCore.Entity.AccountPurchases accountPurchase = GMSCore.Entity.AccountPurchases.RetrieveByKey(hidPurchaseID.Value.Trim());
                    if (accountPurchase == null)
                    {
                        base.JScriptAlertMsg("This purchase cannot be found in database.");
                        return;
                    }
                    if (txtSupplier.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("Supplier Name cannot be empty.");
                        return;
                    }

                    accountPurchase.Supplier = txtSupplier.Text.Trim();
                    accountPurchase.IndustryID = ddlIndustry.SelectedValue;
                    accountPurchase.ProductGroup = txtProductGroup.Text.Trim();
                    accountPurchase.ProductName = txtProductName.Text.Trim();
                    accountPurchase.UOM = ddlUOM.SelectedValue;
                    accountPurchase.Qty = GMSUtil.ToShort(txtQuantity.Text);

                    if (contractEndDate.Text.Trim() == "")
                        accountPurchase.ContractEndDate = GMSCoreBase.DEFAULT_NO_DATE;

                    else if (GMSUtil.ToDate(contractEndDate.Text.Trim()) != GMSCoreBase.DEFAULT_NO_DATE)
                        accountPurchase.ContractEndDate = GMSUtil.ToDate(contractEndDate.Text.Trim());
                    accountPurchase.Remarks = txtRemarks.Text.Trim(); 
                    accountPurchase.ModifiedBy = session.UserId;
                    accountPurchase.ModifiedDate = DateTime.Now;

                    accountPurchase.Save();
                    accountPurchase.Resync();
                    hidPurchaseID.Value = accountPurchase.PurchaseID.ToString();
                    LoadData();
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
                    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
                    str.Append("if (r) {window.location.href = \"../../Debtors/Debtors/AddEditContact.aspx\";}");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
                #endregion
            }
            

        }
    }
}
