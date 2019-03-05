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
using System.IO;


using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Collections.Generic;


namespace GMSWeb.Reports.Report
{
    public partial class ExternalReportViewer : GMSBasePage
    {
        private short reportId = 0;
        string fileName = "";
        string fileDescription = "";
        protected short loginUserOrAlternateParty = 0;
        protected DataSet ds = new DataSet();
        protected DataSet ds_lms = new DataSet();
        private string currentLink = "";

        string isLargeFont, isOptimizedTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }

            //Getting LargerFont Cookies
            HttpCookie isLargeFontCookie = Request.Cookies["isLargeFont"];
            if (null == isLargeFontCookie)
                isLargeFont = "";
            else
                isLargeFont = isLargeFontCookie.Value == "true" ? "largeFont" : "";

            //Getting optimizedtable Cookies
            HttpCookie isOptimizedTableCookie = Request.Cookies["isOptimizedTable"];
            if (null == isOptimizedTableCookie)
                isOptimizedTable = "";
            else
                isOptimizedTable = isOptimizedTableCookie.Value == "true" ? "optimizedTable" : "";

            //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
            //                                                                49);
            //if (uAccess == null)
            //    Response.Redirect("../../Unauthorized.htm");
            this.currentLink = Request.QueryString["CurrentLink"];

            DataSet lstAlterParty = new DataSet();
            //new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Report", ref lstAlterParty);
            if (currentLink == "Products")
                new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Report", ref lstAlterParty);
            else
                new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Sales Detail", ref lstAlterParty);

            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {
                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;

            this.reportId = GMSUtil.ToShort(Request.QueryString["REPORTID"]);


            if (reportId != -1 && reportId != -2 && reportId != -3)
            {
                GMSCore.Entity.UserAccessReport accessreport = UserAccessReport.RetrieveByKey(loginUserOrAlternateParty, reportId);
                GMSCore.Entity.UserAccessReportForCompany accessreportcoy = UserAccessReportForCompany.RetrieveByKey(session.CompanyId, loginUserOrAlternateParty, reportId);

                if (accessreport == null && accessreportcoy == null)
                {
                    this.lblFeedback.Text = "You do not have access to this data.";
                    return;
                }

                fileName = new ReportsActivity().RetrieveReportById(this.reportId).FileName;
                fileDescription = new ReportsActivity().RetrieveReportById(this.reportId).Description;
                fileDescription = fileDescription.Substring(0, 1);

                GMSCore.Entity.AuditForReportAccess audit = AuditForReportAccess.RetrieveByKey(session.CompanyId, session.UserId, reportId, GMSUtil.ToDate(session.LastLoginDate));
                if (audit == null)
                {
                    audit = new AuditForReportAccess();
                    audit.CoyID = session.CompanyId;
                    audit.UserID = session.UserId;
                    audit.ReportID = reportId;
                    audit.AccessDate = GMSUtil.ToDate(session.LastLoginDate);
                    audit.Save();
                }
            }
            else
            {
                fileName = Request.QueryString["REPORT"].Trim() + ".rpt";
            }

            if (IsControlAdded)
            {
                //JScriptAlertMsg("IsControlAdded");
                AddControls();
            }

            if (!IsPostBack)
            {
                AddControls();
                //JScriptAlertMsg("!IsPostBack");
            } 

            string javaScript = @"<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>";
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
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
            GMSGeneralDALC dacl = new GMSGeneralDALC();

            int controlCount = 0;

            pnlParameter.Controls.Add(new LiteralControl("<div class='form-horizontal m-t-20'>"));

            if (reportId.ToString() == "525" || reportId.ToString() == "526" || reportId.ToString() == "527")
            {                
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Date :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"input-group date\">"));
                    TextBox txtDate = new TextBox();
                    txtDate.ID = "txtDate";
                    txtDate.Columns = 15;
                    txtDate.CssClass = "form-control datepicker";
                    txtDate.Text = System.DateTime.Today.ToString("dd/MM/yyyy");
                    pnlParameter.Controls.Add(txtDate);
                    if (ViewState["txtDate"] == null)
                        ViewState["txtDate"] = System.DateTime.Today.ToString();
                    pnlParameter.Controls.Add(new LiteralControl("<span class=\"input-group-addon\"><i class=\"ti-calendar\"></i></span>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                

            }
            else if (reportId.ToString() == "530")
            {                   
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Document No. :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    TextBox txtDocumentNo = new TextBox();
                    txtDocumentNo.ID = "txtDocumentNo";
                    txtDocumentNo.Text = "";
                    txtDocumentNo.Attributes["placeholder"] = "e.g. W1700158";
                    txtDocumentNo.CssClass = "form-control";
                    pnlParameter.Controls.Add(txtDocumentNo);
                    if (ViewState["txtDocumentNo"] == null)
                        ViewState["txtDocumentNo"] = "";                    

                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Batch / Serial No. :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    TextBox txtBatchSerialNo = new TextBox();
                    txtBatchSerialNo.ID = "txtBatchSerialNo";
                    txtBatchSerialNo.Text = "";
                    txtBatchSerialNo.Attributes["placeholder"] = "e.g. SN005";
                    txtBatchSerialNo.CssClass = "form-control";
                    pnlParameter.Controls.Add(txtBatchSerialNo);
                    if (ViewState["txtBatchSerialNo"] == null)
                        ViewState["txtBatchSerialNo"] = "";                    
                
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                
            }
            /*
            else if (reportId.ToString() == "222")
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Customer Code :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtAccountCode = new TextBox();
                txtAccountCode.ID = "txtAccountCode";
                txtAccountCode.Text = "";
                txtAccountCode.Attributes["placeholder"] = "e.g. DLK690";
                txtAccountCode.CssClass = "form-control";
                pnlParameter.Controls.Add(txtAccountCode);
                if (ViewState["txtAccountCode"] == null)
                    ViewState["txtAccountCode"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Customer Name :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtAccountName = new TextBox();
                txtAccountName.ID = "txtAccountName";
                txtAccountName.Text = "";
                txtAccountName.Attributes["placeholder"] = "e.g. Keppel";
                txtAccountName.CssClass = "form-control";
                pnlParameter.Controls.Add(txtAccountName);
                if (ViewState["txtAccountName"] == null)
                    ViewState["txtAccountName"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }
            */

            Button dynamicbutton = new Button();
            dynamicbutton.Click += new System.EventHandler(btnSubmit_Click);
            dynamicbutton.Text = "Export";
            dynamicbutton.CssClass = "btn btn-primary pull-right";
            dynamicbutton.ID = "btnSubmit";           

            pnlParameter.Controls.Add(new LiteralControl("</div>"));
            pnlParameter.Controls.Add(new LiteralControl("</div>"));
            pnlParameter.Controls.Add(new LiteralControl("<div class='panel-footer clearfix'>"));
            pnlParameter.Controls.Add(dynamicbutton);
            pnlParameter.Controls.Add(new LiteralControl("</div>"));

            IsControlAdded = true;

        }

        private void SaveParameter()
        {
            LogSession session = base.GetSessionInfo();
            if (session.StatusType.ToString() == "L")
            {
                if (reportId.ToString() == "525" || reportId.ToString() == "526" || reportId.ToString() == "527")
                {
                    ViewState["txtDate"] = GMSUtil.ToDate(((TextBox)pnlParameter.FindControl("txtDate")).Text.ToString());
                }
                else if (reportId.ToString() == "527")
                {
                    ViewState["txtDate"] = GMSUtil.ToDate(((TextBox)pnlParameter.FindControl("txtDate")).Text.ToString());
                }
                else if (reportId.ToString() == "530")
                {
                    ViewState["txtDocumentNo"] = ((TextBox)pnlParameter.FindControl("txtDocumentNo")).Text.ToString();
                    ViewState["txtBatchSerialNo"] = ((TextBox)pnlParameter.FindControl("txtBatchSerialNo")).Text.ToString();
                }
            }
        }

        private void test()
        {

        }

        private void PrintReport()
        {
            LogSession session = base.GetSessionInfo();
            DateTime RequiredDate;
            string DocumentNo;
            string BatchSerialNo;
            CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
            DivisionUser du;
            DataRow newRow;

            if (session.StatusType.ToString() == "L")
            {
                if (reportId.ToString() == "525")
                {
                    RequiredDate = GMSUtil.ToDate(ViewState["txtDate"].ToString());                    
                    du = DivisionUser.RetrieveByKey(session.CompanyId, loginUserOrAlternateParty);
                    if (du != null)
                    {
                        if (du.DivisionID == "GAS")
                        {
                            if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                                sc1.Url = session.GASLMSWebServiceAddress.Trim();
                            else
                                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                            ds = sc1.GetDeliveryOrderByDeliveryZone(RequiredDate.ToString("yyyy-MM-dd"));

                        }
                        else if (du.DivisionID == "WSD")
                        {
                            if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                                sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                            else
                                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                            ds_lms = sc1.GetDeliveryOrderByDeliveryZone(RequiredDate.ToString("yyyy-MM-dd"));

                        }

                        if (ds_lms != null && ds_lms.Tables.Count > 0 && ds_lms.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables.Count == 0)
                                ds = ds_lms;
                            else
                            {
                                for (int i = 0; i < ds_lms.Tables[0].Rows.Count; i++)
                                {
                                    ds.Tables[0].ImportRow(ds_lms.Tables[0].Rows[i]);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        ds = sc1.GetDeliveryOrderByDeliveryZone(RequiredDate.ToString("yyyy-MM-dd"));

                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        newRow = ds.Tables[0].NewRow();
                        newRow["CoyCode"] = "Date Printed : ";
                        newRow["DocNo"] = DateTime.Today.ToString("yyyy-MM-dd");
                        ds.Tables[0].Rows.Add(newRow);
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=DeliveryOrder.xls");
                }
                else if (reportId.ToString() == "526")
                {                    
                    RequiredDate = GMSUtil.ToDate(ViewState["txtDate"].ToString());
                    du = DivisionUser.RetrieveByKey(session.CompanyId, loginUserOrAlternateParty);
                    if (du != null)
                    {
                        if (du.DivisionID == "GAS")
                        {
                            if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                                sc1.Url = session.GASLMSWebServiceAddress.Trim();
                            else
                                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                            ds = sc1.GetDeliveryOrderByDeliveryZoneByProduct(RequiredDate.ToString("yyyy-MM-dd"));

                        }
                        else if (du.DivisionID == "WSD")
                        {
                            if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                                sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                            else
                                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                            ds_lms = sc1.GetDeliveryOrderByDeliveryZoneByProduct(RequiredDate.ToString("yyyy-MM-dd"));

                        }

                        if (ds_lms != null && ds_lms.Tables.Count > 0 && ds_lms.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables.Count == 0)
                                ds = ds_lms;
                            else
                            {
                                for (int i = 0; i < ds_lms.Tables[0].Rows.Count; i++)
                                {
                                    ds.Tables[0].ImportRow(ds_lms.Tables[0].Rows[i]);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        ds = sc1.GetDeliveryOrderByDeliveryZoneByProduct(RequiredDate.ToString("yyyy-MM-dd"));

                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        newRow = ds.Tables[0].NewRow();
                        newRow["CoyCode"] = "Date Printed : ";
                        newRow["DocNo"] = DateTime.Today.ToString("yyyy-MM-dd");
                        ds.Tables[0].Rows.Add(newRow);
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=DeliveryOrderByProduct.xls");
                }
                else if (reportId.ToString() == "527")
                {                    
                    RequiredDate = GMSUtil.ToDate(ViewState["txtDate"].ToString());
                    du = DivisionUser.RetrieveByKey(session.CompanyId, loginUserOrAlternateParty);
                    if (du != null)
                    {
                        if (du.DivisionID == "GAS")
                        {
                            if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                                sc1.Url = session.GASLMSWebServiceAddress.Trim();
                            else
                                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                            ds = sc1.GetDeliveryOrderByDeliveryZone(RequiredDate.ToString("yyyy-MM-dd"));

                        }
                        else if (du.DivisionID == "WSD")
                        {
                            if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                                sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                            else
                                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                            ds_lms = sc1.GetDeliveryOrderByDeliveryZone(RequiredDate.ToString("yyyy-MM-dd"));

                        }

                        if (ds_lms != null && ds_lms.Tables.Count > 0 && ds_lms.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables.Count == 0)
                                ds = ds_lms;
                            else
                            {
                                for (int i = 0; i < ds_lms.Tables[0].Rows.Count; i++)
                                {
                                    ds.Tables[0].ImportRow(ds_lms.Tables[0].Rows[i]);
                                }
                            }
                        }

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            int desiredSize = 2;
                            while (ds.Tables[0].Columns.Count > desiredSize)
                            {
                                ds.Tables[0].Columns.RemoveAt(desiredSize);
                            }

                            ds.Tables[0].Columns.Add("Signature");
                            ds.Tables[0].Columns.Add("Date");
                            ds.Tables[0].Columns.Add("Remarks");
                        }
                    }
                    else
                    {
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        ds = sc1.GetDeliveryOrderByDeliveryZone(RequiredDate.ToString("yyyy-MM-dd"));
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        newRow = ds.Tables[0].NewRow();
                        newRow["CoyCode"] = "Date Printed : ";
                        newRow["DocNo"] = DateTime.Today.ToString("yyyy-MM-dd");
                        ds.Tables[0].Rows.Add(newRow);
                    }
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=DeliveryOrderByDriver.xls");
                }
                /*
                else if (reportId.ToString() == "222")
                {
                    string accountCode = "%" + ((TextBox)pnlParameter.FindControl("txtAccountCode")).Text.ToString() + "%";
                    string accountName = "%" + ((TextBox)pnlParameter.FindControl("txtAccountName")).Text.ToString() + "%";

                    DateTime LMSParallelRunEndDate = GMSUtil.ToDate(session.LMSParallelRunEndDate);
                    
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                    {
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    }
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    ds = sc1.ReportGetProductOnSODetail(accountCode, accountName);

                    Response.Clear();
                    Response.Buffer = true;

                    Response.AddHeader("content-disposition", "attachment;filename=PendingSO.xls");
                }
                */
                else if (reportId.ToString() == "222" || reportId.ToString() == "515")
                {
                    string salesmanID = "'0'".ToString();
                    GMSGeneralDALC dacl = new GMSGeneralDALC();
                    DataSet dsSalesPerson = new DataSet();
                    try
                    {
                        dacl.GetSalesPersonListSelect(session.CompanyId, loginUserOrAlternateParty, ref dsSalesPerson);
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }

                    if (dsSalesPerson != null && dsSalesPerson.Tables.Count > 0 && dsSalesPerson.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsSalesPerson.Tables[0].Rows)
                        {
                            salesmanID += ",'" + dr["salespersonid"].ToString() + "'";
                        }
                    }

                    du = DivisionUser.RetrieveByKey(session.CompanyId, loginUserOrAlternateParty);
                    if (du != null)
                    {
                        if (du.DivisionID == "GAS")
                        {
                            if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                                sc1.Url = session.GASLMSWebServiceAddress.Trim();
                            else
                                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                            ds = sc1.GetPendingSO(session.UserRealName, salesmanID);

                        }
                        else if (du.DivisionID == "WSD")
                        {
                            if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                                sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                            else
                                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                            ds_lms = sc1.GetPendingSO(session.UserRealName, salesmanID);

                        }

                        if (ds_lms != null && ds_lms.Tables.Count > 0 && ds_lms.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables.Count == 0)
                                ds = ds_lms;
                            else
                            {
                                for (int i = 0; i < ds_lms.Tables[0].Rows.Count; i++)
                                {
                                    ds.Tables[0].ImportRow(ds_lms.Tables[0].Rows[i]);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        ds = sc1.GetPendingSO(session.UserRealName, salesmanID);

                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=PendingSO.xls");

                }
                else if (reportId.ToString() == "530")
                {                   
                    DocumentNo = ViewState["txtDocumentNo"].ToString();
                    BatchSerialNo = ViewState["txtBatchSerialNo"].ToString();

                    du = DivisionUser.RetrieveByKey(session.CompanyId, loginUserOrAlternateParty);
                    if (du != null)
                    {
                        if (du.DivisionID == "GAS")
                        {
                            if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                                sc1.Url = session.GASLMSWebServiceAddress.Trim();
                            else
                                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                            ds = sc1.GetBatchSerialDO(DocumentNo, BatchSerialNo);

                        }
                        else if (du.DivisionID == "WSD")
                        {
                            if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                                sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                            else
                                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                            ds_lms = sc1.GetBatchSerialDO(DocumentNo, BatchSerialNo);

                        }

                        if (ds_lms != null && ds_lms.Tables.Count > 0 && ds_lms.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables.Count == 0)
                                ds = ds_lms;
                            else
                            {
                                for (int i = 0; i < ds_lms.Tables[0].Rows.Count; i++)
                                {
                                    ds.Tables[0].ImportRow(ds_lms.Tables[0].Rows[i]);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        ds = sc1.GetBatchSerialDO(DocumentNo, BatchSerialNo);
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=BatchSerial.xls");

                }

                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                DataTable copyDataTable;
                copyDataTable = ds.Tables[0].Copy();

                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = copyDataTable;
                GridView1.DataBind();
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    GridViewRow row = GridView1.Rows[i];
                    //Change Color back to white
                    row.BackColor = System.Drawing.Color.White;
                    //Apply text style to each Row
                    row.Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {    
            LogSession session = base.GetSessionInfo();
            if (session.StatusType.ToString() == "L")
            {
                if (reportId.ToString() == "525" || reportId.ToString() == "526" || reportId.ToString() == "527")
                {                    
                    ViewState["txtDate"] = GMSUtil.ToDate(((TextBox)pnlParameter.FindControl("txtDate")).Text.ToString());
                }
                else if (reportId.ToString() == "527")
                {                   
                    ViewState["txtDate"] = GMSUtil.ToDate(((TextBox)pnlParameter.FindControl("txtDate")).Text.ToString());
                }
                else if (reportId.ToString() == "530")
                {
                    ViewState["txtDocumentNo"] = ((TextBox)pnlParameter.FindControl("txtDocumentNo")).Text.ToString();
                    ViewState["txtBatchSerialNo"] = ((TextBox)pnlParameter.FindControl("txtBatchSerialNo")).Text.ToString();
                }
                PrintReport();                
            }   
        }

        private string GetFinanceReportType()
        {
            string ReportType = string.Empty;

            if (reportId == 441 || reportId == 448 || reportId == 452 || reportId == 456 || reportId == 460)
            {
                return "PNL";
            }
            else if (reportId == 445 || reportId == 449 || reportId == 453 || reportId == 457 || reportId == 461)
            {
                return "PNLD";
            }
            else if (reportId == 446 || reportId == 450 || reportId == 454 || reportId == 458 || reportId == 462)
            {
                return "BS";
            }
            else //447, 451, 455, 459, 463
            {
                return "BSD";
            }

        }

        public string getIsOptimizedTable
        {
            get { return isOptimizedTable; }
        }

        public string getIsLargeFont
        {
            get { return isLargeFont; }
        }
    }
}