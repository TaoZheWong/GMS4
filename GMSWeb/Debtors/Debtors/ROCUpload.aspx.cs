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

namespace GMSWeb.Debtors.Debtors
{
    public partial class ROCUpload : GMSBasePage
    {
        #region Page_Load
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
                                                                            119);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                //preload
                ViewState["SortField"] = "CompanyName";
                ViewState["SortDirection"] = "ASC";
            }

            string javaScript =
@"
<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>
<script type=""text/javascript"">
function checkAll(checkItem, checkVal)
		{
				var frm = document.aspnetForm;
				for(i = 1; i < frm.length; i++) 
				{
					var elm = frm.elements[i];
					if( elm.type == 'checkbox' && elm.name.indexOf(checkItem) != -1 && elm.disabled != true )
					{
						elm.checked = checkVal;
					}
				}
		} 
		function DeselectMainCheckbox(checkbox)
		{
				document.getElementById('"; javaScript += dgData.ClientID; javaScript += @"').rows[0].cells[12].childNodes[0].checked = false;
				
		}
		

</script>
";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);

            


        }
        #endregion

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            int checkedSize = 0;
            for (int i = 0; i < this.dgData.Items.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)this.dgData.Items[i].FindControl("chkUpload");
                if (chkSelect != null && chkSelect.Checked)
                {
                    checkedSize = checkedSize + 1;
                }
            }

            if (checkedSize <= 0)
            {
                JScriptAlertMsg("Please select at least one company to upload!");
                return;
            }

            LogSession session = base.GetSessionInfo();
            string fileName = "";
            string doc_num = "";

            FileUpload FileUpload1 = (FileUpload)pnlUpload.FindControl("FileUpload1");
            if (FileUpload1.HasFile)
            {
                try
                {
                    string folderPath = "C://GMS/CRM";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    for (int i = 0; i < this.dgData.Items.Count; i++)
                    {
                        CheckBox chkSelect = (CheckBox)this.dgData.Items[i].FindControl("chkUpload");
                        HtmlInputHidden hidCoyID = (HtmlInputHidden)this.dgData.Items[i].FindControl("hidCoyID");
                        HtmlInputHidden hidCustomerCode = (HtmlInputHidden)this.dgData.Items[i].FindControl("hidCustomerCode");
                        if (chkSelect != null && hidCoyID != null && hidCustomerCode != null)
                        {
                            if (chkSelect.Checked)
                            {
                                short companyId = GMSUtil.ToShort(hidCoyID.Value);
                                string customerCode = hidCustomerCode.Value;

                                if (fileName == "")
                                {
                                    DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(companyId, (short)DateTime.Now.Year);
                                    if (documentNumber == null) //If tbDocumentNumber does not exist
                                    {
                                        documentNumber = new DocumentNumber();
                                        documentNumber.CoyID = companyId;
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
                                    doc_num = documentNumber.AttachmentNo;
                                    fileName = (DateTime.Now.Year).ToString() + "-" + companyId + "-" + doc_num + Path.GetExtension(this.FileUpload1.FileName);
                                    
                                    FileUpload1.SaveAs(folderPath + "/" + fileName);

                                    string nxtStr = ((short)(short.Parse(documentNumber.AttachmentNo) + 1)).ToString();
                                    for (int j = nxtStr.Length; j < documentNumber.AttachmentNo.Length; j++)
                                    {
                                        nxtStr = "0" + nxtStr;
                                    }
                                    documentNumber.AttachmentNo = nxtStr;
                                    documentNumber.Save();
                                }


                                
                                GMSCore.Entity.AccountAttachment accountAttachment = new GMSCore.Entity.AccountAttachment();

                                accountAttachment.CoyID = companyId;
                                accountAttachment.AccountCode = customerCode.Trim();
                                accountAttachment.DocumentID = doc_num;
                                accountAttachment.DocumentName = txtFileName.Text.Trim();
                                accountAttachment.FileName = fileName;
                                accountAttachment.IsActive = true;
                                accountAttachment.CreatedBy = session.UserId;
                                accountAttachment.CreatedDate = DateTime.Now;

                                ResultType result = new AccountAttachmentActivity().CreateAccountAttachment(ref accountAttachment, session);

                                switch (result)
                                {
                                    case ResultType.Ok:
                                        //LoadAttachmentData();
                                        //txtFileName.Text = "";
                                        break;
                                    default:
                                        this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                        return;
                                }

                                

                            }
                        }

                    }
                    txtFileName.Text = "";
                    txtAccountName.Text = "";
                    LoadData();
                    //ScriptManager.RegisterStartupScript(upOutter, upOutter.GetType(), "Report1", "alert('File has been uploaded!');", true);
                    JScriptAlertMsg("File has been uploaded!");
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }

            
        }

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            //if (this.txtAccountCode.Text.Trim() == "" && this.txtAccountName.Text.Trim() == "")
            //{
            //    base.JScriptAlertMsg("Please input a customer to search.");
            //    return;
            //}

           
            string accountName = "%" + txtAccountName.Text.Trim() + "%";

            DebtorCommentaryDALC dacl = new DebtorCommentaryDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetCustomerWithoutFilterByCompany(accountName, session.UserId, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (endIndex < ds.Tables[0].Rows.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();

                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = dv;
                this.dgData.DataBind();
                this.dgData.Visible = true;
                this.pnlUpload.Visible = true;
            }
            else
            {
                this.pnlUpload.Visible = false;
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = null;
                this.dgData.DataBind();
            }


        }
        #endregion

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region SortData
        protected void SortData(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
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
                ViewState["SortField"] = e.SortExpression;
                ViewState["SortDirection"] = "ASC";
            }
            LoadData();
        }
        #endregion

        

        
    }
}
