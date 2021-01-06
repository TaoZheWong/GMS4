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
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Products.Products
{
    public partial class ProductUOM : GMSBasePage
    {
        private string productCode = "", type = "", warehouse = "";
        protected short loginUserOrAlternateParty = 0;
        string isLargeFont, isOptimizedTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            this.productCode = Request.Params["PRODUCTCODE"];
            this.warehouse = Request.Params["WAREHOUSE"];
            this.type = Request.Params["TYPE"];

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Search", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {

                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;

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


            LoadData();
        }

        protected void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();           

            CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
            if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
            {
                sc1.Url = session.CMSWebServiceAddress.Trim();
            }
            else
                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
            ds = sc1.GetProductUOM(productCode);

            try
            {               
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.dgData.DataSource = ds;
                    this.dgData.DataBind();
                }
                else
                {
                        lblMsg.Text = "No record is found.";
                }
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
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