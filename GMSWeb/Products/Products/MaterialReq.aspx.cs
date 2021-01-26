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

using GMSCore;
using GMSWeb.CustomCtrl;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Web.Services;
using System.Text;
using AjaxControlToolkit;

namespace GMSWeb.Products.Products
{
    public partial class MaterialReq : GMSBasePage
    {
        //DataSet lstUserAccess = new DataSet();
        protected string userRole = "";
        protected string currentLink = "";
        protected short loginUserOrAlternateParty = 0;
        public string coy_id = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            currentLink = "Products";
            //lblPageHeader.Text = "Products";

            LogSession session = base.GetSessionInfo();

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();                
            }

            Master.setCurrentLink(currentLink); 
           
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            RetriveAccess();

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                            120);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (session.CompanyId == 28 || session.CompanyId == 57 || session.CompanyId == 103 || session.CompanyId == 81)
            {
                btnAdd.Visible = false;
            }

            if (!Page.IsPostBack)
            {
               
                //preload
                trnDateFrom.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("dd/MM/yyyy");
                trnDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                PopulateProductManager();

                ViewState["SortField"] = "MRID";
                ViewState["SortDirection"] = "DESC";
                LoadDDL();
                LoadData();
              
            }

          


            string javaScript =
@"
<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>
<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/importing.js""></script>
";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);

            //btnImport.Attributes.Add("onclick", "javascript:" + btnImport.ClientID + ".disabled=true;" + this.GetPostBackEventReference(btnImport));
            btnSearch.Focus();
        }

        private void LoadDDL()
        {
            LogSession session = base.GetSessionInfo();

            SystemDataActivity sDataActivity = new SystemDataActivity();
            IList<GMSCore.Entity.MRStatus> lstMRStatus = null;

            lstMRStatus = sDataActivity.RetrieveAllMRStatus();
            ddlStatus.DataSource = lstMRStatus;
            ddlStatus.SelectedValue = "%%";
            ddlStatus.DataBind();

            MRActivity mrActivity = new MRActivity();
            IList<GMSCore.Entity.MRPurchaser> lstMRPurchaser = null;
            lstMRPurchaser = mrActivity.RetrieveMRPurchaserByCoyID(session.CompanyId);
            ddlPurchaser.DataSource = lstMRPurchaser;            
            ddlPurchaser.DataBind();

            ddlPurchaser.Items.Add(new ListItem("All", "%%"));
            ddlPurchaser.SelectedValue = "%%";
            
        }

        #region PopulateProductManager
        private void PopulateProductManager()
        {
            LogSession session = base.GetSessionInfo();

            ProductsDataDALC dacl = new ProductsDataDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetProductManagersForMR(session.CompanyId, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            ddlProductManager.DataSource = ds;
            ddlProductManager.DataBind();

            ddlProductManager.Items.Insert(0, new ListItem("-All-", "%%"));
        }
        #endregion

        

        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            DateTime dateFrom = GMSUtil.ToDate(trnDateFrom.Text.Trim());
            DateTime dateTo = GMSUtil.ToDate(trnDateTo.Text.Trim());
            string accountCode = "%" + txtAccountCode.Text.Trim() + "%";
            string accountName = "%" + txtAccountName.Text.Trim() + "%";
            string productCode = "%" + txtProductCode.Text.Trim() + "%";
            string productName = "%" + txtProductName.Text.Trim() + "%";
            string productGroup = "%" + txtProductGroup.Text.Trim() + "%";
            string productGroupName = "%" + txtProductGroupName.Text.Trim() + "%";
            string prouductManagerID = ddlProductManager.SelectedValue;
            string requestor = "%" + txtRequestor.Text.Trim() + "%";
            string mrNo = "%" + txtMRNo.Text.Trim() + "%";
            string vendor = "%" + txtVendor.Text.Trim() + "%";
            string poNo = "%" + txtPONo.Text.Trim() + "%";
            string purchaser = ddlPurchaser.SelectedValue;
            string refNo = "%" + txtRefNo.Text.Trim() + "%";
            string budgetCode = "%" + txtBudgetCode.Text.Trim() + "%";
            string projectNo = "%" + txtProjectNo.Text.Trim() + "%";
            string requestorRemarks = "%" + txtRequestorRemarks.Text.Trim() + "%";




            string status = ddlStatus.SelectedValue;
            
            DataSet ds = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisition(session.CompanyId, loginUserOrAlternateParty, dateFrom, dateTo, status, accountCode, accountName, productCode, productName, productGroup, productGroupName, prouductManagerID, requestor, userRole, mrNo, vendor, poNo, purchaser, refNo, budgetCode, projectNo, requestorRemarks, session.MRScheme ,ref ds);


            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (endIndex < ds.Tables[0].Rows.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();

                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = dv;
                try
                {
                    this.dgData.DataBind();
                }
                catch
                {
                    this.dgData.CurrentPageIndex = 0;
                    this.dgData.DataBind();

                }
                this.dgData.Visible = true;
            }
            else
            {
                this.lblSearchSummary.Text = "No records.";

                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = null;
                try
                {
                    this.dgData.DataBind();
                }
                catch
                {
                    this.dgData.CurrentPageIndex = 0;
                    this.dgData.DataBind();
                }
            }

        }

        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if ((userRole == "Product Team") || (userRole == "Purchasing")) 
            {
                this.dgData.Columns[6].Visible = true;
            }
            else
            {
                this.dgData.Columns[6].Visible = false;
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink hlc = (HyperLink)e.Item.FindControl("HyperLink1");
                HtmlInputHidden hidMRNo = (HtmlInputHidden)e.Item.FindControl("hidMRNo");
                hlc.NavigateUrl = "NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&CoyID=" + session.CompanyId+ "&MRNo="+ hidMRNo.Value; ;
            }              
        }

        
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }

        protected void SortData(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
            {
                switch (ViewState["SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["SortField"] = e.SortExpression;
                ViewState["SortDirection"] = "ASC";
            }
            LoadData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnAddMR_Click(object sender, EventArgs e)
        {

            LogSession session = base.GetSessionInfo();

            Session.Remove("PurchaseInformation");
            Session.Remove("ConfirmedSalesInformation");
            Session.Remove("VendorInformation");
            Session.Remove("ProductDetail");
            Session.Remove("FileUpload1");
            Session.Remove("Purchaser");
           

            SystemDataActivity dalc = new SystemDataActivity();
            Company coy = null;
            try
            {
                coy = dalc.RetrieveCompanyById(session.CompanyId, session);
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }

            Response.Redirect("NewMaterialRequisition.aspx?CoyID="+session.CompanyId+"&CurrentLink=" + currentLink);
           
        }

        private void RetriveAccess()
        {
            LogSession session = base.GetSessionInfo(); 

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


            DataSet lstUserRole = new DataSet();

            new GMSGeneralDALC().GetMRUserRoleByUserNumIDCoyID(session.CompanyId, loginUserOrAlternateParty, ref lstUserRole);

            if ((lstUserRole != null) && (lstUserRole.Tables[0].Rows.Count > 0))
            {
                userRole = lstUserRole.Tables[0].Rows[0]["UserRole"].ToString();
            }
           
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            //LogSession session = base.GetSessionInfo();


            //SystemDataActivity sysDataActivity = new SystemDataActivity();
            //Company company = sysDataActivity.RetrieveCompanyById(GMSUtil.ToShort(session.CompanyId), session);
            //if (company != null)
            //{
            //    Package2Class package = new Package2Class();
            //    object pVarPersistStgOfHost = null;

            //    try
            //    {
            //        package.LoadFromSQLServer(
            //            "(local)",
            //            "sa",
            //            "",
            //            DTSSQLServerStorageFlags.DTSSQLStgFlag_UseTrustedConnection,
            //            null,
            //            null,
            //            null,
            //            "Update MR GRN Detail - " + company.Code.Trim().ToString(),
            //            ref pVarPersistStgOfHost);
            //        package.Execute();
            //        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "progress_stop", "progress_stop();", true);

            //        StringBuilder str = new StringBuilder();
            //        str.Append("<script language='javascript'>");
            //        str.Append("alert('Finished Importing.');");
            //        str.Append("window.location.href = \"MaterialReq.aspx\"");
            //        str.Append("</script>");
            //        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
            //    }
            //    catch (System.Runtime.InteropServices.COMException ex)
            //    {
            //        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            //    }
            //    catch (System.Exception ex)
            //    {
            //        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            //    }
            //    finally
            //    {
            //        package.UnInitialize(); // unwrap the package
            //        System.Runtime.InteropServices.Marshal.ReleaseComObject(package); // tell interop to release the reference
            //        package = null;
            //    }
                

            //}

            /*
            new GMSGeneralDALC().RemoveMSTProduct(session.CompanyId);

            DataSet ds = new DataSet();

            try
            {
                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                {
                    sc.Url = session.WebServiceAddress.Trim();
                }
                else
                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";

                ds = sc.GetProductFromA21(session.CompanyId);

                
               
                if ((ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0))
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GMSCore.Entity.MSTProduct mst = new GMSCore.Entity.MSTProduct();
                        
                        mst.Company = session.CompanyId.ToString();
                        mst.Product = dr["Product"].ToString();
                        mst.PName = dr["PName"].ToString();
                        mst.Class1 = dr["Class1"].ToString();
                        mst.Volume = GMSUtil.ToDouble(dr["Volume"].ToString());
                        mst.PurchaseUnit = dr["purchaseunit"].ToString();
                        mst.CostWt = GMSUtil.ToDouble(dr["costwt"].ToString());
                        mst.OnOrderQuantity = GMSUtil.ToDouble(dr["OnOrderQuantity"].ToString());
                        mst.OnPOQuantity = GMSUtil.ToDouble(dr["OnPOQuantity"].ToString());                
                        mst.Save();
                    }

                   
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('" + ex.Message + "')", true);
                ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('" + ex.Message + "')", true);

            }

            


            new GMSGeneralDALC().ImportNewProdFromA21(session.CompanyId);
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "progress_stop", "progress_stop();", true);   

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("alert('Finished Importing.');");
            str.Append("window.location.href = \"MaterialReq.aspx\"");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);

            */
        }

              
    }
}
