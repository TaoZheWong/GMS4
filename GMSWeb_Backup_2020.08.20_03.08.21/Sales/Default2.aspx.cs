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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;
using System.Data.SqlClient;



using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Text;
using System.Web.Services;
using AjaxControlToolkit;

namespace GMSWeb.Sales
{
    public partial class Default2 : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            Master.setCurrentLink("Sales");

            DataSet lstAlterParty = new DataSet();

            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "MR", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {

                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;


            if (!IsPostBack)
            {
                //LoadData();
            }
            WorkFlow WF = new WorkFlow();


            int[] users = new int[] {1, 2, 3, 4, 5};
            DataTable dt = new WorkFlowRouting().GenerateRoutingTable_Default(users);
            Guid piid = WF.InitiateProcessInstance(1, "test", dt);

            if (piid != Guid.Empty)
            {
                int test = 0;
                test=WF.CompleteWorkItem(WF.GetPendingWorkItem(piid), 1, 1, "TEST1", "TEST2", true, false, true);
            }

            DataTable dt2 = new DataTable();
            dt2 = WF.GetWorkItemByProcess(piid);
        }

        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            DataSet ds2 = new DataSet();

            new GMSGeneralDALC().GetListOfMRRequiresApprovalByUserNumId(session.CompanyId, loginUserOrAlternateParty, ref ds2);

            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ViewState["SortDirection"] = "ASC";
                ViewState["SortField"] = "LevelID";

                int startIndex = ((dgPendingForYourApproval.CurrentPageIndex + 1) * this.dgPendingForYourApproval.PageSize) - (this.dgPendingForYourApproval.PageSize - 1);
                int endIndex = (dgPendingForYourApproval.CurrentPageIndex + 1) * this.dgPendingForYourApproval.PageSize;

                DataView dv = ds2.Tables[0].DefaultView;
                dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();


                this.dgPendingForYourApproval.DataSource = dv;
                this.dgPendingForYourApproval.DataBind();
                this.dgPendingForYourApproval.Visible = true;

            }
            else
            {
                this.lblSummaryPendingForYourApproval.Text = "No records.";

                this.lblSummaryPendingForYourApproval.Visible = true;
                this.dgPendingForYourApproval.DataSource = null;
                this.dgPendingForYourApproval.DataBind();
            }


            DataSet ds = new DataSet();

            new GMSGeneralDALC().GetListOfRejectedOrCancelledMRByUserNumId(session.CompanyId, loginUserOrAlternateParty, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["SortDirection"] = "ASC";
                ViewState["SortField"] = "MRNo";

                int startIndex = ((dgMRFormApproval.CurrentPageIndex + 1) * this.dgMRFormApproval.PageSize) - (this.dgMRFormApproval.PageSize - 1);
                int endIndex = (dgMRFormApproval.CurrentPageIndex + 1) * this.dgMRFormApproval.PageSize;

                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();



                this.dgMRFormApproval.DataSource = dv;
                this.dgMRFormApproval.DataBind();
                this.dgMRFormApproval.Visible = true;

            }
            else
            {
                this.lblSummary.Text = "No records.";

                this.lblSummary.Visible = true;
                this.dgMRFormApproval.DataSource = null;
                this.dgMRFormApproval.DataBind();
            }

            DataSet ds1 = new DataSet();


            //List of MRs requires your product Managers's approval

            new GMSGeneralDALC().GetListOfMRFailedDeliveryDateByUserNumId(session.CompanyId, loginUserOrAlternateParty, ref ds1);

            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                ViewState["SortDirection"] = "ASC";
                ViewState["SortField"] = "MRNo";

                int startIndex = ((dgMRFormApprovalManager.CurrentPageIndex + 1) * this.dgMRFormApprovalManager.PageSize) - (this.dgMRFormApprovalManager.PageSize - 1);
                int endIndex = (dgMRFormApprovalManager.CurrentPageIndex + 1) * this.dgMRFormApprovalManager.PageSize;

                DataView dv = ds1.Tables[0].DefaultView;
                dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();



                this.dgMRFormApprovalManager.DataSource = dv;
                this.dgMRFormApprovalManager.DataBind();
                this.dgMRFormApprovalManager.Visible = true;

            }
            else
            {
                this.lblSummaryApprovalManager.Text = "No records.";

                this.lblSummaryApprovalManager.Visible = true;
                this.dgMRFormApprovalManager.DataSource = null;
                this.dgMRFormApprovalManager.DataBind();
            }


        }

        protected void dgPendingForYourApproval_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }




        protected void dgMRFormApproval_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }



        protected void dgMRFormApprovalManager_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }

    }
}
