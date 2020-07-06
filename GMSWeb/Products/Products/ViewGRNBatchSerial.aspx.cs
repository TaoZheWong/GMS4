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
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Products.Products
{
    public partial class ViewGRNBatchSerial : GMSBasePage
    {
        private string trnno = "", detailno = "";
        protected short loginUserOrAlternateParty = 0;
        string isLargeFont, isOptimizedTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            this.trnno = Request.Params["TRNNO"];
            this.detailno = Request.Params["DETAILNO"];

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            165); //165
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            165); //165

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
            DataSet dsBatch = new DataSet();
            DataSet dsSerial = new DataSet();
            try
            {
                SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                //string queryBatch = "SELECT \"ItemCode\", \"BatchNum\", \"ExpDate\", \"PrdDate\", \"InDate\", \"Quantity\", \"BaseLinNum\" FROM OIBT WHERE \"BaseNum\" = '" + this.trnno + "' AND \"BaseLinNum\" = " + this.detailno ;
                string queryBatch = "SELECT T0.\"ItemCode\", T0.\"BatchNum\", T0.\"ExpDate\", T0.\"PrdDate\", T0.\"InDate\", T1.\"Quantity\", T0.\"BaseLinNum\" FROM OIBT T0 INNER JOIN IBT1 T1 ON T0.\"ItemCode\" = T1.\"ItemCode\" AND T0.\"BatchNum\" = T1.\"BatchNum\"  AND T0.\"BaseNum\" = T1.\"BaseNum\" WHERE T0.\"BaseNum\" = '" + this.trnno + "' AND T0.\"BaseLinNum\" = " + this.detailno;
                
                dsBatch = sop.GET_SAP_QueryData(session.CompanyId, queryBatch,
                    "ItemCode", "BatchNo", "ExpirationDate", "ManufacturingDate", "AdmissionDate", "Qty", "DetailNo", "field8", "field9", "Active", "field11", "field12", "Field13", "Field14", "Field15", "Division", "Field17", "ShortName", "Team", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                string querySerial = "SELECT \"ItemCode\", \"IntrSerial\",  \"ExpDate\", \"PrdDate\", \"InDate\",  \"Quantity\",  \"BaseLinNum\" FROM OSRI WHERE \"BaseNum\" = '" + this.trnno + "' AND \"BaseLinNum\" = " + this.detailno;
                dsSerial = sop.GET_SAP_QueryData(session.CompanyId, querySerial,
                    "ItemCode", "SerialNo", "ExpirationDate", "ManufacturingDate", "AdmissionDate", "Quantity", "DetailNo", "field8", "field9", "Active", "field11", "field12", "Field13", "Field14", "Field15", "Division", "Field17", "ShortName", "Team", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                if (dsBatch != null && dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
                {
                    this.dgData.DataSource = dsBatch;
                    this.dgData.DataBind();
                }
                else if (dsSerial != null && dsSerial.Tables.Count > 0 && dsSerial.Tables[0].Rows.Count > 0)
                {
                    this.dgSerial.DataSource = dsSerial;
                    this.dgSerial.DataBind();
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