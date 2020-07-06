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
using System.Text;
using System.Web.Services;
using Newtonsoft.Json.Serialization;

namespace GMSWeb.Products.Products
{
    public partial class GRNSearch : GMSBasePage
    {
        #region Page_Load
        protected short loginUserOrAlternateParty = 0;
        protected string userDivision;
        protected DataSet ds = new DataSet();
        protected DataSet ds2 = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "Products";
           
            lblPageHeader.Text = "Products";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            if(session.IsOffline.ToString() == "True")
                Response.Redirect(base.OfflinePage(currentLink));

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            165); //165
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            165); //165

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

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload
                ViewState["SortField"] = "TrnNo";
                ViewState["SortDirection"] = "ASC";
                this.txtGRNDateFrom.Text = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");//preload 7days ago date
                this.txtGRNDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");//preload today's date
            }

            getUserDivision(session, loginUserOrAlternateParty);//get user's division
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
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
       
        #region LoadData
        private void LoadData() {
            LogSession session = base.GetSessionInfo();

            string poNo ="";
            string trnNo = "";
            string trnDateFrom = "";
            string trnDateTo = "";
            string productCode = "";
            string productName = "";

            if (!string.IsNullOrEmpty(txtPoNo.Text.Trim()) || !string.IsNullOrEmpty(txtGRNNo.Text.Trim()) ||
                !string.IsNullOrEmpty(txtGRNDateFrom.Text.Trim()) || !string.IsNullOrEmpty(txtProductCode.Text.Trim()) ||
                !string.IsNullOrEmpty(txtGRNDateTo.Text.Trim()) || !string.IsNullOrEmpty(txtProductName.Text.Trim()))
            {
                poNo = txtPoNo.Text.Trim();
                trnNo = txtGRNNo.Text.Trim();
                if (txtGRNDateFrom.Text.Trim() != "") 
                    trnDateFrom = DateTime.Parse(txtGRNDateFrom.Text.Trim()).ToString("yyyy-MM-dd");
                if (txtGRNDateTo.Text.Trim() != "")
                    trnDateTo = DateTime.Parse(txtGRNDateTo.Text.Trim()).ToString("yyyy-MM-dd");
                productCode = txtProductCode.Text.Trim();
                productName = txtProductName.Text.Trim();
            }
            try
            {
                if (session.StatusType.ToString() == "S" || session.StatusType.ToString() == "L")
                {
                    string query = "CALL \"AF_API_GET_SAP_GRN_DETAIL\" ('" + trnNo + "', '" + trnNo + "', '" + trnDateFrom + "', '" + trnDateTo + "')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "TrnNo", "TrnDate", "DetailNo", "PONo", "PODate", "ProductCode", "ProductDescription", "ProductGroupCode", "ProductGroupName", "UOM", "Quantity", "UnitPrice", "LandedCostUnitPrice", "Cost", "Currency", "WH", "ExchangeRate", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    string query2 = "CALL \"AF_API_GET_SAP_GRN_HEADER\" ('" + trnNo + "', '" + trnNo + "', '" + trnDateFrom + "', '" + trnDateTo + "')";
                    SAPOperation sop2 = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());

                    ds2 = sop2.GET_SAP_QueryData(session.CompanyId, query2,
                    "trnno", "TrnDate", "RefNo", "code", "cname", "Add1", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    ds.Tables[0].Columns.Add("VendorCode", typeof(string));
                    ds.Tables[0].Columns.Add("VendorName", typeof(string));

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (ds2.Tables[0].Rows.Count != 0)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                                {
                                    if (ds2.Tables[0].Rows[i][0].ToString() == ds.Tables[0].Rows[j][0].ToString())
                                    {
                                        ds.Tables[0].Rows[j]["VendorCode"] = ds2.Tables[0].Rows[i]["code"].ToString();
                                        ds.Tables[0].Rows[j]["VendorName"] = ds2.Tables[0].Rows[i]["cname"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        DataView dvOri = new DataView(ds.Tables[0]);
                        dvOri.RowFilter = "PONo like '*" + poNo + "*'";
                        dvOri.RowFilter = "ProductCode like '*" + productCode + "*'";
                        dvOri.RowFilter = "ProductDescription like '*" + productName + "*'";
                        if (session.CompanyId == 120 && userDivision == "GAS")
                            dvOri.RowFilter = "VendorCode not like '*C2*'";
                        else if (session.CompanyId == 120 && userDivision == "WSD")
                            dvOri.RowFilter = "VendorCode like '*C2*'";
                        DataTable dtOri = dvOri.ToTable(true, "TrnNo", "TrnDate", "PONo", "PODate", "VendorCode", "VendorName");
                        ds.Reset();
                        ds.Tables.Add(dtOri);
                    }
                }

                int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
                int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (endIndex < ds.Tables[0].Rows.Count)
                        this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                    else
                        this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                    ds.Tables[0].DefaultView.Sort = "TrnNo ASC";
                    DataView dv = ds.Tables[0].DefaultView;
                    dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
                   
                    this.lblSearchSummary.Visible = true;
                    this.dgData.DataSource = dv;
                    this.dgData.DataBind();
                }
                else
                {
                    this.lblSearchSummary.Text = "No records.";
                    this.lblSearchSummary.Visible = true;
                    this.dgData.DataSource = null;
                    this.dgData.DataBind();
                }
                resultList.Visible = true;
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                //this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. ", MessagePanelControl.MessageEnumType.Alert);
            }
        }
        #endregion
 
        #region SortData
        protected void SortData(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
            {
                switch (ViewState["SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["SortField"] = e.SortExpression;
                ViewState["SortDirection"] = "ASC";
            }
            LoadData();
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
