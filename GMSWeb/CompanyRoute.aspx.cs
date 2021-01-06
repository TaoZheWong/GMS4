using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMSWeb
{
    public partial class CompanyRoute : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            short companyID = 0;
            if (Request.Params["Type"] != null)
            {
                try{companyID = short.Parse(Request.Params["Type"].ToString());
                }catch(Exception ex) { return; }
            }
            sessionCompanyUpdate(companyID);
        }

        public void sessionCompanyUpdate(short companyId)
        {
            LogSession session = GetSessionInfo();
            SharpPieces.Web.Controls.ExtendedDropDownList ddlCompany = (SharpPieces.Web.Controls.ExtendedDropDownList)this.Master.FindControl("ddlCompany");

            UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
                                                                            companyId);
            if (uAccess == null && companyId != 61)
            {
                ddlCompany.SelectedValue = session.CompanyId.ToString();
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                "<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>", false);
            }

            else
            {
                Company oldCoy = Company.RetrieveByKey(session.CompanyId);
                Company newCoy = Company.RetrieveByKey(companyId);
                session.CompanyId = newCoy.CoyID;
                session.CountryId = newCoy.CountryID;
                session.DivisionId = newCoy.DivisionID;
                session.WebServiceAddress = newCoy.WebServiceAddress;
                session.CMSWebServiceAddress = newCoy.CMSWebServiceAddress;
                session.DefaultCurrency = newCoy.DefaultCurrencyCode;
                session.TBType = newCoy.TBType;
                session.IsOffline = newCoy.IsOffline;
                session.FYE = newCoy.FYE;
                session.DBName = newCoy.DBName;
                session.StatusType = newCoy.StatusType;
                session.LMSParallelRunEndDate = newCoy.LMSParallelRunEndDate;
                session.SAPStartDate = newCoy.SAPStartDate;
                session.GASLMSWebServiceAddress = newCoy.GASLMSWebServiceAddress;
                session.WSDLMSWebServiceAddress = newCoy.WSDLMSWebServiceAddress;
                session.MRScheme = newCoy.MRScheme;
                session.DimensionL1 = newCoy.DimensionL1;
                session.DefaultWarehouse = newCoy.DefaultWarehouse;
                session.TableSuffix = newCoy.TableSuffix;
                session.SAPURI = newCoy.SAPURI;
                session.SAPKEY = newCoy.SAPKEY;
                session.SAPDB = newCoy.SAPDB;
                string appPath = HttpRuntime.AppDomainAppVirtualPath;

                //commented by OSS 12 Sep 2011
                //if (oldCoy.CountryID != 9 && newCoy.CountryID == 9)
                if (newCoy.CountryID == 9)
                {
                    List<short> lstModuleCategory = session.UserAccessModuleCategory;
                    if (lstModuleCategory.Contains(5))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Admin/Default.aspx\";\n</script>", false);
                    }
                    else if (lstModuleCategory.Contains(2))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/SysFinance/Default.aspx\";\n</script>", false);
                    }
                    else if (lstModuleCategory.Contains(3))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/SysHR/Default.aspx\";\n</script>", false);
                    }
                    else System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>", false);
                }
                //commented by OSS 12 Sep 2011
                //else if (oldCoy.CountryID == 9 && newCoy.CountryID != 9)
                else if (newCoy.CountryID != 9)
                {
                    List<short> lstModuleCategory = session.UserAccessModuleCategory;
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "3",
                    //"<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/News/Default.aspx\";\n</script>", false);

                    if (lstModuleCategory.Contains(10))
                        //ddlDivision.Page.Response.Redirect("../Finance/Default.aspx");
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "3",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Finance/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(8))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Sales/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(9))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Debtors/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(7))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Products/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(6))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Suppliers/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(11))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/HR/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(4))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/UsefulResources/Default.aspx\";\n</script>", false);
                    else System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                  "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Sales/Default.aspx\";\n</script>", false);

                }
            }
            ddlCompany.SelectedValue = session.CompanyId.ToString();
        }
    }
}