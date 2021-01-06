using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace GMSWeb.Finance.ProductVolume
{
    public partial class RefillingProductVolumeInput : GMSBasePage
    {
        short loginUserOrAlternateParty = 0;
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
                                                                            183);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            183);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!IsPostBack)
            {
                this.txtDateFrom.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                this.txtDateTo.SelectedDate = DateTime.Now.Date;
                this.RadGrid1.CurrentPageIndex = 0;
                RetrieveProductionVolume();
                this.RadGrid1.DataBind();
            }
        }

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.RadGrid1.CurrentPageIndex = 0;
            RetrieveProductionVolume();
            this.RadGrid1.DataBind();
        }
        #endregion


        #region Retrieve Data
        private void RetrieveProductionVolume()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            resultList.Visible = true;
            //DateTime datefrom = DateTime.Now.Date;
            //DateTime dateto = DateTime.Now.Date;
            DateTime datefrom = DateTime.Parse(this.txtDateFrom.SelectedDate.ToString());
            DateTime dateto = DateTime.Parse(this.txtDateTo.SelectedDate.ToString());

            try
            {
                ggdal.SelectProductionVolume(session.CompanyId, session.UserId, datefrom, dateto, ref ds);
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString());
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                this.lblSearchSummary.Visible = false;
                this.RadGrid1.DataSource = ds;
            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                this.RadGrid1.DataSource = null;
            }
        }
        #endregion

        #region Radgrid Control
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RetrieveProductionVolume();
        }

        protected void radGrid_OnPageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveProductionVolume();
        }

        protected void radGrid_OnPageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageSize;
            RetrieveProductionVolume();
        }

        protected void radGrid_OnCancel(object source, GridCommandEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.MasterTableView.ClearEditItems();
            RetrieveProductionVolume();
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem editableItem = e.Item as GridEditableItem;
                RadDatePicker datePicker = (RadDatePicker)editableItem.FindControl("txtDate");
                RadDropDownList ddlNewProduct = (RadDropDownList)editableItem.FindControl("ddlNewProduct");
                DateTime productDate = DateTime.Parse(datePicker.SelectedDate.ToString());
                int product = int.Parse(ddlNewProduct.SelectedValue);
                RadTextBox txtVolume = (RadTextBox)editableItem.FindControl("txtVolume");
                int volume = 0;
                if (!string.IsNullOrEmpty(txtVolume.Text))
                    volume = int.Parse(txtVolume.Text);
                RefillingProduct rp = RefillingProduct.RetrieveByDateByProduct(session.CompanyId, productDate, product);
                if (rp == null)
                {
                    try
                    {
                        RefillingProduct rp_new = new RefillingProduct();
                        rp_new.CoyID = session.CompanyId;
                        rp_new.ProductDate = productDate;
                        rp_new.ProductID = product;
                        rp_new.Volume = volume;
                        rp_new.UpdatedBy = session.UserId;
                        rp_new.UpdatedDate = DateTime.Now;
                        rp_new.Save();
                        alertMessage("New Refilling Product Added.");
                        this.RadGrid1.Rebind();
                    }
                    catch (Exception ex)
                    {
                        alertMessage(ex.ToString());
                        e.Canceled = true;
                        return;
                    }
                }
                else
                {
                    alertMessage("Product already existed on that date.");
                    e.Canceled = true;
                    return;
                }
            }
        }

        protected void RadGrid1_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
        {
            string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DateTime datefrom = DateTime.Parse(this.txtDateFrom.SelectedDate.ToString());
            DateTime dateto = DateTime.Parse(this.txtDateTo.SelectedDate.ToString());
            try
            {
                ggdal.SelectProductionVolume(session.CompanyId, session.UserId, datefrom, dateto, ref ds);
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString());
            }
            DataView dvOri = new DataView(ds.Tables[0]);
            DataTable dtOri = dvOri.ToTable(true, DataField);
            e.ListBox.DataSource = dtOri;
            e.ListBox.DataKeyField = DataField;
            e.ListBox.DataTextField = DataField;
            e.ListBox.DataValueField = DataField;
            e.ListBox.DataBind();
        }

        protected void RadGrid1_OnItemCreated(object sender, GridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            if (e.Item is GridEditableItem && e.Item.IsInEditMode && e.Item is IGridInsertItem)
            {
                GridEditableItem item = (GridEditableItem)e.Item;

                RadDropDownList ddlNewProduct = (RadDropDownList)item.FindControl("ddlNewProduct");
                ggdal.SelectRefillingProduct(session.CompanyId, session.UserId, ref ds);

                if (ds != null && ds.Tables.Count > 0)
                {
                    ddlNewProduct.DataSource = ds.Tables[0];
                    ddlNewProduct.DataBind();
                }
                else
                {
                    ddlNewProduct.DataSource = null;
                }
            }
            else if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadDropDownList ddlProduct = (RadDropDownList)item.FindControl("ddlProduct");
                ggdal.SelectRefillingProduct(session.CompanyId, session.UserId, ref ds);

                if (ds != null && ds.Tables.Count > 0)
                {
                    ddlProduct.DataSource = ds.Tables[0];
                    ddlProduct.DataBind();
                }
                else
                {
                    ddlProduct.DataSource = null;
                }
            }
        }

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.CommandName == "RebindGrid")
            {
                foreach (GridColumn column in RadGrid1.MasterTableView.Columns)
                {
                    column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                    column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.AndCurrentFilterValue = string.Empty;
                }
                RadGrid1.MasterTableView.FilterExpression = string.Empty;
                RadGrid1.MasterTableView.Rebind();
            }

            if (e.CommandName == "Save")
            {
                GridDataItem item = (GridDataItem)e.Item;
                short ID = 0;
                if (!string.IsNullOrEmpty(item.GetDataKeyValue("ID").ToString()))
                    ID = short.Parse(item.GetDataKeyValue("ID").ToString());
                RadDatePicker datePicker = (RadDatePicker)item.FindControl("txtDate");
                DateTime productDate = DateTime.Parse(datePicker.SelectedDate.ToString());

                RadDropDownList ddlProduct = (RadDropDownList)item.FindControl("ddlProduct");

                RadTextBox txtVolume = (RadTextBox)item.FindControl("txtVolume");
                double volume = 0;
                if (!string.IsNullOrEmpty(txtVolume.Text))
                    volume = double.Parse(txtVolume.Text);
                RefillingProduct rp = RefillingProduct.RetrieveByID(session.CompanyId, ID);
                if (rp != null)
                {
                    try
                    {
                        rp.ProductDate = productDate;
                        rp.ProductID = int.Parse(ddlProduct.SelectedValue);
                        rp.Volume = volume;
                        rp.UpdatedBy = session.UserId;
                        rp.UpdatedDate = DateTime.Now;
                        rp.Save();
                        alertMessage("Data Updated.");
                        this.RadGrid1.Rebind();
                    }
                    catch (Exception ex)
                    {
                        alertMessage(ex.ToString());
                        e.Canceled = true;
                        return;
                    }
                }
            }

            if (e.CommandName == "Delete")
            {
                GridDataItem item = (GridDataItem)e.Item;
                short ID = 0;
                if (!string.IsNullOrEmpty(item.GetDataKeyValue("ID").ToString()))
                    ID = short.Parse(item.GetDataKeyValue("ID").ToString());
             
                RefillingProduct rp = RefillingProduct.RetrieveByID(session.CompanyId, ID);
                if (rp != null)
                {
                    try
                    {
                        rp.Delete();
                        rp.Resync();
                        alertMessage("Data Deleted.");
                        this.RadGrid1.Rebind();
                    }
                    catch (Exception ex)
                    {
                        alertMessage(ex.ToString());
                        e.Canceled = true;
                        return;
                    }
                }
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            if (e.Item is GridEditableItem && e.Item.IsInEditMode && (e.Item is IGridInsertItem))
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                RadDatePicker datePicker = (RadDatePicker)item.FindControl("txtDate");
                datePicker.SelectedDate = DateTime.Now.Date;
            }
            else if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                HtmlInputHidden hidProduct = (HtmlInputHidden)item.FindControl("hidProduct");
                RadDropDownList ddlProduct = (RadDropDownList)item.FindControl("ddlProduct");

                ddlProduct.SelectedValue = hidProduct.Value.ToString();
            }
        }
        #endregion

        protected void alertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }
    }
}