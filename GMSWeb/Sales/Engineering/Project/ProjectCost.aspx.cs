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
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;
using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Text;
using System.Web.Services;
using AjaxControlToolkit;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace GMSWeb.Sales.Engineering.Project
{
    public partial class ProjectCost : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId, 132);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {

            }
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialRequisitionList(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            string PrevProjectNo = "";
            short CompanyId = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "PrevProjectNo")
                    PrevProjectNo = idict[key].ToString();
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);

            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMaterialRequisitionList(CompanyId, ProjectNo, PrevProjectNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetLaborCostList(short CompanyId, string ProjectNo, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetLaborCostList(CompanyId, ProjectNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);


        }

        [WebMethod]
        public static List<Dictionary<string, string>> InsertLaborCost(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            short LCID = 0;
            string Period = "";
            string PIC = "";
            string Hour = "";
            string CurrencyCode = "";
            decimal Rate = 0;
            decimal Amount = 0;
            string Remarks = "";
            short CompanyId = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "LCID")
                    LCID = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "Period")
                    Period = idict[key].ToString();
                else if (key.ToString() == "PIC")
                    PIC = idict[key].ToString();
                else if (key.ToString() == "Hour")
                    Hour = idict[key].ToString();
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "Rate")
                    Rate = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "Amount")
                    Amount = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "Remarks")
                    Remarks = idict[key].ToString();
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
           (new EngineeringDataDALC()).InsertLaborCost(CompanyId, ProjectNo, Period, PIC, Hour, CurrencyCode, Rate, Amount, Remarks, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }


        [WebMethod]
        public static List<Dictionary<string, string>> InsertProjectMR(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            string MRNo = "";
            string InvoiceNo = "";
            decimal InvoiceAmount = 0;
            string InvoiceRemarks = "";
            short CompanyId = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "MrNo")
                    MRNo = idict[key].ToString();
                else if (key.ToString() == "InvoiceNo")
                    InvoiceNo = idict[key].ToString();
                else if (key.ToString() == "InvoiceAmount")
                    InvoiceAmount = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "InvoiceRemarks")
                    InvoiceRemarks = idict[key].ToString();
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).InsertProjectMR(CompanyId, ProjectNo, MRNo, InvoiceNo, InvoiceAmount, InvoiceRemarks, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> UpdateLaborCost(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            short LCID = 0;
            string Period = "";
            string PIC = "";
            string Hour = "";
            string CurrencyCode = "";
            decimal Rate = 0;
            decimal Amount = 0;
            string Remarks = "";
            short CompanyId = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "LCID")
                    LCID = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "Period")
                    Period = idict[key].ToString();
                else if (key.ToString() == "PIC")
                    PIC = idict[key].ToString();
                else if (key.ToString() == "Hour")
                    Hour = idict[key].ToString();
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "Rate")
                    Rate = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "Amount")
                    Amount = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "Remarks")
                    Remarks = idict[key].ToString();
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).UpdateLaborCost(CompanyId, ProjectNo, LCID, Period, PIC, Hour, CurrencyCode, Rate, Amount, Remarks, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> DeleteLaborCost(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            short LCID = 0;
            short CompanyId = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "LCID")
                    LCID = GMSUtil.ToShort(idict[key]);

                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).DeleteLaborCost(CompanyId, ProjectNo, LCID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);


        }


        [WebMethod]
        public static List<Dictionary<string, string>> GetMiscCostList(short CompanyId, string ProjectNo, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMiscCostList(CompanyId, ProjectNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);


        }

        [WebMethod]
        public static List<Dictionary<string, string>> InsertMiscCost(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string projectno = "";
            short OCID = 0;
            string Description = "";
            string CurrencyCode = "";
            decimal Amount = 0;
            string Location = "";
            string Remarks = "";
            short CompanyId = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    projectno = idict[key].ToString();
                else if (key.ToString() == "OCID")
                    OCID = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "Description")
                    Description = idict[key].ToString();
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "Amount")
                    Amount = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "Location")
                    Location = idict[key].ToString();
                else if (key.ToString() == "Remarks")
                    Remarks = idict[key].ToString();
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();

            (new EngineeringDataDALC()).InsertMiscCost(CompanyId, projectno, Description, CurrencyCode, Amount, Location, Remarks, UserID, ref dsTemp);
           
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> UpdateMiscCost(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string projectno = "";
            short OCID = 0;
            string Description = "";
            string CurrencyCode = "";
            decimal Amount = 0;
            string Location = "";
            string Remarks = "";
            short CompanyId = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    projectno = idict[key].ToString();
                else if (key.ToString() == "OCID")
                    OCID = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "Description")
                    Description = idict[key].ToString();
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "Amount")
                    Amount = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "Location")
                    Location = idict[key].ToString();
                else if (key.ToString() == "Remarks")
                    Remarks = idict[key].ToString();
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).UpdateMiscCost(CompanyId, projectno, OCID, Description, CurrencyCode, Amount, Location, Remarks, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> DeleteMiscCost(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            short OCID = 0;
            short CompanyId = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "OCID")
                    OCID = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).DeleteMiscCost(CompanyId, ProjectNo, OCID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);


        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialRequisitionByMRNo(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string MRNo = "";
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "MRNo")
                    MRNo = idict[key].ToString();
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);

            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMRInfoByMRNo(CompanyId, MRNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialRequisitionListByMRNo(short CompanyId, string MRNo)
        {
           
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMaterialRequisitionListByMRNo(CompanyId, MRNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }


        [WebMethod]
        public static List<Dictionary<string, string>> DeleteMRInv(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            string MRNo = "";
            int ItemID = 0;
            short CompanyId = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "ItemID")
                    ItemID = GMSUtil.ToInt(idict[key]);
                else if (key.ToString() == "MRNo")
                    MRNo = idict[key].ToString();
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).DeleteMRInv(CompanyId, ProjectNo, MRNo, ItemID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);


        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetGrandTotalByProjectNo(short CompanyId, string ProjectNo, short UserId)
        {

            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetGrandTotalByProjectNo(CompanyId, ProjectNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }
    }

    
}
