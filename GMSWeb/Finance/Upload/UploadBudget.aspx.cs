using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

namespace GMSWeb.Finance.Upload
{
	public partial class UploadBudget : GMSBasePage
	{
        private HSSFWorkbook hssfworkbook;
        protected short loginUserOrAlternateParty = 0;
        string[] itemForThick = { 
                        "(PNL)Total Sales", "(PNL)Total Cost of Sales", "(PNL)Gross Profit", 
                        "(PNL)Total Other Op Income/ (Exp)" ,"(PNL)Total S&D Expenses",
                        "(PNL)A&G Direct Expenses","(PNL)Total A&G Expenses","(PNL)Other A&G Expenses","(PNL)Total Non-Op Income/(Exp)",
                        "(PNL)Profit Before Taxation","(PNL)Less: Taxation","Performance Indicators",
                        "Profit After Taxation %"
                    };
        string[] itemForDouble = { "(PNL)Gross Profit", "(PNL)Profit from Operations", "(PNL)Profit After Taxation", "(PNL)Dividend Declared" };

		protected void Page_Load(object sender, EventArgs e)
		{
			Master.setCurrentLink("CompanyFinance"); 
			LogSession session = base.GetSessionInfo();
			if (session == null)
			{
				Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
				return;
			}
			UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
																			14);
			if (uAccess == null)
				Response.Redirect(base.UnauthorizedPage("CompanyFinance"));


			if (!Page.IsPostBack)
			{
				LoadDDLs();
                LoadDownloadDDLs();
			}

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Finance Report", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {

                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;


			string javaScript =
			@"
			<script type=""text/javascript"">

			function SelectPurpose(dropDownList)
			{
				var prefix = dropDownList.id.substring(0,dropDownList.id.lastIndexOf(""_"")+1);

				var e = document.getElementById(prefix+""ddlPurpose""); 
				var purposeText = e.options[e.selectedIndex].text; 

			   
				
				if (purposeText == ""Balance Sheet"" || purposeText == ""Product"" || 
					purposeText == ""Listing Of Expenses (S & D)"" || purposeText == ""Listing Of Expenses (G & A)"")
				{
					document.getElementById(prefix+""ddlProject"").disabled = true;                    
				} else
				{
					document.getElementById(prefix+""ddlProject"").disabled = false;
				}

				if (purposeText != ""Product"")                  
				{
					document.getElementById(prefix+""ddlCustomerType"").disabled = true;      
					document.getElementById(prefix+""ddlDepartment"").disabled = false;               
				} else
				{
					document.getElementById(prefix+""ddlCustomerType"").disabled = false;
					document.getElementById(prefix+""ddlDepartment"").disabled = true;
				}

				
				

	
			} 
			 
			</script>
			";
			Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
		}

        protected void LoadDownloadDDLs() {
            LogSession session = base.GetSessionInfo();

            //Dim1 DDL
            IList<CompanyProject> lstProject = null;
            lstProject = new SystemDataActivity().RetrieveAllCompanyProjectListByCompanyIDSortByProjectID(session.CompanyId);

            this.ddlDim1.DataSource = lstProject;
            this.ddlDim1.DataBind();

            if (lstProject.Count == 0)
                this.ddlDim1.Items.Insert(0, new ListItem("NONE", "-1"));

            //Defult Option 
            this.ddlDim2.Items.Insert(0, new ListItem("NONE", "-1"));
            this.ddlDim2.SelectedIndex = -1;
            this.ddlDim3.Items.Insert(0, new ListItem("NONE", "-1"));
            this.ddlDim3.SelectedIndex = -1;
            this.ddlDim4.Items.Insert(0, new ListItem("NONE", "-1"));
            this.ddlDim4.SelectedIndex = -1;
        }

		protected void LoadDDLs()
		{
			// load year ddl
			DataTable dtt1 = new DataTable();
			dtt1.Columns.Add("Year", typeof(string));

			for (int i = -1; i < 5; i++)
			{
				DataRow dr1 = dtt1.NewRow();
				dr1["Year"] = DateTime.Now.Year + i;

				dtt1.Rows.Add(dr1);
			}

			this.ddlYear.DataSource = dtt1;
			this.ddlYear.DataBind();

            this.templateYear.DataSource = dtt1;
            this.templateYear.DataBind();

			if(DateTime.Now.Month > 10)
				this.ddlYear.SelectedValue = (Convert.ToInt32(DateTime.Now.Year + 1)).ToString();
			else
				this.ddlYear.SelectedValue = (Convert.ToInt32(DateTime.Now.Year)).ToString();

			// load purpose ddl
			IList<ItemPurpose> lstPurpose = null;
			lstPurpose = new SystemDataActivity().RetrieveAllItemPurposeListSortByID();
			
			this.ddlPurpose.DataSource = lstPurpose;
			this.ddlPurpose.DataBind();

			// load project ddl
			// modified by OSS on 26 Apr 2012
			LogSession session = base.GetSessionInfo();
			IList<CompanyProject> lstProject = null;
			lstProject = new SystemDataActivity().RetrieveAllCompanyProjectListByCompanyIDSortByProjectID(session.CompanyId);

			this.ddlProject.DataSource = lstProject;
			this.ddlProject.DataBind();

			if (lstProject.Count == 0) 
				this.ddlProject.Items.Insert(0, new ListItem("Not Applicable", "-1"));

			
			IList<CompanyDepartment> lstDepartment = null;
			lstDepartment = new SystemDataActivity().RetrieveAllCompanyDepartmentListByCompanyIDSortByDepartmentName(session.CompanyId);

			this.ddlDepartment.DataSource = lstDepartment;
			this.ddlDepartment.DataBind();

			if (lstDepartment.Count == 0)
				this.ddlDepartment.Items.Insert(0, new ListItem("Not Applicable", "-1"));
			


		}

		protected void btnUpload_Click(object sender, EventArgs e)
		{
			if (FileUpload1.HasFile)
			{
				FileUpload1.SaveAs(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + "\\" + FileUpload1.FileName);

				this.IFrame1.Attributes["style"] = "";
				this.IFrame1.Attributes["src"] = String.Format("UploadParsing.aspx?FILENAME={0}&YEAR={1}&PURPOSEID={2}&PROJECTID={3}&DEPARTMENTID={4}",
															Server.UrlEncode(FileUpload1.FileName),
															this.ddlYear.SelectedValue,
															this.ddlPurpose.SelectedValue,
															this.ddlProject.SelectedValue,
															//this.ddlCustomerType.SelectedValue,
															this.ddlDepartment.SelectedValue);


			}
			else
			{
				lblMsg.Text = "You have not specified a file.";
			}
		}


        MemoryStream GetExcelStream()
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }

        private ICellStyle setCellStyleByItem(ICellStyle thinBorder ,ICellStyle thickBorder, ICellStyle doubleBorder, string itemName)
        {
            if(System.Array.IndexOf(itemForDouble,itemName) > -1)
            {
                return doubleBorder;
            }
            else if (System.Array.IndexOf(itemForThick, itemName) > -1)
            {
                return thickBorder;
            }
            else
            {
                return thinBorder;
            }

        }

        protected void btnDownload_Click(object sender, EventArgs e) {

            LogSession session = base.GetSessionInfo();
            
            Company company = new SystemDataActivity().RetrieveCompanyByCoyId(session.CompanyId);

            string fileName = "PNL_Budget_Worksheet" + ".xls";
            string currencyLess = "000s";
            var financialMonth = 3;

            if (session.DefaultCurrency == "IDR")
                currencyLess = "000,000s";

            hssfworkbook = new HSSFWorkbook();

            //Font setup
            IFont boldFont = hssfworkbook.CreateFont();
            boldFont.Boldweight = 600;
            boldFont.FontHeight = 170;

            IFont defaultFont = hssfworkbook.CreateFont();
            defaultFont.FontHeight = 180;

            ICellStyle topBorderStyle = hssfworkbook.CreateCellStyle();
            topBorderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            topBorderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;

            ICellStyle rightBorderStyle = hssfworkbook.CreateCellStyle();
            rightBorderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            rightBorderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;

            ICellStyle headerRowStyle = hssfworkbook.CreateCellStyle();
            headerRowStyle.SetFont(boldFont);
            headerRowStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;

            ICellStyle companyStyle = hssfworkbook.CreateCellStyle();
            companyStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            companyStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            companyStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;
            companyStyle.SetFont(boldFont);

            ICellStyle asOfStyle = hssfworkbook.CreateCellStyle();
            asOfStyle.Alignment = HorizontalAlignment.Center;
            asOfStyle.SetFont(boldFont);
            asOfStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;
            asOfStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            ICellStyle reportNameStyle = hssfworkbook.CreateCellStyle();
            reportNameStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Hair;
            reportNameStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            reportNameStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            reportNameStyle.SetFont(boldFont);

            ICellStyle dim1Style = hssfworkbook.CreateCellStyle();
            dim1Style.BorderTop = NPOI.SS.UserModel.BorderStyle.Hair;
            dim1Style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            dim1Style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;
            dim1Style.SetFont(boldFont);

            ICellStyle forTheYearCellStyle = hssfworkbook.CreateCellStyle();
            forTheYearCellStyle.SetFont(boldFont);
            forTheYearCellStyle.Alignment = HorizontalAlignment.Right;
            forTheYearCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;
            forTheYearCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            ICellStyle yearCellStyle = hssfworkbook.CreateCellStyle();
            yearCellStyle.Alignment = HorizontalAlignment.Right;
            yearCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;
            yearCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            yearCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            yearCellStyle.SetFont(boldFont);

            ICellStyle prepareByStyle = hssfworkbook.CreateCellStyle();
            prepareByStyle.Alignment = HorizontalAlignment.Right;
            prepareByStyle.SetFont(boldFont);

            ICellStyle prepareByNameStyle = hssfworkbook.CreateCellStyle();
            prepareByNameStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            prepareByNameStyle.Alignment = HorizontalAlignment.Right;
            prepareByNameStyle.SetFont(boldFont);

            ICellStyle lockCell = hssfworkbook.CreateCellStyle();
            lockCell.IsLocked = true;

            ICellStyle titleCellStyle = hssfworkbook.CreateCellStyle();
            titleCellStyle.Alignment = HorizontalAlignment.Center;
            titleCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            titleCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            titleCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            titleCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            titleCellStyle.SetFont(boldFont);

            ICellStyle currencyStyle = hssfworkbook.CreateCellStyle();
            currencyStyle.Alignment = HorizontalAlignment.Center;
            currencyStyle.SetFont(boldFont);

            //SN Cell Style
            ICellStyle snRowCellStyleThin = hssfworkbook.CreateCellStyle();
            snRowCellStyleThin.Alignment = HorizontalAlignment.Center;
            snRowCellStyleThin.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            snRowCellStyleThin.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            snRowCellStyleThin.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;
            snRowCellStyleThin.BottomBorderColor = 4321;
            snRowCellStyleThin.SetFont(defaultFont);

            ICellStyle snRowCellStyleMedium = hssfworkbook.CreateCellStyle();
            snRowCellStyleMedium.Alignment = HorizontalAlignment.Center;
            snRowCellStyleMedium.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            snRowCellStyleMedium.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            snRowCellStyleMedium.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            snRowCellStyleMedium.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            snRowCellStyleMedium.SetFont(defaultFont);

            ICellStyle snRowCellStyleDouble = hssfworkbook.CreateCellStyle();
            snRowCellStyleDouble.Alignment = HorizontalAlignment.Center;
            snRowCellStyleDouble.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            snRowCellStyleDouble.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            snRowCellStyleDouble.BorderBottom = NPOI.SS.UserModel.BorderStyle.Double;
            snRowCellStyleDouble.SetFont(defaultFont);

            //Item Cell Style
            ICellStyle itemCellStyleThin = hssfworkbook.CreateCellStyle();
            itemCellStyleThin.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;
            itemCellStyleThin.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            itemCellStyleThin.BottomBorderColor = 4321;
            itemCellStyleThin.SetFont(defaultFont);

            ICellStyle itemCellStyleMedium = hssfworkbook.CreateCellStyle();
            itemCellStyleMedium.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            itemCellStyleMedium.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            itemCellStyleMedium.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            itemCellStyleMedium.SetFont(boldFont);
         
            ICellStyle itemCellStyleDouble = hssfworkbook.CreateCellStyle();
            itemCellStyleDouble.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            itemCellStyleDouble.BorderBottom = NPOI.SS.UserModel.BorderStyle.Double;
            itemCellStyleDouble.SetFont(boldFont);
            
            // Figure Cell Style
            ICellStyle figureRowCellStyleThin = hssfworkbook.CreateCellStyle();
            figureRowCellStyleThin.IsLocked = false;
            figureRowCellStyleThin.BorderRight = NPOI.SS.UserModel.BorderStyle.Hair;
            figureRowCellStyleThin.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;
            figureRowCellStyleThin.BottomBorderColor = 4321;
            figureRowCellStyleThin.SetFont(defaultFont);

            ICellStyle figureRowCellStyleMedium = hssfworkbook.CreateCellStyle();
            figureRowCellStyleMedium.IsLocked = false;
            figureRowCellStyleMedium.BorderRight = NPOI.SS.UserModel.BorderStyle.Hair;
            figureRowCellStyleMedium.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            figureRowCellStyleMedium.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            figureRowCellStyleMedium.SetFont(boldFont);
    
            ICellStyle figureRowCellStyleDouble = hssfworkbook.CreateCellStyle();
            figureRowCellStyleDouble.IsLocked = false;
            figureRowCellStyleDouble.BorderRight = NPOI.SS.UserModel.BorderStyle.Hair;
            figureRowCellStyleDouble.BorderBottom = NPOI.SS.UserModel.BorderStyle.Double;
            figureRowCellStyleDouble.SetFont(boldFont);
    
            // Total Figure Cell Style
            ICellStyle totalFigureRowCellStyleThin = hssfworkbook.CreateCellStyle();
            totalFigureRowCellStyleThin.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            totalFigureRowCellStyleThin.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;
            totalFigureRowCellStyleThin.BottomBorderColor = 4321;
            totalFigureRowCellStyleThin.SetFont(defaultFont);

            ICellStyle totalFigureRowCellStyleMedium = hssfworkbook.CreateCellStyle();
            totalFigureRowCellStyleMedium.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            totalFigureRowCellStyleMedium.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            totalFigureRowCellStyleMedium.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            totalFigureRowCellStyleMedium.SetFont(boldFont);
    
            ICellStyle totalFigureRowCellStyleDouble = hssfworkbook.CreateCellStyle();
            totalFigureRowCellStyleDouble.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            totalFigureRowCellStyleDouble.BorderBottom = NPOI.SS.UserModel.BorderStyle.Double;
            totalFigureRowCellStyleDouble.SetFont(boldFont);
 
            //Sheet setup
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");
            sheet1.PrintSetup.Landscape = true;
            sheet1.SetMargin(MarginType.LeftMargin, 0.55);
            sheet1.SetMargin(MarginType.RightMargin, 0.1);
            sheet1.SetMargin(MarginType.BottomMargin, 0.1);
            sheet1.SetMargin(MarginType.TopMargin, 0.7);
            
            sheet1.CreateFreezePane(0, 4, 2, 4);

            //Set Column width
            sheet1.SetColumnWidth(0, 3 * 300);
            sheet1.SetColumnWidth(1, 26 * 300);
            sheet1.SetColumnWidth(2, 6 * 340);
            sheet1.SetColumnWidth(3, 6 * 340);
            sheet1.SetColumnWidth(4, 6 * 340);
            sheet1.SetColumnWidth(5, 6 * 340);
            sheet1.SetColumnWidth(6, 6 * 340);
            sheet1.SetColumnWidth(7, 6 * 340);
            sheet1.SetColumnWidth(8, 6 * 340);
            sheet1.SetColumnWidth(9, 6 * 340);
            sheet1.SetColumnWidth(10, 6 * 340);
            sheet1.SetColumnWidth(11, 6 * 340);
            sheet1.SetColumnWidth(12, 6 * 340);
            sheet1.SetColumnWidth(13, 6 * 340);
            sheet1.SetColumnWidth(14, 6 * 340);
            sheet1.SetColumnWidth(15, 6 * 340);
          
            //merging cell
            // first row to last , first cell to last cell
            var companySpace = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 2);
            var asOfSpace = new NPOI.SS.Util.CellRangeAddress(0, 0, 6, 8);
            var forYearSpace = new NPOI.SS.Util.CellRangeAddress(0, 0, 12, 13);
            var dim1Space = new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 2);
            var dim2Space = new NPOI.SS.Util.CellRangeAddress(1, 1, 3, 6);
            var dim3Space = new NPOI.SS.Util.CellRangeAddress(1, 1, 7, 10);
            var dim4Space = new NPOI.SS.Util.CellRangeAddress(1, 1, 11, 14);
            var reportNameSpace = new NPOI.SS.Util.CellRangeAddress(2, 2, 0, 1);
            var prepareNameSpace = new NPOI.SS.Util.CellRangeAddress(2, 2, 13, 14);
            var currencySpace = new NPOI.SS.Util.CellRangeAddress(2, 2, 6, 8);

            sheet1.AddMergedRegion(companySpace);
            sheet1.AddMergedRegion(asOfSpace);
            sheet1.AddMergedRegion(forYearSpace);
            sheet1.AddMergedRegion(dim1Space);
            sheet1.AddMergedRegion(dim2Space);
            sheet1.AddMergedRegion(dim3Space);
            sheet1.AddMergedRegion(dim4Space);
            sheet1.AddMergedRegion(reportNameSpace);
            sheet1.AddMergedRegion(prepareNameSpace);
            sheet1.AddMergedRegion(currencySpace);

            ICell tempCell;

            IRow row0 = sheet1.CreateRow(0);
            row0.RowStyle = headerRowStyle;
            ICell companyCell = row0.CreateCell(0);
            companyCell.SetCellValue(company.Name.ToUpper());
            companyCell.CellStyle = companyStyle;

            tempCell = row0.CreateCell(1);
            tempCell.CellStyle = topBorderStyle;
            tempCell = row0.CreateCell(2);
            tempCell.CellStyle = topBorderStyle;
            tempCell = row0.CreateCell(3);
            tempCell.CellStyle = topBorderStyle;
            tempCell = row0.CreateCell(4);
            tempCell.CellStyle = topBorderStyle;
            tempCell = row0.CreateCell(5);
            tempCell.CellStyle = topBorderStyle;
            tempCell = row0.CreateCell(7);
            tempCell.CellStyle = topBorderStyle;
            tempCell = row0.CreateCell(8);
            tempCell.CellStyle = topBorderStyle;
            tempCell = row0.CreateCell(9);
            tempCell.CellStyle = topBorderStyle;
            tempCell = row0.CreateCell(10);
            tempCell.CellStyle = topBorderStyle;
            tempCell = row0.CreateCell(11);
            tempCell.CellStyle = topBorderStyle;
            tempCell = row0.CreateCell(13);
            tempCell.CellStyle = topBorderStyle;

            ICell asOfCell = row0.CreateCell(6);
            asOfCell.SetCellValue("AS OF:" + DateTime.Now.ToString("dd/MM/yyy hh:mm:ss"));
            asOfCell.CellStyle = asOfStyle;

            ICell forYearCell = row0.CreateCell(12);
            forYearCell.SetCellValue("FOR THE YEAR OF:");
            forYearCell.CellStyle = forTheYearCellStyle;

            ICell yearCell = row0.CreateCell(14);
            yearCell.SetCellValue(this.templateYear.SelectedValue + "/" + (Convert.ToInt32(this.templateYear.SelectedValue)+1).ToString());
            yearCell.CellStyle = yearCellStyle;
           
            IRow row1 = sheet1.CreateRow(1);
            row1.RowStyle = headerRowStyle;
            ICell dim1Cell = row1.CreateCell(0);
            dim1Cell.SetCellValue(this.ddlDim1.SelectedItem.Text != null ? "DIM1: " + this.ddlDim1.SelectedItem.Text.ToUpper() : "DIM1: " + "NONE");
            dim1Cell.CellStyle = dim1Style;

            ICell dim2Cell = row1.CreateCell(3);
            dim2Cell.SetCellValue(this.ddlDim2.SelectedItem.Text != null ? "DIM2: " + this.ddlDim2.SelectedItem.Text.ToUpper() : "DIM2: " + "NONE");
            dim2Cell.CellStyle = headerRowStyle;

            ICell dim3Cell = row1.CreateCell(7);
            dim3Cell.SetCellValue(this.ddlDim3.SelectedItem.Text != null ? "DIM3: " + this.ddlDim3.SelectedItem.Text.ToUpper() : "DIM3: " + "NONE");
            dim3Cell.CellStyle = headerRowStyle;

            ICell dim4Cell = row1.CreateCell(11);
            dim4Cell.SetCellValue(this.ddlDim4.SelectedItem.Text != null ? "DIM4: " + this.ddlDim4.SelectedItem.Text.ToUpper() : "DIM4: " + "NONE");
            dim4Cell.CellStyle = headerRowStyle;

            //borderRight
            tempCell = row1.CreateCell(14);
            tempCell.CellStyle = rightBorderStyle;

            IRow row2 = sheet1.CreateRow(2);
            ICell reportNameCell = row2.CreateCell(0);
            reportNameCell.SetCellValue("F21B PROFIT & LOSS BUDGET");
            reportNameCell.CellStyle = reportNameStyle;

            ICell currencyCell = row2.CreateCell(6);
            currencyCell.SetCellValue("CURRENCY: " + session.DefaultCurrency + " " + currencyLess);
            currencyCell.CellStyle = currencyStyle;

            ICell prepareByTitleCell = row2.CreateCell(12);
            prepareByTitleCell.SetCellValue("PREPARED BY:");
            prepareByTitleCell.CellStyle = prepareByStyle;

            ICell prepareByName = row2.CreateCell(13);
            prepareByName.SetCellValue(session.UserRealName);
            prepareByName.CellStyle = prepareByNameStyle;

            tempCell = row2.CreateCell(14);
            tempCell.CellStyle = rightBorderStyle;


            //Print Title
            IRow  row3 = sheet1.CreateRow(3);
           
            ICell snTitleCell = row3.CreateCell(0);
            ICell itemTitleCell = row3.CreateCell(1);
          
            snTitleCell.CellStyle = titleCellStyle;
            itemTitleCell.SetCellValue("Item");
            itemTitleCell.CellStyle = titleCellStyle;

            var startMonth = new DateTime( int.Parse(this.templateYear.SelectedValue), financialMonth, 1);

            for (int i = 1; i <=12 ; i++)
            {
                ICell monthCell = row3.CreateCell(i + 1); // +1 to because of the sn cell
                
                monthCell.SetCellValue(startMonth.AddMonths(i).ToString("MMM"));
                
                monthCell.CellStyle = titleCellStyle;

                if (i == 12) {
                    monthCell = row3.CreateCell(12 + 2);
                    monthCell.SetCellValue("Total");
                    monthCell.CellStyle = titleCellStyle;
                }

            }

            //Retrive and print PNL finance item
            DataSet dsPNLItem = new DataSet();
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            dacl.GetFinanceItemByReport("PNL", ref dsPNLItem);

            int rowCount = 4 ,cellIndex =5;
            ICell cellDetail,cellFigure;
          
            IRow rowItem;
            
            foreach (DataRow dr in dsPNLItem.Tables[0].Rows)
            {
                rowItem = sheet1.CreateRow(rowCount);
              
                cellDetail = rowItem.CreateCell(0);
                cellDetail.CellStyle = setCellStyleByItem(snRowCellStyleThin, snRowCellStyleMedium, snRowCellStyleDouble, dr["itemName"].ToString());

                if (dr["itemName"].ToString() != "Performance Indicators")
                    cellDetail.SetCellValue(dr["itemSN"].ToString());
                
                cellDetail = rowItem.CreateCell(1);
                cellDetail.CellStyle = setCellStyleByItem(itemCellStyleThin, itemCellStyleMedium, itemCellStyleDouble, dr["itemName"].ToString());
                cellDetail.SetCellValue(dr["itemName"].ToString());
                
                for (int i = 1; i <=12 ; i++){

                    cellFigure = rowItem.CreateCell(i + 1);// +1 because of sn cell
                    //cellFigure.SetCellType(CellType.Numeric);
                    
                    if (dr["itemName"].ToString() == "(PNL)Total Sales")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "5:" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "6)");
                    else if (dr["itemName"].ToString() == "(PNL)Total Cost of Sales")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "8:" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "9)");
                    else if (dr["itemName"].ToString() == "(PNL)Gross Profit")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "7," + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "10)");
                    else if (dr["itemName"].ToString() == "(PNL)Total Other Op Income/ (Exp)")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "12:" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "15)");
                    else if (dr["itemName"].ToString() == "(PNL)Total S&D Expenses")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "17:" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "26)");
                    else if (dr["itemName"].ToString() == "(PNL)A&G Direct Expenses")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "28:" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "38)");
                    else if (dr["itemName"].ToString() == "(PNL)Other A&G Expenses")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "40:" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "45)");
                    else if (dr["itemName"].ToString() == "(PNL)Total A&G Expenses")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "39," + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "46)");
                    else if (dr["itemName"].ToString() == "(PNL)Profit from Operations")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "11," + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "16," + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "27," + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "47)");
                    else if (dr["itemName"].ToString() == "(PNL)Total Non-Op Income/(Exp)")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "49:" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "54)");
                    else if (dr["itemName"].ToString() == "(PNL)Profit Before Taxation")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "48," + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "55)");
                    else if (dr["itemName"].ToString() == "(PNL)Profit After Taxation")
                        cellFigure.SetCellFormula("SUM(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "56:" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "57)");
                    else if (dr["itemName"].ToString() == "Gross Profits Margin %")
                        cellFigure.SetCellFormula(NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "11/" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "7*100%");
                    else if (dr["itemName"].ToString() == "- External Sales GP %")
                        cellFigure.SetCellFormula("("+NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "5-" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "8)/"+ NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "5");
                    else if (dr["itemName"].ToString() == "- Interco Sales GP %")
                        cellFigure.SetCellFormula("(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "6-" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "9)/" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "6");
                    else if (dr["itemName"].ToString() == "Selling & Dist Margin %")
                        cellFigure.SetCellFormula("-(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "27)/" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "7");
                    else if (dr["itemName"].ToString() == "A&G Margin Total %")
                        cellFigure.SetCellFormula("-(" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "47)/" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "7");
                    else if (dr["itemName"].ToString() == "Operating Profit Margin %")
                        cellFigure.SetCellFormula(NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "48/" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "7");
                    else if (dr["itemName"].ToString() == "Profit Before Taxation %")
                        cellFigure.SetCellFormula(NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "56/" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "7");
                    else if (dr["itemName"].ToString() == "Profit After Taxation %")
                        cellFigure.SetCellFormula(NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "58/" + NPOI.SS.Util.CellReference.ConvertNumToColString(cellFigure.ColumnIndex) + "7");
                    else
                        cellFigure.SetCellValue(0);
                    
                    
                    cellFigure.CellStyle = setCellStyleByItem(figureRowCellStyleThin, figureRowCellStyleMedium, figureRowCellStyleDouble, dr["itemName"].ToString());


                    //Create Figure for Total Column
                    if (i == 12) {

                        cellFigure = rowItem.CreateCell(i + 2);
                        cellFigure.SetCellType(CellType.Numeric);
                        cellFigure.SetCellFormula("SUM(C" + cellIndex + ":N" + cellIndex + ")");
                        cellFigure.CellStyle = setCellStyleByItem(totalFigureRowCellStyleThin, totalFigureRowCellStyleMedium, totalFigureRowCellStyleDouble, dr["itemName"].ToString());
                    }

                }
                rowCount++;
                cellIndex++;
            }

            ICellStyle hiddenStyle = hssfworkbook.CreateCellStyle();
            hiddenStyle.IsHidden = true;

            //add dim id
            IRow hiddenRow = sheet1.CreateRow(rowCount);
            hiddenRow.RowStyle = hiddenStyle;
            sheet1.GetRow(rowCount).RowStyle.IsHidden = true;

            //company
            ICell hiddenCell = hiddenRow.CreateCell(0);
           //  
            hiddenCell.SetCellValue(session.CompanyId.ToString() + "," + this.ddlDim1.SelectedItem.Text.ToString() + "," + this.ddlDim2.SelectedItem.Text.ToString() + "," + this.ddlDim3.SelectedItem.Text.ToString() + "," + this.ddlDim4.SelectedItem.Text.ToString() + "," + this.templateYear.SelectedValue);
            hiddenCell.CellStyle = hiddenStyle;
           
            // hide the row
            hiddenRow.ZeroHeight = true;

            //Sheet Protection
            sheet1.ProtectSheet("password");
            
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/vnd.ms-excel";
            GetExcelStream().WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
        
        }

        protected void ddlDim1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            //Dim2 DDL
            //Retreive Dim2 base on Dim 1 Selected Value
            GMSGeneralDALC dacl = new GMSGeneralDALC();         
            DataSet dsDepartments = new DataSet();
           
            this.ddlDim2.Enabled = true;
            this.ddlDim2.Items.Clear();
            dacl.GetDepartmentByDivision(session.CompanyId, Convert.ToInt16(this.ddlDim1.SelectedValue), ref dsDepartments);
            foreach (DataRow dr in dsDepartments.Tables[0].Rows)
            {
                this.ddlDim2.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
            }

            //if empty department is empty
            if(dsDepartments.Tables[0].Rows.Count == 0){
                this.ddlDim2.Items.Insert(0, new ListItem("NONE", "-1"));
                this.ddlDim2.SelectedIndex = -1;
            }

            this.ddlDim3.Items.Clear();
            this.ddlDim3.Items.Insert(0, new ListItem("NONE", "-1"));
            this.ddlDim3.SelectedIndex = -1;
            this.ddlDim3.Enabled = false;

            this.ddlDim4.Items.Clear();
            this.ddlDim4.Items.Insert(0, new ListItem("NONE", "-1"));
            this.ddlDim4.SelectedIndex = -1;
            this.ddlDim4.Enabled = false;


        }

        protected void ddlDim2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            //Dim3 DDL
            //Retreive Dim3 base on Dim 2 Selected Value
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsSections = new DataSet();

            this.ddlDim3.Enabled = true;
            this.ddlDim3.Items.Clear();
            dacl.GetCompanySection(session.CompanyId, Convert.ToInt16(this.ddlDim2.SelectedValue), ref dsSections);
            foreach (DataRow dr in dsSections.Tables[0].Rows)
            {
                this.ddlDim3.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
            }
            
            //if empty department is empty
            if (dsSections.Tables[0].Rows.Count == 0)
            {
                this.ddlDim3.Items.Insert(0, new ListItem("NONE", "-1"));
                this.ddlDim3.SelectedIndex = -1;
            }

            this.ddlDim4.Items.Clear();
            this.ddlDim4.Items.Insert(0, new ListItem("NONE", "-1"));
            this.ddlDim4.SelectedIndex = -1;
            this.ddlDim4.Enabled = false;
        }

        protected void ddlDim3_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            //Dim4 DDL
            //Retreive Dim4 base on Dim 3 Selected Value
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsUnits = new DataSet();

            this.ddlDim4.Enabled = true;
            this.ddlDim4.Items.Clear();

            dacl.GetCompanyUnit(session.CompanyId, Convert.ToInt16(this.ddlDim3.SelectedValue), ref dsUnits);
            foreach (DataRow dr in dsUnits.Tables[0].Rows)
            {
                this.ddlDim4.Items.Add(new ListItem(dr["UnitName"].ToString(), dr["UnitID"].ToString()));
            }

            //if empty department is empty
            if (dsUnits.Tables[0].Rows.Count == 0)
            {
                this.ddlDim4.Items.Insert(0, new ListItem("NONE", "-1"));
                this.ddlDim4.SelectedIndex = -1;
            }
        }

	}
}
