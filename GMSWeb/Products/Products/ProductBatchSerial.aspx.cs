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
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Products.Products
{
    public partial class ProductBatchSerial : GMSBasePage
    {
        private string productCode = "", type = "", warehouse = "";
        protected short loginUserOrAlternateParty = 0;
        string isLargeFont, isOptimizedTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            this.productCode = Request.Params["PRODUCTCODE"];
            this.warehouse = Request.Params["WAREHOUSE"];
            this.type = Request.Params["TYPE"];

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Search", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {

                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;

            //Getting LargerFont Cookies
            HttpCookie isLargeFontCookie = Request.Cookies["isLargeFont"];
            if (null == isLargeFontCookie)
                isLargeFont = "";
            else
                isLargeFont = isLargeFontCookie.Value == "true" ? "largeFont" : "";

            //Getting optimizedtable Cookies
            HttpCookie isOptimizedTableCookie = Request.Cookies["isOptimizedTable"];
            if (null == isOptimizedTableCookie)
                isOptimizedTable = "";
            else
                isOptimizedTable = isOptimizedTableCookie.Value == "true" ? "optimizedTable" : "";

            LoadData();
        }

        protected void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            DataSet dsUnpostBatchSerial = new DataSet();
            DataSet dsUnpostBatchSerial_lms = new DataSet();
           
            if (session.StatusType.ToString() == "L")
            {
                CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                    sc1.Url = session.CMSWebServiceAddress.Trim();
                else
                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                    sc1.Url = session.GASLMSWebServiceAddress.Trim();
                else
                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                dsUnpostBatchSerial = sc1.GetUnpostBatchSerial(productCode, this.type);

                if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                    sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                else
                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                dsUnpostBatchSerial_lms = sc1.GetUnpostBatchSerial(productCode, this.type);
            }

            //CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
            //if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
            //{
            //    sc1.Url = session.CMSWebServiceAddress.Trim();
            //}
            //else
            //    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

            //dsUnpostBatchSerial = sc1.GetUnpostBatchSerial(productCode, this.type);

            if (this.type == "Batch")
            {
                try
                {
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GetProductAvailableBatch(session.CompanyId, this.productCode, this.warehouse);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < dsUnpostBatchSerial.Tables[0].Rows.Count; k++)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if (dsUnpostBatchSerial.Tables[0].Rows[k]["BatchNo"].ToString() == ds.Tables[0].Rows[j]["BatchNo"].ToString() && dsUnpostBatchSerial.Tables[0].Rows[k]["Warehouse"].ToString() == ds.Tables[0].Rows[j]["Warehouse"].ToString())
                                {
                                    decimal final = Convert.ToDecimal(ds.Tables[0].Rows[j]["Qty"].ToString()) - Convert.ToDecimal(dsUnpostBatchSerial.Tables[0].Rows[k]["Qty"].ToString());
                                    if (final <= 0)
                                        ds.Tables[0].Rows[j].Delete();
                                    else
                                        ds.Tables[0].Rows[j]["Qty"] = final;
                                }
                            }
                        }

                        for (int k = 0; k < dsUnpostBatchSerial_lms.Tables[0].Rows.Count; k++)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if (dsUnpostBatchSerial_lms.Tables[0].Rows[k]["BatchNo"].ToString() == ds.Tables[0].Rows[j]["BatchNo"].ToString() && dsUnpostBatchSerial_lms.Tables[0].Rows[k]["Warehouse"].ToString() == ds.Tables[0].Rows[j]["Warehouse"].ToString())
                                {
                                    decimal final = Convert.ToDecimal(ds.Tables[0].Rows[j]["Qty"].ToString()) - Convert.ToDecimal(dsUnpostBatchSerial_lms.Tables[0].Rows[k]["Qty"].ToString());
                                    if (final <= 0)
                                        ds.Tables[0].Rows[j].Delete();
                                    else
                                        ds.Tables[0].Rows[j]["Qty"] = final;
                                }
                            }
                        }

                        this.dgData.DataSource = ds;
                        this.dgData.DataBind();
                    }
                    else
                    {
                        lblMsg.Text = "No record is found.";
                    }
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
            else if (this.type == "Serial")
            {
                try
                {
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GetProductAvailableSerial(session.CompanyId, this.productCode, this.warehouse);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < dsUnpostBatchSerial.Tables[0].Rows.Count; k++)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if (ds.Tables[0].Rows[j].RowState.ToString() != "Deleted")
                                {
                                    if (dsUnpostBatchSerial.Tables[0].Rows[k]["SerialNo"].ToString() == ds.Tables[0].Rows[j]["SerialNo"].ToString())
                                    {
                                        ds.Tables[0].Rows[j].Delete();
                                    }
                                }
                            }
                        }

                        for (int k = 0; k < dsUnpostBatchSerial_lms.Tables[0].Rows.Count; k++)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if (ds.Tables[0].Rows[j].RowState.ToString() != "Deleted")
                                {
                                    if (dsUnpostBatchSerial_lms.Tables[0].Rows[k]["SerialNo"].ToString() == ds.Tables[0].Rows[j]["SerialNo"].ToString())
                                    {
                                        ds.Tables[0].Rows[j].Delete();
                                    }
                                }
                            }
                        }

                        this.dgSerial.DataSource = ds;
                        this.dgSerial.DataBind();
                    }
                    else
                    {
                        lblMsg.Text = "No record is found.";
                    }
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
        }


        #region dgPO_ItemDataBound
        protected void dgPO_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            
        }
        #endregion

        public string getIsOptimizedTable
        {
            get { return isOptimizedTable; }
        }

        public string getIsLargeFont
        {
            get { return isLargeFont; }
        }
    }
}
