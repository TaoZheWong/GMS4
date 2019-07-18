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
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Collections.Generic;
using GMSCore.Entity;
using System.Data.SqlClient;


namespace GMSWeb.Procurement.Forms
{
    public partial class VendorApplicationForm : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Procurement");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Procurement"));
                return;
            }

            if (!Page.IsPostBack)
            { 
                //preload
                LoadData();
            }

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }


        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.Vendor> lstData = null;

            lstData = new SystemDataActivity().RetrieveVendorApplicationFormByVendorID(session.CompanyId);

            //Update search result
            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;


            if (lstData.Count > 0)
            {
                if (endIndex < lstData.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstData.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstData.Count.ToString() + " " + "of" + " " + lstData.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";
            this.lblSearchSummary.Visible = true;

            this.dgData.DataSource = lstData;
            this.dgData.DataBind();
        }
        #endregion
        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            lblSearchSummary.Text = e.NewPageIndex.ToString();

            lblSearchSummary.Visible = true;
            LoadData();

        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion

        #region btnAdd_Click
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddEditVendorApplicationForm.aspx");

        }

        #endregion
    }
}
