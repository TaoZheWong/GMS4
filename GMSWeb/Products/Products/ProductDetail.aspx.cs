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
using GMSWeb.CustomCtrl;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Collections.Generic;

namespace GMSWeb.Products.Products
{
	public partial class ProductDetail : GMSBasePage
	{
		protected short loginUserOrAlternateParty = 0;
        protected bool canAccessCost = false;
        protected bool canAccessProductStatus = false;
        protected bool isGasDivision = false;
        protected bool isWeldingDivision = false;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
		{
			LogSession session = base.GetSessionInfo();
			string currentLink = "Products";	
			if (Request.Params["CurrentLink"] != null)
			{
				currentLink = Request.Params["CurrentLink"].ToString().Trim();				
			}
			Master.setCurrentLink(currentLink);						
			if (session == null)
			{
				Response.Redirect(base.SessionTimeOutPage("Products"));
				return;
			}

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

			UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
																			89);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            89);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Products"));           

            if (!Page.IsPostBack)
			{
				//preload
				if (Request.Params["ProductCode"] != null)
				{
					hidProductCode.Value = Request.Params["ProductCode"].ToString().Trim();
                    LoadData();
				}
			}

			uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
																		   127);
            uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            127);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
            {
				btnSave.Visible = false;
				txtRemarks.Enabled = false;
			}
			else
			{
				btnSave.Visible = true;
				txtRemarks.Enabled = true;
				this.dgData3.ShowFooter = true;
				this.dgData3.Columns[1].Visible = true;
			}

            

            string javaScript =
@"
<script type=""text/javascript"">
function ViewSODetail()
{			
	var url = 'ProductOrderDetail.aspx?PRODUCTCODE=' + document.getElementById('"; javaScript += hidProductCode.ClientID; javaScript += @"').value + '&TYPE=SO'; 
	window.open(url,"""",""width="" + 900 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
	return false;
}
function ViewBODetail()
{			
	var url = 'ProductOrderDetail.aspx?PRODUCTCODE=' + document.getElementById('"; javaScript += hidProductCode.ClientID; javaScript += @"').value + '&TYPE=BO'; 
	window.open(url,"""",""width="" + 900 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
	return false;
}
function ViewPODetail()
{			
	var url = 'ProductOrderDetail.aspx?PRODUCTGROUPCODE=' + document.getElementById('"; javaScript += hidProductGroupCode.ClientID; javaScript += @"').value + '&PRODUCTCODE=' + document.getElementById('"; javaScript += hidProductCode.ClientID; javaScript += @"').value + '&TYPE=PO'; 
	window.open(url,"""",""width="" + 900 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
	return false;
}
function ViewTNDetail()
{			
	var url = 'ProductOrderDetail.aspx?PRODUCTCODE=' + document.getElementById('"; javaScript += hidProductCode.ClientID; javaScript += @"').value + '&TYPE=TN'; 
	window.open(url,"""",""width="" + 900 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
	return false;
}
function ViewBatchSerial(wh)
{     
    var url = 'ProductBatchSerial.aspx?PRODUCTCODE=' + document.getElementById('"; javaScript += hidProductCode.ClientID; javaScript += @"').value + '&WAREHOUSE='+wh+'&TYPE=' + document.getElementById('"; javaScript += hidTrackType.ClientID; javaScript += @"').value; 
	window.open(url,"""",""width="" + 900 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
	return false;
}
function ViewWHDetail(wh)
{     
    var url = 'ProductOrderDetail.aspx?PRODUCTCODE=' + document.getElementById('"; javaScript += hidProductCode.ClientID; javaScript += @"').value + '&TYPE=Detail&WAREHOUSE='+wh; 
	window.open(url,"""",""width="" + 900 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
	return false;
}
function ViewMultipleUOM()
{			
	var url = 'ProductUOM.aspx?PRODUCTCODE=' + document.getElementById('"; javaScript += hidProductCode.ClientID; javaScript += @"').value; 
	window.open(url,"""",""width="" + 900 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
	return false;
}
</script>
";
			Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
		}
		#endregion

		#region LoadData
		private void LoadData()
		{
			LogSession session = base.GetSessionInfo();
            

            if (this.hidProductCode.Value.Trim() == "")
			{
				base.JScriptAlertMsg("Please input a product to view.");
				return;
			}

			string productCode = this.hidProductCode.Value.Trim();

			// Get Product Info
			DataSet dsProductInfo = new DataSet();
			new GMSGeneralDALC().GetProductInfoByProductCode(session.CompanyId, productCode, ref dsProductInfo);
			if ((dsProductInfo != null) && (dsProductInfo.Tables[0].Rows.Count > 0))
			{
				for (int i = 0; i < dsProductInfo.Tables[0].Rows.Count; i++)
				{
				   txtRemarks.Text = dsProductInfo.Tables[0].Rows[i]["Remarks"].ToString();
				   lblModifiedDate.Text = "Last Modified on "+dsProductInfo.Tables[0].Rows[i]["ModifiedDate"].ToString();
				}
			}

			//Get Product Price (restriction by access right and user role below(N = cannot see even have access right))
			UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
																		   141);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            141);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
			{
				dgData4.Visible = false;
			}
			else
			{
				DataSet dsProductPrice = new DataSet();
				try
				{
					new GMSGeneralDALC().GetProductPriceByProductCode(session.CompanyId, productCode, ref dsProductPrice);
					if ((dsProductPrice != null))
					{
						this.dgData4.DataSource = dsProductPrice.Tables[0];
						this.dgData4.DataBind();
					}
				}
				catch (Exception ex)
				{
					this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
				}
			}			

			//Get Product Location
			DataSet dsProductLocation = new DataSet();
			try
			{
				new GMSGeneralDALC().GetProductLocationByProductCode(session.CompanyId, productCode, ref dsProductLocation);
				if ((dsProductLocation != null))
				{
					this.dgData3.DataSource = dsProductLocation.Tables[0];
					this.dgData3.DataBind();

                    UserAccessModule uAccess1 = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                           127);
                    IList<UserAccessModuleForCompany> uAccessForCompanyList1 = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                           127);
                    if (uAccess1 == null && (uAccessForCompanyList1 != null && uAccessForCompanyList1.Count == 0))
                    {
                        this.dgData3.ShowFooter = false;
                        this.dgData3.Columns[1].Visible = false;
                    }  
				}
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}
            		 
			ProductsDataDALC dacl = new ProductsDataDALC();
			DataSet ds = new DataSet();
            
            try
			{
                if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    ds = sc.GetProductDetail(session.CompanyId, productCode);
                }
                else if (session.StatusType.ToString() == "L")
                {
                    
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                    {
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    }
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    ds = sc1.GetProductDetail(productCode);                    

                }
                else if(session.StatusType.ToString() == "S")
                {
                    string query = "CALL \"AF_API_GET_SAP_ITEMMASTERINFO\" ('" + productCode + "', '', '', '', '', '')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "ProductCode", "ProductName", "ProductGroupCode", "Volume", "UOM", "WeightedCost", "OnOrderQuantity", "OnPOQuantity", "OnBOQuantity", "AvailableQuantity", "IsGasDivision", "IsWeldingDivision", "ProdForeignName", "TrackedByBatch", "TrackedBySerial", "ProductNotes", "IsActive", "ItemType", "ProductGroupName", "OnHandQuantity",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                }
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				this.lblProductCode.Text = ds.Tables[0].Rows[0]["ProductCode"].ToString();
				this.lblProductName.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();
				this.lblProductGroup.Text = ds.Tables[0].Rows[0]["ProductGroupName"].ToString();
				this.lblUOM.Text = ds.Tables[0].Rows[0]["UOM"].ToString();
                this.hidProductGroupCode.Value = ds.Tables[0].Rows[0]["ProductGroupCode"].ToString();
                if (session.StatusType.ToString() == "L")
                {
                    this.hidTrackType.Value = ds.Tables[0].Rows[0]["TrackType"].ToString();
                    isGasDivision = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsGasDivision"].ToString());
                    isWeldingDivision = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsWeldingDivision"].ToString());
                }

                DataSet dsAccessCost = new DataSet();
                new GMSGeneralDALC().CanUserAccessCost(session.CompanyId, hidProductCode.Value, loginUserOrAlternateParty, ref dsAccessCost);
                canAccessCost = Convert.ToBoolean(dsAccessCost.Tables[0].Rows[0]["result"]);

                DivisionUser du = DivisionUser.RetrieveByKey(session.CompanyId, loginUserOrAlternateParty);
                if (du != null)
                {
                    if (du.DivisionID == "GAS" && isGasDivision)
                    {
                        canAccessProductStatus = true;
                    }
                    else if (du.DivisionID == "WSD" && isWeldingDivision)
                    {
                        canAccessProductStatus = true;
                    }
                }
                else
                    canAccessProductStatus = true;

                if (this.hidTrackType.Value == "")
                    dgData.Columns[3].Visible = false;

                DataSet dsPMName = new DataSet();
				(new GMSGeneralDALC()).IsProductManager(session.CompanyId, ds.Tables[0].Rows[0]["ProductCode"].ToString(), 1, ref dsPMName);
				if (dsPMName != null && dsPMName.Tables.Count > 0 && dsPMName.Tables[0].Rows.Count > 0)
				{
					this.lblProductManagerName.Text = dsPMName.Tables[0].Rows[0]["UserRealName"].ToString();
				}
				
				string userRole = "N";
				DataSet dsUserRole = new DataSet();
				 (new GMSGeneralDALC()).GetUserRoleByUserID(session.CompanyId, loginUserOrAlternateParty, ref dsUserRole);
				 if (dsUserRole != null && dsUserRole.Tables.Count > 0 && dsUserRole.Tables[0].Rows.Count > 0)
				{
					userRole = dsUserRole.Tables[0].Rows[0]["UserRole"].ToString();
				}

				//added by kim 2013-Jan-22 to allow all user to view product stock details
				if (userRole == "N")
				{
					userRole = "S";
					//Added by Adam to limit normal user cannot see price
					dgData4.Visible = false;
				}
				
                if(canAccessCost)
                    setProductTeamAccess(ds);
				else if (userRole == "P" || userRole == "C" || userRole == "G" || userRole == "B")
				{
                    DataSet ds1 = new DataSet();
                    (new GMSGeneralDALC()).IsProductManagerByProductGroupCode(session.CompanyId, ds.Tables[0].Rows[0]["ProductGroupCode"].ToString(), loginUserOrAlternateParty, ref ds1);
					//Is product manager
					if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
					{
                        setProductTeamAccess(ds);
                    }
					else
					{
						PMRegion1.Visible = false;
						//PMRegion2.Visible = false;
						PMRegion3.Visible = false;
						PMRegion4.Visible = false;
						//Added by Adam to limit not product manager user cannot view price
						dgData4.Visible = false;

						if (userRole == "B")
						{
                            if (canAccessProductStatus)
                            {
                                PMRegion2.Visible = true;
                                PMRegion3.Visible = true;
                            }
							PMRegion4.Visible = true;

                            //Added by Adam to limit B user with access can view price
                            //if (uAccess != null)   
                            if (!(uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0)))
                            {
								dgData4.Visible = true;
							}

							//Stock Status
							#region Stock Status
							DataSet ds2 = new DataSet();
                            DataSet ds2_lms = new DataSet();
							try
							{
                                if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                                {
                                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                                    {
                                        sc.Url = session.WebServiceAddress.Trim();
                                    }
                                    else
                                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                                    ds2 = sc.GetProductStockStatus(session.CompanyId, productCode);
                                }
                                // Get ProductStatus From LMS
                                if (session.StatusType.ToString() == "H")
                                {
                                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                                    {
                                        sc1.Url = session.CMSWebServiceAddress.Trim();
                                    }
                                    else
                                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";                                  
                                    ds2_lms = sc1.GetProductStockStatus(productCode);                                   
                                }
                                else if (session.StatusType.ToString() == "L")
                                {
                                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                                    {
                                        sc1.Url = session.CMSWebServiceAddress.Trim();
                                    }
                                    else
                                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                                    ds2 = sc1.GetProductWarehouse(productCode);
                                }
                                else if (session.StatusType.ToString() == "S")
                                {
                                    string query = "CALL \"AF_API_GET_SAP_STOCK_STATUS\" ('" + productCode + "', '', '', '', '', '2099-12-31', 'Y')";
                                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                                    ds2 = sop.GET_SAP_QueryData(session.CompanyId, query,
                                    "ItemCode", "Warehouse", "OnHand", "Committed", "Quantity", "WarehouseName", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                                    
                                }
                            }
							catch (Exception ex)
							{
								this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
							}

							if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
							{
                                if (session.StatusType.ToString() == "H" && ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds2_lms.Tables[0].Rows.Count; i++)
                                    {
                                        bool isDupe = false;
                                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                        {
                                            if (ds2_lms.Tables[0].Rows[i][0].ToString() == ds2.Tables[0].Rows[j][0].ToString())
                                            {
                                                ds2.Tables[0].Rows[j][2] = GMSUtil.ToDecimal(ds2.Tables[0].Rows[j][2].ToString()) + GMSUtil.ToDecimal(ds2_lms.Tables[0].Rows[i][2].ToString());
                                                isDupe = true;
                                                break;
                                            }
                                        }
                                        if (!isDupe)
                                        {
                                            ds2.Tables[0].ImportRow(ds2_lms.Tables[0].Rows[i]);
                                        }
                                    }
                                }
								this.dgData.DataSource = ds2;
								this.dgData.DataBind();
							}
							#endregion

							//Product Detail
							#region Product Detail
							DataSet ds4 = new DataSet();
                            DataSet ds4_lms = new DataSet();
                            try
							{
                                if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                                {
                                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                                    {
                                        sc.Url = session.WebServiceAddress.Trim();
                                    }
                                    else
                                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                                    ds4 = sc.GetProductDetailByProductCode(session.CompanyId, productCode);
                                }
                               
                                if (session.StatusType.ToString() == "H" && (DateTime.Now > (DateTime.Parse(session.LMSParallelRunEndDate))))
                                {
                                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                                    {
                                        sc1.Url = session.CMSWebServiceAddress.Trim();
                                    }
                                    else
                                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                                    ds4_lms = sc1.GetProductStockStatusByProductCode(productCode);
                                }
                                else if(session.StatusType.ToString() == "L")
                                {
                                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                                        sc1.Url = session.CMSWebServiceAddress.Trim();
                                    else
                                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                                    ds4 = sc1.GetProductDetailByProductCode(productCode);

                                    if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                                    {
                                        sc1.Url = session.GASLMSWebServiceAddress.Trim();
                                        ds4 = sc1.GetProductDetailByProductCode(productCode);
                                    }

                                    if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                                    {
                                        sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                                        ds4_lms = sc1.GetProductDetailByProductCode(productCode);
                                    }
                                }
							}
							catch (Exception ex)
							{
								this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
							}

                            if ((session.StatusType.ToString() == "H" && (DateTime.Now <= (DateTime.Parse(session.LMSParallelRunEndDate)))) || session.StatusType.ToString() == "A")
                            {
                                if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                                {
                                    this.lblSO.Text = ds4.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                                    this.lblBO.Text = ds4.Tables[0].Rows[0]["OnBOQuantity"].ToString();
                                    this.lblTN.Text = ds4.Tables[0].Rows[0]["OnTNQuantity"].ToString();
                                    if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                                    {
                                        this.lblPO.Text = ds4.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                                    }                                  
                                }
                            }
                            else if (session.StatusType.ToString() == "H" && (DateTime.Now > (DateTime.Parse(session.LMSParallelRunEndDate))))
                            {
                                if (ds4_lms != null && ds4_lms.Tables.Count > 0 && ds4_lms.Tables[0].Rows.Count > 0)
                                {
                                    this.lblSO.Text = ds4_lms.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                                    this.lblBO.Text = ds4_lms.Tables[0].Rows[0]["OnBOQuantity"].ToString();
                                    this.lblTN.Text = ds4_lms.Tables[0].Rows[0]["OnTNQuantity"].ToString();
                                    if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                                    {
                                        this.lblPO.Text = ds4.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                                    }
                                }
                            }
                            else if(session.StatusType.ToString() == "L")
                            {
                                if(ds4_lms != null && ds4_lms.Tables.Count > 0)
                                {
                                    for (int i = 0; i < ds4_lms.Tables[0].Rows.Count; i++)
                                    {
                                        for (int j = 0; j < ds4.Tables[0].Rows.Count; j++)
                                        {
                                            if (ds4_lms.Tables[0].Rows[i][0].ToString() == ds4.Tables[0].Rows[j][0].ToString())
                                            {
                                                ds4.Tables[0].Rows[j][1] = GMSUtil.ToDecimal(ds4.Tables[0].Rows[j][1].ToString()) + GMSUtil.ToDecimal(ds4_lms.Tables[0].Rows[i][1].ToString()); // SO
                                                ds4.Tables[0].Rows[j][2] = GMSUtil.ToDecimal(ds4.Tables[0].Rows[j][2].ToString()) + GMSUtil.ToDecimal(ds4_lms.Tables[0].Rows[i][2].ToString()); // BO
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                                {
                                    this.lblSO.Text = ds4.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                                    this.lblBO.Text = ds4.Tables[0].Rows[0]["OnBOQuantity"].ToString();                                    
                                    this.lblPO.Text = ds4.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                                }
                            }
                            else if (session.StatusType.ToString() == "S")
                            {
                                this.lblSO.Text = ds.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                                this.lblBO.Text = ds.Tables[0].Rows[0]["OnBOQuantity"].ToString();
                                this.lblPO.Text = ds.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                            }

                            #endregion

                            //Stock Movement
                            #region Stock Movement
                            DataSet ds3 = new DataSet();
                            DataSet ds3_lms = new DataSet();

							try
							{
                                if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                                {
                                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                                    {
                                        sc.Url = session.WebServiceAddress.Trim();
                                    }
                                    else
                                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                                    ds3 = sc.GetProductStockMovement(session.CompanyId, productCode);
                                }

                                if (session.StatusType.ToString() == "H")
                                {
                                    DateTime LMSParallelRunEndDate = GMSUtil.ToDate(session.LMSParallelRunEndDate);
                                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                                    {
                                        sc1.Url = session.CMSWebServiceAddress.Trim();
                                    }
                                    else
                                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                                    ds3_lms = sc1.GetProductStockMovement(productCode, LMSParallelRunEndDate.ToString("yyyy-MM-dd"));
                                }
                                else if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
                                {
                                    string query = "CALL \"AF_API_GET_SAP_STOCK_MOVEMENT\" ('" + productCode + "', '" + productCode + "', '', '', '', '')";
                                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                                    ds3 = sop.GET_SAP_QueryData(session.CompanyId, query,
                                    "TrnType", "TrnNo", "TrnDate", "RefNo", "AccountCode", "AccountName", "ProductCode", "ProductName", "ProductGroupCode", "ProductGroupName", "ReceivedQty", "IssuedQty", "BalanceQty", "Cost", "CostWT", "Currency", "ExchangeRate", "Narration", "DocNo", "Warehouse",
                                    "TransNum", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                                    System.Data.DataColumn newColumn = new System.Data.DataColumn("DBVersion", typeof(int));
                                    newColumn.DefaultValue = 0;
                                    ds3.Tables[0].Columns.Add(newColumn);
                                }
							}
							catch (Exception ex)
							{
								this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
							}
							if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
							{
                                if (session.StatusType.ToString() == "H" && ds3_lms != null && ds3_lms.Tables.Count > 0 && ds3_lms.Tables[0].Rows.Count > 0)
                                {
                                    ds3.Tables[0].Merge(ds3_lms.Tables[0], true, System.Data.MissingSchemaAction.Ignore);
                                }

								DataView dv = new DataView(ds3.Tables[0]);                                
                                if (session.StatusType.ToString() == "L")
                                    dv.Sort = "TrnDate desc, TransNum desc, DocNo desc";
                                else
                                    dv.Sort = "DBVersion, TrnDate desc";
                                this.dgData2.DataSource = dv;
								this.dgData2.DataBind();
							}
							#endregion
						}
					}
				}
				else
				{
					PMRegion1.Visible = false;
					//PMRegion2.Visible = false;
					PMRegion3.Visible = false;
					PMRegion4.Visible = false;

					if (userRole == "S")
					{
                        if (canAccessProductStatus)
                        {
                            PMRegion3.Visible = true;
                            PMRegion4.Visible = true;
                        }

						//Stock Status
						#region Stock Status
						DataSet ds2 = new DataSet();
                        DataSet ds2_lms = new DataSet();
						try
						{
                            if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                            {
                                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                                {
                                    sc.Url = session.WebServiceAddress.Trim();
                                }
                                else
                                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                                ds2 = sc.GetProductStockStatus(session.CompanyId, productCode);
                            }

                            // Get ProductStatus From LMS
                            if (session.StatusType.ToString() == "H")
                            {
                                CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                                if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                                {
                                    sc1.Url = session.CMSWebServiceAddress.Trim();
                                }
                                else
                                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";                              
                                ds2_lms = sc1.GetProductStockStatus(productCode);                              
                            }
                            else if (session.StatusType.ToString() == "L")
                            {
                                CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                                if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                                {
                                    sc1.Url = session.CMSWebServiceAddress.Trim();
                                }
                                else
                                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                                ds2 = sc1.GetProductWarehouse(productCode);
                            }
                            else if (session.StatusType.ToString() == "S")
                            {
                                string query = "CALL \"AF_API_GET_SAP_STOCK_STATUS\" ('" + productCode + "', '', '', '', '', '2099-12-31', 'Y')";
                                SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                                ds2 = sop.GET_SAP_QueryData(session.CompanyId, query,
                                "ItemCode", "Warehouse", "OnHand", "Committed", "Quantity", "WarehouseName", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                                "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                               
                            }
                        }
						catch (Exception ex)
						{
							this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
						}

						if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
						{
                            if (session.StatusType.ToString() == "H" && ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds2_lms.Tables[0].Rows.Count; i++)
                                {
                                    bool isDupe = false;
                                    for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                    {
                                        if (ds2_lms.Tables[0].Rows[i][0].ToString() == ds2.Tables[0].Rows[j][0].ToString())
                                        {
                                            ds2.Tables[0].Rows[j][2] = GMSUtil.ToDecimal(ds2.Tables[0].Rows[j][2].ToString()) + GMSUtil.ToDecimal(ds2_lms.Tables[0].Rows[i][2].ToString());
                                            isDupe = true;
                                            break;
                                        }
                                    }
                                    if (!isDupe)
                                    {
                                        ds2.Tables[0].ImportRow(ds2_lms.Tables[0].Rows[i]);
                                    }
                                }
                            }

							this.dgData.DataSource = ds2;
							this.dgData.DataBind();
						}
						#endregion

						//Product Detail
						#region Product Detail
						DataSet ds4 = new DataSet();
                        DataSet ds4_lms = new DataSet();

                        try
						{
                            if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                            {
                                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                                {
                                    sc.Url = session.WebServiceAddress.Trim();
                                }
                                else
                                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                                ds4 = sc.GetProductDetailByProductCode(session.CompanyId, productCode);
                            }

                            if (session.StatusType.ToString() == "H" && (DateTime.Now > (DateTime.Parse(session.LMSParallelRunEndDate))))
                            {
                                CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                                if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                                {
                                    sc1.Url = session.CMSWebServiceAddress.Trim();
                                }
                                else
                                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                                ds4_lms = sc1.GetProductStockStatusByProductCode(productCode);
                            }
                            else if(session.StatusType.ToString() == "L")
                            {
                                CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                                if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                                    sc1.Url = session.CMSWebServiceAddress.Trim();
                                else
                                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                                ds4 = sc1.GetProductDetailByProductCode(productCode);

                                if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                                {
                                    sc1.Url = session.GASLMSWebServiceAddress.Trim();                                    
                                    ds4 = sc1.GetProductDetailByProductCode(productCode);
                                }

                                if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                                {
                                    sc1.Url = session.WSDLMSWebServiceAddress.Trim();                                
                                    ds4_lms = sc1.GetProductDetailByProductCode(productCode);
                                }
                            }
						}
						catch (Exception ex)
						{
							this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
						}

                        if ((session.StatusType.ToString() == "H" && (DateTime.Now <= (DateTime.Parse(session.LMSParallelRunEndDate)))) || session.StatusType.ToString() == "A")
                        {
                            if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                            {
                                this.lblSO.Text = ds4.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                                this.lblBO.Text = ds4.Tables[0].Rows[0]["OnBOQuantity"].ToString();
                                this.lblTN.Text = ds4.Tables[0].Rows[0]["OnTNQuantity"].ToString();
                                if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                                {
                                    this.lblPO.Text = ds4.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                                }
                            }
                        }
                        else if (session.StatusType.ToString() == "H" && (DateTime.Now > (DateTime.Parse(session.LMSParallelRunEndDate))))
                        {
                            if (ds4_lms != null && ds4_lms.Tables.Count > 0 && ds4_lms.Tables[0].Rows.Count > 0)
                            {
                                this.lblSO.Text = ds4_lms.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                                this.lblBO.Text = ds4_lms.Tables[0].Rows[0]["OnBOQuantity"].ToString();
                                this.lblTN.Text = ds4_lms.Tables[0].Rows[0]["OnTNQuantity"].ToString();
                                if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                                {
                                    this.lblPO.Text = ds4.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                                }
                            }
                        }
                        else if (session.StatusType.ToString() == "L")
                        {
                            if (ds4_lms != null && ds4_lms.Tables.Count > 0)
                            {
                                for (int i = 0; i < ds4_lms.Tables[0].Rows.Count; i++)
                                {
                                    for (int j = 0; j < ds4.Tables[0].Rows.Count; j++)
                                    {
                                        if (ds4_lms.Tables[0].Rows[i][0].ToString() == ds4.Tables[0].Rows[j][0].ToString())
                                        {
                                            ds4.Tables[0].Rows[j][1] = GMSUtil.ToDecimal(ds4.Tables[0].Rows[j][1].ToString()) + GMSUtil.ToDecimal(ds4_lms.Tables[0].Rows[i][1].ToString()); // SO
                                            ds4.Tables[0].Rows[j][2] = GMSUtil.ToDecimal(ds4.Tables[0].Rows[j][2].ToString()) + GMSUtil.ToDecimal(ds4_lms.Tables[0].Rows[i][2].ToString()); // BO
                                            break;
                                        }
                                    }
                                }
                            }

                            if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                            {
                                this.lblSO.Text = ds4.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                                this.lblBO.Text = ds4.Tables[0].Rows[0]["OnBOQuantity"].ToString();
                                this.lblPO.Text = ds4.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                            }
                        }
                        else if (session.StatusType.ToString() == "S")
                        {
                            this.lblSO.Text = ds.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                            this.lblBO.Text = ds.Tables[0].Rows[0]["OnBOQuantity"].ToString();
                            this.lblPO.Text = ds.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                        }


                        #endregion

                        //Stock Movement
                        #region Stock Movement
                        DataSet ds3 = new DataSet();
                        DataSet ds3_lms = new DataSet();

                        try
                        {
                            if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                            {
                                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                                {
                                    sc.Url = session.WebServiceAddress.Trim();
                                }
                                else
                                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                                ds3 = sc.GetProductStockMovement(session.CompanyId, productCode);
                            }

                            if (session.StatusType.ToString() == "H")
                            {
                                DateTime LMSParallelRunEndDate = GMSUtil.ToDate(session.LMSParallelRunEndDate);
                                CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                                if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                                {
                                    sc1.Url = session.CMSWebServiceAddress.Trim();
                                }
                                else
                                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                                ds3_lms = sc1.GetProductStockMovement(productCode, LMSParallelRunEndDate.ToString("yyyy-MM-dd"));
                            }
                            else if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
                            {
                                string query = "CALL \"AF_API_GET_SAP_STOCK_MOVEMENT\" ('" + productCode + "', '" + productCode + "', '', '', '', '')";
                                SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());

                                ds3 = sop.GET_SAP_QueryData(session.CompanyId, query,
                                "TrnType", "TrnNo", "TrnDate", "RefNo", "AccountCode", "AccountName", "ProductCode", "ProductName", "ProductGroupCode", "ProductGroupName", "ReceivedQty", "IssuedQty", "BalanceQty", "Cost", "CostWT", "Currency", "ExchangeRate", "Narration", "DocNo", "Warehouse",
                                "TransNum", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                                System.Data.DataColumn newColumn = new System.Data.DataColumn("DBVersion", typeof(int));
                                newColumn.DefaultValue = 0;
                                ds3.Tables[0].Columns.Add(newColumn);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        }
                        if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                        {
                            if (session.StatusType.ToString() == "H" && ds3_lms != null && ds3_lms.Tables.Count > 0 && ds3_lms.Tables[0].Rows.Count > 0)
                            {
                                ds3.Tables[0].Merge(ds3_lms.Tables[0], true, System.Data.MissingSchemaAction.Ignore);
                            }

                            DataView dv = new DataView(ds3.Tables[0]);
                            if (session.StatusType.ToString() == "L")
                                dv.Sort = "TrnDate desc, TransNum desc, DocNo desc";
                            else
                                dv.Sort = "DBVersion, TrnDate desc";
                            this.dgData2.DataSource = dv;
                            this.dgData2.DataBind();
                        }
                        #endregion

                    }


                }
			}
			
		}
		#endregion

		#region dgData datagrid PageIndexChanged event handling
		protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			DataGrid dtg = (DataGrid)source;
			dtg.CurrentPageIndex = e.NewPageIndex;
			LoadData();
		}
		#endregion

		#region dgData2 datagrid PageIndexChanged event handling
		protected void dgData2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			DataGrid dtg = (DataGrid)source;
			dtg.CurrentPageIndex = e.NewPageIndex;
			LoadData();
		}
        #endregion

        protected void dgData2_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                           142);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            142);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
            {
                this.dgData2.Columns[9].Visible = false;
            }
            else
            {
                this.dgData2.Columns[9].Visible = true;
            }

        }

        #region dgData3_ItemDataBound
        protected void dgData3_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			LogSession session = base.GetSessionInfo();

			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
				if (lnkDelete != null)
					lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
			}

		}
		#endregion

		#region dgData3_CreateCommand
		protected void dgData3_CreateCommand(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Create")
			{
				LogSession session = base.GetSessionInfo();
				TextBox txtNewLocation = (TextBox)e.Item.FindControl("txtNewLocation");
				try
				{
					new GMSGeneralDALC().InsertProductLocationByProductCode(session.CompanyId, this.hidProductCode.Value.Trim(), 0, txtNewLocation.Text, session.UserId);
					LoadData();

				}
				catch (Exception ex)
				{
					this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
					return;
				}


			}
		}
		#endregion

		#region dgData3_DeleteCommand
		protected void dgData3_DeleteCommand(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				LogSession session = base.GetSessionInfo();
				HtmlInputHidden hidPLID = (HtmlInputHidden)e.Item.FindControl("hidPLID");

				if (hidPLID != null )
				{
					try
					{
						new GMSGeneralDALC().DeleteProductLocationByProductCode(session.CompanyId, this.hidProductCode.Value.Trim(), GMSUtil.ToInt(hidPLID.Value));
						LoadData();

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


	    protected void btnSave_Click(object sender, EventArgs e)
		{
			LogSession session = base.GetSessionInfo();

			string productCode = this.hidProductCode.Value.Trim();
			DataSet dsProductInfo = new DataSet();

			string remarks = txtRemarks.Text.Trim();

			if (remarks.Length > 4000)
			{
				lblMessage.Text = "Remarks Length cannot exceed 4000 characters!";
				
			}
			else
			{ 
				new GMSGeneralDALC().UpdateProductInfoByProductCode(session.CompanyId, productCode, remarks, session.UserId);
				lblMessage.Text = "Data has been Updated!";               
			}	
		}

        protected void setProductTeamAccess(DataSet ds)
        {
            LogSession session = base.GetSessionInfo();
            string productCode = this.hidProductCode.Value.Trim();
            PMRegion1.Visible = true;
            if (canAccessProductStatus)
            {
                PMRegion2.Visible = true;
                PMRegion3.Visible = true;
            }
            PMRegion4.Visible = true;

            if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
            {
                this.lblWeightedCost.Text = double.Parse(ds.Tables[0].Rows[0]["WeightedCost"].ToString(), System.Globalization.NumberStyles.Any).ToString("#0.0000");
            }
            else if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
            {
                DataSet dsProductCost = new DataSet();
                SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                dsProductCost = sop.GetProductCost(session.CompanyId, productCode);
                if (dsProductCost != null && dsProductCost.Tables.Count > 0 && dsProductCost.Tables[0].Rows.Count > 0)
                {
                    this.lblWeightedCost.Text = double.Parse(dsProductCost.Tables[0].Rows[0]["WeightedCost"].ToString(), System.Globalization.NumberStyles.Any).ToString("#0.0000");
                }
            }

            //Stock Status
            #region Stock Status
            DataSet ds2 = new DataSet();
            DataSet ds2_lms = new DataSet();
            try
            {
                if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    ds2 = sc.GetProductStockStatus(session.CompanyId, productCode);
                }
                // Get ProductStatus From LMS
                if (session.StatusType.ToString() == "H")
                {
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                    {
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    }
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    ds2_lms = sc1.GetProductStockStatus(productCode);
                }
                else if (session.StatusType.ToString() == "L")
                {
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                    {
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    }
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    ds2 = sc1.GetProductWarehouse(productCode);
                }
                else if (session.StatusType.ToString() == "S")
                {
                    string query = "CALL \"AF_API_GET_SAP_STOCK_STATUS\" ('" + productCode + "', '', '', '', '', '2099-12-31', 'Y')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds2 = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "ItemCode", "Warehouse", "OnHand", "Committed", "Quantity", "WarehouseName", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");


                }
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                if (session.StatusType.ToString() == "H" && ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds2_lms.Tables[0].Rows.Count; i++)
                    {
                        bool isDupe = false;
                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                        {
                            if (ds2_lms.Tables[0].Rows[i][0].ToString() == ds2.Tables[0].Rows[j][0].ToString())
                            {
                                ds2.Tables[0].Rows[j][2] = GMSUtil.ToDecimal(ds2.Tables[0].Rows[j][2].ToString()) + GMSUtil.ToDecimal(ds2_lms.Tables[0].Rows[i][2].ToString());
                                isDupe = true;
                                break;
                            }
                        }
                        if (!isDupe)
                        {
                            ds2.Tables[0].ImportRow(ds2_lms.Tables[0].Rows[i]);
                        }
                    }
                }

                this.dgData.DataSource = ds2;
                this.dgData.DataBind();
            }
            #endregion

            //Product Detail
            #region Product Detail
            DataSet ds4 = new DataSet();
            DataSet ds4_lms = new DataSet();
            try
            {

                if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    ds4 = sc.GetProductDetailByProductCode(session.CompanyId, productCode);
                }

                if (session.StatusType.ToString() == "H" && (DateTime.Now > (DateTime.Parse(session.LMSParallelRunEndDate))))
                {
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                    {
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    }
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    ds4_lms = sc1.GetProductStockStatusByProductCode(productCode);
                }
                else if (session.StatusType.ToString() == "L")
                {
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    ds4 = sc1.GetProductDetailByProductCode(productCode);

                    if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                    {
                        sc1.Url = session.GASLMSWebServiceAddress.Trim();
                        ds4 = sc1.GetProductDetailByProductCode(productCode);
                    }

                    if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                    {
                        sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                        ds4_lms = sc1.GetProductDetailByProductCode(productCode);
                    }

                }
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if ((session.StatusType.ToString() == "H" && (DateTime.Now <= (DateTime.Parse(session.LMSParallelRunEndDate)))) || session.StatusType.ToString() == "A")
            {
                if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                {
                    this.lblSO.Text = ds4.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                    this.lblBO.Text = ds4.Tables[0].Rows[0]["OnBOQuantity"].ToString();
                    this.lblTN.Text = ds4.Tables[0].Rows[0]["OnTNQuantity"].ToString();
                    if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                    {
                        this.lblPO.Text = ds4.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                    }
                }
            }
            else if (session.StatusType.ToString() == "H" && (DateTime.Now > (DateTime.Parse(session.LMSParallelRunEndDate))))
            {
                if (ds4_lms != null && ds4_lms.Tables.Count > 0 && ds4_lms.Tables[0].Rows.Count > 0)
                {
                    this.lblSO.Text = ds4_lms.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                    this.lblBO.Text = ds4_lms.Tables[0].Rows[0]["OnBOQuantity"].ToString();
                    this.lblTN.Text = ds4_lms.Tables[0].Rows[0]["OnTNQuantity"].ToString();
                    if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                    {
                        this.lblPO.Text = ds4.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                    }
                }
            }
            else if (session.StatusType.ToString() == "L")
            {
                if (ds4_lms != null && ds4_lms.Tables.Count > 0)
                {
                    for (int i = 0; i < ds4_lms.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < ds4.Tables[0].Rows.Count; j++)
                        {
                            if (ds4_lms.Tables[0].Rows[i][0].ToString() == ds4.Tables[0].Rows[j][0].ToString())
                            {
                                ds4.Tables[0].Rows[j][1] = GMSUtil.ToDecimal(ds4.Tables[0].Rows[j][1].ToString()) + GMSUtil.ToDecimal(ds4_lms.Tables[0].Rows[i][1].ToString()); // SO
                                ds4.Tables[0].Rows[j][2] = GMSUtil.ToDecimal(ds4.Tables[0].Rows[j][2].ToString()) + GMSUtil.ToDecimal(ds4_lms.Tables[0].Rows[i][2].ToString()); // BO
                                break;
                            }
                        }
                    }
                }

                if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)                      
                {
                    this.lblSO.Text = ds4.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                    this.lblBO.Text = ds4.Tables[0].Rows[0]["OnBOQuantity"].ToString();
                    this.lblPO.Text = ds4.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                }

            }
            else if (session.StatusType.ToString() == "S")
            {
                this.lblSO.Text = ds.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                this.lblBO.Text = ds.Tables[0].Rows[0]["OnBOQuantity"].ToString();
                this.lblPO.Text = ds.Tables[0].Rows[0]["OnPOQuantity"].ToString();
            }

            #endregion

            //Stock Movement
            #region Stock Movement
            DataSet ds3 = new DataSet();
            DataSet ds3_lms = new DataSet();
            try
            {
                if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    ds3 = sc.GetProductStockMovement(session.CompanyId, productCode);
                }

                if (session.StatusType.ToString() == "H")
                {
                    DateTime LMSParallelRunEndDate = GMSUtil.ToDate(session.LMSParallelRunEndDate);
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                    {
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    }
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    ds3_lms = sc1.GetProductStockMovement(productCode, LMSParallelRunEndDate.ToString("yyyy-MM-dd"));
                }
                else if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
                {
                    string query = "CALL \"AF_API_GET_SAP_STOCK_MOVEMENT\" ('" + productCode + "', '" + productCode + "', '', '', '', '')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds3 = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "TrnType", "TrnNo", "TrnDate", "RefNo", "AccountCode", "AccountName", "ProductCode", "ProductName", "ProductGroupCode", "ProductGroupName", "ReceivedQty", "IssuedQty", "BalanceQty", "Cost", "CostWT", "Currency", "ExchangeRate", "Narration", "DocNo", "Warehouse",
                    "TransNum", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    System.Data.DataColumn newColumn = new System.Data.DataColumn("DBVersion", typeof(int));
                    newColumn.DefaultValue = 0;
                    ds3.Tables[0].Columns.Add(newColumn);
                }
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                if (session.StatusType.ToString() == "H" && ds3_lms != null && ds3_lms.Tables.Count > 0 && ds3_lms.Tables[0].Rows.Count > 0)
                {
                    ds3.Tables[0].Merge(ds3_lms.Tables[0], true, System.Data.MissingSchemaAction.Ignore);
                }
                
                DataView dv = new DataView(ds3.Tables[0]);
                if(session.StatusType.ToString() == "L")
                    dv.Sort = "TrnDate desc, TransNum desc, DocNo desc";
                else
                    dv.Sort = "DBVersion, TrnDate desc";                
                this.dgData2.DataSource = dv;
                this.dgData2.DataBind();                

            }

            #endregion
        }
    }
}
