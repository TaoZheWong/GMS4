using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.Spreadsheet;
using Telerik.Web.UI;

namespace GMSWeb.Budget
{
    public partial class HRBudget : GMSBasePage
    {
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
                                                                            185);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            185);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!IsPostBack)
            {
                LoadControl();
                this.hidYear.Value = this.ddlYear.SelectedValue;
                this.hidBudgetYear.Value = this.ddlYear.SelectedText;
            }
        }

        protected void LoadControl()
        {
            LogSession session = base.GetSessionInfo();
            //ddlyear
            var startYear = int.Parse(DateTime.Now.AddYears(-2).ToString("yyyy"));
            int currentYear = DateTime.Now.Year;
            for (int i = startYear; i <= currentYear; i++)
            {
                ddlYear.Items.Add(new DropDownListItem(i.ToString() + "/" + (i + 1).ToString().Substring(2), Convert.ToString(i)));
            }
            if (DateTime.Now.Month <= 3)
                this.ddlYear.SelectedValue = (currentYear - 1).ToString();
            else
                this.ddlYear.SelectedValue = currentYear.ToString();

            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet dsDim1 = new DataSet(); DataSet dsDim2 = new DataSet();
            DataSet dsDim3 = new DataSet(); DataSet dsDim4 = new DataSet();
            short dim1 = -1; short dim2 = -1; short dim3 = -1;

            ggdal.SelectHrBudgetDim(session.CompanyId, session.UserId, "Dim1", short.Parse(this.ddlYear.SelectedValue),
                 dim1, dim2, dim3, ref dsDim1);
            if (dsDim1.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < dsDim1.Tables[0].Rows.Count; j++)
                {
                    ddlSearchDim1.Items.Add(new DropDownListItem(dsDim1.Tables[0].Rows[j]["Dim1Name"].ToString(), dsDim1.Tables[0].Rows[j]["Dim1"].ToString()));
                }

                ggdal.SelectHrBudgetDim(session.CompanyId, session.UserId, "Dim2", short.Parse(this.ddlYear.SelectedValue),
                 short.Parse(ddlSearchDim1.SelectedValue), dim2, dim3, ref dsDim2);
                foreach (DataRow dr in dsDim2.Tables[0].Rows)
                {
                    this.ddlSearchDim2.Items.Add(new DropDownListItem(dr["Dim2Name"].ToString(), dr["Dim2"].ToString()));
                }
                ddlSearchDim2.Enabled = true;

                if (dsDim2.Tables[0].Rows.Count > 0)
                {
                    ggdal.SelectHrBudgetDim(session.CompanyId, session.UserId, "Dim3", short.Parse(this.ddlYear.SelectedValue),
                        short.Parse(ddlSearchDim1.SelectedValue), short.Parse(ddlSearchDim2.SelectedValue), dim3, ref dsDim3);
                    foreach (DataRow dr in dsDim3.Tables[0].Rows)
                    {
                        this.ddlSearchDim3.Items.Add(new DropDownListItem(dr["Dim3Name"].ToString(), dr["Dim3"].ToString()));
                    }
                    ddlSearchDim3.Enabled = true;

                    if (dsDim3.Tables[0].Rows.Count > 0)
                    {
                        ggdal.SelectHrBudgetDim(session.CompanyId, session.UserId, "Dim4", short.Parse(this.ddlYear.SelectedValue),
                            dim1, dim2, dim3, ref dsDim4);
                        foreach (DataRow dr in dsDim4.Tables[0].Rows)
                        {
                            this.ddlSearchDim4.Items.Add(new DropDownListItem(dr["Dim4Name"].ToString(), dr["Dim4"].ToString()));
                        }
                        ddlSearchDim4.Enabled = true;
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadHrTemplate();

            LoadB52Spreadsheet("B");
            LoadB52Spreadsheet("F");
            LoadB52Spreadsheet("A");

            this.hidType.Value = this.ddlType.SelectedValue;

            string yearFormat = this.hidYear.Value + "/" + (int.Parse(this.hidYear.Value) + 1).ToString().Substring(2);

            this.btnSubmit.Text = "Convert to " + yearFormat + " Budget";
            this.btnSubmitForecast.Text = "Convert to " + this.hidBudgetYear.Value + " Forecast";
            this.RadMultiPage1.Visible = true;
            this.RadTabStrip1.Visible = true;
            this.RadSpreadsheet1.Visible = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            short dim1 = -1; short dim2 = -1; short dim3 = -1; short dim4 = -1;
            if (!string.IsNullOrEmpty(this.ddlSearchDim1.SelectedValue))
                dim1 = short.Parse(this.ddlSearchDim1.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim2.SelectedValue))
                dim2 = short.Parse(this.ddlSearchDim2.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim3.SelectedValue))
                dim3 = short.Parse(this.ddlSearchDim3.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim4.SelectedValue))
                dim4 = short.Parse(this.ddlSearchDim4.SelectedValue);

            short year = short.Parse(DateTime.Now.Year.ToString());
            if (!string.IsNullOrEmpty(this.hidYear.Value))
                year = short.Parse(this.hidYear.Value);
            try
            {
                ggdal.DeleteHrBudget(session.CompanyId, year, dim1, dim2, dim3, dim4, this.ddlType.SelectedValue);
                JScriptAlertMsg("Data deleted");
                this.RadMultiPage1.Visible = false;
                this.RadTabStrip1.Visible = false;
            }
            catch(Exception ex) { JScriptAlertMsg(ex.ToString()); }
        }

        protected void btnSubmitBudget_Click(object sender, EventArgs e)
        {
            ProcessData("B");
            LoadB52Spreadsheet("B");
            LoadB52Spreadsheet("F");
            LoadB52Spreadsheet("A");//reload b52 to get latest converted data
        }

        protected void btnSubmitForecast_Click(object sender, EventArgs e)
        {
            ProcessData("F");
            LoadB52Spreadsheet("B");
            LoadB52Spreadsheet("F");
            LoadB52Spreadsheet("A");//reload b52 to get latest converted data
        }

        protected void btnSubmitB52B_Click(object sender, EventArgs e)
        {
            UpdateB52("B");
            LoadHrTemplate();
            LoadB52Spreadsheet("B");
            LoadB52Spreadsheet("F");
            LoadB52Spreadsheet("A");
        }

        protected void btnSubmitB52F_Click(object sender, EventArgs e)
        {
            UpdateB52("F");
            LoadHrTemplate();
            LoadB52Spreadsheet("B");
            LoadB52Spreadsheet("F");
            LoadB52Spreadsheet("A");
        }

        protected void ProcessData(string budgetType)
        {
            LogSession session = base.GetSessionInfo();

            short dim1 = -1; short dim2 = -1; short dim3 = -1; short dim4 = -1;
            if (!string.IsNullOrEmpty(this.ddlSearchDim1.SelectedValue))
                dim1 = short.Parse(this.ddlSearchDim1.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim2.SelectedValue))
                dim2 = short.Parse(this.ddlSearchDim2.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim3.SelectedValue))
                dim3 = short.Parse(this.ddlSearchDim3.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim4.SelectedValue))
                dim4 = short.Parse(this.ddlSearchDim4.SelectedValue);

            short year = short.Parse(DateTime.Now.Year.ToString());
            if (!string.IsNullOrEmpty(this.hidYear.Value))
            {
                if (budgetType == "B")
                    year = short.Parse((int.Parse(this.hidYear.Value) + 1).ToString());
                else if (budgetType == "F")
                    year = short.Parse(this.hidYear.Value);
                else if (budgetType == "A")
                    year = short.Parse((int.Parse(this.hidYear.Value) - 1).ToString());
            }

            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            var workbook = Workbook.FromJson(HiddenField1.Value);

           //remove title row as in telerik radspreadsheet hidden row is started with rows[0]
            workbook.Sheets[0].Rows.Remove(workbook.Sheets[0].Rows[1]);
            
            if (workbook.Sheets[0].Rows.Count > 0)
            {
                try
                {
                    var rowID = workbook.Sheets[0].Rows[0];
                    var rowCOA = workbook.Sheets[0].Rows[1];
                    var rowQuarter = workbook.Sheets[0].Rows[2];
                    var rowTotal = workbook.Sheets[0].Rows[3];
                    int id = 0;
                    double total = 0;
                    for (int i = 0; i < rowID.Cells.Count; i++)
                    {
                        if (!(rowID.Cells[i].Value == null && rowTotal.Cells[i].Value == null))
                        {
                            if (rowID.Cells[i].Value != null)
                            {
                                if (!string.IsNullOrEmpty(rowID.Cells[i].Value.ToString()))
                                    id = int.Parse(rowID.Cells[i].Value.ToString());
                            }

                            if (!string.IsNullOrEmpty(rowTotal.Cells[i].Value.ToString()))
                                total = double.Parse(rowTotal.Cells[i].Value.ToString());

                            #region for Salary & Leave 
                            if (i == 0 || i == 1)//for salary & leave
                            {
                                switch (hidType.Value)
                                {
                                    case "A":
                                        id = 55;
                                        break;
                                    case "B":
                                        id = 55;
                                        break;
                                    case "C":
                                        id = 55;
                                        break;
                                    case "D":
                                        id = 55;
                                        break;
                                    case "E":
                                        id = 55;
                                        break;
                                    default:
                                        id = 0;
                                        break;
                                }

                                if (rowQuarter.Cells[i].Value.ToString() == "1Q")//1Q will be distribute to Apr,May,Jun only
                                {
                                    for (short j = 4; j <= 6; j++)
                                    {
                                        HrBudget hb = HrBudget.RetrieveByKey(session.CompanyId, dim1, dim2, dim3, dim4, year, j, id, budgetType);
                                        if (hb != null)
                                        {
                                            hb.Total = total;
                                            hb.UpdatedBy = session.UserId;
                                            hb.UpdatedDate = DateTime.Now;
                                            hb.Status = "Saved";
                                            hb.Save();
                                        }
                                        else
                                        {
                                            HrBudget newHb = new HrBudget();
                                            newHb.CoyID = session.CompanyId;
                                            newHb.Dim1 = dim1;
                                            newHb.Dim2 = dim2;
                                            newHb.Dim3 = dim3;
                                            newHb.Dim4 = dim4;
                                            newHb.tbYear = year;
                                            newHb.tbMonth = j;
                                            newHb.HrParentID = 0;
                                            newHb.Total = total;
                                            newHb.UpdatedBy = session.UserId;
                                            newHb.UpdatedDate = DateTime.Now;
                                            newHb.Type = budgetType;
                                            newHb.Status = "Saved";
                                            newHb.Save();
                                        }
                                    }
                                }
                                else if (rowQuarter.Cells[i].Value.ToString() == "3Q")//3Q will distribute to Jul - next yr Mar only
                                {
                                    int newMonth = 0;
                                    short newYear = 0;
                                    for (int j = 7; j <= 15; j++)
                                    {
                                        if (j > 12)
                                        {
                                            newMonth = j - 12;//to get next year Month
                                            newYear = short.Parse((int.Parse(year.ToString()) + 1).ToString());
                                        }
                                        else
                                        {
                                            newMonth = j;
                                            newYear = year;
                                        }

                                        HrBudget hb = HrBudget.RetrieveByKey(session.CompanyId, dim1, dim2, dim3, dim4, newYear, short.Parse(newMonth.ToString()), id, budgetType);
                                        if (hb != null)
                                        {
                                            hb.Total = total;
                                            hb.UpdatedBy = session.UserId;
                                            hb.UpdatedDate = DateTime.Now;
                                            hb.Status = "Saved";
                                            hb.Save();
                                        }
                                        else
                                        {
                                            HrBudget newHb = new HrBudget();
                                            newHb.CoyID = session.CompanyId;
                                            newHb.Dim1 = dim1;
                                            newHb.Dim2 = dim2;
                                            newHb.Dim3 = dim3;
                                            newHb.Dim4 = dim4;
                                            newHb.tbYear = newYear;
                                            newHb.tbMonth = short.Parse(newMonth.ToString());
                                            newHb.HrParentID = id;
                                            newHb.Total = total;
                                            newHb.UpdatedBy = session.UserId;
                                            newHb.UpdatedDate = DateTime.Now;
                                            newHb.Type = budgetType;
                                            newHb.Status = "Saved";
                                            newHb.Save();
                                        }
                                    }
                                }
                            }
                            #endregion
                            else if (i > 1)
                            {
                                int newMonth = 0;
                                short newYear = 0;
                                for (int j = 4; j <= 15; j++)
                                {
                                    if (j > 12)
                                    {
                                        newMonth = j - 12;//to get next year Month
                                        newYear = short.Parse((int.Parse(year.ToString()) + 1).ToString());
                                    }
                                    else
                                    {
                                        newMonth = j;
                                        newYear = year;
                                    }

                                    HrBudget hb = HrBudget.RetrieveByKey(session.CompanyId, dim1, dim2, dim3, dim4, newYear, short.Parse(newMonth.ToString()), id, budgetType);
                                    if (hb != null)
                                    {
                                        hb.Total = total;
                                        hb.UpdatedBy = session.UserId;
                                        hb.UpdatedDate = DateTime.Now;
                                        hb.Status = "Saved";
                                        hb.Save();
                                    }
                                    else
                                    {
                                        HrBudget newHb = new HrBudget();
                                        newHb.CoyID = session.CompanyId;
                                        newHb.Dim1 = dim1;
                                        newHb.Dim2 = dim2;
                                        newHb.Dim3 = dim3;
                                        newHb.Dim4 = dim4;
                                        newHb.tbYear = newYear;
                                        newHb.tbMonth = short.Parse(newMonth.ToString());
                                        newHb.HrParentID = id;
                                        newHb.Total = total;
                                        newHb.UpdatedBy = session.UserId;
                                        newHb.UpdatedDate = DateTime.Now;
                                        newHb.Type = budgetType;
                                        newHb.Status = "Saved";
                                        newHb.Save();
                                    }
                                }
                            }
                        }
                    }
                    JScriptAlertMsg("B52" + budgetType + " Saved.");
                }
                catch (Exception ex) { JScriptAlertMsg(ex.ToString()); }
            }
        }

        protected void UpdateB52(string budgetType)
        {
            LogSession session = base.GetSessionInfo();

            short dim1 = -1; short dim2 = -1; short dim3 = -1; short dim4 = -1;
            if (!string.IsNullOrEmpty(this.ddlSearchDim1.SelectedValue))
                dim1 = short.Parse(this.ddlSearchDim1.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim2.SelectedValue))
                dim2 = short.Parse(this.ddlSearchDim2.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim3.SelectedValue))
                dim3 = short.Parse(this.ddlSearchDim3.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim4.SelectedValue))
                dim4 = short.Parse(this.ddlSearchDim4.SelectedValue);

            short year = short.Parse(DateTime.Now.Year.ToString());
            if (!string.IsNullOrEmpty(this.hidYear.Value))
                year = short.Parse(this.hidYear.Value);

            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            Workbook workbook;
            string rowChanged = "";
            if (budgetType == "F")
            {
                workbook = Workbook.FromJson(HiddenField3.Value);
                rowChanged = this.hidRowChangedB52F.Value;//get row changed value
            }
            else
            {
                workbook = Workbook.FromJson(HiddenField2.Value);
                rowChanged = this.hidRowChangedB52B.Value;//get row changed value
                year = short.Parse((int.Parse(year.ToString()) + 1).ToString());
            }

            int[] listRowChanged = rowChanged.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c)).ToArray();

            for (int i = 0; i < 5; i++)
            {   //exclude title row
                workbook.Sheets[0].Rows.Remove(workbook.Sheets[0].Rows[0]);
            }
            if (workbook.Sheets[0].Rows.Count > 0)
            {
                //try
                //{
                int rowIndex = 0;
                foreach (var row in workbook.Sheets[0].Rows)
                {
                    if (rowIndex < workbook.Sheets[0].Rows.Count - 1)//to exclude total row
                    {
                        //if (listRowChanged.Length != 0)//check whether there is row changed that stored in array of the hidden field
                        //{
                        bool isRowChanged = listRowChanged.Contains(rowIndex + 5);
                        bool hasFormula = false;
                        if (budgetType == "F")
                            hasFormula = !string.IsNullOrEmpty(row.Cells[11].Formula);
                        else
                            hasFormula = !string.IsNullOrEmpty(row.Cells[7].Formula);
                        if (isRowChanged || hasFormula)//save only the row changed or cells with formula
                        {
                            int hrparentid = 0;
                            if (!string.IsNullOrEmpty(row.Cells[20].Value.ToString()))
                                hrparentid = int.Parse(row.Cells[20].Value.ToString());
                            for (int i = 1; i <= 12; i++)
                            {
                                if (budgetType == "F")//save only nov - mar un b52F
                                {
                                    int[] forecastmonth = { 11, 12, 1, 2, 3 };
                                    bool isForecast = forecastmonth.Contains(i);
                                    if (!isForecast)
                                        continue;//skip current iteration in for loop
                                }

                                short year_ = year;
                                if (i <= 3)
                                    year_ = short.Parse((int.Parse(year.ToString()) + 1).ToString());
                                double value = 0;
                                double value2 = 0;
                                if (i <= 3)
                                {
                                    if (!string.IsNullOrEmpty(row.Cells[i + 12].Value.ToString()))
                                        value = double.Parse(row.Cells[i + 12].Value.ToString());
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(row.Cells[i].Value.ToString()))
                                        value2 = double.Parse(row.Cells[i].Value.ToString());
                                }
                                HrBudget hb = HrBudget.RetrieveByKey(session.CompanyId, dim1, dim2, dim3, dim4, year_, short.Parse(i.ToString()), hrparentid, budgetType);
                                if (hb != null)
                                {
                                    if (i <= 3)
                                    {
                                        if (hb.Total == value)
                                            continue;
                                        else
                                            hb.Total = value;
                                    }
                                    else
                                    {
                                        if (hb.Total == value2)
                                            continue;
                                        else
                                            hb.Total = value2;
                                    }

                                    hb.UpdatedBy = session.UserId;
                                    hb.UpdatedDate = DateTime.Now;
                                    hb.Status = "Saved";
                                    hb.Save();
                                    JScriptAlertMsg("B52" + budgetType + " Updated");
                                }
                                else
                                {
                                    HrBudget newHb = new HrBudget();
                                    newHb.CoyID = session.CompanyId;
                                    newHb.Dim1 = dim1;
                                    newHb.Dim2 = dim2;
                                    newHb.Dim3 = dim3;
                                    newHb.Dim4 = dim4;
                                    newHb.tbYear = year_;
                                    newHb.tbMonth = short.Parse(i.ToString());
                                    newHb.HrParentID = hrparentid;
                                    if (i <= 3)
                                        newHb.Total = value;
                                    else
                                        newHb.Total = value2;
                                    newHb.UpdatedBy = session.UserId;
                                    newHb.UpdatedDate = DateTime.Now;
                                    newHb.Type = budgetType;
                                    newHb.Status = "Saved";
                                    newHb.Save();
                                    JScriptAlertMsg("B52" + budgetType + " Updated");
                                }
                            }
                        }

                    }
                    rowIndex++;
                }
                //}
                //catch (Exception ex) { JScriptAlertMsg(ex.ToString()); }
            }
        }

        #region dimension drop down list selected index changed event in search 
        protected void ddlYearMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC ggdal = new GMSGeneralDALC();
                DataSet dsDim1 = new DataSet(); DataSet dsDim2 = new DataSet();
                DataSet dsDim3 = new DataSet(); DataSet dsDim4 = new DataSet();
                short dim1 = -1; short dim2 = -1; short dim3 = -1;

                this.ddlSearchDim1.Items.Clear(); this.ddlSearchDim2.Items.Clear();
                this.ddlSearchDim3.Items.Clear(); this.ddlSearchDim4.Items.Clear();

                ddlSearchDim2.Enabled = false;
                ddlSearchDim3.Enabled = false;
                ddlSearchDim4.Enabled = false;

                ggdal.SelectHrBudgetDim(session.CompanyId, session.UserId, "Dim1", short.Parse(this.ddlYear.SelectedValue),
                 dim1, dim2, dim3, ref dsDim1);
                if (dsDim1.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsDim1.Tables[0].Rows.Count; j++)
                    {
                        ddlSearchDim1.Items.Add(new DropDownListItem(dsDim1.Tables[0].Rows[j]["Dim1Name"].ToString(), dsDim1.Tables[0].Rows[j]["Dim1"].ToString()));
                    }

                    ggdal.SelectHrBudgetDim(session.CompanyId, session.UserId, "Dim2", short.Parse(this.ddlYear.SelectedValue),
                     short.Parse(ddlSearchDim1.SelectedValue), dim2, dim3, ref dsDim2);
                    foreach (DataRow dr in dsDim2.Tables[0].Rows)
                    {
                        this.ddlSearchDim2.Items.Add(new DropDownListItem(dr["Dim2Name"].ToString(), dr["Dim2"].ToString()));
                    }
                    ddlSearchDim2.Enabled = true;
                    //Bind Section if Department > 0
                    if (dsDim2.Tables[0].Rows.Count > 0)
                    {
                        ggdal.SelectHrBudgetDim(session.CompanyId, session.UserId, "Dim3", short.Parse(this.ddlYear.SelectedValue),
                            short.Parse(ddlSearchDim1.SelectedValue), short.Parse(ddlSearchDim2.SelectedValue), dim3, ref dsDim3);
                        foreach (DataRow dr in dsDim3.Tables[0].Rows)
                        {
                            this.ddlSearchDim3.Items.Add(new DropDownListItem(dr["Dim3Name"].ToString(), dr["Dim3"].ToString()));
                        }
                        ddlSearchDim3.Enabled = true;

                        if (dsDim3.Tables[0].Rows.Count > 0)
                        {
                            ggdal.SelectHrBudgetDim(session.CompanyId, session.UserId, "Dim4", short.Parse(this.ddlYear.SelectedValue),
                                short.Parse(ddlSearchDim1.SelectedValue), short.Parse(ddlSearchDim2.SelectedValue), short.Parse(ddlSearchDim3.SelectedValue), ref dsDim4);
                            foreach (DataRow dr in dsDim4.Tables[0].Rows)
                            {
                                this.ddlSearchDim4.Items.Add(new DropDownListItem(dr["Dim4Name"].ToString(), dr["Dim4"].ToString()));
                            }
                            ddlSearchDim4.Enabled = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.ToString());
            }
            this.RadMultiPage1.Visible = false;
            this.RadTabStrip1.Visible = false;
            this.hidYear.Value = this.ddlYear.SelectedValue;
            this.hidBudgetYear.Value = this.ddlYear.SelectedText;
        }

        protected void ddlDim1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet dsDim2 = new DataSet();
            short dim2 = -1; short dim3 = -1;
            this.ddlSearchDim2.Items.Clear(); this.ddlSearchDim3.Items.Clear(); this.ddlSearchDim4.Items.Clear();

            ddlSearchDim2.Enabled = true;
            ddlSearchDim3.Enabled = false;
            ddlSearchDim4.Enabled = false;

            ggdal.SelectHrBudgetDim(session.CompanyId, session.UserId, "Dim2", short.Parse(this.ddlYear.SelectedValue),
                         short.Parse(ddlSearchDim1.SelectedValue), dim2, dim3, ref dsDim2);
            foreach (DataRow dr in dsDim2.Tables[0].Rows)
            {
                this.ddlSearchDim2.Items.Add(new DropDownListItem(dr["Dim2Name"].ToString(), dr["Dim2"].ToString()));
            }
            this.RadMultiPage1.Visible = false;
            this.RadTabStrip1.Visible = false;
        }

        protected void ddlDim2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet dsDim3 = new DataSet();
            short dim3 = -1;
            this.ddlSearchDim3.Items.Clear(); this.ddlSearchDim4.Items.Clear();

            ddlSearchDim3.Enabled = true;
            ddlSearchDim4.Enabled = false;

            ggdal.SelectHrBudgetDim(session.CompanyId, session.UserId, "Dim3", short.Parse(this.ddlYear.SelectedValue),
                         short.Parse(ddlSearchDim1.SelectedValue), short.Parse(ddlSearchDim2.SelectedValue), dim3, ref dsDim3);
            foreach (DataRow dr in dsDim3.Tables[0].Rows)
            {
                this.ddlSearchDim3.Items.Add(new DropDownListItem(dr["Dim3Name"].ToString(), dr["Dim3"].ToString()));
            }
            this.RadMultiPage1.Visible = false;
            this.RadTabStrip1.Visible = false;
        }

        protected void ddlDim3_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet dsDim4 = new DataSet();
            this.ddlSearchDim4.Items.Clear();

            ddlSearchDim4.Enabled = true;

            ggdal.SelectHrBudgetDim(session.CompanyId, session.UserId, "Dim4", short.Parse(this.ddlYear.SelectedValue),
                         short.Parse(ddlSearchDim1.SelectedValue), short.Parse(ddlSearchDim2.SelectedValue), short.Parse(ddlSearchDim3.SelectedValue), ref dsDim4);
            foreach (DataRow dr in dsDim4.Tables[0].Rows)
            {
                this.ddlSearchDim4.Items.Add(new DropDownListItem(dr["Dim4Name"].ToString(), dr["Dim4"].ToString()));
            }
            this.RadMultiPage1.Visible = false;
            this.RadTabStrip1.Visible = false;
        }
        #endregion

        #region RadSpreadsheet
        protected void LoadHrTemplate()
        {
            LogSession session = base.GetSessionInfo();
            var path = @"D:/GMSDocuments/HrBudget/" + session.CompanyId + "/HrTemplate_"+ this.ddlType.SelectedValue +".xlsx";
            if (File.Exists(path))
            {
                SpreadsheetDocumentProvider provider = new SpreadsheetDocumentProvider(path);

                this.lblSearchSummary.Text = "***This template is used for distribution of amount into B52B. Data entered here will not be saved.";
                try { RadSpreadsheet1.Provider = provider; } catch (Exception ex) { }
            }
            else
            {
                //JScriptAlertMsg("Template not found!");
            }
        }

        protected void LoadB52Spreadsheet(string budgetType)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            short dim1 = -1; short dim2 = -1; short dim3 = -1; short dim4 = -1;
            string d1 = ""; string d2 = ""; string d3 = ""; string d4 = "";
            if (!string.IsNullOrEmpty(this.ddlSearchDim1.SelectedValue))
            {
                dim1 = short.Parse(this.ddlSearchDim1.SelectedValue);
                d1 = this.ddlSearchDim1.SelectedText;
            }
            if (!string.IsNullOrEmpty(this.ddlSearchDim2.SelectedValue))
            {
                dim2 = short.Parse(this.ddlSearchDim2.SelectedValue);
                d2 = this.ddlSearchDim2.SelectedText;
            }
            if (!string.IsNullOrEmpty(this.ddlSearchDim3.SelectedValue))
            {
                dim3 = short.Parse(this.ddlSearchDim3.SelectedValue);
                d3 = this.ddlSearchDim3.SelectedText;
            }
            if (!string.IsNullOrEmpty(this.ddlSearchDim4.SelectedValue))
            {
                dim4 = short.Parse(this.ddlSearchDim4.SelectedValue);
                d4 = this.ddlSearchDim4.SelectedText;
            }

            short year = short.Parse(DateTime.Now.Year.ToString());
            if (!string.IsNullOrEmpty(this.hidYear.Value))
            {
                if (budgetType == "B")
                    year = short.Parse((int.Parse(this.hidYear.Value) + 1).ToString());
                else if (budgetType == "F")
                    year = short.Parse(this.hidYear.Value);
                else if (budgetType == "A")
                    year = short.Parse((int.Parse(this.hidYear.Value) - 1).ToString());
            }
            string yearFormat = year + "/" + (int.Parse(year.ToString()) + 1).ToString().Substring(2);

            DataSet ds = new DataSet();

            string companyName = "";
            try
            {
                ggdal.RetrieveCompanyName(session.CompanyId, ref ds);
                companyName = ds.Tables[0].Rows[0][0].ToString();
                ds.Reset();
            }
            catch (Exception ex) { }

            ggdal.SelectHrBudgetReport(session.CompanyId, session.UserId, this.ddlType.SelectedValue, year, dim1, dim2, dim3, dim4, budgetType, ref ds);

            if (ds.Tables[0].Rows.Count > 0 || ds != null)
            {
                var sheet1 = FillWorksheetForBudget(ds.Tables[0], session,
                     yearFormat, d1, d2, d3, d4, budgetType, companyName, this.ddlType.SelectedText);
                var workbook2 = new Workbook();
                if (budgetType == "F")
                {
                    this.RadSpreadsheet3.Sheets.Add(sheet1);
                    workbook2.Sheets = RadSpreadsheet3.Sheets;
                }
                else if (budgetType == "B")
                {
                    this.RadSpreadsheet2.Sheets.Add(sheet1);
                    workbook2.Sheets = RadSpreadsheet2.Sheets;
                }
                else if (budgetType == "A")
                {
                    this.RadSpreadsheet4.Sheets.Add(sheet1);
                    workbook2.Sheets = RadSpreadsheet4.Sheets;
                }

                SpreadsheetStylingB52(workbook2, this.ddlType.SelectedValue, budgetType);
                var json2 = workbook2.ToJson();
                if (budgetType == "F")
                    HiddenField3.Value = json2;
                else if (budgetType == "B")
                    HiddenField2.Value = json2;
            }
        }

        private static Worksheet FillWorksheetForBudget(DataTable data, LogSession session, string year,
           string d1, string d2, string d3, string d4, string budgetType, string companyName, string hrType)
        {
            var workbook = new Workbook();
            var sheet = workbook.AddSheet();
            sheet.Columns = new List<Column>();
            sheet.FrozenRows = 5;//freeze row when scrolling
            int totalCol = 17;

            #region first row header
            var firstRow = new Row() { Index = 0 };
            var cellCoy = new Cell() { Index = 0, Value = companyName, Bold = true };
            firstRow.AddCell(cellCoy);
            for (int i = 6; i <= totalCol; i++)//fill row with cell first
            {
                var cell = new Cell() { Index = i, Value = "", Bold = true };
                firstRow.AddCell(cell);
            }
            var cellYear = new Cell() { Index = 16, Value = "For Year: " + year, Bold = true };
            firstRow.AddCell(cellYear);
            sheet.AddRow(firstRow);
            sheet.AddMergedCells("A1:F1");
            sheet.AddMergedCells("Q1:R1");
            #endregion

            #region second row header
            var secondRow = new Row() { Index = 1 };
            if (budgetType == "B")
                secondRow.Height = 40;
            string title = "";
            if (budgetType == "B")
                title = "B52B HR Budget ";
            else if (budgetType == "F")
                title = "B52F HR Forecast";
            else if (budgetType == "A")
                title = "B52A HR Actual";
            var cellInFirstRow = new Cell() { Index = 0, Value = title, Bold = true };
            secondRow.AddCell(cellInFirstRow);
            for (int i = 2; i <= totalCol; i++)
            {
                var cell = new Cell() { Index = i, Value = "", Bold = true };
                secondRow.AddCell(cell);
            }
            var cellDim1 = new Cell() { Index = 3, Value = "D1: " + d1, Bold = true, Wrap = true };
            secondRow.AddCell(cellDim1);
            var cellDim2 = new Cell() { Index = 6, Value = "D2: " + d2, Bold = true, Wrap = true };
            secondRow.AddCell(cellDim2);
            var cellDim3 = new Cell() { Index = 9, Value = "D3: " + d3, Bold = true, Wrap = true };
            secondRow.AddCell(cellDim3);
            var cellDim4 = new Cell() { Index = 12, Value = "D4: " + d4, Bold = true, Wrap = true };
            secondRow.AddCell(cellDim4);

            if (budgetType == "B")
            {
                var cellSalary = new Cell() { Index = 13, Value = "Salary Inc:", Bold = true, Wrap = true, TextAlign = "Right" };
                secondRow.AddCell(cellSalary);
                var cellSalaryInc = new Cell() { Index = 14, Value = 1.02, Bold = true, TextAlign = "center" };
                secondRow.AddCell(cellSalaryInc);
                var cellBonus = new Cell() { Index = 15, Value = "Bonus Prov:", Bold = true, Wrap = true, TextAlign = "Right" };
                secondRow.AddCell(cellBonus);
                var cellBousProv = new Cell() { Index = 16, Value = 1.5, Bold = true, TextAlign = "center" };
                secondRow.AddCell(cellBousProv);
                var cellMth = new Cell() { Index = 17, Value = "Mth", Bold = true, TextAlign = "left" };
                secondRow.AddCell(cellMth);
            }
            sheet.AddRow(secondRow);
            sheet.AddMergedCells("A2:C2");
            //sheet.AddMergedCells("D2:E2");
            //sheet.AddMergedCells("G2:H2");
            //sheet.AddMergedCells("J2:K2");
            //sheet.AddMergedCells("M2:N2");
            #endregion

            #region data column header
            var row = new Row() { Index = 2 };
            int columnIndex = 0;
            foreach (DataColumn dataColumn in data.Columns)
            {
                sheet.Columns.Add(new Column());
                string cellValue = dataColumn.ColumnName;
                var cell = new Cell() { Index = columnIndex++, Value = cellValue, Bold = true, TextAlign = "center" };
                row.AddCell(cell);
            }
            sheet.AddRow(row);

            var row2 = new Row() { Index = 3 };
            for (int i = 0; i <= totalCol; i++)
            {
                var cell = new Cell() { Index = i, Value = "", Bold = true };
                row2.AddCell(cell);
            }
            for (int i = 4; i <= 15; i++)//set month is actual or forecast or budget
            {
                if (budgetType == "B")
                {
                    var cell = new Cell() { Index = i, Value = "Budget", Bold = true, TextAlign = "center" };
                    row2.AddCell(cell);
                }
                else if (budgetType == "F")
                {
                    Cell cell;
                    if (i <= 10)
                        cell = new Cell() { Index = i, Value = "Actual", Bold = true, TextAlign = "center" };
                    else
                        cell = new Cell() { Index = i, Value = "Forecast", Bold = true, TextAlign = "center" };
                    row2.AddCell(cell);
                }
                else if (budgetType == "A")
                {
                    var cell = new Cell() { Index = i, Value = "Actual", Bold = true, TextAlign = "center" };
                    row2.AddCell(cell);
                }
            }
            sheet.AddRow(row2);
            if (budgetType == "B" || budgetType == "A")
                sheet.AddMergedCells("E4:P4");
            else if (budgetType == "F")
            {
                sheet.AddMergedCells("E4:K4");
                sheet.AddMergedCells("L4:P4");
            }
            for (int i = 1; i <= 18; i++)
            {
                if (i <= 4 || i > 16)
                {
                    var alphabet = IntToAlpha(i);
                    sheet.AddMergedCells(alphabet + "3:" + alphabet + "4");
                }
            }

            var row3 = new Row() { Index = 4 };
            for (int i = 0; i <= totalCol; i++)
            {
                string hrTypeValue = "";
                if (i == 0)
                    hrTypeValue = hrType;
                var cell = new Cell() { Index = i, Value = hrTypeValue, Bold = true, TextAlign = "center" };
                row3.AddCell(cell);
            }

            sheet.AddRow(row3);
            sheet.AddMergedCells("A5:R5");

            #endregion

            int rowIndex = 5;
            #region load monthly figures
            foreach (DataRow dataRow in data.Rows)
            {
                row = new Row() { Index = rowIndex++ };
                columnIndex = 0;
                foreach (DataColumn dataColumn in data.Columns)
                {
                    string cellValue = dataRow[dataColumn.ColumnName].ToString();
                    var cell = new Cell() { Index = columnIndex++, Value = cellValue, Format = "#,##0" };
                    row.AddCell(cell);
                }
                sheet.AddRow(row);
            }
            #endregion

            #region add total row
            row = new Row() { Index = rowIndex++ };
            columnIndex = 0;
            for (columnIndex = 0; columnIndex <= 40; columnIndex++)
            {
                var alphabet = IntToAlpha(columnIndex + 1);

                if (columnIndex == 2)
                {
                    var cell = new Cell() { Index = columnIndex, Value = "Total", Bold = true };
                    row.AddCell(cell);
                }
                else if (columnIndex >= 4 && columnIndex <= 16)
                {
                    var cell = new Cell() { Index = columnIndex, Value = 0, Bold = true, Formula = "IFERROR(SUM(" + alphabet + "6:" + alphabet + (rowIndex - 1) + "),0)", Format = "#,###" };
                    row.AddCell(cell);
                }
                else
                {
                    var cell = new Cell() { Index = columnIndex, Value = "" };
                    row.AddCell(cell);
                }
            }
            sheet.AddRow(row);
            #endregion

            sheet.Columns[0].Width = 30;
            sheet.Columns[1].Width = 65;
            sheet.Columns[2].Width = 200;
            for (int i = 3; i < sheet.Columns.Count; i++)
            {
                sheet.Columns[i].Width = 60;
            }
            return sheet;
        }

        protected void SpreadsheetStylingB52(Workbook workbook, string type, string budgetType)
        {
            #region header style
            string color = "";
            switch (type)//display title color based on HR item type
            {
                case "A":
                    color = "#bdd7ee";
                    break;
                case "B":
                    color = "#ffcc66";
                    break;
                case "C":
                    color = "#ffccff";
                    break;
                case "D":
                    color = "#c6e0b4";
                    break;
                case "E":
                    color = "#ffff00";
                    break;
                default:
                    color = "#FFFFFF";
                    break;
            }
            foreach (var cell in workbook.Sheets[0].Rows[4].Cells)
            {
                cell.Wrap = true;
                cell.Bold = true;
                cell.Background = color;
                cell.Color = "#000000";
                cell.TextAlign = "center";
                cell.VerticalAlign = "center";
            }

            #endregion

            #region item style
            for (int i = 3; i < workbook.Sheets[0].Rows.Count; i++)
            {
                var row = workbook.Sheets[0].Rows[i];
                row.Cells[0].TextAlign = "center";
                row.Cells[1].TextAlign = "center";
                row.Cells[3].TextAlign = "center";
                //set all cell to double to enable formula calculation
                for (int j = 4; j <= 16; j++)
                {
                    if (!string.IsNullOrEmpty(row.Cells[j].Value.ToString()))
                        row.Cells[j].Value = double.Parse(row.Cells[j].Value.ToString());
                }
                foreach (var cell in row.Cells)
                {
                    cell.Enable = true;
                    cell.FontSize = 12;
                    //vertical align
                    cell.VerticalAlign = "center";
                    cell.Color = "#000000";//pure black font color
                    cell.Format = "#,##0";
                }
                //text align
                for (int j = 4; j <= 17; j++)
                {
                    row.Cells[j].TextAlign = "right";
                }
                if (i > 4)
                {
                    if (i < workbook.Sheets[0].Rows.Count - 1)
                    {
                        //formula for forecast
                        if (budgetType == "F")
                        {
                            for (int j = 11; j <= 15; j++)//for nov to mar month column
                            {
                                string formula = "";
                                string id = row.Cells[20].Value.ToString().Trim();
                                //determine row that follow oct data by itemid
                                List<string> sameAsLastMonth = new List<string> { "55", "56", "58", "60",
                                                                                "71","72","74","76",
                                                                                "92","93","113","114",
                                                                                "116","117","28","29",
                                                                                "34","35","135","136",
                                                                                "138","138","1","2","7","8"};
                                if (sameAsLastMonth.Contains(id))
                                    formula = "IFERROR(K" + (i + 1).ToString() + ",0)";
                                else
                                    formula = "IFERROR(AVERAGE(E" + (i + 1).ToString() + ":K" + (i + 1).ToString() + "),0)";
                                if (row.Cells[j].Value.ToString() == "0")
                                    row.Cells[j].Formula = formula;
                            }
                        }
                        else if (budgetType == "B")//formula for budget
                        {
                            string sn = row.Cells[0].Value.ToString();
                            if (sn == "1")
                            {
                                for (int j = 7; j <= 15; j++)
                                {
                                    string formula = "";
                                    formula = "IFERROR(G" + (i + 1).ToString() + "*O2,0)";
                                    if (string.IsNullOrEmpty(row.Cells[19].Value.ToString()))
                                        row.Cells[j].Formula = formula;
                                }
                            }
                            else if (sn == "2")
                            {
                                for (int j = 4; j <= 15; j++)
                                {
                                    string formula = "";
                                    formula = "IFERROR(M6*Q2/12,0)";
                                    if (string.IsNullOrEmpty(row.Cells[19].Value.ToString()))
                                        row.Cells[j].Formula = formula;
                                }
                            }
                        }
                    }
                    //total of 12 month formula
                    row.Cells[16].Formula = "IFERROR(SUM(E" + (i + 1) + ":P" + (i + 1) + "),0)";
                    if (i != workbook.Sheets[0].Rows.Count - 1)
                    {//Inc% formula
                        row.Cells[17].Format = "0 %";
                        row.Cells[17].Formula = "IFERROR(((Q" + (i + 1) + "-S" + (i + 1) + ")/Q" + (i + 1) + "),0)";
                    }
                }
            }
            #endregion
        }

        protected static string IntToAlpha(int x)
        {
            int lowChar;
            StringBuilder result = new StringBuilder();
            do
            {
                lowChar = (x - 1) % 26;
                x = (x - 1) / 26;
                result.Insert(0, (char)(lowChar + 65));
            } while (x > 0);
            return result.ToString();
        }
        #endregion
    }
}