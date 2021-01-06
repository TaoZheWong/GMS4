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

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Text;
using System.Web.Services;
using AjaxControlToolkit;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;





namespace GMSWeb.Sales.Sales
{
    public partial class AddEditQuotation2 : GMSBasePage
    {

        protected ReportDocument crReportDocument;
        private short reportId = 0;
        private string trnNo = "";
        string filePath = "";
        string fileName = "";
        string fileURLPath = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Sales");



            if (!IsPostBack)
            {



                

                if (Request.Params["RANDOMID"] != null && Request.Params["RANDOMID"].ToString() != "")
                {
                    TransactionApproval ta = (new SystemDataActivity()).RetrieveTransactionApprovalByRandomID(Request.Params["RANDOMID"].ToString());
                    if (ta != null)
                    {
                        if (base.IsSessionActive())
                        {
                            GMSUser user = null;
                            try
                            {
                                user = new GMSUserActivity().RetrieveUser((int)ta.ApprovedUserID);
                            }
                            catch (Exception ex)
                            {
                                //e.Authenticated = false;
                                //return;
                                throw ex;
                            }

                            if (user != null)
                            {
                                if (!user.AllowRemoteAccess && Request.UserHostAddress != "127.0.0.1")
                                {
                                    string[] userIP = Request.UserHostAddress.Split('.');
                                    if (userIP[0] != "192" || userIP[1] != "168")
                                    {
                                        JScriptAlertMsg("You are not allowed to access GMS from outside.");
                                        //e.Authenticated = false;
                                        return;
                                    }
                                }
                                LogSession sess = new LogSession();
                                sess.CountryId = 1;
                                sess.DivisionId = 2;
                                sess.CompanyId = ta.CoyID;
                                sess.UserId = user.Id;
                                sess.UserName = user.UserName;
                                sess.UserRealName = user.UserRealName;
                                sess.IPAddress = Request.UserHostAddress;
                                sess.LastLoginDate = DateTime.Now.ToString();

                                #region Plug in user's available Access Items/Operations
                                IList<UserAccessModuleCategory> lstModuleCategory = new GMSUserActivity().RetrieveUserAccessModuleCategoryByUserId(user.Id);
                                List<short> userAccessModuleCategory = new List<short>();
                                foreach (UserAccessModuleCategory mc in lstModuleCategory)
                                {
                                    userAccessModuleCategory.Add(mc.ModuleCategoryID);
                                }
                                sess.UserAccessModuleCategory = userAccessModuleCategory;
                                #endregion

                                Session[GMSCoreBase.SESSIONNAME] = sess;

                                //update user's last login date
                                user.GMSLastLoginDate = DateTime.Now;
                                user.Save();
                                user = user.Resync();

                                //e.Authenticated = true;
                            }
                        }
                        this.lblQuotationNo.Text = ta.TrnNo;
                    }
                }

                if (Request.Params["QuotationNo"] != null && Request.Params["QuotationNo"].ToString() != "")
                {
                    this.lblQuotationNo.Text = Request.Params["QuotationNo"].ToString();

                }

                PopulateDDL();

                DataSet dsTemp = new DataSet();
                (new QuotationDataDALC()).GetTNCByNameForAutoComplete("", ref dsTemp);
                this.dgTNC1.DataSource = dsTemp;
                this.dgTNC1.DataBind();

                btnLoadInformation_Click(btnLoadInformation, null);
                LoadData();

                txtQuotationDate.Attributes.Add("readonly", "readonly");
            }

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            105); //View Quotation
            if (uAccess == null)
            {
                UserAccessModule uAccess2 = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                             102);//Add/Edit Quotation
                if (uAccess2 == null)
                {
                    Response.Redirect(base.UnauthorizedPage("Sales"));
                }
            }

            uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                             106);//Display GP
            if (uAccess == null)
            {
                this.dgData.Columns[8].Visible = false;
                this.dgPackage.Columns[5].Visible = false;
                lblGPercentage.Visible = false;
            }
            else
            {
                this.dgData.Columns[8].Visible = true;
                this.dgPackage.Columns[5].Visible = true;
                lblGPercentage.Visible = true;
            }



            if (!IsPostBack)
            {
                ddlReport.Items.Clear();
                if (this.lblStatus.Text == "Pending For Approval")
                    ddlReport.Items.Add(new ListItem("Sales Quotation Draft", "SalesQuotationDraft"));
                else if (this.lblStatus.Text == "Pending")
                {
                    ddlReport.Items.Add(new ListItem("Sales Quotation", "SalesQuotation"));
                    ddlReport.Items.Add(new ListItem("Sales Quotation With Letter Head", "SalesQuotationWithLetterHead_" + session.CompanyId.ToString()));
                    if (session.DivisionId == 4)
                    {
                        ddlReport.Items.Add(new ListItem("Sales Quotation With Letter Head For Dealer", "SalesQuotationWithLetterHeadForDealer_" + session.CompanyId.ToString()));
                        ddlReport.Items.Add(new ListItem("Sales Quotation With Letter Head For Trading", "SalesQuotationWithLetterHeadForTrading_" + session.CompanyId.ToString()));
                    }
                    ddlReport.Items.Add(new ListItem("Sales Quotation Full Product Listing", "SalesQuotationFullProductListing"));
                }
                else
                {
                    ddlReport.Visible = false;
                    lnkPrintReport.Visible = false;
                    lnkPrintPDF.Visible = false;
                    lnkEmailPDF.Visible = false;

                }
            }





            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS/scripts/popcalendar.js""></script>
                <script language=""javascript"" type=""text/javascript"">
                    function escapeText(lnk)
                    {

                        var idText = lnk.id.substring(0, lnk.id.length - 9) + 'txtNewName';
                        document.getElementById(idText).value = escape(document.getElementById(idText).value); 

                    }

                    function SearchAccount()
                    {			
	                    var url = 'AccountSearch.aspx'; 
                        detailWindow = window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");	
                        //detailWindow.moveTo(100,0); 	
	                    return false;
                    }	
                    
                    function SetSelectedAccountCode(accountCode,AccountName,Address1,Address2,Address3,Address4,AttentionTo,MobilePhone,OfficePhone,Fax,Email,SalesPersonID)
                    {
                        if(accountCode != null)
                        {					
                            document.getElementById('ctl00_ContentPlaceHolderMain_txtAccountCode').value = accountCode.replace(/^\s+|\s+$/g, '');
                            document.getElementById('ctl00_ContentPlaceHolderMain_txtAccountName').value = AccountName.replace(/^\s+|\s+$/g, '');
                            document.getElementById('ctl00_ContentPlaceHolderMain_ddlSalesman').value = SalesPersonID;
                            document.getElementById('ctl00_ContentPlaceHolderMain_txtAddress1').value = Address1.replace(/^\s+|\s+$/g, '');
                            document.getElementById('ctl00_ContentPlaceHolderMain_txtAddress2').value = Address2.replace(/^\s+|\s+$/g, '');
                            document.getElementById('ctl00_ContentPlaceHolderMain_txtAddress3').value = Address3.replace(/^\s+|\s+$/g, '');
                            document.getElementById('ctl00_ContentPlaceHolderMain_txtAddress4').value = Address4.replace(/^\s+|\s+$/g, '');
                            document.getElementById('ctl00_ContentPlaceHolderMain_txtAttentionTo').value = AttentionTo.replace(/^\s+|\s+$/g, '');
                            document.getElementById('ctl00_ContentPlaceHolderMain_txtMobilePhone').value = MobilePhone.replace(/^\s+|\s+$/g, '');
                            document.getElementById('ctl00_ContentPlaceHolderMain_txtOfficePhone').value = OfficePhone.replace(/^\s+|\s+$/g, '');
                            document.getElementById('ctl00_ContentPlaceHolderMain_txtFax').value = Fax.replace(/^\s+|\s+$/g, '');
                            document.getElementById('ctl00_ContentPlaceHolderMain_txtEmail').value = Email.replace(/^\s+|\s+$/g, '');
                            __doPostBack('ctl00_ContentPlaceHolderMain_txtAccountCode', '');
                            /*
                            if (document.getElementById('ctl00_ContentPlaceHolderMain_txtAccountCode').value != """")
                            {	
                                if (document.all)			
                                {	
                                    //IE			
                                    document.getElementById('ctl00_ContentPlaceHolderMain_txtAccountCode').fireEvent('onchange');
                                }
                                else
                                {
                                    //FireFox
                                    var e = document.createEvent('HTMLEvents');
                                    e.initEvent('change', false, false);
                                    document.getElementById('ctl00_ContentPlaceHolderMain_txtAccountCode').dispatchEvent(e);
                                }
                            }
                            */
                            		
                        }
                    }		

                    var txtNewProductCode_id;                    
                    var txtNewProductDescription_id;
                    var hidWeightedCost_id;
                    var ddlNewUOM_id;
                    var lblListPrice_id;
                    var lblMinSellingPrice_id;
                    var txtNewQuantity_id;
                    var txtNewPackageProductCode_id;
                    var txtNewUnitPrice_id;                    
                    
                    
                    function SearchProduct(ctl)
                    {	
                        txtNewProductCode_id = ctl.id.replace('lnkFindProduct', 'txtNewProductCode');
                        txtNewProductDescription_id = ctl.id.replace('lnkFindProduct', 'txtNewProductDescription');                        
                        hidWeightedCost_id = ctl.id.replace('lnkFindProduct', 'hidWeightedCost');
                        ddlNewUOM_id = ctl.id.replace('lnkFindProduct', 'ddlNewUOM');
                        lblListPrice_id = ctl.id.replace('lnkFindProduct', 'lblListPrice');
                        lblMinSellingPrice_id = ctl.id.replace('lnkFindProduct', 'lblMinSellingPrice');
                    
                        
                        var acctCode = document.getElementById('ctl00_ContentPlaceHolderMain_txtAccountCode').value; 
                        if (acctCode != """")
                        {		
	                        var url = 'ProductSearch.aspx?AccountCode=' + acctCode; 
                            window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");	
                        }
                        else
                        {
	                        var url = 'ProductSearch.aspx?AccountCode=xxx'; 
                            window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");	

                        }
	                    return false;
                    }	

                    function SetSelectedProductCode(productCode,productName,uom,weightedCost,listPrice,minSellingPrice,intercoPrice)
                    {
                        if(productCode != null)
                        {					
	                        document.getElementById(txtNewProductCode_id).value = productCode.replace(/^\s+|\s+$/g, '');
                            document.getElementById(txtNewProductDescription_id).value = productName.replace(/^\s+|\s+$/g, '');
                            document.getElementById(hidWeightedCost_id).value = weightedCost.replace(/^\s+|\s+$/g, '');
                            document.getElementById(ddlNewUOM_id).value = uom;
                            document.getElementById(lblListPrice_id).innerText = listPrice;
                            
                            if(!(document.getElementById('ctl00_ContentPlaceHolderMain_chkIsNewCustomer').checked) && (document.getElementById('ctl00$ContentPlaceHolderMain$txtAccountCode').value.replace(/^\s+|\s+$/g, '') != '') && (document.getElementById('ctl00$ContentPlaceHolderMain$txtAccountCode').value.substring(0,2).toUpperCase() == 'DI')) 
                            {                            
                            document.getElementById(lblMinSellingPrice_id).innerText = intercoPrice;
                            }
                            else 
                            {
                            document.getElementById(lblMinSellingPrice_id).innerText = minSellingPrice;
                            }
                           
                           
                            //__doPostBack('txtNewProductDescription_id', '');

                            
                            if (document.getElementById(txtNewProductCode_id).value != """")
                            {	
                                if (document.all)			
                                {	
                                    //IE	
                                    
                                    document.getElementById(txtNewProductDescription_id).fireEvent('onchange');
                                }
                                else
                                {
                                    //FireFox
                                    var e = document.createEvent('HTMLEvents');
                                    e.initEvent('change', false, false);
                                    document.getElementById(txtNewProductDescription_id).dispatchEvent(e);
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

                    function btnEmailClick() 
                    {    

                        var txtto = document.getElementById('ctl00_ContentPlaceHolderMain_txtTo').value;
                        var txtcc = document.getElementById('ctl00_ContentPlaceHolderMain_txtCC').value;
                        var txtbcc = document.getElementById('ctl00_ContentPlaceHolderMain_txtBCC').value;

                        var ccstatus =  true;
                        var bccstatus = true;

                        
                        
                        if (txtcc != """" && !validateEmail(txtcc)) 
                            ccstatus = false;

                        if (txtbcc != """" && !validateEmail(txtbcc)) 
                            bccstatus = false;                       
                                      
                        
                        if ((txtto != """" && validateEmail(txtto)) && ccstatus && bccstatus)
                        {
                            var behavior = $find('btnEmailPopupBehavior');   
                            var behavior1 = $find('btnOkPopupBehavior');  
                            if (behavior)         
                            {  
                   
                                behavior.hide();         
                            } 

                            if (behavior1)         
                            {  
                   
                                behavior1.show();         
                            } 
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

                    

                    function EditDescription(snNo)
                    {			
                        var qNo = document.getElementById('" + this.lblQuotationNo.ClientID + @"').firstChild.nodeValue.replace(/^\s+|\s+$/g, '');		
                        var rNo = document.getElementById('" + this.ddlRevisionNo.ClientID + @"').options[document.getElementById('" + ddlRevisionNo.ClientID + @"').selectedIndex].value;
	                    var url = 'AddEditQuotationDetailDescription.aspx?QuotationNo=' + qNo + '&SNNo=' + snNo + '&RevisionNo=' + rNo; 
                        detailWindow = window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
                        //detailWindow.moveTo(100,0); 	
	                    return false;
                    }

                    function EditPackageDescription(snNo)
                    {	
                        var qNo = document.getElementById('" + this.lblQuotationNo.ClientID + @"').firstChild.nodeValue.replace(/^\s+|\s+$/g, '');	
                        var rNo = document.getElementById('" + this.ddlRevisionNo.ClientID + @"').options[document.getElementById('" + ddlRevisionNo.ClientID + @"').selectedIndex].value;				
	                    var url = 'EditQuotationPackageDescription.aspx?QuotationNo=' +qNo + '&SNNo=' + snNo + '&RevisionNo=' + rNo; 
                        detailWindow = window.open(url,"""",""width="" + 650 + "",height="" + 650 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
                        //detailWindow.moveTo(100,0); 
	                    return false;
                    }	

                    function viewDetail(snNo)
                    {			
                        var qNo = document.getElementById('" + this.lblQuotationNo.ClientID + @"').firstChild.nodeValue.replace(/^\s+|\s+$/g, '');
                        var rNo = document.getElementById('" + this.ddlRevisionNo.ClientID + @"').options[document.getElementById('" + ddlRevisionNo.ClientID + @"').selectedIndex].value;		
	                    var url = ""EditQuotationPackageDetail.aspx?QuotationNo="" + qNo + ""&SNNo="" + snNo + ""&RevisionNo="" + rNo;                       
                        detailWindow = window.open(url,""detailWindow"",""width="" + 650 + "",height="" + 650 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
                        //detailWindow.moveTo(100,0); 		
	                    return false;
                    }	
                
                    function reloadPage()
                    {
                        window.location.href = window.location.href;
                    }


                    var nameSpace = null; 
                    var mailFolder = null; 
                    var mailItem = null; 
                    var tempDoc = null; 
                    var outlookApp = null;
                    function OpenOutlookDoc(whatform,file) 
                    { 
                          try 
                          { 
                          outlookApp = new ActiveXObject('Outlook.Application'); 
                          nameSpace = outlookApp.getNameSpace('MAPI'); 
                          mailFolder = nameSpace.getDefaultFolder(6); 
                          mailItem = mailFolder.Items.add(whatform); 
                          mailItem.Attachments.Add(file);  
                          mailItem.Display(true) 
                          } 
                          catch(e) 
                          {                           
                          } 
                   } 
                    
                </script>
                ";

            //Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onload", javaScript, false);
        }

        public bool IsControlAdded
        {
            get
            {
                if (ViewState["IsControlAdded"] == null)
                    ViewState["IsControlAdded"] = false;
                return (bool)ViewState["IsControlAdded"];
            }
            set { ViewState["IsControlAdded"] = value; }
        }

        #region PopulateEmailData
        private void PopulateEmailData()
        {
            LogSession session = base.GetSessionInfo();
            lblFileName.Text = "Quotation_" + this.lblQuotationNo.Text + ".pdf";


            Employee employee = new EmployeeActivity().RetrieveEmployeeByEmployeeName(session.UserRealName);
            string empName = "";
            string empDesignation = "";
            string empDepartment = "";

            if (employee != null)
            {
                empName = employee.Name.ToString();
                empDesignation = employee.Designation.ToString();
                empDepartment = employee.Department.ToString();
            }
            txtTo.Text = txtEmail.Text;
            txtEmailSubject.Text = "Quotation - " + this.lblQuotationNo.Text;


            string signature = "Dear " + txtAttentionTo.Text + ",";
            signature += "<p>Regards,<br />";
            signature += "<b>" + empName + "</b><br/>" + empDesignation + ", " + empDepartment + "</p>";
            signature += "<p><b>Leeden Limited</b><br />";
            signature += "1 Shipyard Road, Singapore 628128<br />";
            signature += "<a href=\"http://www.leedenlimited.com/\">www.leedenlimited.com</a></p>";



            //CKEditor1.Text = signature.ToString();

        }
        #endregion

        #region PopulateDDL
        private void PopulateDDL()
        {
            LogSession session = base.GetSessionInfo();

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


            ddlCurrency.DataSource = cList;
            ddlCurrency.DataTextField = "CurrencyCode";
            ddlCurrency.DataValueField = "CurrencyCode";
            ddlCurrency.DataBind();
            Company coy = null;
            try
            {
                coy = dalc.RetrieveCompanyById(session.CompanyId, session);
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }

            ddlCurrency.SelectedValue = coy.DefaultCurrencyCode;


            lblCurrency.Text = coy.DefaultCurrencyCode; ;
            lblCurrency2.Text = coy.DefaultCurrencyCode;
            lblCurrency3.Text = coy.DefaultCurrencyCode;
            //Edit by Kim to remove display currency rate
            //txtCurrencyRate.Text = "1.00";



            IList<TaxType> cList2 = null;
            try
            {
                cList2 = dalc.RetrieveAllTaxTypeListByCompanyCode(session.CompanyId);
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


            //this.txtTaxRate.Text = "7.00%";

            ProductsDataDALC dacl = new ProductsDataDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetSalesman(session.CompanyId, session.UserId, ref ds);
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }

            ddlSalesman.DataSource = ds;
            ddlSalesman.DataBind();

            if (lblQuotationNo.Text.Trim() != "")
            {
                ddlRevisionNo.Enabled = true;
                QuotationDataDALC qDacl = new QuotationDataDALC();
                DataSet dsRevision = new DataSet();
                try
                {
                    qDacl.GetAllRevisionsByQuotationNoSelect(session.CompanyId, lblQuotationNo.Text.Trim(), ref dsRevision);
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }

                ddlRevisionNo.DataSource = dsRevision;
                ddlRevisionNo.DataBind();
            }
            else
            {
                ddlRevisionNo.Enabled = false;
                ddlRevisionNo.Items.Add("0");
            }
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            if (this.lblQuotationNo.Text.Trim() != "")
            {
                #region Edit
                QuotationHeader qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), GMSUtil.ToByte(ddlRevisionNo.SelectedValue));
                this.txtQuotationDate.Text = qh.QuotationDate.Value.ToString("dd/MM/yyyy");
                this.lblStatus.Text = qh.QuotationStatusObject.QuotationStatusName;
                switch (qh.QuotationStatusID)
                {
                    case "3": //Pending For Approval
                        //this.btnApprove.Visible = true;
                        break;
                    case "4":
                        this.btnSubmit.Enabled = false;
                        this.btnSave.Enabled = false;
                        this.btnAddToRevision.Enabled = false;
                        this.btnApplyDiscount.Enabled = false;
                        break;
                    case "5":
                        this.btnSubmit.Enabled = false;
                        this.btnSave.Enabled = false;
                        this.btnAddToRevision.Enabled = false;
                        this.btnApplyDiscount.Enabled = false;
                        break;
                }
                btnDuplicate.Visible = true;
                btnAddToRevision.Visible = true;
                btnSubmit.Visible = true;
                btnApplyDiscount.Visible = true;
                this.chkIsNewCustomer.Checked = qh.IsNewCustomer.Value;
                if (qh.IsNewCustomer.Value)
                {
                    this.txtNewCustomerName.Text = qh.NewCustomerName;
                    this.txtAccountCode.ReadOnly = true;
                    this.txtAccountName.ReadOnly = true;
                    this.txtNewCustomerName.ReadOnly = false;
                    this.lnkFindAccount.Visible = false;
                }
                else
                {
                    this.txtAccountCode.Text = qh.AccountCode;
                    this.hidAccountCode.Value = qh.AccountCode;
                    this.txtAccountName.Text = qh.AccountObject.AccountName;
                    this.txtAccountCode.ReadOnly = false;
                    this.txtAccountName.ReadOnly = false;
                    this.txtNewCustomerName.ReadOnly = true;
                    this.lnkFindAccount.Visible = true;
                }
                this.ddlSalesman.SelectedValue = qh.SalesPersonID;
                this.hidSalesPersonID.Value = qh.SalesPersonID;
                this.txtSubject.Text = qh.Subject;
                this.chkIsUnsuccessful.Checked = qh.IsUnsuccessful.Value;
                if (qh.IsUnsuccessful.Value)
                {
                    this.txtOrderLostReason.Text = qh.OrderLostReason;
                    this.txtOrderLostReason.ReadOnly = false;
                    this.lnkCancelUnsuccessful.Visible = true;
                }
                else
                    this.txtOrderLostReason.ReadOnly = true;
                this.lblSORefNo.Text = qh.SORefNo;
                this.txtCustomerPONo.Text = qh.CustomerPONo;
                this.txtAttentionTo.Text = qh.AttentionTo;
                this.txtMobilePhone.Text = qh.MobilePhone;
                this.txtOfficePhone.Text = qh.OfficePhone;
                this.txtFax.Text = qh.Fax;
                this.txtEmail.Text = qh.Email;
                this.ddlCurrency.SelectedValue = qh.Currency;
                //this.txtCurrencyRate.Text = qh.CurrencyRate.Value.ToString();
                lblCurrency.Text = ddlCurrency.SelectedValue;
                lblCurrency2.Text = ddlCurrency.SelectedValue;
                lblCurrency3.Text = ddlCurrency.SelectedValue;
                this.txtInternalRemarks.Text = qh.InternalRemarks;
                this.txtExternalRemarks.Text = qh.ExternalRemarks;
                this.ddlTaxType.SelectedValue = qh.TaxTypeID;
                this.txtTaxRate.Text = qh.TaxRate * 100 + "%";
                this.txtAddress1.Text = qh.Address1;
                this.txtAddress2.Text = qh.Address2;
                this.txtAddress3.Text = qh.Address3;
                this.txtAddress4.Text = qh.Address4;
                if (qh.CreatedBy != null && qh.CreatedBy > 0)
                    this.lblCreatedBy.Text = "Created By " + qh.CreatedByUsersObject.UserRealName + " on " + qh.CreatedDate.Value.ToString();
                if (qh.ModifiedBy != null && qh.ModifiedBy > 0)
                    this.lblModifiedBy.Text = "Modified By " + qh.ModifiedByUsersObject.UserRealName + " on " + qh.ModifiedDate.Value.ToString();

                QuotationDetailDataSet ds = (QuotationDetailDataSet)ViewState["dsQuotationDetail"];
                if (ds == null)
                {
                    ds = new QuotationDetailDataSet();
                    DataSet dsTemp = new DataSet();
                    (new QuotationDataDALC()).GetQuotationDetailByQuotationNoSelect(session.CompanyId, lblQuotationNo.Text.Trim(), session.UserId,
                                                GMSUtil.ToByte(ddlRevisionNo.SelectedValue), ref dsTemp);
                    if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                    {
                        dsTemp.Tables[0].TableName = ds.QuotationDetail.TableName;
                        ds.QuotationDetail.Merge(dsTemp.Tables[0]);
                    }
                    this.dgData.DataSource = ds.QuotationDetail;
                    this.dgData.DataBind();

                    ViewState["dsQuotationDetail"] = ds;
                    hidDetailsCount.Value = ds.QuotationDetail.Rows.Count.ToString();
                }
                else
                {
                    this.dgData.DataSource = ds.QuotationDetail;
                    this.dgData.DataBind();
                }
                if (ds.QuotationDetail.Rows.Count > 0)
                {
                    this.ddlCurrency.Enabled = false;
                    //this.txtCurrencyRate.ReadOnly = true;
                }
                else
                {
                    this.ddlCurrency.Enabled = true;
                    //this.txtCurrencyRate.ReadOnly = false;
                }

                DataTable dt = (DataTable)ViewState["TNC"];
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("Name");

                    DataSet dsTemp = new DataSet();
                    (new QuotationDataDALC()).GetQuotationTNCByQuotationNoSelect(session.CompanyId, lblQuotationNo.Text.Trim(), GMSUtil.ToByte(ddlRevisionNo.SelectedValue), ref dsTemp);
                    if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                    {
                        dt.Merge(dsTemp.Tables[0]);
                    }
                    this.dgTNC.DataSource = dt;
                    this.dgTNC.DataBind();

                    ViewState["TNC"] = dt;
                }

                DataSet dsTemp2 = new DataSet();
                (new QuotationDataDALC()).GetTransactionApprovalWithLimitByQuotationNoSelect(session.CompanyId, lblQuotationNo.Text.Trim(), ref dsTemp2);
                if (dsTemp2 != null && dsTemp2.Tables.Count > 0 && dsTemp2.Tables[0].Rows.Count > 0)
                {
                    this.dgApproval.DataSource = dsTemp2;
                    this.dgApproval.DataBind();
                }

                IList<QuotationPackage> lstQuotationPackage = (new SystemDataActivity()).RetrieveAllPackageByQuotationNo(lblQuotationNo.Text.Trim(), session.CompanyId, GMSUtil.ToByte(ddlRevisionNo.SelectedValue));
                if (lstQuotationPackage != null)
                {
                    this.dgPackage.DataSource = lstQuotationPackage;
                    this.dgPackage.DataBind();
                }

                CalculateTotal();
                #endregion
            }
            else
            {
                #region Add New
                this.txtQuotationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                this.txtNewCustomerName.ReadOnly = true;
                this.txtOrderLostReason.ReadOnly = true;
                this.btnDuplicate.Visible = false;
                this.btnAddToRevision.Visible = false;
                this.btnSubmit.Visible = false;
                this.btnApplyDiscount.Visible = false;

                QuotationDetailDataSet ds = null;
                if (ViewState["dsQuotationDetail"] != null)
                    ds = (QuotationDetailDataSet)ViewState["dsQuotationDetail"];
                else
                {
                    ds = new QuotationDetailDataSet();
                }
                this.dgData.DataSource = ds.QuotationDetail;
                this.dgData.DataBind();
                if (ds.QuotationDetail.Rows.Count > 0)
                {
                    this.ddlCurrency.Enabled = false;
                    //this.txtCurrencyRate.ReadOnly = true;
                }
                else
                {
                    this.ddlCurrency.Enabled = true;
                    //this.txtCurrencyRate.ReadOnly = false;
                }


                ViewState["dsQuotationDetail"] = ds;
                hidDetailsCount.Value = "0";

                DataTable dt = new DataTable();
                dt.Columns.Add("Name");
                this.dgTNC.DataSource = dt;
                this.dgTNC.DataBind();
                ViewState["TNC"] = dt;

                DataTable qp = new DataTable();
                qp.Columns.Add("ProductCode");
                qp.Columns.Add("ProductDescription");
                qp.Columns.Add("Qty");
                qp.Columns.Add("PackageID");
                this.dgPackage.DataSource = qp;
                this.dgPackage.DataBind();
                #endregion
            }
        }
        #endregion

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblGP = (Label)e.Item.FindControl("lblGP");
                if (lblGP != null)
                {
                    if (lblGP.Text.Trim() != "")
                    {
                        decimal gp = GMSUtil.ToDecimal(lblGP.Text.Trim());
                        if (gp > 0)
                            lblGP.ForeColor = System.Drawing.Color.Green;
                        else
                            lblGP.ForeColor = System.Drawing.Color.Red;
                    }
                }

                PopupControlExtender pce = e.Item.FindControl("PopupControlExtender1") as PopupControlExtender;

                string behaviorID = "pce_" + e.Item.ItemIndex;
                pce.BehaviorID = behaviorID;

                Image img = (Image)e.Item.FindControl("imgMagnify");

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

            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddlEditDOptSNNo = (DropDownList)e.Item.FindControl("ddlEditDOptSNNo");
                if (ddlEditDOptSNNo != null)
                {
                    ddlEditDOptSNNo.Items.Add(new ListItem("-", ""));
                    ddlEditDOptSNNo.Items.Add("A");
                    ddlEditDOptSNNo.Items.Add("B");
                    ddlEditDOptSNNo.Items.Add("C");
                    ddlEditDOptSNNo.Items.Add("D");
                    ddlEditDOptSNNo.Items.Add("E");
                    DataRowView qd = (DataRowView)e.Item.DataItem;
                    ddlEditDOptSNNo.SelectedValue = qd["DOptSNNo"].ToString();
                }

                DropDownList ddlEditUOM = (DropDownList)e.Item.FindControl("ddlEditUOM");
                if (ddlEditUOM != null)
                {
                    DataSet dsTemp = new DataSet();
                    (new QuotationDataDALC()).GetAllUOMByCoyIDSelect(session.CompanyId, ref dsTemp);
                    ddlEditUOM.DataSource = dsTemp;
                    ddlEditUOM.DataValueField = "UOM";
                    ddlEditUOM.DataTextField = "UOM";
                    ddlEditUOM.DataBind();
                    DataRowView qd = (DataRowView)e.Item.DataItem;
                    ddlEditUOM.SelectedValue = qd["UOM"].ToString();
                }

                PopupControlExtender pce = e.Item.FindControl("PopupControlExtender1") as PopupControlExtender;

                string behaviorID = "pce_" + e.Item.ItemIndex;
                pce.BehaviorID = behaviorID;

                string OnMouseOverScript = string.Format("$find('{0}').showPopup();", behaviorID);
                string OnMouseOutScript = string.Format("$find('{0}').hidePopup();", behaviorID);

                Image img = (Image)e.Item.FindControl("imgMagnify");

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
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewUOM = (DropDownList)e.Item.FindControl("ddlNewUOM");
                DataSet dsTemp = new DataSet();
                (new QuotationDataDALC()).GetAllUOMByCoyIDSelect(session.CompanyId, ref dsTemp);
                ddlNewUOM.DataSource = dsTemp;
                ddlNewUOM.DataValueField = "UOM";
                ddlNewUOM.DataTextField = "UOM";
                ddlNewUOM.DataBind();


            }
        }
        #endregion

        #region dgData_CreateCommand
        protected void dgData_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (!CheckAddEditAccess())
                return;

            #region Create
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();

                ViewState["TabNo"] = "1";

                bool NewQuotation = false;
                if (lblQuotationNo.Text.Trim() == "")
                {
                    SaveQuotationHeader(true);
                    NewQuotation = true;
                }

                QuotationDetailDataSet ds = null;
                if (ViewState["dsQuotationDetail"] != null)
                    ds = (QuotationDetailDataSet)ViewState["dsQuotationDetail"];
                else
                {
                    ds = new QuotationDetailDataSet();
                }

                TextBox txtNewProductCode = (TextBox)e.Item.FindControl("txtNewProductCode");
                TextBox txtNewQuantity = (TextBox)e.Item.FindControl("txtNewQuantity");
                TextBox txtNewUnitPrice = (TextBox)e.Item.FindControl("txtNewUnitPrice");
                Label lblListPrice = (Label)e.Item.FindControl("lblListPrice");
                Label lblMinSellingPrice = (Label)e.Item.FindControl("lblMinSellingPrice");
                HtmlInputHidden hidWeightedCost = (HtmlInputHidden)e.Item.FindControl("hidWeightedCost");
                Label lblUOM = (Label)e.Item.FindControl("lblUOM");
                Label lblNewTotalPrice = (Label)e.Item.FindControl("lblNewTotalPrice");
                TextBox txtNewProductDescription = (TextBox)e.Item.FindControl("txtNewProductDescription");
                DropDownList ddlNewUOM = (DropDownList)e.Item.FindControl("ddlNewUOM");

                try
                {
                    /*
                    if (GMSUtil.ToDecimal(lblMinSellingPrice.Text.Trim()) > 0 && GMSUtil.ToDecimal(lblMinSellingPrice.Text.Trim()) > GMSUtil.ToDecimal(txtNewUnitPrice.Text.Trim()))
                    {
                        UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                           104);
                        if (uAccess == null)
                        {
                            ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Your price cannot be lower than minimum selling price!')", true);
                            return;
                        }
                    }
                    else if (GMSUtil.ToDecimal(lblMinSellingPrice.Text.Trim()) == 0)
                    {
                        UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                          104);
                        if (uAccess == null)
                        {
                            ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You do not have access to add product with 0 mininum price!')", true);
                            return;
                        }
                    }*/

                    //GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow qd = ds.QuotationDetail.NewQuotationDetailRow();
                    //qd.CoyID = session.CompanyId;
                    //qd.QuotationNo = this.lblQuotationNo.Text.Trim();
                    //qd.ProductCode = txtNewProductCode.Text.Trim();
                    //qd.Quantity = GMSUtil.ToDecimal(txtNewQuantity.Text.Trim());
                    //qd.ListPrice = GMSUtil.ToDecimal(lblListPrice.Text.Trim());
                    //qd.MinSellingPrice = GMSUtil.ToDecimal(lblMinSellingPrice.Text.Trim());
                    //qd.UnitPrice = GMSUtil.ToDecimal(txtNewUnitPrice.Text.Trim());
                    //qd.Cost = GMSUtil.ToDecimal(hidWeightedCost.Value.Trim());
                    //qd.UOM = lblUOM.Text.Trim();
                    //qd.TotalPrice = GMSUtil.ToDecimal(lblNewTotalPrice.Text.Trim());
                    //qd.TotalCost = qd.Cost * qd.Quantity;
                    //qd.ProductDescription = txtNewProductDescription.Text.Trim();
                    //if (qd.UnitPrice == 0)
                    //    qd.GPPercentage = "0.00";
                    //else if (hidWeightedCost.Value.Trim() == "")
                    //        qd.GPPercentage = null;
                    //    else
                    //        qd.GPPercentage = ((qd.UnitPrice - qd.Cost) / qd.UnitPrice * 100).ToString("#0.00");
                    //qd.DSNNo = (byte) (ds.QuotationDetail.Rows.Count + 1);
                    //ds.QuotationDetail.AddQuotationDetailRow(qd);
                    //this.dgData.DataSource = ds.QuotationDetail;
                    //this.dgData.DataBind();
                    //ViewState["dsQuotationDetail"] = ds;

                    byte max = 0;
                    if (ds.QuotationDetail.Count > 0)
                    {
                        GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow maxRow = (GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow)(ds.QuotationDetail.Select("", "SNNo Desc"))[0];
                        if (maxRow != null && !maxRow.IsSNNoNull())
                            max = maxRow.SNNo;
                    }

                    byte maxDSNNo = 1;
                    if (ds.QuotationDetail.Count > 0)
                    {
                        GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow maxRow = (GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow)(ds.QuotationDetail.Select("", "DSNNo Desc"))[0];
                        if (maxRow != null && !maxRow.IsDSNNoNull())
                            maxDSNNo = (byte)(maxRow.DSNNo + 1);
                    }

                    IList<QuotationPackage> lstQuotationPackage = (new SystemDataActivity()).RetrieveAllPackageByQuotationNo(lblQuotationNo.Text.Trim(), session.CompanyId, 0);
                    if (lstQuotationPackage != null && lstQuotationPackage.Count > 0)
                    {
                        foreach (QuotationPackage tempQP in lstQuotationPackage)
                        {
                            if (tempQP.DSNNo >= maxDSNNo)
                                maxDSNNo = (byte)(tempQP.DSNNo + 1);
                        }
                    }

                    QuotationDetail qd = new QuotationDetail();
                    qd.CoyID = session.CompanyId;
                    qd.QuotationNo = this.lblQuotationNo.Text.Trim();
                    qd.SNNo = ++max;
                    qd.CreatedBy = session.UserId;
                    qd.CreatedDate = DateTime.Now;
                    qd.RevisionNo = 0;
                    qd.ProductCode = txtNewProductCode.Text.Trim();
                    qd.ProductDescription = txtNewProductDescription.Text.Trim();
                    qd.Quantity = GMSUtil.ToDecimal(txtNewQuantity.Text.Trim());
                    qd.ListPrice = GMSUtil.ToDecimal(lblListPrice.Text.Trim());
                    qd.MinSellingPrice = GMSUtil.ToDecimal(lblMinSellingPrice.Text.Trim());
                    qd.UnitPrice = GMSUtil.ToDecimal(txtNewUnitPrice.Text.Trim());
                    qd.Cost = GMSUtil.ToDecimal(hidWeightedCost.Value.Trim());
                    qd.DSNNo = maxDSNNo;
                    qd.UOM = ddlNewUOM.SelectedValue;
                    qd.Save();
                    qd.Resync();
                    ViewState["dsQuotationDetail"] = null;
                    LoadData();

                    CalculateTotal();
                    
                    if (NewQuotation)
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("<script language='javascript'>");
                        str.Append("alert('Quotation " + lblQuotationNo.Text.Trim() + " is created.');");
                        str.Append("window.location.href = \"AddEditQuotation2.aspx?ActiveTab=1&QuotationNo=" + lblQuotationNo.Text.Trim() + "\";");
                        str.Append("</script>");
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "NewQuotation", str.ToString(), false);
                    }

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('" + ex.Message + "')", true);
                }
            }
            #endregion

            #region GoUp
            if (e.CommandName == "GoUp")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                //                                                           112);
                //if (uAccess == null)
                //    Response.Redirect(base.UnauthorizedPage("Sales"));

                if (e.Item.ItemIndex > 0)
                {
                    QuotationDetailDataSet ds = (QuotationDetailDataSet)ViewState["dsQuotationDetail"];
                    GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow row = (GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow)ds.QuotationDetail.Rows[e.Item.ItemIndex];
                    GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow prevRow = (GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow)ds.QuotationDetail.Rows[e.Item.ItemIndex - 1];
                    //row.ItemArray = .ItemArray;
                    //ds.QuotationDetail.Rows.RemoveAt(e.Item.ItemIndex);
                    //ds.QuotationDetail.Rows.InsertAt(row,(e.Item.ItemIndex - 1));
                    //this.dgData.EditItemIndex = -1;
                    //this.dgData.DataSource = ds.QuotationDetail;
                    //this.dgData.DataBind();
                    //ViewState["dsQuotationDetail"] = ds;

                    QuotationDetail qd = QuotationDetail.RetrieveByKey(row.CoyID, row.QuotationNo, row.SNNo, 0);
                    QuotationDetail prevQD = QuotationDetail.RetrieveByKey(prevRow.CoyID, prevRow.QuotationNo, prevRow.SNNo, 0);
                    byte tempSNNo = qd.DSNNo.Value;
                    qd.DSNNo = prevQD.DSNNo.Value;
                    prevQD.DSNNo = tempSNNo;
                    qd.Save();
                    prevQD.Save();
                    ViewState["dsQuotationDetail"] = null;
                    LoadData();
                }
            }
            #endregion

            #region GoDown
            if (e.CommandName == "GoDown")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                //                                                           112);
                //if (uAccess == null)
                //    Response.Redirect(base.UnauthorizedPage("Sales"));

                QuotationDetailDataSet ds = (QuotationDetailDataSet)ViewState["dsQuotationDetail"];
                if (e.Item.ItemIndex < (ds.QuotationDetail.Rows.Count - 1))
                {
                    GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow row = (GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow)ds.QuotationDetail.Rows[e.Item.ItemIndex];
                    GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow nxtRow = (GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow)ds.QuotationDetail.Rows[e.Item.ItemIndex + 1];
                    //row.ItemArray = .ItemArray;
                    //ds.QuotationDetail.Rows.RemoveAt(e.Item.ItemIndex);
                    //ds.QuotationDetail.Rows.InsertAt(row, (e.Item.ItemIndex + 1));
                    //this.dgData.EditItemIndex = -1;
                    //this.dgData.DataSource = ds.QuotationDetail;
                    //this.dgData.DataBind();
                    //ViewState["dsQuotationDetail"] = ds;
                    //CalculateTotal();

                    QuotationDetail qd = QuotationDetail.RetrieveByKey(row.CoyID, row.QuotationNo, row.SNNo, 0);
                    QuotationDetail nxtQD = QuotationDetail.RetrieveByKey(nxtRow.CoyID, nxtRow.QuotationNo, nxtRow.SNNo, 0);
                    byte tempSNNo = qd.DSNNo.Value;
                    qd.DSNNo = nxtQD.DSNNo.Value;
                    nxtQD.DSNNo = tempSNNo;
                    qd.Save();
                    nxtQD.Save();
                    ViewState["dsQuotationDetail"] = null;
                    LoadData();
                }
            }
            #endregion
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();

                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                if (!CheckAddEditAccess())
                    return;

                //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                //                                                                100);
                //if (uAccess == null)
                //    Response.Redirect(base.UnauthorizedPage("Sales"));

                QuotationDetailDataSet ds = (QuotationDetailDataSet)ViewState["dsQuotationDetail"];

                //Label lblSN = (Label)e.Item.FindControl("lblSN");
                //ds.QuotationDetail.Rows.RemoveAt(GMSUtil.ToInt(lblSN.Text.Trim()) - 1);

                //this.dgData.DataSource = ds.QuotationDetail;
                //this.dgData.DataBind();
                //ViewState["dsQuotationDetail"] = ds;

                GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow row = (GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow)ds.QuotationDetail.Rows[e.Item.ItemIndex];
                QuotationDetail qd = QuotationDetail.RetrieveByKey(row.CoyID, row.QuotationNo, row.SNNo, 0);
                if (qd != null)
                {
                    IList<QuotationDetailDescription> qddList = (new SystemDataActivity()).RetrieveAllQuotationDetailDescriptionListByCompanyCodeQuotationNoSNNo(session.CompanyId,
                                                                                                   qd.QuotationNo, qd.SNNo, 0);
                    for (int j = qddList.Count - 1; j >= 0; j--)
                    {
                        QuotationDetailDescription qdd = qddList[j];
                        qdd.Delete();
                        qdd.Resync();
                    }
                    qd.Delete();
                    qd.Resync();
                }

                ViewState["dsQuotationDetail"] = null;
                LoadData();
                CalculateTotal();
                
            }
        }
        #endregion

        #region dgData_EditCommand
        protected void dgData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            this.dgData.EditItemIndex = e.Item.ItemIndex;
            QuotationDetailDataSet ds = (QuotationDetailDataSet)ViewState["dsQuotationDetail"];
            this.dgData.DataSource = ds.QuotationDetail;
            this.dgData.DataBind();
            //LoadData();
        }
        #endregion

        #region dgData_CancelCommand
        protected void dgData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData.EditItemIndex = -1;
            QuotationDetailDataSet ds = (QuotationDetailDataSet)ViewState["dsQuotationDetail"];
            this.dgData.DataSource = ds.QuotationDetail;
            this.dgData.DataBind();
        }
        #endregion

        #region dgData_UpdateCommand
        protected void dgData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            if (!CheckAddEditAccess())
                return;

            QuotationDetailDataSet ds = (QuotationDetailDataSet)ViewState["dsQuotationDetail"];

            TextBox txtEditQuantity = (TextBox)e.Item.FindControl("txtEditQuantity");
            TextBox txtEditUnitPrice = (TextBox)e.Item.FindControl("txtEditUnitPrice");
            TextBox txtEditProductDescription = (TextBox)e.Item.FindControl("txtEditProductDescription");
            TextBox txtEditDSNNo = (TextBox)e.Item.FindControl("txtEditDSNNo");
            DropDownList ddlEditDOptSNNo = (DropDownList)e.Item.FindControl("ddlEditDOptSNNo");
            DropDownList ddlEditUOM = (DropDownList)e.Item.FindControl("ddlEditUOM");

            GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow row = (GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow)ds.QuotationDetail.Rows[e.Item.ItemIndex];
            QuotationDetail qd = QuotationDetail.RetrieveByKey(row.CoyID, row.QuotationNo, row.SNNo, 0);
            //row.ProductDescription = txtEditProductDescription.Text.Trim().ToUpper();
            //row.Quantity = GMSUtil.ToDecimal(txtEditQuantity.Text.Trim());
            //row.UnitPrice = GMSUtil.ToDecimal(txtEditUnitPrice.Text.Trim());
            //row.TotalPrice = row.Quantity * row.UnitPrice;
            //row.TotalCost = row.Cost * row.Quantity;
            //if (row.UnitPrice == 0)
            //    row.GPPercentage = "0.00";
            //else
            //    row.GPPercentage = ((row.UnitPrice - row.Cost) / row.UnitPrice * 100).ToString("#0.00");
            //row.AcceptChanges();
            //this.dgData.EditItemIndex = -1;
            //this.dgData.DataSource = ds.QuotationDetail;
            //this.dgData.DataBind();
            //ViewState["dsQuotationDetail"] = ds;

            qd.ProductDescription = txtEditProductDescription.Text.Trim().ToUpper();
            qd.Quantity = GMSUtil.ToDecimal(txtEditQuantity.Text.Trim());
            qd.UnitPrice = GMSUtil.ToDecimal(txtEditUnitPrice.Text.Trim());
            qd.DSNNo = GMSUtil.ToByte(txtEditDSNNo.Text.Trim());
            qd.DOptSNNo = ddlEditDOptSNNo.SelectedValue.Trim();
            qd.UOM = ddlEditUOM.SelectedValue.Trim();
            qd.Save();
            qd.Resync();
            ViewState["dsQuotationDetail"] = null;
            this.dgData.EditItemIndex = -1;
            LoadData();
            CalculateTotal();
            
        }
        #endregion

        #region chkIsNewCustomer_OnCheckedChanged
        protected void chkIsNewCustomer_OnCheckedChanged(object sender, EventArgs e)
        {
            if (this.chkIsNewCustomer.Checked)
            {
                this.txtNewCustomerName.ReadOnly = false;
                this.txtAccountCode.Text = "";
                this.txtAccountCode.ReadOnly = true;
                this.txtAccountName.Text = "";
                this.txtAccountName.ReadOnly = true;
                lnkFindAccount.Visible = false;
            }
            else
            {
                //this.txtNewCustomerName.Text = "";
                this.txtNewCustomerName.ReadOnly = true;
                this.txtAccountCode.ReadOnly = false;
                this.txtAccountName.ReadOnly = false;
                lnkFindAccount.Visible = true;
            }
        }
        #endregion

        #region chkIsUnsuccessful_OnCheckedChanged
        protected void chkIsUnsuccessful_OnCheckedChanged(object sender, EventArgs e)
        {
            if (this.chkIsUnsuccessful.Checked)
            {
                this.txtOrderLostReason.ReadOnly = false;
            }
            else
            {
                this.txtOrderLostReason.ReadOnly = true;
            }
        }
        #endregion

        #region txtNewProductDescription_OnTextChanged
        protected void txtNewProductDescription_OnTextChanged(object sender, EventArgs e)
        {
        }
        #endregion

        #region txtNewProductCode_OnTextChanged
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
                dalc.GetProductPrice(session.CompanyId, prodCode, session.UserId, ref ds);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TableRow tr = (TableRow)txtNewProductCode.Parent.Parent;
                    Label lblListPrice = (Label)tr.FindControl("lblListPrice");
                    Label lblMinSellingPrice = (Label)tr.FindControl("lblMinSellingPrice");
                    TextBox txtNewQuantity = (TextBox)tr.FindControl("txtNewQuantity");
                    TextBox txtNewProductDescription = (TextBox)tr.FindControl("txtNewProductDescription");
                    HtmlInputHidden hidWeightedCost = (HtmlInputHidden)tr.FindControl("hidWeightedCost");
                    DropDownList ddlNewUOM = (DropDownList)tr.FindControl("ddlNewUOM");

                    lblListPrice.Text = (GMSUtil.ToDecimal(ds.Tables[0].Rows[0]["ListPrice"].ToString())).ToString("#0.00");
                    if (!chkIsNewCustomer.Checked && txtAccountCode.Text.Trim() != "" && txtAccountCode.Text.StartsWith("DI"))
                    {
                        lblMinSellingPrice.Text = (GMSUtil.ToDecimal(ds.Tables[0].Rows[0]["IntercoPrice"].ToString())).ToString("#0.00");
                    }
                    else
                    {
                        lblMinSellingPrice.Text = (GMSUtil.ToDecimal(ds.Tables[0].Rows[0]["MinSellingPrice"].ToString())).ToString("#0.00");
                    }
                    ddlNewUOM.SelectedValue = ds.Tables[0].Rows[0]["UOM"].ToString();
                    hidWeightedCost.Value = (GMSUtil.ToDecimal(ds.Tables[0].Rows[0]["WeightedCost"].ToString())).ToString("#0.00");
                    txtNewProductDescription.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();
                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                    scriptManager.SetFocus(txtNewQuantity);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Product is not found!')", true);
                    txtNewProductCode.Text = "";
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('" + ex.Message + "')", true);
            }
        }
        #endregion



        #region Price_OnTextChanged
        protected void Price_OnTextChanged(object sender, EventArgs e)
        {

            LogSession session = base.GetSessionInfo();
            TextBox txtNewUnitPrice = (TextBox)sender;

            TableRow tr = (TableRow)txtNewUnitPrice.Parent.Parent;
            TextBox txtNewQuantity = (TextBox)tr.FindControl("txtNewQuantity");
            txtNewUnitPrice = (TextBox)tr.FindControl("txtNewUnitPrice");
            Label lblNewTotalPrice = (Label)tr.FindControl("lblNewTotalPrice");
            Label lblGPPercentage = (Label)tr.FindControl("lblGPPercentage");
            HtmlInputHidden hidWeightedCost = (HtmlInputHidden)tr.FindControl("hidWeightedCost");
            LinkButton lnkCreate = (LinkButton)tr.FindControl("lnkCreate");
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

            if (txtNewUnitPrice.Text.Trim() == "" || txtNewQuantity.Text.Trim() == "")
            {
                lblNewTotalPrice.Text = "";
                lblGPPercentage.Text = "";
                scriptManager.SetFocus(txtNewUnitPrice);
                return;
            }

            decimal unitPrice, qty, cost;
            if (decimal.TryParse(txtNewUnitPrice.Text.Trim(), out unitPrice) && decimal.TryParse(txtNewQuantity.Text.Trim(), out qty))
            {
                if (hidWeightedCost.Value.Trim() != "")
                {
                    decimal.TryParse(hidWeightedCost.Value.Trim(), out cost);
                    lblNewTotalPrice.Text = (unitPrice * qty).ToString();
                    if (unitPrice == 0)
                        lblGPPercentage.Text = "0.00";
                    else
                        lblGPPercentage.Text = ((unitPrice - cost) / unitPrice * 100).ToString("#0.00");
                    scriptManager.SetFocus(lnkCreate);
                }
                else
                {
                    lblNewTotalPrice.Text = (unitPrice * qty).ToString();
                    lblGPPercentage.Text = "";
                    scriptManager.SetFocus(lnkCreate);
                }
            }
            else
            {
                lblNewTotalPrice.Text = "";
                lblGPPercentage.Text = "";
                scriptManager.SetFocus(txtNewUnitPrice);
            }


        }
        #endregion

        #region txtAccountCode_OnTextChanged
        protected void txtAccountCode_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtAccountCode = (TextBox)sender;
            if (txtAccountCode.Text.Trim() != "")
            {
                A21Account acct = A21Account.RetrieveByKey(session.CompanyId, txtAccountCode.Text.Trim());
                if (acct != null && acct.AccountType == "C")
                {
                    DataSet ds = new DataSet();
                    (new QuotationDataDALC()).GetCustomerInfoByAccountCode(session.CompanyId, txtAccountCode.Text.Trim(), ref ds);
                    if (ddlSalesman.Items.FindByValue(ds.Tables[0].Rows[0]["SalesPersonID"].ToString()) != null)
                        this.ddlSalesman.SelectedValue = ds.Tables[0].Rows[0]["SalesPersonID"].ToString();
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You are not authorised for this customer!')", true);
                        txtAccountCode.Text = "";
                        txtAccountName.Text = "";
                        txtAccountCode.Focus();
                        txtAddress1.Text = "";
                        txtAddress2.Text = "";
                        txtAddress3.Text = "";
                        txtAddress4.Text = "";
                        txtAttentionTo.Text = "";
                        txtMobilePhone.Text = "";
                        txtOfficePhone.Text = "";
                        txtFax.Text = "";
                        txtEmail.Text = "";
                        return;
                    }

                    this.txtAccountCode.Text = this.txtAccountCode.Text.ToUpper();
                    this.txtAccountName.Text = acct.AccountName;
                    txtAddress1.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                    txtAddress2.Text = ds.Tables[0].Rows[0]["Address2"].ToString();
                    txtAddress3.Text = ds.Tables[0].Rows[0]["Address3"].ToString();
                    txtAddress4.Text = ds.Tables[0].Rows[0]["Address4"].ToString();
                    txtAttentionTo.Text = ds.Tables[0].Rows[0]["AttentionTo"].ToString();
                    txtMobilePhone.Text = ds.Tables[0].Rows[0]["MobilePhone"].ToString();
                    txtOfficePhone.Text = ds.Tables[0].Rows[0]["OfficePhone"].ToString();
                    txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
                    txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Account is not found!')", true);
                    txtAccountCode.Text = "";
                    txtAccountName.Text = "";
                    txtAccountCode.Focus();
                    txtAddress1.Text = "";
                    txtAddress2.Text = "";
                    txtAddress3.Text = "";
                    txtAddress4.Text = "";
                    txtAttentionTo.Text = "";
                    txtMobilePhone.Text = "";
                    txtOfficePhone.Text = "";
                    txtFax.Text = "";
                    txtEmail.Text = "";
                }
            }
            else
            {
                txtAccountName.Text = "";
                txtAccountCode.Focus();
                txtAddress1.Text = "";
                txtAddress2.Text = "";
                txtAddress3.Text = "";
                txtAddress4.Text = "";
                txtAttentionTo.Text = "";
                txtMobilePhone.Text = "";
                txtOfficePhone.Text = "";
                txtFax.Text = "";
                txtEmail.Text = "";
                txtAccountCode.Focus();
            }
        }
        #endregion

        #region ddlTaxType_SelectedIndexChanged
        protected void ddlTaxType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TaxType rate = TaxType.RetrieveByKey(session.CompanyId, ddlTaxType.SelectedValue);
            txtTaxRate.Text = (rate.TaxRate.Value * 100).ToString("#0.00") + "%";
            CalculateTotal();
        }
        #endregion

        #region btnHideMSP_Click
        protected void btnHideMSP_Click(object sender, EventArgs e)
        {
            Button btnHideMSP = (Button)sender;
            if (btnHideMSP.Text == "Hide MSP")
            {
                this.dgData.Columns[6].Visible = false;
                btnHideMSP.Text = "Show MSP";
            }
            else
            {
                this.dgData.Columns[6].Visible = true;
                btnHideMSP.Text = "Hide MSP";
            }
        }
        #endregion

        protected void CalculateTotal()
        {
            LogSession session = base.GetSessionInfo();

            decimal subTotal = 0;
            decimal cost = 0;
            bool noRight = false;
            foreach (DataGridItem item in dgData.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    Label lblTotalPrice = (Label)item.FindControl("lblTotalPrice");
                    subTotal += GMSUtil.ToDecimal(lblTotalPrice.Text.Trim());
                    HtmlInputHidden hidTotalCost = (HtmlInputHidden)item.FindControl("hidTotalCost");
                    cost += GMSUtil.ToDecimal(hidTotalCost.Value.Trim());
                    if (hidTotalCost.Value.Trim() == "")
                        noRight = true;
                }
            }



            if (dgPackage.Items.Count > 0) //Add package price and cost
            {
                string userRole = "N";
                DataSet dsUserRole = new DataSet();
                (new GMSGeneralDALC()).GetUserRoleByUserID(session.CompanyId, session.UserId, ref dsUserRole);
                if (dsUserRole != null && dsUserRole.Tables.Count > 0 && dsUserRole.Tables[0].Rows.Count > 0)
                {
                    userRole = dsUserRole.Tables[0].Rows[0]["UserRole"].ToString();
                }

                foreach (DataGridItem item in dgPackage.Items)
                {
                    if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                    {
                        Label lblQuantity = (Label)item.FindControl("lblQuantity");
                        HtmlInputHidden hidUnitPackageCost = (HtmlInputHidden)item.FindControl("hidUnitPackageCost");
                        Label lblUnitPackagePrice = (Label)item.FindControl("lblUnitPackagePrice");

                        if (userRole == "G" || userRole == "C") //only Group or Company user can view Package GP%
                        {
                            cost += (GMSUtil.ToDecimal(hidUnitPackageCost.Value.Trim()) * GMSUtil.ToDecimal(lblQuantity.Text.Trim()));
                        }
                        else
                        {
                            noRight = true;
                        }

                        subTotal += (GMSUtil.ToDecimal(lblUnitPackagePrice.Text.Trim()) * GMSUtil.ToDecimal(lblQuantity.Text.Trim()));
                    }
                }


            }

            this.lblSubTotal.Text = subTotal.ToString("#0.00");
            Decimal taxRate = GMSUtil.ToDecimal(this.txtTaxRate.Text.Trim().TrimEnd('%')) / 100;
            this.lblTaxAmount.Text = (subTotal * taxRate).ToString("#0.00");
            this.lblGrandTotal.Text = (GMSUtil.ToDecimal(this.lblSubTotal.Text) + GMSUtil.ToDecimal(this.lblTaxAmount.Text)).ToString("#0.00");
            this.lblGPercentage.ForeColor = System.Drawing.Color.Red;
            if (subTotal == 0)
                this.lblGPercentage.Text = "0.00";
            else if (noRight)
                this.lblGPercentage.Text = "";
            else
            {
                this.lblGPercentage.Text = ((subTotal - cost) / subTotal * 100).ToString("#0.00");
                if ((subTotal - cost) > 0)
                    this.lblGPercentage.ForeColor = System.Drawing.Color.Green;
            }

            if (lblQuotationNo.Text.Trim() != "" && ddlRevisionNo.SelectedValue.Trim() == "0")
            {
                QuotationHeader qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), 0);
                if (qh.SubTotal.Value != GMSUtil.ToDecimal(lblSubTotal.Text.Trim()) || qh.GrandTotal.Value != GMSUtil.ToDecimal(lblGrandTotal.Text.Trim()))
                {
                    qh.TaxTypeID = this.ddlTaxType.SelectedValue;
                    qh.TaxRate = GMSUtil.ToDecimal(this.txtTaxRate.Text.Trim().TrimEnd('%')) / 100;
                    qh.SubTotal = GMSUtil.ToDecimal(lblSubTotal.Text.Trim());
                    qh.GrandTotal = GMSUtil.ToDecimal(lblGrandTotal.Text.Trim());
                    qh.Save();
                    qh.Resync();
                }
            }
        }

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (!CheckAddEditAccess())
                return;

            Button butt = (Button)sender;
            if (butt.ID == "btnSubmit")
                SubmitForApproval();
            else
                SaveQuotationHeader(false);


            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            if (butt.ID == "btnSubmit")
            {
                str.Append("alert('Quotation submitted successfully!');");
                str.Append("window.location.href = \"AddEditQuotation2.aspx?ActiveTab=0&QuotationNo=" + lblQuotationNo.Text.Trim() + "\"");
            }
            else
                str.Append("alert('Quotation saved successfully!');");

            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);


        }
        #endregion



        #region btnApprove_Click
        /*protected void btnApprove_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            103);
            if (uAccess == null)
            {
                JScriptAlertMsg("You are not authorised to approve!");
                return;
            }

            QuotationHeader qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim());
            if (qh.IsNewCustomer.Value)
            {
                qh.QuotationStatusID = "2";
            }
            else
            {
                qh.QuotationStatusID = "1";
            }
            qh.Save();
            qh.Resync();

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("alert('Quotation has been approved!';");
            str.Append("window.location.href = \"AddEditQuotation.aspx?QuotationNo=" + qh.QuotationNo + "\"");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "approve", str.ToString(), false);
        }*/
        #endregion

        #region dgTNC_CreateCommand
        protected void dgTNC_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            #region Create
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();

                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                if (!CheckAddEditAccess())
                    return;

                bool NewQuotation = false;
                if (lblQuotationNo.Text.Trim() == "")
                {
                    SaveQuotationHeader(true);
                    NewQuotation = true;
                }

                //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                //                                                                100);
                //if (uAccess == null)
                //    Response.Redirect(base.UnauthorizedPage("Sales"));

                //TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");
                //DataTable dt = (DataTable)ViewState["TNC"];
                //DataRow row = dt.NewRow();
                //row["Name"] = System.Web.HttpUtility.HtmlEncode(System.Web.HttpUtility.UrlDecode(txtNewName.Text.Trim()));
                //dt.Rows.Add(row);
                //dgTNC.DataSource = dt;
                //dgTNC.DataBind();
                //ViewState["TNC"] = dt;

                IList<QuotationTermsNConditions> lstTNC = (new SystemDataActivity()).RetrieveAllTNCByQuotationNo(session.CompanyId,
                                        lblQuotationNo.Text.Trim(), 0);
                byte SeqID = 0;
                if (lstTNC != null && lstTNC.Count > 0)
                {
                    foreach (QuotationTermsNConditions tempTNC in lstTNC)
                    {
                        if (tempTNC.DSNNo >= SeqID)
                            SeqID = (byte)(tempTNC.DSNNo + 1);
                    }
                }

                TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");
                string termandcondition = txtNewName.Text.Trim();
                termandcondition = termandcondition.Replace("<", "< ");
                termandcondition = termandcondition.Replace(">", " >");

                QuotationTermsNConditions tc = new QuotationTermsNConditions();
                tc.CoyID = session.CompanyId;
                tc.QuotationNo = lblQuotationNo.Text.Trim();
                //tc.TermsNConditions = System.Web.HttpUtility.HtmlEncode(System.Web.HttpUtility.UrlDecode(txtNewName.Text.Trim()));
                tc.TermsNConditions = termandcondition;
                tc.RevisionNo = 0;
                tc.DSNNo = SeqID;
                tc.Save();
                tc.Resync();

                ViewState["TNC"] = null;
                LoadData();

                if (NewQuotation)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("alert('Quotation " + lblQuotationNo.Text.Trim() + " is created.');");
                    str.Append("window.location.href = \"AddEditQuotation2.aspx?ActiveTab=3&QuotationNo=" + lblQuotationNo.Text.Trim() + "\";");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "NewQuotation", str.ToString(), false);
                }
            }
            #endregion

            #region GoUp
            if (e.CommandName == "GoUp")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                Label lblName = (Label)e.Item.FindControl("lblName");

                QuotationTermsNConditions tnc = QuotationTermsNConditions.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), lblName.Text.Trim(), 0);
                if (tnc != null)
                {
                    IList<GMSCore.Entity.QuotationTermsNConditions> lstTNC = null;
                    try
                    {
                        lstTNC = (new SystemDataActivity()).RetrieveAllTNCByQuotationNo(session.CompanyId,
                                        lblQuotationNo.Text.Trim(), 0);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Delete", "alert('" + ex.Message + "')", true);
                    }

                    QuotationTermsNConditions swatTNC = null;
                    if (lstTNC != null && lstTNC.Count > 0)
                    {
                        foreach (QuotationTermsNConditions pDetail in lstTNC)
                        {
                            if (pDetail.DSNNo < tnc.DSNNo)
                                swatTNC = pDetail;
                        }
                        if (swatTNC != null)
                        {
                            foreach (QuotationTermsNConditions pDetail in lstTNC)
                            {
                                if (pDetail.DSNNo > swatTNC.DSNNo && pDetail.DSNNo < tnc.DSNNo)
                                    swatTNC = pDetail;
                            }
                        }
                    }
                    if (swatTNC != null && swatTNC.DSNNo < tnc.DSNNo)
                    {
                        byte swat = tnc.DSNNo.Value;
                        tnc.DSNNo = swatTNC.DSNNo.Value;
                        tnc.Save();
                        tnc.Resync();
                        swatTNC.DSNNo = swat;
                        swatTNC.Save();
                        swatTNC.Resync();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
                }
                ViewState["TNC"] = null;
                LoadData();
            }
            #endregion

            #region GoDown
            if (e.CommandName == "GoDown")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                Label lblName = (Label)e.Item.FindControl("lblName");

                QuotationTermsNConditions tnc = QuotationTermsNConditions.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), lblName.Text.Trim(), 0);
                if (tnc != null)
                {
                    IList<GMSCore.Entity.QuotationTermsNConditions> lstTNC = null;
                    try
                    {
                        lstTNC = (new SystemDataActivity()).RetrieveAllTNCByQuotationNo(session.CompanyId,
                                        lblQuotationNo.Text.Trim(), 0);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Delete", "alert('" + ex.Message + "')", true);
                    }

                    QuotationTermsNConditions swatTNC = null;
                    if (lstTNC != null && lstTNC.Count > 0)
                    {
                        foreach (QuotationTermsNConditions pDetail in lstTNC)
                        {
                            if (pDetail.DSNNo > tnc.DSNNo)
                                swatTNC = pDetail;
                        }
                        if (swatTNC != null)
                        {
                            foreach (QuotationTermsNConditions pDetail in lstTNC)
                            {
                                if (pDetail.DSNNo < swatTNC.DSNNo && pDetail.DSNNo > tnc.DSNNo)
                                    swatTNC = pDetail;
                            }
                        }
                    }
                    if (swatTNC != null && swatTNC.DSNNo > tnc.DSNNo)
                    {
                        byte swat = tnc.DSNNo.Value;
                        tnc.DSNNo = swatTNC.DSNNo.Value;
                        tnc.Save();
                        tnc.Resync();
                        swatTNC.DSNNo = swat;
                        swatTNC.Save();
                        swatTNC.Resync();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
                }
                ViewState["TNC"] = null;
                LoadData();
            }
            #endregion
        }
        #endregion

        #region dgTNC_DeleteCommand
        protected void dgTNC_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                if (!CheckAddEditAccess())
                    return;

                //DataTable dt = (DataTable)ViewState["TNC"];

                Label lblName = (Label)e.Item.FindControl("lblName");
                //dt.Rows.RemoveAt(GMSUtil.ToInt(lblSN.Text.Trim()) - 1);

                //this.dgTNC.DataSource = dt;
                //this.dgTNC.DataBind();
                //ViewState["TNC"] = dt;

                QuotationTermsNConditions tc = QuotationTermsNConditions.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), lblName.Text.Trim(), 0);
                if (tc != null)
                {
                    tc.Delete();
                    tc.Resync();
                }
                ViewState["TNC"] = null;
                LoadData();
            }
        }
        #endregion

        #region GenerateReport
        protected void GenerateReport(object sender, EventArgs e)
        {
            string selectedReport = ddlReport.SelectedValue;
            ScriptManager.RegisterStartupScript(upReportLinks, upReportLinks.GetType(), "Report1",
                string.Format("jsOpenOperationalReport('Finance/BankFacilities/ReportViewer.aspx?REPORT={0}&&TRNNO=" + this.lblQuotationNo.Text.Trim() + "&&REPORTID=-3');",
                                    selectedReport)
                                    , true);

            LoadData();
            //upReportLinks,upReportLinks.GetType(), "alert", javaScript, true
            ScriptManager.RegisterStartupScript(upReportLinks, upReportLinks.GetType(), "Report1", string.Format("SendAttach();", selectedReport), true);

        }
        #endregion

        #region GeneratePDFReport
        protected void GeneratePDFReport(object sender, EventArgs e)
        {

            string selectedReport = ddlReport.SelectedValue;
            ScriptManager.RegisterStartupScript(upReportLinks, upReportLinks.GetType(), "Report1",
                string.Format("jsOpenOperationalReport('Finance/BankFacilities/PdfReportViewer.aspx?REPORT={0}&&TRNNO=" + this.lblQuotationNo.Text.Trim() + "&&REPORTID=-3');",
                                    selectedReport)
                                    , true);


            LoadData();

        }
        #endregion




        #region GenerateEmailPDFReport
        protected void GenerateEmailPDFReport(object sender, EventArgs e)
        {

            LogSession session = base.GetSessionInfo();

            string selectedReport = ddlReport.SelectedValue;

            this.reportId = GMSUtil.ToShort("-3");
            this.trnNo = this.lblQuotationNo.Text.Trim();
            this.filePath = "C://GMS/Quotation/" + "Quotation_" + this.trnNo.ToString() + ".pdf";
            //this.filePath = "C:/Inetpub/wwwroot/GMSWeb/PDF/" + "Quotation_" + this.trnNo.ToString() + ".pdf";
            this.fileName = selectedReport.Trim() + ".rpt";
            try
            {
                crReportDocument = new ReportDocument();
                crReportDocument.Load(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.DOC_PATH + "\\" + fileName);

            }
            catch (Exception ex)
            {

                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('" + ex.Message + "')", true);
            }

            ConnectionInfo connection = new ConnectionInfo();
            connection.DatabaseName = DBManager.GetInstance().DatabaseName;
            connection.ServerName = DBManager.GetInstance().ServerName;
            connection.UserID = DBManager.GetInstance().UserLoginName;
            connection.Password = DBManager.GetInstance().UserLoginPwd;

            foreach (CrystalDecisions.CrystalReports.Engine.Table table in crReportDocument.Database.Tables)
            {
                // Cache the logon info block
                TableLogOnInfo logOnInfo = table.LogOnInfo;

                // Set the connection
                logOnInfo.ConnectionInfo = connection;

                // Apply the connection to the table!
                table.ApplyLogOnInfo(logOnInfo);
            }

            foreach (CrystalDecisions.CrystalReports.Engine.Section section in crReportDocument.ReportDefinition.Sections)
            {
                // In each section we need to loop through all the reporting objects
                foreach (CrystalDecisions.CrystalReports.Engine.ReportObject reportObject in section.ReportObjects)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject subReport = (SubreportObject)reportObject;
                        ReportDocument subDocument = subReport.OpenSubreport(subReport.SubreportName);

                        foreach (CrystalDecisions.CrystalReports.Engine.Table table in subDocument.Database.Tables)
                        {
                            // Cache the logon info block
                            TableLogOnInfo logOnInfo = table.LogOnInfo;

                            // Set the connection
                            logOnInfo.ConnectionInfo = connection;

                            // Apply the connection to the table!
                            table.ApplyLogOnInfo(logOnInfo);
                        }
                    }
                }
            }

            if (reportId == -3 && crReportDocument.ParameterFields["Quotation No"] != null)
                crReportDocument.SetParameterValue("Quotation No", trnNo);

            if (crReportDocument.ParameterFields["@CoyID"] != null)
                crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
            if (crReportDocument.ParameterFields["CoyID"] != null)
                crReportDocument.SetParameterValue("CoyID", session.CompanyId);
            if (crReportDocument.ParameterFields["@UserNumID"] != null)
                crReportDocument.SetParameterValue("@UserNumID", session.UserId);

            crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

            lblFileName.Text = "Quotation_" + this.trnNo.ToString() + ".pdf";

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsSalesPersonDetail = new DataSet();
            dacl.GetSalesPersonDetailsSelect(session.CompanyId, session.UserId, ref dsSalesPersonDetail);

            //Employee employee = new EmployeeActivity().RetrieveEmployeeByEmployeeName(session.UserId);
            string empName = "";
            string empDesignation = "";
            string attnTo = "";
            string bccemail = "";
            string did = "";
            string hp = "";

            attnTo = txtAttentionTo.Text;

            if (dsSalesPersonDetail.Tables[0].Rows.Count > 0)
            {

                empName = dsSalesPersonDetail.Tables[0].Rows[0]["SalesPersonName"].ToString();
                empDesignation = dsSalesPersonDetail.Tables[0].Rows[0]["Designation"].ToString();
                bccemail = dsSalesPersonDetail.Tables[0].Rows[0]["Email"].ToString();
                did = dsSalesPersonDetail.Tables[0].Rows[0]["DID"].ToString();
                hp = dsSalesPersonDetail.Tables[0].Rows[0]["MobilePhone"].ToString();

            }

            /*if (employee != null)
            {
                empName = employee.Name.ToString();
                empDesignation = employee.Designation.ToString();
                empDepartment = employee.Department.ToString();
            }
            */



            txtTo.Text = txtEmail.Text;
            txtBCC.Text = bccemail;

            txtEmailSubject.Text = "Quotation - " + this.trnNo.ToString();



            string signature = "";
            signature += "<P style=\"MARGIN: 0in 0in 0pt\" class=MsoNormal><SPAN style=\"FONT-FAMILY: Arial; COLOR: #002060; FONT-SIZE: 10pt; mso-ansi-language: EN-US; mso-no-proof: yes\">Dear " + attnTo + ",</SPAN></P>";
            signature += "<P style=\"MARGIN: 0in 0in 0pt\" class=MsoNormal><SPAN style=\"FONT-FAMILY: Arial; COLOR: #002060; FONT-SIZE: 10pt; mso-ansi-language: EN-US; mso-no-proof: yes\"></SPAN>&nbsp;</P>";
            signature += "<P style=\"MARGIN: 0in 0in 0pt\" class=MsoNormal><SPAN style=\"FONT-FAMILY: Arial; COLOR: #002060; FONT-SIZE: 10pt; mso-ansi-language: EN-US; mso-no-proof: yes\">Regards,<?xml:namespace prefix = o ns = \"urn:schemas-microsoft-com:office:office\" /><o:p></o:p></SPAN></P>";
            signature += "<P style=\"MARGIN: 0in 0in 0pt\" class=MsoNormal><B><SPAN style=\"FONT-FAMILY: Arial; COLOR: #002060; FONT-SIZE: 10pt; mso-ansi-language: EN-US; mso-no-proof: yes\">" + empName + "</SPAN></B><B><SPAN style=\"FONT-FAMILY: Arial; COLOR: black; mso-no-proof: yes\" lang=EN-SG><BR></SPAN></B><SPAN style=\"FONT-FAMILY: Arial; COLOR: #999999; FONT-SIZE: 10pt; mso-no-proof: yes\" lang=EN-SG>" + empDesignation + "</SPAN></P>";
            //signature += "<P style=\"MARGIN: 0in 0in 0pt\" class=MsoNormal><SPAN style=\"FONT-FAMILY: Arial; COLOR: #999999; FONT-SIZE: 10pt; mso-no-proof: yes\" lang=EN-SG><SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 10pt; mso-fareast-font-family: SimSun; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG><STRONG>Leeden Limited</STRONG></SPAN><SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 10pt; mso-fareast-font-family: SimSun; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG>&nbsp; <SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 8.5pt; mso-fareast-font-family: SimSun; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG>1</SPAN><SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 8.5pt; mso-fareast-font-family: SimSun; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG> Shipyard Road, <?xml:namespace prefix = st1 ns = \"urn:schemas-microsoft-com:office:smarttags\" /><st1:country-region w:st=\"on\">Singapore</st1:country-region></SPAN><SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 8.5pt; mso-fareast-font-family: SimSun; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG> 628128 </SPAN></SPAN></SPAN><SPAN style=\"COLOR: #999999; mso-bidi-font-family: Calibri; mso-no-proof: yes\" lang=EN-SG><o:p></o:p></SPAN></P>";
            //signature += "<P style=\"MARGIN: 0in 0in 0pt\" class=MsoNormal><SPAN style=\"FONT-FAMILY: Arial; COLOR: #999999; FONT-SIZE: 10pt; mso-no-proof: yes\" lang=EN-SG><SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 10pt; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-font-family: SimSun; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG><SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 8.5pt; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-font-family: SimSun; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG>";
            signature += "<P style=\"MARGIN: 0in 0in 0pt\" class=MsoNormal><SPAN style=\"FONT-FAMILY: Arial; COLOR: #999999; FONT-SIZE: 10pt; mso-no-proof: yes\" lang=EN-SG><SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 8.5pt; mso-fareast-font-family: SimSun; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG>EXT : " + did + "</SPAN><SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 10pt; mso-fareast-font-family: SimSun; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG>&nbsp; <SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 8.5pt; mso-fareast-font-family: SimSun; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG>HP : " + hp + "</SPAN></SPAN></SPAN><SPAN style=\"COLOR: #999999; mso-bidi-font-family: Calibri; mso-no-proof: yes\" lang=EN-SG><o:p></o:p></SPAN></P>";
            signature += "<P style=\"MARGIN: 0in 0in 0pt\" class=MsoNormal><SPAN style=\"FONT-FAMILY: Arial; COLOR: #999999; FONT-SIZE: 10pt; mso-no-proof: yes\" lang=EN-SG><SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 10pt; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-font-family: SimSun; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG><SPAN style=\"FONT-FAMILY: Arial; COLOR: black; FONT-SIZE: 8.5pt; mso-ansi-language: EN-SG; mso-no-proof: yes; mso-fareast-font-family: SimSun; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA\" lang=EN-SG>";



            FTBContent.Text = signature.ToString();


            ModalPopupExtender2.Hide();

            ModalPopupExtender3.Show();


            //ClientScript.RegisterStartupScript(GetType(), "Email", "OpenOutlookDoc('IPM.Note.FormA','" + fileURLPath + "');", true);




            /*
            if (System.IO.File.Exists(@"C:\Inetpub\wwwroot\GMSWeb\PDF\" + "Quotation_" + this.trnNo.ToString() + ".pdf"))
            {
                // Use a try block to catch IOExceptions, to 
                // handle the case of the file already being 
                // opened by another process. 
                try
                {
                    System.IO.File.Delete(@"C:\Inetpub\wwwroot\GMSWeb\PDF\" + "Quotation_" + this.trnNo.ToString() + ".pdf");
                }
                catch (Exception ex)
                {

                    ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('" + ex.Message + "')", true);
                }
            }
            */

            /*
            Microsoft.Office.Interop.Outlook.Application objApp = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook.MailItem mail = null;
            mail = (Microsoft.Office.Interop.Outlook.MailItem)objApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            //The CreateItem method returns an object which has to be typecast to MailItem 
            //before using it.
            mail.Attachments.Add((object)@"C:\Inetpub\wwwroot\GMSWeb\PDF\" + "Quotation_" + this.trnNo.ToString() + ".pdf", Microsoft.Office.Interop.Outlook.OlAttachmentType.olEmbeddeditem, 1, (object)"Attachment");
            mail.Display(true);
            */











        }
        #endregion

        #region SendApprovalEmail
        private void SendApprovalEmail(short userId, string quotationNo, string randomId)
        {
            GMSUser user = GMSUser.RetrieveByKey(userId);
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(user.UserEmail, user.UserRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[Quotation - Approval of Price]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + user.UserRealName + ",</p>" +
                        "<p>Please click the link below to approve or reject quotation " + quotationNo + ".<br />" +
                        "<a href=\"http://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS/Sales/Sales/AddEditQuotation2.aspx?RANDOMID=" + randomId + "\">Click here.</a></p>" +

                        "***** This is a computer-generated email. Please do not reply.*****";
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.Host = smtpServer;
                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
                mailClient.Credentials = authentication;
                mailClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region dgApproval_ItemCommand
        protected void dgApproval_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            if (this.lblStatus.Text.Trim() == "Unsuccessful")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Unsuccessful", "alert('You cannot modify unsuccessful quotation!')", true);
                return;
            }

            HtmlInputHidden hidApprovedUserID = (HtmlInputHidden)e.Item.FindControl("hidApprovedUserID");
            HtmlInputHidden hidApprovedAlternateUserID = (HtmlInputHidden)e.Item.FindControl("hidApprovedAlternateUserID");
            HtmlInputHidden hidApprovalType = (HtmlInputHidden)e.Item.FindControl("hidApprovalType");
            if (GMSUtil.ToShort(hidApprovedUserID.Value.Trim()) == session.UserId || GMSUtil.ToShort(hidApprovedAlternateUserID.Value.Trim()) == session.UserId)
            {
                TransactionApproval ta = TransactionApproval.RetrieveByKey(session.CompanyId, "QO", lblQuotationNo.Text.Trim(), GMSUtil.ToShort(hidApprovedUserID.Value.Trim()), hidApprovalType.Value.Trim());
                if (e.CommandName == "Approve")
                    ta.ApprovalStatus = "A";
                else if (e.CommandName == "Reject")
                    ta.ApprovalStatus = "R";
                ta.ApprovedByUserID = session.UserId;
                ta.ApprovalModifiedDate = DateTime.Now;
                ta.Save();

                DataSet ds = new DataSet();
                (new QuotationDataDALC()).GetTransactionApprovalWithLimitByQuotationNoSelect(session.CompanyId, lblQuotationNo.Text.Trim(), ref ds);
                bool isPendingForApproval = false;
                bool isRejected = false;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["ApprovalStatus"].ToString() == "R")
                            isRejected = true;
                        if (ds.Tables[0].Rows[i]["ApprovalStatus"].ToString() == "P")
                            isPendingForApproval = true;
                    }
                }

                QuotationHeader qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), 0);
                if (qh.QuotationStatusID != "5" && qh.QuotationStatusID != "2")
                {
                    if (isRejected)
                        qh.QuotationStatusID = "4";
                    else if (isPendingForApproval)
                        qh.QuotationStatusID = "3";
                    else
                        qh.QuotationStatusID = "1";
                    qh.Save();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Not Authorised", "alert('You are not authorised!')", true);
                return;
            }

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("alert('Quotation approved/rejected successfully!');");
            str.Append("window.location.href = \"AddEditQuotation2.aspx?QuotationNo=" + lblQuotationNo.Text.Trim() + "\"");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "approve/reject", str.ToString(), false);
        }
        #endregion

        #region dgApproval_ItemDataBound
        protected void dgApproval_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlInputHidden hidApprovalStatus = (HtmlInputHidden)e.Item.FindControl("hidApprovalStatus");
                HtmlInputHidden hidApprovedUserID = (HtmlInputHidden)e.Item.FindControl("hidApprovedUserID");
                HtmlInputHidden hidApprovedAlternateUserID = (HtmlInputHidden)e.Item.FindControl("hidApprovedAlternateUserID");
                Button lnkApprove = (Button)e.Item.FindControl("lnkApprove");
                Button lnkReject = (Button)e.Item.FindControl("lnkReject");
                if (hidApprovedUserID.Value.Trim() != session.UserId.ToString() && hidApprovedAlternateUserID.Value.Trim() != session.UserId.ToString())
                {
                    lnkApprove.Visible = false;
                    lnkReject.Visible = false;
                }
                else
                {
                    if (hidApprovalStatus.Value == "A")
                        lnkApprove.Visible = false;
                    if (hidApprovalStatus.Value == "R")
                        lnkReject.Visible = false;
                }
            }
        }
        #endregion

        #region ddlCurrency_SelectedIndexChanged
        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
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
            //Edit by Kim to if to change SGD to coy.DefaultCurrencyCode (this.ddlCurrency.SelectedValue != "SGD")
            if (this.ddlCurrency.SelectedValue != coy.DefaultCurrencyCode)
            {
                /*
                try
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    decimal currencyRate = sc.A21CurrencyRateByCurrencyCode(session.CompanyId, this.ddlCurrency.SelectedValue);
                    this.txtCurrencyRate.Text = currencyRate.ToString();
                }
                catch (Exception ex)
                {
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ex", "<script language='javascript'>alert('" + ex.Message + "');</script>", false);
                    //return;
                }
                */
                lblCurrency.Text = ddlCurrency.SelectedValue;
                lblCurrency2.Text = ddlCurrency.SelectedValue;
                lblCurrency3.Text = ddlCurrency.SelectedValue;
            }
            else
            {
                //this.txtCurrencyRate.Text = "1";
                lblCurrency.Text = ddlCurrency.SelectedValue;
                lblCurrency2.Text = ddlCurrency.SelectedValue;
                lblCurrency3.Text = ddlCurrency.SelectedValue;
            }
        }
        #endregion

        #region lnkCancelUnsuccessful_Click
        protected void lnkCancelUnsuccessful_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                 102);
            if (uAccess == null)
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You are not authorised!')", true);
                return;
            }

            if (this.chkIsNewCustomer.Checked && this.txtNewCustomerName.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Please enter customer name!')", true);
                return;
            }

            if (!this.chkIsNewCustomer.Checked && this.txtAccountCode.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Please enter customer account code!')", true);
                return;
            }

            if (ddlRevisionNo.SelectedValue != "0")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                return;
            }

            //LogSession session = base.GetSessionInfo();
            //QuotationHeader qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), 0);
            //qh.IsUnsuccessful = false;
            //qh.OrderLostReason = "";

            //DataSet dsTA = new DataSet();
            //(new QuotationDataDALC()).GetTransactionApprovalWithLimitByQuotationNoSelect(session.CompanyId, qh.QuotationNo, ref dsTA);
            //bool isPendingForApproval = false;
            //bool isRejected = false;
            //if (dsTA != null && dsTA.Tables.Count > 0 && dsTA.Tables[0].Rows.Count > 0)
            //{

            //    for (int i = 0; i < dsTA.Tables[0].Rows.Count; i++)
            //    {
            //        if (dsTA.Tables[0].Rows[i]["ApprovalStatus"].ToString() == "R")
            //            isRejected = true;
            //        if (dsTA.Tables[0].Rows[i]["ApprovalStatus"].ToString() == "P")
            //            isPendingForApproval = true;
            //    }
            //}

            //if (qh.QuotationStatusID == "5" )
            //{
            //    if (isRejected)
            //        qh.QuotationStatusID = "4";
            //    else if (isPendingForApproval)
            //        qh.QuotationStatusID = "3";
            //    else
            //        qh.QuotationStatusID = "1";
            //    qh.Save();
            //}
            chkIsUnsuccessful.Checked = false;
            SaveQuotationHeader(true);

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("alert('Quotation status reverted successfully!');");
            str.Append("window.location.href = \"AddEditQuotation2.aspx?QuotationNo=" + lblQuotationNo.Text.Trim() + "\"");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Cancel UnSuccessful", str.ToString(), false);
        }
        #endregion

        #region txtNewPackageProductCode_OnTextChanged
        protected void txtNewPackageProductCode_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtNewPackageProductCode = (TextBox)sender;

            if (txtNewPackageProductCode.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "Package", "alert('Please choose valid package!')", true);
                txtNewPackageProductCode.Text = "";
                return;
            }

            int packageID = GMSUtil.ToInt(txtNewPackageProductCode.Text.Trim().Split(' ')[0]);
            try
            {
                TableRow tr = (TableRow)txtNewPackageProductCode.Parent.Parent;
                TextBox txtNewProductDescription = (TextBox)tr.FindControl("txtNewProductDescription");
                HtmlInputHidden hidNewPackageID = (HtmlInputHidden)tr.FindControl("hidNewPackageID");
                DropDownList ddlNewUOM = (DropDownList)tr.FindControl("ddlNewUOM");
                Package pkg = Package.RetrieveByKey(packageID);
                if (pkg != null)
                {
                    txtNewProductDescription.Text = pkg.ProductDescription;
                    txtNewPackageProductCode.Text = pkg.ProductCode;
                    ddlNewUOM.SelectedValue = pkg.UOM;
                    hidNewPackageID.Value = pkg.PackageID.ToString();
                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                    scriptManager.SetFocus(txtNewProductDescription);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "Package", "alert('Package is not found!')", true);
                    txtNewPackageProductCode.Text = "";
                    hidNewPackageID.Value = "";
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "Package", "alert('" + ex.Message + "')", true);
            }
        }
        #endregion

        #region dgPackage_CreateCommand
        protected void dgPackage_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (!CheckAddEditAccess())
                return;

            #region Create
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                ViewState["TabNo"] = "2";
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                bool NewQuotation = false;
                if (lblQuotationNo.Text.Trim() == "")
                {
                    QuotationHeader qh = SaveQuotationHeader(true);
                    NewQuotation = true;
                }

                TextBox txtNewPackageProductCode = (TextBox)e.Item.FindControl("txtNewPackageProductCode");
                TextBox txtNewQuantity = (TextBox)e.Item.FindControl("txtNewQuantity");
                TextBox txtNewProductDescription = (TextBox)e.Item.FindControl("txtNewProductDescription");
                HtmlInputHidden hidNewPackageID = (HtmlInputHidden)e.Item.FindControl("hidNewPackageID");
                DropDownList ddlNewUOM = (DropDownList)e.Item.FindControl("ddlNewUOM");

                if (hidNewPackageID.Value.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Package is not valid!')", true);
                    return;
                }

                try
                {
                    /*
                    if (GMSUtil.ToDecimal(lblMinSellingPrice.Text.Trim()) > 0 && GMSUtil.ToDecimal(lblMinSellingPrice.Text.Trim()) > GMSUtil.ToDecimal(txtNewUnitPrice.Text.Trim()))
                    {
                        UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                           104);
                        if (uAccess == null)
                        {
                            ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Your price cannot be lower than minimum selling price!')", true);
                            return;
                        }
                    }
                    else if (GMSUtil.ToDecimal(lblMinSellingPrice.Text.Trim()) == 0)
                    {
                        UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                          104);
                        if (uAccess == null)
                        {
                            ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You do not have access to add product with 0 mininum price!')", true);
                            return;
                        }
                    }*/

                    byte maxDSNNo = 1;
                    QuotationDetailDataSet dsQuotationDetail = null;
                    if (ViewState["dsQuotationDetail"] != null)
                        dsQuotationDetail = (QuotationDetailDataSet)ViewState["dsQuotationDetail"];
                    else
                    {
                        dsQuotationDetail = new QuotationDetailDataSet();
                    }
                    if (dsQuotationDetail.QuotationDetail.Count > 0)
                    {
                        GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow maxRow = (GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow)(dsQuotationDetail.QuotationDetail.Select("", "DSNNo Desc"))[0];
                        if (maxRow != null && !maxRow.IsDSNNoNull())
                            maxDSNNo = (byte)(maxRow.DSNNo + 1);
                    }

                    IList<QuotationPackage> lstQuotationPackage = (new SystemDataActivity()).RetrieveAllPackageByQuotationNo(lblQuotationNo.Text.Trim(), session.CompanyId, 0);
                    byte snNo = 0;
                    if (lstQuotationPackage != null && lstQuotationPackage.Count > 0)
                    {
                        foreach (QuotationPackage tempQP in lstQuotationPackage)
                        {
                            if (tempQP.SNNo >= snNo)
                                snNo = (byte)(tempQP.SNNo + 1);
                            if (tempQP.DSNNo >= maxDSNNo)
                                maxDSNNo = (byte)(tempQP.DSNNo + 1);
                        }
                    }

                    QuotationPackage qp = new QuotationPackage();
                    qp.CoyID = session.CompanyId;
                    qp.QuotationNo = lblQuotationNo.Text.Trim();
                    qp.PackageID = GMSUtil.ToInt(hidNewPackageID.Value.Trim());
                    qp.SNNo = snNo;
                    qp.ProductCode = txtNewPackageProductCode.Text.Trim();
                    qp.ProductDescription = txtNewProductDescription.Text.Trim();
                    qp.Qty = GMSUtil.ToDecimal(txtNewQuantity.Text.Trim());
                    qp.UnitPrice = 0;
                    qp.CreatedBy = session.UserId;
                    qp.CreatedDate = DateTime.Now;
                    qp.DSNNo = maxDSNNo;
                    qp.UnitPackagePrice = 0;
                    qp.RevisionNo = 0;
                    qp.UOM = ddlNewUOM.SelectedValue.Trim();

                    decimal currencyRate = 1;
                    /*if (txtCurrencyRate.Text.Trim() != "")
                    {
                        currencyRate = GMSUtil.ToDecimal(txtCurrencyRate.Text.Trim());
                    }
                    */

                    if (currencyRate == 0) currencyRate = 1;
                    ProductsDataDALC dalc = new ProductsDataDALC();
                    DataSet ds = new DataSet();
                    dalc.GetProductPrice(session.CompanyId, txtNewPackageProductCode.Text.Trim(), session.UserId, ref ds);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        qp.Cost = GMSUtil.ToDecimal(ds.Tables[0].Rows[0]["WeightedCost"].ToString()) / currencyRate;
                        qp.UnitPackageCost = qp.Cost;
                    }
                    else
                    {
                        qp.Cost = 0;
                        qp.UnitPackageCost = 0;
                    }

                    qp.Save();
                    qp.Resync();

                    IList<GMSCore.Entity.PackageProduct> lstPackageProduct = null;
                    lstPackageProduct = new SystemDataActivity().RetrieveAllPackageProductByPackageID(GMSUtil.ToInt(hidNewPackageID.Value.Trim()));
                    if (lstPackageProduct != null && lstPackageProduct.Count > 0)
                    {
                        foreach (PackageProduct pp in lstPackageProduct)
                        {
                            QuotationPackageProduct qpp = new QuotationPackageProduct();
                            qpp.CoyID = session.CompanyId;
                            qpp.QuotationNo = lblQuotationNo.Text.Trim();
                            qpp.SNNo = qp.SNNo;
                            qpp.ProductCode = pp.ProductCode;
                            qpp.ProductDescription = pp.ProductDescription;
                            qpp.Qty = pp.Qty;
                            qpp.UnitPrice = 0;
                            qpp.RevisionNo = 0;

                            ds = new DataSet();
                            dalc.GetProductPrice(session.CompanyId, pp.ProductCode, session.UserId, ref ds);
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                qpp.Cost = GMSUtil.ToDecimal(ds.Tables[0].Rows[0]["WeightedCost"].ToString()) / currencyRate;
                                qp.UnitPackageCost += (qpp.Cost.Value * GMSUtil.ToDecimal(qpp.Qty.Value));
                            }
                            else
                            {
                                qpp.Cost = 0;
                            }

                            qpp.Save();
                            qpp.Resync();
                        }

                        qp.Save();
                        qp.Resync();
                    }
                    IList<GMSCore.Entity.PackageDetail> lstPackageDetail = null;
                    lstPackageDetail = new SystemDataActivity().RetrieveAllPackageDetailByPackageID(GMSUtil.ToInt(hidNewPackageID.Value.Trim()));
                    if (lstPackageDetail != null && lstPackageDetail.Count > 0)
                    {
                        byte count = 0;
                        foreach (PackageDetail pp in lstPackageDetail)
                        {
                            QuotationPackageDetail qpd = new QuotationPackageDetail();
                            qpd.CoyID = session.CompanyId;
                            qpd.QuotationNo = lblQuotationNo.Text.Trim();
                            qpd.SNNo = qp.SNNo;
                            qpd.DetailID = count;
                            qpd.Description = pp.Description;
                            qpd.RevisionNo = 0;
                            qpd.SeqID = count;
                            qpd.Save();
                            qpd.Resync();

                            count++;
                        }
                    }

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('" + ex.Message + "')", true);
                }

                LoadData();
                CalculateTotal();
                

                if (NewQuotation)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("alert('Quotation " + lblQuotationNo.Text.Trim() + " is created.');");
                    str.Append("window.location.href = \"AddEditQuotation2.aspx?ActiveTab=2&QuotationNo=" + lblQuotationNo.Text.Trim() + "\";");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "NewQuotation", str.ToString(), false);
                }
            }
            #endregion

            #region GoUp
            if (e.CommandName == "GoUp")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                HtmlInputHidden hidSNNo = (HtmlInputHidden)e.Item.FindControl("hidSNNo");

                QuotationPackage pkg = QuotationPackage.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), GMSUtil.ToByte(hidSNNo.Value.Trim()), 0);
                if (pkg != null)
                {
                    IList<GMSCore.Entity.QuotationPackage> lstPackage = null;
                    try
                    {
                        lstPackage = (new SystemDataActivity()).RetrieveAllPackageByQuotationNo(lblQuotationNo.Text.Trim(), session.CompanyId, 0);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Delete", "alert('" + ex.Message + "')", true);
                    }

                    QuotationPackage swatPackage = null;
                    if (lstPackage != null && lstPackage.Count > 0)
                    {
                        foreach (QuotationPackage pDetail in lstPackage)
                        {
                            if (pDetail.DSNNo < pkg.DSNNo)
                                swatPackage = pDetail;
                        }
                        if (swatPackage != null)
                        {
                            foreach (QuotationPackage pDetail in lstPackage)
                            {
                                if (pDetail.DSNNo > swatPackage.DSNNo && pDetail.DSNNo < pkg.DSNNo)
                                    swatPackage = pDetail;
                            }
                        }
                    }
                    if (swatPackage != null && swatPackage.DSNNo < pkg.DSNNo)
                    {
                        byte swat = pkg.DSNNo.Value;
                        pkg.DSNNo = swatPackage.DSNNo.Value;
                        pkg.Save();
                        pkg.Resync();
                        swatPackage.DSNNo = swat;
                        swatPackage.Save();
                        swatPackage.Resync();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
                }
                LoadData();
            }
            #endregion

            #region GoDown
            if (e.CommandName == "GoDown")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                HtmlInputHidden hidSNNo = (HtmlInputHidden)e.Item.FindControl("hidSNNo");

                QuotationPackage pkg = QuotationPackage.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), GMSUtil.ToByte(hidSNNo.Value.Trim()), 0);
                if (pkg != null)
                {
                    IList<GMSCore.Entity.QuotationPackage> lstPackage = null;
                    try
                    {
                        lstPackage = (new SystemDataActivity()).RetrieveAllPackageByQuotationNo(lblQuotationNo.Text.Trim(), session.CompanyId, 0);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Delete", "alert('" + ex.Message + "')", true);
                    }

                    QuotationPackage swatPackage = null;
                    if (lstPackage != null && lstPackage.Count > 0)
                    {
                        foreach (QuotationPackage pDetail in lstPackage)
                        {
                            if (pDetail.DSNNo > pkg.DSNNo)
                                swatPackage = pDetail;
                        }
                        if (swatPackage != null)
                        {
                            foreach (QuotationPackage pDetail in lstPackage)
                            {
                                if (pDetail.DSNNo < swatPackage.DSNNo && pDetail.DSNNo > pkg.DSNNo)
                                    swatPackage = pDetail;
                            }
                        }
                    }
                    if (swatPackage != null && swatPackage.DSNNo > pkg.DSNNo)
                    {
                        byte swat = pkg.DSNNo.Value;
                        pkg.DSNNo = swatPackage.DSNNo.Value;
                        pkg.Save();
                        pkg.Resync();
                        swatPackage.DSNNo = swat;
                        swatPackage.Save();
                        swatPackage.Resync();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
                }
                LoadData();
            }
            #endregion
        }
        #endregion

        #region dgPackage_EditCommand
        protected void dgPackage_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                           102);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            this.dgPackage.EditItemIndex = e.Item.ItemIndex;




            LoadData();
        }
        #endregion

        #region dgPackage_CancelCommand
        protected void dgPackage_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgPackage.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgPackage_UpdateCommand
        protected void dgPackage_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (!CheckAddEditAccess())
                return;

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            HtmlInputHidden hidSNNo = (HtmlInputHidden)e.Item.FindControl("hidSNNo");
            TextBox txtEditProductDescription = (TextBox)e.Item.FindControl("txtEditProductDescription");
            TextBox txtEditQuantity = (TextBox)e.Item.FindControl("txtEditQuantity");
            TextBox txtEditDSNNo = (TextBox)e.Item.FindControl("txtEditDSNNo");
            DropDownList ddlEditDOptSNNo = (DropDownList)e.Item.FindControl("ddlEditDOptSNNo");
            DropDownList ddlEditUOM = (DropDownList)e.Item.FindControl("ddlEditUOM");

            QuotationPackage pkg = QuotationPackage.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), GMSUtil.ToByte(hidSNNo.Value.Trim()), 0);
            if (pkg != null)
            {
                pkg.ProductDescription = txtEditProductDescription.Text.Trim();
                //pkg.UnitPackagePrice = pkg.UnitPackagePrice - pkg.UnitPrice;
                pkg.Qty = GMSUtil.ToDecimal(txtEditQuantity.Text.Trim());
                //pkg.UnitPackagePrice = pkg.UnitPackagePrice + pkg.UnitPrice;
                pkg.DSNNo = GMSUtil.ToByte(txtEditDSNNo.Text.Trim());
                pkg.DOptSNNo = ddlEditDOptSNNo.SelectedValue.Trim();
                pkg.UOM = ddlEditUOM.SelectedValue.Trim();
                pkg.Save();
                pkg.Resync();
                this.dgPackage.EditItemIndex = -1;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Update", "alert('Pacakge not found!')", true);
            }
            LoadData();
            CalculateTotal();
            
        }
        #endregion

        #region dgPackage_DeleteCommand
        protected void dgPackage_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (!CheckAddEditAccess())
                return;

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            HtmlInputHidden hidSNNo = (HtmlInputHidden)e.Item.FindControl("hidSNNo");

            QuotationPackage pkg = QuotationPackage.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), GMSUtil.ToByte(hidSNNo.Value.Trim()), 0);
            if (pkg != null)
            {
                SystemDataActivity sActivity = new SystemDataActivity();
                IList<QuotationPackageProduct> lstPackageProduct = sActivity.RetrieveAllPackageProductByQuotationNoSNNo(session.CompanyId,
                                                           lblQuotationNo.Text.Trim(), GMSUtil.ToByte(hidSNNo.Value.Trim()), 0);
                for (int i = lstPackageProduct.Count - 1; i >= 0; i--)
                {
                    QuotationPackageProduct pp = lstPackageProduct[i];
                    pp.Delete();
                    pp.Resync();
                }

                pkg.Delete();
                pkg.Resync();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Update", "alert('Pacakge not found!')", true);
            }
            LoadData();
            CalculateTotal();
           
        }
        #endregion

        #region dgPackage_ItemDataBound
        protected void dgPackage_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string userRole = "N";
            DataSet dsUserRole = new DataSet();
            (new GMSGeneralDALC()).GetUserRoleByUserID(session.CompanyId, session.UserId, ref dsUserRole);
            if (dsUserRole != null && dsUserRole.Tables.Count > 0 && dsUserRole.Tables[0].Rows.Count > 0)
            {
                userRole = dsUserRole.Tables[0].Rows[0]["UserRole"].ToString();
            }
            if (userRole == "G" || userRole == "C") //Only Group or Company user can view Package GP%
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblGP = (Label)e.Item.FindControl("lblGP");
                    Label lblUnitPackagePrice = (Label)e.Item.FindControl("lblUnitPackagePrice");
                    HtmlInputHidden hidSNNo = (HtmlInputHidden)e.Item.FindControl("hidSNNo");
                    HtmlInputHidden hidUnitPackageCost = (HtmlInputHidden)e.Item.FindControl("hidUnitPackageCost");

                    QuotationPackage pkg = QuotationPackage.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), GMSUtil.ToByte(hidSNNo.Value.Trim()), 0);
                    if (pkg != null && lblGP != null && hidUnitPackageCost != null && lblUnitPackagePrice != null)
                    {
                        decimal unitPackagePrice = 0;
                        if (lblUnitPackagePrice.Text.Trim() != "")
                            unitPackagePrice = GMSUtil.ToDecimal(lblUnitPackagePrice.Text.Trim());
                        decimal unitPackageCost = 0;
                        if (hidUnitPackageCost.Value.Trim() != "")
                            unitPackageCost = GMSUtil.ToDecimal(hidUnitPackageCost.Value.Trim());
                        decimal GPPercentage = 0;
                        if (unitPackagePrice > 0 && unitPackagePrice > unitPackageCost)
                            GPPercentage = (unitPackagePrice - unitPackageCost) / unitPackagePrice * 100;
                        lblGP.Text = GPPercentage.ToString("#0.00");
                        if (lblGP.Text.Trim() != "")
                        {
                            decimal gp = GMSUtil.ToDecimal(lblGP.Text.Trim());
                            if (gp > 0)
                                lblGP.ForeColor = System.Drawing.Color.Green;
                            else
                                lblGP.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");

                PopupControlExtender pce2 = e.Item.FindControl("PopupControlExtender2") as PopupControlExtender;

                string behaviorID = "pce2_" + e.Item.ItemIndex;
                pce2.BehaviorID = behaviorID;

                Image img = (Image)e.Item.FindControl("imgMagnify2");

                string OnMouseOverScript = string.Format("$find('{0}').showPopup();", behaviorID);
                string OnMouseOutScript = string.Format("$find('{0}').hidePopup();", behaviorID);

                img.Attributes.Add("onmouseover", OnMouseOverScript);
                img.Attributes.Add("onmouseout", OnMouseOutScript);
            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddlEditDOptSNNo = (DropDownList)e.Item.FindControl("ddlEditDOptSNNo");
                if (ddlEditDOptSNNo != null)
                {
                    ddlEditDOptSNNo.Items.Add(new ListItem("-", ""));
                    ddlEditDOptSNNo.Items.Add("A");
                    ddlEditDOptSNNo.Items.Add("B");
                    ddlEditDOptSNNo.Items.Add("C");
                    ddlEditDOptSNNo.Items.Add("D");
                    ddlEditDOptSNNo.Items.Add("E");
                    QuotationPackage qp = (QuotationPackage)e.Item.DataItem;
                    ddlEditDOptSNNo.SelectedValue = qp.DOptSNNo;
                }

                DropDownList ddlEditUOM = (DropDownList)e.Item.FindControl("ddlEditUOM");
                if (ddlEditUOM != null)
                {
                    DataSet dsTemp = new DataSet();
                    (new QuotationDataDALC()).GetAllUOMByCoyIDSelect(session.CompanyId, ref dsTemp);
                    ddlEditUOM.DataSource = dsTemp;
                    ddlEditUOM.DataValueField = "UOM";
                    ddlEditUOM.DataTextField = "UOM";
                    ddlEditUOM.DataBind();
                    QuotationPackage qd = (QuotationPackage)e.Item.DataItem;
                    ddlEditUOM.SelectedValue = qd.UOM;
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewUOM = (DropDownList)e.Item.FindControl("ddlNewUOM");
                DataSet dsTemp = new DataSet();
                (new QuotationDataDALC()).GetAllUOMByCoyIDSelect(session.CompanyId, ref dsTemp);
                ddlNewUOM.DataSource = dsTemp;
                ddlNewUOM.DataValueField = "UOM";
                ddlNewUOM.DataTextField = "UOM";
                ddlNewUOM.DataBind();
            }
        }
        #endregion

        #region btnAddToRevision_Click
        protected void btnAddToRevision_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                             102);
            if (uAccess == null)
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You are not authorised!')", true);
                return;
            }

            if (lblQuotationNo.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Please save quotation before adding new revision!')", true);
                return;
            }

            if (ddlRevisionNo.SelectedValue != "0")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('This is not the most current version!')", true);
                return;
            }

            byte nxtRevNo = (byte)ddlRevisionNo.Items.Count;

            QuotationHeader qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), 0);
            qh.RevisionNo = nxtRevNo;
            qh.Save();

            qh = null;
            qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), 0);
            qh.CreatedBy = session.UserId;
            qh.CreatedDate = DateTime.Now;
            qh.Save();



            IList<QuotationDetail> qdList = (new SystemDataActivity()).RetrieveAllQuotationDetailListByCompanyCodeQuotationNo(session.CompanyId,
                                                                               lblQuotationNo.Text.Trim(), 0);
            for (int i = qdList.Count - 1; i >= 0; i--)
            {
                QuotationDetail qd = qdList[i];

                IList<QuotationDetailDescription> qddList = (new SystemDataActivity()).RetrieveAllQuotationDetailDescriptionListByCompanyCodeQuotationNoSNNo(session.CompanyId,
                                                                                                lblQuotationNo.Text.Trim(), qd.SNNo, 0);
                for (int j = qddList.Count - 1; j >= 0; j--)
                {
                    QuotationDetailDescription qdd = qddList[j];
                    qdd.RevisionNo = nxtRevNo;
                    qdd.Save();
                }

                qd.RevisionNo = nxtRevNo;
                qd.Save();
            }

            DataSet dsTemp = new DataSet();
            (new QuotationDataDALC()).GetQuotationTNCByQuotationNoSelect(session.CompanyId, lblQuotationNo.Text.Trim(), 0, ref dsTemp);
            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsTemp.Tables[0].Rows)
                {
                    QuotationTermsNConditions tc = QuotationTermsNConditions.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), r["Name"].ToString(), 0);
                    if (tc != null)
                    {
                        tc.RevisionNo = nxtRevNo;
                        tc.Save();
                    }
                }
            }

            IList<QuotationPackage> lstQuotationPackage = (new SystemDataActivity()).RetrieveAllPackageByQuotationNo(lblQuotationNo.Text.Trim(), session.CompanyId, 0);
            if (lstQuotationPackage != null)
            {
                foreach (QuotationPackage qp in lstQuotationPackage)
                {
                    SystemDataActivity sActivity = new SystemDataActivity();
                    IList<QuotationPackageProduct> lstPackageProduct = sActivity.RetrieveAllPackageProductByQuotationNoSNNo(session.CompanyId,
                                                               lblQuotationNo.Text.Trim(), qp.SNNo, 0);
                    for (int i = lstPackageProduct.Count - 1; i >= 0; i--)
                    {
                        QuotationPackageProduct pp = lstPackageProduct[i];
                        pp.RevisionNo = nxtRevNo;
                        pp.Save();
                    }

                    IList<QuotationPackageDetail> lstPackageDetail = sActivity.RetrieveAllPackageDetailByQuotationNoSNNo(session.CompanyId,
                                                               lblQuotationNo.Text.Trim(), qp.SNNo, 0);
                    for (int i = lstPackageDetail.Count - 1; i >= 0; i--)
                    {
                        QuotationPackageDetail pd = lstPackageDetail[i];
                        pd.RevisionNo = nxtRevNo;
                        pd.Save();
                    }

                    qp.RevisionNo = nxtRevNo;
                    qp.Save();
                }
            }

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("alert('Quotation revision added successfully!');");
            str.Append("window.location.href = \"AddEditQuotation2.aspx?QuotationNo=" + qh.QuotationNo + "\"");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }
        #endregion

        #region ddlRevisionNo_SelectedIndexChanged
        protected void ddlRevisionNo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            ViewState["dsQuotationDetail"] = null;
            ViewState["TNC"] = null;
            LoadData();

            if (ddlRevisionNo.SelectedValue == "0")
            {
                btnDuplicate.Visible = true;
                btnAddToRevision.Visible = true;
                btnSubmit.Visible = true;
                btnSave.Visible = true;
                btnDeleteRevision.Visible = false;
                btnApplyDiscount.Visible = true;
            }
            else
            {
                btnDuplicate.Visible = false;
                btnAddToRevision.Visible = false;
                btnSubmit.Visible = false;
                btnSave.Visible = false;
                btnDeleteRevision.Visible = false;//Disable Deletion of Revision
                btnApplyDiscount.Visible = false;
            }
        }
        #endregion

        #region btnDeleteRevision_Click
        protected void btnDeleteRevision_Click(object sender, EventArgs e)
        {
            return;
            LogSession session = base.GetSessionInfo();

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                             102);
            if (uAccess == null)
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You are not authorised!')", true);
                return;
            }

            if (ddlRevisionNo.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You cannot delete this revision!')", true);
                return;
            }

            byte revNo = GMSUtil.ToByte(ddlRevisionNo.SelectedValue);

            IList<QuotationPackage> lstQuotationPackage = (new SystemDataActivity()).RetrieveAllPackageByQuotationNo(lblQuotationNo.Text.Trim(), session.CompanyId, revNo);
            if (lstQuotationPackage != null)
            {
                foreach (QuotationPackage qp in lstQuotationPackage)
                {
                    SystemDataActivity sActivity = new SystemDataActivity();
                    IList<QuotationPackageProduct> lstPackageProduct = sActivity.RetrieveAllPackageProductByQuotationNoSNNo(session.CompanyId,
                                                               lblQuotationNo.Text.Trim(), qp.SNNo, revNo);
                    for (int i = lstPackageProduct.Count - 1; i >= 0; i--)
                    {
                        QuotationPackageProduct pp = lstPackageProduct[i];
                        pp.Delete();
                        pp.Resync();
                    }

                    IList<QuotationPackageDetail> lstPackageDetail = sActivity.RetrieveAllPackageDetailByQuotationNoSNNo(session.CompanyId,
                                                              lblQuotationNo.Text.Trim(), qp.SNNo, revNo);
                    for (int i = lstPackageDetail.Count - 1; i >= 0; i--)
                    {
                        QuotationPackageDetail pd = lstPackageDetail[i];
                        pd.Delete();
                        pd.Resync();
                    }

                    qp.Delete();
                    qp.Resync();
                }
            }

            DataSet dsTemp = new DataSet();
            (new QuotationDataDALC()).GetQuotationTNCByQuotationNoSelect(session.CompanyId, lblQuotationNo.Text.Trim(), revNo, ref dsTemp);
            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsTemp.Tables[0].Rows)
                {
                    QuotationTermsNConditions tc = QuotationTermsNConditions.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), r["Name"].ToString(), revNo);
                    if (tc != null)
                    {
                        tc.Delete();
                        tc.Resync();
                    }
                }
            }

            IList<QuotationDetail> qdList = (new SystemDataActivity()).RetrieveAllQuotationDetailListByCompanyCodeQuotationNo(session.CompanyId,
                                                                               lblQuotationNo.Text.Trim(), revNo);
            for (int i = qdList.Count - 1; i >= 0; i--)
            {
                QuotationDetail qd = qdList[i];

                IList<QuotationDetailDescription> qddList = (new SystemDataActivity()).RetrieveAllQuotationDetailDescriptionListByCompanyCodeQuotationNoSNNo(session.CompanyId,
                                                                                                lblQuotationNo.Text.Trim(), qd.SNNo, revNo);
                for (int j = qddList.Count - 1; j >= 0; j--)
                {
                    QuotationDetailDescription qdd = qddList[j];
                    qdd.Delete();
                    qdd.Resync();
                }

                qd.Delete();
                qd.Resync();
            }

            QuotationHeader qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), revNo);
            qh.Delete();
            qh.Resync();

            ddlRevisionNo.SelectedValue = "0";

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("alert('Quotation revision deleted successfully!');");
            str.Append("window.location.href = \"AddEditQuotation2.aspx?QuotationNo=" + qh.QuotationNo + "\"");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }
        #endregion

        #region btnDuplicate_Click
        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                             102);
            if (uAccess == null)
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You are not authorised!')", true);
                return;
            }

            if (lblQuotationNo.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Please save quotation before adding new revision!')", true);
                return;
            }

            if (ddlRevisionNo.SelectedValue != "0")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('This is not the most current version!')", true);
                return;
            }

            string nxtQuotationNo = "";
            DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(session.CompanyId, (short)DateTime.Now.Year);
            if (documentNumber == null)
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
            }
            nxtQuotationNo = DateTime.Now.ToString("yy") + "-" + documentNumber.QuotationNo;

            string nxtStr = ((short)(short.Parse(documentNumber.QuotationNo) + 1)).ToString();
            for (int i = nxtStr.Length; i < documentNumber.QuotationNo.Length; i++)
            {
                nxtStr = "0" + nxtStr;
            }
            documentNumber.QuotationNo = nxtStr;
            documentNumber.Save();

            QuotationHeader qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), 0);
            qh.QuotationNo = nxtQuotationNo;
            qh.QuotationStatusID = "0";
            qh.CreatedDate = DateTime.Now;
            qh.CreatedBy = session.CompanyId;
            qh.Save();

            IList<QuotationDetail> qdList = (new SystemDataActivity()).RetrieveAllQuotationDetailListByCompanyCodeQuotationNo(session.CompanyId,
                                                                               lblQuotationNo.Text.Trim(), 0);
            for (int i = qdList.Count - 1; i >= 0; i--)
            {
                QuotationDetail qd = qdList[i];

                IList<QuotationDetailDescription> qddList = (new SystemDataActivity()).RetrieveAllQuotationDetailDescriptionListByCompanyCodeQuotationNoSNNo(session.CompanyId,
                                                                                                lblQuotationNo.Text.Trim(), qd.SNNo, 0);
                for (int j = qddList.Count - 1; j >= 0; j--)
                {
                    QuotationDetailDescription qdd = qddList[j];
                    qdd.QuotationNo = nxtQuotationNo;
                    qdd.Save();
                }

                qd.QuotationNo = nxtQuotationNo;
                qd.Save();
            }

            DataSet dsTemp = new DataSet();
            (new QuotationDataDALC()).GetQuotationTNCByQuotationNoSelect(session.CompanyId, lblQuotationNo.Text.Trim(), 0, ref dsTemp);
            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsTemp.Tables[0].Rows)
                {
                    QuotationTermsNConditions tc = QuotationTermsNConditions.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), r["Name"].ToString(), 0);
                    if (tc != null)
                    {
                        tc.QuotationNo = nxtQuotationNo;
                        tc.Save();
                    }
                }
            }

            IList<QuotationPackage> lstQuotationPackage = (new SystemDataActivity()).RetrieveAllPackageByQuotationNo(lblQuotationNo.Text.Trim(), session.CompanyId, 0);
            if (lstQuotationPackage != null)
            {
                foreach (QuotationPackage qp in lstQuotationPackage)
                {
                    SystemDataActivity sActivity = new SystemDataActivity();
                    IList<QuotationPackageProduct> lstPackageProduct = sActivity.RetrieveAllPackageProductByQuotationNoSNNo(session.CompanyId,
                                                               lblQuotationNo.Text.Trim(), qp.SNNo, 0);
                    for (int i = lstPackageProduct.Count - 1; i >= 0; i--)
                    {
                        QuotationPackageProduct pp = lstPackageProduct[i];
                        pp.QuotationNo = nxtQuotationNo;
                        pp.Save();
                    }

                    IList<QuotationPackageDetail> lstPackageDetail = sActivity.RetrieveAllPackageDetailByQuotationNoSNNo(session.CompanyId,
                                         lblQuotationNo.Text.Trim(), qp.SNNo, 0);
                    for (int i = lstPackageDetail.Count - 1; i >= 0; i--)
                    {
                        QuotationPackageDetail pd = lstPackageDetail[i];
                        pd.QuotationNo = nxtQuotationNo;
                        pd.Save();
                    }

                    qp.QuotationNo = nxtQuotationNo;
                    qp.Save();
                }
            }

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("alert('Quotation duplicated successfully!');");
            str.Append("window.location.href = \"AddEditQuotation2.aspx?QuotationNo=" + qh.QuotationNo + "\"");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }
        #endregion

        #region CheckAddEditAccess
        private bool CheckAddEditAccess()
        {
            LogSession session = base.GetSessionInfo();
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                 102);
            if (uAccess == null)
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You are not authorised!')", true);
                return false;
            }

            if (this.chkIsNewCustomer.Checked && this.txtNewCustomerName.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Please enter customer name!')", true);
                return false;
            }

            if (!this.chkIsNewCustomer.Checked && this.txtAccountCode.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Please enter customer account code!')", true);
                return false;
            }

            if (chkIsUnsuccessful.Checked && this.txtOrderLostReason.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Please enter order lost reason!')", true);
                return false;
            }

            if (ddlRevisionNo.SelectedValue != "0")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                return false;
            }

            if (this.lblStatus.Text.ToUpper() == "UNSUCCESSFUL")
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Please cancel the unsuccsseful status first!')", true);
                return false;
            }

            return true;
        }
        #endregion

        #region SaveQuotationHeader
        private QuotationHeader SaveQuotationHeader(bool ChangeToDraft)
        {
            LogSession session = base.GetSessionInfo();

            QuotationHeader qh = null;
            //CRM
            //AccountProspect prospect = null;

            if (this.lblQuotationNo.Text.Trim() != "")
                qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), 0);
            else
            {
                qh = new QuotationHeader();
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
                    //CRM
                    //documentNumber.ProspectNo = "0001";

                }
                qh.QuotationNo = DateTime.Now.ToString("yy") + "-" + documentNumber.QuotationNo;
                qh.RevisionNo = 0;
                //CRM
                /*
                if (this.chkIsNewCustomer.Checked)
                {
                    prospect = new AccountProspect();
                    prospect.AccountCode = "P" + DateTime.Now.ToString("yy") + "-" + documentNumber.ProspectNo;

                    string nxtProspectStr = ((short)(short.Parse(documentNumber.ProspectNo) + 1)).ToString();
                    for (int i = nxtProspectStr.Length; i < documentNumber.ProspectNo.Length; i++)
                    {
                        nxtProspectStr = "0" + nxtProspectStr;
                    }
                    documentNumber.ProspectNo = nxtProspectStr;
                    
                }
                */

                string nxtStr = ((short)(short.Parse(documentNumber.QuotationNo) + 1)).ToString();
                for (int i = nxtStr.Length; i < documentNumber.QuotationNo.Length; i++)
                {
                    nxtStr = "0" + nxtStr;
                }
                documentNumber.QuotationNo = nxtStr;
                documentNumber.Save();
            }

            #region Integration with A21
            /* Integration with A21
            if (qh.A21QuotationNo.HasValue)
            {
                try
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    DataTable dtQH = sc.A21QuotationUpdateStatusByA21TrnNo(session.CompanyId, qh.A21QuotationNo.Value);
                    if (qh.QuotationStatusID == "1" || qh.QuotationStatusID == "4" || qh.QuotationStatusID == "6")
                    {
                        DataRow[] dr = dtQH.Select("TrnNo = '" + qh.A21QuotationNo.ToString() + "'");
                        if (dr.Length > 0)
                        {
                            if (dr[0]["Completed"].ToString() == "0")
                            {
                                if (qh.QuotationStatusID != "6")
                                {
                                    qh.QuotationStatusID = "6";
                                    qh.Save();
                                }
                            }
                            else
                            {
                                if (qh.QuotationStatusID != "4")
                                {
                                    qh.QuotationStatusID = "4";
                                    qh.Save();
                                }
                            }
                        }
                        else
                        {
                            if (qh.QuotationStatusID != "1")
                            {
                                qh.QuotationStatusID = "1";
                                qh.Save();
                            }
                        }
                    }
                    qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim());
                }
                catch (Exception ex)
                {
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ex", "<script language='javascript'>alert('" + ex.Message + "');</script>", false);
                    //return;
                }
            }*/

            //if (qh.QuotationStatusID == "4" || qh.QuotationStatusID == "6")
            //{
            //    ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('You cannot modify this record as it has been fulfilled by Sales Order!')", true);
            //    return;
            //}
            #endregion

            qh.CoyID = session.CompanyId;
            qh.QuotationDate = GMSUtil.ToDate(this.txtQuotationDate.Text.Trim());
            if (ChangeToDraft || qh.QuotationStatusID == null || qh.QuotationStatusID.ToString().Trim() == "" || qh.QuotationStatusID.ToString() == "0")
            {
                qh.QuotationStatusID = "0"; //Draft
            }
            else
            {
                //if (chkIsNewCustomer.Checked)
                //qh.QuotationStatusID = "2"; //Pending For Customer Creation
            }

            if (hidAccountCode.Value.Trim() != "" && hidAccountCode.Value.Trim() != this.txtAccountCode.Text.Trim().ToUpper())
                qh.QuotationStatusID = "0";
            if (hidSalesPersonID.Value.Trim() != "" && hidSalesPersonID.Value.Trim() != this.ddlSalesman.SelectedValue)
                qh.QuotationStatusID = "0";
            if (qh.GrandTotal.HasValue && qh.GrandTotal.Value != GMSUtil.ToDecimal(lblGrandTotal.Text.Trim()))
                qh.QuotationStatusID = "0";

            if (chkIsUnsuccessful.Checked)
                qh.QuotationStatusID = "5"; //Unsuccessful

            if (this.chkIsNewCustomer.Checked)
            {
                qh.IsNewCustomer = true;
                qh.NewCustomerName = this.txtNewCustomerName.Text.Trim().ToUpper();
            }
            else
            {
                qh.IsNewCustomer = false;
                qh.AccountCode = this.txtAccountCode.Text.Trim().ToUpper();
            }
            qh.Subject = this.txtSubject.Text.Trim();
            if (chkIsUnsuccessful.Checked)
            {
                qh.OrderLostReason = this.txtOrderLostReason.Text.Trim();
            }
            else
            {
                qh.OrderLostReason = "";
            }
            qh.IsUnsuccessful = chkIsUnsuccessful.Checked;
            qh.CustomerPONo = this.txtCustomerPONo.Text.Trim();
            qh.AttentionTo = this.txtAttentionTo.Text.Trim();
            qh.MobilePhone = this.txtMobilePhone.Text.Trim();
            qh.OfficePhone = this.txtOfficePhone.Text.Trim();
            qh.Fax = this.txtFax.Text.Trim();
            qh.Email = this.txtEmail.Text.Trim();
            qh.Currency = this.ddlCurrency.SelectedValue;
            //qh.CurrencyRate = GMSUtil.ToDecimal(this.txtCurrencyRate.Text.Trim());
            qh.InternalRemarks = this.txtInternalRemarks.Text.Trim();
            qh.ExternalRemarks = this.txtExternalRemarks.Text.Trim();
            qh.TaxTypeID = this.ddlTaxType.SelectedValue;
            qh.TaxRate = GMSUtil.ToDecimal(this.txtTaxRate.Text.Trim().TrimEnd('%')) / 100;
            qh.Address1 = this.txtAddress1.Text.Trim();
            qh.Address2 = this.txtAddress2.Text.Trim();
            qh.Address3 = this.txtAddress3.Text.Trim();
            qh.Address4 = this.txtAddress4.Text.Trim();
            qh.SalesPersonID = this.ddlSalesman.SelectedValue;
            qh.SubTotal = GMSUtil.ToDecimal(lblSubTotal.Text.Trim());
            qh.GrandTotal = GMSUtil.ToDecimal(lblGrandTotal.Text.Trim());
            if (lblQuotationNo.Text.Trim() != "")
            {
                qh.ModifiedBy = session.UserId;
                qh.ModifiedDate = DateTime.Now;
            }
            else
            {
                qh.CreatedBy = session.UserId;
                qh.CreatedDate = DateTime.Now;
            }
            qh.Save();
            qh.Resync();

            // Add records into tbProspect
            //CRM
            /*
            if (this.chkIsNewCustomer.Checked && lblQuotationNo.Text.Trim() == "")
            {              

                prospect.CoyID = session.CompanyId;
                
                prospect.AccountName = this.txtNewCustomerName.Text.Trim().ToUpper();
                prospect.SalesPersonID = this.ddlSalesman.SelectedValue;
                prospect.DefaultCurrency = this.ddlCurrency.SelectedValue;
                prospect.Industry = "--";
                prospect.Country = "--";               
                prospect.CreditTerm = 0;
                prospect.CreditLimit = 0;
                prospect.Address1 = this.txtAddress1.Text.Trim();
                prospect.Address2 = this.txtAddress2.Text.Trim();
                prospect.Address3 = this.txtAddress3.Text.Trim();
                prospect.Address4 = this.txtAddress4.Text.Trim();            
                prospect.IsActive = true;
                prospect.CreatedBy = session.UserId;
                prospect.CreatedDate = DateTime.Now;
                prospect.Save();
                prospect.Resync();

            }
            */

            this.lblQuotationNo.Text = qh.QuotationNo;

            #region Integration with A21
            /* Integration with A21
            if (qh.QuotationStatusID == "1")
            {
                if (!qh.A21QuotationNo.HasValue)
                {
                    try
                    {
                        GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                        if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                        {
                            sc.Url = session.WebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                        int trnNo = sc.InsertA21QuotationHeader(session.CompanyId, qh.AccountCode, qh.Currency, qh.QuotationDate.Value, 1,
                                                                qh.QuotationNo, qh.Address1, qh.Address2, qh.Address3, qh.Address4,
                                                                qh.TaxTypeID, qh.TaxRate.Value, qh.SalesPersonID, decimal.Parse(lblGrandTotal.Text.Trim()));

                        qh.A21QuotationNo = trnNo;
                        qh.Save();
                        qh.Resync();
                    }
                    catch (Exception ex)
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ex", "<script language='javascript'>alert('" + ex.Message + "');</script>", false);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                        if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                        {
                            sc.Url = session.WebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                        sc.UpdateA21QuotationHeader(session.CompanyId, qh.A21QuotationNo.Value, qh.AccountCode, qh.Currency, qh.QuotationDate.Value, 1,
                                                                qh.QuotationNo, qh.Address1, qh.Address2, qh.Address3, qh.Address4,
                                                                qh.TaxTypeID, qh.TaxRate.Value, qh.SalesPersonID, decimal.Parse(lblGrandTotal.Text.Trim()));
                    }
                    catch (Exception ex)
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ex", "<script language='javascript'>alert('" + ex.Message + "');</script>", false);
                        return;
                    }
                }
            }*/
            #endregion

            return qh;
        }
        #endregion

        #region SaveQuotationDetail
        private void SaveQuotationDetail()
        {
            /* Comment out due to details are saved directly during Add, Update action
            QuotationDetailDataSet ds = (QuotationDetailDataSet)ViewState["dsQuotationDetail"];
            if (lblQuotationNo.Text.Trim() != "")
            {
                IList<QuotationDetail> qdList = (new SystemDataActivity()).RetrieveAllQuotationDetailListByCompanyCodeQuotationNo(session.CompanyId,
                                                                                lblQuotationNo.Text.Trim(), 0);
                for (int i = qdList.Count - 1; i >= 0; i--)
                {
                    QuotationDetail qd = qdList[i];

                    DataRow[] drs = ds.QuotationDetail.Select("SNNo = " + qd.SNNo.ToString());
                    if (drs.Length == 0)
                    {
                        IList<QuotationDetailDescription> qddList = (new SystemDataActivity()).RetrieveAllQuotationDetailDescriptionListByCompanyCodeQuotationNoSNNo(session.CompanyId,
                                                                                                    lblQuotationNo.Text.Trim(), qd.SNNo, 0);
                        for (int j = qddList.Count - 1; j >= 0; j--)
                        {
                            QuotationDetailDescription qdd = qddList[j];
                            qdd.Delete();
                            qdd.Resync();
                        }
                        qd.Delete();
                        qd.Resync();
                    }

                    /* Integration with A21
                    if (qh.QuotationStatusID == "1")
                    {
                        if (qh.A21QuotationNo.HasValue)
                        {
                            try
                            {
                                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                                {
                                    sc.Url = session.WebServiceAddress.Trim();
                                }
                                else
                                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                                sc.DeleteA21QuotationDetail(session.CompanyId, qh.A21QuotationNo.Value, i);
                            }
                            catch (Exception ex)
                            {
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ex", "<script language='javascript'>alert('" + ex.Message + "');</script>", false);
                                return;
                            }
                        }
                    }
                     * * /
                }
            }
            ArrayList approvalUsers = new ArrayList();
            if (ds != null && ds.QuotationDetail.Count >= 0 && ds.QuotationDetail.Rows.Count > 0)
            {
                GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow maxRow = (GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow)(ds.QuotationDetail.Select("", "SNNo Desc"))[0];
                byte max = 0;
                if (!maxRow.IsSNNoNull())
                    max = maxRow.SNNo;

                for (byte i = 0; i < ds.QuotationDetail.Rows.Count; i++)
                {
                    GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow row = (GMSCore.Entity.QuotationDetailDataSet.QuotationDetailRow)ds.QuotationDetail.Rows[i];
                    QuotationDetail qd = null;
                    if (!row.IsSNNoNull())
                    {
                        qd = QuotationDetail.RetrieveByKey(session.CompanyId, qh.QuotationNo, row.SNNo, 0);
                        qd.ModifiedBy = session.UserId;
                        qd.ModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        qd = new QuotationDetail();
                        qd.CoyID = session.CompanyId;
                        qd.QuotationNo = qh.QuotationNo;
                        qd.SNNo = ++max;
                        qd.CreatedBy = session.UserId;
                        qd.CreatedDate = DateTime.Now;
                        qd.RevisionNo = 0;
                        needSend = true;
                    }

                    qd.ProductCode = row.ProductCode;
                    qd.ProductDescription = row.ProductDescription.Trim().ToUpper();
                    if (qd.UnitPrice != row.UnitPrice || qd.Quantity != row.Quantity)
                        needSend = true;
                    qd.Quantity = row.Quantity;
                    qd.ListPrice = row.ListPrice;
                    qd.MinSellingPrice = row.MinSellingPrice;
                    qd.UnitPrice = row.UnitPrice;
                    qd.Cost = row.Cost;
                    qd.DSNNo = (byte)(i + 1);
                    qd.Save();
                    qd.Resync();
                    decimal cost = qd.Cost.Value;
                    if (txtAccountCode.Text.Trim().StartsWith("DI"))
                        cost = cost * (decimal)1.1;

                    

                    /* Integration with A21
                    if (qh.QuotationStatusID == "1")
                    {
                        if (qh.A21QuotationNo.HasValue)
                        {
                            try
                            {
                                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                                {
                                    sc.Url = session.WebServiceAddress.Trim();
                                }
                                else
                                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                                sc.InsertUpdateA21QuotationDetail(session.CompanyId, qh.A21QuotationNo.Value, qd.SNNo, qd.ProductCode, qd.Quantity.Value, row.UOM, qd.UnitPrice.Value, row.ProductCodeAndName.Replace(row.ProductCode + " ", ""));
                            }
                            catch (Exception ex)
                            {
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ex", "<script language='javascript'>alert('" + ex.Message + "');</script>", false);
                                return;
                            }
                        }
                    }* /
                }
            }

            DataTable dt = (DataTable)ViewState["TNC"];
            if (dt != null && dt.Rows.Count > 0)
            {
                if (lblQuotationNo.Text.Trim() != "")
                {
                    DataSet dsTemp = new DataSet();
                    (new QuotationDataDALC()).GetQuotationTNCByQuotationNoSelect(session.CompanyId, lblQuotationNo.Text.Trim(), 0, ref dsTemp);
                    if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow r in dsTemp.Tables[0].Rows)
                        {
                            QuotationTermsNConditions tc = QuotationTermsNConditions.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), r["Name"].ToString(), 0);
                            if (tc != null)
                            {
                                tc.Delete();
                                tc.Resync();
                            }
                        }
                    }
                }
                foreach (DataRow r in dt.Rows)
                {
                    QuotationTermsNConditions tc = new QuotationTermsNConditions();
                    tc.CoyID = session.CompanyId;
                    tc.QuotationNo = qh.QuotationNo;
                    tc.TermsNConditions = r["Name"].ToString().Trim();
                    tc.RevisionNo = 0;
                    tc.Save();
                    tc.Resync();
                }
            }
             */
        }
        #endregion

        #region SubmitForApproval
        private void SubmitForApproval()
        {
            if (!CheckAddEditAccess())
                return;

            LogSession session = base.GetSessionInfo();

            QuotationHeader qh = QuotationHeader.RetrieveByKey(session.CompanyId, lblQuotationNo.Text.Trim(), 0);

            /* Comment out approval
            if ((session.CompanyId == 1 && txtAccountCode.Text.Trim() == "DIL222") ||
                    session.CompanyId == 28 && txtAccountCode.Text.Trim() == "DIA001")
            {
                isInternal = true;
            }
            else
            {
                Company com = Company.RetrieveByKey(session.CompanyId);
                decimal subTotal = GMSUtil.ToDecimal(this.lblSubTotal.Text.Trim());
                TransactionApprovalParty tap = TransactionApprovalParty.RetrieveByKey(session.CompanyId, "QO", "Limit", "GM");
                if (tap != null)
                {
                    DataSet dsApprovalUser = new DataSet();
                    (new QuotationDataDALC()).GetQuotationApprovalParty(session.CompanyId, subTotal, ddlSalesman.SelectedValue, ref dsApprovalUser);

                    IList<TransactionApproval> approvalList = (new SystemDataActivity()).RetrieveAllQuotationLimitApprovalByQuotationNo(session.CompanyId, qh.QuotationNo);
                    if (dsApprovalUser != null && dsApprovalUser.Tables.Count > 0 && dsApprovalUser.Tables[0].Rows.Count > 0 && dsApprovalUser.Tables[0].Rows[0]["ApprovedUserID"].ToString() != "0" && !isInternal)
                    {
                        qh.QuotationStatusID = "3"; //Pending For Approval
                        if (approvalList == null || approvalList.Count <= 0)
                        {
                            for (int i = 0; i < dsApprovalUser.Tables[0].Rows.Count; i++)
                            {
                                TransactionApproval ta = new TransactionApproval();
                                ta.CoyID = session.CompanyId;
                                ta.TrnType = "QO";
                                ta.ApprovalType = "Limit";
                                ta.TrnNo = qh.QuotationNo;
                                ta.ApprovalStatus = "P";
                                ta.ApprovalCreatedDate = DateTime.Now;
                                ta.ApprovedUserID = GMSUtil.ToShort(dsApprovalUser.Tables[0].Rows[i]["ApprovedUserID"].ToString());
                                if (dsApprovalUser.Tables[0].Rows[i]["ApprovedAlternateUserID"] != null && dsApprovalUser.Tables[0].Rows[i]["ApprovedAlternateUserID"].ToString() != "")
                                    ta.ApprovedAlternateUserID = GMSUtil.ToShort(dsApprovalUser.Tables[0].Rows[i]["ApprovedAlternateUserID"].ToString());
                                ta.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("Quotation" + DateTime.Now.Ticks.ToString(), "MD5");
                                ta.Save();
                                SendApprovalEmail(ta.ApprovedUserID, qh.QuotationNo, ta.ApprovalRandomID);
                            }
                        }
                        else
                        {
                            if (needSend)
                            {
                                for (int i = approvalList.Count - 1; i >= 0; i--)
                                {
                                    TransactionApproval ta = approvalList[i];
                                    DataRow[] drs = dsApprovalUser.Tables[0].Select("ApprovedUserID = " + ta.ApprovedUserID.ToString());
                                    if (drs == null || drs.Length <= 0)
                                    {
                                        ta.Delete();
                                        ta.Resync();
                                    }
                                }
                                for (int i = 0; i < dsApprovalUser.Tables[0].Rows.Count; i++)
                                {
                                    TransactionApproval ta = TransactionApproval.RetrieveByKey(session.CompanyId, "QO", qh.QuotationNo,
                                                                GMSUtil.ToShort(dsApprovalUser.Tables[0].Rows[i]["ApprovedUserID"].ToString()), "Limit");
                                    if (ta != null)
                                    {
                                        if (ta.ApprovalStatus != "P")
                                        {
                                            ta.ApprovalStatus = "P";
                                            ta.ApprovalCreatedDate = DateTime.Now;
                                            ta.ApprovalModifiedDate = null;
                                            ta.ApprovedUserID = GMSUtil.ToShort(dsApprovalUser.Tables[0].Rows[i]["ApprovedUserID"].ToString());
                                            if (dsApprovalUser.Tables[0].Rows[i]["ApprovedAlternateUserID"] != null && dsApprovalUser.Tables[0].Rows[i]["ApprovedAlternateUserID"].ToString() != "")
                                                ta.ApprovedAlternateUserID = GMSUtil.ToShort(dsApprovalUser.Tables[0].Rows[i]["ApprovedAlternateUserID"].ToString());
                                            ta.Save();
                                            SendApprovalEmail(ta.ApprovedUserID, qh.QuotationNo, ta.ApprovalRandomID);
                                        }
                                    }
                                    else
                                    {
                                        ta = new TransactionApproval();
                                        ta.CoyID = session.CompanyId;
                                        ta.TrnType = "QO";
                                        ta.ApprovalType = "Limit";
                                        ta.TrnNo = qh.QuotationNo;
                                        ta.ApprovalStatus = "P";
                                        ta.ApprovalCreatedDate = DateTime.Now;
                                        ta.ApprovedUserID = GMSUtil.ToShort(dsApprovalUser.Tables[0].Rows[i]["ApprovedUserID"].ToString());
                                        if (dsApprovalUser.Tables[0].Rows[i]["ApprovedAlternateUserID"] != null && dsApprovalUser.Tables[0].Rows[i]["ApprovedAlternateUserID"].ToString() != "")
                                            ta.ApprovedAlternateUserID = GMSUtil.ToShort(dsApprovalUser.Tables[0].Rows[i]["ApprovedAlternateUserID"].ToString());
                                        ta.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("Quotation" + DateTime.Now.Ticks.ToString(), "MD5");
                                        ta.Save();
                                        SendApprovalEmail(ta.ApprovedUserID, qh.QuotationNo, ta.ApprovalRandomID);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        qh.QuotationStatusID = "1"; //Pending
                        if (approvalList != null && approvalList.Count > 0)
                        {
                            for (int i = approvalList.Count - 1; i >= 0; i--)
                            {
                                TransactionApproval ta = approvalList[i];
                                //DataRow[] drs = dsApprovalUser.Tables[0].Select("ApprovedUserID = " + ta.ApprovedUserID.ToString());
                                //if (drs == null || drs.Length <= 0)
                                //{
                                    ta.Delete();
                                    ta.Resync();
                                //}
                            }
                        }
                    }
                }
                 */

            /* Comment out approval
                if (cost > qd.UnitPrice && !isInternal)
                {
                    uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                           104);
                    if (uAccess == null)
                    {
                        DataSet dsPH = new DataSet();
                        (new QuotationDataDALC()).GetPHUserIDByProductCodeSelect(session.CompanyId, qd.ProductCode, ref dsPH);
                        short[] userId = new short[2];
                        if (dsPH != null && dsPH.Tables.Count > 0 && dsPH.Tables[0].Rows.Count > 0)
                        {
                            userId[0] = GMSUtil.ToShort(dsPH.Tables[0].Rows[0]["PHUserID"].ToString());
                            if (dsPH.Tables[0].Rows[0]["PH2UserID"] != null && dsPH.Tables[0].Rows[0]["PH2UserID"].ToString() != "")
                                userId[1] = GMSUtil.ToShort(dsPH.Tables[0].Rows[0]["PH2UserID"].ToString());
                            if (userId[0] <= 0)
                            {
                                TransactionApprovalParty user = TransactionApprovalParty.RetrieveByKey(session.CompanyId, "QO", "MSP", "GM");
                                if (user != null)
                                {
                                    userId[0] = user.ApprovedUserID;
                                    if (user.ApprovedAlternateUserID.HasValue)
                                        userId[1] = user.ApprovedAlternateUserID.Value;
                                }
                            }
                        }
                        else
                        {
                            TransactionApprovalParty user = TransactionApprovalParty.RetrieveByKey(session.CompanyId, "QO", "MSP", "GM");
                            if (user != null)
                            {
                                userId[0] = user.ApprovedUserID;
                                if (user.ApprovedAlternateUserID.HasValue)
                                    userId[1] = user.ApprovedAlternateUserID.Value;
                            }
                        }
                        if (!approvalUsers.Contains(userId) && userId[0] != 0)
                            approvalUsers.Add(userId);
                    }
                }
                else if (((qd.MinSellingPrice > 0 && qd.MinSellingPrice > qd.UnitPrice) || qd.MinSellingPrice == 0) && !isInternal)
                {
                    uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                       104);
                    if (uAccess == null)
                    {
                        short[] userId = new short[2];
                        TransactionApprovalParty user = TransactionApprovalParty.RetrieveByKey(session.CompanyId, "QO", "MSP", "GM");
                        if (user != null)
                        {
                            userId[0] = user.ApprovedUserID;
                            if (user.ApprovedAlternateUserID.HasValue)
                                userId[1] = user.ApprovedAlternateUserID.Value;
                        }
                        if (!approvalUsers.Contains(userId) && userId[0] != 0)
                            approvalUsers.Add(userId);
                    }
                }
                 */

            /* Comment out approval
        DataSet dsTAList = new DataSet();
        (new QuotationDataDALC()).GetTransactionApprovalByQuotationNoSelect(session.CompanyId, qh.QuotationNo, ref dsTAList);
        if (dsTAList != null && dsTAList.Tables.Count > 0 && dsTAList.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsTAList.Tables[0].Rows.Count; i++)
            {
                bool exists = false;
                for (int j = 0; j < approvalUsers.Count; j++)
                {
                    short[] userId = (short[])approvalUsers[j];
                    if (userId[0] == GMSUtil.ToShort(dsTAList.Tables[0].Rows[i]["ApprovedUserID"].ToString()))
                        exists = true;
                }
                if (!exists)
                {
                    TransactionApproval ta = TransactionApproval.RetrieveByKey(session.CompanyId, "QO", qh.QuotationNo, GMSUtil.ToShort(dsTAList.Tables[0].Rows[i]["ApprovedUserID"].ToString()), "MSP");
                    if (ta != null)
                    {
                        ta.Delete();
                        ta.Resync();
                    }
                }
            }
        }
        if (approvalUsers.Count > 0 && qh.QuotationStatusID != "5" && qh.QuotationStatusID != "2" && needSend && !isInternal)
        {
            for (int i = 0; i < approvalUsers.Count; i++)
            {
                short[] userId = (short[])approvalUsers[i];
                TransactionApproval ta = TransactionApproval.RetrieveByKey(session.CompanyId, "QO", qh.QuotationNo, userId[0], "MSP");
                if (ta == null)
                {
                    ta = new TransactionApproval();
                    ta.CoyID = session.CompanyId;
                    ta.TrnType = "QO";
                    ta.ApprovalType = "MSP";
                    ta.TrnNo = qh.QuotationNo;
                    ta.ApprovalStatus = "P";
                    ta.ApprovalCreatedDate = DateTime.Now;
                    ta.ApprovedUserID = userId[0];
                    if (userId[1] != null && userId[1] > 0)
                        ta.ApprovedAlternateUserID = userId[1];
                    ta.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("Quotation" + DateTime.Now.Ticks.ToString(), "MD5");
                    ta.Save();
                    SendApprovalEmail(ta.ApprovedUserID, qh.QuotationNo, ta.ApprovalRandomID);
                }
                else
                {
                    if (ta.ApprovalStatus != "P")
                    {
                        ta.ApprovalStatus = "P";
                        ta.ApprovalCreatedDate = DateTime.Now;
                        ta.ApprovalModifiedDate = null;
                        ta.ApprovedUserID = userId[0];
                        if (userId[1] != null && userId[1] > 0)
                            ta.ApprovedAlternateUserID = userId[1];
                        ta.Save();
                        SendApprovalEmail(ta.ApprovedUserID, qh.QuotationNo, ta.ApprovalRandomID);
                    }
                }
            }
        }
             
        }*/

            DataSet dsTA = new DataSet();
            (new QuotationDataDALC()).GetTransactionApprovalWithLimitByQuotationNoSelect(session.CompanyId, lblQuotationNo.Text.Trim(), ref dsTA);
            bool isPendingForApproval = false;
            bool isRejected = false;
            if (dsTA != null && dsTA.Tables.Count > 0 && dsTA.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dsTA.Tables[0].Rows.Count; i++)
                {
                    if (dsTA.Tables[0].Rows[i]["ApprovalStatus"].ToString() == "R")
                        isRejected = true;
                    if (dsTA.Tables[0].Rows[i]["ApprovalStatus"].ToString() == "P")
                        isPendingForApproval = true;
                }
            }

            if (qh.QuotationStatusID != "5" && qh.QuotationStatusID != "2")
            {
                if (isRejected)
                    qh.QuotationStatusID = "4";
                else if (isPendingForApproval)
                    qh.QuotationStatusID = "3";
                else
                    qh.QuotationStatusID = "1";
                qh.Save();
            }

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
            //padding:5px; margin:5px; background-color: #FEF6E8; border: 1px solid #969696;

            //b.Append("<table style='background-color:#f3f3f3; border: #336699 3px solid; ");
            b.Append("<table class='tInfoTable'>");
            //b.Append("width:350px; font-size:10pt; font-family:Verdana;' cellspacing='0' cellpadding='3'>");
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

        #region ApplyDiscount
        protected void ApplyDiscount(object sender, CommandEventArgs e)
        {
            if (!CheckAddEditAccess())
                return;

            if (GMSUtil.ToDecimal(lblSubTotal.Text.Trim()) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel2, this.GetType(), "click", "alert('Total Amount must be bigger than 0!')", true);
                return;
            }

            LogSession session = base.GetSessionInfo();

            decimal discountRate = 0;
            if (rblDiscountType.SelectedValue == "0")
            {
                discountRate = GMSUtil.ToDecimal(txtDiscountRate.Text.Trim()) / GMSUtil.ToDecimal(lblSubTotal.Text.Trim());
            }
            else
            {
                discountRate = GMSUtil.ToDecimal(txtDiscountRate.Text.Trim()) / 100;
            }

            IList<QuotationDetail> lstQuotationDetail = (new SystemDataActivity()).RetrieveAllQuotationDetailListByCompanyCodeQuotationNo(session.CompanyId, lblQuotationNo.Text.Trim(), 0);
            foreach (QuotationDetail qd in lstQuotationDetail)
            {
                qd.UnitPrice = qd.UnitPrice.Value * (1 - discountRate);
                qd.Save();
            }

            IList<QuotationPackage> lstQuotationPackage = (new SystemDataActivity()).RetrieveAllPackageByQuotationNo(lblQuotationNo.Text.Trim(), session.CompanyId, 0);
            foreach (QuotationPackage qp in lstQuotationPackage)
            {
                qp.UnitPrice = qp.UnitPrice.Value * (1 - discountRate);
                qp.Cost = qp.Cost.Value * (1 - discountRate);
                qp.UnitPackagePrice = qp.UnitPrice;
                qp.UnitPackageCost = qp.Cost;
                IList<QuotationPackageProduct> lstPackageProduct = new SystemDataActivity().RetrieveAllPackageProductByQuotationNoSNNo(session.CompanyId,
                                             lblQuotationNo.Text.Trim(), qp.SNNo, 0);
                foreach (QuotationPackageProduct qpp in lstPackageProduct)
                {
                    qpp.UnitPrice = qpp.UnitPrice.Value * (1 - discountRate);
                    qp.UnitPackagePrice = qp.UnitPackagePrice.Value + ((decimal)qpp.Qty * qpp.UnitPrice.Value);
                    qpp.Save();
                }
                qp.Save();
            }

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("alert('Discount applied to all product prices!');");
            str.Append("window.location.href = \"AddEditQuotation2.aspx?QuotationNo=" + lblQuotationNo.Text.Trim() + "\";");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "NewQuotation", str.ToString(), false);
        }
        #endregion


        public static bool isEmail(string inputEmail)
        {

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }


        protected void SendEmail(object sender, CommandEventArgs e)
        {

            String ckContentValue = FTBContent.Text.Trim();

            LogSession session = base.GetSessionInfo();
            GMSUser user = GMSUser.RetrieveByKey(session.UserId);
            System.Net.Mail.MailAddress fmEmailAddress = new System.Net.Mail.MailAddress(user.UserEmail, user.UserRealName);
            //System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress("keith.wong@leedenlimited.com", "Kim Yong, Wong");
            string smtpServer = "smtp.leedenlimited.com";


            string recipient = txtTo.Text;


            string cclist = txtCC.Text;
            string bcclist = txtBCC.Text;

            string[] emails = recipient.Split(';');
            string[] CCList = cclist.Split(';');
            string[] BCCList = bcclist.Split(';');

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = fmEmailAddress;

            foreach (string email in emails)
            {
                if (isEmail(email.ToString()))
                    mail.To.Add(email.ToString());
            }



            if (CCList.Length > 0)
            {
                foreach (string cc in CCList)
                {
                    if (isEmail(cc.ToString()))
                        mail.CC.Add(cc.ToString());

                }
            }

            if (BCCList.Length > 0)
            {
                foreach (string bcc in BCCList)
                {
                    if (isEmail(bcc.ToString()))
                        mail.Bcc.Add(bcc.ToString());

                }
            }





            mail.ReplyTo = new System.Net.Mail.MailAddress(user.UserEmail, user.UserRealName);
            mail.Body = ckContentValue.ToString();
            mail.BodyEncoding = System.Text.Encoding.UTF8;

            mail.Subject = txtEmailSubject.Text.Trim();
            System.Net.Mail.Attachment atth = new System.Net.Mail.Attachment("C:\\GMS\\Quotation\\" + "Quotation_" + lblQuotationNo.Text + ".pdf");
            mail.Attachments.Add(atth);



            mail.IsBodyHtml = true;


            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.Host = smtpServer;
                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
                mailClient.Credentials = authentication;
                mailClient.Send(mail);
                atth.Dispose();
                mail = null;
                ModalPopupExtender2.Hide();


                ScriptManager.RegisterStartupScript(upReportLinks, upReportLinks.GetType(), "Report1", "alert('Email has been sent successfully!');", true);

                //JScriptAlertMsg("Email has been sent successfully!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region DownloadPDFReport
        protected void DownloadPDFReport(object sender, EventArgs e)
        {
            //string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
            //string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
            //this.fileURLPath = strUrl + "GMSWeb/PDF/" + "Quotation_" + this.trnNo.ToString() + ".pdf";
            this.trnNo = this.lblQuotationNo.Text.Trim();
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Quotation_" + this.trnNo.ToString() + ".pdf");
            Response.TransmitFile("C://GMS/Quotation/" + "Quotation_" + this.trnNo.ToString() + ".pdf");
            Response.End();

        }
        #endregion

        protected void btnLoadInformation_Click(object sender, EventArgs args)
        {
            reset();
            btnLoadInformation.Attributes["class"] = "crm_button_selected";
            updatePanel1.Visible = true;
           

        }

        protected void btnLoadProductItem_Click(object sender, EventArgs args)
        {
            reset();
            btnLoadProductItem.Attributes["class"] = "crm_button_selected";
            updatePanel2.Visible = true;
          
        }

        protected void btnLoadPackageItem_Click(object sender, EventArgs args)
        {
            reset();
            btnLoadPackageItem.Attributes["class"] = "crm_button_selected";
            updatePanel4.Visible = true;
           
        }

        protected void btnLoadTerms_Click(object sender, EventArgs args)
        {

            reset();
            btnLoadTerms.Attributes["class"] = "crm_button_selected";
            updatePanel3.Visible = true;
            
        }

        protected void reset()
        {
            btnLoadInformation.Attributes["class"] = "crm_button";
            btnLoadProductItem.Attributes["class"] = "crm_button";
            btnLoadPackageItem.Attributes["class"] = "crm_button";
            btnLoadTerms.Attributes["class"] = "crm_button";

            updatePanel1.Visible = false;
            updatePanel2.Visible = false;
            updatePanel3.Visible = false;
            updatePanel4.Visible = false;

        }
    }
}
