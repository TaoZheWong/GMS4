using GMSCore;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System.Data;
using GhostscriptSharp;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace GMSWeb.HR.Recruitment
{
    public partial class Recruitment : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("CompanyHR");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            164);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyHR"));
            if (!Page.IsPostBack)
            {
                this.dgAttachment.CurrentPageIndex = 0;
                ViewState["SortAttachmentField"] = "CreatedDate";
                ViewState["SortDirection"] = "ASC";
                LoadData();
            }
            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>
            <script language=""javascript"" type=""text/javascript"">
            function ChangeSalesman(ctl)
            { 
                document.getElementById(ddlNewGPQ).value = ctl.value;
                if (document.getElementById(ddlNewGPQ).selectedIndex >= 0) 
                    document.getElementById(lblGPQ2).innerHTML = document.getElementById(ddlNewGPQ).options[document.getElementById(ddlNewGPQ).selectedIndex].text; 
                else 
                    document.getElementById(lblGPQ2).innerHTML = 'Nill'; 
                document.getElementById(ddlNewCommRate).value = ctl.value;
                if (document.getElementById(ddlNewCommRate).selectedIndex >= 0) 
                    document.getElementById(lblCommRate2).innerHTML = document.getElementById(ddlNewCommRate).options[document.getElementById(ddlNewCommRate).selectedIndex].text; 
                else 
                    document.getElementById(lblCommRate2).innerHTML = 'Nill';
            }
            </script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetResumeSelect(session.CompanyId, session.UserId, ref ds);
               
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgAttachment.CurrentPageIndex + 1) * this.dgAttachment.PageSize) - (this.dgAttachment.PageSize - 1);
            int endIndex = (dgAttachment.CurrentPageIndex + 1) * this.dgAttachment.PageSize;

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (endIndex < ds.Tables[0].Rows.Count)
                    this.lblAttachmentSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblAttachmentSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = ViewState["SortAttachmentField"].ToString() + " " + ViewState["SortDirection"].ToString();
                this.lblAttachmentSummary.Visible = true;
                this.dgAttachment.DataSource = dv;
                this.dgAttachment.DataBind();
                this.dgAttachment.Visible = true;
            }
            else
            {
                this.lblAttachmentSummary.Text = "No records.";
                this.lblAttachmentSummary.Visible = true;
                this.dgAttachment.DataSource = null;
                this.dgAttachment.DataBind();
            }
        }
        #endregion

        protected void dgAttachment_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                    return;
                }
                HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidFileID");
                HtmlInputHidden hidFileName = (HtmlInputHidden)e.Item.FindControl("hidFileName");
                if (hidID != null)
                {
                    ResumeActivity apActivity = new ResumeActivity();
                    try
                    {
                        ResultType result = apActivity.DeleteResumeUpload(hidID.Value, session);
                        switch (result)
                        {
                            case ResultType.Ok:
                                string path = @"D:\\GMSDocuments\\Resume\\" + session.CompanyId.ToString() + "\\" + hidFileName.Value.ToString()+"_";
                                Directory.Delete(path,true);
                                this.dgAttachment.EditItemIndex = -1;
                                this.dgAttachment.CurrentPageIndex = 0;
                                lblMsg.Text = "Record deleted successfully!<br /><br />";
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
        }
        protected void SortAttachment(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["SortAttachmentField"].ToString())
            {
                switch (ViewState["SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["SortAttachmentField"] = e.SortExpression;
                ViewState["SortDirection"] = "ASC";
            }
            LoadData();
        }

        protected void dgAttachment_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        protected void dgAttachment_Command(Object sender, DataGridCommandEventArgs e)
        {
            
        }

        protected void dgAttachment_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }

        #region btnUpload_Click
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            164);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyHR"));

            FileUpload FileUpload = (FileUpload)upAttachment.FindControl("FileUpload");
            if (FileUpload.HasFile)
            {
                try
                {
                    if ((Path.GetExtension(this.FileUpload.FileName)).ToUpper() != ".PDF")
                    {
                        ScriptManager.RegisterStartupScript(upAttachment, upAttachment.GetType(), "Alert", "alert('Executable File Extensions is not allow!');", true);
                        return;
                    }
                    string folderPath = @"D:\\GMSDocuments\\Resume\\" + session.CompanyId;
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    
                    string randomIDFileName = (txtName.Text.Trim() + "-Resume" + Path.GetExtension(this.FileUpload.FileName)).Replace(" ","");
                    string imagePath = @"D:\\GMSDocuments\\Resume\\" + session.CompanyId + "\\" + randomIDFileName + "_";
                    if (!Directory.Exists(imagePath))
                       Directory.CreateDirectory(imagePath);
                    try
                    {   
                        ResumeUpload resumeUpload = new ResumeUpload();
                        resumeUpload.CoyID = session.CompanyId;
                        resumeUpload.FileName = randomIDFileName;
                        resumeUpload.CandidateName = txtName.Text.Trim().Replace(" ", "");
                        resumeUpload.CreatedBy = session.UserId;
                        resumeUpload.CreatedDate = DateTime.Now;
                        ResultType result = new ResumeActivity().CreateResumeUpload(ref resumeUpload, session);
                        switch (result)
                        {
                            case ResultType.Ok:
                                //save pdf in D:/ drive first
                                FileUpload.SaveAs(folderPath + "/" + randomIDFileName);
                                LoadData();
                                txtName.Text = "";
                                this.FileUpload.Attributes.Clear();
                                //then convert pdf to separate images here
                                GhostscriptWrapper.GeneratePageThumbs(folderPath + "\\" + randomIDFileName, imagePath + "\\" + randomIDFileName + "%d.png", 1, 5, 130, 130);
                                //merge all images into single image here
                                combineImage(imagePath, randomIDFileName);  

                                string path = @"D:\\GMSDocuments\\Resume\\" + session.CompanyId.ToString() + "\\" + randomIDFileName;
                                File.Delete(path);

                                ScriptManager.RegisterStartupScript(upAttachment, upAttachment.GetType(), "Report1", "alert('File has been uploaded!');", true);
                                break;

                            default:

                                txtName.Text = "";
                                this.FileUpload.Attributes.Clear();
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message + " Duplicate Candidate's name exists in System.");
                    }
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }          
        }
        #endregion

        public void lnkView_Click(object sender, CommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                return;
            }
            string FileName = Encrypt(e.CommandArgument.ToString());
            string CoyID = Encrypt(session.CompanyId.ToString());
            string strPopup = "<script language='javascript' ID='script1'>"
           + "window.open('../../HR/Recruitment/ViewPage.aspx?ID=" + CoyID + "&FileName=" + FileName 
          + "','new window', 'top=90, left=200, width=100%, height=auto, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=yes, toolbar=no, status=no, center=yes')"
           + "</script>";
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        }

        private static string Encrypt(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private void combineImage(string imagePath, string randomIDFileName)
        {
            int width = 800;//default width
            int height = 0;//default height

            int FileNumber = Directory.GetFiles(imagePath, "*.png").Length;
            if (FileNumber == 1)
                height = 1200;
            else if (FileNumber == 2)
                height = 2450;
            else if (FileNumber == 3)
                height = 3700;
            else if (FileNumber == 4)
                height = 4950;
            else if (FileNumber == 5)
                height = 6200;

            DirectoryInfo directory = new DirectoryInfo(imagePath);
            FileInfo[] files = directory.GetFiles("*.png");
            int yaxis = 0;
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(SystemColors.AppWorkspace);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                foreach (FileInfo file in files)
                {
                    Bitmap img = new Bitmap(file.FullName);
                    g.DrawImage(img, new Rectangle(0, yaxis, width, 1200));
                    yaxis = yaxis + 1250;
                    img.Dispose();
                    file.Delete();
                }
                g.Dispose();
            }
            bitmap.Save(imagePath + "\\" + randomIDFileName + ".png", ImageFormat.Png);
            bitmap.Dispose();
        }

        public void lnkSend_Click(object sender, CommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                return;
            }
            string FileName = Encrypt(e.CommandArgument.ToString());
            string CoyID = Encrypt(session.CompanyId.ToString());
            string hr = Encrypt(session.UserRealName);
            string strPopup = "<script language='javascript' ID='script1'>"
           + "window.open('../../HR/Recruitment/EmailResume.aspx?ID=" + CoyID + "&FileName=" + FileName + "&HRinCharge=" + hr
           + "','new window', 'top=90, left=200, width=600, height=420, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=no, toolbar=no, status=no, center=yes')"
           + "</script>";
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
       }
    }
}