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
using System.Data.SqlClient;
using System.IO;
using System.Text;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


using SharpPieces.Web.Controls;

namespace GMSWeb.HR.Staff
{
    public partial class StaffDetail : GMSBasePage
    {
        private string employeeNo;

        protected void Page_Init(object sender, EventArgs e)
        {
            dlData.DataBind(); 
        } 

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("HR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,44);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,44);

            if (uAccess == null && (uAccessForCompanyList == null || uAccessForCompanyList.Count <= 0))
                Response.Redirect(base.UnauthorizedPage("HR"));

            if (!Page.IsPostBack)
            {
                ViewState["EmployeeID"] = Request.Params["EmployeeID"].Trim();

                DataSet ds = new DataSet();
                new GMSGeneralDALC().CanUserAccessDocument(session.CompanyId, "EMPDETAIL", ViewState["EmployeeID"].ToString(), session.UserId, ref ds);

                if (!(Convert.ToBoolean(ds.Tables[0].Rows[0]["result"])))
                {
                    Response.Redirect(base.SessionTimeOutPage("HR"));
                    return;
                }
            }
            LoadData();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        } 

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            GMSCore.Entity.Employee employee = null;
            try
            {
                employee = new EmployeeActivity().RetrieveEmployeeByEmployeeID(GMSUtil.ToShort(ViewState["EmployeeID"]));
                employeeNo = employee.EmployeeNo; 
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            List<GMSCore.Entity.Employee> lstEmployee = new List<Employee>();
            lstEmployee.Add(employee);
            this.dlData.DataSource = lstEmployee;
            this.dlData.DataBind();
        }
        #endregion
                
        #region dlData_EditCommand
        protected void dlData_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            dlData.EditItemIndex = e.Item.ItemIndex;
            this.LoadData();
        }
        #endregion

        #region dgData_ItemDataBound
        protected void dlData_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                ExtendedDropDownList ddlCompany = (ExtendedDropDownList)e.Item.FindControl("ddlCompany");
                if (ddlCompany != null)
                {
                    IList<Country> countryList = (new SystemDataActivity()).RetrieveAllCountryListSortBySeqID();
                    foreach (Country country in countryList)
                    {
                        bool isNewGrp = true;
                        IList<Company> companyList = (new SystemDataActivity()).RetrieveCompanyByCountryId(country.CountryID);
                        foreach (Company company in companyList)
                        {
                            if (isNewGrp)
                                ddlCompany.ExtendedItems.Add(new ExtendedListItem(company.Name, company.CoyID.ToString(), true, ListItemGroupingType.New, country.Name));
                            else
                                ddlCompany.ExtendedItems.Add(new ExtendedListItem(company.Name, company.CoyID.ToString(), ListItemGroupingType.Inherit));
                            isNewGrp = false;
                        }
                    }
                    ddlCompany.SelectedValue = ((HtmlInputHidden)e.Item.FindControl("hidCoyID")).Value;
                }
            }

            LogSession session = base.GetSessionInfo();
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId, 99);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,99);

            if (uAccess == null && (uAccessForCompanyList == null || uAccessForCompanyList.Count <= 0))
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
                    if (lnkEdit != null)
                        lnkEdit.Enabled = false;                    
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                /*
                System.Web.UI.HtmlControls.HtmlGenericControl divPMP = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("divPMP");
                IList<DocumentForEmployee> lstDocument = new SystemDataActivity().RetrieveAllDocumentsByEmployeeID((GMSUtil.ToShort(ViewState["EmployeeID"])));
                foreach (DocumentForEmployee doc in lstDocument) 
                {
                    //divPMP.InnerHtml += "<asp:LinkButton runat=\"server\" Text=\"" + doc.Year + " " + doc.Type +
                      //                  "CommandName=\"Load\" ForeColor=\"#005DAA\" CommandArgument=\"" + doc.FileName + "\"></asp:LinkButton></br>";  
                    //divPMP.InnerHtml = "<a href=\"D:/GMSDocuments/PMP/0013/000014.pdf\">Click</a>"; 
                    //divPMP.InnerHtml = "<asp:LinkButton runat=\"server\" Text=\"something\"></a>"; 
                }*/
                System.Web.UI.WebControls.PlaceHolder plhPMP = (System.Web.UI.WebControls.PlaceHolder)e.Item.FindControl("plhPMP"); 
                IList<DocumentForEmployee> lstDocument = new SystemDataActivity().RetrieveAllDocumentsByEmployeeID((GMSUtil.ToShort(ViewState["EmployeeID"])));
                string type = ""; 
                foreach (DocumentForEmployee doc in lstDocument)
                {
                    LinkButton lb = new LinkButton();
                    lb = new LinkButton();
                    if (doc.Type.ToString() == "F") 
                        type = "Full Year"; 
                    else 
                        type = "Mid Year";
                    lb.Text = doc.Year.ToString() + " " + type; 
                    lb.ID = doc.DocumentID.ToString();
                    lb.CommandArgument = doc.FileName.ToString(); 
                    lb.CommandName = "Load";
                    //lb.Command += new CommandEventHandler(lb_Command);
                    lb.Command += lb_Command; 
                    plhPMP.Controls.Add(lb);
                    plhPMP.Controls.Add(new LiteralControl("<br />")); 
                }

                System.Web.UI.WebControls.PlaceHolder plhStaffDetail = (System.Web.UI.WebControls.PlaceHolder)e.Item.FindControl("plhStaffDetail");
                LinkButton lbStaffDetail = new LinkButton();
                lbStaffDetail.Text = "Staff Detail";
                lbStaffDetail.ID = this.employeeNo;
                lbStaffDetail.CommandArgument = this.employeeNo.ToString();
                lbStaffDetail.CommandName = "LoadStaffDetail";
                lbStaffDetail.Command += lb_Command;
                plhStaffDetail.Controls.Add(lbStaffDetail); 
            }
        }
        #endregion

        
        #region lb_Command
        protected void lb_Command(object sender, CommandEventArgs e)
        {
            LinkButton lnk = sender as LinkButton;
            if (e.CommandName == "Load")
            {
                string ext = Path.GetExtension(e.CommandArgument.ToString());
                string ContentType = "";

                if (ext == ".asf")
                    ContentType = "video/x-ms-asf";
                else if (ext == ".avi")
                    ContentType = "video/avi";
                else if (ext == ".doc")
                    ContentType = "application/msword";
                else if (ext == ".zip")
                    ContentType = "application/zip";
                else if (ext == ".xls")
                    ContentType = "application/vnd.ms-excel";
                else if (ext == ".gif")
                    ContentType = "image/gif";
                else if (ext == ".jpg" || ext == "jpeg")
                    ContentType = "image/jpeg";
                else if (ext == ".wav")
                    ContentType = "audio/wav";
                else if (ext == ".mp3")
                    ContentType = "audio/mpeg3";
                else if (ext == ".mpg" || ext == "mpeg")
                    ContentType = "video/mpeg";
                else if (ext == ".mp3")
                    ContentType = "audio/mpeg3";
                else if (ext == ".rtf")
                    ContentType = "application/rtf";
                else if (ext == ".htm" || ext == "html")
                    ContentType = "text/html";
                else if (ext == ".asp")
                    ContentType = "text/asp";
                else
                    ContentType = "application/octet-stream";

                Response.ContentType = ContentType.ToString();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + e.CommandArgument.ToString());
                try
                {
                    Response.TransmitFile(@"D:/GMSDocuments/PMP/" + this.employeeNo + "/" + e.CommandArgument.ToString());
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
                Response.End();
            }
            else if (e.CommandName == "LoadStaffDetail")
            {
                string selectedReport = "StaffInfo";                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Report1",
                    string.Format("jsOpenOperationalReport('/GMS3/Finance/BankFacilities/PdfReportViewer.aspx?REPORT={0}&&TRNNO=" + this.employeeNo + "&&REPORTID=-5');",
                                        selectedReport)
                                        , true);
                LoadData();

            }
        }
        #endregion

        #region dlData_CancelCommand
        protected void dlData_CancelCommand(object sender, DataListCommandEventArgs e)
        {
            this.dlData.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dlData_UpdateCommand
        protected void dlData_UpdateCommand(object sender, DataListCommandEventArgs e)
        {
            TextBox txtEditEmployeeNo = (TextBox)e.Item.FindControl("txtEditEmployeeNo");
            TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");
            TextBox txtEditDepartment = (TextBox)e.Item.FindControl("txtEditDepartment");
            TextBox txtEditDesignation = (TextBox)e.Item.FindControl("txtEditDesignation");
            TextBox txtEditGrade = (TextBox)e.Item.FindControl("txtEditGrade");
            TextBox editDateJoined = (TextBox)e.Item.FindControl("editDateJoined");
            TextBox txtEditQualification = (TextBox)e.Item.FindControl("txtEditQualification");
            TextBox editDOB = (TextBox)e.Item.FindControl("editDOB");
            TextBox txtEditNRIC = (TextBox)e.Item.FindControl("txtEditNRIC");
            TextBox txtEditEmail = (TextBox)e.Item.FindControl("txtEditEmail");
            TextBox txtCarPlate = (TextBox)e.Item.FindControl("txtCarPlate");
            TextBox txtEditSupervisorName = (TextBox)e.Item.FindControl("txtEditSupervisorName");
            TextBox txtDateResigned = (TextBox)e.Item.FindControl("txtDateResigned");
            RadioButton rbEditIsUnitHead = (RadioButton)e.Item.FindControl("rbEditIsUnitHead");
            RadioButton rdEditActive = (RadioButton)e.Item.FindControl("rdEditActive");
            ExtendedDropDownList ddlCompany = (ExtendedDropDownList)e.Item.FindControl("ddlCompany");
            HtmlInputHidden hidEmployeeID = (HtmlInputHidden)e.Item.FindControl("hidEmployeeID");
            Label lblMsg = (Label)e.Item.FindControl("lblMsg");

            if (hidEmployeeID != null && editDateJoined != null && editDOB != null && txtEditName != null && txtEditDesignation != null
                && txtEditQualification != null && txtEditNRIC != null && txtEditGrade != null && txtEditDepartment != null && rdEditActive != null && ddlCompany != null
                && txtEditEmail != null && txtEditSupervisorName != null && rbEditIsUnitHead != null && txtDateResigned != null && txtEditEmployeeNo != null)
            {
                LogSession session = base.GetSessionInfo();
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script2", "<script type=\"text/javascript\"> edited = true;</script>", false);

                Employee employee = new EmployeeActivity().RetrieveEmployeeByEmployeeID(GMSUtil.ToShort(hidEmployeeID.Value));
                if (employee.EmployeeNo != txtEditEmployeeNo.Text.Trim())
                {
                    Employee employee2 = new SystemDataActivity().RetrieveEmployeeListByEmployeeNoSortByEmployeeName(txtEditEmployeeNo.Text.Trim());
                    if (employee2 != null)
                    {
                        this.PageMsgPanel.ShowMessage("The Employee NO. is already existed in database!", MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
                if (txtEditSupervisorName.Text.Trim() != "")
                {
                    Employee supervisor = new EmployeeActivity().RetrieveEmployeeByEmployeeName(txtEditSupervisorName.Text.Trim());
                    if (supervisor != null)
                    {
                        employee.SuperiorID = supervisor.EmployeeID;
                    }
                    else
                    {
                        this.PageMsgPanel.ShowMessage("The supervisor name is not found in database!", MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
                employee.CoyID = short.Parse(ddlCompany.SelectedValue);
                employee.EmployeeNo = txtEditEmployeeNo.Text.Trim();
                employee.Name = txtEditName.Text.Trim();
                employee.Department = txtEditDepartment.Text.Trim();
                employee.Designation = txtEditDesignation.Text.Trim();
                employee.DateJoined = GMSUtil.ToDate(editDateJoined.Text);
                employee.Qualification = txtEditQualification.Text.Trim();
                employee.DOB = GMSUtil.ToDate(editDOB.Text);
                employee.NRIC = txtEditNRIC.Text.Trim();
                employee.Grade = txtEditGrade.Text.Trim();
                employee.EmailAddress = txtEditEmail.Text.Trim();
                employee.CarPlate = txtCarPlate.Text.Trim();
                employee.IsUnitHead = false;
                if (rbEditIsUnitHead.Checked)
                    employee.IsUnitHead = true;
                if (rdEditActive.Checked == true)
                {
                    employee.IsInactive = false;
                    employee.DateResigned = null;
                }
                else
                {
                    employee.IsInactive = true;
                    employee.DateResigned = GMSUtil.ToDate(txtDateResigned.Text.Trim());
                }
                employee.ModifiedBy = session.UserId;
                employee.ModifiedDate = DateTime.Now;

                FileUpload FileUpload1 = (FileUpload)e.Item.FindControl("FileUpload1");
                HtmlInputHidden employeeID = (HtmlInputHidden)e.Item.FindControl("hidEmployeeID");
                if (FileUpload1.HasFile && employeeID != null && !string.IsNullOrEmpty(employeeID.Value))
                {
                    try
                    {
                        string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Photo";
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        FileUpload1.SaveAs(folderPath + "\\" + employeeID.Value + ".JPG");
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = "Error: " + ex.Message.ToString();
                    }
                }
                FileUpload FileUpload2 = (FileUpload)e.Item.FindControl("FileUpload2");
                if (FileUpload2.HasFile && employeeID != null && !string.IsNullOrEmpty(employeeID.Value))
                {
                    try
                    {
                        string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\KPI";
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        FileUpload2.SaveAs(folderPath + "\\" + employeeID.Value + ".doc");
                        employee.KPIUploadedBy = session.UserId;
                        employee.KPIUploadedDate = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = "Error: " + ex.Message.ToString();
                    }
                }
                FileUpload FileUpload3 = (FileUpload)e.Item.FindControl("FileUpload3");
                if (FileUpload3.HasFile && employeeID != null && !string.IsNullOrEmpty(employeeID.Value))
                {
                    try
                    {
                        string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\JobDescription";
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        FileUpload3.SaveAs(folderPath + "\\" + employeeID.Value + ".doc");
                        employee.JDUploadedBy = session.UserId;
                        employee.JDUploadedDate = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = "Error: " + ex.Message.ToString();
                    }
                }

                try
                {
                    if (employee.IsInactive && hidConfirmSendEmail.Value == "True")
                    {
                        employee.Notification = true;
                    }

                    ResultType result = new EmployeeActivity().UpdateEmployee(ref employee, session);

                    switch (result)
                    {
                        case ResultType.Ok:
                            //this.dlData.EditItemIndex = -1;
                            //if (employee.IsInactive && hidConfirmSendEmail.Value == "True")
                            //{
                            //    SendEmail(employee.EmployeeID);
                            //}
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script1", "<script type=\"text/javascript\"> alert('This employee has been updated successfully!');</script>", false);
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

        #region dlData_DeleteCommand
        protected void dlData_DeleteCommand(object sender, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                HtmlInputHidden hidEmployeeID = (HtmlInputHidden)e.Item.FindControl("hidEmployeeID2");

                if (hidEmployeeID != null)
                {
                    LogSession session = base.GetSessionInfo();

                    EmployeeActivity eActivity = new EmployeeActivity();

                    try
                    {
                        ResultType result = eActivity.DeleteEmployee(GMSUtil.ToShort(hidEmployeeID.Value), session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dlData.EditItemIndex = -1;
                                StringBuilder str = new StringBuilder();
                                str.Append("<script language='javascript'>");
                                str.Append("alert('This employee has been deleted successfully!');window.close();var sData = dialogArguments; sData.location.reload();");
                                str.Append("</script>");
                                Response.Write(str.ToString());
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
                            this.PageMsgPanel.ShowMessage("This employee record cannot be deleted because it has been referenced by history records.", MessagePanelControl.MessageEnumType.Alert);
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

        #region SendEmail
        private void SendEmail(short employeeId)
        {
            LogSession session = base.GetSessionInfo();
            Employee employee = new EmployeeActivity().RetrieveEmployeeByEmployeeID(employeeId);

            if (employee != null)
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.From = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
                IList<NotificationParty> nList = (new SystemDataActivity()).RetrieveNotificationPartyListByPurpose("R");
                if (nList != null && nList.Count > 0)
                {
                    foreach (NotificationParty np in nList)
                    {
                        mail.To.Add(new System.Net.Mail.MailAddress(np.EmployeeObject.EmailAddress, np.EmployeeObject.Name));
                    }
                }
                else
                {
                    base.JScriptAlertMsg("Notification Party Can Not Be Found!");
                    return;
                }
                string smtpServer = "smtp.leedenlimited.com";
                mail.ReplyTo = new System.Net.Mail.MailAddress("keith.wong@leedenlimited.com", "Tong Rui, Ray");
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.Subject = "[GMS - Staff Resigned]";
                mail.IsBodyHtml = true;
                mail.Body = "<p>Dear all,</p>\n" +
                            "<p>The following staff has resigned from our company:</p>\n" +
                            "<p>Name: " + employee.Name + "<br />\n" +
                            "Designation: " + employee.Designation + "<br />\n" +
                            "Department: " + employee.Department + "<br />\n" +
                            "Company: " + employee.CompanyObject.Name + "<br />\n" +
                            "Date Resigned: " + employee.DateResigned.Value.ToString("dd/MM/yyyy") + "</p>\n" +
                            "<br />" +
                            "***** This is a computer-generated email. Please do not reply.*****";

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
        }
        #endregion
    }
}
