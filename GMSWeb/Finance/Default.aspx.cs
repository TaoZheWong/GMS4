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
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Collections.Generic;
using System.Web.Services;

namespace GMSWeb.Finance
{
    public partial class Default : GMSBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            var currentYear = DateTime.Now.Year;
            var previousYear = DateTime.Now.Year - 1;
            var currentMonth = DateTime.Now.Month;
            var previousMonth = DateTime.Now.Month - 1;

            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }

            Master.setCurrentLink("CompanyFinance");

            string javaScript = @"<script language=""javascript"" type=""text/javascript"" >
                        var coyId = '"+ session.CompanyId +
                    @"' ,currentYear = '" + currentYear +
                    @"' ,previousYear = '" + previousYear +
                    @"' ,currentMonth = '" + currentMonth +
                    @"' ,previousMonth = '" + previousMonth +
                    @"' ;
                </script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        public class ResponseModel
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public IDictionary<string, object> Params { get; set; }

            public ResponseModel()
            {
                Status = 0; // 0=success; 1=error
                Message = "Success";
            }

            public ResponseModel(int status, string message)
            {
                Status = status;
                Message = message;
            }
        }

        private static string[] getFinanceItemByName(string itemName ,string type, short CompanyId, DateTime start, DateTime end)
        {
            DataSet dS = new DataSet();
            (new FinanceDALC()).GetFinanceItemByName(
                itemName,
                CompanyId,
                short.Parse(start.Year.ToString()),
                short.Parse(end.Year.ToString()),
                short.Parse(start.Month.ToString()),
                short.Parse(end.Month.ToString()),
                ref dS
                );
            var raw = GMSUtil.ToJson(dS, 0);
            string[] data = { "0", "0", "0", "0", "0", "0" };
            for (int i = 0; i < raw.Count; i++)
            {
                foreach (var row in raw[i])
                {
                    if (row.Key == type)
                        data[i] = row.Value;
                }
            }

            return data;
        }

        private static string[] getFinanceBudget(string itemName, string type, short CompanyId, DateTime start, DateTime end)
        {
            DataSet dS = new DataSet();
            (new FinanceDALC()).GetFinanceBudget(
                itemName,
                CompanyId,
                short.Parse(start.Year.ToString()),
                short.Parse(end.Year.ToString()),
                short.Parse(start.Month.ToString()),
                short.Parse(end.Month.ToString()),
                ref dS
                );
            var raw = GMSUtil.ToJson(dS, 0);
            string[] data = { "0", "0", "0", "0", "0", "0" };
            for (int i = 0; i < raw.Count; i++)
            {
                foreach (var row in raw[i])
                {
                    if (row.Key == type)
                        data[i] = row.Value;
                }
            }

            return data;
        }

        [WebMethod]
        public static ResponseModel GetMonthlySale(string coyId)
        {
            var m = new ResponseModel();
            var start = DateTime.Now.AddMonths(-6);
            var end = DateTime.Now.AddMonths(-1);
            var lastYearStart = DateTime.Now.AddYears(-1).AddMonths(-6);
            var lastYearEnd = lastYearStart.AddMonths(5);
            short CompanyId = short.Parse(coyId);
            var responseList = new List<Dictionary<string, string[]>>();
            var list = new Dictionary<string, string[]>();

            try
            {
                /*Label*/
                var label = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    label[i] = start.AddMonths(i).ToString("MMM yy");
                }
                /*Current Month Data*/
                string[] currentMonthData = getFinanceItemByName("(PNL)Total Sales" , "mtd", CompanyId, start, end);
                /*Last Year Data*/
                string[] lastYearData = getFinanceItemByName("(PNL)Total Sales" , "mtd", CompanyId, lastYearStart, lastYearEnd);
                /*Get Budget*/
                string[] budget = getFinanceBudget("(PNL)Total Sales", "mtd", CompanyId, start, end);

                list.Add("label", label);
                list.Add("budget", budget);
                list.Add("current_month", currentMonthData);
                list.Add("last_year", lastYearData);
                responseList.Add(list);

                m.Params = new Dictionary<string, object> { { "data", responseList } };
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;
                
            }

            return m;
        }

        [WebMethod]
        public static ResponseModel GetMonthlyOperatingIncome(string coyId)
        {
            var m = new ResponseModel();
            var start = DateTime.Now.AddMonths(-6);
            var end = DateTime.Now.AddMonths(-1);
            var lastYearStart = DateTime.Now.AddYears(-1).AddMonths(-6);
            var lastYearEnd = lastYearStart.AddMonths(5);
            short CompanyId = short.Parse(coyId);
            var responseList = new List<Dictionary<string, string[]>>();
            var list = new Dictionary<string, string[]>();

            try
            {
                /*Label*/
                var label = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    label[i] = start.AddMonths(i).ToString("MMM yy");
                }
                /*Current Month Data*/
                string[] currentMonthData = getFinanceItemByName("(PNL)Profit from Operations","mtd", CompanyId, start, end);
                /*Last Year Data*/
                string[] lastYearData = getFinanceItemByName("(PNL)Profit from Operations","mtd", CompanyId, lastYearStart, lastYearEnd);
                /*Get Budget*/
                string[] budget = getFinanceBudget("(PNL)Profit from Operations", "mtd", CompanyId, start, end);

                list.Add("label", label);
                list.Add("budget", budget);
                list.Add("current_month", currentMonthData);
                list.Add("last_year", lastYearData);
                responseList.Add(list);

                m.Params = new Dictionary<string, object> { { "data", responseList } };
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;

        }

        [WebMethod]
        public static ResponseModel GetYtdSale(string coyId)
        {
            var m = new ResponseModel();
            var start = DateTime.Now.AddMonths(-6);
            var end = DateTime.Now.AddMonths(-1);
            var lastYearStart = DateTime.Now.AddYears(-1).AddMonths(-6);
            var lastYearEnd = lastYearStart.AddMonths(5);
            short CompanyId = short.Parse(coyId);
            var responseList = new List<Dictionary<string, string[]>>();
            var list = new Dictionary<string, string[]>();

            try
            {
                /*Label*/
                var label = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    label[i] = start.AddMonths(i).ToString("MMM yy");
                }
                /*Current Month Data*/
                string[] currentMonthData = getFinanceItemByName("(PNL)Total Sales","ytd", CompanyId, start, end);
                /*Last Year Data*/
                string[] lastYearData = getFinanceItemByName("(PNL)Total Sales","ytd", CompanyId, lastYearStart, lastYearEnd);
                /*Get Budget*/
                string[] budget = getFinanceBudget("(PNL)Total Sales", "ytd", CompanyId, start, end);

                list.Add("label", label);
                list.Add("budget", budget);
                list.Add("current_month", currentMonthData);
                list.Add("last_year", lastYearData);
                responseList.Add(list);

                m.Params = new Dictionary<string, object> { { "data", responseList } };
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }

        [WebMethod]
        public static ResponseModel GetYtdOperatingIncome(string coyId)
        {

            var m = new ResponseModel();
            var start = DateTime.Now.AddMonths(-6);
            var end = DateTime.Now.AddMonths(-1);
            var lastYearStart = DateTime.Now.AddYears(-1).AddMonths(-6);
            var lastYearEnd = lastYearStart.AddMonths(5);
            short CompanyId = short.Parse(coyId);
            var responseList = new List<Dictionary<string, string[]>>();
            var list = new Dictionary<string, string[]>();

            try
            {
                /*Label*/
                var label = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    label[i] = start.AddMonths(i).ToString("MMM yy");
                }
                /*Current Month Data*/
                string[] currentMonthData = getFinanceItemByName("(PNL)Profit from Operations","ytd", CompanyId, start, end);
                /*Last Year Data*/
                string[] lastYearData = getFinanceItemByName("(PNL)Profit from Operations","ytd", CompanyId, lastYearStart, lastYearEnd);
                /*Get Budget*/
                string[] budget = getFinanceBudget("(PNL)Profit from Operations", "ytd", CompanyId, start, end);

                list.Add("label", label);
                list.Add("budget", budget);
                list.Add("current_month", currentMonthData);
                list.Add("last_year", lastYearData);
                responseList.Add(list);

                m.Params = new Dictionary<string, object> { { "data", responseList } };
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }
    }
}
