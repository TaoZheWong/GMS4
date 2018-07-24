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
using System.Data.SqlClient;
using System.Globalization;



using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Finance.CashFlow
{
    public partial class CashFlowProjPopUp : GMSBasePage
    {
        private const string SCRIPT_DOFOCUS =
        @"window.setTimeout('DoFocus()', 1);
        function DoFocus()
        {
            try {
                
                document.getElementById('REQUEST_LASTFOCUS').focus();
                document.getElementById('REQUEST_LASTFOCUS').select();
                
            } catch (ex) {}
        }";

        private const int DaysPerWeek = 7;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            117);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyFinance"));

            if (!IsPostBack)
            {
                LoadDDLs();
                LoadCashFlowProjectionData();
                HookOnFocus(this.Page as Control);
            }

            ScriptManager.RegisterStartupScript(this, typeof(Page), "ScriptDoFocus", SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]), true);



        }

        private void HookOnFocus(Control CurrentControl)
        {
            //checks if control is one of TextBox, DropDownList, ListBox or Button
            if ((CurrentControl is TextBox) ||
                (CurrentControl is DropDownList) ||
                (CurrentControl is ListBox) ||
                (CurrentControl is Button))
                //adds a script which saves active control on receiving focus 
                //in the hidden field __LASTFOCUS.
                (CurrentControl as WebControl).Attributes.Add(
                   "onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
            //checks if the control has children
            if (CurrentControl.HasControls())
                //if yes do them all recursively
                foreach (Control CurrentChildControl in CurrentControl.Controls)
                    HookOnFocus(CurrentChildControl);
        }


        protected void LoadCashFlowProjectionData()
        {

            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsCashFlowProjections = new DataSet();
            short year = 0;
            short month = 0;
            short week = 0;

            for (int i = 1; i < 9; i++)
            {
                HiddenField hidYear1 = (HiddenField)upCashFlowData.FindControl("hidYear" + i);
                year = GMSUtil.ToShort(hidYear1.Value);
                HiddenField hidMonth1 = (HiddenField)upCashFlowData.FindControl("hidMonth" + i);
                month = GMSUtil.ToShort(hidMonth1.Value);
                Label lblWeek1 = (Label)upCashFlowData.FindControl("lblWeek" + i);
                week = GMSUtil.ToShort(lblWeek1.Text);

                dsCashFlowProjections.Clear();

                dacl.GetCashFlowProjection(session.CompanyId, year, week, ref dsCashFlowProjections);
                if (dsCashFlowProjections.Tables[0].Rows.Count > 0)
                {
                    LoadCashFlowProjectionDataByWeek(dsCashFlowProjections, i);
                }
                else
                {
                    ResetCashFlowProjectionDataByWeek(i);
                }
            }


        }

        protected void ResetCashFlowProjectionDataByWeek(int i)
        {
            //Cash Inflow From Operating Activities

            TextBox txtCFS1 = (TextBox)upCashFlowData.FindControl("txtCFS" + i);
            txtCFS1.Text = "0".ToString();
            TextBox txtOI1 = (TextBox)upCashFlowData.FindControl("txtOI" + i);
            txtOI1.Text = "0".ToString();
            Label lblTCI1 = (Label)upCashFlowData.FindControl("lblTCI" + i);
            lblTCI1.Text = "0".ToString();

            //Less Cash Outflow from Operating Activities
            TextBox txtPTOS1 = (TextBox)upCashFlowData.FindControl("txtPTOS" + i);
            txtPTOS1.Text = "0".ToString();
            TextBox txtPTLS1 = (TextBox)upCashFlowData.FindControl("txtPTLS" + i);
            txtPTLS1.Text = "0".ToString();
            TextBox txtSC1 = (TextBox)upCashFlowData.FindControl("txtSC" + i);
            txtSC1.Text = "0".ToString();
            TextBox txtSP1 = (TextBox)upCashFlowData.FindControl("txtSP" + i);
            txtSP1.Text = "0".ToString();
            TextBox txtOP1 = (TextBox)upCashFlowData.FindControl("txtOP" + i);
            txtOP1.Text = "0".ToString();
            TextBox txtTP1 = (TextBox)upCashFlowData.FindControl("txtTP" + i);
            txtTP1.Text = "0".ToString();

            Label lblTOE1 = (Label)upCashFlowData.FindControl("lblTOE" + i);
            lblTOE1.Text = "0".ToString();

            Label lblNPCF1 = (Label)upCashFlowData.FindControl("lblNPCF" + i);
            lblNPCF1.Text = "0".ToString();





            //Less/Add Cash Flow from Investing Activities
            TextBox txtPOFA1 = (TextBox)upCashFlowData.FindControl("txtPOFA" + i);
            txtPOFA1.Text = "0".ToString();
            TextBox txtIO1 = (TextBox)upCashFlowData.FindControl("txtIO" + i);
            txtIO1.Text = "0".ToString();

            TextBox txtDOFA1 = (TextBox)upCashFlowData.FindControl("txtDOFA" + i);
            txtDOFA1.Text = "0".ToString();

            TextBox txtDOIO1 = (TextBox)upCashFlowData.FindControl("txtDOIO" + i);
            txtDOIO1.Text = "0".ToString();
            TextBox txtLTI1 = (TextBox)upCashFlowData.FindControl("txtLTI" + i);
            txtLTI1.Text = "0".ToString();
            TextBox txtIR1 = (TextBox)upCashFlowData.FindControl("txtIR" + i);
            txtIR1.Text = "0".ToString();
            TextBox txtDR1 = (TextBox)upCashFlowData.FindControl("txtDR" + i);
            txtDR1.Text = "0".ToString();
            TextBox txtDP1 = (TextBox)upCashFlowData.FindControl("txtDP" + i);
            txtDP1.Text = "0".ToString();

            Label lblNCFFI1 = (Label)upCashFlowData.FindControl("lblNCFFI" + i);
            lblNCFFI1.Text = "0".ToString();


            //Less/Add Financing Activities
            TextBox txtPOBL1 = (TextBox)upCashFlowData.FindControl("txtPOBL" + i);
            txtPOBL1.Text = "0".ToString();
            TextBox txtROBL1 = (TextBox)upCashFlowData.FindControl("txtROBL" + i);
            txtROBL1.Text = "0".ToString();
            TextBox txtROTF1 = (TextBox)upCashFlowData.FindControl("txtROTF" + i);
            txtROTF1.Text = "0".ToString();
            TextBox txtPOI1 = (TextBox)upCashFlowData.FindControl("txtPOI" + i);
            txtPOI1.Text = "0".ToString();
            TextBox txtNCCL1 = (TextBox)upCashFlowData.FindControl("txtNCCL" + i);
            txtNCCL1.Text = "0".ToString();
            TextBox txtLFI1 = (TextBox)upCashFlowData.FindControl("txtLFI" + i);
            txtLFI1.Text = "0".ToString();
            TextBox txtROIL1 = (TextBox)upCashFlowData.FindControl("txtROIL" + i);
            txtROIL1.Text = "0".ToString();

            Label lblNCFFF1 = (Label)upCashFlowData.FindControl("lblNCFFF" + i);
            lblNCFFF1.Text = "0".ToString();

            Label lblNCFSD1 = (Label)upCashFlowData.FindControl("lblNCFSD" + i);
            lblNCFSD1.Text = "0".ToString();
        }

        protected void LoadCashFlowProjectionDataByWeek(DataSet dsCashFlowProjections, int i)
        {

            //Cash Inflow From Operating Activities

            lblPreparedBy.Text = dsCashFlowProjections.Tables[0].Rows[1]["username"].ToString();

            TextBox txtCFS1 = (TextBox)upCashFlowData.FindControl("txtCFS" + i);
            txtCFS1.Text = dsCashFlowProjections.Tables[0].Rows[1]["Total"].ToString();
            TextBox txtOI1 = (TextBox)upCashFlowData.FindControl("txtOI" + i);
            txtOI1.Text = dsCashFlowProjections.Tables[0].Rows[2]["Total"].ToString();
            Label lblTCI1 = (Label)upCashFlowData.FindControl("lblTCI" + i);
            lblTCI1.Text = dsCashFlowProjections.Tables[0].Rows[3]["Total"].ToString();


            //Less Cash Outflow from Operating Activities
            TextBox txtPTOS1 = (TextBox)upCashFlowData.FindControl("txtPTOS" + i);
            txtPTOS1.Text = dsCashFlowProjections.Tables[0].Rows[5]["Total"].ToString();
            TextBox txtPTLS1 = (TextBox)upCashFlowData.FindControl("txtPTLS" + i);
            txtPTLS1.Text = dsCashFlowProjections.Tables[0].Rows[6]["Total"].ToString();
            TextBox txtSC1 = (TextBox)upCashFlowData.FindControl("txtSC" + i);
            txtSC1.Text = dsCashFlowProjections.Tables[0].Rows[7]["Total"].ToString();
            TextBox txtSP1 = (TextBox)upCashFlowData.FindControl("txtSP" + i);
            txtSP1.Text = dsCashFlowProjections.Tables[0].Rows[8]["Total"].ToString();
            TextBox txtOP1 = (TextBox)upCashFlowData.FindControl("txtOP" + i);
            txtOP1.Text = dsCashFlowProjections.Tables[0].Rows[9]["Total"].ToString();
            TextBox txtTP1 = (TextBox)upCashFlowData.FindControl("txtTP" + i);
            txtTP1.Text = dsCashFlowProjections.Tables[0].Rows[10]["Total"].ToString();

            Label lblTOE1 = (Label)upCashFlowData.FindControl("lblTOE" + i);
            lblTOE1.Text = dsCashFlowProjections.Tables[0].Rows[11]["Total"].ToString();

            Label lblNPCF1 = (Label)upCashFlowData.FindControl("lblNPCF" + i);
            lblNPCF1.Text = dsCashFlowProjections.Tables[0].Rows[12]["Total"].ToString();





            //Less/Add Cash Flow from Investing Activities
            TextBox txtPOFA1 = (TextBox)upCashFlowData.FindControl("txtPOFA" + i);
            txtPOFA1.Text = dsCashFlowProjections.Tables[0].Rows[14]["Total"].ToString();
            TextBox txtIO1 = (TextBox)upCashFlowData.FindControl("txtIO" + i);
            txtIO1.Text = dsCashFlowProjections.Tables[0].Rows[15]["Total"].ToString();

            TextBox txtDOFA1 = (TextBox)upCashFlowData.FindControl("txtDOFA" + i);
            txtDOFA1.Text = dsCashFlowProjections.Tables[0].Rows[16]["Total"].ToString();

            TextBox txtDOIO1 = (TextBox)upCashFlowData.FindControl("txtDOIO" + i);
            txtDOIO1.Text = dsCashFlowProjections.Tables[0].Rows[17]["Total"].ToString();
            TextBox txtLTI1 = (TextBox)upCashFlowData.FindControl("txtLTI" + i);
            txtLTI1.Text = dsCashFlowProjections.Tables[0].Rows[18]["Total"].ToString();
            TextBox txtIR1 = (TextBox)upCashFlowData.FindControl("txtIR" + i);
            txtIR1.Text = dsCashFlowProjections.Tables[0].Rows[19]["Total"].ToString();
            TextBox txtDR1 = (TextBox)upCashFlowData.FindControl("txtDR" + i);
            txtDR1.Text = dsCashFlowProjections.Tables[0].Rows[20]["Total"].ToString();
            TextBox txtDP1 = (TextBox)upCashFlowData.FindControl("txtDP" + i);
            txtDP1.Text = dsCashFlowProjections.Tables[0].Rows[21]["Total"].ToString();

            Label lblNCFFI1 = (Label)upCashFlowData.FindControl("lblNCFFI" + i);
            lblNCFFI1.Text = dsCashFlowProjections.Tables[0].Rows[22]["Total"].ToString();


            //Less/Add Financing Activities
            TextBox txtPOBL1 = (TextBox)upCashFlowData.FindControl("txtPOBL" + i);
            txtPOBL1.Text = dsCashFlowProjections.Tables[0].Rows[24]["Total"].ToString();
            TextBox txtROBL1 = (TextBox)upCashFlowData.FindControl("txtROBL" + i);
            txtROBL1.Text = dsCashFlowProjections.Tables[0].Rows[25]["Total"].ToString();
            TextBox txtROTF1 = (TextBox)upCashFlowData.FindControl("txtROTF" + i);
            txtROTF1.Text = dsCashFlowProjections.Tables[0].Rows[26]["Total"].ToString();
            TextBox txtPOI1 = (TextBox)upCashFlowData.FindControl("txtPOI" + i);
            txtPOI1.Text = dsCashFlowProjections.Tables[0].Rows[27]["Total"].ToString();
            TextBox txtNCCL1 = (TextBox)upCashFlowData.FindControl("txtNCCL" + i);
            txtNCCL1.Text = dsCashFlowProjections.Tables[0].Rows[28]["Total"].ToString();
            TextBox txtLFI1 = (TextBox)upCashFlowData.FindControl("txtLFI" + i);
            txtLFI1.Text = dsCashFlowProjections.Tables[0].Rows[29]["Total"].ToString();
            TextBox txtROIL1 = (TextBox)upCashFlowData.FindControl("txtROIL" + i);
            txtROIL1.Text = dsCashFlowProjections.Tables[0].Rows[30]["Total"].ToString();

            Label lblNCFFF1 = (Label)upCashFlowData.FindControl("lblNCFFF" + i);
            lblNCFFF1.Text = dsCashFlowProjections.Tables[0].Rows[31]["Total"].ToString();

            Label lblNCFSD1 = (Label)upCashFlowData.FindControl("lblNCFSD" + i);
            lblNCFSD1.Text = dsCashFlowProjections.Tables[0].Rows[32]["Total"].ToString();





        }



        protected void LoadDDLs()
        {
            LogSession session = base.GetSessionInfo();
            // load year ddl
            DataTable dtt1 = new DataTable();
            dtt1.Columns.Add("Year", typeof(string));

            for (int i = -11; i < 10; i++)
            {
                DataRow dr1 = dtt1.NewRow();
                dr1["Year"] = DateTime.Now.Year + i;

                dtt1.Rows.Add(dr1);
            }

            this.ddlYear.DataSource = dtt1;
            this.ddlYear.DataBind();
            this.ddlYear.SelectedValue = DateTime.Now.Year.ToString();

            // load month ddl
            DataTable dtt2 = new DataTable();
            dtt2.Columns.Add("Week", typeof(string));
            dtt2.Columns.Add("WeekRange", typeof(string));

            for (int i = 1; i < 53; i++)
            {
                //DateTime startDate = new DateTime(DateTime.Now.Year.ToString(), month, 1).AddDays((week - 1) * 7);
                //DateTime endDate = startDate.AddDays(6); 

                DataRow dr2 = dtt2.NewRow();
                string dateRange = GetFirstDayInWeek(DateTime.Now.Year, i);

                dr2["Week"] = i;
                dr2["WeekRange"] = "Week " + i + " (" + dateRange + ")";


                dtt2.Rows.Add(dr2);
            }



            this.ddlWeek.DataSource = dtt2;
            this.ddlWeek.DataBind();
            this.ddlWeek.SelectedValue = GetWeekNumber(DateTime.Now).ToString();


            int startweek = GetWeekNumber(DateTime.Now);
            int startyear = DateTime.Now.Year;

            populateHeader(startweek, startyear);

        }

        public string GetDateTime(int year, int week)
        {
            int yearsOffset = GetYearsOffset(year);

            int dayNumberOfFirstDayInWeek = ((week - 1) * DaysPerWeek) - yearsOffset;
            DateTime firstDayInYear = new DateTime(year, 01, 01);
            DateTime firstDayInWeek = firstDayInYear.AddDays(dayNumberOfFirstDayInWeek);
            DateTime LastDayInWeek = firstDayInWeek.AddDays(6);

            
            return String.Format("{0:d-MMM-yy}", firstDayInWeek) + "<br /> to <br />" + String.Format("{0:d-MMM-yy}", LastDayInWeek);
        }

        public string GetDateRangeForDDL(int year, int week, int day)
        {
            /* 
            CultureInfo cultureInfo = new CultureInfo("en-US");
            DateTime firstDayOfYear = new DateTime(year, 1, 1);
            int firstWeek = cultureInfo.Calendar.GetWeekOfYear(firstDayOfYear, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);
            int dayOffSet = day - (int)cultureInfo.DateTimeFormat.FirstDayOfWeek + 1;
            DateTime startDateOfWeek = firstDayOfYear.AddDays((week - (firstWeek + 1)) * 7 + dayOffSet + 1);
            DateTime endDateOfWeek = startDateOfWeek.AddDays(day);

            return String.Format("{0:d-MMM-yy}", startDateOfWeek) + " to " + String.Format("{0:d-MMM-yy}", endDateOfWeek);
            */
            
            DateTime startOfYear = new DateTime(year, 1, 1);

            // The +7 and %7 stuff is to avoid negative numbers etc.
            int daysToFirstCorrectDay = (((int)day - (int)startOfYear.DayOfWeek) + 7) % 7;

            return (startOfYear.AddDays(7 * (week - 1) + daysToFirstCorrectDay)).ToString();
            


        }







        public string getMonth(int weekNum, int year)
        {
            DateTime Current = new DateTime(year, 1, 1);
            System.DayOfWeek StartDOW = Current.DayOfWeek;
            int DayOfYear = (weekNum * 7) - 6; //1st day of the week
            if (StartDOW != System.DayOfWeek.Sunday) //means that last week of last year's month  
            {
                Current = Current.AddDays(7 - (int)Current.DayOfWeek);
            } return Current.AddDays(DayOfYear).ToString("MMM");
        }

        public int getMonthNum(int weekNum, int year)
        {
            DateTime Current = new DateTime(year, 1, 1);
            System.DayOfWeek StartDOW = Current.DayOfWeek;
            int DayOfYear = (weekNum * 7) - 6; //1st day of the week
            if (StartDOW != System.DayOfWeek.Sunday) //means that last week of last year's month  
            {
                Current = Current.AddDays(7 - (int)Current.DayOfWeek);
            } return Current.AddDays(DayOfYear).Month;
        }


        public static int GetWeekNumber(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        protected void populateHeader(int startweek, int startyear)
        {
            if (startweek > 52)
            {
                lblMonth1.Text = (getMonth(startweek % 52, startyear + 1)).ToString();
                hidMonth1.Value = (getMonthNum(startweek % 52, startyear + 1)).ToString();
                lblWeek1.Text = (startweek % 52).ToString();
                lblYear1.Text = ((startyear + 1).ToString()).Substring(2, 2);
                hidYear1.Value = (startyear + 1).ToString();
                lblRange1.Text = GetDateTime(startyear + 1, startweek % 52);

            }
            else
            {
                lblMonth1.Text = (getMonth(startweek, startyear)).ToString();
                hidMonth1.Value = (getMonthNum(startweek, startyear)).ToString();
                lblWeek1.Text = (startweek).ToString();
                lblYear1.Text = ((startyear).ToString()).Substring(2, 2);
                hidYear1.Value = (startyear).ToString();
                lblRange1.Text = GetDateTime(startyear, startweek);
            }

            if ((startweek + 1) > 52)
            {
                lblMonth2.Text = (getMonth((startweek + 1) % 52, startyear + 1)).ToString();
                hidMonth2.Value = (getMonthNum((startweek + 1) % 52, startyear + 1)).ToString();
                lblWeek2.Text = ((startweek + 1) % 52).ToString();
                lblYear2.Text = ((startyear + 1).ToString()).Substring(2, 2);
                hidYear2.Value = (startyear + 1).ToString();
                lblRange2.Text = GetDateTime(startyear + 1, (startweek + 1) % 52);
            }
            else
            {
                lblMonth2.Text = (getMonth(startweek + 1, startyear)).ToString();
                hidMonth2.Value = (getMonthNum(startweek + 1, startyear)).ToString();
                lblWeek2.Text = (startweek + 1).ToString();
                lblYear2.Text = ((startyear).ToString()).Substring(2, 2);
                hidYear2.Value = (startyear).ToString();
                lblRange2.Text = GetDateTime(startyear, startweek + 1);
            }

            if ((startweek + 2) > 52)
            {
                lblMonth3.Text = (getMonth((startweek + 2) % 52, startyear + 1)).ToString();
                hidMonth3.Value = (getMonthNum((startweek + 2) % 52, startyear + 1)).ToString();
                lblWeek3.Text = ((startweek + 2) % 52).ToString();
                lblYear3.Text = ((startyear + 1).ToString()).Substring(2, 2);
                hidYear3.Value = (startyear + 1).ToString();
                lblRange3.Text = GetDateTime(startyear + 1, (startweek + 2) % 52);
            }
            else
            {
                lblMonth3.Text = (getMonth(startweek + 2, startyear)).ToString();
                hidMonth3.Value = (getMonthNum(startweek + 2, startyear)).ToString();
                lblWeek3.Text = (startweek + 2).ToString();
                lblYear3.Text = ((startyear).ToString()).Substring(2, 2);
                hidYear3.Value = (startyear).ToString();
                lblRange3.Text = GetDateTime(startyear, startweek + 2);
            }

            if ((startweek + 3) > 52)
            {
                lblMonth4.Text = (getMonth((startweek + 3) % 52, startyear + 1)).ToString();
                hidMonth4.Value = (getMonthNum((startweek + 3) % 52, startyear + 1)).ToString();
                lblWeek4.Text = ((startweek + 3) % 52).ToString();
                lblYear4.Text = ((startyear + 1).ToString()).Substring(2, 2);
                hidYear4.Value = (startyear + 1).ToString();
                lblRange4.Text = GetDateTime(startyear + 1, (startweek + 3) % 52);
            }
            else
            {
                lblMonth4.Text = (getMonth(startweek + 3, startyear)).ToString();
                hidMonth4.Value = (getMonthNum(startweek + 3, startyear)).ToString();
                lblWeek4.Text = (startweek + 3).ToString();
                lblYear4.Text = ((startyear).ToString()).Substring(2, 2);
                hidYear4.Value = (startyear).ToString();
                lblRange4.Text = GetDateTime(startyear, startweek + 3);
            }

            if ((startweek + 4) > 52)
            {
                lblMonth5.Text = (getMonth((startweek + 4) % 52, startyear + 1)).ToString();
                hidMonth5.Value = (getMonthNum((startweek + 4) % 52, startyear + 1)).ToString();
                lblWeek5.Text = ((startweek + 4) % 52).ToString();
                lblYear5.Text = ((startyear + 1).ToString()).Substring(2, 2);
                hidYear5.Value = (startyear + 1).ToString();
                lblRange5.Text = GetDateTime(startyear + 1, (startweek + 4) % 52);
            }
            else
            {
                lblMonth5.Text = (getMonth(startweek + 4, startyear)).ToString();
                hidMonth5.Value = (getMonthNum(startweek + 4, startyear)).ToString();
                lblWeek5.Text = (startweek + 4).ToString();
                lblYear5.Text = ((startyear).ToString()).Substring(2, 2);
                hidYear5.Value = (startyear).ToString();
                lblRange5.Text = GetDateTime(startyear, startweek + 4);
            }

            if ((startweek + 5) > 52)
            {
                lblMonth6.Text = (getMonth((startweek + 5) % 52, startyear + 1)).ToString();
                hidMonth6.Value = (getMonthNum((startweek + 5) % 52, startyear + 1)).ToString();
                lblWeek6.Text = ((startweek + 5) % 52).ToString();
                lblYear6.Text = ((startyear + 1).ToString()).Substring(2, 2);
                hidYear6.Value = (startyear + 1).ToString();
                lblRange6.Text = GetDateTime(startyear + 1, (startweek + 5) % 52);
            }
            else
            {
                lblMonth6.Text = (getMonth(startweek + 5, startyear)).ToString();
                hidMonth6.Value = (getMonthNum(startweek + 5, startyear)).ToString();
                lblWeek6.Text = (startweek + 5).ToString();
                lblYear6.Text = ((startyear).ToString()).Substring(2, 2);
                hidYear6.Value = (startyear).ToString();
                lblRange6.Text = GetDateTime(startyear, startweek + 5);
            }


            if ((startweek + 6) > 52)
            {
                lblMonth7.Text = (getMonth((startweek + 6) % 52, startyear + 1)).ToString();
                hidMonth7.Value = (getMonthNum((startweek + 6) % 52, startyear + 1)).ToString();
                lblWeek7.Text = ((startweek + 6) % 52).ToString();
                lblYear7.Text = ((startyear + 1).ToString()).Substring(2, 2);
                hidYear7.Value = (startyear + 1).ToString();
                lblRange7.Text = GetDateTime(startyear + 1, (startweek + 6) % 52);
            }
            else
            {
                lblMonth7.Text = (getMonth(startweek + 6, startyear)).ToString();
                hidMonth7.Value = (getMonthNum(startweek + 6, startyear)).ToString();
                lblWeek7.Text = (startweek + 6).ToString();
                lblYear7.Text = ((startyear).ToString()).Substring(2, 2);
                hidYear7.Value = (startyear).ToString();
                lblRange7.Text = GetDateTime(startyear, startweek + 6);
            }

            if ((startweek + 7) > 52)
            {
                lblMonth8.Text = (getMonth((startweek + 7) % 52, startyear + 1)).ToString();
                hidMonth8.Value = (getMonthNum((startweek + 7) % 52, startyear + 1)).ToString();
                lblWeek8.Text = ((startweek + 7) % 52).ToString();
                lblYear8.Text = ((startyear + 1).ToString()).Substring(2, 2);
                hidYear8.Value = (startyear + 1).ToString();
                lblRange8.Text = GetDateTime(startyear + 1, (startweek + 7) % 52);
            }
            else
            {
                lblMonth8.Text = (getMonth(startweek + 7, startyear)).ToString();
                hidMonth8.Value = (getMonthNum(startweek + 7, startyear)).ToString();
                lblWeek8.Text = (startweek + 7).ToString();
                lblYear8.Text = ((startyear).ToString()).Substring(2, 2);
                hidYear8.Value = (startyear).ToString();
                lblRange8.Text = GetDateTime(startyear, startweek + 7);
            }

        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int startweek = GMSUtil.ToInt(ddlWeek.SelectedValue);
            int startyear = GMSUtil.ToInt(ddlYear.SelectedValue);
            populateHeader(startweek, startyear);
            LoadCashFlowProjectionData();

        }

        protected void InsertCashFlowProjectionData()
        {


            LogSession session = base.GetSessionInfo();

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsCashFlowProjectionData = new DataSet();

            short year = 0;
            short month = 0;
            short week = 0;
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double SalesmanClaim = 0;
            double SalaryPayment = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            for (int i = 1; i < 9; i++)
            {

                HiddenField hidYear1 = (HiddenField)upCashFlowData.FindControl("hidYear" + i);
                year = GMSUtil.ToShort(hidYear1.Value);
                HiddenField hidMonth1 = (HiddenField)upCashFlowData.FindControl("hidMonth" + i);
                month = GMSUtil.ToShort(hidMonth1.Value);
                Label lblWeek1 = (Label)upCashFlowData.FindControl("lblWeek" + i);
                week = GMSUtil.ToShort(lblWeek1.Text);


                //Cash Inflow From Operating Activities
                TextBox txtCFS1 = (TextBox)upCashFlowData.FindControl("txtCFS" + i);
                CollectionFromSales = GMSUtil.ToDouble(txtCFS1.Text);
                TextBox txtOI1 = (TextBox)upCashFlowData.FindControl("txtOI" + i);
                OtherIncome = GMSUtil.ToDouble(txtOI1.Text);
                TotalCashInflow = CollectionFromSales + OtherIncome;


                //Less Cash Outflow from Operating Activities
                TextBox txtPTOS1 = (TextBox)upCashFlowData.FindControl("txtPTOS" + i);
                PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS1.Text);
                TextBox txtPTLS1 = (TextBox)upCashFlowData.FindControl("txtPTLS" + i);
                PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS1.Text);
                TextBox txtSC1 = (TextBox)upCashFlowData.FindControl("txtSC" + i);
                SalesmanClaim = GMSUtil.ToDouble(txtSC1.Text);
                TextBox txtSP1 = (TextBox)upCashFlowData.FindControl("txtSP" + i);
                SalaryPayment = GMSUtil.ToDouble(txtSP1.Text);
                TextBox txtOP1 = (TextBox)upCashFlowData.FindControl("txtOP" + i);
                OtherPayment = GMSUtil.ToDouble(txtOP1.Text);
                TextBox txtTP1 = (TextBox)upCashFlowData.FindControl("txtTP" + i);
                TaxsPayment = GMSUtil.ToDouble(txtTP1.Text);
                TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + SalesmanClaim + SalaryPayment + OtherPayment + TaxsPayment;
                NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;

                //Less/Add Cash Flow from Investing Activities
                TextBox txtPOFA1 = (TextBox)upCashFlowData.FindControl("txtPOFA" + i);
                PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA1.Text);
                TextBox txtIO1 = (TextBox)upCashFlowData.FindControl("txtIO" + i);
                Investments = GMSUtil.ToDouble(txtIO1.Text);
                TextBox txtDOFA = (TextBox)upCashFlowData.FindControl("txtDOFA" + i);
                DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA1.Text);
                TextBox txtDOIO1 = (TextBox)upCashFlowData.FindControl("txtDOIO" + i);
                DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO1.Text);
                TextBox txtLTI1 = (TextBox)upCashFlowData.FindControl("txtLTI" + i);
                LoanToIntercompany = GMSUtil.ToDouble(txtLTI1.Text);
                TextBox txtIR1 = (TextBox)upCashFlowData.FindControl("txtIR" + i);
                InterestReceived = GMSUtil.ToDouble(txtIR1.Text);
                TextBox txtDR1 = (TextBox)upCashFlowData.FindControl("txtDR" + i);
                DividendReceived = GMSUtil.ToDouble(txtDR1.Text);
                TextBox txtDP1 = (TextBox)upCashFlowData.FindControl("txtDP" + i);
                DividendPaid = GMSUtil.ToDouble(txtDP1.Text);
                NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;

                //Less/Add Financing Activities
                TextBox txtPOBL1 = (TextBox)upCashFlowData.FindControl("txtPOBL" + i);
                ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL1.Text);
                TextBox txtROBL1 = (TextBox)upCashFlowData.FindControl("txtROBL" + i);
                RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL1.Text);
                TextBox txtROTF1 = (TextBox)upCashFlowData.FindControl("txtROTF" + i);
                RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF1.Text);
                TextBox txtPOI1 = (TextBox)upCashFlowData.FindControl("txtPOI" + i);
                PaymentOfInterests = GMSUtil.ToDouble(txtPOI1.Text);
                TextBox txtNCCL1 = (TextBox)upCashFlowData.FindControl("txtNCCL" + i);
                NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL1.Text);
                TextBox txtLFI1 = (TextBox)upCashFlowData.FindControl("txtLFI" + i);
                LoanFromIntercompany = GMSUtil.ToDouble(txtLFI1.Text);
                TextBox txtROIL1 = (TextBox)upCashFlowData.FindControl("txtROIL" + i);
                RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL1.Text);

                NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
                NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;


                dacl.InsertCashFlowProjection(session.CompanyId, year, month, week,
                    CollectionFromSales, OtherIncome, TotalCashInflow, PaymentToOverseasSupplier, PaymentToLocalSupplier,
                    SalesmanClaim, SalaryPayment, OtherPayment, TaxsPayment, TotalOperatingExpenses,
                    NetOperatingCashFlow, PurchaseofFixedAssets, Investments, DisposalOfFixedAssets, DisposalOfInvestmentsOthers,
                    LoanToIntercompany, InterestReceived, DividendReceived, DividendPaid, NetCashFlowFromInvesting,
                    ProceedsOfBankLoans, RepaymentOfBankLoans, RepaymentOfTradeFinancing, PaymentOfInterests, NewCapitalConvertibleLoan,
                    LoanFromIntercompany, RepaymentOfIntercompanyLoan, NetCashFlowFromFinancing, NetCashFlowSurplusDeficit,
                    session.UserId, ref dsCashFlowProjectionData);

            }


        }

        private void RemoveCashFlowProjections()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsCashFlowProjections = new DataSet();
            short year = 0;
            short month = 0;
            short week = 0;

            for (int i = 1; i < 9; i++)
            {
                HiddenField hidYear1 = (HiddenField)upCashFlowData.FindControl("hidYear" + i);
                year = GMSUtil.ToShort(hidYear1.Value);
                HiddenField hidMonth1 = (HiddenField)upCashFlowData.FindControl("hidMonth" + i);
                month = GMSUtil.ToShort(hidMonth1.Value);
                Label lblWeek1 = (Label)upCashFlowData.FindControl("lblWeek" + i);
                week = GMSUtil.ToShort(lblWeek1.Text);

                dsCashFlowProjections.Clear();

                dacl.GetCashFlowProjection(session.CompanyId, year, week, ref dsCashFlowProjections);
                if (dsCashFlowProjections.Tables[0].Rows.Count > 0)
                {
                    new GMSGeneralDALC().DeleteCashFlowProj(session.CompanyId, year, week);
                }
            }
            /*
            dsCashFlowProjections.Clear();
            dacl.GetCashFlowProjection(session.CompanyId, GMSUtil.ToShort(lblYear2.Text), GMSUtil.ToShort(lblWeek2.Text), ref dsCashFlowProjections);
            if (dsCashFlowProjections.Tables[0].Rows.Count > 0)
            {
                new GMSGeneralDALC().DeleteCashFlowProj(session.CompanyId, GMSUtil.ToShort(lblYear2.Text), GMSUtil.ToShort(lblWeek2.Text));
            }
            dsCashFlowProjections.Clear();
            dacl.GetCashFlowProjection(session.CompanyId, GMSUtil.ToShort(lblYear3.Text), GMSUtil.ToShort(lblWeek3.Text), ref dsCashFlowProjections);
            if (dsCashFlowProjections.Tables[0].Rows.Count > 0)
            {
                new GMSGeneralDALC().DeleteCashFlowProj(session.CompanyId, GMSUtil.ToShort(lblYear3.Text), GMSUtil.ToShort(lblWeek3.Text));
            }
            dsCashFlowProjections.Clear();
            dacl.GetCashFlowProjection(session.CompanyId, GMSUtil.ToShort(lblYear4.Text), GMSUtil.ToShort(lblWeek4.Text), ref dsCashFlowProjections);
            if (dsCashFlowProjections.Tables[0].Rows.Count > 0)
            {
                new GMSGeneralDALC().DeleteCashFlowProj(session.CompanyId, GMSUtil.ToShort(lblYear4.Text), GMSUtil.ToShort(lblWeek4.Text));
            }
            dsCashFlowProjections.Clear();
            dacl.GetCashFlowProjection(session.CompanyId, GMSUtil.ToShort(lblYear5.Text), GMSUtil.ToShort(lblWeek5.Text), ref dsCashFlowProjections);
            if (dsCashFlowProjections.Tables[0].Rows.Count > 0)
            {
                new GMSGeneralDALC().DeleteCashFlowProj(session.CompanyId, GMSUtil.ToShort(lblYear5.Text), GMSUtil.ToShort(lblWeek5.Text));
            }
            dsCashFlowProjections.Clear();
            dacl.GetCashFlowProjection(session.CompanyId, GMSUtil.ToShort(lblYear6.Text), GMSUtil.ToShort(lblWeek6.Text), ref dsCashFlowProjections);
            if (dsCashFlowProjections.Tables[0].Rows.Count > 0)
            {
                new GMSGeneralDALC().DeleteCashFlowProj(session.CompanyId, GMSUtil.ToShort(lblYear6.Text), GMSUtil.ToShort(lblWeek6.Text));
            }
            dsCashFlowProjections.Clear();
            dacl.GetCashFlowProjection(session.CompanyId, GMSUtil.ToShort(lblYear7.Text), GMSUtil.ToShort(lblWeek7.Text), ref dsCashFlowProjections);
            if (dsCashFlowProjections.Tables[0].Rows.Count > 0)
            {
                new GMSGeneralDALC().DeleteCashFlowProj(session.CompanyId, GMSUtil.ToShort(lblYear7.Text), GMSUtil.ToShort(lblWeek7.Text));
            }
            dsCashFlowProjections.Clear();
            dacl.GetCashFlowProjection(session.CompanyId, GMSUtil.ToShort(lblYear8.Text), GMSUtil.ToShort(lblWeek8.Text), ref dsCashFlowProjections);
            if (dsCashFlowProjections.Tables[0].Rows.Count > 0)
            {
                new GMSGeneralDALC().DeleteCashFlowProj(session.CompanyId, GMSUtil.ToShort(lblYear8.Text), GMSUtil.ToShort(lblWeek8.Text));
            }
            */



        }

        protected void btnSubmitData_Click(object sender, EventArgs e)
        {
            RemoveCashFlowProjections();
            InsertCashFlowProjectionData();
            ModalPopupExtender2.Hide();
            LoadCashFlowProjectionData();
            ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Data has been successfully saved!')", true);

        }

        protected void Calculate_OnTextChanged(object sender, EventArgs e)
        {
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double SalesmanClaim = 0;
            double SalaryPayment = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS1.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI1.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI1.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS1.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS1.Text);
            SalesmanClaim = GMSUtil.ToDouble(txtSC1.Text);
            SalaryPayment = GMSUtil.ToDouble(txtSP1.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP1.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP1.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + SalesmanClaim + SalaryPayment + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE1.Text = TotalOperatingExpenses.ToString();
            lblNPCF1.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA1.Text);
            Investments = GMSUtil.ToDouble(txtIO1.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA1.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO1.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI1.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR1.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR1.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP1.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI1.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL1.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL1.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF1.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI1.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL1.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI1.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL1.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF1.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD1.Text = NetCashFlowSurplusDeficit.ToString();


        }

        protected void Calculate2_OnTextChanged(object sender, EventArgs e)
        {
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double SalesmanClaim = 0;
            double SalaryPayment = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS2.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI2.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI2.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS2.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS2.Text);
            SalesmanClaim = GMSUtil.ToDouble(txtSC2.Text);
            SalaryPayment = GMSUtil.ToDouble(txtSP2.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP2.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP2.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + SalesmanClaim + SalaryPayment + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE2.Text = TotalOperatingExpenses.ToString();
            lblNPCF2.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA2.Text);
            Investments = GMSUtil.ToDouble(txtIO2.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA2.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO2.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI2.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR2.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR2.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP2.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI2.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL2.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL2.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF2.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI2.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL2.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI2.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL2.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF2.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD2.Text = NetCashFlowSurplusDeficit.ToString();

        }

        protected void Calculate3_OnTextChanged(object sender, EventArgs e)
        {
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double SalesmanClaim = 0;
            double SalaryPayment = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS3.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI3.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI3.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS3.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS3.Text);
            SalesmanClaim = GMSUtil.ToDouble(txtSC3.Text);
            SalaryPayment = GMSUtil.ToDouble(txtSP3.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP3.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP3.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + SalesmanClaim + SalaryPayment + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE3.Text = TotalOperatingExpenses.ToString();
            lblNPCF3.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA3.Text);
            Investments = GMSUtil.ToDouble(txtIO3.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA3.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO3.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI3.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR3.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR3.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP3.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI3.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL3.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL3.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF3.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI3.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL3.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI3.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL3.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF3.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD3.Text = NetCashFlowSurplusDeficit.ToString();

        }

        protected void Calculate4_OnTextChanged(object sender, EventArgs e)
        {
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double SalesmanClaim = 0;
            double SalaryPayment = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS4.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI4.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI4.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS4.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS4.Text);
            SalesmanClaim = GMSUtil.ToDouble(txtSC4.Text);
            SalaryPayment = GMSUtil.ToDouble(txtSP4.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP4.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP4.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + SalesmanClaim + SalaryPayment + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE4.Text = TotalOperatingExpenses.ToString();
            lblNPCF4.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA4.Text);
            Investments = GMSUtil.ToDouble(txtIO4.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA4.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO4.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI4.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR4.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR4.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP4.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI4.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL4.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL4.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF4.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI4.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL4.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI4.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL4.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF4.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD4.Text = NetCashFlowSurplusDeficit.ToString();

        }

        protected void Calculate5_OnTextChanged(object sender, EventArgs e)
        {
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double SalesmanClaim = 0;
            double SalaryPayment = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS5.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI5.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI5.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS5.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS5.Text);
            SalesmanClaim = GMSUtil.ToDouble(txtSC5.Text);
            SalaryPayment = GMSUtil.ToDouble(txtSP5.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP5.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP5.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + SalesmanClaim + SalaryPayment + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE5.Text = TotalOperatingExpenses.ToString();
            lblNPCF5.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA5.Text);
            Investments = GMSUtil.ToDouble(txtIO5.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA5.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO5.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI5.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR5.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR5.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP5.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI5.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL5.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL5.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF5.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI5.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL5.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI5.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL5.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF5.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD5.Text = NetCashFlowSurplusDeficit.ToString();

        }

        protected void Calculate6_OnTextChanged(object sender, EventArgs e)
        {
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double SalesmanClaim = 0;
            double SalaryPayment = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS6.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI6.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI6.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS6.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS6.Text);
            SalesmanClaim = GMSUtil.ToDouble(txtSC6.Text);
            SalaryPayment = GMSUtil.ToDouble(txtSP6.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP6.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP6.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + SalesmanClaim + SalaryPayment + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE6.Text = TotalOperatingExpenses.ToString();
            lblNPCF6.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA6.Text);
            Investments = GMSUtil.ToDouble(txtIO6.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA6.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO6.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI6.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR6.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR6.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP6.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI6.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL6.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL6.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF6.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI6.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL6.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI6.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL6.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF6.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD6.Text = NetCashFlowSurplusDeficit.ToString();

        }

        protected void Calculate7_OnTextChanged(object sender, EventArgs e)
        {
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double SalesmanClaim = 0;
            double SalaryPayment = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS7.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI7.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI7.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS7.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS7.Text);
            SalesmanClaim = GMSUtil.ToDouble(txtSC7.Text);
            SalaryPayment = GMSUtil.ToDouble(txtSP7.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP7.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP7.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + SalesmanClaim + SalaryPayment + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE7.Text = TotalOperatingExpenses.ToString();
            lblNPCF7.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA7.Text);
            Investments = GMSUtil.ToDouble(txtIO7.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA7.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO7.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI7.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR7.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR7.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP7.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI7.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL7.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL7.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF7.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI7.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL7.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI7.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL7.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF7.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD7.Text = NetCashFlowSurplusDeficit.ToString();

        }

        protected void Calculate8_OnTextChanged(object sender, EventArgs e)
        {
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double SalesmanClaim = 0;
            double SalaryPayment = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS8.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI8.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI8.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS8.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS8.Text);
            SalesmanClaim = GMSUtil.ToDouble(txtSC8.Text);
            SalaryPayment = GMSUtil.ToDouble(txtSP8.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP8.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP8.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + SalesmanClaim + SalaryPayment + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE8.Text = TotalOperatingExpenses.ToString();
            lblNPCF8.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA8.Text);
            Investments = GMSUtil.ToDouble(txtIO8.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA8.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO8.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI8.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR8.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR8.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP8.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI8.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL8.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL8.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF8.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI8.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL8.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI8.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL8.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF8.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD8.Text = NetCashFlowSurplusDeficit.ToString();

        }


        protected void ddlYear_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ddlWeek.Items.Clear();
            int startyear = GMSUtil.ToInt(ddlYear.SelectedValue);


            // load month ddl
            DataTable dtt2 = new DataTable();
            dtt2.Columns.Add("Week", typeof(string));
            dtt2.Columns.Add("WeekRange", typeof(string));

            for (int i = 1; i < 53; i++)
            {

                DataRow dr2 = dtt2.NewRow();
                string dateRange = GetFirstDayInWeek(startyear, i);

                dr2["Week"] = i;
                dr2["WeekRange"] = "Week " + i + " (" + dateRange + ")";


                dtt2.Rows.Add(dr2);
            }

            this.ddlWeek.DataSource = dtt2;
            this.ddlWeek.DataBind();
            this.ddlWeek.SelectedValue = 1.ToString();

        }

        public static string GetFirstDayInWeek(int year, int week)
        {

            int yearsOffset = GetYearsOffset(year);

            int dayNumberOfFirstDayInWeek = ((week - 1) * DaysPerWeek) - yearsOffset;
            DateTime firstDayInYear = new DateTime(year, 01, 01);           

            DateTime firstDayInWeek = firstDayInYear.AddDays(dayNumberOfFirstDayInWeek);
            DateTime LastDayInWeek = firstDayInWeek.AddDays(6);

            return String.Format("{0:d-MMM-yy}", firstDayInWeek) + " to " + String.Format("{0:d-MMM-yy}", LastDayInWeek);

        }

        




        private static int GetYearsOffset(int year)
        {
           
            // create the first day in the year

            DateTime firstDayInYear = new DateTime(year, 01, 01);

            



            // find out what day it is and return the offset

            switch (firstDayInYear.DayOfWeek)
            {

                case (DayOfWeek.Monday):

                    return 0;

                case (DayOfWeek.Tuesday):

                    return 1;

                case (DayOfWeek.Wednesday):

                    return 2;

                case (DayOfWeek.Thursday):

                    return 3;

                case (DayOfWeek.Friday):

                    return 4;

                case (DayOfWeek.Saturday):

                    return 5;

                case (DayOfWeek.Sunday):

                    return 6;

                default:

                    throw new Exception("Error calculating day of week. Input: " + year);

            }

        }

        
    }
}
