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
using System.Transactions;
using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Text;
using System.Web.Services;
using AjaxControlToolkit;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace GMSWeb.Products.Products
{
    public partial class NewMaterialRequisition : GMSBasePage    
    {
        protected ReportDocument crReportDocument;
        protected string userRole = "";       
        DataSet lstWarehouse = new DataSet();
        UserAccessModule viewPurchaseInformationAccess = null;
        UserAccessModule viewStockInformationAccess = null;
        UserAccessModule viewAllProductGroupPurchaseInformationAccess = null;
        string currentLink = "";
        DataTable workTable = null;
        protected int levelID = 0;
        protected string currentrandomID = "";
        protected short loginUserOrAlternateParty = 0;
        Company coy = null;
        protected string purchaserApproval = "0";
        protected string coy_id = "";

        protected void Page_Load(object sender, EventArgs e)
        {    
            currentLink = "Products";            
            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink); 

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            coy = new SystemDataActivity().RetrieveCompanyByCoyId(session.CompanyId);

            RetriveAccess();  

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                           120);

            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage(currentLink));

            StringBuilder str = new StringBuilder();

                    

            if (!Page.IsPostBack)
            {                
                //preload
                if (Request.Params["MRNo"] != null && Request.Params["MRNo"].ToString() != "")
                {
                    ddlReport.Visible = true;
                    lnkPrintPDF.Visible = true;
                    lnkPrintReport.Visible = true;
                    this.lblMRNo.Text = Request.Params["MRNo"].ToString();

                    DataSet ds = new DataSet();
                    new GMSGeneralDALC().CanUserAccessDocument(session.CompanyId, "MR", this.lblMRNo.Text, loginUserOrAlternateParty, ref ds);

                    if (!(Convert.ToBoolean(ds.Tables[0].Rows[0]["result"])))
                        Response.Redirect(base.UnauthorizedPage(currentLink));

                }
                else
                {
                    ddlReport.Visible = false;
                    lnkPrintPDF.Visible = false;
                    lnkPrintReport.Visible = false;
                    this.txtMRDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }

                Session.Remove("Purchaser");                
                                 
                LoadRequestorDDL();
                LoadData();               
                LoadReportDDL();          
               
                if (Request.Params["ActiveTab"] != null && Request.Params["ActiveTab"].ToString() != "")
                {
                    Tabs.ActiveTabIndex = GMSUtil.ToInt(Request.Params["ActiveTab"].ToString());
                }                
            }
            
            if (Page.IsPostBack)
            {
                if (Request.Params["CoyID"] != null && Request.Params["CoyID"].ToString() != "")
                {
                    coy_id = Request.Params["CoyID"].ToString();

                    if (session.CompanyId.ToString() != Request.Params["CoyID"].ToString())
                    {
                        Response.Redirect("MaterialReq.aspx?CurrentLink=" + currentLink);
                        /*
                        str.Append("<script language='javascript'>");
                        str.Append("alert('Data inconsistency found in the page, the page will be refreshed.');");
                        str.Append("window.location.href = \"MaterialReq.aspx?CoyID=" + coy_id + "&CurrentLink=" + currentLink);
                        str.Append("</script>");
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                        return;
                        */

                    }

                }
                else
                {
                    Response.Redirect("MaterialReq.aspx?CurrentLink=" + currentLink);
                    /*
                    str.Append("<script language='javascript'>");
                    str.Append("alert('Data inconsistency found in the page, the page will be refreshed.');");
                    str.Append("window.location.href = \"MaterialReq.aspx?CoyID=" + coy_id + "&CurrentLink=" + currentLink);
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                    return;
                    */

                }   


                if(ddlPurchaser.Visible)
                    Session.Add("Purchaser", ddlPurchaser.SelectedValue.ToString());

                
                if (chkSales.Checked)
                    upConfirmedSalesInformation.Visible = true;
                else
                    upConfirmedSalesInformation.Visible = false;

                if (userRole == "Product Team")
                {
                    upVendorInformation.Visible = true;
                }
                else if (userRole == "Purchasing")
                {
                    upVendorInformation.Visible = true;
                }
                else
                {
                    if (Request.Params["MRNo"] != null && Request.Params["MRNo"].ToString() != "")
                    {
                        if (hidPMUserId.Value == loginUserOrAlternateParty.ToString() || hidPH.Value == loginUserOrAlternateParty.ToString() || hidPH2.Value == loginUserOrAlternateParty.ToString() || hidPH3.Value == loginUserOrAlternateParty.ToString())
                        {
                            upVendorInformation.Visible = true;                          
                        }
                        else
                        {
                            if (ddlSource.SelectedValue == "Local")
                            {
                                upVendorInformation.Visible = true;                               
                            }
                            else
                            {
                                upVendorInformation.Visible = false;                                
                            }  
                        }
                    }
                    else
                    {
                        if (coy.MRScheme.ToString() == "Product")
                        {
                            if (ddlSource.SelectedValue == "Local")
                                upVendorInformation.Visible = true;
                            else
                                upVendorInformation.Visible = false;
                        }
                        else
                        {
                            upVendorInformation.Visible = true;
                        }

                    }         
                    
                }
               

                
            }

            LoadApproveReject();

            if (btnSave.Visible)
                btnSave.Focus();
            
           

           

            string javaScript =
             @"<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>
               <script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/Common.js""></script>
               <script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/importing.js""></script>
               
                <script language=""javascript"" type=""text/javascript"">
                    
                    
                    function jsOpenOperationalReport( url )
                    {
	                    jsWinOpen2( url, 795, 580, 'yes');
                    }

                    function viewDetail(CoyId, ProductCode, UserId)
	                {		                    
		                var url = ""PurchaseHistory.aspx?CoyID="" + CoyId + ""&ProductCode="" + 
		                            ProductCode+ ""&UserId=""+UserId; 
		                	
		                window.open(url,"""",""width="" + 600 + "",height="" + 600 +"",resizable=yes,status=yes,menubar=no,scrollbars=no"");			
		                return false;
	                }	
                                        

                    function escapeText(lnk)
                    {

                        var idText = lnk.id.substring(0, lnk.id.length - 9) + 'txtNewName';
                        document.getElementById(idText).value = escape(document.getElementById(idText).value); 

                    }

                    var txtNewAccountCode_id;                    
                    var txtNewAccountName_id;   
                	
                    var txtNewProductCode_id;                    
                    var txtNewProductDescription_id;
                    var hidWeightedCost_id;
                    var ddlNewUOM_id;
                    var lblListPrice_id;
                    var lblMinSellingPrice_id;
                    var txtNewQuantity_id;
                    var txtNewPackageProductCode_id;
                    var txtNewUnitPrice_id;    
                    var txtNewPMUSERID_id;
                    var txtNewPHUSERID_id;                                          
                    var txtNewPH2USERID_id; 
                    var accountType;
                    var txtNewVendor_id
                    var txtNewAttnTo_id;
                    var txtNewTel_id;
                    var txtNewFax_id;
                    var txtNewEmail_id;

                    function SearchAccount(ctl)
                    {	
                        txtNewAccountCode_id = ctl.id.replace('lnkFindAccount', 'txtNewAccountCode');
                        txtNewAccountName_id = ctl.id.replace('lnkFindAccount', 'txtNewAccountName');   
                        accountType = 'C';                     
                        		
	                    var url = 'AccountSearch.aspx'; 
                        detailWindow = window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");	                       
	                    return false;
                    }	

                    function SearchSupplier(ctl)
                    {	
                        accountType = 'S'; 
                        txtNewVendor_id = ctl.id.replace('lnkFindAccount', 'txtNewVendor');  
                        txtNewAttnTo_id = ctl.id.replace('lnkFindAccount', 'txtNewAttnTo');  
                        txtNewTel_id = ctl.id.replace('lnkFindAccount', 'txtNewTel');  
                        txtNewFax_id = ctl.id.replace('lnkFindAccount', 'txtNewFax');  
                        txtNewEmail_id = ctl.id.replace('lnkFindAccount', 'txtNewEmail');                       
                        		
	                    var url = 'AccountSearch.aspx?AccountType=S'; 
                        detailWindow = window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");	                       
	                    return false;
                    }

                    function openPopUp(url)
                    {
                        window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");	
                    }
                    
                    function SetSelectedAccountCode(accountCode,AccountName,Address1,Address2,Address3,Address4,AttentionTo,MobilePhone,OfficePhone,Fax,Email,SalesPersonID)
                    {
                        if(accountCode != null)
                        {	
                            if(accountType == 'C')
                            {	
                                document.getElementById(txtNewAccountCode_id).value = accountCode.replace(/^\s+|\s+$/g, '');
                                document.getElementById(txtNewAccountName_id).value = AccountName.replace(/^\s+|\s+$/g, '');	
                            }	
                            else 
                            {
                                document.getElementById(txtNewVendor_id).value = AccountName.replace(/^\s+|\s+$/g, '');
                                document.getElementById(txtNewAttnTo_id).value = AttentionTo.replace(/^\s+|\s+$/g, '');
                                document.getElementById(txtNewTel_id).value = OfficePhone.replace(/^\s+|\s+$/g, '');
                                document.getElementById(txtNewFax_id).value = Fax.replace(/^\s+|\s+$/g, '');
                                document.getElementById(txtNewEmail_id).value = Email.replace(/^\s+|\s+$/g, '');            
                            }	
                                                     		
                        }
                    }  
                       
                  
                    function SearchProduct(ctl,a)
                    {	
                        txtNewProductCode_id = ctl.id.replace('lnkFindProduct', a);
                                                
                        var url = '';
                        
                        if (document.getElementById('ctl00_ContentPlaceHolderMain_hidPM') == null)
                        { 
                            url = 'ProductSearch.aspx?AccountCode=xxx';	
                                             
                        }
                        else {
                            var PM = document.getElementById('ctl00_ContentPlaceHolderMain_ddlPM').value;
                            var PH = document.getElementById('ctl00_ContentPlaceHolderMain_ddlPH').value; 
                            var PH3 = document.getElementById('ctl00_ContentPlaceHolderMain_hidPH3').value; 
                            url = 'ProductSearch.aspx?AccountCode=xxx&PM='+PM+'&PH='+PH+'&PH3='+PH3;
                        }                   
                        
                        window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");	                       
	                    return false;
                    }	

                    function SetSelectedProductCode(productCode)
                    {
                        if(productCode != null)
                        {	                           			
	                        document.getElementById(txtNewProductCode_id).value = productCode.replace(/^\s+|\s+$/g, '');  
                            if (document.getElementById(txtNewProductCode_id).value != """")
                            {	
                                if (document.all)			
                                {	
                                    //IE	                                    
                                    document.getElementById(txtNewProductCode_id).fireEvent('onchange');
                                }
                                else
                                {
                                    //FireFox
                                    var e = document.createEvent('HTMLEvents');
                                    e.initEvent('change', false, false);
                                    document.getElementById(txtNewProductCode_id).dispatchEvent(e);
                                }
                            }	

                        }
                    }

                    
                    var txtNewPackageProductCode_id;
                    function SearchPackage(ctl)
                    {	
                        txtNewPackageProductCode_id = ctl.id.replace('lnkFindPackage', 'txtNewPackageProductCode');
	                    var url = 'ProductSearch.aspx?Package=TRUE'; 
                        detailWindow = window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
                        //detailWindow.moveTo(100,0); 	
	                    return false;
                    }

                    function btnActionClick() 
                    {            
                        var behavior = $find('btnOkPopupBehavior');         
                        if (behavior)         
                        {                 
                            behavior.show();         
                        }      
                    } 

                    
                    function validateEmail(elementValue){ 
                            var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
                            return emailPattern.test(elementValue);    

                    }        


                    

                    function btnAddProductClick(ctl) 
                    {     
                        
                        
                        var behavior = $find('btnOkPopupBehavior');         
                        if (behavior)         
                        {    
                            txtNewProductCode_id = ctl.id.replace('lnkCreate', 'txtNewProductCode');
                            txtNewProductDescription_id = ctl.id.replace('lnkCreate', 'txtNewProductDescription');                   
                            ddlNewUOM_id = ctl.id.replace('lnkCreate', 'ddlNewUOM');
                            txtNewQuantity_id = ctl.id.replace('lnkCreate', 'txtNewQuantity');
                            txtNewUnitPrice_id = ctl.id.replace('lnkCreate', 'txtNewUnitPrice');                   
                                                 			
	                        if ((document.getElementById(txtNewProductCode_id).value.trim() != """") && 
                                (document.getElementById(txtNewProductDescription_id).value.trim() != """") &&
                                (document.getElementById(ddlNewUOM_id).value.trim() != ""--"") &&
                                (document.getElementById(txtNewQuantity_id).value.trim() != """") &&
                                (document.getElementById(txtNewUnitPrice_id).value.trim() != """"))
                                {       
                                behavior.show();      
                            }
                               
            
                                    
                        } 
                            
                    } 
            
                    function btnAddPackageClick(ctl) 
                    {     
                        
                        
                        var behavior = $find('btnOkPopupBehavior');         
                        if (behavior)         
                        {    
                            txtNewPackageProductCode_id = ctl.id.replace('lnkCreate', 'txtNewPackageProductCode');
                            txtNewProductDescription_id = ctl.id.replace('lnkCreate', 'txtNewProductDescription');                   
                            ddlNewUOM_id = ctl.id.replace('lnkCreate', 'ddlNewUOM');
                            txtNewQuantity_id = ctl.id.replace('lnkCreate', 'txtNewQuantity');                                            
                                                 			
	                        if ((document.getElementById(txtNewPackageProductCode_id).value.trim() != """") && 
                                (document.getElementById(txtNewProductDescription_id).value.trim() != """") &&
                                (document.getElementById(ddlNewUOM_id).value.trim() != ""--"") &&
                                (document.getElementById(txtNewQuantity_id).value.trim() != """")){       
                                behavior.show();      
                            }
                               
            
                                    
                        } 
                            
                    } 



                    function SetSelectedPackageID(packageID)
                    {
	                    if(packageID != null)
	                    {
		                    document.getElementById(txtNewPackageProductCode_id).value = packageID.replace(/^\s+|\s+$/g, '');
                            if (document.getElementById(txtNewPackageProductCode_id).value != """")
			                {								
				                if (document.all)			
                                {	
                                    //IE	
                                   		
                                    document.getElementById(txtNewPackageProductCode_id).fireEvent('onchange');
                                }
                                else
                                {
                                    //FireFox
                                    var e = document.createEvent('HTMLEvents');
                                    e.initEvent('change', false, false);
                                    document.getElementById(txtNewPackageProductCode_id).dispatchEvent(e);
                                }
			                }			
	                    }
                    }	
                   
                    function EditDescription(detailNo)
                    {			
                        var mrNo = document.getElementById('" + this.lblMRNo.ClientID + @"').firstChild.nodeValue.replace(/^\s+|\s+$/g, '');		                       
	                    var url = 'AddEditMRDetailDescription.aspx?MRNo=' + mrNo + '&DetailNo=' + detailNo; 
                        detailWindow = window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
                        //detailWindow.moveTo(100,0); 	
	                    return false;
                    }
                
                    function reloadPage()
                    {
                        window.location.href = window.location.href;
                    }

                    
                    
                </script>
                ";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Anchor", "window.location.href = '#bottom';");
            

        }
                       

        private void LoadReportDDL()
        {
            LogSession session = base.GetSessionInfo();
            ddlReport.Items.Clear();

            if (this.lblMRNo.Text.Trim() != "")
            {
                if ((hidPMUserId.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidPH.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidPH2.Value.ToString() == loginUserOrAlternateParty.ToString() || (userRole == "Purchasing")))
                {
                    ddlReport.Items.Add(new ListItem("Material Requisition", "MaterialRequisitionWithLetterHead_" + session.CompanyId.ToString()));

                }
                else
                {
                    if (ddlSource.SelectedValue == "Local")
                    {
                        ddlReport.Items.Add(new ListItem("Material Requisition", "MaterialRequisitionWithLetterHead_" + session.CompanyId.ToString()));

                    }
                    else
                    {
                        ddlReport.Items.Add(new ListItem("Material Requisition", "MaterialRequisitionWithLetterHeadEmptyVendor_" + session.CompanyId.ToString()));

                    }
                }
                
                if (coy.MRScheme.ToString() != "Product" && ddlStatus.SelectedValue.ToString() == "A")
                    ddlReport.Items.Add(new ListItem("Purchase Order", "PurchaseOrder_" + session.CompanyId.ToString()));
            }            
            
        }

        private void ReloadSetting()
        {            
                ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "reloadSetting()", true);
        }

        private void SetAccess(string status, string source)
        {
            LogSession session = base.GetSessionInfo();

            if (this.lblMRNo.Text.Trim() != "")
            {
                
                ddlStatus.Enabled = false;
                string undoUserId = "0";
                MRFormApproval lastUndoCalcellation = new MRFormApprovalActivity().RetrieveLastUndoCancellationFormApproval(session.CompanyId, this.lblMRNo.Text.Trim());
                if (lastUndoCalcellation != null)
                    undoUserId = lastUndoCalcellation.ApproverUserID.ToString();
                
                if (userRole == "Product Team")
                {
                    //Product Team access

                    this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  

                    if (!((hidPMUserId.Value == loginUserOrAlternateParty.ToString()) || (hidPH.Value == loginUserOrAlternateParty.ToString()) || (hidPH2.Value == loginUserOrAlternateParty.ToString()) || (hidPH3.Value == loginUserOrAlternateParty.ToString())))
                    {                      
                        ddlSource.Enabled = false;
                        rbAir.Attributes.Add("disabled", "disabled");
                        rbSea.Attributes.Add("disabled", "disabled");
                        rbCourier.Attributes.Add("disabled", "disabled");
                        rbLand.Attributes.Add("disabled", "disabled");

                        if (source != "Local")
                        {
                            upVendorInformation.Visible = false;                            
                        }
                    }
                    
                    if (!(((hidPMUserId.Value == loginUserOrAlternateParty.ToString()) || (hidPH.Value == loginUserOrAlternateParty.ToString()) || (hidPH2.Value == loginUserOrAlternateParty.ToString())) && (viewStockInformationAccess != null)))
                        chkStock.Enabled = false;                   
                                       
                    switch (status)
                    {
                        case "A":
                            btnSave.Visible = false;  // hide save button                
                            this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                            this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                           
                            this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                            DisabledControls();                           
                            break;   
                        case "D":
                            if (!((hidRequestor.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidCreator.Value.ToString() == session.UserId.ToString()) || (undoUserId == loginUserOrAlternateParty.ToString())))
                            {
                                btnSave.Visible = false; // Can change only for requestor/creator/ last undo cancellation user.
                                this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                                this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                                
                                this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                                this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                                DisabledControls();
                            }
                            TabPanel4.Visible = false; // hide Delivery Tab           
                            break;
                        case "F":
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                            break;
                        case "R":
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                            break;
                        case "P":
                            btnSave.Visible = false;  // hide save button                
                            this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                            this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                            
                            this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                            DisabledControls();
                            break;   
                        case "X":
                            btnSave.Visible = false;  // hide save button                
                            this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                            this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                            
                            this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                            DisabledControls();
                            btnUndoCancel.Visible = true;
                            break;
                        case "C":
                            btnSave.Visible = false;  // hide save button                
                            this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                            this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                           
                            this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery
                            DisabledControls();
                            break;
                        default:                          
                            break;
                    }
                }
                else if (userRole == "Purchasing")
                {   
                    //Purchasing access
                    if (viewStockInformationAccess == null)
                        chkStock.Enabled = false;                  
                    
                    switch (status)
                    {
                        case "A":
                            if (chkWorkshop.Checked || (coy.MRScheme.ToString() != "Product") || (coy.MRScheme.ToString() == "Product" && purchaserApproval == "1"))
                                ddlStatus.Enabled = true; // Allow to change the status from ¡®Approved¡¯ to ¡®Closed¡¯ for those MR that don¡¯t have PO.
                            break;
                        case "D":
                            if (!((hidRequestor.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidCreator.Value.ToString() == session.UserId.ToString()) || (undoUserId == loginUserOrAlternateParty.ToString())))
                            {
                                btnSave.Visible = false; // Can change only for requestor/creator/ last undo cancellation user.
                                this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                                this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                                
                                this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                                this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                                DisabledControls();                                 
                            }
                            TabPanel4.Visible = false; // hide Delivery Tab           
                            break;
                        case "F":
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery                              
                            break;
                        case "R":
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                            break;
                        case "X":
                            btnSave.Visible = false;  // hide save button                
                            this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                            this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                           
                            this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                            DisabledControls();
                            btnUndoCancel.Visible = true;
                            break;
                        case "C":
                                          
                            this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                            this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                            
                            this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery
                            
                            if (purchaserApproval == "1")
                            {
                                ddlStatus.Enabled = true;
                                btnSave.Visible = true;  

                            }
                            else
                            {
                                btnSave.Visible = false;  // hide save button  
                                DisabledControls();
                            }
                            break;
                            
                        default: 
                            break;
                    }
                }
                else
                {   // Requestor or CS access                       
                    if (viewStockInformationAccess != null && ((hidRequestor.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidCreator.Value.ToString() == session.UserId.ToString())))
                        chkStock.Enabled = true;
                    else
                        chkStock.Enabled = false;

                    ddlSource.Enabled = false;

                    if ((source != "Local") && (coy.MRScheme.ToString() == "Product") && (!((hidPMUserId.Value == loginUserOrAlternateParty.ToString()) || (hidPH.Value == loginUserOrAlternateParty.ToString()) || (hidPH2.Value == loginUserOrAlternateParty.ToString()) || (hidPH3.Value == loginUserOrAlternateParty.ToString()))))
                        upVendorInformation.Visible = false;
                   
                    this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  

                    switch (status)
                    {
                        case "A":
                            btnSave.Visible = false;  // hide save button                
                            this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                            this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                            
                            this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                            DisabledControls();
                            break;
                        case "D":
                            if (!((hidRequestor.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidCreator.Value.ToString() == session.UserId.ToString()) || (undoUserId == loginUserOrAlternateParty.ToString())))
                            {
                                btnSave.Visible = false; // Can change only for requestor/creator/ last undo cancellation user.
                                this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                                this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                                
                                this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                                this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                                DisabledControls();
                            }
                            else
                            {
                                ddlSource.Enabled = true;
                                if(coy.MRScheme.ToString() == "Product")
                                    hidRole.Value = "N";
                            }
                            TabPanel4.Visible = false; // hide Delivery Tab           
                            break;
                        case "F":
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery                             
                            break;
                        case "R":
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                            break;
                        case "P":
                            btnSave.Visible = false;  // hide save button                
                            this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                            this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                           
                            this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery     
                            DisabledControls();
                            break;
                        case "X":
                            btnSave.Visible = false;  // hide save button                
                            this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                            this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                           
                            this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery  
                            DisabledControls();
                            btnUndoCancel.Visible = true;
                            break;
                        case "C":
                            btnSave.Visible = false;  // hide save button                
                            this.dgConfirmedSalesData.Columns[7].Visible = false; // hide action column for Confirmed Sales
                            this.dgVendorData.Columns[6].Visible = false; // hide action column for vendor                            
                            this.dgProductData.Columns[10].Visible = false; // hide action column for Product
                            this.dgDeliveryData.Columns[8].Visible = false; // hide action column for Delivery
                            DisabledControls();
                            break;
                        default:                                                
                            break;
                    }                    
                }
            }
            else
            {
                TabPanel4.Visible = false; // hide Delivery Tab                
                ddlStatus.Visible = false; // hide status dropdow 
                ddlPM.Visible = false; // hide PM dropdown
                ddlPH.Visible = false; // hide PH dropdown 
                ddlPH3.Visible = false; // hide PH dropdown 

                if (viewStockInformationAccess  == null)
                    chkStock.Enabled = false;

                if (coy.MRScheme.ToString() == "Product")
                {
                    hidRole.Value = "N";
                    if ((ddlSource.SelectedValue != "Local") && (userRole != "Purchasing") && (userRole != "Product Team"))
                    {
                        upVendorInformation.Visible = false;
                    }
                    else
                        upVendorInformation.Visible = true;

                }
                else
                {
                    upVendorInformation.Visible = true;

                }

                
            }
        }

        private void RetriveAccess()
        {
            LogSession session = base.GetSessionInfo();            

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "MR",ref lstAlterParty);
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
                purchaserApproval = lstUserRole.Tables[0].Rows[0]["Approval"].ToString();
            }
   
          
            new GMSGeneralDALC().GetMRWareHouseByCoyID(session.CompanyId, ref lstWarehouse);
            if (coy.MRScheme.ToString() == "Product")
                viewPurchaseInformationAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 121);
            else
                viewPurchaseInformationAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 131);

            viewStockInformationAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 122);
            viewAllProductGroupPurchaseInformationAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 123);

            hidMRRoleName.Value = userRole;
            hidMRScheme.Value = coy.MRScheme.ToString();
            
        }

        private void LoadRequestorDDL()
        {
            LogSession session = base.GetSessionInfo();
            SystemDataActivity dalc = new SystemDataActivity();

            DataSet lstRequestor = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionRequestorByUserNumIDCoyID(session.CompanyId, loginUserOrAlternateParty, session.MRScheme ,ref lstRequestor);
            ddlRequestor.DataSource = lstRequestor;
            ddlRequestor.DataValueField = "UserNumId";
            ddlRequestor.DataTextField = "UserRealName";
            ddlRequestor.SelectedValue = loginUserOrAlternateParty.ToString();
            ddlRequestor.DataBind();


            IList<TaxType> cList2 = null;
            try
            {
                cList2 = dalc.RetrieveAllTaxTypeListByCompanyCode(session.CompanyId, false);
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }

            ddlTaxType.DataSource = cList2;
            ddlTaxType.DataValueField = "TaxTypeID";
            ddlTaxType.DataTextField = "TaxName";
            ddlTaxType.DataBind();
            ddlTaxType.SelectedValue = "G";

            try
            {
                TaxType rate = TaxType.RetrieveByKey(session.CompanyId, "G");
                if (rate != null)
                    this.txtTaxRate.Text = (rate.TaxRate.Value * 100).ToString("#0.00") + "%";
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }
            
        }

        private void LoadApproveReject()
        {
            LogSession session = base.GetSessionInfo();

            if (this.lblMRNo.Text.Trim() != "")
            {
                btnDuplicate.Visible = true;

                if (ddlStatus.SelectedValue.ToString() == "X")
                {
                    btnUndoCancel.Visible = true;
                    btnCancel.Visible = false;
                }
                else if ((ddlStatus.SelectedValue.ToString() == "D") || (ddlStatus.SelectedValue.ToString() == "F") || (ddlStatus.SelectedValue.ToString() == "R"))
                {
                    btnCancel.Visible = true;
                    btnUndoCancel.Visible = false;
                }
                else if ((userRole == "Purchasing") && (ddlStatus.SelectedValue.ToString() == "A"))
                {
                    btnCancel.Visible = true;
                    btnUndoCancel.Visible = false;
                }

                string undoUserId = "0";
                MRFormApproval lastUndoCalcellation = new MRFormApprovalActivity().RetrieveLastUndoCancellationFormApproval(session.CompanyId, this.lblMRNo.Text.Trim());
                if (lastUndoCalcellation != null)
                    undoUserId = lastUndoCalcellation.ApproverUserID.ToString();

                if ((hidRequestor.Value == loginUserOrAlternateParty.ToString() || hidCreator.Value == session.UserId.ToString() || undoUserId == loginUserOrAlternateParty.ToString()) && ((ddlStatus.SelectedValue.ToString() == "D") || (ddlStatus.SelectedValue.ToString() == "R")))
                {
                    btnSubmitForApproval.Visible = true;
                    if ((hidPMUserId.Value.ToString() == "0") && (hidPH.Value.ToString() == "0") && (coy.MRScheme.ToString() == "Product"))                   
                        btnSubmitForApproval.Attributes.Add("onclick", "return confirm_submit_approval();");                   
                    else                    
                        btnSubmitForApproval.Attributes.Add("onclick", "return confirm_submit_approval_normal();");                                     
                }
                else
                {
                    btnSubmitForApproval.Visible = false;
                }

                MRFormApproval latest = new MRFormApprovalActivity().RetrieveLastestFormApproval(session.CompanyId, this.lblMRNo.Text.Trim());

                if (latest != null)
                {
                    MRFormApproval approve = new MRFormApprovalActivity().RetrieveFormApprovalByCoyIDByUser(session.CompanyId, this.lblMRNo.Text.Trim(), loginUserOrAlternateParty, latest.LevelID);

                    if (approve != null)
                    {
                        levelID = approve.LevelID;
                        
                        if ((approve.IsLastLevel == "Y") && (approve.Status == "P"))
                        {
                            MRActivity mrActivity = new MRActivity();
                            IList<GMSCore.Entity.MRPurchaser> lstMRPurchaser = null;
                            lstMRPurchaser = mrActivity.RetrieveMRPurchaserByCoyID(session.CompanyId);
                            ddlPurchaser.DataSource = lstMRPurchaser;
                            if (Session["Purchaser"] != null)
                                ddlPurchaser.SelectedValue = Session["Purchaser"].ToString();
                            ddlPurchaser.DataBind();
                            
                            ddlPurchaser.Visible = true;
                            lblAssignedPurchaser.Visible = true;
                        }

                        
                        if (approve.IsCurrentLevel == "Y" && approve.Status == "P")
                        {
                            btnApprove.Visible = true;
                            btnReject.Visible = true;
                            btnCancel.Visible = true;
                            ModalPopupExtender1.TargetControlID = "btnReject";
                            ModalPopupExtender3.TargetControlID = "btnCancel";
                            btnHighLevelApprove.Visible = false;
                            btnHighLevelReject.Visible = false;
                            btnHighLevelCancel.Visible = false;
                            btnApprove.Attributes.Add("onclick", "return confirm_approve();");

                        }
                        else if ((approve.IsCurrentLevel == "N") && (approve.Status == "P"))
                        {
                            btnApprove.Visible = false;
                            btnReject.Visible = false;
                            btnCancel.Visible = false;
                            ModalPopupExtender1.TargetControlID = "btnHighLevelReject";
                            ModalPopupExtender3.TargetControlID = "btnHighLevelCancel";
                            btnHighLevelApprove.Visible = true;
                            btnHighLevelReject.Visible = true;
                            btnHighLevelCancel.Visible = true;                           
                        }
                    }
                    else
                    {
                        MRFormApproval requestedApproval = new MRFormApprovalActivity().RetrieveRequestedFormApprovalByCoyIDByUser(session.CompanyId, this.lblMRNo.Text.Trim(), loginUserOrAlternateParty, latest.LevelID);
                        if(requestedApproval != null)
                            levelID = (short)requestedApproval.LevelID;
                        btnApprove.Visible = false;
                        btnReject.Visible = false;
                        btnHighLevelApprove.Visible = false;
                        btnHighLevelReject.Visible = false;
                    }
                }
                else
                {
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                    btnHighLevelApprove.Visible = false;
                    btnHighLevelReject.Visible = false;
                }
            }            
                
        }

        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            if (this.lblMRNo.Text.Trim() != "")
            { 
                
                string mrNo = lblMRNo.Text.Trim();
                DataSet ds = new DataSet();
                new GMSGeneralDALC().GetMaterialRequisitionByMRNo(session.CompanyId, mrNo, ref ds);

                hidRequestor.Value = ds.Tables[0].Rows[0]["requestor"].ToString();
                hidCreator.Value = ds.Tables[0].Rows[0]["createdby"].ToString();

                if (((hidRequestor.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidCreator.Value.ToString() == session.UserId.ToString())) && (ds.Tables[0].Rows[0]["StatusID"].ToString() == "D"))
                {  
                    ddlRequestor.SelectedValue = ds.Tables[0].Rows[0]["requestor"].ToString();  
                   
                }
                else
                {
                    ddlRequestor.Visible = false;
                    lblRequestor.Visible = true;
                    lblRequestor.Text = ds.Tables[0].Rows[0]["requestorname"].ToString();
                }           
                      

                DataSet ProductTeam = new DataSet();
                new GMSGeneralDALC().GetNonZeroProductByMRNo(session.CompanyId, this.lblMRNo.Text.Trim(), ref ProductTeam);
                if (coy.MRScheme == "Product" && (ProductTeam != null) && (ProductTeam.Tables[0].Rows.Count == 0) && ((ds.Tables[0].Rows[0]["StatusID"].ToString() == "D") || (ds.Tables[0].Rows[0]["StatusID"].ToString() == "R") || (ds.Tables[0].Rows[0]["StatusID"].ToString() == "F")))
                {                
                    DataSet lstRequestorPMPH = new DataSet();
                    new GMSGeneralDALC().GetMaterialRequisitionPMPHRequestorByCoyID(session.CompanyId, loginUserOrAlternateParty, session.MRScheme,ref lstRequestorPMPH);

                   
                        ddlPM.DataSource = lstRequestorPMPH.Tables[1];
                        ddlPM.DataValueField = "UserNumId";
                        ddlPM.DataTextField = "UserRealName";
                        ddlPM.SelectedValue = ds.Tables[0].Rows[0]["PMUserId"].ToString();
                        ddlPM.DataBind();
                   

                    
                        ddlPH.DataSource = lstRequestorPMPH.Tables[2];
                        ddlPH.DataValueField = "UserNumId";
                        ddlPH.DataTextField = "UserRealName";
                        ddlPH.SelectedValue = ds.Tables[0].Rows[0]["PHUserId"].ToString();
                        ddlPH.DataBind();                  

                    if (lstRequestorPMPH.Tables[3].Rows.Count > 1)
                    {
                        ddlPH3.DataSource = lstRequestorPMPH.Tables[3];
                        ddlPH3.DataValueField = "UserNumId";
                        ddlPH3.DataTextField = "UserRealName";
                        ddlPH3.SelectedValue = ds.Tables[0].Rows[0]["PH3UserId"].ToString();
                        ddlPH3.DataBind();
                    }
                    else
                    {
                        ddlPH3.Visible = false;
                        lblPH3.Visible = true;
                        if (ds.Tables[0].Rows[0]["ph3name"].ToString() != "")
                            lblPH3.Text = ds.Tables[0].Rows[0]["ph3name"].ToString();
                        else
                            lblPH3.Text = "NIL";
                    }
                }
                else
                {
                    ddlPM.Visible = false;
                    lblPM.Visible = true;
                    if (ds.Tables[0].Rows[0]["pmname"].ToString() != "")
                        lblPM.Text = ds.Tables[0].Rows[0]["pmname"].ToString();
                    else
                        lblPM.Text = "NIL";

                    ddlPH.Visible = false;
                    lblPH.Visible = true;
                    if (ds.Tables[0].Rows[0]["phname"].ToString() != "")
                        lblPH.Text = ds.Tables[0].Rows[0]["phname"].ToString();
                    else
                        lblPH.Text = "NIL";

                    ddlPH3.Visible = false;
                    lblPH3.Visible = true;
                    if (ds.Tables[0].Rows[0]["ph3name"].ToString() != "")
                        lblPH3.Text = ds.Tables[0].Rows[0]["ph3name"].ToString();
                    else
                        lblPH3.Text = "NIL";
                }




                // Load Purchase Information Data
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {                
                    if (ds.Tables[0].Rows[0]["SourceID"].ToString() == "Local")
                    {
                        ddlSource.SelectedValue = "Local";
                    }
                    else if (ds.Tables[0].Rows[0]["SourceID"].ToString() == "Overseas")
                    {
                        ddlSource.SelectedValue = "Overseas";    
                    }

                    rbAir.Checked = false;
                    rbSea.Checked = false;
                    rbCourier.Checked = false;
                    rbLand.Checked = false;

                    if((ds.Tables[0].Rows[0]["freightmode"].ToString() == "Air"))
                        rbAir.Checked = true;
                    else if((ds.Tables[0].Rows[0]["freightmode"].ToString() == "Sea"))
                        rbSea.Checked = true;
                    else if ((ds.Tables[0].Rows[0]["freightmode"].ToString() == "Courier"))
                        rbCourier.Checked = true;
                    else if ((ds.Tables[0].Rows[0]["freightmode"].ToString() == "Land"))
                        rbLand.Checked = true;
                                  
                    hidPMUserId.Value = ds.Tables[0].Rows[0]["PMUserId"].ToString();
                    hidPH.Value = ds.Tables[0].Rows[0]["PHUserId"].ToString();
                    hidPH2.Value = ds.Tables[0].Rows[0]["PH2UserId"].ToString();
                    hidPH3.Value = ds.Tables[0].Rows[0]["PH3UserId"].ToString();                   

                    if (ds.Tables[0].Rows[0]["isconsole"].ToString() == "True")
                    {
                        rbIsConsole.Checked = true;
                        rbIsNotConsole.Checked = false;
                        txtConsoleDate.Text = string.Format("{0:dd/MM/yyyy}", ds.Tables[0].Rows[0]["ConsoleDate"]);
                    }
                    else
                    {
                        rbIsConsole.Checked = false;
                        rbIsNotConsole.Checked = true;
                        txtConsoleDate.Text = "";
                    }

                     
                 
                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    IList<GMSCore.Entity.MRStatus> lstMRStatus = null;
                    if ((userRole == "Purchasing") && ((ds.Tables[0].Rows[0]["StatusID"].ToString() == "A") || (ds.Tables[0].Rows[0]["StatusID"].ToString() == "C")) && (((coy.MRScheme.ToString() != "Product") || ((ds.Tables[0].Rows[0]["IntendedUsage"].ToString().Contains("Workshop")))) || (coy.MRScheme.ToString() == "Product" && purchaserApproval == "1")))
                    {   
                        // Allow purchasing to close the MR for those MR that dont have PO. 
                        ddlStatus.Items.Clear();
                        ddlStatus.Items.Add(new ListItem("Approved", "A"));
                        ddlStatus.Items.Add(new ListItem("In Progress", "P"));
                        ddlStatus.Items.Add(new ListItem("Closed", "C"));
                        ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["StatusID"].ToString();
                        ddlStatus.Enabled = true;
                    }
                    else
                    {
                        lstMRStatus = sDataActivity.RetrieveAllMRStatus();
                        ddlStatus.DataSource = lstMRStatus;
                        ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["StatusID"].ToString();
                        ddlStatus.DataBind();
                        ddlStatus.Enabled = false;
                    }  

                    lblPurchaser.Text = ds.Tables[0].Rows[0]["Purchaser"].ToString();  
                    this.txtMRDate.Text = string.Format("{0:dd/MM/yyyy}", ds.Tables[0].Rows[0]["MRDate"]);
                    
                    string[] usages = ds.Tables[0].Rows[0]["IntendedUsage"].ToString().Split(',');                    
                    foreach (string usage in usages)
                    {
                        if (usage == "Stock")
                            chkStock.Checked = true;
                        else if (usage == "Sales")
                        {
                            chkSales.Checked = true;
                            upConfirmedSalesInformation.Visible = true;
                        }
                        else if (usage == "Repair & Maintenance")
                        {    
                            chkRepair.Checked = true;
                            if (ds.Tables[0].Rows[0]["StatusID"].ToString() == "N")
                                ddlStatus.Enabled = true;
                        }
                        else if (usage == "Asset")
                        {   
                            chkAsset.Checked = true;
                            txtGLCode.Text = ds.Tables[0].Rows[0]["GLCode"].ToString();
                        }
                        else if (usage == "Sample")
                        {
                            chkSample.Checked = true;
                        }
                        else if (usage == "Workshop")
                        {
                            chkWorkshop.Checked = true;
                        }
                        else if (usage == "Project")
                        {
                            chkProject.Checked = true;
                        }

                    }
                    ddlTaxType.SelectedValue = ds.Tables[0].Rows[0]["TaxTypeID"].ToString();
                    txtTaxRate.Text = GMSUtil.ToDouble(ds.Tables[0].Rows[0]["TaxRate"].ToString()) * 100 + "%";
                    txtDiscount.Text =  GMSUtil.ToDouble(ds.Tables[0].Rows[0]["Discount"].ToString()).ToString();
                    txtBudgetCode.Text = ds.Tables[0].Rows[0]["BudgetCode"].ToString();
                    txtProjectNo.Text = ds.Tables[0].Rows[0]["ProjectNo"].ToString();
                    txtRefNo.Text = ds.Tables[0].Rows[0]["RefNo"].ToString();
                    txtOtherPurchaseReason.Text = ds.Tables[0].Rows[0]["OrderReason"].ToString();                                      
                    txtRemarksByRequestor.Text = ds.Tables[0].Rows[0]["RequestorRemarks"].ToString();
                    txtVendorRemarks.Text = ds.Tables[0].Rows[0]["VendorRemarks"].ToString();
                    txtPurchasingRemarks.Text = ds.Tables[0].Rows[0]["PurchasingRemarks"].ToString();
                    lblCancelReason.Text = ds.Tables[0].Rows[0]["CancelledReason"].ToString();
                    if (ds.Tables[0].Rows[0]["createdbyname"].ToString() != "")
                        this.lblCreatedBy.Text = "Created By " + ds.Tables[0].Rows[0]["createdbyname"].ToString() + " on " + ds.Tables[0].Rows[0]["createddate"].ToString();
                    if (ds.Tables[0].Rows[0]["modifiedbyname"].ToString() != "")
                        this.lblModifiedBy.Text = "Modified By " + ds.Tables[0].Rows[0]["modifiedbyname"].ToString() + " on " + ds.Tables[0].Rows[0]["modifieddate"].ToString();
         
                    SetAccess(ds.Tables[0].Rows[0]["StatusID"].ToString(), ds.Tables[0].Rows[0]["SourceID"].ToString()); // set access based on status
                }
                
                // Load Confirm Sales Information Data
                if (ds != null && ds.Tables.Count > 0)
                {
                    ViewState["SortDirection"] = "ASC";
                    ViewState["SortField"] = "CustomerAccountCode";
                    int startIndex = ((dgConfirmedSalesData.CurrentPageIndex + 1) * this.dgConfirmedSalesData.PageSize) - (this.dgConfirmedSalesData.PageSize - 1);
                    int endIndex = (dgConfirmedSalesData.CurrentPageIndex + 1) * this.dgConfirmedSalesData.PageSize;
                    DataView dv = ds.Tables[1].DefaultView;
                    dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
                    if (ds != null && ds.Tables[1].Rows.Count > 0)
                    {
                        if (endIndex < ds.Tables[1].Rows.Count)
                            this.lblPurchaseSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                endIndex.ToString() + " " + "of" + " " + ds.Tables[1].Rows.Count.ToString();
                        else
                            this.lblPurchaseSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                ds.Tables[1].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[1].Rows.Count.ToString();
                        this.lblPurchaseSummary.Visible = true;

                     }
                    else
                    {
                        this.lblPurchaseSummary.Text = "No records.";
                        this.lblPurchaseSummary.Visible = true;
                    }
                    this.dgConfirmedSalesData.DataSource = dv;
                    this.dgConfirmedSalesData.DataBind();
                    this.dgConfirmedSalesData.Visible = true;
                }               

                // Load Vendor Information Data
                if (ds != null && ds.Tables.Count > 0)
                {
                    ViewState["SortDirection"] = "ASC";
                    ViewState["SortField"] = "VendorName";
                    int startIndex = ((dgVendorData.CurrentPageIndex + 1) * this.dgVendorData.PageSize) - (this.dgVendorData.PageSize - 1);
                    int endIndex = (dgVendorData.CurrentPageIndex + 1) * this.dgVendorData.PageSize;
                    DataView dv = ds.Tables[2].DefaultView;
                    dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
                    if (ds != null && ds.Tables[2].Rows.Count > 0)
                    {
                        if (endIndex < ds.Tables[2].Rows.Count)
                            this.lblVendorSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                endIndex.ToString() + " " + "of" + " " + ds.Tables[2].Rows.Count.ToString();
                        else
                            this.lblVendorSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                ds.Tables[2].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[2].Rows.Count.ToString();
                        this.lblVendorSummary.Visible = true;
                    }
                    else
                    {
                        this.lblVendorSummary.Text = "No records.";
                        this.lblVendorSummary.Visible = true;
                    }
                    this.dgVendorData.DataSource = dv;
                    this.dgVendorData.DataBind();
                    this.dgVendorData.Visible = true;
                }

                MRActivity maActivity = new MRActivity();
                IList<MRVendor> lstAutoInsertedMRVendor = null;
                lstAutoInsertedMRVendor = maActivity.RetrieveAutoInsertedVendorByMRNo(GMSUtil.ToShort(session.CompanyId), this.lblMRNo.Text.Trim());
                if ((lstAutoInsertedMRVendor.Count > 0) && (hidPMUserId.Value.ToString() == loginUserOrAlternateParty.ToString() || hidPH.Value.ToString() == loginUserOrAlternateParty.ToString() || hidPH3.Value.ToString() == loginUserOrAlternateParty.ToString() || (userRole == "Purchasing")))
                    btnConfirmVendor.Visible = true;
                                
                // Load Product Information Data
                if (ds != null && ds.Tables.Count > 0)
                {
                    ViewState["SortDirection"] = "ASC";
                    ViewState["SortField"] = "DetailNo";
                    int startIndex = ((dgProductData.CurrentPageIndex + 1) * this.dgProductData.PageSize) - (this.dgProductData.PageSize - 1);
                    int endIndex = (dgProductData.CurrentPageIndex + 1) * this.dgProductData.PageSize;
                    DataView dv = ds.Tables[3].DefaultView;
                    dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
                    if (ds != null && ds.Tables[3].Rows.Count > 0)
                    {
                        if (endIndex < ds.Tables[3].Rows.Count)
                            this.lblProductSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                endIndex.ToString() + " " + "of" + " " + ds.Tables[3].Rows.Count.ToString();
                        else
                            this.lblProductSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                ds.Tables[3].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[3].Rows.Count.ToString();

                        this.lblProductSummary.Visible = true;
                    }
                    else
                    {
                        this.lblProductSummary.Text = "No records.";
                        this.lblProductSummary.Visible = true;
                    }
                    this.dgProductData.DataSource = dv;
                    this.dgProductData.DataBind();
                    this.dgProductData.Visible = true;
                }

                // Load Product Delivery Data
                if (ds != null && ds.Tables.Count > 0)
                {
                    ViewState["SortDirection"] = "ASC";
                    ViewState["SortField"] = "PONo";
                    int startIndex = ((dgDeliveryData.CurrentPageIndex + 1) * this.dgDeliveryData.PageSize) - (this.dgDeliveryData.PageSize - 1);
                    int endIndex = (dgDeliveryData.CurrentPageIndex + 1) * this.dgDeliveryData.PageSize;
                    DataView dv = ds.Tables[4].DefaultView;
                    dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
                    if (ds != null && ds.Tables[4].Rows.Count > 0)
                    {
                        if (endIndex < ds.Tables[4].Rows.Count)
                            this.lblDeliverySummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                endIndex.ToString() + " " + "of" + " " + ds.Tables[4].Rows.Count.ToString();
                        else
                            this.lblDeliverySummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                ds.Tables[4].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[4].Rows.Count.ToString();
                        this.lblDeliverySummary.Visible = true;
                    }
                    else
                    {
                        this.lblDeliverySummary.Text = "No records.";
                        this.lblDeliverySummary.Visible = true;
                    }
                    this.dgDeliveryData.DataSource = dv;
                    this.dgDeliveryData.DataBind();
                    this.dgDeliveryData.Visible = true;
                }

                //Load approver data
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[5].Rows.Count > 0)
                {
                    ViewState["SortDirection"] = "ASC";
                    ViewState["SortField"] = "LevelID";
                    int startIndex = ((dgMRFormApproval.CurrentPageIndex + 1) * this.dgMRFormApproval.PageSize) - (this.dgMRFormApproval.PageSize - 1);
                    int endIndex = (dgMRFormApproval.CurrentPageIndex + 1) * this.dgMRFormApproval.PageSize;
                    DataView dv = ds.Tables[5].DefaultView;
                    dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
                    this.dgMRFormApproval.DataSource = dv;                    
                    this.dgMRFormApproval.DataBind();
                    this.dgMRFormApproval.Visible = true;
                    this.routingTable.Visible = true;                    
                }

                // Load Attachment Information Data
                if (ds != null && ds.Tables.Count > 0)
                {
                    ViewState["SortDirection"] = "ASC";
                    ViewState["SortField"] = "FileID";
                    int startIndex = ((dgAttachmentData.CurrentPageIndex + 1) * this.dgAttachmentData.PageSize) - (this.dgAttachmentData.PageSize - 1);
                    int endIndex = (dgAttachmentData.CurrentPageIndex + 1) * this.dgAttachmentData.PageSize;
                    DataView dv = ds.Tables[6].DefaultView;
                    dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
                    if (ds != null && ds.Tables[6].Rows.Count > 0)
                    {
                        if (endIndex < ds.Tables[6].Rows.Count)
                            this.lblAttachmentSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                endIndex.ToString() + " " + "of" + " " + ds.Tables[6].Rows.Count.ToString();
                        else
                            this.lblAttachmentSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                ds.Tables[6].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[6].Rows.Count.ToString();
                        this.lblAttachmentSummary.Visible = true;
                    }
                    else
                    {
                        this.lblAttachmentSummary.Text = "No records.";
                        this.lblAttachmentSummary.Visible = true;
                    }
                    this.dgAttachmentData.DataSource = dv;
                    this.dgAttachmentData.DataBind();
                    this.dgAttachmentData.Visible = true;
                }                

            }
            else
            {
                SetAccess("", "");
                if (coy.MRScheme.ToString() != "Product")
                {
                    DataSet lstRequestorPMPH = new DataSet();
                    new GMSGeneralDALC().GetMaterialRequisitionPMPHRequestorByCoyID(session.CompanyId, loginUserOrAlternateParty, session.MRScheme ,ref lstRequestorPMPH);

                    if (lstRequestorPMPH.Tables[1].Rows[0]["UserRealName"].ToString() != "")
                    {
                        lblPM.Text = lstRequestorPMPH.Tables[1].Rows[0]["UserRealName"].ToString();
                        hidPMUserId.Value = lstRequestorPMPH.Tables[1].Rows[0]["UserNumId"].ToString();
                    }
                    else
                        lblPM.Text = "NIL";

                    if (lstRequestorPMPH.Tables[2].Rows[0]["UserRealName"].ToString() != "")
                    {
                        lblPH.Text = lstRequestorPMPH.Tables[2].Rows[0]["UserRealName"].ToString();
                        hidPH.Value = lstRequestorPMPH.Tables[2].Rows[0]["UserNumId"].ToString();
                    }
                    else
                        lblPH.Text = "NIL";

                    if (lstRequestorPMPH.Tables[3].Rows[0]["UserRealName"].ToString() != "")
                    {
                        lblPH3.Text = lstRequestorPMPH.Tables[3].Rows[0]["UserRealName"].ToString();
                        hidPH3.Value = lstRequestorPMPH.Tables[3].Rows[0]["UserNumId"].ToString();
                    }
                    else
                        lblPH3.Text = "NIL";

                    lblPM.Visible = true;
                    lblPH.Visible = true;
                    lblPH3.Visible = true;

                   
                }

                if (Session["ConfirmedSalesInformation"] != null)
                {
                    workTable = (DataTable)Session["ConfirmedSalesInformation"];
                    this.dgConfirmedSalesData.DataSource = workTable;
                    this.dgConfirmedSalesData.DataBind();
                    this.dgConfirmedSalesData.Visible = true;
                }
                else
                {
                    DataSet ds = null;
                    ds = new DataSet();
                    ds.Tables.Add("SalesInformation");
                    this.dgConfirmedSalesData.DataSource = ds.Tables["SalesInformation"];
                    this.dgConfirmedSalesData.DataBind();
                    this.dgConfirmedSalesData.Visible = true;

                }

                if (Session["VendorInformation"] != null)
                {
                    workTable = (DataTable)Session["VendorInformation"];
                    this.dgVendorData.DataSource = workTable;
                    this.dgVendorData.DataBind();
                }
                else
                {
                    DataSet ds = null;
                    ds = new DataSet();
                    ds.Tables.Add("VendorInformation");
                    this.dgVendorData.DataSource = ds.Tables["VendorInformation"];
                    this.dgVendorData.DataBind();
                }

                if (Session["ProductDetail"] != null)
                {
                    workTable = (DataTable)Session["ProductDetail"];
                    this.dgProductData.DataSource = workTable;
                    this.dgProductData.DataBind();
                }
                else
                {
                    DataSet ds = null;
                    ds = new DataSet();
                    ds.Tables.Add("ProductDetail");
                    this.dgProductData.DataSource = ds.Tables["ProductDetail"];
                    this.dgProductData.DataBind();
                }

                if (Session["Attachment"] != null)
                {
                    workTable = (DataTable)Session["Attachment"];
                    this.dgAttachmentData.DataSource = workTable;
                    this.dgAttachmentData.DataBind();
                }
                else
                {
                    DataSet ds = null;
                    ds = new DataSet();
                    ds.Tables.Add("Attachment");
                    this.dgAttachmentData.DataSource = ds.Tables["Attachment"];
                    this.dgAttachmentData.DataBind();
                } 
            }
            CalculateTotal();
            
        }

        private void Resubmission(string section, string alertMessage, string anchor)
        {
            LogSession session = base.GetSessionInfo();
            StringBuilder str = new StringBuilder();

            string activeTab = "0";
            if (section == "Confirmed Sales updated.")
                activeTab = "0";
            else if (section == "Vendor updated.")
                activeTab = "1";
            else if (section == "Product updated.")
                activeTab = "2";
            else if (section == "Delivery updated.")
                activeTab = "3";

            DataSet ds = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(session.CompanyId, lblMRNo.Text.Trim(), ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if ((ddlStatus.SelectedValue.ToString() != ds.Tables[0].Rows[0]["StatusID"].ToString()) && ds.Tables[0].Rows[0]["StatusID"].ToString() != "P")
                {
                    str.Append("<script language='javascript'>");
                    str.Append("alert('Data inconsistency found in the page, the page will be refreshed.');");
                    str.Append("window.location.href = \"NewMaterialRequisition.aspx?ActiveTab=" + activeTab + "&CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + anchor + "\"");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                    
                    throw new ArgumentException("Data inconsistency found in the page, the page will be refreshed.");
               
                }
               
            }   


            /*
            Principle of Five:
            ---------------------
            R - Header updated.
            R - Confirmed Sales updated.
            U - Vendor updated. If the MR is Rejected and user updated the vendor information, insert R instead of U.  
            R - Product updated.  
            U- Delivery updated.               
            */
            string status = "";
            string action = "U";
            if ((ddlStatus.SelectedValue.ToString() == "A") || (ddlStatus.SelectedValue.ToString() == "P"))
                action = "U";
            else if ((ddlStatus.SelectedValue.ToString() == "R") || (ddlStatus.SelectedValue.ToString() == "F"))
            {
                if ((section == "Delivery updated.") || ((section == "Vendor updated.") && (ddlStatus.SelectedValue.ToString() == "F")))
                    action = "U";
                else
                    action = "R";
            }            

            if ((ddlStatus.SelectedValue.ToString() == "A") || (ddlStatus.SelectedValue.ToString() == "R") || (ddlStatus.SelectedValue.ToString() == "F") || (ddlStatus.SelectedValue.ToString() == "P"))
            {
                status = new ApprovalActivity().ReSubmitMR(session.CompanyId, loginUserOrAlternateParty, this.lblMRNo.Text.Trim(), GMSUtil.ToShort(hidPMUserId.Value), GMSUtil.ToShort(hidPH.Value), GMSUtil.ToShort(hidPH3.Value), levelID, section, action, false, session.UserId);
                                              
            }

            
            

           
            str.Append("<script language='javascript'>");
            str.Append("alert('" + alertMessage + "');");
            str.Append("window.location.href = \"NewMaterialRequisition.aspx?ActiveTab=" + activeTab + "&CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + anchor + "\"");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);

        }

        private void DisabledControls()
        {           
            ddlSource.Enabled = false;
            rbAir.Enabled = false;
            rbSea.Enabled = false;
            rbCourier.Enabled = false;
            rbLand.Enabled = false;            
            txtRemarksByRequestor.ReadOnly = true;
            chkStock.Attributes.Add("disabled", "disabled");
            chkRepair.Attributes.Add("disabled", "disabled");
            chkAsset.Attributes.Add("disabled", "disabled");
            chkSales.Attributes.Add("disabled", "disabled");
            chkSample.Attributes.Add("disabled", "disabled");
            txtOtherPurchaseReason.ReadOnly = true;
            txtMRDate.ReadOnly = true;
            txtVendorRemarks.ReadOnly = true;
            txtRemarksByRequestor.ReadOnly = true;
            txtOtherPurchaseReason.ReadOnly = true;
            //txtCancellationReason.ReadOnly = true;
            rbIsConsole.Enabled = false;
            rbIsNotConsole.Enabled = false;
            btnConfirmVendor.Visible = false;
            
        }

        protected void dgProductData_Command(Object sender, DataGridCommandEventArgs e)
        {                    

            LogSession session = base.GetSessionInfo();             
            switch (((LinkButton)e.CommandSource).CommandName)
            {
                case "Create":
                    TextBox txtNewProductCode = (TextBox)e.Item.FindControl("txtNewProductCode");
                    TextBox txtNewProductDescription = (TextBox)e.Item.FindControl("txtNewProductDescription");
                    DropDownList ddlNewUOM = (DropDownList)e.Item.FindControl("ddlNewUOM");                    
                    HiddenField hidOnHand = (HiddenField)e.Item.FindControl("hidOnHand");
                    HiddenField hidOnOrder = (HiddenField)e.Item.FindControl("hidOnOrder");
                    HiddenField hidOnPO = (HiddenField)e.Item.FindControl("hidOnPO");
                    TextBox txtNewSalesQty = (TextBox)e.Item.FindControl("txtNewSalesQty");
                    TextBox txtNewStkQty = (TextBox)e.Item.FindControl("txtNewStkQty");
                    TextBox txtOrderQty = (TextBox)e.Item.FindControl("txtOrderQty");
                    TextBox txtNewUnitPrice = (TextBox)e.Item.FindControl("txtNewUnitPrice");
                    TextBox txtNewUnitPurchasePrice = (TextBox)e.Item.FindControl("txtNewUnitPurchasePrice");
                    Label lblNewTotalPrice = (Label)e.Item.FindControl("lblNewTotalPrice");                    
                    HtmlInputHidden hidPMUSERID = (HtmlInputHidden)e.Item.FindControl("hidPMUSERID");
                    HtmlInputHidden hidPHUSERID = (HtmlInputHidden)e.Item.FindControl("hidPHUSERID");
                    HtmlInputHidden hidPH2USERID = (HtmlInputHidden)e.Item.FindControl("hidPH2USERID");
                    HtmlInputHidden hidPH3USERID = (HtmlInputHidden)e.Item.FindControl("hidPH3USERID");
                    DropDownList ddlNewPPCurrency = (DropDownList)e.Item.FindControl("ddlNewPPCurrency");
                    DropDownList ddlNewSellingCurrency = (DropDownList)e.Item.FindControl("ddlNewSellingCurrency");
                    TextBox txtReason = (TextBox)e.Item.FindControl("txtReason");
                    Label lblReason = (Label)e.Item.FindControl("lblReason");

                    double purchasePrice;
                    string purchaseCurrency = "";
                    if (viewPurchaseInformationAccess != null || viewAllProductGroupPurchaseInformationAccess != null)
                    {
                        purchaseCurrency = ddlNewPPCurrency.SelectedValue.ToString();
                        purchasePrice = GMSUtil.ToDouble(txtNewUnitPurchasePrice.Text.ToString());                        
                    }
                    else
                    {
                        purchasePrice = 0;
                    }

                    DataSet dsPurchaseAndSellingPrice = new DataSet();
                    new GMSGeneralDALC().ConvertSellingPriceAndPurchasePrice(session.CompanyId, GMSUtil.ToDouble(txtNewUnitPrice.Text.ToString()), purchasePrice, ddlNewSellingCurrency.SelectedValue.ToString(), purchaseCurrency, ref dsPurchaseAndSellingPrice);

                    if (chkSales.Checked)
                    {

                        if (dsPurchaseAndSellingPrice != null && dsPurchaseAndSellingPrice.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsPurchaseAndSellingPrice.Tables[0].Rows.Count; i++)
                            {
                                if (GMSUtil.ToDouble(dsPurchaseAndSellingPrice.Tables[0].Rows[i]["sellingPrice"].ToString()) < GMSUtil.ToDouble(dsPurchaseAndSellingPrice.Tables[0].Rows[i]["purchasePrice"].ToString()) && txtReason.Text.Trim() == "")
                                {
                                    ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('Selling Price < Purchase Price! Please Enter Reason for Selling Price Less Than Purchase Price!')", true);
                                    txtReason.Visible = true;
                                    lblReason.Visible = true;
                                    return;
                                }
                                else if (GMSUtil.ToDouble(dsPurchaseAndSellingPrice.Tables[0].Rows[i]["sellingPrice"].ToString()) > GMSUtil.ToDouble(dsPurchaseAndSellingPrice.Tables[0].Rows[i]["purchasePrice"].ToString()) && txtReason.Text.Trim() != "")
                                {
                                    txtReason.Text = "";
                                }
                            }
                        }
                    }


                    string newProdCode = "";
                                        
                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        if (txtNewProductCode.Text.ToString() == "00000000000")
                        {
                            TextBox txtNewProdCode = (TextBox)e.Item.FindControl("txtNewProdCode");
                            newProdCode = txtNewProdCode.Text.Trim();
                        }
                        else
                        {
                            if (coy.MRScheme.ToString() == "Product")
                            {
                                string strPM = hidPMUserId.Value.ToString();
                                if ((strPM != hidPMUSERID.Value.ToString()) || (hidPH.Value.ToString() != hidPHUSERID.Value.ToString()) || (hidPH3.Value.ToString() != hidPH3USERID.Value.ToString()))
                                {
                                    ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('This product cannot be added into this MR because it belongs to other Product Manager and Product Head.')", true);
                                    return;
                                }
                            }

                        }                
                       

                            GMSCore.Entity.MRDetail md = new GMSCore.Entity.MRDetail();
                            md.CoyID = session.CompanyId;
                            md.MRNo = lblMRNo.Text;
                            md.ProdCode = txtNewProductCode.Text.ToString().Trim();
                            md.NewProdCode = newProdCode;
                            md.ProdName = txtNewProductDescription.Text.ToString();
                            md.UOM = ddlNewUOM.SelectedValue.ToString();
                            md.OnHandQty = GMSUtil.ToDouble(hidOnHand.Value.ToString());
                            md.OnOrderQty = GMSUtil.ToDouble(hidOnOrder.Value.ToString()); ;
                            md.OnPOQty = GMSUtil.ToDouble(hidOnPO.Value.ToString());
                            md.ConfirmedOrderQty = GMSUtil.ToDouble(txtNewSalesQty.Text.ToString());
                            md.ForStockingQty = GMSUtil.ToDouble(txtNewStkQty.Text.ToString());
                            md.OrderQty = GMSUtil.ToDouble(txtOrderQty.Text.ToString());
                            md.UnitSellingPrice = GMSUtil.ToDouble(txtNewUnitPrice.Text.ToString());

                            if (txtNewUnitPurchasePrice.Enabled)
                            {                                
                                md.PurchaseCurrency = ddlNewPPCurrency.SelectedValue.ToString();
                                md.UnitPurchasePrice = GMSUtil.ToDouble(txtNewUnitPurchasePrice.Text.ToString());
                            }
                            else
                            {
                                md.UnitPurchasePrice = 0;
                            }
                            md.SellingCurrency = ddlNewSellingCurrency.SelectedValue.ToString();
                            md.Reason = txtReason.Text.ToString();
                            md.CreatedBy = loginUserOrAlternateParty;
                            md.CreatedDate = DateTime.Now;
                           
                            using (TransactionScope tran = new TransactionScope())
                            {
                                md.Save();
                                Resubmission("Product updated.", "Record created successfully!", "#ProductAddNew");
                                tran.Complete();
                            }
                     }                   
                    else
                    {
                        DataTable workTable = null;
                        if (Session["ProductDetail"] != null)
                        {
                            workTable = (DataTable)Session["ProductDetail"];
                        }
                        else
                        {
                            workTable = new DataTable("ProductDetail");
                            DataColumn column = new DataColumn();
                            column.ColumnName = "DetailNo";
                            column.DataType = System.Type.GetType("System.Int32");
                            column.AutoIncrement = true;
                            column.AutoIncrementSeed = 1;
                            column.AutoIncrementStep = 1;
                            workTable.Columns.Add(column);
                            workTable.Columns.Add("CoyID", typeof(String));
                            workTable.Columns.Add("ProdCode", typeof(String));
                            workTable.Columns.Add("NewProdCode", typeof(String));
                            workTable.Columns.Add("ProdName", typeof(String));
                            workTable.Columns.Add("UOM", typeof(String));
                            workTable.Columns.Add("OnHandQty", typeof(Double));
                            workTable.Columns.Add("OnOrderQty", typeof(Double));
                            workTable.Columns.Add("OnPOQty", typeof(Double));
                            workTable.Columns.Add("ConfirmedOrderQty", typeof(Double));
                            workTable.Columns.Add("ForStockingQty", typeof(Double));
                            workTable.Columns.Add("OrderQty", typeof(Double));
                            workTable.Columns.Add("SellingCurrency", typeof(String));
                            workTable.Columns.Add("UnitSellingPrice", typeof(Double));
                            workTable.Columns.Add("PurchaseCurrency", typeof(String));
                            workTable.Columns.Add("UnitPurchasePrice", typeof(Double));
                            workTable.Columns.Add("TotalPP", typeof(Double));                            
                            workTable.Columns.Add("PMUserID", typeof(Int32));
                            workTable.Columns.Add("PHUserID", typeof(Int32));
                            workTable.Columns.Add("PH2UserID", typeof(Int32));
                            workTable.Columns.Add("PH3UserID", typeof(Int32));
                            workTable.Columns.Add("Reason", typeof(String));
                            workTable.Columns.Add("ProductGroupCode", typeof(String));
                            workTable.Columns.Add("ProductGroupVendorCode", typeof(String));
                            workTable.Columns.Add("KeyToSplit", typeof(String));

                        }

                        DataRow workRow = workTable.NewRow();
                        TextBox txtNewProdCode = (TextBox)e.Item.FindControl("txtNewProdCode");
                        DropDownList ddlNewPM = (DropDownList)e.Item.FindControl("ddlNewPM");
                        DropDownList ddlNewPH = (DropDownList)e.Item.FindControl("ddlNewPH");
                        DropDownList ddlNewPH3 = (DropDownList)e.Item.FindControl("ddlNewPH3");

                        if (coy.MRScheme.ToString() == "Product")
                        {

                            DataSet chkExistPMPH = new DataSet();
                            new GMSGeneralDALC().GetExistPMPHByPHPMCoyID(session.CompanyId, GMSUtil.ToShort(ddlNewPM.SelectedValue.ToString()), GMSUtil.ToShort(ddlNewPH.SelectedValue.ToString()), GMSUtil.ToShort(ddlNewPH3.SelectedValue.ToString()), ref chkExistPMPH);
                            if (chkExistPMPH != null && chkExistPMPH.Tables[0].Rows.Count > 0)
                            {
                                workRow["PMUserID"] = Convert.ToInt32(ddlNewPM.SelectedValue.ToString());
                                workRow["PHUserID"] = Convert.ToInt32(ddlNewPH.SelectedValue.ToString());
                                workRow["PH2UserID"] = Convert.ToInt32("0".ToString());
                                workRow["PH3UserID"] = Convert.ToInt32(ddlNewPH3.SelectedValue.ToString());
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('Selected PM not tided with selected PH in system!')", true);
                                return;
                            }
                        }
                        else
                        {
                            workRow["PMUserID"] = 0;
                            workRow["PHUserID"] = 0;
                            workRow["PH2UserID"] = 0;
                            workRow["PH3UserID"] = 0;

                        }
                                         

                        if (txtNewProdCode.Visible)
                            workRow["NewProdCode"] = txtNewProdCode.Text.ToString().Trim();

                        string tproductCode = "";

                        if (txtNewProductCode.Text.ToString().Trim() == "00000000000")
                            tproductCode = txtNewProdCode.Text.ToString().Trim();
                        else
                            tproductCode = txtNewProductCode.Text.ToString().Trim();

                        DataSet dsProductGrpVendor = new DataSet();
                        new GMSGeneralDALC().GetMRSupplierCodeByCoyIDAndProductCode(session.CompanyId, tproductCode, ref dsProductGrpVendor);
                        if (dsProductGrpVendor != null && dsProductGrpVendor.Tables[0].Rows.Count > 0)
                        {
                            workRow["ProductGroupCode"] = dsProductGrpVendor.Tables[0].Rows[0]["productgroupcode"].ToString();
                            workRow["ProductGroupVendorCode"] = dsProductGrpVendor.Tables[0].Rows[0]["AccountCode"].ToString();

                        }
                        else
                        {
                            workRow["ProductGroupCode"] = "NONE";
                            workRow["ProductGroupVendorCode"] = "NONE";

                        }
                        
                        workRow["CoyID"] = session.CompanyId.ToString();
                        workRow["ProdCode"] = txtNewProductCode.Text.ToString().Trim();                       
                        workRow["ProdName"] = txtNewProductDescription.Text.ToString();
                        workRow["UOM"] = ddlNewUOM.SelectedValue.ToString();
                        workRow["OnHandQty"] = GMSUtil.ToDouble(hidOnHand.Value.ToString());
                        workRow["OnOrderQty"] = GMSUtil.ToDouble(hidOnOrder.Value.ToString());
                        workRow["OnPOQty"] = GMSUtil.ToDouble(hidOnPO.Value.ToString());
                        workRow["ConfirmedOrderQty"] = GMSUtil.ToDouble(txtNewSalesQty.Text.ToString());
                        workRow["ForStockingQty"] = GMSUtil.ToDouble(txtNewStkQty.Text.ToString());
                        workRow["OrderQty"] = GMSUtil.ToDouble(txtOrderQty.Text.ToString());
                        workRow["SellingCurrency"] = ddlNewSellingCurrency.SelectedValue.ToString();
                        workRow["UnitSellingPrice"] = GMSUtil.ToDouble(txtNewUnitPrice.Text.ToString());
                        workRow["Reason"] = txtReason.Text.ToString();
                        workRow["KeyToSplit"] = workRow["PMUserID"] + "-" + workRow["PHUserID"] + "-" + workRow["PH3UserID"] + "-" + workRow["ProductGroupVendorCode"];
                                                                         
                        if (txtNewUnitPurchasePrice.Enabled)
                        {
                            workRow["PurchaseCurrency"] = ddlNewPPCurrency.SelectedValue.ToString();
                            workRow["UnitPurchasePrice"] = GMSUtil.ToDouble(txtNewUnitPurchasePrice.Text.ToString());
                            workRow["TotalPP"] = GMSUtil.ToDouble(lblNewTotalPrice.Text.ToString());
                        }
                        else
                        {
                            workRow["UnitPurchasePrice"] = 0;
                            workRow["TotalPP"] = GMSUtil.ToDouble("0".ToString());  
                        } 
                        workTable.Rows.Add(workRow);
                        Session["ProductDetail"] = workTable;
                    }
                     LoadData();
                     Tabs.ActiveTabIndex = 2;                    
                    break;               
                case "ViewPrice":                   
                    string prodCode = e.CommandArgument.ToString();
                    ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "viewDetail("+ session.CompanyId +",'" + prodCode + "'," + loginUserOrAlternateParty + ")", true);
                    break;
                default:
                    // Do nothing.
                    break;
            }
        }        

        protected void dgProductData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgProductData.EditItemIndex = e.Item.ItemIndex;           
            LoadData();                      
        }

        protected void dgProductData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgProductData.EditItemIndex = -1;
            LoadData();
        }

        protected void dgProductData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Products"));
                return;
            }
            HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidProductID");
            if (hidID != null)
            {
                TextBox txtEditProductCode = (TextBox)e.Item.FindControl("txtEditProductCode");
                TextBox txtEditProductDescription = (TextBox)e.Item.FindControl("txtEditProductDescription");
                DropDownList ddlEditUOM = (DropDownList)e.Item.FindControl("ddlEditUOM");                
                HiddenField hidEditOnHand = (HiddenField)e.Item.FindControl("hidEditOnHand");
                HiddenField hidEditOnOrder = (HiddenField)e.Item.FindControl("hidEditOnOrder");
                HiddenField hidEditOnPO = (HiddenField)e.Item.FindControl("hidEditOnPO");
                TextBox txtEditQuantity = (TextBox)e.Item.FindControl("txtEditQuantity");
                TextBox txtEditStkQty = (TextBox)e.Item.FindControl("txtEditStkQty");
                TextBox txtEditOrderQty = (TextBox)e.Item.FindControl("txtEditOrderQty");
                DropDownList ddlEditSellingCurrency = (DropDownList)e.Item.FindControl("ddlEditSellingCurrency");
                TextBox txtEditUnitPrice = (TextBox)e.Item.FindControl("txtEditUnitPrice");
                DropDownList ddlEditPPCurrency = (DropDownList)e.Item.FindControl("ddlEditPPCurrency");
                TextBox txtEditUnitPurchasePrice = (TextBox)e.Item.FindControl("txtEditUnitPurchasePrice");
                Label lblEditTotalPP = (Label)e.Item.FindControl("lblEditTotalPP");
                Label lblEditReason = (Label)e.Item.FindControl("lblEditReason");
                TextBox txtEditReason = (TextBox)e.Item.FindControl("txtEditReason");
                HiddenField hidEditPMUSERID = (HiddenField)e.Item.FindControl("hidEditPMUSERID");
                HiddenField hidEditPHUSERID = (HiddenField)e.Item.FindControl("hidEditPHUSERID");
                HiddenField hidEditPH2USERID = (HiddenField)e.Item.FindControl("hidEditPH2USERID");
                HiddenField hidEditPH3USERID = (HiddenField)e.Item.FindControl("hidEditPH3USERID");         
                double purchasePrice;
                string purchaseCurrency = "";
                if (viewPurchaseInformationAccess != null || viewAllProductGroupPurchaseInformationAccess != null)
                {                    
                    purchaseCurrency = ddlEditPPCurrency.SelectedValue.ToString();
                    purchasePrice = GMSUtil.ToDouble(txtEditUnitPurchasePrice.Text.ToString());
                }
                else
                {
                    purchasePrice = 0;
                }

                if (chkSales.Checked)
                {

                    DataSet dsPurchaseAndSellingPrice = new DataSet();
                    new GMSGeneralDALC().ConvertSellingPriceAndPurchasePrice(session.CompanyId, GMSUtil.ToDouble(txtEditUnitPrice.Text.ToString()), purchasePrice, ddlEditSellingCurrency.SelectedValue.ToString(), purchaseCurrency, ref dsPurchaseAndSellingPrice);
                    if (dsPurchaseAndSellingPrice != null && dsPurchaseAndSellingPrice.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsPurchaseAndSellingPrice.Tables[0].Rows.Count; i++)
                        {
                            if (GMSUtil.ToDouble(dsPurchaseAndSellingPrice.Tables[0].Rows[i]["sellingPrice"].ToString()) < GMSUtil.ToDouble(dsPurchaseAndSellingPrice.Tables[0].Rows[i]["purchasePrice"].ToString()) && txtEditReason.Text.Trim() == "")
                            {
                                ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('Selling Price < Purchase Price! Please Enter Reason for Selling Price Less Than Purchase Price!')", true);
                                txtEditReason.Visible = true;
                                lblEditReason.Visible = true;
                                return;
                            }
                            else if (GMSUtil.ToDouble(dsPurchaseAndSellingPrice.Tables[0].Rows[i]["sellingPrice"].ToString()) > GMSUtil.ToDouble(dsPurchaseAndSellingPrice.Tables[0].Rows[i]["purchasePrice"].ToString()) && txtEditReason.Text.Trim() != "")
                            {
                                txtEditReason.Text = "";
                            }
                        }
                    }
                }
                try
                {
                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        GMSCore.Entity.MRDetail md = new MRActivity().RetrieveMRProductByID(session.CompanyId, hidID.Value.Trim());
                        if (md == null)
                        {
                            base.JScriptAlertMsg("This product record cannot be found in database.");
                            return;
                        }

                        if (txtEditProductCode.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("Please enter Product Code.");
                            return;
                        }

                        if (txtEditProductCode.Text.ToString() != "00000000000")
                        {
                            string strPM = hidPMUserId.Value.ToString();
                            if (strPM != hidEditPMUSERID.Value.ToString())
                            {
                                ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('This product cannot be added into this MR because it belongs to other Product Manager.')", true);
                                return;
                            }
                        }
                        else
                        {
                            TextBox txtEditNewProdCode = (TextBox)e.Item.FindControl("txtEditNewProdCode");
                            md.NewProdCode = txtEditNewProdCode.Text.ToString().Trim();                            
                        }                      
                        md.ProdCode = txtEditProductCode.Text.ToString().Trim();
                        md.ProdName = txtEditProductDescription.Text.ToString();
                        md.UOM = ddlEditUOM.SelectedValue.ToString();
                        md.OnHandQty = GMSUtil.ToDouble(hidEditOnHand.Value.ToString());
                        md.OnOrderQty = GMSUtil.ToDouble(hidEditOnOrder.Value.ToString());
                        md.OnPOQty = GMSUtil.ToDouble(hidEditOnPO.Value.ToString());
                        md.ConfirmedOrderQty = GMSUtil.ToDouble(txtEditQuantity.Text.ToString());
                        md.ForStockingQty = GMSUtil.ToDouble(txtEditStkQty.Text.ToString());
                        md.OrderQty = GMSUtil.ToDouble(txtEditOrderQty.Text.ToString());
                        md.UnitSellingPrice = GMSUtil.ToDouble(txtEditUnitPrice.Text.ToString());
                        md.PurchaseCurrency = ddlEditPPCurrency.SelectedValue.ToString();
                        md.Reason = txtEditReason.Text.ToString();
                        if (viewPurchaseInformationAccess != null || viewAllProductGroupPurchaseInformationAccess != null)
                        {                            
                            md.SellingCurrency = ddlEditSellingCurrency.SelectedValue.ToString();
                            md.UnitPurchasePrice = GMSUtil.ToDouble(txtEditUnitPurchasePrice.Text.ToString());
                        }
                        else
                        {
                            md.UnitPurchasePrice = 0;                          
                        }                                               
                        md.ModifiedBy = loginUserOrAlternateParty;
                        md.ModifiedDate = DateTime.Now;

                        using (TransactionScope tran = new TransactionScope())
                        {
                            md.Save();                            
                            this.dgProductData.EditItemIndex = -1;
                            Resubmission("Product updated.", "Record modified successfully!", "");
                            tran.Complete();
                        }
                    }
                    else
                    {
                        //Update Session Data
                        workTable = (DataTable)Session["ProductDetail"];
                        if (txtEditProductCode.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("Please enter Product Code.");
                            return;
                        }

                        for (int i = workTable.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = workTable.Rows[i];
                            if (dr["DetailNo"].ToString() == hidID.Value)
                            {
                                TextBox txtEditNewProdCode = (TextBox)e.Item.FindControl("txtEditNewProdCode");

                                dr["ProdCode"] = txtEditProductCode.Text.ToString();
                                if (coy.MRScheme.ToString() == "Product")
                                {
                                    if (txtEditProductCode.Text.ToString() == "00000000000" || hidEditPMUSERID.Value.ToString() == "0" || hidEditPHUSERID.Value.ToString() == "0")
                                    {
                                        DropDownList ddlEditPM = (DropDownList)e.Item.FindControl("ddlEditPM");
                                        DropDownList ddlEditPH = (DropDownList)e.Item.FindControl("ddlEditPH");
                                        DropDownList ddlEditPH3 = (DropDownList)e.Item.FindControl("ddlEditPH3");

                                        dr["NewProdCode"] = txtEditNewProdCode.Text.ToString().Trim();
                                        if ((ddlEditPM.SelectedValue.ToString() == "0") && (ddlEditPH.SelectedValue.ToString() == "0"))
                                        {
                                            dr["PMUserID"] = Convert.ToInt32(ddlEditPM.SelectedValue.ToString());
                                            dr["PHUserID"] = Convert.ToInt32(ddlEditPH.SelectedValue.ToString());
                                            dr["PH2UserID"] = Convert.ToInt32("0".ToString());
                                            dr["PH3UserID"] = Convert.ToInt32(ddlEditPH3.SelectedValue.ToString());

                                        }
                                        else if ((ddlEditPM.SelectedValue.ToString() != "0") || (ddlEditPH.SelectedValue.ToString() != "0"))
                                        {

                                            DataSet chkExistPMPH = new DataSet();
                                            new GMSGeneralDALC().GetExistPMPHByPHPMCoyID(session.CompanyId, GMSUtil.ToShort(ddlEditPM.SelectedValue.ToString()), GMSUtil.ToShort(ddlEditPH.SelectedValue.ToString()), GMSUtil.ToShort(ddlEditPH3.SelectedValue.ToString()), ref chkExistPMPH);
                                            if (chkExistPMPH != null && chkExistPMPH.Tables[0].Rows.Count > 0)
                                            {
                                                dr["PMUserID"] = Convert.ToInt32(ddlEditPM.SelectedValue.ToString());
                                                dr["PHUserID"] = Convert.ToInt32(ddlEditPH.SelectedValue.ToString());
                                                dr["PH2UserID"] = Convert.ToInt32("0".ToString());
                                                dr["PH3UserID"] = Convert.ToInt32(ddlEditPH3.SelectedValue.ToString());
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('Selected PM not tided with selected PH in system!')", true);
                                                return;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        dr["PMUserID"] = Convert.ToInt32(hidEditPMUSERID.Value);
                                        dr["PHUserID"] = Convert.ToInt32(hidEditPHUSERID.Value);
                                        dr["PH2UserID"] = Convert.ToInt32(hidEditPH2USERID.Value);
                                        dr["PH3UserID"] = Convert.ToInt32(hidEditPH3USERID.Value);
                                    }
                                }
                                else
                                {
                                    dr["PMUserID"] = 0;
                                    dr["PHUserID"] = 0;
                                    dr["PH2UserID"] = 0;
                                    dr["PH3UserID"] = 0;

                                }

                                string tproductCode = "";

                                if (txtEditProductCode.Text.ToString().Trim() == "00000000000")
                                    tproductCode = txtEditNewProdCode.Text.ToString().Trim();
                                else
                                    tproductCode = txtEditProductCode.Text.ToString().Trim();

                                DataSet dsProductGrpVendor = new DataSet();
                                new GMSGeneralDALC().GetMRSupplierCodeByCoyIDAndProductCode(session.CompanyId, tproductCode, ref dsProductGrpVendor);
                                if (dsProductGrpVendor != null && dsProductGrpVendor.Tables[0].Rows.Count > 0)
                                {
                                    dr["ProductGroupCode"] = dsProductGrpVendor.Tables[0].Rows[0]["productgroupcode"].ToString();
                                    dr["ProductGroupVendorCode"] = dsProductGrpVendor.Tables[0].Rows[0]["AccountCode"].ToString();

                                }
                                else
                                {
                                    dr["ProductGroupCode"] = "NONE";
                                    dr["ProductGroupVendorCode"] = "NONE";

                                }


                                dr["ProdName"] = txtEditProductDescription.Text.ToString();
                                dr["UOM"] = ddlEditUOM.SelectedValue.ToString();
                                dr["OnHandQty"] = GMSUtil.ToDouble(hidEditOnHand.Value.ToString());
                                dr["OnOrderQty"] = GMSUtil.ToDouble(hidEditOnOrder.Value.ToString());
                                dr["OnPOQty"] = GMSUtil.ToDouble(hidEditOnPO.Value.ToString());
                                dr["ConfirmedOrderQty"] = GMSUtil.ToDouble(txtEditQuantity.Text.ToString());
                                dr["ForStockingQty"] = GMSUtil.ToDouble(txtEditStkQty.Text.ToString());
                                dr["OrderQty"] = GMSUtil.ToDouble(txtEditOrderQty.Text.ToString());
                                dr["UnitSellingPrice"] = GMSUtil.ToDouble(txtEditUnitPrice.Text.ToString());
                                //if ((userRole == "Product Team") || (userRole == "Purchasing"))        
                                if (txtEditUnitPurchasePrice.Enabled) 
                                    dr["UnitPurchasePrice"] = GMSUtil.ToDouble(txtEditUnitPurchasePrice.Text.ToString());
                                else
                                    dr["UnitPurchasePrice"] = 0;

                                dr["PurchaseCurrency"] = ddlEditPPCurrency.SelectedValue.ToString();  
                                dr["SellingCurrency"] = ddlEditSellingCurrency.SelectedValue.ToString();
                                                            
                                dr["Reason"] = txtEditReason.Text.ToString();
                                dr["KeyToSplit"] = dr["PMUserID"] + "-" + dr["PHUserID"] + "-" + dr["PH3UserID"] + "-" + dr["ProductGroupVendorCode"];
                                break;
                            }
                               
                        }
                        Session["ProductDetail"] = workTable;                        
                        this.dgProductData.EditItemIndex = -1;                        
                        ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('Record modified successfully!')", true);
                    }
                    
                    
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }

                LoadData();
            }
        }

        protected void dgProductData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Products"));
                    return;
                }

                HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidProductID");
                if (hidID != null)
                {
                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        try
                        {

                            MRActivity maActivity = new MRActivity();
                            IList<MRDetail> lstMRProduct = null;
                            lstMRProduct = maActivity.RetrieveMRProductByMRNo(GMSUtil.ToShort(session.CompanyId), lblMRNo.Text.Trim());
                            if (lstMRProduct.Count > 1)
                            {
                                using (TransactionScope tran = new TransactionScope())
                                {
                                    ResultType result = maActivity.DeleteMRProduct(hidID.Value, session);
                                    switch (result)
                                    {
                                        case ResultType.Ok:
                                            this.dgProductData.EditItemIndex = -1;
                                            this.dgProductData.CurrentPageIndex = 0;
                                            LoadData();
                                            Resubmission("Product updated.", "Record deleted successfully!", "");
                                            tran.Complete();
                                            break;
                                        default:
                                            this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                            return;
                                    }                                   
                                }
                            }
                            else
                            {                               
                                ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('This record cannot be deleted. This Material Requisition must have minimum of one Product record!')", true);
                            }
                        }
                        catch (SqlException exSql)
                        {
                            if (exSql.Number == 547)
                            {
                                this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                                LoadData();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);

                            return;
                        }
                    }
                    else
                    {
                        workTable = (DataTable)Session["ProductDetail"];
                        for (int i = workTable.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = workTable.Rows[i];
                            if (dr["DetailNo"].ToString() == hidID.Value)
                                dr.Delete();
                        }
                        Session["ProductDetail"] = workTable;
                        LoadData();
                    }                  
                }
            } 
        }

        protected void dgProductData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
           
            LogSession session = base.GetSessionInfo();           
            if (viewPurchaseInformationAccess != null || viewAllProductGroupPurchaseInformationAccess != null)
            {  
                if (this.lblMRNo.Text.Trim() != "")
                {
                    if ((hidPMUserId.Value == loginUserOrAlternateParty.ToString() || hidPH.Value == loginUserOrAlternateParty.ToString() || hidPH2.Value == loginUserOrAlternateParty.ToString() || hidPH3.Value == loginUserOrAlternateParty.ToString()) ||
                        (userRole == "Purchasing") || (coy.MRScheme.ToString() != "Product"))
                    {
                        CalculateTotal();
                        this.dgProductData.Columns[8].Visible = true;
                        this.dgProductData.Columns[9].Visible = true;
                        GrandTotalPurchase.Visible = true;
                    }
                    else
                    {
                        this.dgProductData.Columns[8].Visible = false;
                        this.dgProductData.Columns[9].Visible = false;
                        GrandTotalPurchase.Visible = false;
                    }
                }

            }
            else
            {

                this.dgProductData.Columns[8].Visible = false;
                this.dgProductData.Columns[9].Visible = false;
                GrandTotalPurchase.Visible = false;
            }

            

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                /*
                Label lblUnitPP = (Label)e.Item.FindControl("lblUnitPP");
                Label lblOrderQty = (Label)e.Item.FindControl("lblOrderQty");
                Label lblPurchaseCurrency = (Label)e.Item.FindControl("lblPurchaseCurrency");
                
                
                double unitPP = 0.00;
                double orderQty = 0.00;
                Double.TryParse(lblUnitPP.Text, out unitPP);
                Double.TryParse(lblOrderQty.Text, out orderQty);
                finalTotal = finalTotal + unitPP * orderQty;

                purchaseCurrency = lblPurchaseCurrency.Text;
                */


                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
                if (!this.dgProductData.Columns[10].Visible)
                    lnkEdit.Enabled = false;

                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

                if (lnkDelete != null)
                {
                    lnkDelete.Attributes.Add("onclick", "return confirm_delete();");
                }

                              

                PopupControlExtender pce = e.Item.FindControl("PopupControlExtender1") as PopupControlExtender;
                string behaviorID = "pce_" + e.Item.ItemIndex;
                pce.BehaviorID = behaviorID; 
                Image img = (Image)e.Item.FindControl("imgMagnify");
                Image imgPrice = (Image)e.Item.FindControl("imgPrice");
                string OnMouseOverScript = string.Format("$find('{0}').showPopup();", behaviorID);
                string OnMouseOutScript = string.Format("$find('{0}').hidePopup();", behaviorID);

                img.Attributes.Add("onmouseover", OnMouseOverScript);
                img.Attributes.Add("onmouseout", OnMouseOutScript);
                if (session.WebServiceAddress.Contains("gms.leedenlimited.com"))               
                    img.Visible = true;  
                else
                    img.Visible = false;              

                Label lblProdCode = (Label)e.Item.FindControl("lblProdCode");
                Label lblNewProdCode = (Label)e.Item.FindControl("lblNewProdCode");
                if (lblProdCode.Text.Trim() != "00000000000")
                    lblNewProdCode.Visible = false;
                if (this.lblMRNo.Text.Trim() != "")                
                    imgPrice.Visible = true;               
                else
                    imgPrice.Visible = false;
            }
            /*
            lblSubTotal.Text = String.Format("{0:0.00}", finalTotal);
            lblCurrency1.Text = purchaseCurrency;
            lblCurrency2.Text = purchaseCurrency;
            lblCurrency3.Text = purchaseCurrency;
            */


            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddlEditUOM = (DropDownList)e.Item.FindControl("ddlEditUOM");
                HiddenField hidUOM = (HiddenField)e.Item.FindControl("hidUOM");
                DataSet dsTemp = new DataSet();
                (new QuotationDataDALC()).GetAllUOMByCoyIDSelect(session.CompanyId, ref dsTemp);
                ddlEditUOM.DataSource = dsTemp;
                ddlEditUOM.DataValueField = "UOM";
                ddlEditUOM.DataTextField = "UOM";
                ddlEditUOM.DataBind();
                ddlEditUOM.SelectedValue = hidUOM.Value;

                SystemDataActivity dalc = new SystemDataActivity();
                IList<Currency> cList = null;
                try
                {
                    cList = dalc.RetrieveAllCurrencyListSortByCode();
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }

                Company coy = null;
                try
                {
                    coy = dalc.RetrieveCompanyById(session.CompanyId, session);
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }

                DropDownList ddlEditSellingCurrency = (DropDownList)e.Item.FindControl("ddlEditSellingCurrency");
                HiddenField hidSellingCurrency = (HiddenField)e.Item.FindControl("hidSellingCurrency");

                if (ddlEditSellingCurrency != null)
                {
                    ddlEditSellingCurrency.DataSource = cList;
                    ddlEditSellingCurrency.DataTextField = "CurrencyCode";
                    ddlEditSellingCurrency.DataValueField = "CurrencyCode";
                    ddlEditSellingCurrency.DataBind();
                    ddlEditSellingCurrency.SelectedValue = hidSellingCurrency.Value;
                }
                
                DropDownList ddlEditPPCurrency = (DropDownList)e.Item.FindControl("ddlEditPPCurrency");
                HiddenField hidPurchaseCurrency = (HiddenField)e.Item.FindControl("hidPurchaseCurrency");
                if (ddlEditPPCurrency != null)
                {
                    ddlEditPPCurrency.DataSource = cList;
                    ddlEditPPCurrency.DataTextField = "CurrencyCode";
                    ddlEditPPCurrency.DataValueField = "CurrencyCode";
                    ddlEditPPCurrency.DataBind();
                    if (hidPurchaseCurrency.Value == "")
                        ddlEditPPCurrency.SelectedValue = coy.DefaultCurrencyCode;
                    else
                        ddlEditPPCurrency.SelectedValue = hidPurchaseCurrency.Value;
                }

                TextBox txtEditUnitPurchasePrice = (TextBox)e.Item.FindControl("txtEditUnitPurchasePrice");
                TextBox txtEditStkQty = (TextBox)e.Item.FindControl("txtEditStkQty");
                HiddenField hidEditPMUSERID = (HiddenField)e.Item.FindControl("hidEditPMUSERID");
                HiddenField hidEditPHUSERID = (HiddenField)e.Item.FindControl("hidEditPHUSERID");
                HiddenField hidEditPH2USERID = (HiddenField)e.Item.FindControl("hidEditPH2USERID");
                HiddenField hidEditPH3USERID = (HiddenField)e.Item.FindControl("hidEditPH3USERID");
                TextBox txtEditProductCode = (TextBox)e.Item.FindControl("txtEditProductCode");
                TextBox txtEditNewProdCode = (TextBox)e.Item.FindControl("txtEditNewProdCode");
                TextBox txtEditProductDescription = (TextBox)e.Item.FindControl("txtEditProductDescription");
                PopupControlExtender pce = e.Item.FindControl("PopupControlExtender1") as PopupControlExtender;               

                string behaviorID = "pce_" + e.Item.ItemIndex;
                pce.BehaviorID = behaviorID;
                Image img = (Image)e.Item.FindControl("imgMagnify");
                Image imgPrice = (Image)e.Item.FindControl("imgPrice");
                if (txtEditProductCode.Text.Trim() != "00000000000")
                {
                    txtEditNewProdCode.Attributes.Add("readonly", "readonly");
                    txtEditProductDescription.Attributes.Add("readonly", "readonly");
                }
                if (this.lblMRNo.Text.Trim() != "")
                {
                    if ((hidPMUserId.Value == loginUserOrAlternateParty.ToString() || hidPH.Value == loginUserOrAlternateParty.ToString() || hidPH2.Value == loginUserOrAlternateParty.ToString()) ||
                                (viewStockInformationAccess != null))
                    {
                        txtEditStkQty.ReadOnly = false;
                    }
                    else
                    {
                        txtEditStkQty.ReadOnly = true;
                    }
                }
                else
                {
                    if ((hidEditPMUSERID.Value.ToString() == loginUserOrAlternateParty.ToString() || hidEditPHUSERID.ToString() == loginUserOrAlternateParty.ToString() || hidEditPH2USERID.Value.ToString() == loginUserOrAlternateParty.ToString()) ||
                            (viewStockInformationAccess != null))
                    {
                        txtEditStkQty.ReadOnly = false;
                    }
                    else
                    {
                        txtEditStkQty.ReadOnly = true;
                    }
                }
                
                if (viewPurchaseInformationAccess != null || viewAllProductGroupPurchaseInformationAccess != null)
                { 
                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        if ((hidPMUserId.Value == loginUserOrAlternateParty.ToString() || hidPH.Value == loginUserOrAlternateParty.ToString() || hidPH2.Value.ToString() == loginUserOrAlternateParty.ToString() || hidEditPH3USERID.Value.ToString() == loginUserOrAlternateParty.ToString()) ||
                            (userRole == "Purchasing") || (coy.MRScheme.ToString() != "Product"))
                        {
                            txtEditUnitPurchasePrice.ReadOnly = false;
                            ddlEditPPCurrency.Enabled = true;
                            imgPrice.Visible = true;
                        }
                        else
                        {
                            //txtEditUnitPurchasePrice.Attributes.Add("readonly", "readonly");
                            txtEditUnitPurchasePrice.Enabled = false;
                            ddlEditPPCurrency.Enabled = false;
                            imgPrice.Visible = false;
                        }
                    }
                    else
                    {
                        if ((hidEditPMUSERID.Value.ToString() == loginUserOrAlternateParty.ToString() || hidEditPHUSERID.ToString() == loginUserOrAlternateParty.ToString() || hidEditPH2USERID.Value.ToString() == loginUserOrAlternateParty.ToString() || hidEditPH3USERID.Value.ToString() == loginUserOrAlternateParty.ToString()) ||
                            (userRole == "Purchasing") || (coy.MRScheme.ToString() != "Product"))
                        {
                            txtEditUnitPurchasePrice.ReadOnly = false;
                            ddlEditPPCurrency.Enabled = true;
                            imgPrice.Visible = true;
                        }
                        else
                        {
                            txtEditUnitPurchasePrice.Enabled = false;
                            ddlEditPPCurrency.Enabled = false;
                            imgPrice.Visible = false;
                        }
                    }                   
                }
                else
                {
                    //txtEditUnitPurchasePrice.Attributes.Add("readonly", "readonly");
                    txtEditUnitPurchasePrice.Enabled = false;
                    imgPrice.Visible = false;                    
                }

                DataSet lstRequestorPMPH = new DataSet();
                new GMSGeneralDALC().GetMaterialRequisitionPMPHRequestorByCoyID(session.CompanyId, loginUserOrAlternateParty, session.MRScheme ,ref lstRequestorPMPH);
                DropDownList ddlEditPM = (DropDownList)e.Item.FindControl("ddlEditPM");
                DropDownList ddlEditPH = (DropDownList)e.Item.FindControl("ddlEditPH");
                DropDownList ddlEditPH3 = (DropDownList)e.Item.FindControl("ddlEditPH3");
                ddlEditPM.DataSource = lstRequestorPMPH.Tables[1];
                ddlEditPM.DataValueField = "UserNumId";
                ddlEditPM.DataTextField = "UserRealName";
                ddlEditPM.DataBind();
                ddlEditPM.SelectedValue = hidEditPMUSERID.Value;

                ddlEditPH.DataSource = lstRequestorPMPH.Tables[2];
                ddlEditPH.DataValueField = "UserNumId";
                ddlEditPH.DataTextField = "UserRealName";
                ddlEditPH.DataBind();
                ddlEditPH.SelectedValue = hidEditPHUSERID.Value;

                ddlEditPH3.DataSource = lstRequestorPMPH.Tables[3];
                ddlEditPH3.DataValueField = "UserNumId";
                ddlEditPH3.DataTextField = "UserRealName";
                ddlEditPH3.DataBind();
                ddlEditPH3.SelectedValue = hidEditPH3USERID.Value;
                if (lstRequestorPMPH.Tables[3].Rows.Count == 1)
                    ddlEditPH3.Enabled = false;

                ddlEditPM.Visible = true;
                ddlEditPH.Visible = true;
                ddlEditPH3.Visible = true;
                if (this.lblMRNo.Text.Trim() != "")
                {
                    ddlEditPM.Enabled = false;
                    ddlEditPH.Enabled = false;
                    ddlEditPH3.Enabled = false;
                }
                else
                {
                    if (txtEditProductCode.Text.Trim() == "00000000000")
                    {
                        ddlEditPM.Enabled = true;
                        ddlEditPH.Enabled = true;
                        ddlEditPH3.Enabled = true;
                    }
                    else
                    {
                        ddlEditPM.Enabled = false;
                        ddlEditPH.Enabled = false;
                        ddlEditPH3.Enabled = false;
                    }
                }               

                string OnMouseOverScript = string.Format("$find('{0}').showPopup();", behaviorID);
                string OnMouseOutScript = string.Format("$find('{0}').hidePopup();", behaviorID);

                img.Attributes.Add("onmouseover", OnMouseOverScript);
                img.Attributes.Add("onmouseout", OnMouseOutScript);
                
                if (session.WebServiceAddress.Contains("gms.leedenlimited.com"))
                {
                    img.Visible = true;
                }
                else
                {
                    img.Visible = false;
                }
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);               
                scriptManager.SetFocus(txtEditProductCode);            
             }

            

            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewUOM = (DropDownList)e.Item.FindControl("ddlNewUOM");
                DataSet dsTemp = new DataSet();
                (new QuotationDataDALC()).GetAllUOMByCoyIDSelect(session.CompanyId, ref dsTemp);
                ddlNewUOM.DataSource = dsTemp;
                ddlNewUOM.DataValueField = "UOM";
                ddlNewUOM.DataTextField = "UOM";
                ddlNewUOM.DataBind();

                SystemDataActivity dalc = new SystemDataActivity();
                IList<Currency> cList = null;
                try
                {
                    cList = dalc.RetrieveAllCurrencyListSortByCode();
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
                Company coy = null;
                try
                {
                    coy = dalc.RetrieveCompanyById(session.CompanyId, session);
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
               
                DropDownList ddlNewSellingCurrency = (DropDownList)e.Item.FindControl("ddlNewSellingCurrency");
                ddlNewSellingCurrency.DataSource = cList;
                ddlNewSellingCurrency.DataTextField = "CurrencyCode";
                ddlNewSellingCurrency.DataValueField = "CurrencyCode";
                ddlNewSellingCurrency.DataBind();
                ddlNewSellingCurrency.SelectedValue = coy.DefaultCurrencyCode;

                DropDownList ddlNewPPCurrency = (DropDownList)e.Item.FindControl("ddlNewPPCurrency");
                ddlNewPPCurrency.DataSource = cList;
                ddlNewPPCurrency.DataTextField = "CurrencyCode";
                ddlNewPPCurrency.DataValueField = "CurrencyCode";
                ddlNewPPCurrency.DataBind();
                ddlNewPPCurrency.SelectedValue = coy.DefaultCurrencyCode;

                TextBox txtNewStkQty = (TextBox)e.Item.FindControl("txtNewStkQty");

                if (viewStockInformationAccess == null)
                {
                    txtNewStkQty.ReadOnly = true;
                    txtNewStkQty.Text = "0";
                }
                else
                {
                    txtNewStkQty.ReadOnly = false;
                }           
                                         
                DataSet lstRequestorPMPH = new DataSet();
                new GMSGeneralDALC().GetMaterialRequisitionPMPHRequestorByCoyID(session.CompanyId, loginUserOrAlternateParty, session.MRScheme ,ref lstRequestorPMPH);
                DropDownList ddlNewPM = (DropDownList)e.Item.FindControl("ddlNewPM");
                DropDownList ddlNewPH = (DropDownList)e.Item.FindControl("ddlNewPH");
                DropDownList ddlNewPH3 = (DropDownList)e.Item.FindControl("ddlNewPH3");

                ddlNewPM.DataSource = lstRequestorPMPH.Tables[1];
                ddlNewPM.DataValueField = "UserNumId";
                ddlNewPM.DataTextField = "UserRealName";
                ddlNewPM.DataBind();
                ddlNewPM.SelectedValue = "0";

                ddlNewPH.DataSource = lstRequestorPMPH.Tables[2];
                ddlNewPH.DataValueField = "UserNumId";
                ddlNewPH.DataTextField = "UserRealName";
                ddlNewPH.DataBind();
                ddlNewPH.SelectedValue = "0";

                ddlNewPH3.DataSource = lstRequestorPMPH.Tables[3];
                ddlNewPH3.DataValueField = "UserNumId";
                ddlNewPH3.DataTextField = "UserRealName";
                ddlNewPH3.DataBind();
                ddlNewPH3.SelectedValue = "0";
                if (lstRequestorPMPH.Tables[3].Rows.Count == 1)
                    ddlNewPH3.Enabled = false;

                PopupControlExtender pce = e.Item.FindControl("PopupControlExtender1") as PopupControlExtender;               

                string behaviorID = "pce_" + e.Item.ItemIndex;
                pce.BehaviorID = behaviorID;                 
                Image img = (Image)e.Item.FindControl("imgMagnify");
                Image imgPrice = (Image)e.Item.FindControl("imgPrice");               
                string OnMouseOverScript = string.Format("$find('{0}').showPopup();", behaviorID);
                string OnMouseOutScript = string.Format("$find('{0}').hidePopup();", behaviorID);
                img.Attributes.Add("onmouseover", OnMouseOverScript);
                img.Attributes.Add("onmouseout", OnMouseOutScript);
                img.Visible = false;
                imgPrice.Visible = false;
            }
        }

        protected void PurchasePrice_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtNewUnitPurchasePrice = (TextBox)sender;
            TableRow tr = (TableRow)txtNewUnitPurchasePrice.Parent.Parent;
            Label lblNewTotalPrice = (Label)tr.FindControl("lblNewTotalPrice");
            TextBox txtOrderQty = (TextBox)tr.FindControl("txtOrderQty");
            LinkButton lnkCreate = (LinkButton)tr.FindControl("lnkCreate");

            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            decimal qty, unitpp;
            if (decimal.TryParse(txtOrderQty.Text.Trim(), out qty) && decimal.TryParse(txtNewUnitPurchasePrice.Text.Trim(), out unitpp))
            {
                lblNewTotalPrice.Text = (qty * unitpp).ToString();
            }
            scriptManager.SetFocus(lnkCreate);
        }       

        protected void EditOrderQty_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtEditSalesQty = (TextBox)sender;
            TableRow tr = (TableRow)txtEditSalesQty.Parent.Parent;
            TextBox txtEditProductCode = (TextBox)tr.FindControl("txtEditProductCode");
            TextBox txtEditStkQty = (TextBox)tr.FindControl("txtEditStkQty");            
            txtEditSalesQty = (TextBox)tr.FindControl("txtEditQuantity");
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();        
            HiddenField hidEditOnHand = (HiddenField)tr.FindControl("hidEditOnHand");
            HiddenField hidEditOnOrder = (HiddenField)tr.FindControl("hidEditOnOrder");
            HiddenField hidEditOnPO = (HiddenField)tr.FindControl("hidEditOnPO");
            TextBox txtEditOrderQty = (TextBox)tr.FindControl("txtEditOrderQty");
            try
            {
                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                {
                    sc.Url = session.WebServiceAddress.Trim();
                }
                else
                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";

                ds = sc.GetProductStockStatus(session.CompanyId, txtEditProductCode.Text.Trim());
                ds1 = sc.GetProductFullDetail(session.CompanyId, txtEditProductCode.Text.Trim(), "%%", "%%", "%%");
                double hqonhand = 0;
                if ((ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) || (ds1 != null && ds.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if ((lstWarehouse != null) && (lstWarehouse.Tables[0].Rows.Count > 0))
                        {
                            for (int k = 0; k < lstWarehouse.Tables[0].Rows.Count; k++)
                            {
                                if (lstWarehouse.Tables[0].Rows[k]["WH"].ToString() == dr["Warehouse"].ToString())
                                    hqonhand += GMSUtil.ToDouble(dr["Quantity"].ToString());
                            }
                        }
                    }

                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        hidEditOnHand.Value = hqonhand.ToString();
                        hidEditOnOrder.Value = ds1.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                        hidEditOnPO.Value = ds1.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {                
                ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('" + ex.Message + "')", true);
            }
            TextBox txtEditUnitPrice = (TextBox)tr.FindControl("txtEditUnitPrice");
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            decimal salesqty, stkqty, onhand, onso, onpo;
            decimal.TryParse(hidEditOnHand.Value.Trim(), out onhand);
            decimal.TryParse(hidEditOnOrder.Value.Trim(), out onso);
            decimal.TryParse(hidEditOnPO.Value.Trim(), out onpo);
            decimal.TryParse(txtEditSalesQty.Text.Trim(), out salesqty);
            decimal.TryParse(txtEditStkQty.Text.Trim(), out stkqty);
            txtEditOrderQty.Text = (stkqty - (onhand + onpo - onso)).ToString();            
            scriptManager.SetFocus(txtEditUnitPrice);
        }

        protected void EditOrderQty2_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtEditSalesQty = (TextBox)sender;
            TableRow tr = (TableRow)txtEditSalesQty.Parent.Parent;
            TextBox txtEditProductCode = (TextBox)tr.FindControl("txtEditProductCode");
            TextBox txtEditStkQty = (TextBox)tr.FindControl("txtEditStkQty");
            txtEditSalesQty = (TextBox)tr.FindControl("txtEditQuantity");
            TextBox txtEditOrderQty = (TextBox)tr.FindControl("txtEditOrderQty");           
            HiddenField hidEditOnHand = (HiddenField)tr.FindControl("hidEditOnHand");
            TextBox txtEditUnitPrice = (TextBox)tr.FindControl("txtEditUnitPrice");
            HiddenField hidEditOnOrder = (HiddenField)tr.FindControl("hidEditOnOrder");
            HiddenField hidEditOnPO = (HiddenField)tr.FindControl("hidEditOnPO");
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            try
            {
                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                {
                    sc.Url = session.WebServiceAddress.Trim();
                }
                else
                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";

                ds = sc.GetProductStockStatus(session.CompanyId, txtEditProductCode.Text.Trim());
                ds1 = sc.GetProductFullDetail(session.CompanyId, txtEditProductCode.Text.Trim(), "%%", "%%", "%%");
                double hqonhand = 0;

                if ((ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) || (ds1 != null && ds.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if ((lstWarehouse != null) && (lstWarehouse.Tables[0].Rows.Count > 0))
                        {
                            for (int k = 0; k < lstWarehouse.Tables[0].Rows.Count; k++)
                            {
                                if (lstWarehouse.Tables[0].Rows[k]["WH"].ToString() == dr["Warehouse"].ToString())
                                    hqonhand += GMSUtil.ToDouble(dr["Quantity"].ToString());
                            }
                        }
                    }

                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        hidEditOnHand.Value = hqonhand.ToString();
                        hidEditOnOrder.Value = ds1.Tables[0].Rows[0]["OnOrderQuantity"].ToString();
                        hidEditOnPO.Value = ds1.Tables[0].Rows[0]["OnPOQuantity"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {                
                ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('" + ex.Message + "')", true);
            }     
            
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            decimal salesqty, stkqty, onhand, onso, onpo;
            decimal.TryParse(hidEditOnHand.Value.Trim(), out onhand);
            decimal.TryParse(hidEditOnOrder.Value.Trim(), out onso);
            decimal.TryParse(hidEditOnPO.Value.Trim(), out onpo);
            decimal.TryParse(txtEditSalesQty.Text.Trim(), out salesqty);
            decimal.TryParse(txtEditStkQty.Text.Trim(), out stkqty);
            txtEditOrderQty.Text = (stkqty - (onhand + onpo - onso)).ToString();
            scriptManager.SetFocus(txtEditStkQty);
        }

        protected void OrderQty_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtNewSalesQty = (TextBox)sender;
            TableRow tr = (TableRow)txtNewSalesQty.Parent.Parent;
            TextBox txtNewStkQty = (TextBox)tr.FindControl("txtNewStkQty");
            txtNewSalesQty = (TextBox)tr.FindControl("txtNewSalesQty");
            TextBox txtOrderQty = (TextBox)tr.FindControl("txtOrderQty");            
            HiddenField hidOnHand = (HiddenField)tr.FindControl("hidOnHand");
            HiddenField hidOnOrder = (HiddenField)tr.FindControl("hidOnOrder");
            HiddenField hidOnPO = (HiddenField)tr.FindControl("hidOnPO"); 
            TextBox txtNewUnitPrice = (TextBox)tr.FindControl("txtNewUnitPrice");
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            decimal salesqty, stkqty, onhand, onso, onpo;
            decimal.TryParse(hidOnHand.Value.Trim(), out onhand);
            decimal.TryParse(hidOnOrder.Value.Trim(), out onso);
            decimal.TryParse(hidOnPO.Value.Trim(), out onpo);
            decimal.TryParse(txtNewSalesQty.Text.Trim(), out salesqty);
            decimal.TryParse(txtNewStkQty.Text.Trim(), out stkqty);
            txtOrderQty.Text = (stkqty - (onhand + onpo - onso)).ToString();
            scriptManager.SetFocus(txtNewUnitPrice);
        }

        protected void OrderQty2_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtNewSalesQty = (TextBox)sender;
            TableRow tr = (TableRow)txtNewSalesQty.Parent.Parent;
            TextBox txtNewStkQty = (TextBox)tr.FindControl("txtNewStkQty");
            txtNewSalesQty = (TextBox)tr.FindControl("txtNewSalesQty");
            TextBox txtOrderQty = (TextBox)tr.FindControl("txtOrderQty");           
            HiddenField hidOnHand = (HiddenField)tr.FindControl("hidOnHand");
            HiddenField hidOnOrder = (HiddenField)tr.FindControl("hidOnOrder");
            HiddenField hidOnPO = (HiddenField)tr.FindControl("hidOnPO");
            TextBox txtNewUnitPrice = (TextBox)tr.FindControl("txtNewUnitPrice");
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);            
            decimal salesqty, stkqty, onhand, onso, onpo;
            decimal.TryParse(hidOnHand.Value.Trim(), out onhand);
            decimal.TryParse(hidOnOrder.Value.Trim(), out onso);
            decimal.TryParse(hidOnPO.Value.Trim(), out onpo);
            decimal.TryParse(txtNewSalesQty.Text.Trim(), out salesqty);
            decimal.TryParse(txtNewStkQty.Text.Trim(), out stkqty);
            txtOrderQty.Text = (stkqty - (onhand + onpo - onso)).ToString();
            scriptManager.SetFocus(txtNewStkQty);
        }    
       
        protected void txtEditProductCode_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtEditProductCode = (TextBox)sender;
            string prodCode = "";
            prodCode = txtEditProductCode.Text.Trim();

            DataSet ds = new DataSet();
            try
            {
                ProductsDataDALC dalc = new ProductsDataDALC();
                dalc.GetProductParticular(session.CompanyId, prodCode, loginUserOrAlternateParty, ref ds);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TableRow tr = (TableRow)txtEditProductCode.Parent.Parent;
                    TextBox txtEditProductDescription = (TextBox)tr.FindControl("txtEditProductDescription");
                    DropDownList ddlEditUOM = (DropDownList)tr.FindControl("ddlEditUOM");
                    HiddenField hidEditPMUSERID = (HiddenField)tr.FindControl("hidEditPMUSERID");
                    HiddenField hidEditPHUSERID = (HiddenField)tr.FindControl("hidEditPHUSERID");
                    HiddenField hidEditPH2USERID = (HiddenField)tr.FindControl("hidEditPH2USERID");
                    HiddenField hidEditPH3USERID = (HiddenField)tr.FindControl("hidEditPH3USERID");
                    HiddenField hidEditProductGroupCode = (HiddenField)tr.FindControl("hidEditProductGroupCode");
                    TextBox txtEditQuantity = (TextBox)tr.FindControl("txtEditQuantity");
                    TextBox txtEditOrderQty = (TextBox)tr.FindControl("txtEditOrderQty");
                    PopupControlExtender PopupControlExtender1 = (PopupControlExtender)tr.FindControl("PopupControlExtender1");
                    PopupControlExtender PopupControlExtender2 = (PopupControlExtender)tr.FindControl("PopupControlExtender2");
                    Image img = (Image)tr.FindControl("imgMagnify");
                    Image imgPrice = (Image)tr.FindControl("imgPrice");   
                    PopupControlExtender1.DynamicContextKey = session.CompanyId.ToString() + ";" + prodCode.ToString();
                    img.Visible = true;
                    PopupControlExtender2.DynamicContextKey = session.CompanyId.ToString() + ";" + prodCode.ToString();
                    imgPrice.Visible = true;
                    ddlEditUOM.SelectedValue = ds.Tables[0].Rows[0]["UOM"].ToString();
                    txtEditProductDescription.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();
                    hidEditPMUSERID.Value = (GMSUtil.ToInt(ds.Tables[0].Rows[0]["pmuserid"].ToString())).ToString();
                    hidEditPHUSERID.Value = (GMSUtil.ToInt(ds.Tables[0].Rows[0]["phuserid"].ToString())).ToString();
                    hidEditPH2USERID.Value = (GMSUtil.ToInt(ds.Tables[0].Rows[0]["ph2userid"].ToString())).ToString();
                    hidEditPH3USERID.Value = (GMSUtil.ToInt(ds.Tables[0].Rows[0]["ph3userid"].ToString())).ToString();
                    
                    txtEditOrderQty.Text = "";
                    TextBox txtEditStkQty = (TextBox)tr.FindControl("txtEditStkQty");
                    TextBox txtEditUnitPurchasePrice = (TextBox)tr.FindControl("txtEditUnitPurchasePrice");
                    DropDownList ddlEditPPCurrency = (DropDownList)tr.FindControl("ddlEditPPCurrency");                  
                    HiddenField hidEditOnHand = (HiddenField)tr.FindControl("hidEditOnHand");
                    HiddenField hidEditOnOrder = (HiddenField)tr.FindControl("hidEditOnOrder");
                    HiddenField hidEditOnPO = (HiddenField)tr.FindControl("hidEditOnPO");
                    Label lblEditPM = (Label)tr.FindControl("lblEditPM");
                    Label lblEditPH = (Label)tr.FindControl("lblEditPH");
                    Label lblEditPH3 = (Label)tr.FindControl("lblEditPH3");
                    DropDownList ddlEditPM = (DropDownList)tr.FindControl("ddlEditPM");
                    DropDownList ddlEditPH = (DropDownList)tr.FindControl("ddlEditPH");
                    DropDownList ddlEditPH3 = (DropDownList)tr.FindControl("ddlEditPH3");
                    DataSet ds1 = new DataSet();

                    if (coy.MRScheme.ToString() == "Product")
                    {

                        lblEditPM.Visible = true;
                        lblEditPH.Visible = true;
                        lblEditPH3.Visible = true;
                        ddlEditPM.Visible = true;
                        ddlEditPH.Visible = true;
                        ddlEditPH3.Visible = true;

                        if ((this.lblMRNo.Text.Trim() != "") && (txtEditProductCode.Text.Trim() == "00000000000"))
                        {
                            hidEditPMUSERID.Value = (GMSUtil.ToInt(hidPMUserId.Value)).ToString();
                            hidEditPHUSERID.Value = (GMSUtil.ToInt(hidPH.Value)).ToString();
                            hidEditPH2USERID.Value = (GMSUtil.ToInt(hidPH2.Value)).ToString();
                            hidEditPH3USERID.Value = (GMSUtil.ToInt(hidPH3.Value)).ToString();
                        }

                        if ((hidEditPMUSERID.Value == "0") && (hidEditPHUSERID.Value == "0") && (hidEditPH3USERID.Value == "0"))
                        {
                            ProductManagerProduct pmp = new MRActivity().RetriveProductTeamDetailByCoyID(session.CompanyId, "000");
                            if (pmp != null)
                            {
                                hidEditPMUSERID.Value = (GMSUtil.ToInt(pmp.PMUserID)).ToString();
                                hidEditPHUSERID.Value = (GMSUtil.ToInt(pmp.PHUserID)).ToString();
                                hidEditPH2USERID.Value = (GMSUtil.ToInt(pmp.PH2UserID)).ToString();
                                hidEditPH3USERID.Value = (GMSUtil.ToInt(pmp.PH3UserID)).ToString();
                            }
                        }
                    }
                    else
                    {
                        hidEditPMUSERID.Value = hidPMUserId.Value;
                        hidEditPHUSERID.Value = hidPH.Value;
                        hidEditPH2USERID.Value = hidPH2.Value;
                        hidEditPH3USERID.Value = hidPH3.Value;
                    }
                    
                    if ((hidEditPMUSERID.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidEditPHUSERID.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidEditPH2USERID.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidEditPH3USERID.Value.ToString() == loginUserOrAlternateParty.ToString()) ||
                        (viewStockInformationAccess != null) || (coy.MRScheme.ToString() != "Product"))
                    {
                        txtEditStkQty.Enabled = true;
                    }
                    else
                    {
                        txtEditStkQty.Text = "0";
                        txtEditStkQty.Enabled = false;                        
                    }

                    if ((hidEditPMUSERID.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidEditPHUSERID.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidEditPH2USERID.Value.ToString() == loginUserOrAlternateParty.ToString()) || (hidEditPH3USERID.Value.ToString() == loginUserOrAlternateParty.ToString()) ||
                        (userRole == "Purchasing") || (coy.MRScheme.ToString() != "Product" && viewPurchaseInformationAccess != null))
                    {
                        txtEditUnitPurchasePrice.Enabled = true;
                        ddlEditPPCurrency.Enabled = true;
                        imgPrice.Visible = true;
                    }
                    else
                    {
                        txtEditUnitPurchasePrice.Text = "0";                      
                        txtEditUnitPurchasePrice.Enabled = false;
                        ddlEditPPCurrency.Enabled = false;
                        imgPrice.Visible = false;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('Product is not found!')", true);
                    txtEditProductCode.Text = "";
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('" + ex.Message + "')", true);

            }
        }

        protected void txtEditNewProdCode_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtNewProductCode = (TextBox)sender;
            string prodCode = "";
            prodCode = txtNewProductCode.Text.Trim();
            DataSet ds = new DataSet();
            try
            {
                ProductsDataDALC dalc = new ProductsDataDALC();
                dalc.GetProductParticular(session.CompanyId, prodCode, loginUserOrAlternateParty, ref ds);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('New Product Code existed, Please key in the new Product Code!')", true);
                    txtNewProductCode.Text = "";
                    return;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('" + ex.Message + "')", true);
            }

        }


        protected void txtNewProdCode_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtNewProductCode = (TextBox)sender;
            string prodCode = "";
            prodCode = txtNewProductCode.Text.Trim();           
            DataSet ds = new DataSet();
            try
            {
                ProductsDataDALC dalc = new ProductsDataDALC();
                dalc.GetProductParticular(session.CompanyId, prodCode, loginUserOrAlternateParty, ref ds);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('New Product Code existed, Please key in the new Product Code!')", true);
                    txtNewProductCode.Text = "";
                    return;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('" + ex.Message + "')", true);
            }

        }

        protected void txtNewProductCode_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtNewProductCode = (TextBox)sender;
            string prodCode = "";
            prodCode = txtNewProductCode.Text.Trim();           
            DataSet ds = new DataSet();
            try
            {
                ProductsDataDALC dalc = new ProductsDataDALC();
                dalc.GetProductParticular(session.CompanyId, prodCode, loginUserOrAlternateParty, ref ds);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TableRow tr = (TableRow)txtNewProductCode.Parent.Parent;
                    TextBox txtNewProductDescription = (TextBox)tr.FindControl("txtNewProductDescription");
                    DropDownList ddlNewUOM = (DropDownList)tr.FindControl("ddlNewUOM");
                     
                    TextBox txtNewSalesQty = (TextBox)tr.FindControl("txtNewSalesQty");
                    PopupControlExtender PopupControlExtender1 = (PopupControlExtender)tr.FindControl("PopupControlExtender1");                   
                    LinkButton lnkPrice = (LinkButton)tr.FindControl("lnkPrice");
                    Image img = (Image)tr.FindControl("imgMagnify");
                    Image imgPrice = (Image)tr.FindControl("imgPrice");
                    TextBox txtNewStkQty = (TextBox)tr.FindControl("txtNewStkQty");
                    TextBox txtNewUnitPurchasePrice = (TextBox)tr.FindControl("txtNewUnitPurchasePrice");
                    DropDownList ddlNewPPCurrency = (DropDownList)tr.FindControl("ddlNewPPCurrency");  
                    Label lblNewProductCode = (Label)tr.FindControl("lblNewProductCode");
                    TextBox txtNewProdCode = (TextBox)tr.FindControl("txtNewProdCode");
                    HtmlInputHidden hidPMUSERID = (HtmlInputHidden)tr.FindControl("hidPMUSERID");
                    HtmlInputHidden hidPHUSERID = (HtmlInputHidden)tr.FindControl("hidPHUSERID");
                    HtmlInputHidden hidPH2USERID = (HtmlInputHidden)tr.FindControl("hidPH2USERID");
                    HtmlInputHidden hidPH3USERID = (HtmlInputHidden)tr.FindControl("hidPH3USERID");  
                    Label lblNewPM = (Label)tr.FindControl("lblNewPM");
                    Label lblNewPH = (Label)tr.FindControl("lblNewPH");
                    Label lblNewPH3 = (Label)tr.FindControl("lblNewPH3");
                    DropDownList ddlNewPM = (DropDownList)tr.FindControl("ddlNewPM");
                    DropDownList ddlNewPH = (DropDownList)tr.FindControl("ddlNewPH");
                    DropDownList ddlNewPH3 = (DropDownList)tr.FindControl("ddlNewPH3");
                    HiddenField hidNewProductGroupCode = (HiddenField)tr.FindControl("hidNewProductGroupCode");

                    lnkPrice.CommandArgument = prodCode;
                    PopupControlExtender1.DynamicContextKey = session.CompanyId.ToString() + ";" + prodCode.ToString();                   
                    img.Visible = true;
                    imgPrice.Visible = true;
                    ddlNewUOM.SelectedValue = ds.Tables[0].Rows[0]["UOM"].ToString();                    
                    txtNewProductDescription.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();


                    if (coy.MRScheme.ToString() == "Product")
                    {

                        hidPMUSERID.Value = (GMSUtil.ToInt(ds.Tables[0].Rows[0]["pmuserid"].ToString())).ToString();
                        hidPHUSERID.Value = (GMSUtil.ToInt(ds.Tables[0].Rows[0]["phuserid"].ToString())).ToString();
                        hidPH2USERID.Value = (GMSUtil.ToInt(ds.Tables[0].Rows[0]["ph2userid"].ToString())).ToString();
                        hidPH3USERID.Value = (GMSUtil.ToInt(ds.Tables[0].Rows[0]["ph3userid"].ToString())).ToString();

                        DataSet ds1 = new DataSet();

                        lblNewPM.Visible = true;
                        lblNewPH.Visible = true;
                        lblNewPH3.Visible = true;
                        ddlNewPM.Visible = true;
                        ddlNewPH.Visible = true;
                        ddlNewPH3.Visible = true;


                        if ((this.lblMRNo.Text.Trim() != "") && ((txtNewProductCode.Text.Trim() == "00000000000") || ((hidPMUSERID.Value == "0") && (hidPHUSERID.Value == "0") && (hidPH3USERID.Value == "0"))))
                        {
                            hidPMUSERID.Value = (GMSUtil.ToInt(hidPMUserId.Value)).ToString();
                            hidPHUSERID.Value = (GMSUtil.ToInt(hidPH.Value)).ToString();
                            hidPH2USERID.Value = (GMSUtil.ToInt(hidPH2.Value)).ToString();
                            hidPH3USERID.Value = (GMSUtil.ToInt(hidPH3.Value)).ToString();
                        }

                        if ((hidPMUSERID.Value == "0") && (hidPHUSERID.Value == "0") && (hidPH3USERID.Value == "0"))
                        {
                            ProductManagerProduct pmp = new MRActivity().RetriveProductTeamDetailByCoyID(session.CompanyId, "000");
                            if (pmp != null)
                            {
                                hidPMUSERID.Value = (GMSUtil.ToInt(pmp.PMUserID)).ToString();
                                hidPHUSERID.Value = (GMSUtil.ToInt(pmp.PHUserID)).ToString();
                                hidPH2USERID.Value = (GMSUtil.ToInt(pmp.PH2UserID)).ToString();
                                hidPH3USERID.Value = (GMSUtil.ToInt(pmp.PH3UserID)).ToString();
                            }
                        }

                        ddlNewPM.SelectedValue = hidPMUSERID.Value.ToString();
                        ddlNewPH.SelectedValue = hidPHUSERID.Value.ToString();
                        ddlNewPH3.SelectedValue = hidPH3USERID.Value.ToString();

                        if (this.lblMRNo.Text.Trim() != "")
                        {
                            ddlNewPM.Enabled = false;
                            ddlNewPH.Enabled = false;
                            ddlNewPH3.Enabled = false;
                        }
                        else
                        {
                            if (hidPMUSERID.Value.ToString() != "0")
                                ddlNewPM.Enabled = false;
                            if (hidPHUSERID.Value.ToString() != "0")
                                ddlNewPH.Enabled = false;
                            if (hidPH3USERID.Value.ToString() != "0")
                                ddlNewPH3.Enabled = false;

                        }
                    }
                    else
                    {
                        hidPMUSERID.Value = hidPMUserId.Value;
                        hidPHUSERID.Value = hidPH.Value;
                        hidPH2USERID.Value = hidPH2.Value;
                        hidPH3USERID.Value = hidPH3.Value;
                    }
                 
                    if (txtNewProductCode.Text.Trim() == "00000000000")
                    {                       
                        lblNewProductCode.Visible = true;
                        txtNewProdCode.Visible = true;
                        txtNewProductDescription.Enabled = true;  
                    }
                    else
                    {
                        lblNewProductCode.Visible = false;
                        txtNewProdCode.Visible = false;
                        txtNewProductDescription.Enabled = false;                                           
                    }
                    

                    if ((hidPMUSERID.Value.ToString() == loginUserOrAlternateParty.ToString() || hidPHUSERID.Value.ToString() == loginUserOrAlternateParty.ToString() || hidPH2USERID.Value.ToString() == loginUserOrAlternateParty.ToString() || hidPH3USERID.Value.ToString() == loginUserOrAlternateParty.ToString()) ||
                        (viewStockInformationAccess != null) || (coy.MRScheme.ToString() != "Product"))
                    {                       
                        txtNewStkQty.Enabled = true;
                    }
                    else
                    {
                        txtNewStkQty.Text = "0";
                        txtNewStkQty.Enabled = false;                                   
                    }

                    if ((hidPMUSERID.Value.ToString() == loginUserOrAlternateParty.ToString() || hidPHUSERID.Value.ToString() == loginUserOrAlternateParty.ToString() || hidPH3USERID.Value.ToString() == loginUserOrAlternateParty.ToString()) ||
                        (userRole == "Purchasing") || (coy.MRScheme.ToString() != "Product" && viewPurchaseInformationAccess != null))
                    {
                        txtNewUnitPurchasePrice.Enabled = true;
                        ddlNewPPCurrency.Enabled = true;
                        imgPrice.Visible = true;
                    }
                    else
                    {
                        txtNewUnitPurchasePrice.Text = "0";                        
                        txtNewUnitPurchasePrice.Enabled = false;
                        ddlNewPPCurrency.Enabled = false;
                        imgPrice.Visible = false;
                    }               
                }
                else
                {                    
                    ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('Product is not found!')", true);
                    txtNewProductCode.Text = "";
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(upProductInformation, this.GetType(), "click", "alert('" + ex.Message + "')", true);                
            }
        }
       
        protected void txtPO_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtCustomerPO = (TextBox)sender;
            string pono = "";
            pono = txtCustomerPO.Text.Trim();
            DataSet chkExistCustomerPONo = new DataSet();
            string strMrNo = "0";
            if (this.lblMRNo.Text.Trim() != "")
                strMrNo = this.lblMRNo.Text.Trim();

            string pofound = "";
            new GMSGeneralDALC().GetCheckExistCustomerPO(session.CompanyId, strMrNo, pono, ref chkExistCustomerPONo);
            if (chkExistCustomerPONo != null && chkExistCustomerPONo.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < chkExistCustomerPONo.Tables[0].Rows.Count; i++)
                {
                    if (pofound == "")
                        pofound += chkExistCustomerPONo.Tables[0].Rows[i]["mrno"].ToString();
                    else
                        pofound += "," + chkExistCustomerPONo.Tables[0].Rows[i]["mrno"].ToString();
                }

                ScriptManager.RegisterClientScriptBlock(upConfirmedSalesInformation, this.GetType(), "click", "alert('Customer PO exist in MR : " + pofound + "')", true);
                txtCustomerPO.Text = "";
                return;
            }                
        }            

        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Products"));
                    return;
                }

                HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidID");
                if (hidID != null)
                {
                    workTable = (DataTable)Session["ConfirmedSalesInformation"];
                    for (int i = workTable.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = workTable.Rows[i];
                        if (dr["id"].ToString() == hidID.Value)
                            dr.Delete();
                    }
                    Session["ConfirmedSalesInformation"] = workTable;
                    LoadData();
                }
            }
        }

        protected void dgVendorData_Command(Object sender, DataGridCommandEventArgs e)
        { 
            LogSession session = base.GetSessionInfo();                 
            switch (((LinkButton)e.CommandSource).CommandName)
            {                
                case "Create":
                    TextBox txtNewVendor = (TextBox)e.Item.FindControl("txtNewVendor");
                    TextBox txtNewAttnTo = (TextBox)e.Item.FindControl("txtNewAttnTo");
                    TextBox txtNewTel = (TextBox)e.Item.FindControl("txtNewTel");
                    TextBox txtNewFax = (TextBox)e.Item.FindControl("txtNewFax");
                    TextBox txtNewEmail = (TextBox)e.Item.FindControl("txtNewEmail");
                    if (this.lblMRNo.Text.Trim() != "")
                    {

                        GMSCore.Entity.MRVendor tempVendor = new MRActivity().RetrieveVendorByCoyIDMRNoVendorName(session.CompanyId, lblMRNo.Text.Trim(), txtNewVendor.Text.Trim());
                        if (tempVendor != null)
                        {
                            ScriptManager.RegisterClientScriptBlock(upVendorInformation, this.GetType(), "click", "alert('Duplicate Vendor Name in same MR is not allow.Please enter another Name.')", true);
                            return;
                        }  

                        GMSCore.Entity.MRVendor mv = new GMSCore.Entity.MRVendor();
                        mv.CoyID = session.CompanyId;
                        mv.MRNo = lblMRNo.Text;
                        mv.VendorName = txtNewVendor.Text.Trim().ToString();
                        mv.VendorContact = txtNewAttnTo.Text.Trim().ToString();
                        mv.VendorTel = txtNewTel.Text.Trim().ToString();
                        mv.VendorFax = txtNewFax.Text.Trim().ToString();
                        mv.VendorEmail = txtNewEmail.Text.Trim().ToString();
                        mv.CreatedBy = loginUserOrAlternateParty;
                        mv.CreatedDate = DateTime.Now;

                        try
                        {
                            using (TransactionScope tran = new TransactionScope())
                            {                                
                                //insert manual vendor to Supplier List for auto insertion when MR Split
                                DataSet dsProductGroupCode = new DataSet();
                                new GMSGeneralDALC().GetProductGroupCodeByMRNo(session.CompanyId, this.lblMRNo.Text.Trim(), ref dsProductGroupCode);
                                if (dsProductGroupCode != null && dsProductGroupCode.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsProductGroupCode.Tables[0].Rows.Count; i++)
                                    {

                                        IList<MRSupplier> lstSupplier = new MRActivity().RetrieveMRSupplierByCoyIDVendorDetails(session.CompanyId, dsProductGroupCode.Tables[0].Rows[i]["PRODUCTGROUPCODE"].ToString(), mv.VendorName.ToString());
                                        if (lstSupplier.Count == 0)
                                        {
                                            GMSCore.Entity.MRSupplier ms = new GMSCore.Entity.MRSupplier();
                                            ms.CoyID = session.CompanyId;
                                            ms.ProductGroupCode = dsProductGroupCode.Tables[0].Rows[i]["PRODUCTGROUPCODE"].ToString();
                                            ms.AccountName = mv.VendorName.ToString();
                                            ms.Contact = mv.VendorName.ToString();
                                            ms.Tel = mv.VendorTel.ToString();
                                            ms.Fax = mv.VendorFax.ToString();
                                            ms.Email = mv.VendorEmail.ToString();
                                            ms.CreatedBy = loginUserOrAlternateParty;
                                            ms.CreatedDate = DateTime.Now;
                                            ms.Save();
                                        }
                                        else
                                        {
                                            foreach (MRSupplier supplier in lstSupplier)
                                            {
                                                supplier.Contact = mv.VendorContact.ToString();
                                                supplier.Tel = mv.VendorTel.ToString();
                                                supplier.Fax = mv.VendorFax.ToString();
                                                supplier.Email = mv.VendorEmail.ToString();
                                                supplier.ModifiedBy = loginUserOrAlternateParty;
                                                supplier.ModifiedDate = DateTime.Now;
                                                supplier.Save();
                                            }

                                        }
                                    }
                                }
                               
                                mv.Save();

                                Resubmission("Vendor updated.", "Record created successfully!","");
                                tran.Complete();
                            }
                        }
                        catch (Exception ex)
                        {
                            this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }
                    }
                    else
                    {
                        DataTable workTable = null;
                        if (Session["VendorInformation"] != null)
                        {
                            workTable = (DataTable)Session["VendorInformation"];
                        }
                        else
                        {
                            workTable = new DataTable("VendorInformation");
                            DataColumn column = new DataColumn();
                            column.ColumnName = "VendorID";
                            column.DataType = System.Type.GetType("System.Int32");
                            column.AutoIncrement = true;
                            column.AutoIncrementSeed = 1;
                            column.AutoIncrementStep = 1;
                            workTable.Columns.Add(column);
                            workTable.Columns.Add("VendorName", typeof(String));
                            workTable.Columns.Add("VendorContact", typeof(String));
                            workTable.Columns.Add("VendorTel", typeof(String));
                            workTable.Columns.Add("VendorFax", typeof(String));
                            workTable.Columns.Add("VendorEmail", typeof(String));
                            workTable.Columns.Add("MRSupplierID", typeof(String));                            
                        }

                        DataRow workRow = workTable.NewRow();                     
                        workRow["VendorName"] = txtNewVendor.Text.ToString();
                        workRow["VendorContact"] = txtNewAttnTo.Text.ToString();
                        workRow["VendorTel"] = txtNewTel.Text.ToString();
                        workRow["VendorFax"] = txtNewFax.Text.ToString();
                        workRow["VendorEmail"] = txtNewEmail.Text.ToString();
                        workRow["MRSupplierID"] = "0";      
                        workTable.Rows.Add(workRow);
                        Session["VendorInformation"] = workTable;
                    }
                    LoadData();
                    Tabs.ActiveTabIndex = 1;
                    break;
                case "DeletePermanently":
                    MRActivity maActivity = new MRActivity();
                    short mrSupplierID = GMSUtil.ToShort(e.CommandArgument.ToString());                    

                    HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidVendorID");
                    if (hidID != null)
                    {
                        if (this.lblMRNo.Text.Trim() != "")
                        {
                            try
                            {
                               
                                IList<MRVendor> lstMRVendor = null;
                                lstMRVendor = maActivity.RetrieveVendorByMRNo(GMSUtil.ToShort(session.CompanyId), lblMRNo.Text.Trim());
                                if (lstMRVendor.Count > 1)
                                {
                                    using (TransactionScope tran = new TransactionScope())
                                    {

                                        GMSCore.Entity.MRVendor mv = new MRActivity().RetrieveVendorByID(session.CompanyId, hidID.Value.Trim());
                                        if (mv == null)
                                        {
                                            ScriptManager.RegisterClientScriptBlock(upVendorInformation, this.GetType(), "click", "alert('This vendor data cannot be found in database.')", true);
                                            return;
                                        }    

                                        DataSet dsProductGroupCode = new DataSet();
                                        new GMSGeneralDALC().GetProductGroupCodeByMRNo(session.CompanyId, this.lblMRNo.Text.Trim(), ref dsProductGroupCode);
                                        if (dsProductGroupCode != null && dsProductGroupCode.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dsProductGroupCode.Tables[0].Rows.Count; i++)
                                            {
                                                IList<MRSupplier> lstSupplier = new MRActivity().RetrieveMRSupplierByCoyIDVendorDetails(session.CompanyId, dsProductGroupCode.Tables[0].Rows[i]["PRODUCTGROUPCODE"].ToString(), mv.VendorName);
                                                foreach (MRSupplier supplier in lstSupplier)
                                                {                                                    
                                                    maActivity.DeleteMRSupplier(GMSUtil.ToShort(supplier.Id), session);   
                                                }
                                            }
                                        }

                                        ResultType result = maActivity.DeleteMRVendor(hidID.Value, session);
                                        switch (result)
                                        {
                                            case ResultType.Ok:
                                                this.dgVendorData.EditItemIndex = -1;
                                                this.dgVendorData.CurrentPageIndex = 0;
                                                LoadData();
                                                Resubmission("Vendor updated.", "Record deleted successfully!","");
                                                tran.Complete();
                                                break;
                                            default:
                                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                                return;
                                        }
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(upVendorInformation, this.GetType(), "click", "alert('This record cannot be deleted. This Material Requisition must have minimum of one Vendor record!')", true);
                                }
                            }
                            catch (SqlException exSql)
                            {
                                if (exSql.Number == 547)
                                {
                                    this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                                    LoadData();
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
                    break;
                default:
                    // Do nothing.
                    break;
            }            
        }
                
        protected void dgVendorData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgVendorData.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }

        protected void dgVendorData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgVendorData.EditItemIndex = -1;
            LoadData();
        }

        protected void dgVendorData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Products"));
                return;
            }

            HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidVendorID");
            if (hidID != null)
            {
                TextBox txtEditAttnTo = (TextBox)e.Item.FindControl("txtEditAttnTo");
                TextBox txtEditTel = (TextBox)e.Item.FindControl("txtEditTel");
                TextBox txtEditFax = (TextBox)e.Item.FindControl("txtEditFax");
                TextBox txtEditEmail = (TextBox)e.Item.FindControl("txtEditEmail");

                try
                {
                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        GMSCore.Entity.MRVendor mv = new MRActivity().RetrieveVendorByID(session.CompanyId, hidID.Value.Trim());
                        if (mv == null)
                        {
                            ScriptManager.RegisterClientScriptBlock(upVendorInformation, this.GetType(), "click", "alert('This vendor data cannot be found in database.')", true);
                            return;
                        }                                            
                                               

                        using (TransactionScope tran = new TransactionScope())
                        {  
                            if (mv.MRSupplierID != null)
                            {
                                MRSupplier ms = new MRActivity().RetrieveMRSupplierByID(GMSUtil.ToShort(mv.MRSupplierID));
                                if (ms != null)
                                {
                                    ms.Contact = mv.VendorContact;
                                    ms.Tel = mv.VendorTel;
                                    ms.Fax = mv.VendorFax;
                                    ms.Email = mv.VendorEmail;
                                    ms.ModifiedBy = loginUserOrAlternateParty;
                                    ms.ModifiedDate = DateTime.Now;
                                    ms.Save();
                                }
                            }

                            DataSet dsProductGroupCode = new DataSet();
                            new GMSGeneralDALC().GetProductGroupCodeByMRNo(session.CompanyId, this.lblMRNo.Text.Trim(), ref dsProductGroupCode);
                            if (dsProductGroupCode != null && dsProductGroupCode.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsProductGroupCode.Tables[0].Rows.Count; i++)
                                {
                                    IList<MRSupplier> lstSupplier = new MRActivity().RetrieveMRSupplierByCoyIDVendorDetails(session.CompanyId, dsProductGroupCode.Tables[0].Rows[i]["PRODUCTGROUPCODE"].ToString(), mv.VendorName);
                                    foreach (MRSupplier supplier in lstSupplier)
                                    {
                                        supplier.Contact = txtEditAttnTo.Text.Trim().ToString();
                                        supplier.Tel = txtEditTel.Text.Trim().ToString();
                                        supplier.Fax = txtEditFax.Text.Trim().ToString();
                                        supplier.Email = txtEditEmail.Text.Trim().ToString();
                                        supplier.ModifiedBy = loginUserOrAlternateParty;
                                        supplier.ModifiedDate = DateTime.Now;
                                        supplier.Save();
                                    }
                                }
                            }

                            mv.VendorContact = txtEditAttnTo.Text.Trim().ToString();
                            mv.VendorTel = txtEditTel.Text.Trim().ToString();
                            mv.VendorFax = txtEditFax.Text.Trim().ToString();
                            mv.VendorEmail = txtEditEmail.Text.Trim().ToString();
                            mv.ModifiedBy = loginUserOrAlternateParty;
                            mv.ModifiedDate = DateTime.Now;
                            mv.Save();

                            this.dgVendorData.EditItemIndex = -1;                           
                            Resubmission("Vendor updated.", "Record modified successfully!","");
                            tran.Complete();
                        }
                    }
                    else
                    {
                        // update session Data
                        workTable = (DataTable)Session["VendorInformation"];
                        for (int i = workTable.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = workTable.Rows[i];
                            if (dr["VendorID"].ToString() == hidID.Value)
                            {
                                dr["VendorContact"] = txtEditAttnTo.Text.Trim().ToString();
                                dr["VendorTel"] = txtEditTel.Text.Trim().ToString();
                                dr["VendorFax"] = txtEditFax.Text.Trim().ToString();
                                dr["VendorEmail"] = txtEditEmail.Text.Trim().ToString();
                                break;
                            }
                                
                        }
                        Session["VendorInformation"] = workTable;
                        this.dgVendorData.EditItemIndex = -1;
                        ScriptManager.RegisterClientScriptBlock(upVendorInformation, this.GetType(), "click", "alert('Record modified successfully!')", true);                          
                    }    
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
                LoadData();
            }
            
        }

        protected void dgVendorData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Products"));
                    return;
                }

                HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidVendorID");
                if (hidID != null)
                {
                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        try
                        {
                            MRActivity maActivity = new MRActivity();
                            IList<MRVendor> lstMRVendor = null;
                            lstMRVendor = maActivity.RetrieveVendorByMRNo(GMSUtil.ToShort(session.CompanyId), lblMRNo.Text.Trim());
                            //if (lstMRVendor.Count > 1)
                            //{
                                using (TransactionScope tran = new TransactionScope())
                                {
                                    ResultType result = maActivity.DeleteMRVendor(hidID.Value, session);
                                    switch (result)
                                    {
                                        case ResultType.Ok:
                                            this.dgVendorData.EditItemIndex = -1;
                                            this.dgVendorData.CurrentPageIndex = 0;
                                            LoadData();                                            
                                            Resubmission("Vendor updated.", "Record deleted successfully!","");
                                            tran.Complete();
                                            break;
                                        default:
                                            this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                            return;
                                    }
                                }
                            //}
                            //else
                            //{
                              //  ScriptManager.RegisterClientScriptBlock(upVendorInformation, this.GetType(), "click", "alert('This record cannot be deleted. This Material Requisition must have minimum of one Vendor record!')", true);
                            //}
                        }
                        catch (SqlException exSql)
                        {
                            if (exSql.Number == 547)
                            {
                                this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                                LoadData();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);

                            return;
                        }
                    }
                    else
                    {
                        workTable = (DataTable)Session["VendorInformation"];
                        for (int i = workTable.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = workTable.Rows[i];
                            if (dr["VendorID"].ToString() == hidID.Value)
                                dr.Delete();
                        }
                        Session["VendorInformation"] = workTable;
                        LoadData();
                    }        
                }
            }            
        }

        protected void dgVendorData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {  
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkVendorEdit");


                if (!this.dgVendorData.Columns[6].Visible)
                    lnkEdit.Enabled = false;

                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                {
                    lnkDelete.Attributes.Add("onclick", "return confirm_delete();");
                }

               
                HtmlInputHidden hidSupplierID = (HtmlInputHidden)e.Item.FindControl("hidSupplierID");
                LinkButton lnlDeletePermanently = (LinkButton)e.Item.FindControl("lnlDeletePermanently");
                if (coy.MRScheme.ToString() != "Product")
                    lnlDeletePermanently.Visible = false;
                else
                {
                    if (lnlDeletePermanently != null)
                    {
                        lnlDeletePermanently.Attributes.Add("onclick", "return confirm_Permanently();");
                    }
                }

            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                LinkButton lnkFindAccount = (LinkButton)e.Item.FindControl("lnkFindAccount");

                if (coy.MRScheme.ToString() != "Product")
                    lnkFindAccount.Visible = true;
                else
                    lnkFindAccount.Visible = false;
                
            }
        }       

        protected void dgConfirmedSalesData_Command(Object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string ext = Path.GetExtension(e.CommandArgument.ToString());
            string ContentType = "";
            FileUpload FileUpload1 = (FileUpload)e.Item.FindControl("FileUpload1");          
            switch (((LinkButton)e.CommandSource).CommandName)
            {  
                case "Load":
                    if (ext == ".asf")
                        ContentType = "video/x-ms-asf";
                    else if (ext == ".avi")
                        ContentType = "video/avi";
                    else if (ext == ".doc")
                        ContentType = "application/msword";
                    else if (ext == ".zip")
                        ContentType = "application/zip";
                    else if (ext == ".xls")
                        ContentType = "application/vnd.ms-excel";
                    else if (ext == ".gif")
                        ContentType = "image/gif";
                    else if (ext == ".jpg" || ext == "jpeg")
                        ContentType = "image/jpeg";
                    else if (ext == ".wav")
                        ContentType = "audio/wav";
                    else if (ext == ".mp3")
                        ContentType = "audio/mpeg3";
                    else if (ext == ".mpg" || ext == "mpeg")
                        ContentType = "video/mpeg";
                    else if (ext == ".mp3")
                        ContentType = "audio/mpeg3";
                    else if (ext == ".rtf")
                        ContentType = "application/rtf";
                    else if (ext == ".htm" || ext == "html")
                        ContentType = "text/html";
                    else if (ext == ".asp")
                        ContentType = "text/asp";
                    else
                        ContentType = "application/octet-stream";
                    Response.ContentType = ContentType.ToString();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + e.CommandArgument.ToString());
                    Response.TransmitFile(System.Web.Configuration.WebConfigurationManager.AppSettings["MR_DOWNLOAD_PATH"].ToString() + session.CompanyId + "/" + e.CommandArgument.ToString());
                    Response.End();
                    break;
                case "Create":        
                    TextBox txtNewAccountCode = (TextBox)e.Item.FindControl("txtNewAccountCode");
                    TextBox txtNewAccountName = (TextBox)e.Item.FindControl("txtNewAccountName");
                    TextBox txtNewSONo = (TextBox)e.Item.FindControl("txtNewSONo");
                    TextBox txtNewRequiredDate = (TextBox)e.Item.FindControl("txtNewRequiredDate");
                    TextBox txtNewSODate = (TextBox)e.Item.FindControl("txtNewSODate");
                    Label lblUpload = (Label)e.Item.FindControl("lblUpload");
                    if ((Session["FileName"] == null) || (Session["FileNameEncrypted"] == null) || (Session["FileName"].ToString() == "") || (Session["FileNameEncrypted"].ToString() == ""))
                    {
                        ScriptManager.RegisterClientScriptBlock(upConfirmedSalesInformation, this.GetType(), "click", "alert('Please select a file to upload!')", true);
                        return;
                    }
                    DataSet chkExistCustomerPONo = new DataSet();

                    string strMrNo = "0";
                    if (this.lblMRNo.Text.Trim() != "")                  
                        strMrNo = this.lblMRNo.Text.Trim();                 
                    
                    string pofound = "";
                    new GMSGeneralDALC().GetCheckExistCustomerPO(session.CompanyId, strMrNo, txtNewSONo.Text.Trim().ToString(), ref chkExistCustomerPONo);                    
                    if (chkExistCustomerPONo != null && chkExistCustomerPONo.Tables[0].Rows.Count > 0)
                    {                        
                        for (int i = 0; i < chkExistCustomerPONo.Tables[0].Rows.Count; i++)
                        {
                            if (pofound == "")
                                pofound += chkExistCustomerPONo.Tables[0].Rows[i]["mrno"].ToString();
                            else
                                pofound += "," + chkExistCustomerPONo.Tables[0].Rows[i]["mrno"].ToString();
                        }                        
                        ScriptManager.RegisterClientScriptBlock(upConfirmedSalesInformation, this.GetType(), "click", "alert('This Customer PO can be found in MR : " + pofound + "')", true);                                               
                    }
                    
                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        GMSCore.Entity.MRAttachment ma = new GMSCore.Entity.MRAttachment();
                        ma.CoyID = session.CompanyId;
                        ma.MRNo = lblMRNo.Text;
                        ma.CustomerAccountCode = txtNewAccountCode.Text.Trim().ToString();
                        ma.CustomerName = txtNewAccountName.Text.Trim().ToString();
                        ma.RequiredDate = GMSUtil.ToDate(txtNewRequiredDate.Text.Trim().ToString());
                        ma.SONo = txtNewSONo.Text.Trim().ToString();
                        ma.SODate = GMSUtil.ToDate(txtNewSODate.Text.Trim().ToString());
                        ma.CreatedBy = loginUserOrAlternateParty;
                        ma.CreatedDate = DateTime.Now;                      
                        ma.FileName = Session["FileName"].ToString();
                        ma.FileNameEncrypted = Session["FileNameEncrypted"].ToString();


                        GMSCore.Entity.MR mr = new MRActivity().RetrieveMRByMRNo(session.CompanyId, this.lblMRNo.Text.Trim());

                        string intendedUse = "";
                        if ((chkStock.Checked) && intendedUse == "")
                            intendedUse += "Stock";
                        else if ((chkStock.Checked) && intendedUse != "")
                            intendedUse += ",Stock";

                        if ((chkSales.Checked) && intendedUse == "")
                            intendedUse += "Sales";
                        else if ((chkSales.Checked) && intendedUse != "")
                            intendedUse += ",Sales";

                        if ((chkRepair.Checked) && intendedUse == "")
                            intendedUse += "Repair & Maintenance";
                        else if ((chkRepair.Checked) && intendedUse != "")
                            intendedUse += ",Repair & Maintenance";

                        if ((chkAsset.Checked) && intendedUse == "")
                            intendedUse += "Asset";
                        else if ((chkAsset.Checked) && intendedUse != "")
                            intendedUse += ",Asset";

                        if ((chkSample.Checked) && intendedUse == "")
                            intendedUse += "Sample";
                        else if ((chkSample.Checked) && intendedUse != "")
                            intendedUse += ",Sample";

                        if ((chkWorkshop.Checked) && intendedUse == "")
                            intendedUse += "Workshop";
                        else if ((chkWorkshop.Checked) && intendedUse != "")
                            intendedUse += ",Workshop";

                        if ((chkProject.Checked) && intendedUse == "")
                            intendedUse += "Project";
                        else if ((chkProject.Checked) && intendedUse != "")
                            intendedUse += ",Project";                       

                        mr.IntendedUsage = intendedUse.ToString();

                        try
                        {
                            using (TransactionScope tran = new TransactionScope())
                            {
                                ma.Save();

                                mr.Save();




                                Session.Remove("FileName");
                                Session.Remove("FileNameEncrypted");
                                Resubmission("Confirmed Sales updated.", "Record modified successfully!","");
                                tran.Complete();
                            }
                        }
                        catch (Exception ex)
                        {
                            this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }
                    }
                    else
                    {
                        DataTable workTable = null;
                        if (Session["ConfirmedSalesInformation"] != null)
                        {
                            workTable = (DataTable)Session["ConfirmedSalesInformation"];
                        }
                        else
                        {
                            workTable = new DataTable("ConfirmedSalesInformation");
                            DataColumn column = new DataColumn();
                            column.ColumnName = "FileID";
                            column.DataType = System.Type.GetType("System.Int32");
                            column.AutoIncrement = true;
                            column.AutoIncrementSeed = 1;
                            column.AutoIncrementStep = 1;
                            workTable.Columns.Add(column);
                            workTable.Columns.Add("CustomerAccountCode", typeof(String));
                            workTable.Columns.Add("CustomerName", typeof(String));
                            workTable.Columns.Add("RequiredDate", typeof(DateTime));
                            workTable.Columns.Add("FileName", typeof(String));
                            workTable.Columns.Add("FileNameEncrypted", typeof(String));
                            workTable.Columns.Add("SONo", typeof(String));
                            workTable.Columns.Add("SODate", typeof(DateTime));
                        }

                        DataRow workRow = workTable.NewRow();                        
                        workRow["CustomerAccountCode"] = txtNewAccountCode.Text.ToString();
                        workRow["CustomerName"] = txtNewAccountName.Text.ToString();
                        workRow["RequiredDate"] = Convert.ToDateTime(txtNewRequiredDate.Text.ToString());
                        workRow["SONo"] = txtNewSONo.Text.ToString();
                        workRow["SODate"] = Convert.ToDateTime(txtNewSODate.Text.ToString());
                        workRow["FileName"] = Session["FileName"].ToString(); 
                        workRow["FileNameEncrypted"] = Session["FileNameEncrypted"].ToString();
                        workTable.Rows.Add(workRow);
                        Session["ConfirmedSalesInformation"] = workTable;
                        Session.Remove("FileName");
                        Session.Remove("FileNameEncrypted");
                                            
                    }
                    LoadData();  
                    Tabs.ActiveTabIndex = 0;
                    break;
                default:
                    // Do nothing.
                    break;
            }
        }      

        protected void dgConfirmedSalesData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgConfirmedSalesData.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }

        protected void dgConfirmedSalesData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgConfirmedSalesData.EditItemIndex = -1;
            LoadData();
        }
        
        protected void dgConfirmedSalesData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Products"));
                return;
            }

            HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidConfirmedSalesID");
            if (hidID != null)
            {                
                try
                {
                    TextBox txtEditAccountName = (TextBox)e.Item.FindControl("txtEditAccountName");
                    TextBox txtEditSONo = (TextBox)e.Item.FindControl("txtEditSONo");
                    TextBox txtEditRequiredDate = (TextBox)e.Item.FindControl("txtEditRequiredDate");
                    TextBox txtEditSODate = (TextBox)e.Item.FindControl("txtEditSODate");
                    DataSet chkExistCustomerPONo = new DataSet();

                    string strMrNo = "0";
                    if (this.lblMRNo.Text.Trim() != "")
                        strMrNo = this.lblMRNo.Text.Trim();

                    string pofound = "";
                    new GMSGeneralDALC().GetCheckExistCustomerPO(session.CompanyId, strMrNo, txtEditSONo.Text.Trim().ToString(), ref chkExistCustomerPONo);

                    if (chkExistCustomerPONo != null && chkExistCustomerPONo.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < chkExistCustomerPONo.Tables[0].Rows.Count; i++)
                        {
                            if (pofound == "")
                                pofound += chkExistCustomerPONo.Tables[0].Rows[i]["mrno"].ToString();
                            else
                                pofound += "," + chkExistCustomerPONo.Tables[0].Rows[i]["mrno"].ToString();
                        }
                        ScriptManager.RegisterClientScriptBlock(upConfirmedSalesInformation, this.GetType(), "click", "alert('This Customer PO can be found in MR : " + pofound + "')", true);
                    }

                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        GMSCore.Entity.MRAttachment ma = new MRActivity().RetrieveConfirmedSalesByID(session.CompanyId, hidID.Value.Trim());
                        if (ma == null)
                        {                            
                            ScriptManager.RegisterClientScriptBlock(upConfirmedSalesInformation, this.GetType(), "click", "alert('This sales data cannot be found in database.')", true);
                            return;
                        }
                        
                        ma.CustomerName = txtEditAccountName.Text.Trim().ToString();
                        ma.RequiredDate = GMSUtil.ToDate(txtEditRequiredDate.Text.Trim().ToString());
                        ma.SONo = txtEditSONo.Text.Trim().ToString();
                        ma.SODate = GMSUtil.ToDate(txtEditSODate.Text.Trim().ToString());
                        ma.ModifiedBy = loginUserOrAlternateParty;
                        ma.ModifiedDate = DateTime.Now;

                        using (TransactionScope tran = new TransactionScope())
                        {
                            ma.Save();
                            this.dgConfirmedSalesData.EditItemIndex = -1;
                            //ScriptManager.RegisterClientScriptBlock(upConfirmedSalesInformation, this.GetType(), "click", "alert('Record modified successfully!')", true);
                            Resubmission("Confirmed Sales updated.", "Record modified successfully!","");
                            tran.Complete();
                        }           
                    }
                    else
                    {
                        // Edit session Data
                        workTable = (DataTable)Session["ConfirmedSalesInformation"];
                        for (int i = workTable.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = workTable.Rows[i];
                            if (dr["FileID"].ToString() == hidID.Value)
                            {
                                dr["CustomerName"] = txtEditAccountName.Text.Trim().ToString();
                                dr["RequiredDate"] = GMSUtil.ToDate(txtEditRequiredDate.Text.Trim().ToString());
                                dr["SONo"] = txtEditSONo.Text.Trim().ToString();
                                dr["SODate"] = GMSUtil.ToDate(txtEditSODate.Text.Trim().ToString());                                
                                break;
                            }                               
                        }                       
                        
                        Session["ConfirmedSalesInformation"] = workTable;
                        this.dgConfirmedSalesData.EditItemIndex = -1;
                        ScriptManager.RegisterClientScriptBlock(upConfirmedSalesInformation, this.GetType(), "click", "alert('Record modified successfully!')", true);                        
                    }                  
                    
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
                LoadData();
            }           
        }

        protected void dgConfirmedSalesData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Products"));
                    return;
                }

                HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidConfirmedSalesID");
                if (hidID != null)
                {
                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        MRActivity maActivity = new MRActivity();
                        try
                        {
                            IList<MRAttachment> lstMRAttachemnt = null;
                            lstMRAttachemnt = maActivity.RetrieveConfirmedSalesByMRNo(GMSUtil.ToShort(session.CompanyId), lblMRNo.Text.Trim());
                            //if (lstMRAttachemnt.Count > 1)
                            //{
                                using (TransactionScope tran = new TransactionScope())
                                {
                                    ResultType result = maActivity.DeleteMRConfirmedSales(hidID.Value, session);
                                    switch (result)
                                    {
                                        case ResultType.Ok:
                                            this.dgConfirmedSalesData.EditItemIndex = -1;
                                            this.dgConfirmedSalesData.CurrentPageIndex = 0;
                                            LoadData();                                            
                                            Resubmission("Confirmed Sales updated.", "Record deleted successfully!","");
                                            tran.Complete();
                                            break;
                                        default:
                                            this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                            return;
                                    }
                                }
                            //}
                            //else
                            //{                                
                             //   ScriptManager.RegisterClientScriptBlock(upConfirmedSalesInformation, this.GetType(), "click", "alert('This record cannot be deleted. This Material Requisition must have minimum of one Confirmed Sales Order record!')", true);
                            //}
                        }
                        catch (SqlException exSql)
                        {
                            if (exSql.Number == 547)
                            {
                                LoadData();
                                this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);                              
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }
                    }
                    else
                    {
                        workTable = (DataTable)Session["ConfirmedSalesInformation"];
                        for (int i = workTable.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = workTable.Rows[i];
                            if (dr["FileID"].ToString() == hidID.Value)
                                dr.Delete();
                        }
                        Session["ConfirmedSalesInformation"] = workTable;
                        LoadData();
                    }
                }
            }            
        }
        
        protected void dgConfirmedSalesData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {         
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
                if (!this.dgConfirmedSalesData.Columns[7].Visible)
                    lnkEdit.Enabled = false; 
                LinkButton ibtnDownload = (LinkButton)e.Item.FindControl("linkName");
                
                if (ibtnDownload != null)
                {                    
                    ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                    if (sm != null)
                    {
                        sm.RegisterPostBackControl(ibtnDownload);
                    }
                }
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                {
                    lnkDelete.Attributes.Add("onclick", "return confirm_delete();");
                }
            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox txtEditSODate = (TextBox)e.Item.FindControl("txtEditSODate");
                txtEditSODate.Attributes.Add("readonly", "readonly");
                TextBox txtEditRequiredDate = (TextBox)e.Item.FindControl("txtEditRequiredDate");
                txtEditRequiredDate.Attributes.Add("readonly", "readonly");
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                LinkButton lnkCreate = (LinkButton)e.Item.FindControl("lnkCreate");
                TextBox txtNewAccountCode = (TextBox)e.Item.FindControl("txtNewAccountCode");
                txtNewAccountCode.Attributes.Add("readonly", "readonly");
                TextBox txtNewSODate = (TextBox)e.Item.FindControl("txtNewSODate");
                txtNewSODate.Attributes.Add("readonly", "readonly");
                TextBox txtNewRequiredDate = (TextBox)e.Item.FindControl("txtNewRequiredDate");
                txtNewRequiredDate.Attributes.Add("readonly", "readonly");
                if (lnkCreate != null)
                {                   
                    ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                    if (sm != null)
                    {
                        sm.RegisterPostBackControl(lnkCreate);
                    }
                }
            }           
        }
               
        protected void btnSave_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            short ph3UserId = 0;
            short ph2UserId = 0;

            if (rbIsConsole.Checked == true && txtConsoleDate.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('Please fill in the Console Date.')", true);
                return;
            }

            if (this.lblMRNo.Text.Trim() != "")
            {
                bool changes = false;

                if (!chkStock.Checked && !chkSales.Checked && !chkRepair.Checked && !chkAsset.Checked && !chkSample.Checked && !chkWorkshop.Checked && !chkProject.Checked)
                {
                    ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('Please Select at least one type of Intended Use.')", true);
                    return;
                }

                if (chkAsset.Checked && txtGLCode.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('Please enter GLCode for Intended Use - [Asset].')", true);
                    return;
                }

                string mrNo = lblMRNo.Text.Trim();
                GMSCore.Entity.MR mr = new MRActivity().RetrieveMRByMRNo(session.CompanyId, mrNo);
                
                if (mr == null)
                {
                    ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('This material requisition cannot be found in database.')", true);
                    return;
                }
               

                if ((mr.StatusID.ToString() == "C") && ( purchaserApproval == "0"))
                {
                    ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('This material requisition cannot be edited.')", true);
                    return;
                }                
              
                if (ddlSource.SelectedValue == "Local")
                {
                    if (mr.SourceID != "Local")
                        changes = true;
                    mr.SourceID = "Local";                   
                }
                else
                {
                    if (mr.SourceID != "Overseas")
                        changes = true;
                    mr.SourceID = "Overseas";
                }

                mr.TaxTypeID = this.ddlTaxType.SelectedValue;
                mr.TaxRate = GMSUtil.ToDecimal(this.txtTaxRate.Text.Trim().TrimEnd('%')) / 100;
                mr.Discount = GMSUtil.ToDecimal(this.txtDiscount.Text.Trim()); 

                string intendedUse = "";
                if ((chkStock.Checked) && intendedUse == "")
                    intendedUse += "Stock";
                else if ((chkStock.Checked) && intendedUse != "")
                    intendedUse += ",Stock";

                if ((chkSales.Checked) && intendedUse == "")
                    intendedUse += "Sales";
                else if ((chkSales.Checked) && intendedUse != "")
                    intendedUse += ",Sales";

                if ((chkRepair.Checked) && intendedUse == "")
                    intendedUse += "Repair & Maintenance";
                else if ((chkRepair.Checked) && intendedUse != "")
                    intendedUse += ",Repair & Maintenance";

                if ((chkAsset.Checked) && intendedUse == "")
                    intendedUse += "Asset";
                else if ((chkAsset.Checked) && intendedUse != "")
                    intendedUse += ",Asset";

                if ((chkSample.Checked) && intendedUse == "")
                    intendedUse += "Sample";
                else if ((chkSample.Checked) && intendedUse != "")
                    intendedUse += ",Sample";

                if ((chkWorkshop.Checked) && intendedUse == "")
                    intendedUse += "Workshop";
                else if ((chkWorkshop.Checked) && intendedUse != "")
                    intendedUse += ",Workshop";

                if ((chkProject.Checked) && intendedUse == "")
                    intendedUse += "Project";
                else if ((chkProject.Checked) && intendedUse != "")
                    intendedUse += ",Project";


                if (mr.IntendedUsage != intendedUse.ToString())
                    changes = true;

                mr.IntendedUsage = intendedUse.ToString();

                if (mr.MRDate != GMSUtil.ToDate(txtMRDate.Text.Trim().ToString()))
                    changes = true;

                mr.MRDate = GMSUtil.ToDate(txtMRDate.Text.Trim().ToString());

                if (mr.OrderReason != txtOtherPurchaseReason.Text.Trim().ToString())
                    changes = true;

                mr.OrderReason = txtOtherPurchaseReason.Text.Trim().ToString();

                if (mr.RequestorRemarks != txtRemarksByRequestor.Text.Trim().ToString())
                    changes = true;

                mr.RequestorRemarks = txtRemarksByRequestor.Text.Trim().ToString();

                if (mr.PurchasingRemarks != txtPurchasingRemarks.Text.Trim().ToString())
                    changes = true;

                mr.PurchasingRemarks = txtPurchasingRemarks.Text.Trim().ToString();

                
                if (mr.VendorRemarks != txtVendorRemarks.Text.Trim().ToString())
                    changes = true;

                mr.VendorRemarks = txtVendorRemarks.Text.Trim().ToString();

                bool changePMPH = false;
                if (ddlPM.Visible == true)
                {
                    if (mr.PMUserId != GMSUtil.ToShort(ddlPM.SelectedValue.ToString()))
                    {
                        changes = true;
                        changePMPH = true;
                        hidPMUserId.Value = ddlPM.SelectedValue.ToString();
                        mr.PMUserId = GMSUtil.ToShort(hidPMUserId.Value);
                    }
                }
                else
                {
                    if (mr.PMUserId != GMSUtil.ToShort(hidPMUserId.Value))
                    {
                        changes = true;
                        changePMPH = true;
                        mr.PMUserId = GMSUtil.ToShort(hidPMUserId.Value);
                    }

                }


                if (ddlPH.Visible == true)
                {
                    if (mr.PHUserId != GMSUtil.ToShort(ddlPH.SelectedValue.ToString()))
                    {
                        changes = true;
                        changePMPH = true;
                        hidPH.Value = ddlPH.SelectedValue.ToString();
                        mr.PHUserId = GMSUtil.ToShort(hidPH.Value);
                    }
                }
                else
                {
                    if (mr.PHUserId != GMSUtil.ToShort(hidPH.Value))
                    {
                        changes = true;
                        changePMPH = true;                        
                        mr.PHUserId = GMSUtil.ToShort(hidPH.Value);
                    }

                }

                if (ddlPH3.Visible == true)
                {
                    if (mr.PH3UserId != GMSUtil.ToShort(ddlPH3.SelectedValue.ToString()))
                    {
                        changes = true;
                        changePMPH = true;
                        hidPH3.Value = ddlPH3.SelectedValue.ToString();
                        mr.PH3UserId = GMSUtil.ToShort(hidPH3.Value);
                    }
                }
                else
                {
                    if (mr.PH3UserId != GMSUtil.ToShort(hidPH3.Value))
                    {
                        changes = true;
                        changePMPH = true;                      
                        mr.PH3UserId = GMSUtil.ToShort(hidPH3.Value);
                    }

                }

                if (changePMPH == true && coy.MRScheme.ToString() == "Product")
                {                     
                    DataSet chkExistPMPH = new DataSet();
                    new GMSGeneralDALC().GetExistPMPHByPHPMCoyID(session.CompanyId, GMSUtil.ToShort(ddlPM.SelectedValue.ToString()), GMSUtil.ToShort(ddlPH.SelectedValue.ToString()), GMSUtil.ToShort(ddlPH3.SelectedValue.ToString()), ref chkExistPMPH);
                    if ((chkExistPMPH != null) && (chkExistPMPH.Tables[0].Rows.Count == 0))
                    {
                        ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('Selected PM not tided with selected PH in system!')", true);
                        return;
                    }                        
               }
                   
                if ((chkAsset.Checked) && (mr.GLCode != txtGLCode.Text.Trim().ToString()))
                {
                    changes = true;
                    mr.GLCode = txtGLCode.Text.Trim().ToString();
                }

                //mr.ModifiedBy = loginUserOrAlternateParty;
                mr.ModifiedBy = session.UserId;
                mr.ModifiedDate = DateTime.Now;
                if (rbIsConsole.Checked == true)
                {
                    if (mr.IsConsole == false)
                        changes = true;
                    mr.IsConsole = true;
                    mr.ConsoleDate = GMSUtil.ToDate(txtConsoleDate.Text.Trim().ToString());
                }
                else
                {
                    if (mr.IsConsole == true)
                        changes = true;
                    mr.IsConsole = false;
                    mr.ConsoleDate = GMSUtil.ToDate("");
                }

                if (rbAir.Checked)
                {
                    if (mr.FreightMode != "Air")
                        changes = true;
                    mr.FreightMode = "Air";
                }
                else if (rbSea.Checked)
                {
                    if (mr.FreightMode != "Sea")
                        changes = true;
                    mr.FreightMode = "Sea";
                }
                else if (rbCourier.Checked)
                {
                    if (mr.FreightMode != "Courier")
                        changes = true;
                    mr.FreightMode = "Courier";
                }
                else if (rbLand.Checked)
                {
                    if (mr.FreightMode != "Land")
                        changes = true;
                    mr.FreightMode = "Land";
                }
                else
                    mr.FreightMode = "";               

                if (ddlStatus.Enabled)
                    mr.StatusID = ddlStatus.SelectedValue.ToString();
               
                if (ddlRequestor.Visible)
                {
                    if (mr.Requestor.ToString() != ddlRequestor.SelectedValue.ToString())
                    {
                        changes = true;
                        mr.Requestor = GMSUtil.ToShort(ddlRequestor.SelectedValue.ToString());
                    }
                }

                mr.ProjectNo = txtProjectNo.Text.Trim().ToString();
                mr.RefNo = txtRefNo.Text.Trim().ToString();
                mr.BudgetCode = txtBudgetCode.Text.Trim().ToString();
                                             

                try
                {
                    using (TransactionScope tran = new TransactionScope())
                    {
                        mr.Save();
                        if (((ddlStatus.SelectedValue.ToString() == "R") || (ddlStatus.SelectedValue.ToString() == "F")) && changes)
                        {
                            if (changePMPH == true)
                                new ApprovalActivity().ReSubmitMR(session.CompanyId, loginUserOrAlternateParty, this.lblMRNo.Text.Trim(), GMSUtil.ToShort(hidPMUserId.Value), GMSUtil.ToShort(hidPH.Value), GMSUtil.ToShort(hidPH3.Value), levelID, "Header updated.", "R", true, session.UserId);
                            else
                                new ApprovalActivity().ReSubmitMR(session.CompanyId, loginUserOrAlternateParty, this.lblMRNo.Text.Trim(), GMSUtil.ToShort(hidPMUserId.Value), GMSUtil.ToShort(hidPH.Value), GMSUtil.ToShort(hidPH3.Value), levelID, "Header updated.", "R", false, session.UserId);

                        }
                        else if (((ddlStatus.SelectedValue.ToString() == "A") || (ddlStatus.SelectedValue.ToString() == "P")) && changes)
                        {
                            new ApprovalActivity().ReSubmitMR(session.CompanyId, loginUserOrAlternateParty, this.lblMRNo.Text.Trim(), GMSUtil.ToShort(hidPMUserId.Value), GMSUtil.ToShort(hidPH.Value), GMSUtil.ToShort(hidPH3.Value), levelID, "Header updated.", "U", false, session.UserId);
                        }
                        tran.Complete();
                    }                    
                }
                catch (Exception ex)
                {                  
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }            
                StringBuilder str = new StringBuilder();
                str.Append("<script language='javascript'>");
                str.Append("alert('Record modified successfully!');");
                str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + mrNo + "\"");
                str.Append("</script>");
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
            }
            else
            {
                if (!chkStock.Checked && !chkSales.Checked && !chkRepair.Checked && !chkAsset.Checked && !chkSample.Checked && !chkWorkshop.Checked && !chkProject.Checked)
                {
                    ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('Please Select at least one type of Intended Use.')", true);
                    return;
                }

                if (chkAsset.Checked && txtGLCode.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('Please enter GLCode for Intended Use - [Asset].')", true);
                    return;
                }

                DataTable purchaseInfo = (DataTable)Session["PurchaseInformation"];
                DataTable confirmSalesInfo = (DataTable)Session["ConfirmedSalesInformation"];
                DataTable vendorInfo = (DataTable)Session["VendorInformation"];
                DataTable productInfo = (DataTable)Session["ProductDetail"];
                DataTable attachment = (DataTable)Session["Attachment"];
                
                if ((chkSales.Checked) && ((confirmSalesInfo != null && confirmSalesInfo.Rows.Count == 0) || confirmSalesInfo == null))
                {
                    ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('Please complete essential information in Sales Tab')", true);
                    return;
                }

                if (coy.MRScheme.ToString() == "Product")
                {

                    if ((productInfo != null && productInfo.Rows.Count == 0) || productInfo == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('Please complete essential information in Product Tab')", true);
                        return;
                    }
                }
                    
                string key = "PMUserID";

                
                    if (ddlSource.SelectedValue != "Local")
                        key = "KeyToSplit";
                    else
                        key = "PMUserID";


                DataTable uniqueCols = null;
                int totalUniqueCount = 1;

                if (productInfo != null && productInfo.Rows.Count > 0)
                {
                    uniqueCols = productInfo.DefaultView.ToTable(true, key);
                    totalUniqueCount = uniqueCols.Rows.Count;
                }                  

                DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(session.CompanyId, (short)DateTime.Now.Year);
                if (documentNumber == null) //If tbDocumentNumber does not exist
                {
                    documentNumber = new DocumentNumber();
                    documentNumber.CoyID = session.CompanyId;
                    documentNumber.Year = (short)DateTime.Now.Year;
                    documentNumber.QuotationNo = "0001";
                    documentNumber.ExternalCourseCodePrefix = "E";
                    documentNumber.ExternalCourseCodeNumber = "001";
                    documentNumber.InternalCourseCodePrefix = "I";
                    documentNumber.InternalCourseCodeNumber = "001";
                    documentNumber.OrganizerID = 0;
                    documentNumber.EmployeeCourseRowID = 0;
                    documentNumber.EmployeeID = 0;
                    documentNumber.AttachmentNo = "0001";
                    documentNumber.ProspectNo = "0001";
                    documentNumber.ContactNo = "0001";
                    documentNumber.CommNo = "0001";
                    documentNumber.CommCommentNo = "0001";
                    documentNumber.PurchaseNo = "0001";
                    documentNumber.MRNo = "0001";
                }


                string mrno = "MR" + DateTime.Now.ToString("yy") + "" + documentNumber.MRNo.ToString();
                string lstMrNo = "";


                for (int j = 1; j <= totalUniqueCount; j++)
                {
                    if (uniqueCols != null && uniqueCols.Rows.Count > 1)
                        mrno = "MR" + DateTime.Now.ToString("yy") + "" + documentNumber.MRNo.ToString() + "-" + j.ToString();
                    
                    if (lstMrNo == "")
                        lstMrNo += mrno;
                    else
                        lstMrNo += ", " + mrno;

                    GMSCore.Entity.MR mr = new GMSCore.Entity.MR();
                    mr.CoyID = session.CompanyId;
                    mr.MRNo = mrno;
                    string shipMode = "";
                    string intendedUse = "";
                    mr.MRDate = GMSUtil.ToDate(txtMRDate.Text.Trim().ToString());
                    mr.StatusID = "D";

                    if (ddlSource.SelectedValue == "Local")
                        mr.SourceID = "Local";
                    else
                        mr.SourceID = "Overseas";

                    if ((chkStock.Checked) && intendedUse == "")
                        intendedUse += "Stock";
                    else if ((chkStock.Checked) && intendedUse != "")
                        intendedUse += ",Stock";

                    if ((chkSales.Checked) && intendedUse == "")
                        intendedUse += "Sales";
                    else if ((chkSales.Checked) && intendedUse != "")
                        intendedUse += ",Sales";

                    if ((chkRepair.Checked) && intendedUse == "")
                        intendedUse += "Repair & Maintenance";
                    else if ((chkRepair.Checked) && intendedUse != "")
                        intendedUse += ",Repair & Maintenance";

                    if ((chkAsset.Checked) && intendedUse == "")
                        intendedUse += "Asset";
                    else if ((chkAsset.Checked) && intendedUse != "")
                        intendedUse += ",Asset";

                    if (chkAsset.Checked)
                        mr.GLCode = txtGLCode.Text.ToString();

                    if ((chkSample.Checked) && intendedUse == "")
                        intendedUse += "Sample";
                    else if ((chkSample.Checked) && intendedUse != "")
                        intendedUse += ",Sample";

                    if ((chkWorkshop.Checked) && intendedUse == "")
                        intendedUse += "Workshop";
                    else if ((chkWorkshop.Checked) && intendedUse != "")
                        intendedUse += ",Workshop";

                    if ((chkProject.Checked) && intendedUse == "")
                        intendedUse += "Project";
                    else if ((chkProject.Checked) && intendedUse != "")
                        intendedUse += ",Project";

                    mr.OrderReason = txtOtherPurchaseReason.Text.ToString();
                    mr.IntendedUsage = intendedUse.ToString();
                    mr.CancelledReason = "";
                    mr.Requestor = GMSUtil.ToShort(ddlRequestor.SelectedValue.ToString());
                    mr.RequestorRemarks = txtRemarksByRequestor.Text.ToString();
                    mr.PurchasingRemarks = txtPurchasingRemarks.Text.Trim().ToString();
                    mr.VendorRemarks = txtVendorRemarks.Text.Trim().ToString();
                    //mr.CreatedBy = loginUserOrAlternateParty;
                    mr.CreatedBy = session.UserId;
                    mr.CreatedDate = DateTime.Now;
                    mr.GLCode = txtGLCode.Text.Trim().ToString();
                    mr.ProjectNo = txtProjectNo.Text.Trim().ToString();
                    mr.RefNo = txtRefNo.Text.Trim().ToString();
                    mr.BudgetCode = txtBudgetCode.Text.Trim().ToString();

                    mr.TaxTypeID = this.ddlTaxType.SelectedValue;
                    mr.TaxRate = GMSUtil.ToDecimal(this.txtTaxRate.Text.Trim().TrimEnd('%')) / 100;
                    mr.Discount = GMSUtil.ToDecimal(this.txtDiscount.Text.Trim()); 

                    if (rbIsConsole.Checked == true)
                    {
                        mr.IsConsole = true;
                        mr.ConsoleDate = GMSUtil.ToDate(txtConsoleDate.Text.Trim().ToString());
                    }
                    else
                    {
                        mr.IsConsole = false;
                        mr.ConsoleDate = GMSUtil.ToDate("");
                    }

                    if (rbAir.Checked)
                        mr.FreightMode = "Air";
                    else if (rbSea.Checked)
                        mr.FreightMode = "Sea";
                    else if (rbCourier.Checked)
                        mr.FreightMode = "Courier";
                    else if (rbLand.Checked)
                        mr.FreightMode = "Land";
                    else
                        mr.FreightMode = "";

                    if ((chkSales.Checked) && (confirmSalesInfo != null && confirmSalesInfo.Rows.Count > 0))
                    {
                        foreach (DataRow row in confirmSalesInfo.Rows)
                        {
                            GMSCore.Entity.MRAttachment ma = new GMSCore.Entity.MRAttachment();
                            ma.CoyID = session.CompanyId;
                            ma.MRNo = mrno;
                            ma.CustomerAccountCode = row["CustomerAccountCode"].ToString();
                            ma.CustomerName = row["CustomerName"].ToString();
                            ma.RequiredDate = GMSUtil.ToDate(row["RequiredDate"].ToString());
                            ma.SONo = row["SONo"].ToString();
                            ma.SODate = GMSUtil.ToDate(row["SODate"].ToString());
                            ma.FileName = row["FileName"].ToString();
                            ma.FileNameEncrypted = row["FileNameEncrypted"].ToString();
                            //ma.CreatedBy = loginUserOrAlternateParty;
                            ma.CreatedBy = session.UserId;
                            ma.CreatedDate = DateTime.Now;
                            ma.Save();
                        }
                    }

                    if (attachment != null && attachment.Rows.Count > 0)
                    {
                        foreach (DataRow row in attachment.Rows)
                        {
                            GMSCore.Entity.MRAdditionalAttachment mra = new GMSCore.Entity.MRAdditionalAttachment();
                            mra.CoyID = session.CompanyId;
                            mra.MRNo = mrno;
                            mra.FileDisplayName = row["FileDisplayName"].ToString();
                            mra.FileName = row["FileName"].ToString();
                            mra.FileNameEncrypted = row["FileNameEncrypted"].ToString();
                            //mra.CreatedBy = loginUserOrAlternateParty;
                            mra.CreatedBy = session.UserId;
                            mra.CreatedDate = DateTime.Now;
                            mra.Save();
                        }
                    }
                  
                    if (vendorInfo != null && vendorInfo.Rows.Count > 0)
                    {
                        foreach (DataRow row in vendorInfo.Rows)
                        {
                            GMSCore.Entity.MRVendor mv = new GMSCore.Entity.MRVendor();
                            mv.CoyID = session.CompanyId;
                            mv.MRNo = mrno;
                            mv.VendorName = row["VendorName"].ToString();
                            mv.VendorContact = row["VendorContact"].ToString();
                            mv.VendorTel = row["VendorTel"].ToString();
                            mv.VendorFax = row["VendorFax"].ToString();
                            mv.VendorEmail = row["VendorEmail"].ToString();
                            //mv.CreatedBy = loginUserOrAlternateParty;
                            mv.CreatedBy = session.UserId;
                            mv.CreatedDate = DateTime.Now;
                            mv.Save();
                        }
                    }

                    if (coy.MRScheme.ToString() == "Product")
                    {

                        if (productInfo != null && productInfo.Rows.Count > 0)
                        {
                            bool insertedVendor = false;

                            foreach (DataRow row in productInfo.Rows)
                            {
                                if (row[key].ToString() == uniqueCols.Rows[j - 1][key].ToString())
                                {
                                    if ((!insertedVendor) && (key == "KeyToSplit"))
                                    {
                                        IList<MRSupplier> lstSupplier = new MRActivity().RetrieveMRSupplierByCoyIDProductGroupCode(session.CompanyId, row["ProductGroupCode"].ToString().Trim());

                                        foreach (MRSupplier supplier in lstSupplier)
                                        {
                                            GMSCore.Entity.MRVendor mv = new GMSCore.Entity.MRVendor();
                                            mv.CoyID = session.CompanyId;
                                            mv.MRNo = mrno;
                                            mv.VendorName = supplier.AccountName.ToString();
                                            mv.VendorContact = supplier.Contact.ToString();
                                            mv.VendorTel = supplier.Tel.ToString();
                                            mv.VendorFax = supplier.Fax.ToString();
                                            mv.VendorEmail = supplier.Email.ToString();
                                            mv.CreatedBy = session.UserId;
                                            //mv.CreatedBy = loginUserOrAlternateParty;
                                            mv.CreatedDate = DateTime.Now;
                                            mv.MRSupplierID = (short)supplier.Id;
                                            mv.Save();
                                        }
                                    }

                                    insertedVendor = true;

                                    GMSCore.Entity.MRDetail md = new GMSCore.Entity.MRDetail();
                                    md.CoyID = session.CompanyId;
                                    md.MRNo = mrno;
                                    md.ProdCode = row["ProdCode"].ToString().Trim();
                                    md.NewProdCode = row["NewProdCode"].ToString();
                                    md.ProdName = row["ProdName"].ToString();
                                    md.UOM = row["UOM"].ToString();
                                    md.OnHandQty = GMSUtil.ToDouble(row["OnHandQty"].ToString());
                                    md.OnOrderQty = GMSUtil.ToDouble(row["OnOrderQty"].ToString());
                                    md.OnPOQty = GMSUtil.ToDouble(row["OnPOQty"].ToString());
                                    md.ConfirmedOrderQty = GMSUtil.ToDouble(row["ConfirmedOrderQty"].ToString());
                                    md.ForStockingQty = GMSUtil.ToDouble(row["ForStockingQty"].ToString());
                                    md.OrderQty = GMSUtil.ToDouble(row["OrderQty"].ToString());
                                    md.SellingCurrency = row["SellingCurrency"].ToString();
                                    md.UnitSellingPrice = GMSUtil.ToDouble(row["UnitSellingPrice"].ToString());
                                    md.PurchaseCurrency = row["PurchaseCurrency"].ToString();
                                    md.UnitPurchasePrice = GMSUtil.ToDouble(row["UnitPurchasePrice"].ToString());
                                    md.Reason = row["Reason"].ToString();
                                    //md.CreatedBy = loginUserOrAlternateParty;
                                    md.CreatedBy = session.UserId;
                                    md.CreatedDate = DateTime.Now;

                                    mr.PMUserId = GMSUtil.ToShort(row["PMUserID"].ToString());
                                    mr.PHUserId = GMSUtil.ToShort(row["PHUserID"].ToString());
                                    mr.PH2UserId = GMSUtil.ToShort(row["PH2UserID"].ToString());
                                    mr.PH3UserId = GMSUtil.ToShort(row["PH3UserID"].ToString());
                                    md.Save();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (productInfo != null && productInfo.Rows.Count > 0)
                        {
                            foreach (DataRow row in productInfo.Rows)
                            {
                                GMSCore.Entity.MRDetail md = new GMSCore.Entity.MRDetail();
                                md.CoyID = session.CompanyId;
                                md.MRNo = mrno;
                                md.ProdCode = row["ProdCode"].ToString().Trim();
                                md.NewProdCode = row["NewProdCode"].ToString();
                                md.ProdName = row["ProdName"].ToString();
                                md.UOM = row["UOM"].ToString();
                                md.OnHandQty = GMSUtil.ToDouble(row["OnHandQty"].ToString());
                                md.OnOrderQty = GMSUtil.ToDouble(row["OnOrderQty"].ToString());
                                md.OnPOQty = GMSUtil.ToDouble(row["OnPOQty"].ToString());
                                md.ConfirmedOrderQty = GMSUtil.ToDouble(row["ConfirmedOrderQty"].ToString());
                                md.ForStockingQty = GMSUtil.ToDouble(row["ForStockingQty"].ToString());
                                md.OrderQty = GMSUtil.ToDouble(row["OrderQty"].ToString());
                                md.SellingCurrency = row["SellingCurrency"].ToString();
                                md.UnitSellingPrice = GMSUtil.ToDouble(row["UnitSellingPrice"].ToString());
                                md.PurchaseCurrency = row["PurchaseCurrency"].ToString();
                                md.UnitPurchasePrice = GMSUtil.ToDouble(row["UnitPurchasePrice"].ToString());
                                md.Reason = row["Reason"].ToString();
                                md.CreatedBy = session.UserId;
                                md.CreatedDate = DateTime.Now;
                                md.Save();
                            }
                        }
                        mr.PMUserId = GMSUtil.ToShort(hidPMUserId.Value);
                        mr.PHUserId = GMSUtil.ToShort(hidPH.Value);
                        mr.PH2UserId = GMSUtil.ToShort(hidPH2.Value);
                        mr.PH3UserId = GMSUtil.ToShort(hidPH3.Value);
                        /*
                        DataSet dsApprovalUserLevel1 = new DataSet();
                        new GMSGeneralDALC().GetMRApprovalUserByCoyID(session.CompanyId, session.UserName, ref dsApprovalUserLevel1);
                        if ((dsApprovalUserLevel1 != null) && (dsApprovalUserLevel1.Tables[0].Rows.Count > 0))
                        {
                            mr.PMUserId = GMSUtil.ToShort(dsApprovalUserLevel1.Tables[0].Rows[0]["UserNumID"].ToString());

                            DataSet dsApprovalUserLevel2 = new DataSet();
                            new GMSGeneralDALC().GetMRApprovalUserByCoyID(session.CompanyId, dsApprovalUserLevel1.Tables[0].Rows[0]["approvalusername"].ToString(), ref dsApprovalUserLevel2);
                            if ((dsApprovalUserLevel2 != null) && (dsApprovalUserLevel2.Tables[0].Rows.Count > 0))
                            {
                                mr.PHUserId = GMSUtil.ToShort(dsApprovalUserLevel2.Tables[0].Rows[0]["UserNumID"].ToString());

                                DataSet dsApprovalUserLevel3 = new DataSet();
                                new GMSGeneralDALC().GetMRApprovalUserByCoyID(session.CompanyId, dsApprovalUserLevel2.Tables[0].Rows[0]["approvalusername"].ToString(), ref dsApprovalUserLevel3);
                                if ((dsApprovalUserLevel3 != null) && (dsApprovalUserLevel3.Tables[0].Rows.Count > 0))
                                {
                                    mr.PH2UserId = GMSUtil.ToShort(dsApprovalUserLevel3.Tables[0].Rows[0]["UserNumID"].ToString());

                                }


                            }
                        }
                        */


                    }
                    mr.Save();
                }

                string nxtStr = ((short)(short.Parse(documentNumber.MRNo) + 1)).ToString();
                for (int i = nxtStr.Length; i < documentNumber.MRNo.Length; i++)
                {
                    nxtStr = "0" + nxtStr;
                }
                documentNumber.MRNo = nxtStr;
                documentNumber.Save();

                StringBuilder str = new StringBuilder();
                str.Append("<script language='javascript'>");
                str.Append("alert('Material Requisition " + lstMrNo + " submitted successfully!');");
                //str.Append("window.location.href = \"MaterialReq.aspx?CurrentLink=" + currentLink + "\"");
                str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + mrno + "\"");
                str.Append("</script>");
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
            }
        }

        protected void dgDeliveryData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgDeliveryData.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }

        protected void dgDeliveryData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgDeliveryData.EditItemIndex = -1;
            LoadData();
        }

        protected void dgDeliveryData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Products"));
                return;
            }

            HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidDeliveryID");
            if (hidID != null)
            {
                try
                {
                    GMSCore.Entity.MRDelivery md = new MRActivity().RetrieveDeliveryByID(session.CompanyId, hidID.Value.Trim());
                    if (md == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(upDelivery, this.GetType(), "click", "alert('This delivery record cannot be found in database.')", true);                       
                        return;
                    }
                  
                    TextBox txtEditETD = (TextBox)e.Item.FindControl("txtEditETD");
                    TextBox txtEditETA = (TextBox)e.Item.FindControl("txtEditETA");
                    TextBox txtEditCRD = (TextBox)e.Item.FindControl("txtEditCRD");               
                    md.ETD = GMSUtil.ToDate(txtEditETD.Text.Trim().ToString());
                    md.ETA = GMSUtil.ToDate(txtEditETA.Text.Trim().ToString());
                    md.CRD = GMSUtil.ToDate(txtEditCRD.Text.Trim().ToString());                   
                    md.ModifiedBy = loginUserOrAlternateParty;
                    md.ModifiedDate = DateTime.Now;
                    using (TransactionScope tran = new TransactionScope())
                    {
                        md.Save();
                        Resubmission("Delivery updated.", "Record modified successfully!","");
                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }

        }

        protected void dgDeliveryData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();  
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hidGRNNo = (HiddenField)e.Item.FindControl("hidGRNNo");
                HiddenField hidGRNTrnNo = (HiddenField)e.Item.FindControl("hidGRNTrnNo");
                Label lblGRNNo = (Label)e.Item.FindControl("lblGRNNo");               
                LinkButton lnkPONo = (LinkButton)e.Item.FindControl("lnkPONo");

                if (lnkPONo != null)
                {                    
                    ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                    if (sm != null)
                    {
                        sm.RegisterPostBackControl(lnkPONo);
                    }
                }

                string poNo = lnkPONo.Text.ToString();
                if (viewPurchaseInformationAccess != null || viewAllProductGroupPurchaseInformationAccess != null)
                { 
                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        if ((hidPMUserId.Value == loginUserOrAlternateParty.ToString() || hidPH.Value == loginUserOrAlternateParty.ToString() || hidPH2.Value == loginUserOrAlternateParty.ToString()) || (userRole == "Purchasing") || (viewAllProductGroupPurchaseInformationAccess != null))
                        {                            
                            lnkPONo.Attributes.Add("onclick", "openPopUp(\'ViewPO.aspx?MRNo=" + lblMRNo.Text.Trim() + "&TrnNo=" + poNo + "\'); ");
                        }
                        else
                        {                          
                           lnkPONo.Attributes.Add("onclick", "openPopUp(\'ViewPOWithoutSupplierInfo.aspx?MRNo=" + lblMRNo.Text.Trim() + "&TrnNo=" + poNo + "\'); ");                            
                        }
                    }
                }
                else
                {
                    lnkPONo.Attributes.Add("onclick", "openPopUp(\'ViewPOWithoutSupplierInfo.aspx?MRNo=" + lblMRNo.Text.Trim() + "&TrnNo=" + poNo + "\'); ");                   
                }        
   
                 IList<MRGRNDetail> lstGRNDetail1 =  new MRActivity().RetriveMRGRNDetailByPO(session.CompanyId, poNo);

                 IList<MRGRNDetail> lstGRNDetail = removeDuplicates(lstGRNDetail1);

                 string strGRNNo = "";
                 foreach (MRGRNDetail grnDetail in lstGRNDetail)
                 {
                     if (viewPurchaseInformationAccess != null || viewAllProductGroupPurchaseInformationAccess != null)
                     {
                         if (this.lblMRNo.Text.Trim() != "")
                         {
                             if ((hidPMUserId.Value == loginUserOrAlternateParty.ToString() || hidPH.Value == loginUserOrAlternateParty.ToString() || hidPH2.Value == loginUserOrAlternateParty.ToString()) || (userRole == "Purchasing") || (viewAllProductGroupPurchaseInformationAccess != null))
                             {
                                 strGRNNo += "<a href=\'ViewGRN.aspx?MRNo=" + lblMRNo.Text.Trim() + "&TrnNo=" + grnDetail.GRNNo + "\' target=\'_blank\'>" + grnDetail.GRNNo + "</a>/" + grnDetail.GRNTrnNo + " " + grnDetail.GRNDate.ToString("dd-MMM-yyyy") +"<br>";
                             }
                             else
                             {
                                 strGRNNo += "<a href=\'ViewGRNWithoutSupplierInfo.aspx?MRNo=" + lblMRNo.Text.Trim() + "&TrnNo=" + grnDetail.GRNNo + "\' target=\'_blank\'>" + grnDetail.GRNNo + "</a>/" + grnDetail.GRNTrnNo + " " + grnDetail.GRNDate.ToString("dd-MMM-yyyy") + "<br>";
                             }
                         }
                     }
                     else
                     {
                         strGRNNo += "<a href=\'ViewGRNWithoutSupplierInfo.aspx?MRNo=" + lblMRNo.Text.Trim() + "&TrnNo=" + grnDetail.GRNNo + "\' target=\'_blank\'>" + grnDetail.GRNNo + "</a>/" + grnDetail.GRNTrnNo + " " + grnDetail.GRNDate.ToString("dd-MMM-yyyy") + "<br>";
                     }

                 }
                                
                /*
                string[] arrGRNNo = hidGRNNo.Value.ToString().Split(',');
                string[] arrGRNTrnNo = hidGRNTrnNo.Value.ToString().Split(',');
                for (int i = 0; i < arrGRNNo.Length; i++)
                {
                    if (viewPurchaseInformationAccess != null || viewAllProductGroupPurchaseInformationAccess != null)
                    { 
                        if (this.lblMRNo.Text.Trim() != "")
                        {
                            if ((hidPMUserId.Value == loginUserOrAlternateParty.ToString() || hidPH.Value == loginUserOrAlternateParty.ToString() || hidPH2.Value == loginUserOrAlternateParty.ToString()) || (userRole == "Purchasing") || (viewAllProductGroupPurchaseInformationAccess != null))
                            {
                                strGRNNo += "<a href=\'ViewGRN.aspx?MRNo=" + lblMRNo.Text.Trim() + "&TrnNo=" + arrGRNNo[i] + "\' target=\'_blank\'>" + arrGRNNo[i] + "</a>/" + arrGRNTrnNo[i] + "<br>";
                            }
                            else
                            {                               
                                strGRNNo += "<a href=\'ViewGRNWithoutSupplierInfo.aspx?MRNo=" + lblMRNo.Text.Trim() + "&TrnNo=" + arrGRNNo[i] + "\' target=\'_blank\'>" + arrGRNNo[i] + "</a>/" + arrGRNTrnNo[i] + "<br>";
                            }
                        }
                    }
                    else
                    {
                        strGRNNo += "<a href=\'ViewGRNWithoutSupplierInfo.aspx?MRNo=" + lblMRNo.Text.Trim() + "&TrnNo=" + arrGRNNo[i] + "\' target=\'_blank\'>" + arrGRNNo[i] + "</a>/" + arrGRNTrnNo[i] + "<br>";
                    }
                }
                */

                lblGRNNo.Text = strGRNNo;
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                {
                    lnkDelete.Attributes.Add("onclick", "return confirm_delete();");
                }           
            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {    
                TextBox txtEditETD = (TextBox)e.Item.FindControl("txtEditETD");
                txtEditETD.Attributes.Add("readonly", "readonly");

                TextBox txtEditETA = (TextBox)e.Item.FindControl("txtEditETA");
                txtEditETA.Attributes.Add("readonly", "readonly");

                TextBox txtEditCRD = (TextBox)e.Item.FindControl("txtEditCRD");
                txtEditCRD.Attributes.Add("readonly", "readonly");
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {               
                TextBox txtNewETD = (TextBox)e.Item.FindControl("txtNewETD");
                txtNewETD.Attributes.Add("readonly", "readonly");

                TextBox txtNewETA = (TextBox)e.Item.FindControl("txtNewETA");
                txtNewETA.Attributes.Add("readonly", "readonly");

                TextBox txtNewCRD = (TextBox)e.Item.FindControl("txtNewCRD");
                txtNewCRD.Attributes.Add("readonly", "readonly");
            }
        }

        protected void dgDeliveryData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Products"));
                    return;
                }
                HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidDeliveryID");
                if (hidID != null)
                {
                    MRActivity maActivity = new MRActivity();
                    try
                    {
                        using (TransactionScope tran = new TransactionScope())
                        {

                            ResultType result = maActivity.DeleteMRDelivery(hidID.Value, session);

                            switch (result)
                            {
                                case ResultType.Ok:
                                    this.dgDeliveryData.EditItemIndex = -1;
                                    this.dgDeliveryData.CurrentPageIndex = 0;

                                    IList<MRDelivery> lstMRDelivery = null;
                                    lstMRDelivery = maActivity.RetrieveMRDeliveryByMRNo(GMSUtil.ToShort(session.CompanyId), lblMRNo.Text.Trim());
                                    if (lstMRDelivery.Count == 0)
                                    { // Update MRStatus to Approved if no PO in Delivery
                                        GMSCore.Entity.MR mr = new MRActivity().RetrieveMRByMRNo(session.CompanyId, lblMRNo.Text.Trim());
                                        mr.StatusID = "A";
                                        mr.Save();                                       
                                    }                                   
                                    LoadData();
                                    Resubmission("Delivery updated.", "Record deleted successfully!","");
                                    tran.Complete();
                                    break;
                                default:
                                    this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                    return;
                            }
                        }
                    }
                    catch (SqlException exSql)
                    {
                        if (exSql.Number == 547)
                        {
                            this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);                            
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

        protected void dgDeliveryData_Command(Object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            switch (((LinkButton)e.CommandSource).CommandName)
            {
                case "Create":
                    TextBox txtNewPO = (TextBox)e.Item.FindControl("txtNewPO");                    
                    TextBox txtNewETD = (TextBox)e.Item.FindControl("txtNewETD");
                    TextBox txtNewETA = (TextBox)e.Item.FindControl("txtNewETA");
                    TextBox txtNewCRD = (TextBox)e.Item.FindControl("txtNewCRD");
                    try 
                    {
                        //using (TransactionScope tran = new TransactionScope())
                        //{
                            GMSCore.Entity.MRDelivery existMRDelivery = new MRActivity().RetrieveMRDeliveryPOByMRNo(session.CompanyId, lblMRNo.Text.Trim().ToString(), txtNewPO.Text.Trim().ToString());
                            if (existMRDelivery == null)
                            {
                                GMSCore.Entity.MRDelivery md = new GMSCore.Entity.MRDelivery();                               
                                md.CoyID = session.CompanyId; 
                                md.MRNo = lblMRNo.Text;
                                md.PONo = txtNewPO.Text.Trim().ToString();
                                md.ETD = GMSUtil.ToDate(txtNewETD.Text.Trim().ToString());
                                md.ETA = GMSUtil.ToDate(txtNewETA.Text.Trim().ToString());
                                md.CRD = GMSUtil.ToDate(txtNewCRD.Text.Trim().ToString());
                                md.CreatedBy = loginUserOrAlternateParty;
                                md.CreatedDate = DateTime.Now;
                                md.Save();
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(upDelivery, this.GetType(), "click", "alert('This PO number exist, Please refresh the MR.')", true);                                                               
                                return;
                            }                                                

                            GMSCore.Entity.MR mr = new MRActivity().RetrieveMRByMRNo(session.CompanyId, lblMRNo.Text.Trim().ToString());
                            if (mr == null)
                            {
                                ScriptManager.RegisterClientScriptBlock(upDelivery, this.GetType(), "click", "alert('This material requisition cannot be found in database.')", true);
                                return;
                            }

                            if (mr.StatusID.ToString() == "C" || mr.StatusID.ToString() == "X")
                            {
                                ScriptManager.RegisterClientScriptBlock(upDelivery, this.GetType(), "click", "alert('This material requisition cannot be edited.')", true);
                                return;
                            }
                            mr.StatusID = "P";
                            mr.ModifiedBy = loginUserOrAlternateParty;
                            mr.ModifiedDate = DateTime.Now;                            
                            mr.Save();
                            Resubmission("Delivery updated.", "Record created successfully!","");
                            //tran.Complete();
                       // }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }                         
                            
                    break;
                default:
                    // Do nothing.
                    break;
            }
        }        

       protected void GeneratePDFReport(object sender, EventArgs e)
        {

            string selectedReport = ddlReport.SelectedValue;  
            ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "Report1", string.Format("jsOpenOperationalReport('/GMS3/Finance/BankFacilities/PdfReportViewer.aspx?REPORT={0}&&TRNNO=" + this.lblMRNo.Text.Trim() + "&&REPORTID=-4');", selectedReport), true);
             LoadData();
        }   
        
        #region txtAccountCode_OnTextChanged
        protected void txtAccountCode_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtAccountCode = (TextBox)sender;
            TableRow tr = (TableRow)txtAccountCode.Parent.Parent;
            TextBox txtNewAccountName = (TextBox)tr.FindControl("txtNewAccountName");
            LinkButton lnkCreate = (LinkButton)tr.FindControl("lnkCreate"); 

            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            if (txtAccountCode.Text.Trim() != "")
            {
                A21Account acct = A21Account.RetrieveByKey(session.CompanyId, txtAccountCode.Text.Trim());
                if (acct != null && acct.AccountType == "C")
                {
                    DataSet ds = new DataSet();
                    (new QuotationDataDALC()).GetCustomerInfoByAccountCode(session.CompanyId, txtAccountCode.Text.Trim(), ref ds);                    
                    txtAccountCode.Text = txtAccountCode.Text.ToUpper();
                    txtNewAccountName.Text = acct.AccountName;   
                }
                else
                {                   
                    ScriptManager.RegisterClientScriptBlock(upConfirmedSalesInformation, this.GetType(), "click", "alert('Account is not found!')", true);
                    txtAccountCode.Text = "";
                    scriptManager.SetFocus(txtAccountCode);  
                }
            }
            else
            {
                txtNewAccountName.Text = "";
                scriptManager.SetFocus(txtAccountCode);  
            }            
        }
        #endregion

        protected void btnUpdateFileUpload_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();            
            DataGrid dg = (DataGrid)TabPanel1.FindControl("dgConfirmedSalesData");
            if (dg != null && dg.Controls.Count > 0)
            {
                FileUpload FileUpload1 = (FileUpload)dg.Controls[0].Controls[dg.Items.Count + 1].FindControl("FileUpload1");
                Label uploadStr = (Label)dg.Controls[0].Controls[dg.Items.Count + 1].FindControl("lblUpload");

                if (FileUpload1.HasFile)
                {
                    try
                    {
                        string folderPath = System.Web.Configuration.WebConfigurationManager.AppSettings["MR_DOWNLOAD_PATH"].ToString() + session.CompanyId;
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string c = FileUpload1.FileName.ToString(); // We don't need the path, just the name.
                        if(c.Length > 99)
                            c = c.Substring(0, 99);
                        string randomIDFileName = loginUserOrAlternateParty.ToString() + DateTime.Now.Ticks.ToString() + FileUpload1.FileName.ToString();
                        if (randomIDFileName.Length > 99)
                            randomIDFileName = randomIDFileName.Substring(0, 99);
                        FileUpload1.SaveAs(folderPath + "/" + randomIDFileName);
                        Session["FileName"] = c;
                        Session["FileNameEncrypted"] = randomIDFileName;
                        uploadStr.Text = FileUpload1.FileName.ToString();                       
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }   
                }  
            }
        }
        
        protected void btnLoadSales_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();            
            if (this.lblMRNo.Text.Trim() != "")
            {
                string mrNo = lblMRNo.Text.Trim();
                DataSet ds = new DataSet();

                new GMSGeneralDALC().GetMaterialRequisitionByMRNo(session.CompanyId, mrNo, ref ds);
                // Load Confirm Sales Information Data
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    ViewState["SortDirection"] = "ASC";
                    ViewState["SortField"] = "CustomerAccountCode";
                    int startIndex = ((dgConfirmedSalesData.CurrentPageIndex + 1) * this.dgConfirmedSalesData.PageSize) - (this.dgConfirmedSalesData.PageSize - 1);
                    int endIndex = (dgConfirmedSalesData.CurrentPageIndex + 1) * this.dgConfirmedSalesData.PageSize;
                    DataView dv = ds.Tables[1].DefaultView;
                    dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
                    if (ds != null && ds.Tables[1].Rows.Count > 0)
                    {
                        if (endIndex < ds.Tables[1].Rows.Count)
                            this.lblPurchaseSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                endIndex.ToString() + " " + "of" + " " + ds.Tables[1].Rows.Count.ToString();
                        else
                            this.lblPurchaseSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                                ds.Tables[1].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[1].Rows.Count.ToString();
                        this.lblPurchaseSummary.Visible = true;
                    }
                    else
                    {
                        this.lblPurchaseSummary.Text = "No records.";
                        this.lblPurchaseSummary.Visible = true;
                    }

                    this.dgConfirmedSalesData.DataSource = dv;
                    this.dgConfirmedSalesData.DataBind();
                    this.dgConfirmedSalesData.Visible = true;
                }
            }
            else
            {
                if (Session["ConfirmedSalesInformation"] != null)
                {
                    workTable = (DataTable)Session["ConfirmedSalesInformation"];
                    this.dgConfirmedSalesData.DataSource = workTable;
                    this.dgConfirmedSalesData.DataBind();
                    this.dgConfirmedSalesData.Visible = true;
                }
                else
                {
                    DataSet ds = null;
                    ds = new DataSet();
                    ds.Tables.Add("SalesInformation");
                    this.dgConfirmedSalesData.DataSource = ds.Tables["SalesInformation"];
                    this.dgConfirmedSalesData.DataBind();
                    this.dgConfirmedSalesData.Visible = true;
                }
            }            
        }      
        
        #region GenerateReport
        protected void GenerateReport(object sender, EventArgs e)
        {        
            string selectedReport = ddlReport.SelectedValue;
            ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "Report1", string.Format("jsOpenOperationalReport('/GMS3/Finance/BankFacilities/ReportViewer.aspx?REPORT={0}&&TRNNO=" + this.lblMRNo.Text.Trim() + "&&REPORTID=-4');", selectedReport), true);
            LoadData();            
            ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "Report1", string.Format("SendAttach();", selectedReport), true);
        }
        #endregion

        #region GetDynamicContent
        [WebMethod]
        public static string GetDynamicContent(string contextKey)
        {
            short coyId = 1;
            string prodCode = "";
            string[] str = contextKey.Split(';');
            if (str != null && str.Length == 2)
            {
                coyId = GMSUtil.ToShort(str[0].Trim());
                prodCode = str[1].Trim();
            }

            DataSet ds = new DataSet();
            GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
            Company coy = Company.RetrieveByKey(coyId);
            if (coy.WebServiceAddress != null && coy.WebServiceAddress.Trim() != "")
            {
                sc.Url = coy.WebServiceAddress.Trim();
            }
            else
                sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
            ds = sc.GetProductStockStatus(coyId, prodCode);

            DataSet ds2 = new DataSet();
            ds2 = sc.GetProductDetailByProductCode(coyId, prodCode);

            StringBuilder b = new StringBuilder();         
                       
            b.Append("<table class='tInfoTable'>");           
            b.Append("<tr><td colspan='2' style='background-color:#FAD696;'>");
            b.Append("<b><i>Stock Status For Product " + prodCode + "</i></b>");
            b.Append("</td></tr>");

            if ((ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) || (ds2 != null && ds.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0))
            {
                b.Append("<tr><td style='width:250px;'><i>Warehouse</i></td>");
                b.Append("<td><i>Quantity</i></td></tr>");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    b.Append("<tr>");
                    b.Append("<td><i>" + dr["Warehouse"].ToString() + " - " + dr["WarehouseName"].ToString() + "</i></td>");
                    b.Append("<td><i>" + dr["Quantity"].ToString() + "</i></td>");
                    b.Append("</tr>");
                }

                if (ds2 != null && ds.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    b.Append("<tr>");
                    b.Append("<td><i>On SO</i></td>");
                    b.Append("<td><i>" + ds2.Tables[0].Rows[0]["OnOrderQuantity"].ToString() + "</i></td>");
                    b.Append("</tr>");
                    b.Append("<tr>");
                    b.Append("<td><i>On PO</i></td>");
                    b.Append("<td><i>" + ds2.Tables[0].Rows[0]["OnPOQuantity"].ToString() + "</i></td>");
                    b.Append("</tr>");
                }
            }
            else
            {
                b.Append("<tr>");
                b.Append("<td colspan='2'><i>Not Avaiable.</i></td>");
                b.Append("</tr>");
            }

            b.Append("</table>");

            return b.ToString();
        }
        #endregion              

        protected void btnSubmitForApproval_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
           

            IList<MRVendor> lstAutoInsertedMRVendor = new MRActivity().RetrieveAutoInsertedVendorByMRNo(GMSUtil.ToShort(session.CompanyId), this.lblMRNo.Text.Trim());
            if ((hidPMUserId.Value.ToString() == loginUserOrAlternateParty.ToString() || hidPH.Value.ToString() == loginUserOrAlternateParty.ToString() || hidPH3.Value.ToString() == loginUserOrAlternateParty.ToString()) && (lstAutoInsertedMRVendor.Count > 0))
            {
                str.Append("alert('Please click on [Confirm Vendor Information] button on the Vendor Tab to confirm the Vendor Information!');");
                str.Append("window.location.href = \"NewMaterialRequisition.aspx?ActiveTab=1&CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                
            }
            else
            {
                new ApprovalActivity().InsertApprovaLevelInfoList(session.CompanyId, loginUserOrAlternateParty, this.lblMRNo.Text.Trim(), GMSUtil.ToShort(hidPMUserId.Value), GMSUtil.ToShort(hidPH.Value), GMSUtil.ToShort(hidPH3.Value), session.UserId);
                MR mr = new MRActivity().RetrieveMRByMRNo(session.CompanyId, this.lblMRNo.Text.Trim());
                if (mr != null)
                {
                    mr.StatusID = "F";
                    mr.Save();
                }
                str.Append("alert('MR has been submitted for approval!');");
                str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");                
            }

            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }

        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();    

            DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(session.CompanyId, (short)DateTime.Now.Year);
            string mrno = "MR" + DateTime.Now.ToString("yy") + "" + documentNumber.MRNo.ToString();

            string oldMRNo = lblMRNo.Text.Trim();

            using (TransactionScope tran = new TransactionScope())
            {
                new GMSGeneralDALC().DuplicateMR(GMSUtil.ToByte(session.CompanyId.ToString()), oldMRNo, mrno, GMSUtil.ToShort(session.UserId));

                string nxtStr = ((short)(short.Parse(documentNumber.MRNo) + 1)).ToString();
                for (int i = nxtStr.Length; i < documentNumber.MRNo.Length; i++)
                {
                    nxtStr = "0" + nxtStr;
                }
                documentNumber.MRNo = nxtStr;
                documentNumber.Save();
                tran.Complete();

                StringBuilder str = new StringBuilder();
                str.Append("<script language='javascript'>");
                str.Append("alert('Material Requisition " + mrno + " created successfully!');");
                str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + mrno + "\"");
                str.Append("</script>");
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);

            }
            
           
        }
        

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();            
            if (this.lblMRNo.Text.Trim() != "")
            {
                try
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");

                    string projectCostMessage = "";

                    // Project MR for CMS Project
                    if (txtProjectNo.Text.Trim() != "" && session.DivisionId == 4)
                    {
                        DataSet ds = new DataSet();

                        CMSWebService.CMSWebService sc = new CMSWebService.CMSWebService();
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/CMS.WebServices/Recipe.asmx";
                        ds = sc.GetProjectCostEstimate(txtProjectNo.Text.Trim());

                        if (ds != null && ds.Tables[0].Rows.Count == 0)
                        {
                            projectCostMessage = "Invalid Project No. Please check.";
                        }
                        else if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            DataSet dsPurchasePrice = new DataSet();
                            new GMSGeneralDALC().GetProjectPurchasePrice(session.CompanyId, txtProjectNo.Text.Trim(), ds.Tables[0].Rows[0]["DefaultCurrency"].ToString(), ref dsPurchasePrice);

                            if (dsPurchasePrice != null && dsPurchasePrice.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsPurchasePrice.Tables[0].Rows.Count; i++)
                                {
                                    if (GMSUtil.ToDouble(dsPurchasePrice.Tables[0].Rows[i]["TotalPurchasePrice"].ToString()) > GMSUtil.ToDouble(ds.Tables[0].Rows[i]["CostEstimate"].ToString()))
                                    {
                                        projectCostMessage = "Total MR purchase for Project " + txtProjectNo.Text.Trim() + " is over 80% of Total Project Estimate cost. Please check!";
                                    }
                                }
                            }
                        }
                    }



                    IList<MRVendor> lstAutoInsertedMRVendor = new MRActivity().RetrieveAutoInsertedVendorByMRNo(GMSUtil.ToShort(session.CompanyId), this.lblMRNo.Text.Trim());
                    IList<MRDetail> lstMRDetail = new MRActivity().RetrieveInvalidOrderQtyMRDetailsByCoyIDMRNo(session.CompanyId, this.lblMRNo.Text.Trim());
                    
                    if (lstMRDetail.Count > 0)
                    {
                        str.Append("alert('Please ensure the Order Qty is greater than zero.');");                        
                    }
                    else if (lstAutoInsertedMRVendor.Count > 0)
                    {
                        str.Append("alert('Please click on [Confirm Vendor Information] button on the Vendor Tab to confirm the Vendor Information!');");
                        str.Append("window.location.href = \"NewMaterialRequisition.aspx?ActiveTab=1&CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                    }
                    else
                    {
                        string status = new ApprovalActivity().ApproveMR(levelID, ddlPurchaser.SelectedValue.ToString(), session.UserId);
                        if (status == "OK")
                        {
                            str.Append("alert('" + projectCostMessage + " MR approval request has been approved! ');");
                            str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                        }
                        else
                        {
                            str.Append("alert('No approval info');");
                        }
                    }
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);           
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
        }
        /*
        protected void btnReject_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (this.lblMRNo.Text.Trim() != "")
            {
                try
                {
                    string status = new ApprovalActivity().RejectMR(session.CompanyId,this.lblMRNo.Text.Trim(), levelID, "",session.UserId);                    
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    if (status == "OK")
                    {
                        str.Append("alert('MR approval request has been rejected!');");
                        str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                    }
                    else
                    {
                        str.Append("alert('No approval info');");
                    }
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
        }

        */
        /*

        protected void btnHighLevelApprove_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();                    

            if (this.lblMRNo.Text.Trim() != "")
            {
                try
                {                
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");

                    IList<MRVendor> lstAutoInsertedMRVendor = new MRActivity().RetrieveAutoInsertedVendorByMRNo(GMSUtil.ToShort(session.CompanyId), this.lblMRNo.Text.Trim());
                    IList<MRDetail> lstMRDetail = new MRActivity().RetrieveInvalidOrderQtyMRDetailsByCoyIDMRNo(session.CompanyId, this.lblMRNo.Text.Trim());
                    if (lstMRDetail.Count > 0)
                    {
                        str.Append("alert('Please ensure the Order Qty is greater than zero.');");
                    }
                    else if (lstAutoInsertedMRVendor.Count > 0)
                    {
                        str.Append("alert('Please click on [Confirm Vendor Information] button on the Vendor Tab to confirm the Vendor Information!');");
                        str.Append("window.location.href = \"NewMaterialRequisition.aspx?ActiveTab=1&CurrentLink=" + currentLink + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                    }
                    else
                    {
                        string status = new ApprovalActivity().HighLevelApproveMR(session.CompanyId, this.lblMRNo.Text.Trim(), levelID, ddlPurchaser.SelectedValue.ToString(), txtApproveReason.Text.ToString(), session.UserId);
                        if (status == "OK")
                        {
                            str.Append("alert('MR approval request has been approved!');");
                            str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                        }                        
                        else if (status == "")
                        {
                            str.Append("alert('No approval info');");
                        }
                    }
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
        }
        
        */

        /*
        protected void btnHighLevelReject_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo(); 
            if (this.lblMRNo.Text.Trim() != "")
            {
                try
                {
                    string status = new ApprovalActivity().HighLevelRejectMR(session.CompanyId,this.lblMRNo.Text.Trim(),levelID, "");
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    if (status == "OK")
                    {
                        str.Append("alert('MR approval request has been rejected!');");
                        str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                    }
                    else
                    {
                        str.Append("alert('No approval info');");
                    }                    
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            } 
        }
        */     

        protected void btnUndoCancel_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (this.lblMRNo.Text.Trim() != "")
            {
                    string status  = new ApprovalActivity().UndoCancellation(session.CompanyId, loginUserOrAlternateParty, this.lblMRNo.Text.Trim(), session.UserId);                       
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    if (status == "OK")
                    {
                        str.Append("alert('MR status has been reverted back to Draft!');");
                        str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                    }
                    else
                    {
                        str.Append("alert('This MR has been modified. Please refresh this MR.');");
                    }
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
            }           
        }
                
        protected void dgMRFormApproval_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            HiddenField currentLevel = (HiddenField)e.Item.FindControl("hidCurrentLevel");
            if (currentLevel != null)
            {
                if (currentLevel.Value == "Y")
                {
                    e.Item.Cells[0].BackColor = System.Drawing.ColorTranslator.FromHtml("#FDBE70");
                    e.Item.Cells[1].BackColor = System.Drawing.ColorTranslator.FromHtml("#FDBE70");
                    e.Item.Cells[2].BackColor = System.Drawing.ColorTranslator.FromHtml("#FDBE70");
                    e.Item.Cells[3].BackColor = System.Drawing.ColorTranslator.FromHtml("#FDBE70");
                    e.Item.Cells[4].BackColor = System.Drawing.ColorTranslator.FromHtml("#FDBE70");
                    e.Item.Cells[5].BackColor = System.Drawing.ColorTranslator.FromHtml("#FDBE70");
                    e.Item.Cells[0].Font.Bold = true;
                    e.Item.Cells[1].Font.Bold = true;
                    e.Item.Cells[2].Font.Bold = true;
                    e.Item.Cells[3].Font.Bold = true;
                    e.Item.Cells[4].Font.Bold = true;
                    e.Item.Cells[5].Font.Bold = true;                    
                }
            }
        }

        protected void CancelCommand(object sender, CommandEventArgs e)
        {

            if (txtCancellationReason.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('Please key in cancellation reason!')", true);
            }
            else
            {
                LogSession session = base.GetSessionInfo();
                if (this.lblMRNo.Text.Trim() != "")
                {
                    try
                    {
                        if (btnHighLevelCancel.Visible)
                            new ApprovalActivity().HighLevelCancelMR(session.CompanyId, loginUserOrAlternateParty, this.lblMRNo.Text.Trim(), levelID, this.txtCancellationReason.Text.Trim(), session.UserId);
                        else if (btnCancel.Visible)
                            new ApprovalActivity().CancelMR(session.CompanyId, loginUserOrAlternateParty, this.lblMRNo.Text.Trim(), levelID, this.txtCancellationReason.Text.Trim(), session.UserId);
                        StringBuilder str = new StringBuilder();
                        str.Append("<script language='javascript'>");
                        str.Append("alert('MR approval request has been cancelled!');");
                        str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                        str.Append("</script>");
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }
                }
            }
        }
        
        protected void ApproveCommand(object sender, CommandEventArgs e)
        {
            if (txtApproveReason.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('Please key in the reason for approving this MR!')", true);
            }
            else
            {
                LogSession session = base.GetSessionInfo();

                if (this.lblMRNo.Text.Trim() != "")
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("<script language='javascript'>");


                        string projectCostMessage = "";

                        // Project MR for CMS Project
                        if (txtProjectNo.Text.Trim() != "" && session.DivisionId == 4)
                        {
                            DataSet ds = new DataSet();

                            CMSWebService.CMSWebService sc = new CMSWebService.CMSWebService();
                            if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                            {
                                sc.Url = session.CMSWebServiceAddress.Trim();
                            }
                            else
                                sc.Url = "http://localhost/CMS.WebServices/Recipe.asmx";
                            ds = sc.GetProjectCostEstimate(txtProjectNo.Text.Trim());

                            if (ds != null && ds.Tables[0].Rows.Count == 0)
                            {
                                projectCostMessage = "Invalid Project No. Please check.";
                            }
                            else if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                DataSet dsPurchasePrice = new DataSet();
                                new GMSGeneralDALC().GetProjectPurchasePrice(session.CompanyId, txtProjectNo.Text.Trim(), ds.Tables[0].Rows[0]["DefaultCurrency"].ToString(), ref dsPurchasePrice);

                                if (dsPurchasePrice != null && dsPurchasePrice.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsPurchasePrice.Tables[0].Rows.Count; i++)
                                    {
                                        if (GMSUtil.ToDouble(dsPurchasePrice.Tables[0].Rows[i]["TotalPurchasePrice"].ToString()) > GMSUtil.ToDouble(ds.Tables[0].Rows[i]["CostEstimate"].ToString()))
                                        {
                                            projectCostMessage = "Total MR purchase for Project " + txtProjectNo.Text.Trim() + " is over 80% of Total Project Estimate cost. Please check!";
                                        }
                                    }
                                }
                            }
                        }

                     
                    IList<MRVendor> lstAutoInsertedMRVendor = new MRActivity().RetrieveAutoInsertedVendorByMRNo(GMSUtil.ToShort(session.CompanyId), this.lblMRNo.Text.Trim());

                    if (lstAutoInsertedMRVendor.Count > 0)
                    {
                        str.Append("alert('Please click on [Confirm Vendor Information] button on the Vendor Tab to confirm the Vendor Information!');");
                        str.Append("window.location.href = \"NewMaterialRequisition.aspx?ActiveTab=1&CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                    }
                    else
                    {
                        string status = new ApprovalActivity().HighLevelApproveMR(session.CompanyId, this.lblMRNo.Text.Trim(), levelID, ddlPurchaser.SelectedValue.ToString(), txtApproveReason.Text.ToString(), session.UserId);
                       

                        if (status == "OK")
                        {
                            str.Append("alert('" + projectCostMessage + " .MR approval request has been approved! ');");
                            str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                        }
                        else if (status == "OrderQty Error")
                        {
                            str.Append("alert('Please ensure the Order Qty is greater than zero.');");
                        }
                        else if (status == "")
                        {
                            str.Append("alert('No approval info');");
                        }
                        
                    }
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }

                }
            }
        }
        

        protected void RejectCommand(object sender, CommandEventArgs e)
        {
            if (txtRejectReason.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(upOutter, this.GetType(), "click", "alert('Please enter reject reason!')", true);
            }
            else
            {
                LogSession session = base.GetSessionInfo();
                if (this.lblMRNo.Text.Trim() != "")
                {
                    try
                    {
                        if(btnHighLevelReject.Visible)
                            new ApprovalActivity().HighLevelRejectMR(session.CompanyId, this.lblMRNo.Text.Trim(), levelID, txtRejectReason.Text.Trim(), session.UserId);
                        else if (btnReject.Visible)
                            new ApprovalActivity().RejectMR(session.CompanyId, this.lblMRNo.Text.Trim(), levelID, txtRejectReason.Text.Trim(), session.UserId);

                        StringBuilder str = new StringBuilder();
                        str.Append("<script language='javascript'>");
                        str.Append("alert('MR approval request has been rejected!');");
                        str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                        str.Append("</script>");
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }

                }

            }

        }

        #region dgMRFormApproval datagrid PageIndexChanged event handling
        protected void dgMRFormApproval_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion     

        protected void ddlNewPM_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            DropDownList ddlNewPM = (DropDownList)sender;           
            TableRow tr = (TableRow)ddlNewPM.Parent.Parent;
            DropDownList ddlNewPH = (DropDownList)tr.FindControl("ddlNewPH");
            DropDownList ddlNewPH3 = (DropDownList)tr.FindControl("ddlNewPH3");

           if (loginUserOrAlternateParty.ToString() == (ddlNewPM.SelectedValue))
            {               
                TextBox txtNewUnitPurchasePrice = (TextBox)tr.FindControl("txtNewUnitPurchasePrice");
                DropDownList ddlNewPPCurrency = (DropDownList)tr.FindControl("ddlNewPPCurrency");
                txtNewUnitPurchasePrice.Enabled = true;
                ddlNewPPCurrency.Enabled = true;
            }
            /*
            DataSet lstPMPHPH3 = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionPMPHPH3ByCoyID(session.CompanyId, GMSUtil.ToShort(ddlNewPM.SelectedValue), GMSUtil.ToShort(ddlNewPH.SelectedValue), GMSUtil.ToShort(ddlNewPH3.SelectedValue), ref lstPMPHPH3);

            ddlNewPH.DataSource = lstPMPHPH3.Tables[1];
            ddlNewPH.DataValueField = "UserNumId";
            ddlNewPH.DataTextField = "UserRealName";
           
            if (lstPMPHPH3.Tables[1].Rows.Count > 2)               
                ddlNewPH.SelectedValue = "0";
            else if (lstPMPHPH3.Tables[1].Rows.Count == 2)
            {
                DataView dv = lstPMPHPH3.Tables[1].DefaultView;
                dv.Sort = "UserNumId DESC";
            }
            ddlNewPH.DataBind();

          
            ddlNewPH3.DataSource = lstPMPHPH3.Tables[2];
            ddlNewPH3.DataValueField = "UserNumId";
            ddlNewPH3.DataTextField = "UserRealName";
            ddlNewPH3.DataBind();

            if (lstPMPHPH3.Tables[2].Rows.Count > 2)
                ddlNewPH3.SelectedValue = "0";
            else if (lstPMPHPH3.Tables[2].Rows.Count == 2)
            {
                DataView dv = lstPMPHPH3.Tables[2].DefaultView;
                dv.Sort = "UserNumId DESC";
            }
            ddlNewPH3.DataBind();     
             */


        }

        protected void ddlNewPH_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            DropDownList ddlNewPH = (DropDownList)sender;
            TableRow tr = (TableRow)ddlNewPH.Parent.Parent;
            DropDownList ddlNewPM = (DropDownList)tr.FindControl("ddlNewPM");
            DropDownList ddlNewPH3 = (DropDownList)tr.FindControl("ddlNewPH3");

            if (loginUserOrAlternateParty.ToString() == (ddlNewPH.SelectedValue))
            {               
                TextBox txtNewUnitPurchasePrice = (TextBox)tr.FindControl("txtNewUnitPurchasePrice");
                DropDownList ddlNewPPCurrency = (DropDownList)tr.FindControl("ddlNewPPCurrency");
                txtNewUnitPurchasePrice.Enabled = true;
                ddlNewPPCurrency.Enabled = true;
            }           

        }

        protected void ddlNewPH3_SelectedIndexChanged(object sender, EventArgs e)
        {

            LogSession session = base.GetSessionInfo();
            DropDownList ddlNewPH3 = (DropDownList)sender;
            TableRow tr = (TableRow)ddlNewPH3.Parent.Parent;
            
            if (loginUserOrAlternateParty.ToString() == (ddlNewPH3.SelectedValue))
            {
                TextBox txtNewUnitPurchasePrice = (TextBox)tr.FindControl("txtNewUnitPurchasePrice");
                DropDownList ddlNewPPCurrency = (DropDownList)tr.FindControl("ddlNewPPCurrency");
                txtNewUnitPurchasePrice.Enabled = true;
                ddlNewPPCurrency.Enabled = true;
            }           
        }

        protected void ddlEditPM_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            DropDownList ddlEditPM = (DropDownList)sender;
            short ddlPM = GMSUtil.ToShort(ddlEditPM.SelectedValue);

            if (loginUserOrAlternateParty.ToString() == ddlPM.ToString())
            {
                TableRow tr = (TableRow)ddlEditPM.Parent.Parent;
                TextBox txtEditUnitPurchasePrice = (TextBox)tr.FindControl("txtEditUnitPurchasePrice");
                DropDownList ddlEditPPCurrency = (DropDownList)tr.FindControl("ddlEditPPCurrency");
                txtEditUnitPurchasePrice.Enabled = true;
                ddlEditPPCurrency.Enabled = true;
            }

        }

        protected void ddlRequestor_SelectedIndexChanged(object sender, EventArgs e)
        { 
            LogSession session = base.GetSessionInfo();
            short requestor = GMSUtil.ToShort(ddlRequestor.SelectedValue);

            if (coy.MRScheme.ToString() != "Product")
            {
                DataSet lstRequestorPMPH = new DataSet();
                new GMSGeneralDALC().GetMaterialRequisitionPMPHRequestorByCoyID(session.CompanyId, requestor, session.MRScheme ,ref lstRequestorPMPH);

                if (lstRequestorPMPH.Tables[1].Rows[0]["UserRealName"].ToString() != "")
                {
                    lblPM.Text = lstRequestorPMPH.Tables[1].Rows[0]["UserRealName"].ToString();
                    hidPMUserId.Value = lstRequestorPMPH.Tables[1].Rows[0]["UserNumId"].ToString();
                }
                else
                    lblPM.Text = "NIL";

                if (lstRequestorPMPH.Tables[2].Rows[0]["UserRealName"].ToString() != "")
                {
                    lblPH.Text = lstRequestorPMPH.Tables[2].Rows[0]["UserRealName"].ToString();
                    hidPH.Value = lstRequestorPMPH.Tables[2].Rows[0]["UserNumId"].ToString();
                }
                else
                    lblPH.Text = "NIL";

                if (lstRequestorPMPH.Tables[3].Rows[0]["UserRealName"].ToString() != "")
                {
                    lblPH3.Text = lstRequestorPMPH.Tables[3].Rows[0]["UserRealName"].ToString();
                    hidPH3.Value = lstRequestorPMPH.Tables[3].Rows[0]["UserNumId"].ToString();
                }
                else
                    lblPH3.Text = "NIL";

                lblPM.Visible = true;
                lblPH.Visible = true;
                lblPH3.Visible = true;

            }



        }

        protected void ddlEditPH_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            DropDownList ddlEditPH = (DropDownList)sender;
            short ddlPH = GMSUtil.ToShort(ddlEditPH.SelectedValue);

            if (loginUserOrAlternateParty.ToString() == ddlPH.ToString())
            {
                TableRow tr = (TableRow)ddlEditPH.Parent.Parent;
                TextBox txtEditUnitPurchasePrice = (TextBox)tr.FindControl("txtEditUnitPurchasePrice");
                DropDownList ddlEditPPCurrency = (DropDownList)tr.FindControl("ddlEditPPCurrency");
                txtEditUnitPurchasePrice.Enabled = true;
                ddlEditPPCurrency.Enabled = true;
            }

        }

        protected void ddlEditPH3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnConfirmVendorInfo_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            IList<MRVendor> lstAutoInsertedMRVendor = null;
            lstAutoInsertedMRVendor = new MRActivity().RetrieveAutoInsertedVendorByMRNo(GMSUtil.ToShort(session.CompanyId), this.lblMRNo.Text.Trim());

            if (lstAutoInsertedMRVendor.Count > 0)
            {
                foreach (MRVendor vendor in lstAutoInsertedMRVendor)
                {
                    vendor.MRSupplierID = null;
                    vendor.Save();
                }

                StringBuilder str = new StringBuilder();
                str.Append("<script language='javascript'>");
                str.Append("alert('Vendor information has been confirmed!');");
                str.Append("window.location.href = \"NewMaterialRequisition.aspx?ActiveTab=1&CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                str.Append("</script>");
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);                    
           }           
                
        }

        protected void dgAttachmentData_Command(Object sender, DataGridCommandEventArgs e)
        {
            
            LogSession session = base.GetSessionInfo();
            string ext = Path.GetExtension(e.CommandArgument.ToString());
            string ContentType = "";
            FileUpload FileUpload1 = (FileUpload)e.Item.FindControl("FileUpload1");
            switch (((LinkButton)e.CommandSource).CommandName)
            {
                case "Load":
                    if (ext == ".asf")
                        ContentType = "video/x-ms-asf";
                    else if (ext == ".avi")
                        ContentType = "video/avi";
                    else if (ext == ".doc")
                        ContentType = "application/msword";
                    else if (ext == ".zip")
                        ContentType = "application/zip";
                    else if (ext == ".xls")
                        ContentType = "application/vnd.ms-excel";
                    else if (ext == ".gif")
                        ContentType = "image/gif";
                    else if (ext == ".jpg" || ext == "jpeg")
                        ContentType = "image/jpeg";
                    else if (ext == ".wav")
                        ContentType = "audio/wav";
                    else if (ext == ".mp3")
                        ContentType = "audio/mpeg3";
                    else if (ext == ".mpg" || ext == "mpeg")
                        ContentType = "video/mpeg";
                    else if (ext == ".mp3")
                        ContentType = "audio/mpeg3";
                    else if (ext == ".rtf")
                        ContentType = "application/rtf";
                    else if (ext == ".htm" || ext == "html")
                        ContentType = "text/html";
                    else if (ext == ".asp")
                        ContentType = "text/asp";
                    else
                        ContentType = "application/octet-stream";
                    Response.ContentType = ContentType.ToString();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + e.CommandArgument.ToString());
                    Response.TransmitFile(System.Web.Configuration.WebConfigurationManager.AppSettings["MR_ATTACHMENT_DOWNLOAD_PATH"].ToString() + session.CompanyId + "/" + e.CommandArgument.ToString());
                    Response.End();
                    break;
                case "Create":                   
                    Label lblUpload = (Label)e.Item.FindControl("lblUpload");
                    TextBox txtNewFileName = (TextBox)e.Item.FindControl("txtNewFileName");
                    if ((Session["FileNameAttachment"] == null) || (Session["FileNameEncryptedAttachment"] == null) || (Session["FileNameAttachment"].ToString() == "") || (Session["FileNameEncryptedAttachment"].ToString() == ""))
                    {
                        ScriptManager.RegisterClientScriptBlock(upConfirmedSalesInformation, this.GetType(), "click", "alert('Please select a file to upload!')", true);
                        return;
                    }
                    

                    string strMrNo = "0";
                    if (this.lblMRNo.Text.Trim() != "")
                        strMrNo = this.lblMRNo.Text.Trim();

                    
                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        GMSCore.Entity.MRAdditionalAttachment ma = new GMSCore.Entity.MRAdditionalAttachment();
                        ma.CoyID = session.CompanyId;
                        ma.MRNo = lblMRNo.Text;
                        ma.FileDisplayName = txtNewFileName.Text.Trim();
                        ma.CreatedBy = loginUserOrAlternateParty;                       
                        ma.CreatedDate = DateTime.Now;
                        ma.FileName = Session["FileNameAttachment"].ToString();
                        ma.FileNameEncrypted = Session["FileNameEncryptedAttachment"].ToString();
                        try
                        {
                            using (TransactionScope tran = new TransactionScope())
                            {
                                ma.Save();
                                Session.Remove("FileNameAttachment");
                                Session.Remove("FileNameEncryptedAttachment");                               

                                StringBuilder str = new StringBuilder();
                                str.Append("<script language='javascript'>");
                                str.Append("alert('Record modified successfully!');");
                                str.Append("window.location.href = \"NewMaterialRequisition.aspx?ActiveTab=4&CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");                                
                                str.Append("</script>");
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);

                                LoadData();
                                tran.Complete();
                            }
                        }
                        catch (Exception ex)
                        {
                            this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }
                    }
                    else
                    {
                        DataTable workTable = null;
                        if (Session["Attachment"] != null)
                        {
                            workTable = (DataTable)Session["Attachment"];
                        }
                        else
                        {
                            workTable = new DataTable("Attachment");
                            DataColumn column = new DataColumn();
                            column.ColumnName = "FileID";
                            column.DataType = System.Type.GetType("System.Int32");
                            column.AutoIncrement = true;
                            column.AutoIncrementSeed = 1;
                            column.AutoIncrementStep = 1;
                            workTable.Columns.Add(column);                           
                            workTable.Columns.Add("FileName", typeof(String));
                            workTable.Columns.Add("FileDisplayName", typeof(String));
                            workTable.Columns.Add("FileNameEncrypted", typeof(String));
                            
                        }

                        DataRow workRow = workTable.NewRow();                        
                        workRow["FileName"] = Session["FileNameAttachment"].ToString();
                        workRow["FileDisplayName"] = txtNewFileName.Text.Trim();
                        workRow["FileNameEncrypted"] = Session["FileNameEncryptedAttachment"].ToString();
                        workTable.Rows.Add(workRow);
                        Session["Attachment"] = workTable;
                        Session.Remove("FileNameAttachment");
                        Session.Remove("FileNameEncryptedAttachment");

                    }
                    LoadData();
                    Tabs.ActiveTabIndex = 4;
                    break;
                default:
                    // Do nothing.
                    break;
            }
            


        }

        protected void dgAttachmentData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Products"));
                    return;
                }

                HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidFileID");
                if (hidID != null)
                {
                    if (this.lblMRNo.Text.Trim() != "")
                    {
                        MRActivity maActivity = new MRActivity();
                        try
                        {
                           
                                using (TransactionScope tran = new TransactionScope())
                                {
                                    ResultType result = maActivity.DeleteMRAttachment(hidID.Value, session);
                                    switch (result)
                                    {
                                        case ResultType.Ok:
                                            this.dgAttachmentData.EditItemIndex = -1;
                                            this.dgAttachmentData.CurrentPageIndex = 0;
                                            LoadData();
                                            StringBuilder str = new StringBuilder();
                                            str.Append("<script language='javascript'>");
                                            str.Append("alert('Record deleted successfully!');");
                                            str.Append("window.location.href = \"NewMaterialRequisition.aspx?CurrentLink=" + currentLink + "&CoyID=" + coy_id + "&MRNo=" + this.lblMRNo.Text.Trim() + "\"");
                                            str.Append("</script>");
                                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);

                                            tran.Complete();
                                            break;
                                        default:
                                            this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                            return;
                                    }
                                }
                            
                            
                        }
                        catch (SqlException exSql)
                        {
                            if (exSql.Number == 547)
                            {
                                LoadData();
                                this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }
                    }
                    else
                    {
                        workTable = (DataTable)Session["ConfirmedSalesInformation"];
                        for (int i = workTable.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = workTable.Rows[i];
                            if (dr["FileID"].ToString() == hidID.Value)
                                dr.Delete();
                        }
                        Session["ConfirmedSalesInformation"] = workTable;
                        LoadData();
                    }
                }
            }            

        }

        protected void dgAttachmentData_EditCommand(object sender, DataGridCommandEventArgs e)
        {

        }

        protected void dgAttachmentData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {

        }

        protected void dgAttachmentData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
        }

        protected void dgAttachmentData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                LinkButton ibtnDownload = (LinkButton)e.Item.FindControl("linkName");

                if (ibtnDownload != null)
                {
                    ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                    if (sm != null)
                    {
                        sm.RegisterPostBackControl(ibtnDownload);
                    }
                }
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                {
                    lnkDelete.Attributes.Add("onclick", "return confirm_delete();");
                }
            }
        }

        protected void btnUpdateFileUploadAttachment_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            DataGrid dg = (DataGrid)TabPanel5.FindControl("dgAttachmentData");
            if (dg != null && dg.Controls.Count > 0)
            {
                FileUpload FileUpload1 = (FileUpload)dg.Controls[0].Controls[dg.Items.Count + 1].FindControl("FileUpload1");
                Label uploadStr = (Label)dg.Controls[0].Controls[dg.Items.Count + 1].FindControl("lblUpload");

                if (FileUpload1.HasFile)
                {
                    try
                    {
                        string folderPath = System.Web.Configuration.WebConfigurationManager.AppSettings["MR_ATTACHMENT_DOWNLOAD_PATH"].ToString() + session.CompanyId;
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string c = FileUpload1.FileName.ToString(); // We don't need the path, just the name.
                        if (c.Length > 99)
                            c = c.Substring(0, 99);
                        string randomIDFileName = loginUserOrAlternateParty.ToString() + DateTime.Now.Ticks.ToString() + FileUpload1.FileName.ToString();
                        if (randomIDFileName.Length > 99)
                            randomIDFileName = randomIDFileName.Substring(0, 99);
                        FileUpload1.SaveAs(folderPath + "/" + randomIDFileName);
                        Session["FileNameAttachment"] = c;
                        Session["FileNameEncryptedAttachment"] = randomIDFileName;
                        uploadStr.Text = FileUpload1.FileName.ToString();
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }
                }
            }
        }

        #region ddlTaxType_SelectedIndexChanged
        protected void ddlTaxType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TaxType rate = TaxType.RetrieveByKey(session.CompanyId, ddlTaxType.SelectedValue);
            txtTaxRate.Text = (rate.TaxRate.Value * 100).ToString("#0.00") + "%";
            CalculateTotal();
        }
        #endregion

        protected void CalculateTotal()
        {
            LogSession session = base.GetSessionInfo();

           double subTotal = 0;
           string purchaseCurrency = "";
           double unitPP = 0.00;
           double orderQty = 0.00;
         
           foreach (DataGridItem item in dgProductData.Items)
           {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    Label lblUnitPP = (Label)item.FindControl("lblUnitPP");
                    Label lblOrderQty = (Label)item.FindControl("lblOrderQty");
                    Label lblPurchaseCurrency = (Label)item.FindControl("lblPurchaseCurrency");
                    Double.TryParse(lblUnitPP.Text, out unitPP);
                    Double.TryParse(lblOrderQty.Text, out orderQty);
                    subTotal = subTotal + unitPP * orderQty;
                    purchaseCurrency = lblPurchaseCurrency.Text;
                }
            }
            

            lblSubTotal.Text = String.Format("{0:0.00}", subTotal);

            this.lblSubTotal.Text = subTotal.ToString("#0.00");
            Double discount = GMSUtil.ToDouble(this.txtDiscount.Text);
            Double taxRate = GMSUtil.ToDouble(this.txtTaxRate.Text.Trim().TrimEnd('%')) / 100;
            this.lblTaxAmount.Text = ((subTotal-discount) * taxRate).ToString("#0.00");     
            this.lblGrandTotal.Text = (GMSUtil.ToDecimal(this.lblSubTotal.Text) + GMSUtil.ToDecimal(this.lblTaxAmount.Text) - GMSUtil.ToDecimal(this.txtDiscount.Text)).ToString("#0.00");

            lblCurrency1.Text = purchaseCurrency;
            lblCurrency2.Text = purchaseCurrency;
            lblCurrency3.Text = purchaseCurrency;
            lblCurrency4.Text = purchaseCurrency;
        }

        protected void txtDiscount_OnTextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }


        static IList<MRGRNDetail> removeDuplicates(IList<MRGRNDetail> inputList)
        {
            Dictionary<string, int> uniqueStore = new Dictionary<string, int>();
            IList<MRGRNDetail> finalList = new List<MRGRNDetail>();

            foreach (MRGRNDetail currValue in inputList)
            {
                if (!uniqueStore.ContainsKey(currValue.GRNNo))
                {
                    uniqueStore.Add(currValue.GRNNo, 0);
                    finalList.Add(currValue);
                }
            }
            return finalList;
        }



    }
}
