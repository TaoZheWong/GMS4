using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace GMSWeb.Sales.Sales
{
    public partial class RadPriceList : GMSBasePage
    {
        bool isMgt = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "Sales";
            lblPageHeader.Text = "Sales";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                if (Request.Params["CurrentLink"].ToString().Trim() == "Sales")
                    lblPageHeader.Text = "Sales";
                else
                    lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            166);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage(currentLink));

            UserAccessModule uAccessforMgt = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            167);

            if (uAccessforMgt != null)
                isMgt = true;

        }

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.RadGrid1.CurrentPageIndex = 0;
            this.RadGrid2.CurrentPageIndex = 0;
            RetrieveProduct();
            this.RadGrid1.DataBind();
            this.RadGrid2.DataBind();
        }
        #endregion

        #region RetrieveProduct
        private void RetrieveProduct()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            string ProductCode = "";
            string ProductName = "";
            string ProductGroupCode = "";
            string ProductGroup = "";
            string status = "";

            if (string.IsNullOrEmpty(txtProductGroupCode.Text.Trim()) && string.IsNullOrEmpty(txtProductGroup.Text.Trim()) &&
            string.IsNullOrEmpty(txtProductCode.Text.Trim()) && string.IsNullOrEmpty(txtProductName.Text.Trim()))
            {
                this.MsgPanel2.ShowMessage("Please input brand/product code or name to search", MessagePanelControl.MessageEnumType.Alert);
                return;
            }
            else
            {
                ProductCode = "%" + txtProductCode.Text.Trim() + "%";
                ProductName = "%" + txtProductName.Text.Trim() + "%";
                ProductGroupCode = "%" + txtProductGroupCode.Text.Trim() + "%";
                ProductGroup = "%" + txtProductGroup.Text.Trim() + "%";
                status = ddlSearchStatus.SelectedValue.Trim();
                resultList.Visible = true;
            }

            try
            {
                ggdal.RetrieveProductPrice(session.CompanyId, ProductGroupCode, ProductGroup, ProductCode, ProductName, session.UserId, status, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                this.lblSearchSummary.Visible = false;
                if (ddlSearchStatus.SelectedValue.Trim() != "")
                {
                    this.RadGrid1.Visible = false;
                    this.RadGrid1.DataSource = null;
                    this.RadGrid2.Visible = true;
                    this.RadGrid2.DataSource = ds;
                }
                else
                {
                    this.RadGrid1.Visible = true;
                    this.RadGrid1.DataSource = ds;
                    this.RadGrid2.Visible = false;
                    this.RadGrid2.DataSource = null;
                }
            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                if (ddlSearchStatus.SelectedValue.Trim() != "")
                {
                    this.RadGrid1.Visible = false;
                    this.RadGrid1.DataSource = null;
                    this.RadGrid2.Visible = false;
                    this.RadGrid2.DataSource = null;
                }
                else
                {
                    this.RadGrid1.Visible = false;
                    this.RadGrid1.DataSource = null;
                    this.RadGrid2.Visible = false;
                    this.RadGrid2.DataSource = null;
                }
            }
        }
        #endregion
        #region Current Price list Rad grid
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RetrieveProduct();
        }

        protected void radGrid_OnPageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveProduct();
        }

        protected void radGrid_OnPageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageSize;
            RetrieveProduct();
        }

        protected void radGrid_OnCancel(object source, GridCommandEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.MasterTableView.ClearEditItems();
            RetrieveProduct();
        }

        protected void RadGrid1_OnUpdateCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GridEditableItem item = (GridEditableItem)e.Item;
            String productCode = item.GetDataKeyValue("ProductCode").ToString();
            TextBox txtDealerPrice = (TextBox)item["DealerPrice"].FindControl("txtDealerPrice");
            TextBox txtUserPrice = (TextBox)item["UserPrice"].FindControl("txtUserPrice");
            TextBox txtRetailPrice = (TextBox)item["RetailPrice"].FindControl("txtRetailPrice");
            TextBox txtReorderLevel = (TextBox)item["ReorderLevel"].FindControl("txtReorderLevel");
            CheckBox chkbxTradingStock = (CheckBox)item["TradingStock"].FindControl("chkbxTradingStock");
            TextBox txtEffectiveDate = (TextBox)item["EffectiveDate"].FindControl("txtEffectiveDate");
           try
            {
                ProductPrice pp_new = new ProductPrice();
                ProductPrice pp = ProductPrice.RetrieveByKey(session.CompanyId, productCode);
                Product p = Product.RetrieveByKey(session.CompanyId, productCode);
                pp_new.CoyID = pp.CoyID;
                pp_new.ProductCode = p.ProductCode;
                pp_new.ProductGroupCode = p.ProductGroupCode;
                pp_new.DealerPrice = decimal.Parse(txtDealerPrice.Text.Trim());
                pp_new.UserPrice = decimal.Parse(txtUserPrice.Text.Trim());
                pp_new.RetailPrice = decimal.Parse(txtRetailPrice.Text.Trim());
                pp_new.UpdatedBy = session.UserId;
                pp_new.UpdatedDate = DateTime.Now;
                pp_new.IsExpired = false;
                pp_new.ReorderLevel = int.Parse(txtReorderLevel.Text.Trim());
                pp_new.TradingStock = chkbxTradingStock.Checked;
                pp_new.EffectiveDate = DateTime.Parse(txtEffectiveDate.Text.Trim());
                pp_new.Status = "Pending";
                pp_new.Save();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Price changed is submitted for approval.')", true);
            }catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Fail to submit for approval.')", true);
                return;
            }
            RetrieveProduct();
        }
        #endregion

        #region Pending Price list Rad grid
        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RetrieveProduct();
        }

        protected void radGrid2_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if(e.Item.ItemType == GridItemType.Item)
            {
                LinkButton lnkApprove = (LinkButton)e.Item.FindControl("lnkApprove");
                LinkButton lnkReject = (LinkButton)e.Item.FindControl("lnkReject");
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete2");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");

                if(lnkApprove != null && lnkReject != null)
                {
                    if (isMgt)
                    {
                        lnkReject.Visible = true;
                        lnkApprove.Visible = true;
                    }
                }
            }
        }

        protected void radGrid2_OnPageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveProduct();
        }

        protected void radGrid2_OnPageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageSize;
            RetrieveProduct();
        }

        protected void radGrid2_OnCancel(object source, GridCommandEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.MasterTableView.ClearEditItems();
            RetrieveProduct();
        }

        protected void RadGrid2_OnDeleteCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string productCode = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ProductCode"].ToString();
            
            try
            {
                ProductPrice pp = ProductPrice.RetrieveByKeyPending(session.CompanyId, productCode);
                pp.Delete();
                pp.Resync();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Price submitted is deleted.')", true);

                RetrieveProduct();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Fail to delete.')", true);
            }
        }


        #region lnkApprove_Click
        protected void lnkApprove_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            LinkButton btn = (LinkButton)(sender);
            string hidProductCode = btn.CommandArgument;
            try
            {
                //ProductPrice pp = ProductPrice.RetrieveByKey(session.CompanyId, hidProductCode);
                //pp.IsExpired = true;
                //pp.Save();

                ProductPrice pp_new = ProductPrice.RetrieveByKeyPending(session.CompanyId, hidProductCode);
                pp_new.Status = "";
                pp_new.Save();
                this.PageMsgPanel.ShowMessage("Price is updated.", MessagePanelControl.MessageEnumType.Alert);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                return;
            }
            
            RetrieveProduct();

        }
        #endregion

        #region lnkReject_Click
        protected void lnkReject_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            LinkButton btn = (LinkButton)(sender);
            string[] arg = new string[4];
            arg = btn.CommandArgument.ToString().Split(',');
            string email =arg[0];
            string productGroupName = arg[1];
            string productCode = arg[2];
            string pmName = arg[3];
            try
            {
                ProductPrice pp_new = ProductPrice.RetrieveByKeyPending(session.CompanyId, productCode);
                pp_new.Status = "Rejected";
                pp_new.Save();

                RejectViaEmail(productGroupName, productCode, "Eason", "eason.tan@leedennox.com");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('An email has been sent to " + email + ".')", true);

                RetrieveProduct();
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                return;
            }
        }
        #endregion

        #region RejectViaEmail
        private void RejectViaEmail(string productGroup, string productCode, string userRealName, string userEmail)
        {
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);
            mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[GMS - Price Rejected]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Hi " + userRealName + ",</p>\n" +
                        "<p>Your submitted price has been rejected.</p>\n" +
                        "<p>Brand/Product: " + productGroup + "<br />\n" +
                        "Item Code: " + productCode + "</p>\n" +
                        "<br />" +
                        "***** This is a system-generated email. Please do not reply.*****";
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.Host = smtpServer;
                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
                mailClient.Credentials = authentication;
                mailClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #endregion

    }
}