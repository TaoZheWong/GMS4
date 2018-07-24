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
using System.Text;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using SharpPieces.Web.Controls;

namespace GMSWeb.SysHR.Staff
{
    public partial class AddNewStaff : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["TYPE"] = Request.Params["TYPE"];
            }
            if (ViewState["TYPE"] == null || ViewState["TYPE"].ToString() == "")
                ViewState["TYPE"] = "CompanyHR";

            Master.setCurrentLink(ViewState["TYPE"].ToString());

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(ViewState["TYPE"].ToString()));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            45);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            45);

            if (uAccess == null && (uAccessForCompanyList == null || uAccessForCompanyList.Count <= 0))
                Response.Redirect(base.UnauthorizedPage(ViewState["TYPE"].ToString()));

            // Rename();

            if (!IsPostBack)
            {
                if (ViewState["TYPE"].ToString() == "CompanyHR")
                {
                    trCompany.Visible = false;
                    trCompany2.Visible = false;
                }
                else
                {
                    trCompany.Visible = true;
                    trCompany2.Visible = true;
                    PopulateDDL();
                }
            }

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>
            <script language=""javascript"" type=""text/javascript"" >
            function ConfirmSendEmail()
            {
                var n = confirm(""Send email to the respective parties for new staff information?"");
                if (n)
                    document.getElementById("""; javaScript += hidConfirmSendEmail.ClientID; javaScript += @""").value = ""True"";
                else
                    document.getElementById("""; javaScript += hidConfirmSendEmail.ClientID; javaScript += @""").value = ""False"";
            }
            </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            this.sriptmgr1.RegisterPostBackControl(btnUpload);
            this.sriptmgr1.RegisterPostBackControl(btnAdd);
        }

        private void PopulateDDL()
        {
            LogSession session = base.GetSessionInfo();
            this.ddlCompany.ExtendedItems.Add(new ExtendedListItem("Choose a company...", "0"));
            //IList<Country> countryList = (new SystemDataActivity()).RetrieveAllCountryListSortBySeqID();
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            dacl.GetCountryListSelectUserNumID(session.UserId, ref ds);
            foreach (DataRow country in ds.Tables[0].Rows)
            {
                bool isNewGrp = true;
                //IList<Company> companyList = (new SystemDataActivity()).RetrieveCompanyByCountryId(GMSUtil.ToShort(country["CountryID"].ToString()));
                DataSet companyList = new DataSet();
                dacl.GetCompanyListSelectUserNumID(session.UserId, GMSUtil.ToShort(country["CountryID"].ToString()), ref companyList);
                foreach (DataRow company in companyList.Tables[0].Rows)
                {
                    if (isNewGrp)
                        this.ddlCompany.ExtendedItems.Add(new ExtendedListItem(company["Name"].ToString(), company["CoyID"].ToString(), true, ListItemGroupingType.New, country["Name"].ToString()));
                    else
                        this.ddlCompany.ExtendedItems.Add(new ExtendedListItem(company["Name"].ToString(), company["CoyID"].ToString(), ListItemGroupingType.Inherit));
                    isNewGrp = false;
                }
            }

            this.ddlNewCompany.ExtendedItems.Add(new ExtendedListItem("Choose a company...", "0"));
            foreach (DataRow country in ds.Tables[0].Rows)
            {
                bool isNewGrp = true;
                //IList<Company> companyList = (new SystemDataActivity()).RetrieveCompanyByCountryId(GMSUtil.ToShort(country["CountryID"].ToString()));
                DataSet companyList = new DataSet();
                dacl.GetCompanyListSelectUserNumID(session.UserId, GMSUtil.ToShort(country["CountryID"].ToString()), ref companyList);
                foreach (DataRow company in companyList.Tables[0].Rows)
                {
                    if (isNewGrp)
                        this.ddlNewCompany.ExtendedItems.Add(new ExtendedListItem(company["Name"].ToString(), company["CoyID"].ToString(), true, ListItemGroupingType.New, country["Name"].ToString()));
                    else
                        this.ddlNewCompany.ExtendedItems.Add(new ExtendedListItem(company["Name"].ToString(), company["CoyID"].ToString(), ListItemGroupingType.Inherit));
                    isNewGrp = false;
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string coyID = "";
            string type = "";
            if (ViewState["TYPE"].ToString() == "CompanyHR")
            {
                coyID = session.CompanyId.ToString();
                type = ddlType.SelectedValue;
            }
            else
            {
                if (short.Parse(ddlCompany.SelectedValue) <= 0)
                {
                    base.JScriptAlertMsg("Please select a company!");
                    return;
                }
                coyID = ddlCompany.SelectedValue;
                type = ddlType.SelectedValue;

            }

            if (FileUpload1.HasFile)
            {
                FileUpload1.SaveAs(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + "\\" + FileUpload1.FileName);

                this.IFrame1.Attributes["style"] = "";
                this.IFrame1.Attributes["src"] = String.Format("ParsingStaff.aspx?FILENAME={0}&COYID={1}&TYPE={2}",
                                                            Server.UrlEncode(FileUpload1.FileName), coyID, type);
            }
            else
            {
                lblMsg.Text = "You have not specified a file.";
            }
        }

        #region btnAdd_Click
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            string coyID = "";
            if (ViewState["TYPE"].ToString() == "CompanyHR")
            {
                coyID = session.CompanyId.ToString();
            }
            else
            {
                if (short.Parse(ddlNewCompany.SelectedValue) <= 0)
                {
                    base.JScriptAlertMsg("Please select a company!");
                    return;
                }
                coyID = ddlNewCompany.SelectedValue;
            }

            Employee employee = new SystemDataActivity().RetrieveEmployeeListByEmployeeNoSortByEmployeeName(txtNewEmployeeNo.Text.Trim());
            if (employee != null)
            {
                base.JScriptAlertMsg("The Employee NO. is already existed in database!");
                return;
            }

            employee = new Employee();
            if (txtNewSupervisorName.Text.Trim() != "")
            {
                Employee supervisor = new EmployeeActivity().RetrieveEmployeeByEmployeeName(txtNewSupervisorName.Text.Trim());
                if (supervisor != null)
                {
                    employee.SuperiorID = supervisor.EmployeeID;
                }
                else
                {
                    base.JScriptAlertMsg("The supervisor name is not found in database!");
                    return;
                }
            }
            employee.CoyID = short.Parse(coyID);
            employee.EmployeeNo = txtNewEmployeeNo.Text.Trim();
            employee.Name = txtNewName.Text.Trim();
            employee.Department = txtNewDepartment.Text.Trim();
            employee.Designation = txtNewDesignation.Text.Trim();
            employee.DateJoined = GMSUtil.ToDate(txtNewDateJoined.Text);
            employee.Qualification = txtNewQualification.Text.Trim();
            employee.DOB = GMSUtil.ToDate(txtNewDOB.Text);
            employee.NRIC = txtNewNRIC.Text.Trim();
            employee.Grade = txtNewGrade.Text.Trim();
            employee.EmailAddress = txtNewEmail.Text.Trim();
            employee.CarPlate = txtCarPlate.Text.Trim();
            employee.IsUnitHead = false;
            if (rbIsUnitHead.Checked)
                employee.IsUnitHead = true;
            employee.IsInactive = true;
            if (rbIsActive.Checked == true)
                employee.IsInactive = false;
            employee.CreatedBy = session.UserId;
            employee.CreatedDate = DateTime.Now;
            if (hidConfirmSendEmail.Value == "True")
            {
                employee.Notification = true;
            }
            //GMSCore.Entity.DocumentNumber documentNumber = GMSCore.Entity.DocumentNumber.RetrieveByKey(1, (short)DateTime.Now.Year);
            //employee.EmployeeID = documentNumber.EmployeeID;
            //documentNumber.EmployeeID++;

            try
            {
                ResultType result = new EmployeeActivity().CreateEmployee(ref employee, session);
                employee.Resync();

                switch (result)
                {
                    case ResultType.Ok:
                        //documentNumber.Save();
                        if (FileUpload2.HasFile && employee.EmployeeID != 0)
                        {
                            try
                            {
                                string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Photo";
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                }
                                FileUpload2.SaveAs(folderPath + "\\" + employee.EmployeeID.ToString() + ".JPG");
                            }
                            catch (Exception ex)
                            {
                                lblMsg.Text = "Error: " + ex.Message.ToString();
                            }
                        }
                        //if (hidConfirmSendEmail.Value == "True")
                        //    SendEmail(employee.EmployeeID);
                        StringBuilder str = new StringBuilder();
                        str.Append("<script language='javascript'>");
                        str.Append("alert('This employee has been added successfully!');window.location.href = \"../../SysHR/Staff/AddNewStaff.aspx\";");
                        str.Append("</script>");
                        Response.Write(str.ToString());
                        break;
                    default:
                        this.MsgPanel1.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                        return;
                }
            }
            catch (Exception ex)
            {
                this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                return;
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

                IList<NotificationParty> nListForCoy = (new SystemDataActivity()).RetrieveNotificationPartyListByPurposeByCoyID("J",session.CompanyId);
                if (nListForCoy != null && nListForCoy.Count > 0)
                {
                    foreach (NotificationParty np in nListForCoy)
                    {
                        mail.To.Add(new System.Net.Mail.MailAddress(np.EmployeeObject.EmailAddress, np.EmployeeObject.Name));
                    }
                }
                else
                {
                    IList<NotificationParty> nList = (new SystemDataActivity()).RetrieveNotificationPartyListByPurpose("J");
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
                }
                string smtpServer = "smtp.leedenlimited.com";
                mail.ReplyTo = new System.Net.Mail.MailAddress("keith.wong@leedenlimited.com", "Tong Rui, Ray");
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.Subject = "[GMS - New Staff]";
                mail.IsBodyHtml = true;
                mail.Body = "<p>Dear all,</p>\n" +
                            "<p>The following is our company's new staff:</p>\n" +
                            "<p>Name: " + employee.Name + "<br />\n" +
                            "Designation: " + employee.Designation + "<br />\n" +
                            "Department: " + employee.Department + "<br />\n" +
                            "Company: " + employee.CompanyObject.Name + "<br />\n" +
                            "Date Joined: " + employee.DateJoined.ToString("dd/MM/yyyy") + "<br />\n" +
                            "Car Plate: " + employee.CarPlate + "</p>\n" +
                            "<br />" +
                            "***** This is a computer-generated email. Please do not reply.*****";
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Photo" + "\\" + employee.EmployeeID.ToString() + ".JPG";
                if (File.Exists(filePath))
                    mail.Attachments.Add(new System.Net.Mail.Attachment(filePath));

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

        //Rename photo from name to id
        //protected void Rename()
        //{
        //    foreach (string sFile in
        //        System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Photo\\"))
        //    {
        //        string fileName = new System.IO.FileInfo(sFile).Name;
        //        fileName = fileName.Substring(0, fileName.Length - 4);
        //        Response.Output.Write("Changing photo name: '" + fileName + "' ...<br>");
        //        Response.Flush();
        //        IList<Employee> employeeLst = new SystemDataActivity().RetrieveEmployeeListByEmployeeNameSortByEmployeeName(fileName);
        //        if (employeeLst != null && employeeLst.Count > 0)
        //        {
        //            Employee employee = employeeLst[0];
        //            File.Move(AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Photo\\" + fileName + ".JPG", AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Photo\\" + employee.EmployeeID.ToString() + ".JPG");
        //            Response.Output.Write("Changined photo name to id: '" + employee.EmployeeID.ToString() + ".JPG" + "' ...<br>");
        //            Response.Flush();
        //        }
        //    }
        //}
    }
}
