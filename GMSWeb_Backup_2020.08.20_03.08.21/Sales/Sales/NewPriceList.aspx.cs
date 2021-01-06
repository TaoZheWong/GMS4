using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GMSWeb.Sales.Sales
{
    public partial class NewPriceList : GMSBasePage
    {
        string currentLink = "Products";
        private HSSFWorkbook hssfworkbook;
        protected void Page_Load(object sender, EventArgs e)
        {
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
                                                                            163);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                           163);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));
            

            if (!IsPostBack)
            {
                LoadData();
            }

            string javaScript =
            @"<script type=""text/javascript"">
		    function toggleAccessRow(n)
		    {
			    if( document.getElementById(""rppToggle_"" + n) )
			    {
				    var current = document.getElementById(""rppToggle_"" + n).style.display;
				    document.getElementById(""rppToggle_"" + n).style.display = (current == null || current == ""none"")?"""":""none"";
				    document[""imgAccessBox_"" + n].src = (current == null || current == ""none"")? sDOMAIN+""/App_Themes/Default/images/checkCloseIcon.gif"" : sDOMAIN+""/App_Themes/Default/images/checkOpenIcon.gif"";
			    }
		    }
		    </script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet ds1 = new DataSet();
            ggdal.RetrieveProductBrand(ref ds1);
            ds1.Tables[0].Rows.Add(0, "OthWS", "OthWS");
            this.ddlSearchBrandName.DataSource = ds1.Tables[0];
            this.ddlSearchBrandName.DataBind();
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.DataGrid1.CurrentPageIndex = 0;
            //this.btnExport.Visible = true;
            RetrieveProduct();
        }
        #endregion

        #region btnExport_Click
        protected void btnExport_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            hssfworkbook = new HSSFWorkbook();
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");
            IRow row;
            //create header in excel
            row = sheet1.CreateRow(0);
            row.CreateCell(0).SetCellValue("Product With Sales");
            row = sheet1.CreateRow(1);
            row.CreateCell(0).SetCellValue("SN");
            row.CreateCell(1).SetCellValue("Model No");
            row.CreateCell(2).SetCellValue("Product Type");
            row.CreateCell(3).SetCellValue("Product Description");
            row.CreateCell(4).SetCellValue("Product Code ");
            row.CreateCell(5).SetCellValue("User Price");
            /*
             //exclude for sales person
             UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                163);
            if (uAccess != null){
                row.CreateCell(5).SetCellValue("Average Cost");
                row.CreateCell(6).SetCellValue("Dealer Price");
                row.CreateCell(7).SetCellValue("D Percentage");
                row.CreateCell(8).SetCellValue("User Price");
                row.CreateCell(9).SetCellValue("U Percentage");
                row.CreateCell(10).SetCellValue("Retail Price");
                row.CreateCell(11).SetCellValue("R Percentage");
                row.CreateCell(12).SetCellValue("Avg Selling Price (12 mths)");
                row.CreateCell(13).SetCellValue("Sales LTM (in 000s)");
                row.CreateCell(14).SetCellValue("Stock Balance");
                row.CreateCell(15).SetCellValue("Updated On");
                //row.CreateCell(6).SetCellValue("Updated On");
            }
            else{
                //row.CreateCell(5).SetCellValue("Dealer Price");
                row.CreateCell(5).SetCellValue("User Price");
                //row.CreateCell(7).SetCellValue("Retail Price");
            }*/
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            string ProductCode = "";
            string ProductName = "";
            string ProductGroupCode = "";
            string FileBrand = "";
            int Brand = 0;
            if (string.IsNullOrEmpty(txtProductCode.Text.Trim()) && string.IsNullOrEmpty(txtProductName.Text.Trim()) &&
                string.IsNullOrEmpty(ddlSearchBrandName.SelectedValue))
            {
                this.MsgPanel2.ShowMessage("Please input product to export", MessagePanelControl.MessageEnumType.Alert);
            }else{
                ProductCode = "%" + txtProductCode.Text.Trim() + "%";
                ProductName = "%" + txtProductName.Text.Trim() + "%";
                ProductGroupCode = "%" + ddlSearchProductGroup.SelectedValue + "%";
                Brand = int.Parse(ddlSearchBrandName.SelectedValue);
            }
            try
            {
                //ggdal.RetrieveProductPriceWithoutAgeingStock(session.CompanyId, Brand, ProductCode, ProductName, ProductGroupCode, ref ds);
                //ggdal.RetrieveProductPriceAgeingStock(session.CompanyId, Brand, ProductCode, ProductName, ProductGroupCode, ref ds2);
                if (ds != null)
                {
                    int i = 2;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        row = sheet1.CreateRow(i);
                        string si = (i + 1).ToString();
                        FileBrand = dr["FullName"].ToString();
                        row.CreateCell(0).SetCellValue(i-1);
                        row.CreateCell(1).SetCellValue(dr["ModelNo"].ToString());
                        row.CreateCell(2).SetCellValue(dr["FullName"].ToString());
                        row.CreateCell(3).SetCellValue(dr["ProductName"].ToString());
                        row.CreateCell(4).SetCellValue(dr["ProductCode"].ToString());
                        row.CreateCell(5).SetCellValue(dr["UserPrice"].ToString() == "" ? 0 : Double.Parse(dr["UserPrice"].ToString()));

                        /*
                        if (uAccess != null)
                        {
                            string stockBalance;
                            stockBalance = getLiveStockBalance(dr["ProductCode"].ToString());//get live stock balance
                            row.CreateCell(5).SetCellValue(dr["WeightedCost"].ToString() == "" ? 0 : Double.Parse(dr["WeightedCost"].ToString()));
                            row.CreateCell(6).SetCellValue(dr["DealerPrice"].ToString() == "" ? 0 : Double.Parse(dr["DealerPrice"].ToString()));
                            row.CreateCell(7).SetCellValue(dr["DPercent"].ToString() == "" ? 0 : Double.Parse(dr["DPercent"].ToString()));
                            row.CreateCell(8).SetCellValue(dr["UserPrice"].ToString() == "" ? 0 : Double.Parse(dr["UserPrice"].ToString()));
                            row.CreateCell(9).SetCellValue(dr["UPercent"].ToString() == "" ? 0 : Double.Parse(dr["UPercent"].ToString()));
                            row.CreateCell(10).SetCellValue(dr["RetailPrice"].ToString() == "" ? 0 : Double.Parse(dr["RetailPrice"].ToString()));
                            row.CreateCell(11).SetCellValue(dr["RPercent"].ToString() == "" ? 0 : Double.Parse(dr["RPercent"].ToString()));
                            row.CreateCell(12).SetCellValue(dr["averagePrice"].ToString() == "" ? 0 : Double.Parse(dr["averagePrice"].ToString()));
                            row.CreateCell(13).SetCellValue(dr["LTMamount"].ToString() == "" ? 0 : Double.Parse(dr["LTMamount"].ToString()));
                            row.CreateCell(14).SetCellValue(stockBalance.ToString() == "" ? 0 : Double.Parse(stockBalance.ToString()));
                            row.CreateCell(15).SetCellValue(dr["UpdatedDate"].ToString());
                            //row.CreateCell(6).SetCellValue(dr["UpdatedDate"].ToString());
                        }
                        else
                        {
                            //row.CreateCell(5).SetCellValue(dr["DealerPrice"].ToString() == "" ? 0 : Double.Parse(dr["DealerPrice"].ToString()));
                            row.CreateCell(5).SetCellValue(dr["UserPrice"].ToString() == "" ? 0 : Double.Parse(dr["UserPrice"].ToString()));
                            //row.CreateCell(7).SetCellValue(dr["RetailPrice"].ToString() == "" ? 0 : Double.Parse(dr["RetailPrice"].ToString()));
                        }*/
                        i++;
                    }
                    if (ds2 != null)
                    {
                        row = sheet1.CreateRow(i);
                        row.CreateCell(0).SetCellValue("Product No Sales");
                        i++;
                        row = sheet1.CreateRow(i);
                        row.CreateCell(0).SetCellValue("SN");
                        row.CreateCell(1).SetCellValue("Model No");
                        row.CreateCell(2).SetCellValue("Product Type");
                        row.CreateCell(3).SetCellValue("Product Description");
                        row.CreateCell(4).SetCellValue("Product Code ");
                        row.CreateCell(5).SetCellValue("User Price");
                        i++;
                        int j = 1;
                        foreach (DataRow dr in ds2.Tables[0].Rows)
                        {
                            row = sheet1.CreateRow(i);
                            string si = (i + 1).ToString();
                            FileBrand = dr["FullName"].ToString();
                            row.CreateCell(0).SetCellValue(j);
                            row.CreateCell(1).SetCellValue(dr["ModelNo"].ToString());
                            row.CreateCell(2).SetCellValue(dr["FullName"].ToString());
                            row.CreateCell(3).SetCellValue(dr["ProductName"].ToString());
                            row.CreateCell(4).SetCellValue(dr["ProductCode"].ToString());
                            row.CreateCell(5).SetCellValue(dr["UserPrice"].ToString() == "" ? 0 : Double.Parse(dr["UserPrice"].ToString()));

                            i++;
                            j++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }
            string fileName = "PriceList_"+FileBrand+"_" + DateTime.Now.Date.ToShortDateString() + ".xls";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/vnd.ms-excel";
            GetExcelStream().WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
        }
        #endregion

        MemoryStream GetExcelStream()
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }

        #region DataGrid1 PageIndexChanged event handling
        protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveProduct();
        }
        #endregion

        #region DataGrid1_ItemDataBound
        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewProductGroup = (DropDownList)e.Item.FindControl("ddlNewProductGroup");
                if (ddlNewProductGroup != null)
                {
                    GMSGeneralDALC ggdal = new GMSGeneralDALC();
                    DataSet ds1 = new DataSet();
                    ggdal.RetrieveProductGroup(session.CompanyId, session.UserId, ref ds1);
                    ddlNewProductGroup.DataSource = ds1.Tables[0];
                    ddlNewProductGroup.DataBind();
                    //ddlNewTeamName.SelectedValue = lstSalesGroupTeam.
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");

                //foreach (DataGridItem item in DataGrid1.Items)
                //{   //add id for each row for drag & drop to save order
                //    hidProductCode = (HtmlInputHidden)item.FindControl("hidProductCode");
                //    item.Attributes.Add("id", hidProductCode.Value.ToString());
                //}
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete2");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");

                //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                //                                                                163);
                
                //if (uAccess == null)
                //{   //disable column & edit function for sales person
                //    this.DataGrid1.ShowFooter = false;
                //   this.DataGrid1.Attributes.Add("style", "pointer-events: none");
                //    this.DataGrid1.Columns[5].Visible = false;
                //    this.DataGrid1.Columns[6].Visible = false;
                //    this.DataGrid1.Columns[7].Visible = false;
                //    this.DataGrid1.Columns[9].Visible = false;
                //    this.DataGrid1.Columns[10].Visible = false;
                //    this.DataGrid1.Columns[11].Visible = false;
                //    this.DataGrid1.Columns[12].Visible = false;
                //    this.DataGrid1.Columns[13].Visible = false;
                //    this.DataGrid1.Columns[14].Visible = false;
                //    this.DataGrid1.Columns[15].Visible = false;
                //    this.DataGrid1.Columns[16].Visible = false;
                //}
                //else
                //{   //get live stock balance only when needed
                //    Label lblStockStatus = (Label)e.Item.FindControl("lblStockStatus");
                //    lblStockStatus.Text = getLiveStockBalance(hidProductCode.Value.Trim());
                //}
            }
        }
        #endregion

        #region DataGrid1_EditCommand
        protected void DataGrid1_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.DataGrid1.EditItemIndex = e.Item.ItemIndex;
            RetrieveProduct();
        }
        #endregion

        #region DataGrid1_CancelCommand
        protected void DataGrid1_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.DataGrid1.EditItemIndex = -1;
            RetrieveProduct();
        }
        #endregion

        #region DataGrid1_CreateCommand
        protected void DataGrid1_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage(currentLink));
                    return;
                }

                DropDownList ddlNewProduct = (DropDownList)e.Item.FindControl("ddlNewProduct");
                TextBox txtNewDealerPrice = (TextBox)e.Item.FindControl("txtNewDealerPrice");
                TextBox txtNewUserPrice = (TextBox)e.Item.FindControl("txtNewUserPrice");
                TextBox txtNewRetailPrice = (TextBox)e.Item.FindControl("txtNewRetailPrice");

                TextBox txtNewModelNo = (TextBox)e.Item.FindControl("txtNewModelNo");
                TextBox txtNewProductType = (TextBox)e.Item.FindControl("txtNewProductType");
                TextBox txtNewReorderLevel = (TextBox)e.Item.FindControl("txtNewReorderLevel");
                TextBox txtNewEffectiveDate = (TextBox)e.Item.FindControl("txtNewEffectiveDate");
                CheckBox chkbxNewTradingStock = (CheckBox)e.Item.FindControl("chkbxNewTradingStock");
                CheckBox chkbxNewInactive = (CheckBox)e.Item.FindControl("chkbxNewInactive");

                if (ddlNewProduct != null && !string.IsNullOrEmpty(ddlNewProduct.SelectedValue) &&
                     txtNewDealerPrice != null && !string.IsNullOrEmpty(txtNewDealerPrice.Text)&&
                     txtNewUserPrice != null && !string.IsNullOrEmpty(txtNewUserPrice.Text)&&
                     txtNewRetailPrice != null && !string.IsNullOrEmpty(txtNewRetailPrice.Text))
                {
                    try
                    {
                        Product product = Product.RetrieveByKey(session.CompanyId, ddlNewProduct.SelectedValue);
                        ProductPrice pp_old = ProductPrice.RetrieveByKey(session.CompanyId, ddlNewProduct.SelectedValue);
                        if(pp_old != null)
                        {
                            pp_old.IsExpired = true;
                            pp_old.Save();
                        }
                        ProductPrice pp = new ProductPrice();
                        pp.CoyID = session.CompanyId;
                        pp.ProductCode = ddlNewProduct.SelectedValue;
                        pp.ProductGroupCode = product.ProductGroupCode;
                        pp.WeightedCost = Decimal.Parse(product.WeightedCost.ToString());
                        pp.DealerPrice = decimal.Parse(txtNewDealerPrice.Text.Trim());
                        pp.UserPrice = decimal.Parse(txtNewUserPrice.Text.Trim());
                        pp.RetailPrice = decimal.Parse(txtNewRetailPrice.Text.Trim());
                        pp.UpdatedBy = session.UserId;
                        pp.UpdatedDate = DateTime.Now;
                        pp.IsExpired = false;
                       
                        GMSGeneralDALC ggdal = new GMSGeneralDALC();
                        ggdal.UpdateProductDetail(ddlNewProduct.SelectedValue, chkbxNewTradingStock.Checked, Int32.Parse(txtNewReorderLevel.Text.Trim()),
                            txtNewProductType.Text.Trim(), txtNewModelNo.Text.Trim(), DateTime.Parse(txtNewEffectiveDate.Text.Trim()), chkbxNewInactive.Checked);

                        pp.Save();
                        this.PageMsgPanel.ShowMessage("New product price inserted.", MessagePanelControl.MessageEnumType.Alert);
                        RetrieveProduct();
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region DataGrid1_UpdateCommand
        protected void DataGrid1_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            TextBox txtEditModelNo = (TextBox)e.Item.FindControl("txtEditModelNo");
            TextBox txtEditProductType = (TextBox)e.Item.FindControl("txtEditProductType");
            TextBox txtEditDealerPrice = (TextBox)e.Item.FindControl("txtEditDealerPrice");
            TextBox txtEditUserPrice = (TextBox)e.Item.FindControl("txtEditUserPrice");
            TextBox txtEditRetailPrice = (TextBox)e.Item.FindControl("txtEditRetailPrice");
            TextBox txtEditReorderLevel = (TextBox)e.Item.FindControl("txtEditReorderLevel");
            TextBox txtEditEffectiveDate = (TextBox)e.Item.FindControl("txtEditEffectiveDate");
            CheckBox chkbxEditTradingStock = (CheckBox)e.Item.FindControl("chkbxEditTradingStock");
            CheckBox chkbxEditInactive = (CheckBox)e.Item.FindControl("chkbxEditInactive");
            HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");
            Label lblUpdatedDateEdit = (Label)e.Item.FindControl("lblUpdatedDateEdit");

            if (hidProductCode != null && txtEditDealerPrice != null && !string.IsNullOrEmpty(txtEditDealerPrice.Text)
                 && txtEditUserPrice != null && !string.IsNullOrEmpty(txtEditUserPrice.Text)
                 && txtEditRetailPrice != null && !string.IsNullOrEmpty(txtEditRetailPrice.Text)
                 && txtEditModelNo != null && !string.IsNullOrEmpty(txtEditModelNo.Text)
                 && txtEditProductType != null && !string.IsNullOrEmpty(txtEditProductType.Text)
                 && txtEditReorderLevel != null && !string.IsNullOrEmpty(txtEditReorderLevel.Text))
            {
                //Product product = Product.RetrieveByKey(session.CompanyId, hidProductCode.Value.ToString());
                ProductPrice pp = ProductPrice.RetrieveByKey(session.CompanyId, hidProductCode.Value.ToString());
                //pp.WeightedCost = (Decimal)product.WeightedCost;
                pp.DealerPrice = decimal.Parse(txtEditDealerPrice.Text.Trim());
                pp.UserPrice = decimal.Parse(txtEditUserPrice.Text.Trim());
                pp.RetailPrice = decimal.Parse(txtEditRetailPrice.Text.Trim());
                pp.UpdatedBy = session.UserId;
                pp.UpdatedDate = DateTime.Now;

                try
                {
                    GMSGeneralDALC ggdal = new GMSGeneralDALC();
                    ggdal.UpdateProductDetail(hidProductCode.Value.ToString(), chkbxEditTradingStock.Checked, Int32.Parse(txtEditReorderLevel.Text.Trim()),
                        txtEditProductType.Text.Trim(), txtEditModelNo.Text.Trim(), DateTime.Parse(txtEditEffectiveDate.Text.Trim()), chkbxEditInactive.Checked);

                    pp.Save();
                    this.DataGrid1.EditItemIndex = -1;
                    RetrieveProduct();
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
        }
        #endregion

        #region DataGrid1_DeleteCommand
        protected void DataGrid1_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage(currentLink));
                    return;
                }

                HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");
                Label lblUpdatedDate = (Label)e.Item.FindControl("lblUpdatedDate");

                if (hidProductCode != null)
                {
                    try
                    {
                        ProductPrice pp = ProductPrice.RetrieveByKey(session.CompanyId, hidProductCode.Value.ToString());
                        pp.Delete();
                        pp.Resync();
                        ProductPrice pp_old = ProductPrice.RetrieveByKeyExpired(session.CompanyId, hidProductCode.Value.ToString());
                        if (pp_old != null)
                        {
                            pp_old.IsExpired = false;
                            pp_old.Save();
                        }
                        this.DataGrid1.EditItemIndex = -1;
                        RetrieveProduct();
                    }
                    catch (SqlException exSql)
                    {
                        this.PageMsgPanel.ShowMessage(exSql.Message, MessagePanelControl.MessageEnumType.Alert);
                        RetrieveProduct();
                        return;
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        RetrieveProduct();
                        return;
                    }
                }
            }
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
            //int Brand =0 ;
            
                ProductCode = "%" + txtProductCode.Text.Trim() + "%";
                ProductName = "%" + txtProductName.Text.Trim() + "%";
                ProductGroupCode = "%" + txtProductGroupCode.Text.Trim() + "%";
                ProductGroup = "%" + txtProductGroup.Text.Trim() + "%";
                resultList.Visible = true;
                //Brand = int.Parse(ddlSearchBrandName.SelectedValue);
                //resultList2.Visible = true;
                if(string.IsNullOrEmpty(txtProductGroupCode.Text.Trim())&& string.IsNullOrEmpty(txtProductGroup.Text.Trim()))
                {
                    this.PageMsgPanel.ShowMessage("Please input brand/product code or name to search", MessagePanelControl.MessageEnumType.Alert);
                    return;
                }    

            try
            {
                //ggdal.RetrieveProductPrice(session.CompanyId, ProductGroupCode, ProductGroup, ProductCode, ProductName,session.UserId,ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }
            
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                this.lblSearchSummary.Text = ds.Tables[0].Rows.Count.ToString()+ " Results";
                this.lblSearchSummary.Visible = true;
                this.DataGrid1.Visible = true;
                this.DataGrid1.DataSource = ds;
                this.DataGrid1.DataBind();
            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                this.DataGrid1.Visible = true;
                this.DataGrid1.DataSource = null;
                this.DataGrid1.DataBind();
            }
            //try
            //{
            //    ggdal.RetrieveProductPriceAgeingStock(session.CompanyId, Brand, ProductCode, ProductName, ProductGroupCode, ref ds2);
            //}
            //catch (Exception ex)
            //{
            //    this.PageMsgPanel2.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            //}
            //if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
            //{
            //    this.lblSearchSummary2.Text = ds2.Tables[0].Rows.Count.ToString() + " Results (Product No Sales)";
            //    this.lblSearchSummary2.Visible = true;
            //    this.DataGrid2.Visible = true;
            //    this.DataGrid2.DataSource = ds2;
            //    this.DataGrid2.DataBind();
            //}
            //else
            //{
            //    this.lblSearchSummary2.Text = "No records.";
            //    this.lblSearchSummary2.Visible = true;
            //    this.DataGrid2.Visible = true;
            //    this.DataGrid2.DataSource = null;
            //    this.DataGrid2.DataBind();
            //    return;
            //}
        }
        #endregion

        #region ddlNewProductGroup_SelectedIndexChanged
        protected void ddlNewProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            DropDownList ddlNewProductGroup = (DropDownList)sender;
            string productGroup = ddlNewProductGroup.SelectedValue;

            ProductsDataDALC ppdac = new ProductsDataDALC();
            TableRow tr = (TableRow)ddlNewProductGroup.Parent.Parent;
            DropDownList ddlNewProduct = (DropDownList)tr.FindControl("ddlNewProduct");
            if (ddlNewProduct != null)
            {
                DataSet dsProducts = new DataSet();
                ppdac.GetProductByProductGroup(session.CompanyId, productGroup, ref dsProducts);
                ddlNewProduct.DataSource = dsProducts.Tables[0];
                ddlNewProduct.DataBind();
            }
        }
        #endregion

        #region ddlSearchBrandName_SelectedIndexChanged
        protected void ddlSearchBrandName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            DropDownList ddlSearchBrandName = (DropDownList)sender;
            int brand = int.Parse(ddlSearchBrandName.SelectedValue);

            if (ddlSearchBrandName.SelectedValue.Equals("0")|| ddlSearchBrandName.SelectedValue.Equals("62"))
            {
                GMSGeneralDALC ggdal = new GMSGeneralDALC();
                DataSet ds = new DataSet();
                ggdal.RetrieveProductPriceProductGroup(session.CompanyId, brand,ref ds);
                this.ddlSearchProductGroup.DataSource = ds.Tables[0];
                this.ddlSearchProductGroup.DataBind();
                this.hiddenLabel.Visible = true;
            }
            else
            {
                this.hiddenLabel.Visible = false;
            }
        }
        #endregion

        #region Drag and Drop GridViewReorders
        [WebMethod]
        public static void GridViewReorders(string Reorder)
        {
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            string[] ListID = Reorder.Split('|');
            for (int i = 0; i < ListID.Length; i++){
                if (ListID[i] != "" && ListID[i] != null)
                {
                    ggdal.UpdateProductPriceOrder(ListID[i], i+1);
                }
            }
        }
        #endregion

        #region GetProductLiveStockBalance
        private string getLiveStockBalance(string productCode)
        {
            LogSession session = base.GetSessionInfo();
            string stockBalanceText = "";
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            try
            {
                if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";

                    ds = sc.GetProductFullDetail(session.CompanyId, productCode, "%%", "%%", "%%");
                }
                else if (session.StatusType.ToString() == "L")
                {
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    if (session.StatusType.ToString() == "L")
                        ds = sc1.GetProductFullDetail(productCode, "%%", "%%", "%%", "%%");

                    if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.GASLMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    if (session.StatusType.ToString() == "L" && session.GASLMSWebServiceAddress.Trim() != "")
                        ds = sc1.GetProductFullDetail(productCode, "%%", "%%", "%%", "%%");

                    if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    if (session.StatusType.ToString() == "L" && session.WSDLMSWebServiceAddress.Trim() != "")
                        ds1 = sc1.GetProductFullDetail(productCode, "%%", "%%", "%%", "%%");
                }
                else if (session.StatusType.ToString() == "S")
                {
                    string query = "CALL \"AF_API_GET_SAP_ITEMMASTERINFO\" ('" + productCode + "', '', '', '', '', '')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "ProductCode", "ProductName", "ProductGroupCode", "Volume", "UOM", "WeightedCost", "OnOrderQuantity", "OnPOQuantity", "OnBOQuantity", "AvailableQuantity", "IsGasDivision", "IsWeldingDivision", "ProdForeignName", "TrackedByBatch", "TrackedBySerial", "ProductNotes", "IsActive", "ItemType", "ProductGroupName", "OnHandQuantity",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    DataView dvOri = new DataView(ds.Tables[0]);
                    dvOri.RowFilter = "ProductName NOT LIKE '%DO NOT USE%'";
                    DataTable dtOri = dvOri.ToTable();
                    ds.Reset();
                    ds.Tables.Add(dtOri);
                }
            }
            catch (Exception ex)
            {
                this.JScriptAlertMsg(ex.Message);
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["OnHandQuantity"].ToString() != "0" && Decimal.Round(Convert.ToDecimal(dr["OnHandQuantity"]), 6).ToString() != "0.000000")
                        stockBalanceText = stockBalanceText + Decimal.Round(Convert.ToDecimal(dr["OnHandQuantity"]), 6).ToString() + "<br>";
                }

                if (stockBalanceText.Length >= 4)
                    stockBalanceText = stockBalanceText.Substring(0, stockBalanceText.Length - 4);

                if (string.IsNullOrEmpty(stockBalanceText))
                    stockBalanceText = "0";
            }
            return stockBalanceText;
        }
        #endregion

        //#region DataGrid2_ItemDataBound
        //protected void DataGrid2_ItemDataBound(object sender, DataGridItemEventArgs e)
        //{
        //    LogSession session = base.GetSessionInfo();

        //    if (e.Item.ItemType == ListItemType.Footer)
        //    {
        //        DropDownList ddlNewBrand = (DropDownList)e.Item.FindControl("ddlNewBrand");
        //        if (ddlNewBrand != null)
        //        {
        //            GMSGeneralDALC ggdal = new GMSGeneralDALC();
        //            DataSet ds1 = new DataSet();
        //            ggdal.RetrieveProductBrand(ref ds1);
        //            ds1.Tables[0].Rows.Add(0, "OthWS", "OthWS");
        //            ddlNewBrand.DataSource = ds1.Tables[0];
        //            ddlNewBrand.DataBind();
        //            //ddlNewTeamName.SelectedValue = lstSalesGroupTeam.
        //        }
        //    }

        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");

        //        foreach (DataGridItem item in DataGrid2.Items)
        //        {   //add id for each row for drag & drop to save order
        //            hidProductCode = (HtmlInputHidden)item.FindControl("hidProductCode");
        //            item.Attributes.Add("id", hidProductCode.Value.ToString());
        //        }
        //        LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete2");
        //        if (lnkDelete != null)
        //            lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");

        //        UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
        //                                                                        163);

        //        if (uAccess == null)
        //        {   //disable column & edit function for sales person
        //            this.DataGrid2.ShowFooter = false;
        //            this.DataGrid2.Attributes.Add("style", "pointer-events: none");
        //            this.DataGrid2.Columns[5].Visible = false;
        //            this.DataGrid2.Columns[6].Visible = false;
        //            this.DataGrid2.Columns[7].Visible = false;
        //            this.DataGrid2.Columns[9].Visible = false;
        //            this.DataGrid2.Columns[10].Visible = false;
        //            this.DataGrid2.Columns[11].Visible = false;
        //            this.DataGrid2.Columns[12].Visible = false;
        //            this.DataGrid2.Columns[13].Visible = false;
        //            this.DataGrid2.Columns[14].Visible = false;
        //            this.DataGrid2.Columns[15].Visible = false;
        //            this.DataGrid2.Columns[16].Visible = false;
        //        }
        //    }
        //}
        //#endregion

        //#region DataGrid2_EditCommand
        //protected void DataGrid2_EditCommand(object sender, DataGridCommandEventArgs e)
        //{
        //    this.DataGrid2.EditItemIndex = e.Item.ItemIndex;
        //    RetrieveProduct();
        //}
        //#endregion

        //#region DataGrid2_CancelCommand
        //protected void DataGrid2_CancelCommand(object sender, DataGridCommandEventArgs e)
        //{
        //    this.DataGrid2.EditItemIndex = -1;
        //    RetrieveProduct();
        //}
        //#endregion

        //#region DataGrid2_CreateCommand
        //protected void DataGrid2_CreateCommand(object sender, DataGridCommandEventArgs e)
        //{
        //    if (e.CommandName == "Create")
        //    {
        //        LogSession session = base.GetSessionInfo();
        //        if (session == null)
        //        {
        //            Response.Redirect(base.SessionTimeOutPage("Sales"));
        //            return;
        //        }

        //        UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
        //                                                                        163);
        //        if (uAccess == null)
        //        {
        //            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "You don't have access." + "\");</script>", false);
        //            return;
        //        }

        //        DropDownList ddlNewProduct = (DropDownList)e.Item.FindControl("ddlNewProduct");
        //        TextBox txtNewDealerPrice = (TextBox)e.Item.FindControl("txtNewDealerPrice");
        //        TextBox txtNewUserPrice = (TextBox)e.Item.FindControl("txtNewUserPrice");
        //        TextBox txtNewRetailPrice = (TextBox)e.Item.FindControl("txtNewRetailPrice");

        //        if (ddlNewProduct != null && !string.IsNullOrEmpty(ddlNewProduct.SelectedValue) &&
        //             txtNewDealerPrice != null && !string.IsNullOrEmpty(txtNewDealerPrice.Text) &&
        //             txtNewUserPrice != null && !string.IsNullOrEmpty(txtNewUserPrice.Text) &&
        //             txtNewRetailPrice != null && !string.IsNullOrEmpty(txtNewRetailPrice.Text))
        //        {
        //            try
        //            {
        //                Product product = Product.RetrieveByKey(session.CompanyId, ddlNewProduct.SelectedValue);
        //                ProductPrice pp_old = ProductPrice.RetrieveByKey(session.CompanyId, ddlNewProduct.SelectedValue);
        //                if (pp_old != null)
        //                {
        //                    pp_old.IsExpired = true;
        //                    pp_old.Save();
        //                }
        //                ProductPrice pp = new ProductPrice();
        //                pp.CoyID = session.CompanyId;
        //                pp.ProductCode = ddlNewProduct.SelectedValue;
        //                pp.WeightedCost = Decimal.Parse(product.WeightedCost.ToString());
        //                pp.DealerPrice = decimal.Parse(txtNewDealerPrice.Text.Trim());
        //                pp.UserPrice = decimal.Parse(txtNewUserPrice.Text.Trim());
        //                pp.RetailPrice = decimal.Parse(txtNewRetailPrice.Text.Trim());
        //                pp.UpdatedBy = session.UserId;
        //                pp.UpdatedDate = DateTime.Now;
        //                pp.IsExpired = false;
        //                pp.Save();
        //                RetrieveProduct();
        //            }
        //            catch (Exception ex)
        //            {
        //                this.PageMsgPanel2.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
        //                return;
        //            }
        //        }
        //    }
        //}
        //#endregion

        //#region DataGrid2_UpdateCommand
        //protected void DataGrid2_UpdateCommand(object sender, DataGridCommandEventArgs e)
        //{
        //    LogSession session = base.GetSessionInfo();
        //    if (session == null)
        //    {
        //        Response.Redirect(base.SessionTimeOutPage("Sales"));
        //        return;
        //    }
        //    UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
        //                                                                    153);
        //    if (uAccess == null)
        //        Response.Redirect(base.UnauthorizedPage("Sales"));

        //    TextBox txtEditDealerPrice = (TextBox)e.Item.FindControl("txtEditDealerPrice");
        //    TextBox txtEditUserPrice = (TextBox)e.Item.FindControl("txtEditUserPrice");
        //    TextBox txtEditRetailPrice = (TextBox)e.Item.FindControl("txtEditRetailPrice");
        //    HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");
        //    Label lblUpdatedDateEdit = (Label)e.Item.FindControl("lblUpdatedDateEdit");

        //    if (hidProductCode != null && txtEditDealerPrice != null && !string.IsNullOrEmpty(txtEditDealerPrice.Text)
        //         && txtEditUserPrice != null && !string.IsNullOrEmpty(txtEditUserPrice.Text)
        //         && txtEditRetailPrice != null && !string.IsNullOrEmpty(txtEditRetailPrice.Text))
        //    {
        //        Product product = Product.RetrieveByKey(session.CompanyId, hidProductCode.Value.ToString());
        //        ProductPrice pp = ProductPrice.RetrieveByKey(session.CompanyId, hidProductCode.Value.ToString());
        //        pp.WeightedCost = (Decimal)product.WeightedCost;
        //        pp.DealerPrice = decimal.Parse(txtEditDealerPrice.Text.Trim());
        //        pp.UserPrice = decimal.Parse(txtEditUserPrice.Text.Trim());
        //        pp.RetailPrice = decimal.Parse(txtEditRetailPrice.Text.Trim());
        //        pp.UpdatedBy = session.UserId;
        //        pp.UpdatedDate = DateTime.Now;
        //        try
        //        {
        //            pp.Save();
        //            this.DataGrid2.EditItemIndex = -1;
        //            RetrieveProduct();
        //        }
        //        catch (Exception ex)
        //        {
        //            this.PageMsgPanel2.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
        //            return;
        //        }
        //    }
        //}
        //#endregion

        //#region DataGrid2_DeleteCommand
        //protected void DataGrid2_DeleteCommand(object sender, DataGridCommandEventArgs e)
        //{
        //    if (e.CommandName == "Delete")
        //    {
        //        LogSession session = base.GetSessionInfo();
        //        if (session == null)
        //        {
        //            Response.Redirect(base.SessionTimeOutPage("Sales"));
        //            return;
        //        }
        //        UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
        //                                                                        153);
        //        if (uAccess == null)
        //            Response.Redirect(base.UnauthorizedPage("Sales"));

        //        HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");
        //        Label lblUpdatedDate = (Label)e.Item.FindControl("lblUpdatedDate");

        //        if (hidProductCode != null)
        //        {
        //            try
        //            {
        //                ProductPrice pp = ProductPrice.RetrieveByKey(session.CompanyId, hidProductCode.Value.ToString());
        //                pp.Delete();
        //                pp.Resync();
        //                ProductPrice pp_old = ProductPrice.RetrieveByKeyExpired(session.CompanyId, hidProductCode.Value.ToString());
        //                if (pp_old != null)
        //                {
        //                    pp_old.IsExpired = false;
        //                    pp_old.Save();
        //                }
        //                this.DataGrid2.EditItemIndex = -1;
        //                RetrieveProduct();
        //            }
        //            catch (SqlException exSql)
        //            {
        //                this.PageMsgPanel2.ShowMessage(exSql.Message, MessagePanelControl.MessageEnumType.Alert);
        //                RetrieveProduct();
        //                return;
        //            }
        //            catch (Exception ex)
        //            {
        //                this.PageMsgPanel2.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
        //                RetrieveProduct();
        //                return;
        //            }
        //        }
        //    }
        //}
        //#endregion
    }
}