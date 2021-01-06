using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.Spreadsheet;
using Telerik.Web.UI;

namespace GMSWeb.Sales.Sales
{
    public partial class RadPriceList : GMSBasePage
    {
        bool isMgt = true;
        short loginUserOrAlternateParty = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["authkey"] != null)//for access from GMS3 
            {
                base.loginByAuthKey(Request.Params["authkey"].ToString());
            }
            string currentLink = "Sales";
            lblPageHeader.Text = "Sales";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                if (Request.Params["CurrentLink"].ToString().Trim() == "Sales")
                    lblPageHeader.Text = "Sales";
                else
                    lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);

            LogSession session = base.GetSessionInfo();

            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            166);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            166);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));


            UserAccessModule uAccessforMgt = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            167);

            IList<UserAccessModuleForCompany> uAccessForCompanyListforMgt = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            167);
            if (uAccessforMgt == null && (uAccessForCompanyListforMgt != null && uAccessForCompanyListforMgt.Count == 0))
                isMgt = false;

        }
        #region RadSpreadsheet
        protected void AddSpreadsheetControl()
        {
            #region spreadsheet control
            RadSpreadsheet1.Toolbar.Tabs.Clear();
            RadSpreadsheet1.Toolbar.Tabs.Add(new SpreadsheetToolbarTab() { Text = "Home" });
            SpreadsheetToolbarTab homeToolBarTab = RadSpreadsheet1.Toolbar.Tabs[0];

            SpreadsheetToolbarGroup operationGroup = new SpreadsheetToolbarGroup();
            operationGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.ExportAs, ShowLabel = true });
            operationGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.Undo, ShowLabel = true });
            operationGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.Redo, ShowLabel = true });
            operationGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.Filter, ShowLabel = true });
            operationGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.InsertImage, ShowLabel = true });
            homeToolBarTab.Groups.Add(operationGroup);

            SpreadsheetToolbarGroup fontGroup = new SpreadsheetToolbarGroup();
            fontGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.Bold, ShowLabel = false });
            fontGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.Italic, ShowLabel = false });
            fontGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.Underline, ShowLabel = false });
            fontGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.FontFamily, ShowLabel = false });
            fontGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.FontSize, ShowLabel = false });
            fontGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.BackgroundColor, ShowLabel = false });
            fontGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.TextColor, ShowLabel = false });
            fontGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.BorderType, ShowLabel = false });
            fontGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.BorderColor, ShowLabel = false });
            homeToolBarTab.Groups.Add(fontGroup);

            SpreadsheetToolbarGroup alignmentGroup = new SpreadsheetToolbarGroup();
            alignmentGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.HorizontalAlignment, ShowLabel = false });
            alignmentGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.VerticalAlignment, ShowLabel = false });
            alignmentGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.TextWrap, ShowLabel = false });
            alignmentGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.MergeCells, ShowLabel = false });
            alignmentGroup.Tools.Add(new SpreadsheetTool() { Name = SpreadsheetToolName.Format, ShowLabel = false });
            homeToolBarTab.Groups.Add(alignmentGroup);
            #endregion
        }

        protected void LoadSpreadSheet()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            string ProductGroupCode = "";
            string ProductGroup = "";
            string status = "";

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Report", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {
                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;

            if (string.IsNullOrEmpty(txtProductGroupCode.Text.Trim()) && string.IsNullOrEmpty(txtProductGroup.Text.Trim()))
            {
                this.MsgPanel2.ShowMessage("Please input brand/product code or name to search", MessagePanelControl.MessageEnumType.Alert);
                return;
            }
            else
            {
                ProductGroupCode = "%" + txtProductGroupCode.Text.Trim() + "%";
                ProductGroup = "%" + txtProductGroup.Text.Trim() + "%";
                status = ddlSearchStatus.SelectedValue.Trim();
                resultList.Visible = true;
            }

            try
            {
                ggdal.RetrieveProductPrice2(session.CompanyId, ProductGroupCode, ProductGroup, loginUserOrAlternateParty, status, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }
            if (ds != null)
            {
                this.lblSearchSummary.Visible = false;
                this.RadButton1.Visible = true;
                this.RadSpreadsheet1.Visible = true;
                var sheet1 = FillWorksheet(ds.Tables[0]);
                this.RadSpreadsheet1.Sheets.Add(sheet1);
            }
            var workbook = new Workbook();
            workbook.Sheets = RadSpreadsheet1.Sheets;

            #region radspreadsheet Styling
            workbook.Sheets[0].Columns[0].Width = 90;
            workbook.Sheets[0].Columns[1].Width = 200;
            workbook.Sheets[0].Rows[0].Height = 40;
            foreach (var cell in workbook.Sheets[0].Rows[0].Cells)
            {
                cell.Wrap = true;
                //cell.Enable = false;
                cell.Bold = true;
                cell.Background = "#3f72af";
                cell.Color = "#F6F6F6";
                cell.TextAlign = "Center";
                cell.VerticalAlign = "center";
            }

            for (int i = 1; i < workbook.Sheets[0].Rows.Count; i++)
            {
                var row = workbook.Sheets[0].Rows[i];
                foreach (var cell in row.Cells)
                {
                    cell.FontSize = 10;

                    //vertical align
                    cell.VerticalAlign = "center";
                    cell.Color = "#000000";//font color
                    //background
                    if (IsEven(i))
                    {
                        cell.Background = "#f6f6f6";
                    }
                    else
                    {
                        cell.Background = "#dbe2ef";
                    }
                }
                //disable cells
                row.Cells[0].Enable = false;
                row.Cells[1].Enable = false;
                row.Cells[2].Enable = false;
                row.Cells[4].Enable = false;
                row.Cells[6].Enable = false;
                row.Cells[8].Enable = false;
                //number validate
                row.Cells[3].Format = "#.00";
                row.Cells[5].Format = "#.00";
                row.Cells[7].Format = "#.00";
                //text align
                row.Cells[0].TextAlign = "center";
                row.Cells[1].TextAlign = "left";
                row.Cells[2].TextAlign = "right";
                row.Cells[3].TextAlign = "right";
                row.Cells[4].TextAlign = "right";
                row.Cells[5].TextAlign = "right";
                row.Cells[6].TextAlign = "right";
                row.Cells[7].TextAlign = "right";
                row.Cells[8].TextAlign = "right";
                row.Cells[9].TextAlign = "center";
                row.Cells[10].TextAlign = "left";
                row.Cells[11].TextAlign = "center";
                row.Cells[12].TextAlign = "right";
                row.Cells[13].TextAlign = "right";
                //Validation
                row.Cells[3].Validation = new Validation()
                {
                    AllowNulls = false,
                    DataType = "number",
                    ComparerType = "notBetween",
                    From = "-9999999",
                    To = "-1",
                    Type = "reject",
                    TitleTemplate = "Invalid Number",
                    MessageTemplate = "Invalid Number",
                };

                row.Cells[5].Validation = new Validation()
                {
                    AllowNulls = false,
                    DataType = "number",
                    ComparerType = "notBetween",
                    From = "-9999999",
                    To = "-1",
                    Type = "reject",
                    TitleTemplate = "Invalid Number",
                    MessageTemplate = "Invalid Number",
                };

                row.Cells[7].Validation = new Validation()
                {
                    AllowNulls = false,
                    DataType = "number",
                    ComparerType = "notBetween",
                    From = "-9999999",
                    To = "-1",
                    Type = "reject",
                    TitleTemplate = "Invalid Number",
                    MessageTemplate = "Invalid Number",
                };

                row.Cells[9].Validation = new Validation()
                {
                    AllowNulls = false,
                    DataType = "list",
                    ShowButton = true,
                    ComparerType = "list",
                    From = "\"Yes,No\"",
                    Type = "reject"
                };

                row.Cells[12].Validation = new Validation()
                {
                    AllowNulls = false,
                    DataType = "number",
                    ComparerType = "notBetween",
                    From = "-9999999",
                    To = "-1",
                    Type = "reject",
                    TitleTemplate = "Invalid Number",
                    MessageTemplate = "Invalid Number",
                };
                var date = DateTime.Parse(row.Cells[13].Value.ToString());
                row.Cells[13].Value = date.ToOADate();
                //add date picker
                row.Cells[13].Format = "dd/mm/yyyy";
                row.Cells[13].Validation = new Validation()
                {
                    AllowNulls = false,
                    DataType = "date",
                    ShowButton = true,
                    ComparerType = "between",
                    From = "DATEVALUE(\"1/1/1900\")",
                    To = "DATEVALUE(\"12/31/2099\")",
                    Type = "reject"
                };
            }
            #endregion
            var json = workbook.ToJson();
            HiddenField1.Value = json;
            this.hidEmail.Value = "";
            if (!string.IsNullOrEmpty(txtProductGroupCode.Text.Trim()))
            {
                DataSet dsEmail = new DataSet();
                ggdal.GetPHEmail(session.CompanyId, txtProductGroupCode.Text.Trim(), ref dsEmail);
                this.hidEmail.Value = dsEmail.Tables[0].Rows[0][0].ToString();
            }

            AddSpreadsheetControl();
        }

        protected static bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            var workbook = Workbook.FromJson(HiddenField1.Value);
            workbook.Sheets[0].Rows.Remove(workbook.Sheets[0].Rows[0]);
            string rowChanged = this.hidRowChanged.Value;//get row changed value
            int[] listRowChanged = rowChanged.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c)).ToArray();
            string productList = "";

            if (workbook.Sheets[0].Rows.Count > 0)
            {
                try
                {
                    int rowIndex = 1;
                    foreach (var row in workbook.Sheets[0].Rows)
                    {
                        if (listRowChanged.Length != 0)//check whether there is row changed that stored in array of the hidden field
                        {
                            bool isRowChanged = listRowChanged.Contains(rowIndex);
                            if (isRowChanged)//save only the row changed
                            {
                                string productCode = row.Cells[0].Value.ToString();
                                double dealerPrice = double.Parse(row.Cells[3].Value.ToString());
                                double userPrice = double.Parse(row.Cells[5].Value.ToString());
                                double retailPrice = double.Parse(row.Cells[7].Value.ToString());
                                bool clearingStock = false;
                                if (row.Cells[9].Value.ToString() == "Yes")
                                    clearingStock = true;
                                else
                                    clearingStock = false;
                                string remarks = row.Cells[10].Value.ToString();
                                string country = row.Cells[11].Value.ToString();
                                int reorderLevel = int.Parse(row.Cells[12].Value.ToString());
                                DateTime effectiveDate;
                                try
                                {
                                    effectiveDate = DateTime.ParseExact(row.Cells[13].Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                catch (Exception ex)
                                {
                                    effectiveDate = DateTime.Parse(DateTime.FromOADate(double.Parse(row.Cells[13].Value.ToString())).ToString("dd/MM/yyyy"));
                                }

                                if (effectiveDate <= DateTime.Now)
                                {
                                    alertMessage("Date must be greater than today.");
                                    LoadSpreadSheet();
                                    return;
                                }

                                ggdal.SubmitPriceForApproval(session.CompanyId, productCode, dealerPrice, userPrice, retailPrice,
                                    session.UserId, remarks, country, reorderLevel, effectiveDate, clearingStock);
                            }
                        }
                        rowIndex++;
                    }

                    string phEmail = this.hidEmail.Value;

                    ProductNoticeViaEmail(productList, "-", phEmail, "Submit", "-");
                    alertMessage("Price Changes are submitted for approval.");
                    LoadSpreadSheet();
                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                }
            }
        }

        private static Worksheet FillWorksheet(DataTable data)
        {
            var workbook = new Workbook();
            var sheet = workbook.AddSheet();
            sheet.Columns = new List<Column>();

            var row = new Row() { Index = 0 };
            int columnIndex = 0;
            foreach (DataColumn dataColumn in data.Columns)
            {
                sheet.Columns.Add(new Column());
                string cellValue = dataColumn.ColumnName;
                var cell = new Cell() { Index = columnIndex++, Value = cellValue, Bold = true };
                row.AddCell(cell);
            }
            sheet.AddRow(row);

            int rowIndex = 1;
            foreach (DataRow dataRow in data.Rows)
            {
                row = new Row() { Index = rowIndex++ };
                columnIndex = 0;
                foreach (DataColumn dataColumn in data.Columns)
                {
                    string cellValue = dataRow[dataColumn.ColumnName].ToString();
                    var cell = new Cell() { Index = columnIndex++, Value = cellValue };
                    row.AddCell(cell);
                }
                sheet.AddRow(row);
            }
            return sheet;
        }
        #endregion
        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //this.RadGrid1.CurrentPageIndex = 0;
            this.RadGrid2.CurrentPageIndex = 0;
            if (ddlSearchStatus.SelectedValue.Trim() != "")
                RetrieveProduct();
            else
                LoadSpreadSheet();

            //this.RadGrid1.DataBind();
            this.RadGrid2.DataBind();
        }
        #endregion

        #region RetrieveProduct
        private void RetrieveProduct()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            string ProductCode = "";
            string ProductName = "";
            string ProductGroupCode = "";
            string ProductGroup = "";
            string status = "";

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Report", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {
                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;


            ProductCode = "%" + txtProductCode.Text.Trim() + "%";
            ProductName = "%" + txtProductName.Text.Trim() + "%";
            ProductGroupCode = "%" + txtProductGroupCode.Text.Trim() + "%";
            ProductGroup = "%" + txtProductGroup.Text.Trim() + "%";
            status = ddlSearchStatus.SelectedValue.Trim();
            resultList.Visible = true;


            try
            {
                ggdal.RetrieveProductPrice(session.CompanyId, ProductGroupCode, ProductGroup, ProductCode, ProductName, loginUserOrAlternateParty, status, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                this.lblSearchSummary.Visible = false;
                if (ddlSearchStatus.SelectedValue.Trim() != "")
                {
                    this.RadButton1.Visible = false;
                    this.RadSpreadsheet1.Visible = false;
                    //this.RadGrid1.DataSource = null;
                    this.RadGrid2.Visible = true;
                    this.RadGrid2.DataSource = ds;
                }
                else
                {
                    this.RadButton1.Visible = true;
                    this.RadSpreadsheet1.Visible = true;
                    //this.RadGrid1.DataSource = ds;
                    this.RadGrid2.Visible = false;
                    this.RadGrid2.DataSource = null;
                }
            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                if (ddlSearchStatus.SelectedValue.Trim() != "")
                {
                    this.RadButton1.Visible = false;
                    this.RadSpreadsheet1.Visible = false;
                    this.RadGrid1.DataSource = null;
                    this.RadGrid2.Visible = false;
                    this.RadGrid2.DataSource = null;
                }
                else
                {
                    this.RadButton1.Visible = false;
                    this.RadSpreadsheet1.Visible = false;
                    this.RadGrid1.DataSource = null;
                    this.RadGrid2.Visible = false;
                    this.RadGrid2.DataSource = null;
                }
            }
        }
        #endregion
        #region Current Price list Rad grid
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RetrieveProduct();
        }

        protected void radGrid_OnPageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveProduct();
        }

        protected void radGrid_OnPageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageSize;
            RetrieveProduct();
        }

        protected void radGrid_OnCancel(object source, GridCommandEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.MasterTableView.ClearEditItems();
            RetrieveProduct();
        }

        protected void RadGrid1_OnUpdateCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            GridEditableItem item = (GridEditableItem)e.Item;
            String productCode = item.GetDataKeyValue("ProductCode").ToString();
            TextBox txtDealerPrice = (TextBox)item["DealerPrice"].FindControl("txtDealerPrice");
            TextBox txtUserPrice = (TextBox)item["UserPrice"].FindControl("txtUserPrice");
            TextBox txtRetailPrice = (TextBox)item["RetailPrice"].FindControl("txtRetailPrice");
            TextBox txtReorderLevel = (TextBox)item["ReorderLevel"].FindControl("txtReorderLevel");
            CheckBox chkbxTradingStock = (CheckBox)item["TradingStock"].FindControl("chkbxTradingStock");
            TextBox txtEffectiveDate = (TextBox)item["EffectiveDate"].FindControl("txtEffectiveDate");
            try
            {
                ProductPrice pp_new = new ProductPrice();
                ProductPrice pp = ProductPrice.RetrieveByKey(session.CompanyId, productCode);
                Product p = Product.RetrieveByKey(session.CompanyId, productCode);
                pp_new.CoyID = pp.CoyID;
                pp_new.ProductCode = p.ProductCode;
                pp_new.ProductGroupCode = p.ProductGroupCode;
                pp_new.DealerPrice = decimal.Parse(txtDealerPrice.Text.Trim());
                pp_new.UserPrice = decimal.Parse(txtUserPrice.Text.Trim());
                pp_new.RetailPrice = decimal.Parse(txtRetailPrice.Text.Trim());
                pp_new.UpdatedBy = session.UserId;
                pp_new.UpdatedDate = DateTime.Now;
                pp_new.IsExpired = false;
                pp_new.ReorderLevel = int.Parse(txtReorderLevel.Text.Trim());
                pp_new.TradingStock = chkbxTradingStock.Checked;
                pp_new.EffectiveDate = DateTime.Parse(txtEffectiveDate.Text.Trim());
                pp_new.Status = "Pending";
                pp_new.Save();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Price changed is submitted for approval.')", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Fail to submit for approval.')", true);
                return;
            }
            RetrieveProduct();
        }
        #endregion

        #region Pending Price list Rad grid
        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RetrieveProduct();
        }

        protected void radGrid2_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                LinkButton lnkApprove = (LinkButton)item.FindControl("lnkApprove");
                LinkButton lnkReject = (LinkButton)item.FindControl("lnkReject");
                LinkButton lnkDelete = (LinkButton)item.FindControl("lnkDelete2");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");

                if (lnkApprove != null && lnkReject != null)
                {
                    if (isMgt)
                    {
                        //RadGrid2.MasterTableView.GetColumn("Function").Display = true;
                        lnkReject.Visible = true;
                        lnkApprove.Visible = true;
                    }
                }
            }
        }

        protected void radGrid2_OnPageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveProduct();
        }

        protected void radGrid2_OnPageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageSize;
            RetrieveProduct();
        }

        protected void radGrid2_OnCancel(object source, GridCommandEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.MasterTableView.ClearEditItems();
            RetrieveProduct();
        }

        protected void RadGrid2_OnDeleteCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.Item is GridDataItem)
            {
                try
                {
                    GridDataItem item = e.Item as GridDataItem;
                    string productCode = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ProductCode"].ToString();

                    ProductPrice pp = ProductPrice.RetrieveByKeyPending(session.CompanyId, productCode);
                    pp.Delete();
                    pp.Resync();
                    alertMessage("Price submitted is deleted.");
                    this.RadGrid2.Rebind();
                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                    return;
                }
            }
        }


        #region lnkApprove_Click
        protected void lnkApprove_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            LinkButton btn = (LinkButton)(sender);
            string[] arg = new string[4];
            arg = btn.CommandArgument.ToString().Split(',');
            string email = arg[0];
            string productGroupName = arg[1];
            string productCode = arg[2];
            string pmName = arg[3];
            try
            {
                //ProductPrice pp = ProductPrice.RetrieveByKey(session.CompanyId, hidProductCode);
                //pp.IsExpired = true;
                //pp.Save();

                ProductPrice pp_new = ProductPrice.RetrieveByKeyPending(session.CompanyId, productCode);
                pp_new.Status = "";
                pp_new.Save();
                alertMessage("Price is updated.");
                ProductNoticeViaEmail(productCode, productGroupName, email, "Approved", pmName);
                RetrieveProduct();
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                return;
            }
        }
        #endregion

        #region lnkReject_Click
        protected void lnkReject_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            LinkButton btn = (LinkButton)(sender);
            string[] arg = new string[4];
            arg = btn.CommandArgument.ToString().Split(',');
            string email = arg[0];
            string productGroupName = arg[1];
            string productCode = arg[2];
            string pmName = arg[3];
            try
            {
                ProductPrice pp_new = ProductPrice.RetrieveByKeyPending(session.CompanyId, productCode);
                pp_new.Status = "Rejected";
                pp_new.Save();

                ProductNoticeViaEmail(productCode, productGroupName, email, "Reject", pmName);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('An email has been sent to " + email + ".')", true);

                RetrieveProduct();
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                return;
            }
        }
        #endregion

        private void ProductNoticeViaEmail(string productCode, string productGroupCode, string userEmail, string type, string name)
        {
            try
            {
                new GMSGeneralDALC().SendPriceEmail(userEmail, productCode, productGroupCode, type, name);
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString());
            }
        }

        #region RejectViaEmail
        private void RejectViaEmail(string productGroup, string productCode, string userRealName, string userEmail)
        {
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);
            mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[GMS - Price Rejected]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Hi " + userRealName + ",</p>\n" +
                        "<p>Your submitted price has been rejected.</p>\n" +
                        "<p>Brand/Product: " + productGroup + "<br />\n" +
                        "Item Code: " + productCode + "</p>\n" +
                        "<br />" +
                        "***** This is a system-generated email. Please do not reply.*****";
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
        #endregion
        #endregion

        protected void alertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }
    }
}