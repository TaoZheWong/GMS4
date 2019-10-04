using System;
using System.Data;
using System.Configuration;
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

namespace GMSWeb.HR.Staff
{
    public partial class ParsingStaff : GMSBasePage
    {
        private string excelFilePath = "", excelFileName = "";
        private short coyId;
        private string type;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.excelFileName = Request.Params["FILENAME"];
            this.coyId = short.Parse(Request.Params["COYID"]);
            this.type = Request.Params["TYPE"];

            excelFilePath = AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + Path.DirectorySeparatorChar + excelFileName;
            Response.ContentType = "text/html";
            if (type == "EmployeeInfo")
                ParseExcelFile();
            else if (type == "Qualification")
                ImportQualification();
            else if (type == "History")
                ImportHistory();
            else if (type == "Progression")
                ImportProgression();
                
        }

        protected void ParseExcelFile()
        {
            DataSet dsExcel = new DataSet();

            try
            {
                Response.Output.Write("Parsing excel file...<br>");
                Response.Flush();

                Asiasoft.MSExcelFileReader.SheetDataLoader sheetDataLoader_Audit = new Asiasoft.MSExcelFileReader.SheetDataLoader();
                sheetDataLoader_Audit.ExcelFilePath = this.excelFilePath;
                sheetDataLoader_Audit.IsHeaderIncludedInExcelFile = true;
                sheetDataLoader_Audit.SheetName = "Sheet1";
                sheetDataLoader_Audit.LoadExcelData();

                dsExcel = sheetDataLoader_Audit.ExcelData;

                for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                {
                    Employee employee = new SystemDataActivity().RetrieveEmployeeListByEmployeeNoSortByEmployeeName(dsExcel.Tables[0].Rows[i]["EmployeeNo"].ToString());
                    if (employee != null)
                    {

                        LogSession sess = base.GetSessionInfo();

                        employee.CoyID = this.coyId;
                        employee.Name = dsExcel.Tables[0].Rows[i]["Name"].ToString();
                        employee.Department = dsExcel.Tables[0].Rows[i]["Department"].ToString();
                        employee.DOB = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i]["DOB"].ToString());
                        employee.DateJoined = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i]["DateJoined"].ToString());
                        employee.Designation = dsExcel.Tables[0].Rows[i]["Designation"].ToString();
                        employee.Qualification = dsExcel.Tables[0].Rows[i]["Qualification"].ToString();
                        employee.Grade = dsExcel.Tables[0].Rows[i]["Grade"].ToString();
                        employee.NRIC = dsExcel.Tables[0].Rows[i]["NRIC"].ToString();
                        employee.EmailAddress = dsExcel.Tables[0].Rows[i]["Email"].ToString().Trim();
                        employee.IsUnitHead = (dsExcel.Tables[0].Rows[i]["IsUnitHead"].ToString()=="yes")?true:false;
                        employee.Division = dsExcel.Tables[0].Rows[i]["Division"].ToString();
                        employee.Department2 = dsExcel.Tables[0].Rows[i]["Department2"].ToString();
                        employee.Section = dsExcel.Tables[0].Rows[i]["Section"].ToString();
                        employee.Unit = dsExcel.Tables[0].Rows[i]["Unit"].ToString();

                        employee.ModifiedBy = sess.UserId;
                        employee.ModifiedDate = DateTime.Now;

                        Response.Output.Write("Update Employee Detail For Employee No: '" + employee.EmployeeNo + "' ...<br>");
                        Response.Flush();
                        try
                        {

                            ResultType update = new EmployeeActivity().UpdateEmployee(ref employee, sess);
                            if (update == ResultType.Ok)
                            {
                                Response.Output.Write("Updated successful.<br>");
                                Response.Flush();

                                File.Delete(excelFilePath);
                            }
                            else
                            {
                                Response.Output.Write("<SPAN STYLE='color: red'>Processing error of type : " + update.ToString() + ".</SPAN><br>");
                                Response.Flush();
                            }
                        }
                        catch (Exception ex)
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'>Updating fail. Error:" + ex.Message + ".</SPAN><br>");
                            Response.Flush();
                        }

                        Response.Output.Write("<br>");
                    }
                    else {
                        LogSession sess = base.GetSessionInfo();
                        employee = new Employee();

                        employee.CoyID = this.coyId;
                        employee.EmployeeNo = dsExcel.Tables[0].Rows[i]["EmployeeNo"].ToString();
                        employee.Department = dsExcel.Tables[0].Rows[i]["Department"].ToString();
                        employee.Name = dsExcel.Tables[0].Rows[i]["Name"].ToString();
                        employee.DOB = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i]["DOB"].ToString());
                        employee.DateJoined = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i]["DateJoined"].ToString());
                        employee.Designation = dsExcel.Tables[0].Rows[i]["Designation"].ToString();
                        employee.Qualification = dsExcel.Tables[0].Rows[i]["Qualification"].ToString();
                        employee.Grade = dsExcel.Tables[0].Rows[i]["Grade"].ToString();
                        employee.NRIC = dsExcel.Tables[0].Rows[i]["NRIC"].ToString();
                        employee.EmailAddress = dsExcel.Tables[0].Rows[i]["Email"].ToString().Trim();
                        employee.IsUnitHead = (dsExcel.Tables[0].Rows[i]["IsUnitHead"].ToString().Trim() == "yes")?true:false;
                        employee.IsInactive = false;
                        employee.CreatedBy = sess.UserId;
                        employee.CreatedDate = DateTime.Now;
                        employee.Division = dsExcel.Tables[0].Rows[i]["Division"].ToString();
                        employee.Department2 = dsExcel.Tables[0].Rows[i]["Department2"].ToString();
                        employee.Section = dsExcel.Tables[0].Rows[i]["Section"].ToString();
                        employee.Unit = dsExcel.Tables[0].Rows[i]["Unit"].ToString();

                        //GMSCore.Entity.DocumentNumber documentNumber = GMSCore.Entity.DocumentNumber.RetrieveByKey(1, (short)DateTime.Now.Year);
                        //employee.EmployeeID = documentNumber.EmployeeID;
                        //documentNumber.EmployeeID++;

                        Response.Output.Write("Inserting Employee Detail For Employee No: '" + employee.EmployeeNo + "' ...<br>");
                        Response.Flush();
                        try
                        {
                            ResultType create = new EmployeeActivity().CreateEmployee(ref employee, sess);
                            if (create == ResultType.Ok)
                            {
                                //documentNumber.Save();
                                Response.Output.Write("Inserting successful.<br>");
                                Response.Flush();

                                File.Delete(excelFilePath);
                            }
                            else
                            {
                                Response.Output.Write("<SPAN STYLE='color: red'>Processing error of type : " + create.ToString() + ".</SPAN><br>");
                                Response.Flush();
                            }
                        }
                        catch (Exception ex)
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + ".</SPAN><br>");
                            Response.Flush();
                        }

                        Response.Output.Write("<br>");
                    }
                }
                Response.Output.Write("<SPAN STYLE='color: red'>End of Excel File.</SPAN><br><br>");
                Response.Flush();
            }
            catch (Exception ex)
            {
                Response.Output.Write(("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>"));
            }
        }

        protected void ParseExcelFileNew()
        {
            DataSet dsExcel = new DataSet();

            try
            {
                Response.Output.Write("Parsing excel file...<br>");
                Response.Flush();

                Asiasoft.MSExcelFileReader.SheetDataLoader sheetDataLoader_Audit = new Asiasoft.MSExcelFileReader.SheetDataLoader();
                sheetDataLoader_Audit.ExcelFilePath = this.excelFilePath;
                sheetDataLoader_Audit.IsHeaderIncludedInExcelFile = true;
                sheetDataLoader_Audit.SheetName = "GMS";
                sheetDataLoader_Audit.LoadExcelData();
                IList<Employee> list = new SystemDataActivity().RetrieveEmployeeListByCoyID(this.coyId);
                if (list != null && list.Count > 0)
                {
                    foreach (Employee ee in list)
                    {
                        if (!ee.IsInactive)
                        {
                            ee.IsInactive = true;
                            ee.Notification = true;
                            ee.Save();
                            ee.Resync();
                        }
                    }
                }

                dsExcel = sheetDataLoader_Audit.ExcelData;

                for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                {
                    string eeno = dsExcel.Tables[0].Rows[i][1].ToString().Trim();
                    while (eeno.Length < 4)
                    {
                        eeno = "0" + eeno;
                    }
                    if (dsExcel.Tables[0].Rows[i][13].ToString().Trim() != "NONE")
                    {
                        Employee employee = new SystemDataActivity().RetrieveEmployeeListByEmployeeNoSortByEmployeeName(eeno);
                        if (employee != null)
                        {

                            LogSession sess = base.GetSessionInfo();

                            employee.CoyID = this.coyId;
                            employee.Name = dsExcel.Tables[0].Rows[i][0].ToString().Trim();
                            employee.Department = dsExcel.Tables[0].Rows[i][2].ToString().Trim();
                            employee.Designation = dsExcel.Tables[0].Rows[i][3].ToString().Trim();
                            employee.NRIC = dsExcel.Tables[0].Rows[i][4].ToString().Trim();
                            employee.DOB = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i][5].ToString());
                            employee.DateJoined = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i][6].ToString());
                            employee.Address1 = dsExcel.Tables[0].Rows[i][7].ToString().Trim();
                            employee.Address2 = dsExcel.Tables[0].Rows[i][8].ToString().Trim();
                            employee.Address3 = dsExcel.Tables[0].Rows[i][9].ToString().Trim();
                            employee.PostalCode = dsExcel.Tables[0].Rows[i][10].ToString().Trim();
                            
                            //12 Company Description
                            string seeno = dsExcel.Tables[0].Rows[i][13].ToString().Trim();
                            while (seeno.Length < 4)
                            {
                                seeno = "0" + seeno;
                            }
                            Employee Semployee = new SystemDataActivity().RetrieveEmployeeListByEmployeeNoSortByEmployeeName(seeno);
                            if (Semployee != null)
                            {
                                employee.SuperiorID = Semployee.EmployeeID;
                            }
                            employee.EmailAddress = dsExcel.Tables[0].Rows[i][11].ToString().Trim();
                            if (employee.EmailAddress == "" && Semployee != null)
                                employee.EmailAddress = Semployee.EmailAddress;
                            bool isUnitHead = false;
                            if (dsExcel.Tables[0].Rows[i][14].ToString().Trim() == "YES")
                                isUnitHead = true;
                            employee.IsUnitHead = isUnitHead;
                            employee.IsInactive = false;
                            employee.Notification = false;
                            DateTime terminationDate = new DateTime();
                            if (DateTime.TryParse(dsExcel.Tables[0].Rows[i][15].ToString().Trim(), out terminationDate))
                            {
                                employee.DateResigned = terminationDate;
                                employee.IsInactive = true;
                                employee.Notification = true;
                            }
                            employee.Nationality = dsExcel.Tables[0].Rows[i][16].ToString().Trim();
                            employee.Gender = dsExcel.Tables[0].Rows[i][17].ToString().Trim();
                            employee.Race = dsExcel.Tables[0].Rows[i][18].ToString().Trim();
                            employee.Qualification = dsExcel.Tables[0].Rows[i][19].ToString().Trim();
                            employee.Grade = dsExcel.Tables[0].Rows[i][20].ToString().Trim();
                            employee.ModifiedBy = sess.UserId;
                            employee.ModifiedDate = DateTime.Now;

                            Response.Output.Write("Update Employee Detail For Employee No: '" + employee.EmployeeNo + "' ...<br>");
                            Response.Flush();

                            try
                            {
                                ResultType update = new EmployeeActivity().UpdateEmployee(ref employee, sess);
                                if (update == ResultType.Ok)
                                {
                                    Response.Output.Write("Updated successful.<br>");
                                    Response.Flush();

                                    File.Delete(excelFilePath);
                                }
                                else
                                {
                                    Response.Output.Write("<SPAN STYLE='color: red'>Processing error of type : " + update.ToString() + ".</SPAN><br>");
                                    Response.Flush();
                                }
                            }
                            catch (Exception ex)
                            {
                                Response.Output.Write("<SPAN STYLE='color: red'>Updating fail. Error:" + ex.Message + ".</SPAN><br>");
                                Response.Flush();
                            }

                            Response.Output.Write("<br>");
                        }
                        else
                        {
                            LogSession sess = base.GetSessionInfo();
                            employee = new Employee();
                            employee.CoyID = this.coyId;
                            employee.Name = dsExcel.Tables[0].Rows[i][0].ToString().Trim();
                            employee.EmployeeNo = eeno;
                            employee.Department = dsExcel.Tables[0].Rows[i][2].ToString().Trim();
                            employee.Designation = dsExcel.Tables[0].Rows[i][3].ToString().Trim();
                            employee.NRIC = dsExcel.Tables[0].Rows[i][4].ToString().Trim();
                            employee.DOB = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i][5].ToString());
                            employee.DateJoined = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i][6].ToString());
                            employee.Address1 = dsExcel.Tables[0].Rows[i][7].ToString().Trim();
                            employee.Address2 = dsExcel.Tables[0].Rows[i][8].ToString().Trim();
                            employee.Address3 = dsExcel.Tables[0].Rows[i][9].ToString().Trim();
                            employee.PostalCode = dsExcel.Tables[0].Rows[i][10].ToString().Trim();
                            //12 Company Description
                            string seeno = dsExcel.Tables[0].Rows[i][13].ToString().Trim();
                            while (seeno.Length < 4)
                            {
                                seeno = "0" + seeno;
                            }
                            Employee Semployee = new SystemDataActivity().RetrieveEmployeeListByEmployeeNoSortByEmployeeName(seeno);
                            if (Semployee != null)
                            {
                                employee.SuperiorID = Semployee.EmployeeID;
                            }
                            employee.EmailAddress = dsExcel.Tables[0].Rows[i][11].ToString().Trim();
                            if (employee.EmailAddress == "" && Semployee != null)
                                employee.EmailAddress = Semployee.EmailAddress;
                            bool isUnitHead = false;
                            if (dsExcel.Tables[0].Rows[i][14].ToString().Trim() == "YES")
                                isUnitHead = true;
                            employee.IsUnitHead = isUnitHead;
                            employee.IsInactive = false;
                            employee.Notification = true;
                            employee.Nationality = dsExcel.Tables[0].Rows[i][16].ToString().Trim();
                            employee.Gender = dsExcel.Tables[0].Rows[i][17].ToString().Trim();
                            employee.Race = dsExcel.Tables[0].Rows[i][18].ToString().Trim();
                            employee.Qualification = dsExcel.Tables[0].Rows[i][19].ToString().Trim();
                            employee.CreatedBy = sess.UserId;
                            employee.CreatedDate = DateTime.Now;

                            Response.Output.Write("Inserting Employee Detail For Employee No: '" + employee.EmployeeNo + "' ...<br>");
                            Response.Flush();

                            try
                            {
                                ResultType create = new EmployeeActivity().CreateEmployee(ref employee, sess);
                                if (create == ResultType.Ok)
                                {
                                    //documentNumber.Save();
                                    Response.Output.Write("Inserting successful.<br>");
                                    Response.Flush();

                                    File.Delete(excelFilePath);
                                }
                                else
                                {
                                    Response.Output.Write("<SPAN STYLE='color: red'>Processing error of type : " + create.ToString() + ".</SPAN><br>");
                                    Response.Flush();
                                }
                            }
                            catch (Exception ex)
                            {
                                Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + ".</SPAN><br>");
                                Response.Flush();
                            }

                            Response.Output.Write("<br>");
                        }
                    }
                }
                Response.Output.Write("<SPAN STYLE='color: red'>End of Excel File.</SPAN><br><br>");
                Response.Flush();
            }
            catch (Exception ex)
            {
                Response.Output.Write(("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>"));
            }
        }

        protected void ImportQualification()
        {
            LogSession sess = base.GetSessionInfo();
            DataSet dsExcel = new DataSet();

            try
            {
                Response.Output.Write("Parsing excel file...<br>");
                Response.Flush();

                Asiasoft.MSExcelFileReader.SheetDataLoader sheetDataLoader_Audit = new Asiasoft.MSExcelFileReader.SheetDataLoader();
                sheetDataLoader_Audit.ExcelFilePath = this.excelFilePath;
                sheetDataLoader_Audit.IsHeaderIncludedInExcelFile = true;
                sheetDataLoader_Audit.SheetName = "GMS";
                sheetDataLoader_Audit.LoadExcelData();

                dsExcel = sheetDataLoader_Audit.ExcelData;

                new GMSGeneralDALC().procDeleteEmployeeEducationalQualificationByCompany(this.coyId);
                                
                for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                {
                    string eeno = dsExcel.Tables[0].Rows[i][0].ToString().Trim();
                    while (eeno.Length < 4)
                    {
                        eeno = "0" + eeno;
                    }

                    Employee employee = new SystemDataActivity().RetrieveEmployeeListByEmployeeNoSortByEmployeeName(eeno);
                    if (employee != null)
                    {
                        EmployeeEducationalQualification empQualification = new EmployeeEducationalQualification();
                        empQualification.CoyID = this.coyId;
                        empQualification.EmployeeNo = dsExcel.Tables[0].Rows[i][0].ToString().Trim();
                        empQualification.StartDate = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i][2].ToString().Trim());
                        empQualification.EndDate = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i][3].ToString().Trim());

                        if (dsExcel.Tables[0].Rows[i][4].ToString().Trim() == "")
                            empQualification.InstituteName = "";
                        else
                            empQualification.InstituteName = dsExcel.Tables[0].Rows[i][4].ToString().Trim();

                        if (dsExcel.Tables[0].Rows[i][5].ToString().Trim() == "")
                            empQualification.CourseName = "";
                        else
                            empQualification.CourseName = dsExcel.Tables[0].Rows[i][5].ToString().Trim();

                        if(dsExcel.Tables[0].Rows[i][6].ToString().Trim() == "")
                            empQualification.HighestStandardPassed = "NONE";
                        else
                            empQualification.HighestStandardPassed = dsExcel.Tables[0].Rows[i][6].ToString().Trim();
                        empQualification.CreatedBy = sess.UserId;
                        empQualification.CreatedDate = DateTime.Now;
                        Response.Output.Write("Inserting Employee Qualification For Employee No: '" + eeno + "' ...<br>");
                        Response.Flush();

                        try
                        {

                            empQualification.Save();
                            Response.Output.Write("Inserting successful.<br>");
                            Response.Flush();
                            File.Delete(excelFilePath);
                            
                        }
                        catch (Exception ex)
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + ".</SPAN><br>");
                            Response.Flush();
                        }

                        Response.Output.Write("<br>");
                    }
                   
                }
                

            }
            catch (Exception ex)
            {
                Response.Output.Write(("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>"));
            }
        }

        protected void ImportHistory()
        {
            LogSession sess = base.GetSessionInfo();
            DataSet dsExcel = new DataSet();

            try
            {
                Response.Output.Write("Parsing excel file...<br>");
                Response.Flush();

                Asiasoft.MSExcelFileReader.SheetDataLoader sheetDataLoader_Audit = new Asiasoft.MSExcelFileReader.SheetDataLoader();
                sheetDataLoader_Audit.ExcelFilePath = this.excelFilePath;
                sheetDataLoader_Audit.IsHeaderIncludedInExcelFile = true;
                sheetDataLoader_Audit.SheetName = "GMS";
                sheetDataLoader_Audit.LoadExcelData();

                dsExcel = sheetDataLoader_Audit.ExcelData;

                new GMSGeneralDALC().procDeleteEmployeeHistoryByCompany(this.coyId);

                for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                {
                    string eeno = dsExcel.Tables[0].Rows[i][0].ToString().Trim();
                    while (eeno.Length < 4)
                    {
                        eeno = "0" + eeno;
                    }

                    Employee employee = new SystemDataActivity().RetrieveEmployeeListByEmployeeNoSortByEmployeeName(eeno);
                    if (employee != null)
                    {
                        EmployeeHistory empHistory = new EmployeeHistory();
                        empHistory.CoyID = this.coyId;
                        empHistory.EmployeeNo = dsExcel.Tables[0].Rows[i][0].ToString().Trim();
                        empHistory.StartDate = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i][2].ToString().Trim());
                        empHistory.EndDate = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i][3].ToString().Trim());
                        if (dsExcel.Tables[0].Rows[i][4].ToString().Trim() == "")
                            empHistory.CompanyName = "NONE";
                        else
                            empHistory.CompanyName = dsExcel.Tables[0].Rows[i][4].ToString().Trim();

                        if (dsExcel.Tables[0].Rows[i][5].ToString().Trim() == "")
                            empHistory.Designation = "NONE";
                        else
                            empHistory.Designation = dsExcel.Tables[0].Rows[i][5].ToString().Trim();
                        empHistory.CreatedBy = sess.UserId;
                        empHistory.CreatedDate = DateTime.Now;
                        Response.Output.Write("Inserting Employee History For Employee No: '" + eeno + "' ...<br>");
                        Response.Flush();

                        try
                        {

                            empHistory.Save();
                            Response.Output.Write("Inserting successful.<br>");
                            Response.Flush();
                            File.Delete(excelFilePath);

                        }
                        catch (Exception ex)
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + ".</SPAN><br>");
                            Response.Flush();
                        }

                        Response.Output.Write("<br>");
                    }

                }


            }
            catch (Exception ex)
            {
                Response.Output.Write(("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>"));
            }
        }

        protected void ImportProgression()
        {
            LogSession sess = base.GetSessionInfo();
            DataSet dsExcel = new DataSet();

            try
            {
                Response.Output.Write("Parsing excel file...<br>");
                Response.Flush();

                Asiasoft.MSExcelFileReader.SheetDataLoader sheetDataLoader_Audit = new Asiasoft.MSExcelFileReader.SheetDataLoader();
                sheetDataLoader_Audit.ExcelFilePath = this.excelFilePath;
                sheetDataLoader_Audit.IsHeaderIncludedInExcelFile = true;
                sheetDataLoader_Audit.SheetName = "GMS";
                sheetDataLoader_Audit.LoadExcelData();

                dsExcel = sheetDataLoader_Audit.ExcelData;

                new GMSGeneralDALC().procDeleteEmployeeCareerProgressionByCompany(this.coyId);

                for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                {
                    string eeno = dsExcel.Tables[0].Rows[i][0].ToString().Trim();
                    while (eeno.Length < 4)
                    {
                        eeno = "0" + eeno;
                    }

                    Employee employee = new SystemDataActivity().RetrieveEmployeeListByEmployeeNoSortByEmployeeName(eeno);
                    if (employee != null)
                    {
                        EmployeeCareerProgression empProgression = new EmployeeCareerProgression();
                        empProgression.CoyID = this.coyId;
                        empProgression.EmployeeNo = dsExcel.Tables[0].Rows[i][0].ToString().Trim();
                        empProgression.ProgressionDate = GMSUtil.ToDate(dsExcel.Tables[0].Rows[i][2].ToString().Trim());
                        if (dsExcel.Tables[0].Rows[i][3].ToString().Trim() == "")
                            empProgression.Progression = "NONE";
                        else
                            empProgression.Progression = dsExcel.Tables[0].Rows[i][3].ToString().Trim();

                        if (dsExcel.Tables[0].Rows[i][4].ToString().Trim() == "")
                            empProgression.CompanyName = "NONE";
                        else
                            empProgression.CompanyName = dsExcel.Tables[0].Rows[i][4].ToString().Trim();

                        if (dsExcel.Tables[0].Rows[i][5].ToString().Trim() == "")
                            empProgression.Department = "NONE";
                        else
                            empProgression.Department = dsExcel.Tables[0].Rows[i][5].ToString().Trim();

                        if (dsExcel.Tables[0].Rows[i][6].ToString().Trim() == "")
                            empProgression.Designation = "NONE";
                        else
                            empProgression.Designation = dsExcel.Tables[0].Rows[i][6].ToString().Trim();

                        if (dsExcel.Tables[0].Rows[i][7].ToString().Trim() == "")
                            empProgression.Grade = "NONE";
                        else
                            empProgression.Grade = dsExcel.Tables[0].Rows[i][7].ToString().Trim();

                        empProgression.CreatedBy = sess.UserId;
                        empProgression.CreatedDate = DateTime.Now;
                        Response.Output.Write("Inserting Employee Career Progression For Employee No: '" + eeno + "' ...<br>");
                        Response.Flush();

                        try
                        {

                            empProgression.Save();
                            Response.Output.Write("Inserting successful.<br>");
                            Response.Flush();
                            File.Delete(excelFilePath);

                        }
                        catch (Exception ex)
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + ".</SPAN><br>");
                            Response.Flush();
                        }

                        Response.Output.Write("<br>");
                    }

                }


            }
            catch (Exception ex)
            {
                Response.Output.Write(("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>"));
            }
        }


    }
}
