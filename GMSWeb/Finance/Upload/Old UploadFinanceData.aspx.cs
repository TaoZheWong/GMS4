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
using DTS;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Finance.Upload
{
    public partial class UploadFinanceData : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            68);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                LoadDDLs();
            }
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

            // load month ddl
            DataTable dtt2 = new DataTable();
            dtt2.Columns.Add("Month", typeof(string));

            for (int i = 1; i < 13; i++)
            {
                DataRow dr2 = dtt2.NewRow();
                dr2["Month"] = i;

                dtt2.Rows.Add(dr2);
            }

            this.ddlMonth.DataSource = dtt2;
            this.ddlMonth.DataBind();

            // load purpose ddl
            IList<ItemPurpose> lstPurpose = null;
            lstPurpose = new SystemDataActivity().RetrieveAllItemPurposeListSortByName();

            this.ddlPurpose.DataSource = lstPurpose;
            this.ddlPurpose.DataBind();

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                FileUpload1.SaveAs(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + "\\" + FileUpload1.FileName);

                this.IFrame1.Attributes["style"] = "";
                this.IFrame1.Attributes["src"] = String.Format("ParsingFinanceData.aspx?FILENAME={0}&YEAR={1}&MONTH={2}&PURPOSEID={3}",
                                                            Server.UrlEncode(FileUpload1.FileName),
                                                            this.ddlYear.SelectedValue.ToString(), this.ddlMonth.SelectedValue.ToString(), this.ddlPurpose.SelectedValue);
            }
            else
            {
                lblMsg.Text = "You have not specified a file.";
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            Package2Class package = new Package2Class();
            object pVarPersistStgOfHost = null;

            try
            {
                /* if you need to load from file
                package.LoadFromStorageFile(
                    "c:\\TestPackage.dts",
                    null,
                    null,
                    null,
                    "Test Package",
                    ref pVarPersistStgOfHost);
                */

                package.LoadFromSQLServer(
                    "(local)",
                    null,
                    null,
                    DTSSQLServerStorageFlags.DTSSQLStgFlag_UseTrustedConnection,
                    null,
                    null,
                    null,
                    "DTS_ImportExcelData",
                    ref pVarPersistStgOfHost);
                package.Execute();
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
        }
    }
}
