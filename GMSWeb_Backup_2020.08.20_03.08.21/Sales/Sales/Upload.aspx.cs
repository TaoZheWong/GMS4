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
using System.IO;
using System.Data.SqlClient;

using GMSCore;
using GMSWeb.CustomCtrl;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Web.Services;
using System.Text;
using AjaxControlToolkit;

namespace GMSWeb.Sales.Sales
{
    public partial class Upload : GMSBasePage
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "Sales";


            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();

            }

            Master.setCurrentLink(currentLink); 

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            118);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            118);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            
            if (!Page.IsPostBack)
            {
                this.dgAttachment.CurrentPageIndex = 0;
                ViewState["SortAttachmentField"] = "CreatedDate";
                ViewState["SortDirection"] = "ASC";
                LoadAttachmentData();
            }

            

        }
        #endregion

        private void LoadAttachmentData()
        {
            LogSession session = base.GetSessionInfo();

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetSalesmanAttachmentSelect(session.CompanyId, session.UserId, ref ds);
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

        
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            FileUpload FileUpload1 = (FileUpload)upAttachment.FindControl("FileUpload1");
            if (FileUpload1.HasFile)
            {
                try
                {

                    if ((Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "ACTION" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "APK" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "APP" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "BAT" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "BIN" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "CMD" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "COM" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "COMMAND" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "CPL" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "CSH" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "EXE" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "GADGET" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "INF" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "INS" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "INX" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "IPA" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "ISU" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "JOB" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "JSE" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "KSH" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "LNK" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "MSC" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "MSI" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "MSP" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "MST" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "OSX" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "OUT" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "PAF" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "PIF" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "PRG" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "PS1" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "REG" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "RGS" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "RUN" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "SCT" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "SHB" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "SHS" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "U3P" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "VB" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "VBE" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "VBS" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "VBSCRIPT" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "WORKFLOW" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "WS" ||
                        (Path.GetExtension(this.FileUpload1.FileName)).ToUpper() == "WSF"
                        )
                    {
                        ScriptManager.RegisterStartupScript(upAttachment, upAttachment.GetType(), "Alert", "alert('Executable File Extensions is not allow!');", true);
                        
                        return;
                    }

                    string folderPath = "D://GMSDocuments/Salesman/" + session.CompanyId;
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string randomIDFileName = session.UserId.ToString() + DateTime.Now.Ticks.ToString() + Path.GetExtension(this.FileUpload1.FileName);


                    FileUpload1.SaveAs(folderPath + "/" + randomIDFileName);

                    
                    GMSCore.Entity.SalesmanUpload salesmanUpload = new GMSCore.Entity.SalesmanUpload();

                    salesmanUpload.CoyID = session.CompanyId;  
                    salesmanUpload.DocumentName = txtFileName.Text.Trim();
                    salesmanUpload.FileName = randomIDFileName;                    
                    salesmanUpload.CreatedBy = session.UserId;
                    salesmanUpload.CreatedDate = DateTime.Now;

                    ResultType result = new SalesmanUploadActivity().CreateSalesmanUpload(ref salesmanUpload, session);
                    

                    switch (result)
                    {
                        case ResultType.Ok:
                            LoadAttachmentData();
                            txtFileName.Text = "";
                            break;
                        default:
                            this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                            return;
                    }                    

                    ScriptManager.RegisterStartupScript(upAttachment, upAttachment.GetType(), "Report1", "alert('File has been uploaded!');", true);

                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
        }
       

        protected void dgAttachment_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidDocumentID");

                if (hidID != null)
                {
                    SalesmanUploadActivity apActivity = new SalesmanUploadActivity();


                    try
                    {
                        ResultType result = apActivity.DeleteSalesmanUpload(hidID.Value, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgAttachment.EditItemIndex = -1;
                                this.dgAttachment.CurrentPageIndex = 0;
                                //lblMsg.Text = "Record deleted successfully!<br /><br />";
                                LoadAttachmentData();
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
                            this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                            LoadAttachmentData();
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

        protected void dgAttachment_Command(Object sender, DataGridCommandEventArgs e)
        {
        }

        protected void dgAttachment_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
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
            LoadAttachmentData();
        }

       
        protected void dgAttachment_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
        }
        

    }
}
