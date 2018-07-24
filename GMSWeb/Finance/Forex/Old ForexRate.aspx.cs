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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.SysFinance.SharedInfo
{
    public partial class ForexRate : GMSBasePage
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
                                                                            2);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");


            if (!Page.IsPostBack)
            {
                //preload
                LoadCurrencyDDL();
                ddlHomeCurrency.SelectedValue = "SGD";
                LoadForexGrid();
                LoadYearDDL();
            }

            UserAccessModule uAccess2 = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            24);
            if (uAccess2 != null)
            {
                lnkAddNewRate.Visible = true;
            }
        }

        #region LoadCurrencyDDL
        private void LoadCurrencyDDL()
        {
            SystemDataActivity sDataActivity = new SystemDataActivity();
            IList<Currency> lstPurpose = sDataActivity.RetrieveAllCurrencyListSortByCode();
            this.ddlHomeCurrency.DataSource = lstPurpose;
            this.ddlHomeCurrency.DataBind();
        }
        #endregion

        #region LoadCurrencyDDL
        private void LoadYearDDL()
        {
            DataTable dtt1 = new DataTable();
            dtt1.Columns.Add("Year", typeof(string));

            for (int i = -1; i < 5; i++)
            {
                DataRow dr1 = dtt1.NewRow();
                dr1["Year"] = DateTime.Now.Year + i;

                dtt1.Rows.Add(dr1);
            }
            ddlNewYear.DataSource = dtt1;
            ddlNewYear.DataBind();
        }
        #endregion

        #region ddlHomeCurrency_SelectedIndexChanged
        protected void ddlHomeCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlHomeCurrency.SelectedValue != "0")
                LoadForexGrid();
            else
                this.PageMsgPanel.ShowMessage("You have not select a Home Currency.", MessagePanelControl.MessageEnumType.Alert);
        }
        #endregion

        #region LoadForexGrid
        protected void LoadForexGrid()
        {
            IList<ForeignExchangeRate> lstForexRate = new ForexActivity().RetrieveForexRateByHomeCurrencyCode(this.ddlHomeCurrency.SelectedValue);
            this.dgForex.DataSource = lstForexRate;
            this.dgForex.DataBind();
        }
        #endregion

        protected void lnkViewHistory_Click(object sender, EventArgs e)
        {
            LinkButton lnkViewHistory = (LinkButton)sender;
            TableRow tr = (TableRow)lnkViewHistory.Parent.Parent;
            
            string foreignCurrencyCode = ((HtmlInputHidden) tr.Cells[6].FindControl("hidForeignCurrencyCode")).Value;
            ClientScript.RegisterStartupScript(typeof(string), "CurrencyCode",
                         string.Format("jsOpenReport('SysFinance/SharedInfo/ForexRateHistory.aspx?FOREIGNCODE={0}&HOMECODE={1}');",
                                        foreignCurrencyCode,
                                         this.ddlHomeCurrency.SelectedValue),
                                        true);

            //ClientScript.RegisterStartupScript(typeof(string), "CurrencyCode",
            //             string.Format("jsOpenReport('Finance/SharedInfo/WebForm1.aspx');"),
            //                            true);
            LoadForexGrid();
            //ClientScript.RegisterStartupScript(typeof(string), "CurrencyCode",
            //             "<script>jsWinOpen('ForexRateHistory.aspx?FOREIGNCODE=" + foreignCurrencyCode + "&HOMECODE=" + this.ddlHomeCurrency.SelectedValue + "','795','580','yes')</script>",
            //                            true);

        }

        #region dgForex_ItemDataBound
        protected void dgForex_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewCurrency = (DropDownList)e.Item.FindControl("ddlNewCurrency");

                if (ddlNewCurrency != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in Currency dropdown list
                    IList<Currency> lstCurrency = null;
                    lstCurrency = sDataActivity.RetrieveAllCurrencyListSortByCode();
                    ddlNewCurrency.DataSource = lstCurrency;
                    ddlNewCurrency.DataBind();
                }
            }
        }
        #endregion

        #region dgForex_EditCommand
        protected void dgForex_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                        24);
            if (uAccess == null)
            {
                base.JScriptAlertMsg("You are not authorized to update Foreign Currency Rate.");
                LoadForexGrid();
                return;
            }
            if (ddlHomeCurrency.SelectedValue != "SGD")
            {
                base.JScriptAlertMsg("Please select SGD as the Home Currency before editing the rates.");
                LoadForexGrid();
                return;
            }

            this.dgForex.EditItemIndex = e.Item.ItemIndex;
            LoadForexGrid();
        }
        #endregion

        #region dgForex_CancelCommand
        protected void dgForex_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgForex.EditItemIndex = -1;
            LoadForexGrid();
        }
        #endregion

        #region dgForex_UpdateCommand
        protected void dgForex_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox txtEditBuyRate = (TextBox)e.Item.FindControl("txtEditBuyRate");
            TextBox txtEditSellRate = (TextBox)e.Item.FindControl("txtEditSellRate");
            TextBox txtEditMonthEnd = (TextBox)e.Item.FindControl("txtEditMonthEnd");
            HtmlInputHidden hidForeignCurrencyCode = (HtmlInputHidden)e.Item.FindControl("hidForeignCurrencyCode");
            HtmlInputHidden hidCreatedDate = (HtmlInputHidden)e.Item.FindControl("hidCreatedDate");

            if (txtEditBuyRate != null && txtEditSellRate != null && txtEditMonthEnd != null &&
                !string.IsNullOrEmpty(txtEditBuyRate.Text) && !string.IsNullOrEmpty(txtEditSellRate.Text) &&
                !string.IsNullOrEmpty(txtEditMonthEnd.Text) && hidForeignCurrencyCode != null && hidCreatedDate != null)
            {
                LogSession session = base.GetSessionInfo();

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                       24);
                if (uAccess == null)
                {
                    base.JScriptAlertMsg("You are not authorized to update Foreign Currency Rate.");
                    LoadForexGrid();
                    return;
                }

                if (ddlHomeCurrency.SelectedValue != "SGD")
                {
                    base.JScriptAlertMsg("Please select SGD as the Home Currency before editing the rates.");
                    LoadForexGrid();
                    return;
                }

                ForexActivity fACtivity = new ForexActivity();
                DateTime cDate = GMSUtil.ToDate(hidCreatedDate.Value);

                //Retrieving existing data from database
                double[,] rates = new double[16, 3];
                ForeignExchangeRate fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "AUD", cDate);
                rates[0, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[0, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[0, 2] = (rates[0, 0] + rates[0, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "CAD", cDate);
                rates[1, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[1, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[1, 2] = (rates[1, 0] + rates[1, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "CNY", cDate);
                rates[2, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[2, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[2, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "EUR", cDate);
                rates[3, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[3, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[3, 2] = (rates[3, 0] + rates[3, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "GBP", cDate);
                rates[4, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[4, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[4, 2] = (rates[4, 0] + rates[4, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "HKD", cDate);
                rates[5, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[5, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[5, 2] = (rates[5, 0] + rates[5, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "IDR", cDate);
                rates[6, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[6, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[6, 2] = (rates[6, 0] + rates[6, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "JPY", cDate);
                rates[7, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[7, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[7, 2] = (rates[7, 0] + rates[7, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "MYR", cDate);
                rates[8, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[8, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[8, 2] = (rates[8, 0] + rates[8, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "NOK", cDate);
                rates[9, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[9, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[9, 2] = (rates[9, 0] + rates[9, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "NTD", cDate);
                rates[10, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[10, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[10, 2] = (rates[10, 0] + rates[10, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "SEK", cDate);
                rates[11, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[11, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[11, 2] = (rates[11, 0] + rates[11, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "SFR", cDate);
                rates[12, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[12, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[12, 2] = (rates[12, 0] + rates[12, 1]) / 2.0;
                rates[13, 0] = 1;
                rates[13, 1] = 1;
                rates[13, 2] = 1;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "THB", cDate);
                rates[14, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[14, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[14, 2] = (rates[14, 0] + rates[14, 1]) / 2.0;
                fRate = fACtivity.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate("SGD", "USD", cDate);
                rates[15, 0] = GMSUtil.ToDouble(fRate.BuyRate);
                rates[15, 1] = GMSUtil.ToDouble(fRate.SellRate);
                rates[15, 2] = (rates[15, 0] + rates[15, 1]) / 2.0;

                //Delete all existing Forex Rates
                fACtivity.DeleteForeignExchangeRateByCreatedDate(cDate);

                //Update user selected values
                switch (hidForeignCurrencyCode.Value)
                {
                    case "AUD":
                        rates[0, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[0, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[0, 2] = (rates[0, 0] + rates[0, 1]) / 2.0;
                        break;
                    case "CAD":
                        rates[1, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[1, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[1, 2] = (rates[1, 0] + rates[1, 1]) / 2.0;
                        break;
                    case "CNY":
                        rates[2, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[2, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[2, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "EUR":
                        rates[3, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[3, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[3, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "GBP":
                        rates[4, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[4, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[4, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "HKD":
                        rates[5, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[5, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[5, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "IDR":
                        rates[6, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[6, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[6, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "JPY":
                        rates[7, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[7, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[7, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "MYR":
                        rates[8, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[8, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[8, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "NOK":
                        rates[9, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[9, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[9, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "NTD":
                        rates[10, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[10, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[10, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "SEK":
                        rates[11, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[11, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[11, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "SFR":
                        rates[12, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[12, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[12, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "THB":
                        rates[14, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[14, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[14, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                    case "USD":
                        rates[15, 0] = GMSUtil.ToDouble(txtEditBuyRate.Text.Trim());
                        rates[15, 1] = GMSUtil.ToDouble(txtEditSellRate.Text.Trim());
                        rates[15, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
                        break;
                }

                SystemDataActivity sDataActivity = new SystemDataActivity();
                IList<Currency> lstCurrency = sDataActivity.RetrieveAllCurrencyListSortByCode();
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        if (i != j)
                        {
                            ForeignExchangeRate forex = new ForeignExchangeRate();
                            forex.HomeCurrencyCode = lstCurrency[i].CurrencyCode;
                            forex.ForeignCurrencyCode = lstCurrency[j].CurrencyCode;
                            forex.BuyRate = (decimal)(1 / rates[i, 1] * rates[j, 0]);
                            forex.SellRate = (decimal)(1 / rates[i, 0] * rates[j, 1]);
                            forex.MonthEndRate = (decimal)(1 / rates[i, 2] * rates[j, 2]);
                            forex.CreatedBy = session.UserId;
                            forex.CreatedDate = cDate;
                            forex.ModifiedBy = session.UserId;
                            forex.ModifiedDate = DateTime.Now;
                            forex.IsInEffect = true;

                            ForeignExchangeRate existingForex = new ForexActivity().RetrieveForexRateByHomeForeignCurrencyCodeIsInEffect(forex.HomeCurrencyCode,
                                                                                                                                forex.ForeignCurrencyCode);

                            if (existingForex != null)
                            {
                                existingForex.IsInEffect = false;
                                existingForex.ModifiedBy = session.UserId;
                                existingForex.ModifiedDate = DateTime.Now;
                            }

                            ResultType result = new ForexActivity().CreateForeignExchangeRate(ref forex, ref existingForex,
                                                                                            session);

                            switch (result)
                            {
                                case ResultType.Ok:
                                    break;
                                default:
                                    this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                    return;
                            }
                        }
                    }
                }
            }
            this.dgForex.EditItemIndex = -1;
            ddlHomeCurrency.SelectedValue = "SGD";
            LoadForexGrid();
        }
        #endregion

        #region dgForex_CreateCommand
        protected void dgForex_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            24);
                if (uAccess == null)
                {
                    base.JScriptAlertMsg("You are not authorized to update Foreign Currency Rate.");
                    LoadForexGrid();
                    return;
                }

                DropDownList ddlNewCurrency = (DropDownList)e.Item.FindControl("ddlNewCurrency");
                TextBox txtNewBuyRate = (TextBox)e.Item.FindControl("txtNewBuyRate");
                TextBox txtNewSellRate = (TextBox)e.Item.FindControl("txtNewSellRate");
                TextBox txtNewMonthEnd = (TextBox)e.Item.FindControl("txtNewMonthEnd");

                if (txtNewBuyRate != null && !string.IsNullOrEmpty(txtNewBuyRate.Text) &&
                    txtNewSellRate != null && !string.IsNullOrEmpty(txtNewSellRate.Text) &&
                    txtNewMonthEnd != null && !string.IsNullOrEmpty(txtNewMonthEnd.Text) &&
                    ddlNewCurrency != null)
                {
                    try
                    {
                        // check if newly inserted homecurrencycode-foreigncurrencycode already exist and isineffect = 1
                        ForeignExchangeRate existingForex = new ForexActivity().RetrieveForexRateByHomeForeignCurrencyCodeIsInEffect(this.ddlHomeCurrency.SelectedValue,
                                                                                                                        ddlNewCurrency.SelectedValue);

                        if (existingForex != null)
                        {
                            existingForex.IsInEffect = false;
                            existingForex.ModifiedBy = session.UserId;
                            existingForex.ModifiedDate = DateTime.Now;
                        }

                        ForeignExchangeRate forex = new ForeignExchangeRate();
                        forex.HomeCurrencyCode = this.ddlHomeCurrency.SelectedValue;
                        forex.ForeignCurrencyCode = ddlNewCurrency.SelectedValue;
                        forex.BuyRate = GMSUtil.ToDecimal(txtNewBuyRate.Text.Trim());
                        forex.SellRate = GMSUtil.ToDecimal(txtNewSellRate.Text.Trim());
                        forex.MonthEndRate = GMSUtil.ToDecimal(txtNewMonthEnd.Text.Trim());
                        forex.CreatedBy = session.UserId;
                        forex.CreatedDate = DateTime.Now;
                        forex.IsInEffect = true;

                        ResultType result = new ForexActivity().CreateForeignExchangeRate(ref forex, ref existingForex,
                                                                                        session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                LoadForexGrid();
                                break;
                            default:
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region AddNewRateCommand
        protected void AddNewRateCommand(object sender, CommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            24);
            if (uAccess == null)
            {
                base.JScriptAlertMsg("You are not authorized to update Foreign Currency Rate.");
                LoadForexGrid();
                return;
            }
            double[,] rates = new double[16,3];
            rates[0,0] = GMSUtil.ToDouble(txtNewAUD1.Text.Trim());
            rates[0, 1] = GMSUtil.ToDouble(txtNewAUD2.Text.Trim());
            rates[0, 2] = (rates[0, 0] + rates[0, 1]) / 2.0;
            rates[1, 0] = GMSUtil.ToDouble(txtNewCAD1.Text.Trim());
            rates[1, 1] = GMSUtil.ToDouble(txtNewCAD2.Text.Trim());
            rates[1, 2] = (rates[1, 0] + rates[1, 1]) / 2.0;
            rates[2, 0] = GMSUtil.ToDouble(txtNewCNY1.Text.Trim());
            rates[2, 1] = GMSUtil.ToDouble(txtNewCNY2.Text.Trim());
            rates[2, 2] = (rates[2, 0] + rates[2, 1]) / 2.0;
            rates[3, 0] = GMSUtil.ToDouble(txtNewEUR1.Text.Trim());
            rates[3, 1] = GMSUtil.ToDouble(txtNewEUR2.Text.Trim());
            rates[3, 2] = (rates[3, 0] + rates[3, 1]) / 2.0;
            rates[4, 0] = GMSUtil.ToDouble(txtNewGBP1.Text.Trim());
            rates[4, 1] = GMSUtil.ToDouble(txtNewGBP2.Text.Trim());
            rates[4, 2] = (rates[4, 0] + rates[4, 1]) / 2.0;
            rates[5, 0] = GMSUtil.ToDouble(txtNewHKD1.Text.Trim());
            rates[5, 1] = GMSUtil.ToDouble(txtNewHKD2.Text.Trim());
            rates[5, 2] = (rates[5, 0] + rates[5, 1]) / 2.0;
            rates[6, 0] = GMSUtil.ToDouble(txtNewIDR1.Text.Trim());
            rates[6, 1] = GMSUtil.ToDouble(txtNewIDR2.Text.Trim());
            rates[6, 2] = (rates[6, 0] + rates[6, 1]) / 2.0;
            rates[7, 0] = GMSUtil.ToDouble(txtNewJPY1.Text.Trim());
            rates[7, 1] = GMSUtil.ToDouble(txtNewJPY2.Text.Trim());
            rates[7, 2] = (rates[7, 0] + rates[7, 1]) / 2.0;
            rates[8, 0] = GMSUtil.ToDouble(txtNewMYR1.Text.Trim());
            rates[8, 1] = GMSUtil.ToDouble(txtNewMYR2.Text.Trim());
            rates[8, 2] = (rates[8, 0] + rates[8, 1]) / 2.0;
            rates[9, 0] = GMSUtil.ToDouble(txtNewNOK1.Text.Trim());
            rates[9, 1] = GMSUtil.ToDouble(txtNewNOK2.Text.Trim());
            rates[9, 2] = (rates[9, 0] + rates[9, 1]) / 2.0;
            rates[10, 0] = GMSUtil.ToDouble(txtNewNTD1.Text.Trim());
            rates[10, 1] = GMSUtil.ToDouble(txtNewNTD2.Text.Trim());
            rates[10, 2] = (rates[10, 0] + rates[10, 1]) / 2.0;
            rates[11, 0] = GMSUtil.ToDouble(txtNewSEK1.Text.Trim());
            rates[11, 1] = GMSUtil.ToDouble(txtNewSEK2.Text.Trim());
            rates[11, 2] = (rates[11, 0] + rates[11, 1]) / 2.0;
            rates[12, 0] = GMSUtil.ToDouble(txtNewSFR1.Text.Trim());
            rates[12, 1] = GMSUtil.ToDouble(txtNewSFR2.Text.Trim());
            rates[12, 2] = (rates[12, 0] + rates[12, 1]) / 2.0;
            rates[13, 0] = 1;
            rates[13, 1] = 1;
            rates[13, 2] = 1;
            rates[14, 0] = GMSUtil.ToDouble(txtNewTHB1.Text.Trim());
            rates[14, 1] = GMSUtil.ToDouble(txtNewTHB2.Text.Trim());
            rates[14, 2] = (rates[14, 0] + rates[14, 1]) / 2.0;
            rates[15, 0] = GMSUtil.ToDouble(txtNewUSD1.Text.Trim());
            rates[15, 1] = GMSUtil.ToDouble(txtNewUSD2.Text.Trim());
            rates[15, 2] = (rates[15, 0] + rates[15, 1]) / 2.0;

            SystemDataActivity sDataActivity = new SystemDataActivity();
            IList<Currency> lstCurrency = sDataActivity.RetrieveAllCurrencyListSortByCode();
            for(int i= 0; i<16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (i != j)
                    {
                        ForeignExchangeRate forex = new ForeignExchangeRate();
                        forex.HomeCurrencyCode = lstCurrency[i].CurrencyCode;
                        forex.ForeignCurrencyCode = lstCurrency[j].CurrencyCode;
                        forex.BuyRate = (decimal) (1 / rates[i, 1] * rates[j, 0]);
                        forex.SellRate = (decimal) (1 / rates[i, 0] * rates[j, 1]);
                        forex.MonthEndRate = (decimal) (1 / rates[i, 2] * rates[j, 2]);
                        forex.CreatedBy = session.UserId;
                        forex.CreatedDate = new DateTime(GMSUtil.ToInt(ddlNewYear.SelectedValue), GMSUtil.ToInt(ddlNewMonth.SelectedValue), 1);
                        forex.IsInEffect = true;

                        ForeignExchangeRate existingForex = new ForexActivity().RetrieveForexRateByHomeForeignCurrencyCodeIsInEffect(forex.HomeCurrencyCode,
                                                                                                                            forex.ForeignCurrencyCode);

                        if (existingForex != null)
                        {
                            existingForex.IsInEffect = false;
                            existingForex.ModifiedBy = session.UserId;
                            existingForex.ModifiedDate = DateTime.Now;
                        }

                        ResultType result = new ForexActivity().CreateForeignExchangeRate(ref forex, ref existingForex,
                                                                                        session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                break;
                            default:
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                    }
                }
            }
            ddlHomeCurrency.SelectedValue = "SGD";
            LoadForexGrid();
        }
        #endregion
    }
}
