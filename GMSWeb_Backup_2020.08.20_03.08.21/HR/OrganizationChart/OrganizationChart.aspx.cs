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
using System.IO;
using System.Data.SqlClient;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.HR.OrganizationChart
{
    public partial class OrganizationChart : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            64);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            65);
            if (uAccess == null)
            {
                btnEditLink.Visible = false;
                btnUploadLink.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                ViewState["PageID"] = 1;
                if (Request.QueryString["LinkToPageID"] != null)
                {
                    ViewState["PageID"] = Request.QueryString["LinkToPageID"];
                }
                GMSCore.Entity.OrganizationChart oc = new OrganizationChartActivity().RetrieveOrganizationChartById(GMSUtil.ToShort(ViewState["PageID"].ToString()));
                if (oc != null)
                {
                    OrChart.Src = "../../images/OrganizationChart/" + oc.PageID.ToString() + ".jpg" + "?t=" + new Random().Next().ToString();
                }
                else
                {
                    OrChart.Visible = false;
                }
                if (Request.QueryString["PageID"] != null)
                {
                    btnBack.NavigateUrl = "OrganizationChart.aspx?PageID=" + ViewState["PageID"].ToString() + "&LinkToPageID=" + Request.QueryString["PageID"].ToString();
                    btnBack.Visible = true;
                }
                IList<GMSCore.Entity.OrganizationChartLink> lstLink = null;
                lstLink = new SystemDataActivity().RetrieveAllOrganzationChartLinkByPageID((GMSUtil.ToShort(ViewState["PageID"].ToString())));
                if (lstLink.Count > 0)
                {
                    string inner = "";
                    int i = 1;
                    foreach (OrganizationChartLink ocLink in lstLink)
                    {
                        inner += "<AREA NAME=\"area" + i + "\" shape=\"rect\" COORDS=\"" + ocLink.FirstCoordinates + ", " + ocLink.SecondCoordinates + "\" HREF=\"OrganizationChart.aspx?PageID=" + ocLink.PageID + "&LinkToPageID=" + ocLink.LinkToPageID + "\" TARGET=\"_self\" alt=\"View Detail\" >";
                        i++;
                    }
                    map1.InnerHtml = inner;
                }
            }
        }

        #region btnUpload_Click
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    LogSession session = base.GetSessionInfo();
                    #region  Save chart into tbOrganizationChart
                    OrganizationChartActivity oActivity = new OrganizationChartActivity();
                    GMSCore.Entity.OrganizationChart organizationChart = new OrganizationChartActivity().RetrieveOrganizationChartById(GMSUtil.ToShort(ViewState["PageID"].ToString()));
                    if (organizationChart == null)
                    {
                        organizationChart = new GMSCore.Entity.OrganizationChart();
                    }

                    organizationChart.CoyID = session.CompanyId;
                    organizationChart.ImagePath = FileUpload1.FileName;

                    try
                    {
                        ResultType result = oActivity.CreateOrganizationChart(ref organizationChart, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                break;
                            default:
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                    #endregion
                    string folderPath = AppDomain.CurrentDomain.BaseDirectory + "images\\OrganizationChart";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    FileUpload1.SaveAs(folderPath + "\\" + organizationChart.Resync().PageID.ToString() + ".jpg");

                    lblMsg.Text = "File uploaded successfully. <br>" +
                                    "File Name: " + FileUpload1.PostedFile.FileName + "<br>" +
                                    "File Length: " + FileUpload1.PostedFile.ContentLength + " kb<br>" +
                                    "File Type: " + FileUpload1.PostedFile.ContentType;
                    Response.Redirect(Request.Url.ToString());

                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Error: " + ex.Message.ToString();
                }
            }
            else
            {
                lblMsg.Text = "You have not specified a file.";
            }
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.OrganizationChartLink> lstLink = null;
            try
            {
                lstLink = new SystemDataActivity().RetrieveAllOrganzationChartLinkByPageID((GMSUtil.ToShort(ViewState["PageID"].ToString())));
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgData.DataSource = lstLink;
            this.dgData.DataBind();

        }
        #endregion

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
            if (e.Item.ItemType == ListItemType.Footer && dgData.EditItemIndex < 0)
            {
                TextBox txtNewFirstCoordinates = (TextBox)e.Item.FindControl("txtNewFirstCoordinates");
                TextBox txtNewSecondCoordinates = (TextBox)e.Item.FindControl("txtNewSecondCoordinates");
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script1", "<script type=\"text/javascript\"> var txtNewFirstCoordinates = '" + txtNewFirstCoordinates.ClientID + "';</script>", false);
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script2", "<script type=\"text/javascript\"> var txtNewSecondCoordinates = '" + txtNewSecondCoordinates.ClientID + "';</script>", false);
                LinkButton linkCreate = (LinkButton)e.Item.FindControl("lnkCreate");
                scriptMgr.RegisterPostBackControl(linkCreate);
            }
            if (e.Item.ItemType == ListItemType.EditItem && this.dgData.EditItemIndex == e.Item.ItemIndex)
            {
                TextBox txtEditFirstCoordinates = (TextBox)e.Item.FindControl("txtEditFirstCoordinates");
                TextBox txtEditSecondCoordinates = (TextBox)e.Item.FindControl("txtEditSecondCoordinates");
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script3", "<script type=\"text/javascript\">var txtNewFirstCoordinates = '" + txtEditFirstCoordinates.ClientID + "';</script>", false);
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script4", "<script type=\"text/javascript\">var txtNewSecondCoordinates = '" + txtEditSecondCoordinates.ClientID + "';</script>", false);
            }
        }
        #endregion

        protected void btnEditLink_Click(object sender, EventArgs e)
        {
            btnCancelUpload_Click(sender, e);
            EditPanel.Style.Add("display", "");
            pointer_div.Attributes["onclick"] = "point_it(event)";
            btnEditLink.Visible = false;
            btnCancelEdit.Visible = true;
            map1.InnerHtml = "";
            LoadData();
        }

        protected void btnUploadLink_Click(object sender, EventArgs e)
        {
            btnCancelEdit_Click(sender, e);
            UploadPanel.Style.Add("display", "");
            btnUploadLink.Visible = false;
            btnCancelUpload.Visible = true;
            LoadData();
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            EditPanel.Style.Add("display", "none");
            pointer_div.Attributes.Remove("onclick");
            btnCancelEdit.Visible = false;
            btnEditLink.Visible = true;
            IList<GMSCore.Entity.OrganizationChartLink> lstLink = null;
            lstLink = new SystemDataActivity().RetrieveAllOrganzationChartLinkByPageID((GMSUtil.ToShort(ViewState["PageID"].ToString())));
            if (lstLink.Count > 0)
            {
                string inner = "";
                int i = 1;
                foreach (OrganizationChartLink ocLink in lstLink)
                {
                    inner += "<AREA NAME=\"area" + i + "\" shape=\"rect\" COORDS=\"" + ocLink.FirstCoordinates + ", " + ocLink.SecondCoordinates + "\" HREF=\"OrganizationChart.aspx?PageID=" + ocLink.PageID + "&LinkToPageID=" + ocLink.LinkToPageID + "\" TARGET=\"_self\" alt=\"View Detail\" >";
                    i++;
                }
                map1.InnerHtml = inner;
            }
        }

        protected void btnCancelUpload_Click(object sender, EventArgs e)
        {
            UploadPanel.Style.Add("display", "none");
            btnCancelUpload.Visible = false;
            btnUploadLink.Visible = true;
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
            TextBox txtEditFirstCoordinates = (TextBox)e.Item.FindControl("txtEditFirstCoordinates");
            TextBox txtEditSecondCoordinates = (TextBox)e.Item.FindControl("txtEditSecondCoordinates");
            HtmlInputHidden hidLinkID = (HtmlInputHidden)e.Item.FindControl("hidLinkID");

            if (txtEditFirstCoordinates != null && !string.IsNullOrEmpty(txtEditFirstCoordinates.Text) && hidLinkID != null && txtEditSecondCoordinates != null && !string.IsNullOrEmpty(txtEditSecondCoordinates.Text))
            {
                LogSession session = base.GetSessionInfo();

                GMSCore.Entity.OrganizationChartLink oChartLink = OrganizationChartLink.RetrieveByKey(GMSUtil.ToShort(hidLinkID.Value.Trim()));

                oChartLink.FirstCoordinates = txtEditFirstCoordinates.Text.Trim();
                oChartLink.SecondCoordinates = txtEditSecondCoordinates.Text.Trim();
                try
                {
                    ResultType result = new OrganizationChartActivity().UpdateOrganizationChartLink(ref oChartLink, session);

                    switch (result)
                    {
                        case ResultType.Ok:
                            this.dgData.EditItemIndex = -1;
                            LoadData();
                            break;
                        default:
                            this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                            return;
                    }
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
        }
        #endregion

        #region dgData_CreateCommand
        protected void dgData_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();

                TextBox txtNewFirstCoordinates = (TextBox)e.Item.FindControl("txtNewFirstCoordinates");
                TextBox txtNewSecondCoordinates = (TextBox)e.Item.FindControl("txtNewSecondCoordinates");
                FileUpload newFileUpload = (FileUpload)e.Item.FindControl("newFileUpload");
                GMSCore.Entity.OrganizationChart organizationChart = new GMSCore.Entity.OrganizationChart();

                if (txtNewFirstCoordinates != null && !string.IsNullOrEmpty(txtNewFirstCoordinates.Text) && txtNewSecondCoordinates != null && !string.IsNullOrEmpty(txtNewSecondCoordinates.Text) &&
                    newFileUpload != null)
                {
                    if (newFileUpload.HasFile)
                    {
                        try
                        {
                            #region  Save chart into tbOrganizationChart
                            OrganizationChartActivity oActivity = new OrganizationChartActivity();
                            organizationChart.CoyID = session.CompanyId;
                            organizationChart.ImagePath = newFileUpload.FileName;

                            try
                            {
                                ResultType result = oActivity.CreateOrganizationChart(ref organizationChart, session);

                                switch (result)
                                {
                                    case ResultType.Ok:
                                        break;
                                    default:
                                        this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                        return;
                                }
                            }
                            catch (Exception ex)
                            {
                                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                                return;
                            }
                            #endregion

                            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "images\\OrganizationChart";
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }
                            newFileUpload.SaveAs(folderPath + "\\" + organizationChart.Resync().PageID.ToString() + ".jpg");

                        }
                        catch (Exception ex)
                        {
                            lblMsg.Text = "Error: " + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        lblMsg.Text = "You have not specified a file.";
                    }
                    try
                    {

                        GMSCore.Entity.OrganizationChartLink existingOC = new OrganizationChartLink();
                        existingOC.PageID = GMSUtil.ToShort(ViewState["PageID"].ToString());
                        existingOC.FirstCoordinates = txtNewFirstCoordinates.Text.Trim();
                        existingOC.SecondCoordinates = txtNewSecondCoordinates.Text.Trim();
                        existingOC.LinkToPageID = organizationChart.Resync().PageID;

                        ResultType result = new OrganizationChartActivity().CreateOrganizationChartLink(ref existingOC, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                Page.Response.Redirect(Page.Request.Url.ToString(), true);
                                break;
                            default:
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                HtmlInputHidden hidLinkID = (HtmlInputHidden)e.Item.FindControl("hidLinkID");

                if (hidLinkID != null)
                {
                    LogSession session = base.GetSessionInfo();

                    OrganizationChartActivity oActivity = new OrganizationChartActivity();

                    try
                    {
                        ResultType result = oActivity.DeleteOrganizationChartLink(GMSUtil.ToShort(hidLinkID.Value.Trim()), session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                IList<GMSCore.Entity.OrganizationChart> lstOChart = GMSCore.Entity.OrganizationChart.RetrieveAll();
                                lstOChart.Remove(new OrganizationChartActivity().RetrieveOrganizationChartById(1));
                                bool flag = true;
                                while (flag == true)
                                {
                                    flag = false;
                                    foreach (GMSCore.Entity.OrganizationChart oChart in lstOChart)
                                    {
                                        if (oChart.LinkToOrganizationChartLinkList.Count <= 0)
                                        {
                                            IList<OrganizationChartLink> lstOCLink = new SystemDataActivity().RetrieveAllOrganzationChartLinkByPageID(oChart.PageID);
                                            foreach (OrganizationChartLink OLink in lstOCLink)
                                            {
                                                new OrganizationChartActivity().DeleteOrganizationChartLink(OLink.LinkID, session);
                                            }
                                            string path = AppDomain.CurrentDomain.BaseDirectory + "images\\OrganizationChart\\" + oChart.PageID.ToString() + ".jpg";
                                            if (File.Exists(path))
                                            {
                                                File.Delete(path);
                                            }
                                            new OrganizationChartActivity().DeleteOrganizationChart(oChart.PageID, session);
                                            lstOChart = GMSCore.Entity.OrganizationChart.RetrieveAll();
                                            lstOChart.Remove(new OrganizationChartActivity().RetrieveOrganizationChartById(1));
                                            flag = true;
                                        }
                                    }
                                }
                                this.dgData.EditItemIndex = -1;
                                LoadData();
                                break;
                            default:
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                    }
                    catch (SqlException exSql)
                    {
                        if (exSql.Number == 547)
                        {
                            this.PageMsgPanel.ShowMessage("This course cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                            LoadData();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadData();
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
