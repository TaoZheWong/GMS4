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
using System.Globalization;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System.Collections.Generic;

namespace GMSWeb.Products.Products
{
    public partial class GRNDetail : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;
        protected bool canAccessCost = false;
        protected bool canAccessProductStatus = false;
        protected bool isGasDivision = false;
        protected bool isWeldingDivision = false;
        protected string userDivision;
        DataSet dsBatch = new DataSet();
        DataSet dsSerial = new DataSet();

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string currentLink = "Products";
            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Products"));
                return;
            }

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "GRN Search", ref lstAlterParty);
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
                                                                            165); // 165
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            165); // 165
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Products"));

            getUserDivision(session, loginUserOrAlternateParty);//get user's division
            if (!Page.IsPostBack)
            {
                //preload
                if (Request.Params["TrnNo"] != null)
                {
                    hidTrnNo.Value = Request.Params["TrnNo"].ToString().Trim();
                    LoadData();
                }
            }
            

            string javaScript =
@"
<script type=""text/javascript"">

function ViewBatchSerial(trnno, detailno, type)
{     
    var url = 'ViewGRNBatchSerial.aspx?TRNNO='+trnno+'&DETAILNO='+detailno; 
	window.open(url,"""",""width="" + 900 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
	return false;
}

</script>
";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            if (this.hidTrnNo.Value.Trim() == "")
            {
                base.JScriptAlertMsg("Please provide GRN No to view.");
                return;
            }

            string TrnNo = this.hidTrnNo.Value.Trim();

            ProductsDataDALC dacl = new ProductsDataDALC();
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();            

            try
            {
                if (session.StatusType.ToString() == "S" || session.StatusType.ToString() == "L")
                {
                    string query2 = "CALL \"AF_API_GET_SAP_GRN_HEADER\" ('" + TrnNo + "', '" + TrnNo + "', '', '')";
                    SAPOperation sop2 = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());

                    ds2 = sop2.GET_SAP_QueryData(session.CompanyId, query2,
                    "trnno", "TrnDate", "RefNo", "code", "cname", "Add1", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                    {
                        if (session.CompanyId == 120 && userDivision == "GAS" && (ds2.Tables[0].Rows[0]["code"].ToString()).Substring(0, 2) == "C2")
                        {
                            base.JScriptAlertMsg("Please provide GRN No to view.");
                            return;
                        }
                        else if (session.CompanyId == 120 && userDivision == "WSD" && (ds2.Tables[0].Rows[0]["code"].ToString()).Substring(0, 2) != "C2")
                        {
                            base.JScriptAlertMsg("Please provide GRN No to view.");
                            return;
                        }                       

                       
                            this.lblGRN.Text = ds2.Tables[0].Rows[0]["trnno"].ToString();
                            this.lblGRNDate.Text = DateTime.Parse(ds2.Tables[0].Rows[0]["TrnDate"].ToString()).ToString("yyyy-MM-dd");
                            this.lblVendorCode.Text = ds2.Tables[0].Rows[0]["code"].ToString();
                            this.lblVendorName.Text = ds2.Tables[0].Rows[0]["cname"].ToString();


                            string query = "CALL \"AF_API_GET_SAP_GRN_DETAIL\" ('" + TrnNo + "', '" + TrnNo + "', '', '')";
                            SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                            ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                            "TrnNo", "TrnDate", "DetailNo", "PONo", "PODate", "ProductCode", "ProductDescription", "ProductGroupCode", "ProductGroupName", "UOM", "Quantity", "UnitPrice", "LandedCostUnitPrice", "Cost", "Currency", "WH", "ExchangeRate", "Field18", "Field19", "Field20",
                            "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");


                            string querySerial = "SELECT \"ItemCode\", \"IntrSerial\",  \"ExpDate\", \"PrdDate\", \"InDate\",  \"Quantity\",  \"BaseLinNum\" FROM OSRI WHERE \"BaseNum\" = '" + TrnNo.ToString() + "'";
                            dsSerial = sop.GET_SAP_QueryData(session.CompanyId, querySerial,
                                "ItemCode", "SerialNo", "ExpDate", "MfrDate", "AdmissionDate", "Quantity", "DetailNo", "field8", "field9", "Active", "field11", "field12", "Field13", "Field14", "Field15", "Division", "Field17", "ShortName", "Team", "Field20",
                                "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                            string queryBatch = "SELECT \"ItemCode\", \"BatchNum\", \"ExpDate\", \"PrdDate\", \"InDate\", \"Quantity\", \"BaseLinNum\" FROM OIBT WHERE \"BaseNum\" = '" + TrnNo.ToString() + "'";
                            dsBatch = sop.GET_SAP_QueryData(session.CompanyId, queryBatch,
                                "ItemCode", "BatchNo", "ExpDate", "MfrDate", "AdmissionDate", "Quantity", "DetailNo", "field8", "field9", "Active", "field11", "field12", "Field13", "Field14", "Field15", "Division", "Field17", "ShortName", "Team", "Field20",
                                "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                        
                            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
                            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                ds.Tables[0].Rows[j]["Quantity"] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["Quantity"].ToString()), 6);
                            }
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                if (endIndex < ds.Tables[0].Rows.Count)
                                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                    endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                                else
                                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                    ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                                this.lblSearchSummary.Visible = true;
                                this.dgData.DataSource = ds.Tables[0].DefaultView;
                                this.dgData.DataBind();
                            }
                            else
                            {
                                this.lblSearchSummary.Text = "No records.";
                                this.lblSearchSummary.Visible = true;
                                this.dgData.DataSource = null;
                                this.dgData.DataBind();
                            }
                            this.resultList.Visible = true;                       
                    }
                                        
                }
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataSet dsAccessCost = new DataSet();
                new GMSGeneralDALC().CanUserAccessCost(session.CompanyId, hidTrnNo.Value, loginUserOrAlternateParty, isGasDivision, isWeldingDivision, ref dsAccessCost);
                canAccessCost = Convert.ToBoolean(dsAccessCost.Tables[0].Rows[0]["result"]);
                if (!canAccessCost)
                {

                }
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
        

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            string TrnNo = this.hidTrnNo.Value.Trim();    
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblDetailNo = (Label)e.Item.FindControl("lblDetailNo");
                HtmlGenericControl divBatchSeial = (HtmlGenericControl)e.Item.FindControl("divBatchSeial");

                //LinkButton lnkBatchSerial = (LinkButton)e.Item.FindControl("lnkBatchSerial");
                //Label lblBatchSerial = (Label)e.Item.FindControl("lblBatchSerial");
                string detailNo = lblDetailNo.Text.Trim();

                if (dsBatch != null && dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
                {
                    DataView dvBatch = new DataView(dsBatch.Tables[0]);
                    dvBatch.RowFilter = "DetailNo = '" + detailNo + "'";              
                    DataTable dtBatch = dvBatch.ToTable(true, "ItemCode", "BatchNo", "ExpDate", "MfrDate", "AdmissionDate", "Quantity");
                    if (dtBatch.Rows.Count > 0)
                        divBatchSeial.Visible = true;
                }

                if (dsSerial != null && dsSerial.Tables.Count > 0 && dsSerial.Tables[0].Rows.Count > 0)
                {  
                    DataView dvSerial = new DataView(dsSerial.Tables[0]);
                    dvSerial.RowFilter = "DetailNo = '"+ detailNo +"'";
                    DataTable dtSerial = dvSerial.ToTable(true, "ItemCode", "SerialNo", "ExpDate", "MfrDate", "AdmissionDate", "Quantity");
                                       
                    if (dtSerial.Rows.Count > 0)
                        divBatchSeial.Visible = true;
                }
            }
        }
        #endregion

        #region getUserDivision
        protected void getUserDivision(LogSession session, short usernumid)
        {
            DivisionUser du = DivisionUser.RetrieveByKey(session.CompanyId, usernumid);
            if (du != null)
            {
                if (du.DivisionID == "GAS")
                {
                    userDivision = "GAS";
                }
                else if (du.DivisionID == "WSD")
                {
                    userDivision = "WSD";
                }
            }
            else
                userDivision = "ALL";
        }
        #endregion
    }
}
