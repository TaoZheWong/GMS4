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
using System.Collections.Generic;

using GMSCore;
using GMSWeb.CustomCtrl;
using GMSCore.Entity;
using GMSCore.Activity;

namespace GMSWeb.Sales.Sales
{
    public partial class SalesDetail : GMSBasePage
    {
        string isLargeFont, isOptimizedTable;

        protected short loginUserOrAlternateParty = 0;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            DataSet lstAlterParty = new DataSet();
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


            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                            92);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            92);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Sales"));

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
            

            if (!Page.IsPostBack)
            {
                //preload
                if (Request.Params["TrnType"] != null)
                {
                    hidTrnType.Value = Request.Params["TrnType"].ToString().Trim();
                }
                if (Request.Params["TrnNnumber"] != null)
                {
                    hidTrnNo.Value = Request.Params["TrnNnumber"].ToString().Trim();
                }
                if (Request.Params["SrNo"] != null)
                {
                    hidSrNo.Value = Request.Params["SrNo"].ToString().Trim();
                }
                if (Request.Params["ProdCode"] != null)
                {
                    hidProductCode.Value = Request.Params["ProdCode"].ToString().Trim();
                }
                if (Request.Params["AccountCode"] != null)
                {
                    hidAccountCode.Value = Request.Params["AccountCode"].ToString().Trim();
                }


                LoadData();
            }
        }
        #endregion

        #region LoadData
        protected void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            ProductsDataDALC dacl = new ProductsDataDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetSalesTransactionByPrimanyKey(session.CompanyId, hidTrnType.Value.ToString(), int.Parse(hidTrnNo.Value.ToString()), 
                                                    short.Parse(hidSrNo.Value.ToString()), hidProductCode.Value, hidAccountCode.Value, ref ds);
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                DataSet ds1 = new DataSet();
                (new GMSGeneralDALC()).IsProductManager(session.CompanyId, ds.Tables[0].Rows[0]["ProductCode"].ToString(), loginUserOrAlternateParty, ref ds1);
                if (ds1 == null || ds1.Tables.Count <= 0 || ds1.Tables[0].Rows.Count <= 0)
                {
                    JScriptAlertMsg("You do not have access to this data.");
                    return;
                }


                lblTrnDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["TrnDate"].ToString()).ToString("dd/MM/yyyy");
                lblRefNo.Text = ds.Tables[0].Rows[0]["RefNo"].ToString();
                lblProductCode.Text = ds.Tables[0].Rows[0]["ProductCode"].ToString();
                lblProductName.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();
                lblProductGroup.Text = ds.Tables[0].Rows[0]["ProductGroupName"].ToString();
                lblUnitPrice.Text = double.Parse(ds.Tables[0].Rows[0]["UnitPrice"].ToString()).ToString("#0.00");
                lblUnitCost.Text = double.Parse(ds.Tables[0].Rows[0]["UnitCost"].ToString()).ToString("#0.00");
                lblQty.Text = ds.Tables[0].Rows[0]["Qty"].ToString();
                lblTotalSales.Text = double.Parse(ds.Tables[0].Rows[0]["TotalSales"].ToString()).ToString("#0.00");
                lblTotalCost.Text = double.Parse(ds.Tables[0].Rows[0]["TotalCost"].ToString()).ToString("#0.00");
                lblGP.Text = double.Parse(ds.Tables[0].Rows[0]["GP%"].ToString()).ToString("#0.00") + "%";
            }
        }
        #endregion

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
