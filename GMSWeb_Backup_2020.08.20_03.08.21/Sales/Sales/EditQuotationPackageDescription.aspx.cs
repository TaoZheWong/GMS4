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
using GMSCore.Activity;
using GMSCore.Entity;
using System.Collections.Generic;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Sales.Sales
{
    public partial class EditQuotationPackageDescription : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Master.setCurrentLink("Sales");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            105);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                if (Request.Params["QuotationNo"] != null)
                {
                    ViewState["QuotationNo"] = Request.Params["QuotationNo"].ToString().Trim();
                }
                if (Request.Params["SNNo"] != null)
                {
                    ViewState["SNNo"] = Request.Params["SNNo"].ToString().Trim();
                }
                ViewState["RevisionNo"] = Request.Params["RevisionNo"].ToString().Trim();
                //preload
                this.dgDetail.CurrentPageIndex = 0;
                LoadData();
            }

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>
            <script language=""javascript"" type=""text/javascript"">
	            function update(ctr) {
	                document.getElementById('" + txtCounter.ClientID + @"').value = ctr.value.length;
                 }
            </script>
            ";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            QuotationPackage qp = QuotationPackage.RetrieveByKey(session.CompanyId, ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), GMSUtil.ToByte(ViewState["RevisionNo"]));
            lblPackageProductCode.Text = qp.ProductCode;
            lblPackageProductDescription.Text = qp.ProductDescription;

            IList<GMSCore.Entity.QuotationPackageDetail> lstPackageDetail = null;
            try
            {
                lstPackageDetail = new SystemDataActivity().RetrieveAllPackageDetailByQuotationNoSNNo(session.CompanyId,
                                         ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), GMSUtil.ToByte(ViewState["RevisionNo"]));
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgDetail.DataSource = lstPackageDetail;
            this.dgDetail.DataBind();
        }
        #endregion

        #region dgDetail_ItemCommand
        protected void dgDetail_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            #region Create
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                           102);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                if (ViewState["RevisionNo"].ToString() != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                    return;
                }

                TextBox txtNewProductDescription = (TextBox)e.Item.FindControl("txtNewProductDescription");

                IList<QuotationPackageDetail> lstPackageDetail = (new SystemDataActivity()).RetrieveAllPackageDetailByQuotationNoSNNo(session.CompanyId,
                                         ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), GMSUtil.ToByte(ViewState["RevisionNo"]));
                byte detailID = 0;
                byte SeqID = 0;
                if (lstPackageDetail != null && lstPackageDetail.Count > 0)
                {
                    foreach (QuotationPackageDetail tempQP in lstPackageDetail)
                    {
                        if (tempQP.DetailID >= detailID)
                            detailID = (byte)(tempQP.DetailID + 1);
                        if (tempQP.SeqID >= SeqID)
                            SeqID = (byte)(tempQP.SeqID + 1);
                    }
                }

                try
                {
                    GMSCore.Entity.QuotationPackageDetail qpd = new GMSCore.Entity.QuotationPackageDetail();
                    qpd.CoyID = session.CompanyId;
                    qpd.QuotationNo = ViewState["QuotationNo"].ToString();
                    qpd.SNNo = GMSUtil.ToByte(ViewState["SNNo"]);
                    qpd.DetailID = detailID;
                    qpd.Description = txtNewProductDescription.Text.Trim();
                    qpd.CreatedBy = session.UserId;
                    qpd.CreatedDate = DateTime.Now;
                    qpd.RevisionNo = 0;
                    qpd.SeqID = SeqID;
                    qpd.Save();
                    qpd.Resync();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "click", "alert('" + ex.Message + "')", true);
                }

                LoadData();
            }
            #endregion

            #region GoUp
            if (e.CommandName == "GoUp")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                           102);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                if (ViewState["RevisionNo"].ToString() != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                    return;
                }

                HtmlInputHidden hidDetailID = (HtmlInputHidden)e.Item.FindControl("hidDetailID");

                QuotationPackageDetail qpd = QuotationPackageDetail.RetrieveByKey(session.CompanyId, ViewState["QuotationNo"].ToString(), 
                                               GMSUtil.ToByte(ViewState["SNNo"]), GMSUtil.ToByte(hidDetailID.Value.Trim()), 0);
                if (qpd != null)
                {
                    IList<GMSCore.Entity.QuotationPackageDetail> lstPackageDetail = null;
                    try
                    {
                       lstPackageDetail = (new SystemDataActivity()).RetrieveAllPackageDetailByQuotationNoSNNo(session.CompanyId,
                                         ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), 0);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    }

                    QuotationPackageDetail swatPackage = null;
                    if (lstPackageDetail != null && lstPackageDetail.Count > 0)
                    {
                        foreach (QuotationPackageDetail pDetail in lstPackageDetail)
                        {
                            if (pDetail.SeqID < qpd.SeqID)
                                swatPackage = pDetail;
                        }
                        if (swatPackage != null)
                        {
                            foreach (QuotationPackageDetail pDetail in lstPackageDetail)
                            {
                                if (pDetail.SeqID > swatPackage.SeqID && pDetail.SeqID < qpd.SeqID)
                                    swatPackage = pDetail;
                            }
                        }
                    }
                    if (swatPackage != null && swatPackage.SeqID < qpd.SeqID)
                    {
                        byte swat = qpd.SeqID.Value;
                        qpd.SeqID = swatPackage.SeqID.Value;
                        qpd.Save();
                        qpd.Resync();
                        swatPackage.SeqID = swat;
                        swatPackage.Save();
                        swatPackage.Resync();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
                }
                LoadData();
            }
            #endregion

            #region GoDown
            if (e.CommandName == "GoDown")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                           102);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                if (ViewState["RevisionNo"].ToString() != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                    return;
                }

                HtmlInputHidden hidDetailID = (HtmlInputHidden)e.Item.FindControl("hidDetailID");

                QuotationPackageDetail qpd = QuotationPackageDetail.RetrieveByKey(session.CompanyId,
                                                ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), GMSUtil.ToByte(hidDetailID.Value.Trim()), 0);
                if (qpd != null)
                {
                    IList<GMSCore.Entity.QuotationPackageDetail> lstPackageDetail = null;
                    try
                    {
                        lstPackageDetail = (new SystemDataActivity()).RetrieveAllPackageDetailByQuotationNoSNNo(session.CompanyId,
                                         ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), 0);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    }

                    QuotationPackageDetail swatPackage = null;
                    if (lstPackageDetail != null && lstPackageDetail.Count > 0)
                    {
                        foreach (QuotationPackageDetail pDetail in lstPackageDetail)
                        {
                            if (pDetail.SeqID > qpd.SeqID)
                                swatPackage = pDetail;
                        }
                        if (swatPackage != null)
                        {
                            foreach (QuotationPackageDetail pDetail in lstPackageDetail)
                            {
                                if (pDetail.SeqID < swatPackage.SeqID && pDetail.SeqID > qpd.SeqID)
                                    swatPackage = pDetail;
                            }
                        }
                    }
                    if (swatPackage != null && swatPackage.SeqID > qpd.SeqID)
                    {
                        byte swat = qpd.SeqID.Value;
                        qpd.SeqID = swatPackage.SeqID.Value;
                        qpd.Save();
                        qpd.Resync();
                        swatPackage.SeqID = swat;
                        swatPackage.Save();
                        swatPackage.Resync();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
                }
                LoadData();
            }
            #endregion
        }
        #endregion

        #region dgDetail_EditCommand
        protected void dgDetail_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                           102);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            this.dgDetail.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgDetail_CancelCommand
        protected void dgDetail_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgDetail.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgDetail_UpdateCommand
        protected void dgDetail_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                       102);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (ViewState["RevisionNo"].ToString() != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                return;
            }

            TextBox txtEditProductDescription = (TextBox)e.Item.FindControl("txtEditProductDescription");
            HtmlInputHidden hidDetailID = (HtmlInputHidden)e.Item.FindControl("hidDetailID");

            QuotationPackageDetail qpd = QuotationPackageDetail.RetrieveByKey(session.CompanyId, ViewState["QuotationNo"].ToString(),
                                               GMSUtil.ToByte(ViewState["SNNo"]), GMSUtil.ToByte(hidDetailID.Value.Trim()), 0);
            if (qpd != null)
            {
                qpd.Description = txtEditProductDescription.Text.Trim();
                qpd.Save();
                qpd.Resync();
                this.dgDetail.EditItemIndex = -1;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Update", "alert('Pacakge not found!')", true);
            }
            LoadData();
        }
        #endregion

        #region dgDetail_DeleteCommand
        protected void dgDetail_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                       102);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (ViewState["RevisionNo"].ToString() != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                return;
            }

            HtmlInputHidden hidDetailID = (HtmlInputHidden)e.Item.FindControl("hidDetailID");

            QuotationPackageDetail qpd = QuotationPackageDetail.RetrieveByKey(session.CompanyId, ViewState["QuotationNo"].ToString(),
                                               GMSUtil.ToByte(ViewState["SNNo"]), GMSUtil.ToByte(hidDetailID.Value.Trim()), 0);
            if (qpd != null)
            {
                qpd.Delete();
                qpd.Resync();
                this.dgDetail.EditItemIndex = -1;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
            }
            LoadData();
        }
        #endregion
    }
}
