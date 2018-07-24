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
using System.Data.SqlClient;
using System.Text;


using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using Ionic.Zip;

namespace GMSWeb.SysFinance.Administration
{
    public partial class Exporter : GMSBasePage
    {
        private int coyId;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Finance");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Finance"));
                return;
            }
            coyId = session.CompanyId;

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            68);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Finance"));

            //btnImport.Attributes.Add("onclick", "javascript:" + btnImport.ClientID + ".disabled=true;" + this.GetPostBackEventReference(btnImport));

            if (!Page.IsPostBack)
            {
                // load year ddl
                DataTable dtt1 = new DataTable();
                dtt1.Columns.Add("Year", typeof(string));

                for (int i = -1; i < 1; i++)
                {
                    DataRow dr1 = dtt1.NewRow();
                    dr1["Year"] = DateTime.Now.Year + i;

                    dtt1.Rows.Add(dr1);
                }

                this.ddlYear.DataSource = dtt1;
                this.ddlYear.DataBind();

                DateTime lastMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                this.ddlYear.SelectedValue = lastMonthDate.Year.ToString();

                // load month ddl
                DataTable dtt2 = new DataTable();
                dtt2.Columns.Add("Month", typeof(string));

                for (int i = 1; i <= 12; i++)
                {
                    DataRow dr1 = dtt2.NewRow();
                    dr1["Month"] = i;
                    dtt2.Rows.Add(dr1);
                }

                this.ddlMonth.DataSource = dtt2;
                this.ddlMonth.DataBind();

                this.ddlMonth.SelectedValue = lastMonthDate.Month.ToString();
                PopulateCompanyRepeater();
                /*
                string javaScript = @"
				<script type=""text/javascript"">
				function CheckAll( e )
				{	
					var p = e.parentElement.parentElement.parentElement;	
					var b = e.checked;
					if( p != null )
					{
						for(i=1; i<p.childNodes.length; i++)
						{
							var chk = p.childNodes[i].childNodes[4].childNodes[0];
							if( chk != null )
								chk.checked = b;					
						}
					}
				}

				function RemoveCheckAll( e )
				{
					var p = e.parentElement.parentElement.parentElement;
					if( p != null )
					{
						var unchk = p.childNodes[0].childNodes[4].childNodes[0];
						if( unchk != null )
							unchk.checked = false;
					}
				}
				</script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
                 * */
            }
        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string coyIdList;

            if (Request.Form["CoyID"] != null)
            {
                coyIdList = Request.Form["CoyID"].Trim();
            }
            else
            {
                JScriptAlertMsg("You must select at least one company to export the trial balance.");
                return;
            }

            //string fileName = "FST70" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString().PadLeft(2,'0') + ".txt"; 
            //string folderPath = @"D:\GMSDocuments\Group Finance\"; 
            string folderPath = @"D:\GMSFinance\";  //+ DateTime.Now.ToString("yyyyMMdd_HHmmss"); 
            string zipFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".zip";
            string zipFilePath = folderPath + zipFileName;

            //delete all files from the folder
            string[] deleteFilePaths = Directory.GetFiles(@"D:\GMSFinance\");
            foreach (string deleteFilePath in deleteFilePaths)
                File.Delete(deleteFilePath);

            ZipFile zip = new ZipFile(zipFilePath);

            IDbConnection conn = new ConnectionManager().GetConnection();
            SqlDataReader rdr = null;
            StringBuilder sb = new StringBuilder();

            try
            {
                conn.Open();
                //Prepare FST Output File
                SqlCommand command = new SqlCommand("procFinanceTBForGroupForDIVA", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyIDList", SqlDbType.NVarChar).Value = coyIdList;
                command.Parameters.Add("@Year", SqlDbType.SmallInt).Value = ddlYear.SelectedValue;
                command.Parameters.Add("@Month", SqlDbType.SmallInt).Value = ddlMonth.SelectedValue;
                rdr = command.ExecuteReader();

                string prevCompany = "";
                string fileName = "";
                string filePath = "";
                int i = 0;
                while (rdr.Read())
                {
                    if (rdr["CompanyCode"].ToString() != prevCompany && i != 0)
                    {
                        //reading new company lines, create a file, write to file and add to zip file, clear sb
                        fileName = "FST10" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString().PadLeft(2, '0') + prevCompany + ".txt";
                        filePath = folderPath + fileName;
                        File.WriteAllText(filePath, sb.ToString());
                        zip.AddFile(filePath);
                        zip.Save();
                        sb.Length = 0;
                    }
                    prevCompany = rdr["CompanyCode"].ToString();
                    sb.AppendFormat("{0}\t{1}\t{2}", rdr["AccountCode"], rdr["Total"], rdr["Remarks"]);
                    sb.AppendLine();
                    i++;
                }
                //handling the last line 
                fileName = "FST10" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString().PadLeft(2, '0') + prevCompany + ".txt";
                filePath = folderPath + fileName;
                File.WriteAllText(filePath, sb.ToString());
                zip.AddFile(filePath);
                zip.Save();
                sb.Length = 0;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            try
            {
                conn.Open();
                //Prepare INC Output File
                SqlCommand command = new SqlCommand("procFinanceInterCompanyForGroupForDIVA", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyIDList", SqlDbType.NVarChar).Value = coyIdList;
                command.Parameters.Add("@Year", SqlDbType.SmallInt).Value = ddlYear.SelectedValue;
                command.Parameters.Add("@Month", SqlDbType.SmallInt).Value = ddlMonth.SelectedValue;
                rdr = command.ExecuteReader();

                string prevCompany = "";
                string fileName = "";
                string filePath = "";
                int i = 0;
                while (rdr.Read())
                {
                    if (rdr["CompanyCode"].ToString() != prevCompany && i != 0)
                    {
                        //reading new company lines, create a file, write to file and add to zip file, clear sb
                        fileName = "INC10" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString().PadLeft(2, '0') + prevCompany + ".txt";
                        filePath = folderPath + fileName;
                        File.WriteAllText(filePath, sb.ToString());
                        zip.AddFile(filePath);
                        zip.Save();
                        sb.Length = 0;
                    }
                    prevCompany = rdr["CompanyCode"].ToString();
                    sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", rdr["SegmentCode"], rdr["CounterPartyCode"], rdr["CounterSegmentCode"], rdr["AccountCode"], rdr["Total"], rdr["Remarks"]);
                    sb.AppendLine();
                    i++;
                }
                //handling the last line 
                fileName = "INC10" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString().PadLeft(2, '0') + prevCompany + ".txt";
                filePath = folderPath + fileName;
                File.WriteAllText(filePath, sb.ToString());
                zip.AddFile(filePath);
                zip.Save();
                sb.Length = 0;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + zipFileName);
            //Response.ContentType = "text/csv";
            //Response.ContentType = "application/x-zip-compressed"; 
            Response.ContentType = "application/octet-stream";
            //Response.TransmitFile(zipFilePath);
            Response.WriteFile(zipFilePath);
            Response.End();
        }

        #region PopulateCompanyRepeater
        private void PopulateCompanyRepeater()
        {
            DataTable dttTable = new DataTable();

            dttTable.Columns.Add("CompanyTitle", typeof(string));
            DataRow drRow = dttTable.NewRow();

            drRow["CompanyTitle"] = "Company";
            dttTable.Rows.Add(drRow);

            IList<Company> lstCompany = null;
            lstCompany = new SystemDataActivity().RetrieveAllAliveCompanyListForConsol();

            if (lstCompany != null && lstCompany.Count > 0)
            {
                //Repeater rppCompany = (Repeater)item.FindControl("rppCompany");
                rppCompany.DataSource = lstCompany;
                rppCompany.DataBind();
            }
        }
        #endregion

        /*
		protected void btnImport_Click(object sender, EventArgs e)
		{
			Package2Class package = new Package2Class();
			object pVarPersistStgOfHost = null;

			try
			{
				 if you need to load from file
				package.LoadFromStorageFile(
					"c:\\TestPackage.dts",
					null,
					null,
					null,
					"Test Package",
					ref pVarPersistStgOfHost);
				
				//DTSSQLServerStorageFlags.DTSSQLStgFlag_UseTrustedConnection,
				package.LoadFromSQLServer(
					"(local)",
					"sa",
					"",
					DTSSQLServerStorageFlags.DTSSQLStgFlag_UseTrustedConnection,
					null,
					null,
					null,
					"DTS_ImportExcelData",
					ref pVarPersistStgOfHost);
				package.Execute();
				ScriptManager.RegisterStartupScript(this, typeof(Page), "progress_stop", "progress_stop();", true);  
				JScriptAlertMsg("Finished importing.");
			}
			catch (System.Runtime.InteropServices.COMException ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}
			catch (System.Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}
			finally
			{
				package.UnInitialize(); // unwrap the package
				System.Runtime.InteropServices.Marshal.ReleaseComObject(package); // tell interop to release the reference
				package = null;
			}
		}*/
    }
}