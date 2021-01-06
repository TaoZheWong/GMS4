using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GMSWeb.SysHR.Staff
{
    public partial class PhotoIDUpdate : GMSBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadDDL();
                ViewState["CurrentPageIndex"] = 0;
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

            string javaScript =
            @"
            <script language=""javascript"" type=""text/javascript"">
                 function changeEmployeeNo(txt) {
                var ddl = document.getElementById(""ddlEmployee"");
                var str = txt.value.split(' - ');
                txt.value = str[0].replace(/^\s +|\s +$/, '').toUpperCase();
                var found = false;
                for (var i = 0; i < ddl.options.length; i++)
                {
                    if (ddl.options[i].text.toUpperCase() == txt.value.toUpperCase())
                    {
                        ddl.options[i].selected = true;
                        found = true;
                        alert('true');
                    }
                }
                if (found == false)
                {
                    alert('Please key in the correct employee no!');
                    ddl.options[0].selected = true;
                }
            }

            </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (this.txtEmployeeNo.Text != "") {
                Employee employee = null;
                try
                {
                    employee = new EmployeeActivity().RetrieveEmployeeByEmployeeNo(this.txtEmployeeNo.Text.Trim(),session.CompanyId.ToString());
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                }

                if (employee!=null)
                {
                    List<Employee> lstEmployee = new List<Employee>();
                    lstEmployee.Add(employee);
                    this.dlData.DataSource = lstEmployee;
                    this.dlData.DataBind();
                    resultList.Visible = true;
                }
                else
                {
                    this.PageMsgPanel.ShowMessage("Employee Not Found.", MessagePanelControl.MessageEnumType.Alert);
                }
            }else
            {
                this.PageMsgPanel.ShowMessage("Please input Employee No.", MessagePanelControl.MessageEnumType.Alert);
            }

        }
        #endregion
        
        protected void dlData_OnItemCommand(object sender, DataListCommandEventArgs e)
        {
            HtmlInputHidden hidEmployeeID = (HtmlInputHidden)e.Item.FindControl("hidEmployeeID");

            FileUpload FileUpload = (FileUpload)e.Item.FindControl("FileUpload1");
            if (FileUpload.HasFile)
            {
                if ((Path.GetExtension(FileUpload.FileName)).ToUpper() != ".JPG" && (Path.GetExtension(FileUpload.FileName)).ToUpper() != ".PNG"
                    && (Path.GetExtension(FileUpload.FileName)).ToUpper() != ".JPEG"&& (Path.GetExtension(FileUpload.FileName)).ToUpper() != ".GIF" 
                    && (Path.GetExtension(FileUpload.FileName)).ToUpper() != ".TIFF" && (Path.GetExtension(FileUpload.FileName)).ToUpper() != ".BMP"
                    && (Path.GetExtension(FileUpload.FileName)).ToUpper() != ".SVG" && (Path.GetExtension(FileUpload.FileName)).ToUpper() != ".JFIF")
                {
                    this.PageMsgPanel.ShowMessage("File format is not supported.", MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
                try
                {
                    string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Photo";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    FileUpload.SaveAs(folderPath + "\\" + hidEmployeeID.Value + ".JPG");
                    this.PageMsgPanel.ShowMessage("Photo Updated.", MessagePanelControl.MessageEnumType.Notice);
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
            else
            {
                this.PageMsgPanel.ShowMessage("Please select photo to upload.", MessagePanelControl.MessageEnumType.Alert);
                return;
            }
        }

        private void LoadDDL()
        {
            if (ddlEmployee != null)
            {
                SystemDataActivity sDataActivity = new SystemDataActivity();
                // fill in employee dropdown list
                IList<GMSCore.Entity.Employee> lstEmployee = null;
                lstEmployee = sDataActivity.RetrieveAllEmployeeListSortByEmployeeNo();
                ddlEmployee.DataSource = lstEmployee;
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("0", "0"));
                ddlEmployee.SelectedIndex = 0;
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlNewEmployee, ddlNewEmployee.GetType(), "script1", "<script type=\"text/javascript\"> var ddlNewEmployeeID = '" + ddlNewEmployee.ClientID + "';</script>", false);
            }
        }
    }
}