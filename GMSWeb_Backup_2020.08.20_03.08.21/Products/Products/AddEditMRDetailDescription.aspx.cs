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
using System.Collections.Generic;

namespace GMSWeb.Products.Products
{
    public partial class AddEditMRDetailDescription : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            if (!IsPostBack)
            {
                ViewState["MRNo"] = Request.Params["MRNo"].ToString().Trim();
                ViewState["DetailNo"] = Request.Params["DetailNo"].ToString().Trim();                
                LoadData();

            }

            string javaScript =
                @"<uctrl:Header ID=""MySiteHeader"" runat=""server"" EnableViewState=""true"" />
                <script language=""javascript"" type=""text/javascript"">
	                function update(ctr) {
	                    document.getElementById('"; javaScript += txtCounter.ClientID; javaScript += @"').value = ctr.value.length;
                     }
                </script>
                ";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<MRDetailDescription> dList = (new SystemDataActivity()).RetrieveAllMRDetailDescriptionListByCompanyCodeMRNoDetailNo(
                                                            session.CompanyId, ViewState["MRNo"].ToString(), GMSUtil.ToInt(ViewState["DetailNo"].ToString()));
            this.dgData.DataSource = dList;
            this.dgData.DataBind();
        }

        #region dgData_EditCommand
        protected void dgData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData.EditItemIndex = e.Item.ItemIndex;
            LoadData();

        }
        #endregion

        #region dgData_CancelCommand
        protected void dgData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgData_UpdateCommand
        protected void dgData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtEditDescription = (TextBox)e.Item.FindControl("txtEditDescription");
            HtmlInputHidden hidEditDescNo = (HtmlInputHidden)e.Item.FindControl("hidEditDescNo");
            MRDetailDescription dd = MRDetailDescription.RetrieveByKey(session.CompanyId, ViewState["MRNo"].ToString(),
                  GMSUtil.ToInt(ViewState["DetailNo"].ToString()), GMSUtil.ToByte(hidEditDescNo.Value.Trim()));

            string desc = txtEditDescription.Text.Trim();
            if (desc.Length > 2000)
                desc = desc.Substring(0, 2000);

            dd.Description = desc;
            dd.Save();
            dd.Resync();
            this.dgData.EditItemIndex = -1;
            LoadData();

        }
        #endregion



        #region dgData_CreateCommand
        protected void dgData_CreateCommand(object sender, DataGridCommandEventArgs e)
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
                                                                                 120);
                if (uAccess == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not authorised!')", true);
                    return;
                }

               

                IList<MRDetailDescription> dList = (new SystemDataActivity()).RetrieveAllMRDetailDescriptionListByCompanyCodeMRNoDetailNo(
                                                            session.CompanyId, ViewState["MRNo"].ToString(), GMSUtil.ToInt(ViewState["DetailNo"].ToString()));

                byte DescNo = 0;
                byte SeqID = 0;
                if (dList != null && dList.Count > 0)
                {
                    foreach (MRDetailDescription tempQDD in dList)
                    {
                        if (tempQDD.DescNo >= DescNo)
                            DescNo = (byte)(tempQDD.DescNo + 1);
                        if (tempQDD.SeqID >= SeqID)
                            SeqID = (byte)(tempQDD.SeqID + 1);
                    }
                }

                TextBox txtDescription = (TextBox)e.Item.FindControl("txtDescription");
                MRDetailDescription dd = new MRDetailDescription();
                dd.CoyID = session.CompanyId;
                dd.MRNo = ViewState["MRNo"].ToString();
                dd.DetailNo = GMSUtil.ToInt(ViewState["DetailNo"].ToString());
                dd.DescNo = DescNo;
                dd.Description = txtDescription.Text.Trim();               
                dd.SeqID = SeqID;
                dd.Save();
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
                                                                           120);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Product"));

                

                HtmlInputHidden hidDescNo = (HtmlInputHidden)e.Item.FindControl("hidDescNo");

                MRDetailDescription dd = MRDetailDescription.RetrieveByKey(session.CompanyId, ViewState["MRNo"].ToString(),
                      GMSUtil.ToInt(ViewState["DetailNo"].ToString()), GMSUtil.ToByte(hidDescNo.Value.Trim()));
                if (dd != null)
                {
                    IList<GMSCore.Entity.MRDetailDescription> dList = null;
                    dList = (new SystemDataActivity()).RetrieveAllMRDetailDescriptionListByCompanyCodeMRNoDetailNo(
                                                            session.CompanyId, ViewState["MRNo"].ToString(), GMSUtil.ToInt(ViewState["DetailNo"].ToString()));

                    MRDetailDescription swatDescription = null;
                    if (dList != null && dList.Count > 0)
                    {
                        foreach (MRDetailDescription pDetail in dList)
                        {
                            if (pDetail.SeqID < dd.SeqID)
                                swatDescription = pDetail;
                        }
                        if (swatDescription != null)
                        {
                            foreach (MRDetailDescription pDetail in dList)
                            {
                                if (pDetail.SeqID > swatDescription.SeqID && pDetail.SeqID < dd.SeqID)
                                    swatDescription = pDetail;
                            }
                        }
                    }
                    if (swatDescription != null && swatDescription.SeqID < dd.SeqID)
                    {
                        byte swat = dd.SeqID.Value;
                        dd.SeqID = swatDescription.SeqID.Value;
                        dd.Save();
                        dd.Resync();
                        swatDescription.SeqID = swat;
                        swatDescription.Save();
                        swatDescription.Resync();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
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
                                                                           120);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

               

                HtmlInputHidden hidDescNo = (HtmlInputHidden)e.Item.FindControl("hidDescNo");

                MRDetailDescription dd = MRDetailDescription.RetrieveByKey(session.CompanyId, ViewState["MRNo"].ToString(),
                      GMSUtil.ToInt(ViewState["DetailNo"].ToString()), GMSUtil.ToByte(hidDescNo.Value.Trim()));
                if (dd != null)
                {
                    IList<GMSCore.Entity.MRDetailDescription> dList = null;
                    dList = (new SystemDataActivity()).RetrieveAllMRDetailDescriptionListByCompanyCodeMRNoDetailNo(
                                                            session.CompanyId, ViewState["MRNo"].ToString(), GMSUtil.ToInt(ViewState["DetailNo"].ToString()));

                    MRDetailDescription swatDescription = null;
                    if (dList != null && dList.Count > 0)
                    {
                        foreach (MRDetailDescription pDetail in dList)
                        {
                            if (pDetail.SeqID > dd.SeqID)
                                swatDescription = pDetail;
                        }
                        if (swatDescription != null)
                        {
                            foreach (MRDetailDescription pDetail in dList)
                            {
                                if (pDetail.SeqID < swatDescription.SeqID && pDetail.SeqID > dd.SeqID)
                                    swatDescription = pDetail;
                            }
                        }
                    }
                    if (swatDescription != null && swatDescription.SeqID > dd.SeqID)
                    {
                        byte swat = dd.SeqID.Value;
                        dd.SeqID = swatDescription.SeqID.Value;
                        dd.Save();
                        dd.Resync();
                        swatDescription.SeqID = swat;
                        swatDescription.Save();
                        swatDescription.Resync();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
                }
                LoadData();
            }
            #endregion
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                 120);
                if (uAccess == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not authorised!')", true);
                    return;
                }

                

                HtmlInputHidden hidDescNo = (HtmlInputHidden)e.Item.FindControl("hidDescNo");
                MRDetailDescription dd = MRDetailDescription.RetrieveByKey(session.CompanyId, ViewState["MRNo"].ToString(),
                      GMSUtil.ToInt(ViewState["DetailNo"].ToString()), GMSUtil.ToByte(hidDescNo.Value.Trim()));
                dd.Delete();
                dd.Resync();

                LoadData();
            }
        }
        #endregion
    }
}
