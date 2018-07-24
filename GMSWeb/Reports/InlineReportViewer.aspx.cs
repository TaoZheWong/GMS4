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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GMSCore.Activity;

namespace GMSWeb.Reports
{
    public partial class InlineReportViewer : GMSBasePage
    {
        protected ReportDocument crReportDocument;
        private short reportId = 0;
        string fileName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
            //                                                                49);
            //if (uAccess == null)
            //    Response.Redirect("../../Unauthorized.htm");


            

            this.reportId = GMSUtil.ToShort(Request.QueryString["REPORTID"]);

            if (reportId != -1 && reportId != -2 && reportId != -3)
            {
                fileName = new ReportsActivity().RetrieveReportById(this.reportId).FileName;
            }
            else
            {
                fileName = Request.QueryString["REPORT"].Trim() + ".rpt";
                if (fileName == "Pricelist.rpt")
                    cyReportViewer.HasToggleGroupTreeButton = true;
            }

            try
            {
                crReportDocument = new ReportDocument();
                crReportDocument.Load(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.DOC_PATH + "\\" + fileName);

            }
            catch (Exception ex)
            {
                this.lblFeedback.Text = ex.Message;
            }

            

            if (IsControlAdded)
                AddControls();

            if (!IsPostBack)
                AddControls();  

            
            
        }

        public bool IsControlAdded
        {
            get
            {
                if (ViewState["IsControlAdded"] == null)
                    ViewState["IsControlAdded"] = false;
                return (bool)ViewState["IsControlAdded"];
            }
            set { ViewState["IsControlAdded"] = value; }
        }


        
        private void AddControls()
        {
            LogSession session = base.GetSessionInfo();

            int controlCount = 0;

            pnlParameter.Controls.Add(new LiteralControl("<table class=\"tTable1\" style=\"margin-left: 8px; border: 1px; width:702px\" cellspacing=\"5\" cellpadding=\"5\" border=\"0\">"));
            

            if (crReportDocument.ParameterFields["@Year"] != null || crReportDocument.ParameterFields["Year"] != null)
            {
                
                if ((controlCount % 3) == 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                }
                // add new control
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Year :"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                DropDownList ddlYear = new DropDownList();               
                ddlYear.ID = "ddlYear";
                //ddlYear.AutoPostBack = true;                
                //ddlYear.SelectedIndexChanged += DropDownListYear_SelectedIndexChanged; 
                ddlYear.Items.Clear();
                 
                for (int i = 2011; i <= 2020; i++)
                {
                    ddlYear.Items.Add(i.ToString());
                }

                ddlYear.SelectedValue = DateTime.Now.Year.ToString();

                pnlParameter.Controls.Add(ddlYear);



                pnlParameter.Controls.Add(new LiteralControl("</td>"));

                if ((controlCount % 3) == 2)
                {
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));

                }
                controlCount = controlCount + 1;
                

                // end of creating new control



            }
            if (crReportDocument.ParameterFields["@Month"] != null || crReportDocument.ParameterFields["Month"] != null)
            {
                if ((controlCount % 3) == 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                }

               
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Month :"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                DropDownList ddlMonth = new DropDownList();
                ddlMonth.ID = "ddlMonth";
                //ddlMonth.AutoPostBack = true;
                //ddlMonth.SelectedIndexChanged += DropDownListMonth_SelectedIndexChanged;
                ddlMonth.Items.Clear();
                for (int i = 1; i <= 12; i++)
                {
                    ddlMonth.Items.Add(i.ToString());
                }

               

                pnlParameter.Controls.Add(ddlMonth);
                pnlParameter.Controls.Add(new LiteralControl("</td>"));



                if ((controlCount % 3) == 2)
                {
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));

                }


                controlCount = controlCount + 1;



            }
            if (crReportDocument.ParameterFields["@SelectedCurrency"] != null)
            {
                if ((controlCount % 3) == 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                }
                
                
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Currency :"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                DropDownList ddlCurrency = new DropDownList();
                ddlCurrency.ID = "ddlCurrency";
                //ddlCurrency.AutoPostBack = true;
                //ddlCurrency.SelectedIndexChanged += DropDownListCurrency_SelectedIndexChanged;
                ddlCurrency.Items.Clear();
                ddlCurrency.Items.Add(new ListItem("DEFAULT", "1"));
                ddlCurrency.Items.Add(new ListItem("SGD", "2"));

              

                pnlParameter.Controls.Add(ddlCurrency);
                pnlParameter.Controls.Add(new LiteralControl("</td>"));


                if ((controlCount % 3) == 2)
                {
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));

                }
                controlCount = controlCount + 1;



            }
            if (crReportDocument.ParameterFields["@ProjectID"] != null && session.CompanyId == 28
                && !fileName.Contains("Department"))
            {

                if ((controlCount % 3) == 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                }

                
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Project ID :"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                DropDownList ddlProjectID = new DropDownList();
                ddlProjectID.ID = "ddlProjectID";
                //ddlProjectID.AutoPostBack = true;
                //ddlProjectID.SelectedIndexChanged += DropDownListProjectID_SelectedIndexChanged;


                ddlProjectID.Items.Clear();
                ddlProjectID.Items.Add(new ListItem("COMPANY", "-1"));
                ddlProjectID.Items.Add(new ListItem("WELDING SALES", "1"));
                ddlProjectID.Items.Add(new ListItem("WELDING CONSUMABLES", "2"));
                ddlProjectID.Items.Add(new ListItem("WELDING EQUIPMENT", "3"));
                ddlProjectID.Items.Add(new ListItem("WELDING ACCESSORIES", "4"));

                

                pnlParameter.Controls.Add(ddlProjectID);
                pnlParameter.Controls.Add(new LiteralControl("</td>"));



                if ((controlCount % 3) == 2)
                {
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));

                }
                controlCount = controlCount + 1;

            }
            else if (crReportDocument.ParameterFields["@ProjectID"] != null && session.CompanyId == 28
                && fileName.Contains("Department"))
            {


                

                if ((controlCount % 3) == 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                }



                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Project ID :"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                DropDownList ddlProjectID = new DropDownList();

                ddlProjectID.ID = "ddlProjectID";
                //ddlProjectID.AutoPostBack = true;
                //ddlProjectID.SelectedIndexChanged += DropDownListProjectID_SelectedIndexChanged;
                ddlProjectID.Items.Clear();
                ddlProjectID.Items.Add(new ListItem("CUSTOMER SERVICE", "5"));
                ddlProjectID.Items.Add(new ListItem("PURCHASING", "6"));
                ddlProjectID.Items.Add(new ListItem("LOGISTICS & WAREHOUSING", "7"));
                ddlProjectID.Items.Add(new ListItem("WORKSHOP", "8"));
                ddlProjectID.Items.Add(new ListItem("INTERNATIONAL SALES", "9"));
                ddlProjectID.Items.Add(new ListItem("RENTAL DIVISION", "10"));


                
                pnlParameter.Controls.Add(ddlProjectID);
                pnlParameter.Controls.Add(new LiteralControl("</td>"));



                if ((controlCount % 3) == 2)
                {
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));

                }

                controlCount = controlCount + 1;

            }

            if (crReportDocument.ParameterFields["@Type"] != null)
            {


                if ((controlCount % 3) == 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                }

                

               

                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Customer Type :"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                DropDownList ddlType = new DropDownList();
                ddlType.ID = "ddlType";
              
                //ddlType.AutoPostBack = true;
                //ddlType.SelectedIndexChanged += DropDownListType_SelectedIndexChanged;
                ddlType.Items.Clear();
                ddlType.Items.Add(new ListItem("ALL", "All"));
                ddlType.Items.Add(new ListItem("DIRECT CUSTOMERS", "External"));
                ddlType.Items.Add(new ListItem("INTERCO CUSTOMERS", "Internal"));
                

                pnlParameter.Controls.Add(ddlType);
                pnlParameter.Controls.Add(new LiteralControl("</td>"));



                if ((controlCount % 3) == 2)
                {
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));

                }

                controlCount = controlCount + 1;

            }

            
            Button dynamicbutton = new Button();
            dynamicbutton.Click += new System.EventHandler(btnSubmit_Click);
            dynamicbutton.Text = "Submit";
            dynamicbutton.CssClass = "button";
            dynamicbutton.ID = "btnSubmit";
            
            pnlParameter.Controls.Add(new LiteralControl("<tr>"));
            pnlParameter.Controls.Add(new LiteralControl("<td></td><td></td><td></td><td></td><td></td>"));
            pnlParameter.Controls.Add(new LiteralControl("<td align=\"right\">"));
            pnlParameter.Controls.Add(dynamicbutton);
            pnlParameter.Controls.Add(new LiteralControl("</td>"));
            pnlParameter.Controls.Add(new LiteralControl("</table>"));


            IsControlAdded = true;

            if (controlCount == 0)
            {
                pnlParameter.Visible = false;
                RefreshCrystalReport();
            }
           
        }        

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            RefreshCrystalReport();
        }
        #endregion




        protected void Page_Unload(object sender, EventArgs e)
        {
            cyReportViewer.Dispose();
            cyReportViewer = null;
            crReportDocument.Close();
            crReportDocument.Dispose();
            GC.Collect();
        }

        

        private void RefreshCrystalReport()
        {
            LogSession session = base.GetSessionInfo();

            try
            {
                ConnectionInfo connection = new ConnectionInfo();
                connection.DatabaseName = DBManager.GetInstance().DatabaseName;
                connection.ServerName = DBManager.GetInstance().ServerName;
                connection.UserID = DBManager.GetInstance().UserLoginName;
                connection.Password = DBManager.GetInstance().UserLoginPwd;

                foreach (CrystalDecisions.CrystalReports.Engine.Table table in crReportDocument.Database.Tables)
                {
                    // Cache the logon info block
                    TableLogOnInfo logOnInfo = table.LogOnInfo;

                    // Set the connection
                    logOnInfo.ConnectionInfo = connection;

                    // Apply the connection to the table!
                    table.ApplyLogOnInfo(logOnInfo);
                }

                foreach (CrystalDecisions.CrystalReports.Engine.Section section in crReportDocument.ReportDefinition.Sections)
                {
                    // In each section we need to loop through all the reporting objects
                    foreach (CrystalDecisions.CrystalReports.Engine.ReportObject reportObject in section.ReportObjects)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subReport = (SubreportObject)reportObject;
                            ReportDocument subDocument = subReport.OpenSubreport(subReport.SubreportName);

                            foreach (CrystalDecisions.CrystalReports.Engine.Table table in subDocument.Database.Tables)
                            {
                                // Cache the logon info block
                                TableLogOnInfo logOnInfo = table.LogOnInfo;

                                // Set the connection
                                logOnInfo.ConnectionInfo = connection;

                                // Apply the connection to the table!
                                table.ApplyLogOnInfo(logOnInfo);
                            }
                        }
                    }
                }

                //JScriptAlertMsg(ViewState["ddlYear"].ToString());

                

                if (crReportDocument.ParameterFields["@Year"] != null)
                    crReportDocument.SetParameterValue("@Year", GMSUtil.ToInt(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue.ToString()));
                if (crReportDocument.ParameterFields["Year"] != null)
                    crReportDocument.SetParameterValue("Year", GMSUtil.ToInt(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue.ToString()));
                if (crReportDocument.ParameterFields["@Month"] != null)
                    crReportDocument.SetParameterValue("@Month", GMSUtil.ToInt(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue.ToString()));
                if (crReportDocument.ParameterFields["Month"] != null)
                    crReportDocument.SetParameterValue("Month", GMSUtil.ToInt(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue.ToString()));
                if (crReportDocument.ParameterFields["@ProjectID"] != null && session.CompanyId == 28)
                    crReportDocument.SetParameterValue("@ProjectID", GMSUtil.ToInt(((DropDownList)pnlParameter.FindControl("ddlProjectID")).SelectedValue.ToString()));
                else if (crReportDocument.ParameterFields["@ProjectID"] != null)
                    crReportDocument.SetParameterValue("@ProjectID", -1);
                if (crReportDocument.ParameterFields["@Type"] != null)
                    crReportDocument.SetParameterValue("@Type", GMSUtil.ToInt(((DropDownList)pnlParameter.FindControl("ddlType")).SelectedValue.ToString()));
                if (crReportDocument.ParameterFields["@SelectedCurrency"] != null)
                    crReportDocument.SetParameterValue("@SelectedCurrency", GMSUtil.ToInt(((DropDownList)pnlParameter.FindControl("ddlCurrency")).SelectedValue.ToString()));



                if (crReportDocument.ParameterFields["@CoyID"] != null)
                    crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                if (crReportDocument.ParameterFields["CoyID"] != null)
                    crReportDocument.SetParameterValue("CoyID", session.CompanyId);
                if (crReportDocument.ParameterFields["@UserNumID"] != null)
                    crReportDocument.SetParameterValue("@UserNumID", session.UserId);


                cyReportViewer.ReportSource = crReportDocument;


            }
            catch (Exception ex)
            {
                this.lblFeedback.Text = ex.Message;
            }


        }


         
    }
}
