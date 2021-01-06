<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="NewMaterialRequisition.aspx.cs" Inherits="GMSWeb.Products.Products.NewMaterialRequisition" Title="Material Requisition"  MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">


<script type="text/javascript">
// javascript to add to your aspx page

function test(a)
{
alert('456');

}

function setConsoleDate(a)
{
    if(a==true)
    {       
        document.getElementById('divConsoleDate').style.display = 'block';
        document.getElementById('divConsoleName').style.display = 'block';
        document.getElementById('divColon').style.display = 'block';
        
        
    }   
    else
    {
        document.getElementById('divConsoleDate').style.display = 'none';
        document.getElementById('divConsoleName').style.display = 'none';
        document.getElementById('divColon').style.display = 'none';
    }     

}

function confirm_approve()
{
    if (confirm("Are you sure you want to approve this MR?")==true)
     return true;
   else
     return false;
}


function confirm_submit_approval_normal()
{
    if (confirm("Are you sure you want to submit for approval?")==true)
     return true;
   else
     return false;
}


function confirm_delete()
 {
   if (confirm("Are you sure you want to delete this item?")==true)
     return true;
   else
     return false;
 }

function confirm_Permanently()
 {
   if (confirm("Are you sure you want to delete this item permanently?")==true)
     return true;
   else
     return false;
 } 
 
 function confirm_submit_approval()
 {
   if (confirm("Are you sure you want to submit for approval to purchasing directly as the PM/PH is NIL?")==true)
     return true;
   else
     return false;
 }
 


function btnDisabled(a) 
{

a.disabled=true;
}

function btnUploadClick()
{
document.getElementById('<%= btnUpdateFileUpload.ClientID %>').click();
}

function btnUploadAttachmentClick()
{
document.getElementById('<%= btnUpdateFileUploadAttachment.ClientID %>').click();
}


function btnSaveClick(a)
{
a.disabled=true;
document.getElementById('<%= btnUpdate.ClientID %>').click();
}



function getURLParameters(paramName) 
{
        var sURL = window.document.URL.toString();  
    if (sURL.indexOf("?") > 0)
    {
       var arrParams = sURL.split("?");         
       var arrURLParams = arrParams[1].split("&");      
       var arrParamNames = new Array(arrURLParams.length);
       var arrParamValues = new Array(arrURLParams.length);     
       var i = 0;
       for (i=0;i<arrURLParams.length;i++)
       {
        var sParam =  arrURLParams[i].split("=");
        arrParamNames[i] = sParam[0];
        if (sParam[1] != "")
            arrParamValues[i] = unescape(sParam[1]);
        else
            arrParamValues[i] = "No Value";
       }

       for (i=0;i<arrURLParams.length;i++)
       {
                if(arrParamNames[i] == paramName){
            //alert("Param:"+arrParamValues[i]);
                return arrParamValues[i];
             }
       }
       return "No Parameters Found";
    }

}

function Tab_SelectionChanged(sender,e)
{ 
    if(sender.get_activeTabIndex() == 0)
    {              
        document.getElementById('<%= btnLoadData.ClientID %>').click();
    }
} 

function checkSouceInit()
{
   
  
   var MRNo = getURLParameters("MRNo"); 
   
   if(document.getElementById('<%= chkAsset.ClientID %>').checked) 
     document.getElementById('<%= txtGLCode.ClientID %>').disabled=false;   
   else 
     document.getElementById('<%= txtGLCode.ClientID %>').disabled=true;      
    
   
   if(document.getElementById('<%= rbIsNotConsole.ClientID %>').checked)
   {
        document.getElementById('divConsoleDate').style.display = 'none';
        document.getElementById('divConsoleName').style.display = 'none';
        document.getElementById('divColon').style.display = 'none';
        
        
   }
   else
   {
        document.getElementById('divConsoleDate').style.display = 'block';
        document.getElementById('divConsoleName').style.display = 'block';
        document.getElementById('divColon').style.display = 'block';
   }
   
   
   
        
  
   if(MRNo == 'No Parameters Found')
   {
   
   
      
   if(document.getElementById('<%= ddlSource.ClientID %>').value == "Local")
   {
        document.getElementById('<%= rbAir.ClientID %>').disabled=true;
        document.getElementById('<%= rbSea.ClientID %>').disabled=true;
        document.getElementById('<%= rbCourier.ClientID %>').disabled=true;
        document.getElementById('<%= rbLand.ClientID %>').disabled=true;
   }
   else
   { 
       
        document.getElementById('<%= rbAir.ClientID %>').disabled=false;
        document.getElementById('<%= rbSea.ClientID %>').disabled=false;
        document.getElementById('<%= rbCourier.ClientID %>').disabled=false;
        document.getElementById('<%= rbLand.ClientID %>').disabled=false;
        
        //if(document.getElementById('<%= upVendorInformation.ClientID %>') != null && document.getElementById('<%= hidRole.ClientID %>').value == "N")
        if(document.getElementById('<%= upVendorInformation.ClientID %>') != null && document.getElementById('<%= hidMRRoleName.ClientID %>').value != "Purchasing" && document.getElementById('<%= hidMRRoleName.ClientID %>').value != "Product Team")        
        {
            document.getElementById('<%= upVendorInformation.ClientID %>').style.display = "none"; 
        }
        
        
   } 
   
   if(document.getElementById('<%= dgConfirmedSalesData.ClientID %>') != null)
   {
        
        if(document.getElementById('<%= chkSales.ClientID %>').checked)    
            document.getElementById('<%= dgConfirmedSalesData.ClientID %>').style.display = "block";
        else 
        {
           document.getElementById('<%= dgConfirmedSalesData.ClientID %>').style.display = "none";
        }
    }
   
   }
   else {
   
       if(document.getElementById('<%= ddlSource.ClientID %>').value == "Local")
       { 
            
       }
       
       if(document.getElementById('<%= dgConfirmedSalesData.ClientID %>') != null)
       {
            if(document.getElementById('<%= chkSales.ClientID %>').checked)    
                document.getElementById('<%= dgConfirmedSalesData.ClientID %>').style.display = "block";
            else 
            {
               document.getElementById('<%= dgConfirmedSalesData.ClientID %>').style.display = "none";
            }
        }
       
        
   }
   
    
    
        
}

function ReadOnlyCheckBox()
{

  var MRNo = getURLParameters("MRNo"); 
  
   if(MRNo != 'No Parameters Found')
   {
        
        return false;
   }
        
}


function reloadSetting()
{

    

   if(document.getElementById('<%= ddlSource.ClientID %>').value == "Local")
   {
       
        
   }
   else
   { 
       
        //if(document.getElementById('<%= upVendorInformation.ClientID %>') != null && document.getElementById('<%= hidRole.ClientID %>').value == "N")
        if(document.getElementById('<%= upVendorInformation.ClientID %>') != null && document.getElementById('<%= hidMRRoleName.ClientID %>').value != "Purchasing" && document.getElementById('<%= hidMRRoleName.ClientID %>').value != "Product Team")        
        {
            document.getElementById('<%= upVendorInformation.ClientID %>').style.display = "none"; 
        }
        
   }
   
    if (document.getElementById('<%= chkSales.ClientID %>').checked)
    {
       
        if(document.getElementById('<%= dgConfirmedSalesData.ClientID %>') != null)
        {
            
            document.getElementById('<%= dgConfirmedSalesData.ClientID %>').style.display = "block"; 
        }
    }
    else
    {  
        
        if(document.getElementById('<%= dgConfirmedSalesData.ClientID %>') != null)
        {
            
            document.getElementById('<%= dgConfirmedSalesData.ClientID %>').style.display = "none"; 
        }
    }
    

}



function checkAsset(a)
{
   var MRNo = getURLParameters("MRNo"); 
   if (a.checked)
        {
            document.getElementById('<%= txtGLCode.ClientID %>').disabled=false;
        }
        else
        {
            document.getElementById('<%= txtGLCode.ClientID %>').value="";
            document.getElementById('<%= txtGLCode.ClientID %>').disabled=true;
        }
}



function checkIntendedUse(a)
{
   var MRNo = getURLParameters("MRNo"); 
  
   if(MRNo != 'No Parameters Found')
   {
         /* 
   
         if (document.getElementById('<%= chkSales.ClientID %>').checked)
         {
              document.getElementById('<%= chkSales.ClientID %>').checked = false;  
              
         }
         else
         {
               document.getElementById('<%= chkSales.ClientID %>').checked = true; 
         }
         */
         
        if (a.checked)
        {   
             
            if(document.getElementById('<%= dgConfirmedSalesData.ClientID %>') != null)
            {           
                document.getElementById('<%= dgConfirmedSalesData.ClientID %>').style.display = "block"; 
            }
            else 
            {            
                
                //__doPostBack('<%= btnLoadData.ClientID %>', '');
                document.getElementById('<%= btnLoadData.ClientID %>').click();


                
            }
            
        }
        else
        {
           
            if(document.getElementById('<%= dgConfirmedSalesData.ClientID %>') != null)
            {
                
                document.getElementById('<%= dgConfirmedSalesData.ClientID %>').style.display = "none"; 
            }
        }
         
   }
   else 
   {  
       
    if (a.checked)
    {   
         
        if(document.getElementById('<%= dgConfirmedSalesData.ClientID %>') != null)
        {           
            document.getElementById('<%= dgConfirmedSalesData.ClientID %>').style.display = "block"; 
        }
        else 
        {            
            
            //__doPostBack('<%= btnLoadData.ClientID %>', '');
            document.getElementById('<%= btnLoadData.ClientID %>').click();


            
        }
        
    }
    else
    {
       
        if(document.getElementById('<%= dgConfirmedSalesData.ClientID %>') != null)
        {
            
            document.getElementById('<%= dgConfirmedSalesData.ClientID %>').style.display = "none"; 
        }
    }
    }
    
   
        
}
function checkSource(a)
{

    if(document.getElementById('<%= hidMRScheme.ClientID %>').value == "Product")
    {    
        

        if (a.value == "Local")
        {
                document.getElementById('<%= rbAir.ClientID %>').checked=false;
                document.getElementById('<%= rbSea.ClientID %>').checked=false;
                document.getElementById('<%= rbCourier.ClientID %>').checked=false;
                document.getElementById('<%= rbLand.ClientID %>').checked=false;
                document.getElementById('<%= rbAir.ClientID %>').disabled=true;
                document.getElementById('<%= rbSea.ClientID %>').disabled=true;
                document.getElementById('<%= rbCourier.ClientID %>').disabled=true;
                document.getElementById('<%= rbLand.ClientID %>').disabled=true;           
               
                if(document.getElementById('<%= upVendorInformation.ClientID %>') != null)
                {
                    document.getElementById('<%= upVendorInformation.ClientID %>').style.display = "block"; 
                }
                else {
                    document.getElementById('<%= btnLoadData.ClientID %>').click();
                }
        }
        else {
            
            document.getElementById('<%= rbAir.ClientID %>').disabled=false;
            document.getElementById('<%= rbSea.ClientID %>').disabled=false;
            document.getElementById('<%= rbCourier.ClientID %>').disabled=false;    
            document.getElementById('<%= rbLand.ClientID %>').disabled=false;
            document.getElementById('<%= rbAir.ClientID %>').checked=true;
            
            if(document.getElementById('<%= upVendorInformation.ClientID %>') != null && document.getElementById('<%= hidMRRoleName.ClientID %>').value != "Purchasing" && document.getElementById('<%= hidMRRoleName.ClientID %>').value != "Product Team")        
              document.getElementById('<%= upVendorInformation.ClientID %>').style.display = "none";    

            
        }
    }
    else
    {
        
            if(document.getElementById('<%= upVendorInformation.ClientID %>') != null)
            {
                    document.getElementById('<%= upVendorInformation.ClientID %>').style.display = "block"; 
            }
    
    }

}


//window.onload=checkSouceInit ;


var browserName=navigator.appName; // Get the Browser Name 

if(browserName=="Microsoft Internet Explorer") // For IE 
{ 

 window.onload=checkSouceInit; // Call init function in IE 
} 
else 
{ 

 if (document.addEventListener) // For Firefox 
 { 
 document.addEventListener("DOMContentLoaded", checkSouceInit, false); // Call init function in Firefox 
 } 
} 



</script>

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>

<asp:UpdatePanel ID="upOutter" runat="server" UpdateMode="Conditional">
            <Triggers>                
                <asp:PostBackTrigger ControlID="btnUpdate" /> 
                <asp:PostBackTrigger ControlID="btnUpdateFileUpload" />  
                <asp:PostBackTrigger ControlID="btnUpdateFileUploadAttachment" />              
                <asp:PostBackTrigger ControlID="btnLoadData" /> 
                <asp:PostBackTrigger ControlID="btnApprove" />                            
            </Triggers> 
            
            <ContentTemplate>
            <h1 style="width:100%;">Material Requisition</h1>
            <p>Create new material requisition or edit existing material requisition.</p>
            
            
            
            <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0" width="100%">
                 
                 
                 <tr>
                    <td class="tbLabel">
                       Source</td>
                    <td style="width:2%" valign="top">:</td>
                    <td>
                        
                        <asp:DropDownList ID="ddlSource" runat="server" CssClass="dropdownlist" onchange="javascript:checkSource(this);">
                                    <asp:ListItem Value="Local">Local</asp:ListItem>
                                    <asp:ListItem Value="Overseas">Overseas</asp:ListItem>                                   
                        </asp:DropDownList>
                    </td>
                    <td class="tbLabel">Status</td>
                    <td style="width:5%">:</td>
                    <td><asp:DropDownList CssClass="dropdownlist" ID="ddlStatus" runat="Server" DataTextField="StatusName" DataValueField="StatusID" /></td>       
                </tr>
                
                <tr>
                    <td class="tbLabel" valign="top">
                        <span style="color:Red; size:7px; font-style:italic;">*</span> Ship Via</td>
                    <td style="width:2%" valign="top">
                        :</td>
                    <td>
                        <table>
                        <tr><td><asp:RadioButton ID="rbAir" runat="server" Text="Air" GroupName="FreightMode" /></td><td><asp:RadioButton ID="rbSea" runat="server" Text="Sea" GroupName="FreightMode"  /></td></tr>
                        <tr><td><asp:RadioButton ID="rbCourier" runat="server" Text="Courier" GroupName="FreightMode"  /></td><td><asp:RadioButton ID="rbLand" runat="server" Text="Land" GroupName="FreightMode"  /></td></tr>
                        </table>                        
                    </td>
                    <td class="tbLabel">
                        MR No</td>
                    <td style="width:2%" valign="top">
                        :</td>
                    <td>    
                    <asp:Label ID="lblMRNo" runat="server" Text=""></asp:Label><asp:HiddenField ID="hidRole" runat="server" />
                    <asp:HiddenField ID="hidMRScheme" runat="server" />
                    <asp:HiddenField ID="hidMRRoleName" runat="server" />
                    </td>                   
                                
                 </tr>
                 <tr>
                <td class="tbLabel">
                        Requestor</td>
                    <td style="width:2%" valign="top">
                        :</td>
                    <td >
                        <asp:Label ID="lblRequestor" runat="server" Visible="false"></asp:Label><asp:HiddenField ID="hidRequestor" runat="server" /><asp:HiddenField ID="hidCreator" runat="server" />
                        <asp:DropDownList ID="ddlRequestor" runat="Server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestor_SelectedIndexChanged" />
                    </td>
                    <td class="tbLabel">Date</td>
                    <td style="width:2%" valign="top">                       
                        :</td>
                    <td>
                    <asp:TextBox runat="server" ID="txtMRDate" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                    <img id="img5" src="../../images/imgCalendar.gif" onclick="showCalendar(this, document.getElementById('ctl00_ContentPlaceHolderMain_txtMRDate'), 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0">
                                                     
                    </td>
                    
                
                </tr>
                <tr>                    
                    <td class="tbLabel" valign="top">
                        Remarks by Requestor</td>
                    <td style="width:2%" valign="top"> 
                        :</td>
                    <td >
                    <asp:TextBox runat="server" ID="txtRemarksByRequestor" TextMode="MultiLine" Width="180px"
                            Height="50" onfocus="select();" EnableViewState="true" onchange="this.value = this.value.toUpperCase()" MaxLength="1000"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revMessageBoardContentsRequestor" ControlToValidate ="txtRemarksByRequestor" Text="Exceeding 1000 characters" ValidationExpression="^[\s\S]{0,1000}$" runat="server" />
                    </td>
                    
                    <td class="tbLabel">Approver (1)<br /><br />Approver (2)<br /><br />Approver (3)</td>
                    <td style="width:2%" valign="top">                        
                        :<br /><br />:<br /><br />:</td>
                    <td>
                    <asp:Label ID="lblPM" runat="server" Visible="false"></asp:Label><asp:DropDownList ID="ddlPM" runat="Server" CssClass="dropdownlist" /><asp:HiddenField ID="hidPMUserId" runat="server" /><br /><br />         
                    <asp:Label ID="lblPH" runat="server" Visible="false"></asp:Label><asp:DropDownList ID="ddlPH" runat="Server" CssClass="dropdownlist" /><asp:HiddenField ID="hidPH" runat="server" /><br /><br />
                    <asp:Label ID="lblPH3" runat="server" Visible="false"></asp:Label><asp:DropDownList ID="ddlPH3" runat="Server" CssClass="dropdownlist" /><asp:HiddenField ID="hidPH3" runat="server" />  
                    <asp:HiddenField ID="hidPH2" runat="server" />                           
                    </td>         
                </tr> 
                
                <tr>
                    <td class="tbLabel" valign="top">
                        <span style="color:Red; size:7px; font-style:italic;">*</span> Intended Use</td>
                    <td style="width:2%" valign="top">
                        :</td>
                    <td >
                        <table>
                        <tr><td><asp:CheckBox ID="chkStock" runat="server" Text = "Stock" /></td><td><asp:CheckBox ID="chkSales" runat="server" Text = "Sales" onclick="javascript:checkIntendedUse(this);" /></td></tr>
                        <tr><td colspan="2"><asp:CheckBox ID="chkRepair" runat="server" Text = "Repair & Maintenance" /></td></tr>
                        <tr><td><asp:CheckBox ID="chkAsset" runat="server" Text = "Asset," onclick="javascript:checkAsset(this);" /></td><td>GL Code :</td><td><asp:TextBox ID="txtGLCode" runat="server" CssClass="textbox" Columns="6"></asp:TextBox></td></tr>
                        <tr><td><asp:CheckBox ID="chkSample" runat="server" Text = "Sample" /></td><td><asp:CheckBox ID="chkWorkshop" runat="server" Text = "Workshop / Others" /></td></tr>
                        <tr><td><asp:CheckBox ID="chkProject" runat="server" Text = "Project" /></td></tr>
                        </table>
                    </td> 
                    <td class="tbLabel">
                        Purchaser<br /><br /> Consol<div id="divConsoleName"><br />Console Date</div></td>
                    <td>
                        :<br /><br /> :<div id="divColon"><br />:</div></td>
                    <td>
                        <asp:Label ID="lblPurchaser" runat="server"></asp:Label>
                        <br /><br />
                        <asp:RadioButton ID="rbIsConsole" runat="server" Text="Yes" GroupName="Console" onClick="setConsoleDate(true)" />
                        <asp:RadioButton ID="rbIsNotConsole" runat="server" Text="No" GroupName="Console" Checked onClick="setConsoleDate(false)" />
                        <div id="divConsoleDate">
                        <br />                       
                        <asp:TextBox runat="server" ID="txtConsoleDate" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                        <img id="img2" src="../../images/imgCalendar.gif" onclick="showCalendar(this, document.getElementById('ctl00_ContentPlaceHolderMain_txtConsoleDate'), 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0">
                        </div>
                    </td>
                </tr>
                
                <tr>
                 <td class="tbLabel" valign="top">
                       Other Purchase Reasons</td>
                    <td style="width:2%" valign="top">
                        :</td>
                    <td >
                        <asp:TextBox runat="server" ID="txtOtherPurchaseReason" TextMode="MultiLine" Width="180px"
                            Height="50" onfocus="select();" EnableViewState="true" onchange="this.value = this.value.toUpperCase()" MaxLength="500"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revMessageBoardContentsPurchaseReason" ControlToValidate ="txtOtherPurchaseReason" Text="Exceeding 500 characters" ValidationExpression="^[\s\S]{0,500}$" runat="server" />
                    </td>
                    <td class="tbLabel" valign="top">
                        Cancelled Reason</td>
                    <td style="width:2%" valign="top">
                        :</td>
                    <td>                       
                        <asp:Label ID="lblCancelReason" runat="server"></asp:Label>
                        
                    </td>
                    
                </tr>
                <tr>
                     <td class="tbLabel" valign="top">
                       Budget Code</td>
                    <td style="width:2%" valign="top">
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtBudgetCode" MaxLength="100" Columns="20" onfocus="select();" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td class="tbLabel" valign="top">
                        Project No.</td>
                    <td style="width:2%" valign="top">
                        :</td>
                    <td>
                       <asp:TextBox runat="server" ID="txtProjectNo" MaxLength="100" Columns="20" onfocus="select();" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel" valign="top">
                       Ref. No.</td>
                    <td style="width:2%" valign="top">
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtRefNo" MaxLength="100" Columns="20" onfocus="select();" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td class="tbLabel" valign="top">
                       </td>
                    <td style="width:2%" valign="top">
                        </td>
                    <td>
                    </td>
                </tr>
            </table>
            <br />                      
                  
                    
                    
            <div style="display: none">
                 
                 <asp:Button ID="btnLoadData" runat="server" Text="" OnClick="btnLoadSales_Click" />
                 <asp:Button ID="btnUpdate" runat="server" Text="" OnClick="btnSave_Click" />
                 <asp:Button ID="btnUpdateFileUpload" runat="server" Text="" OnClick="btnUpdateFileUpload_Click" />
                 <asp:Button ID="btnUpdateFileUploadAttachment" runat="server" Text="" OnClick="btnUpdateFileUploadAttachment_Click" />                 
                 
            </div>
            <div style="margin-left: 10px;">         
            <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Width="100%" CssClass="ajax__tab_xp" >
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Confirmed Sales">
                    <ContentTemplate>
                        
                    <asp:UpdatePanel ID="upConfirmedSalesInformation" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="tTable1" cellspacing="2" cellpadding="2" width="99%">
                            <tr>
                                <td>
                                <div id="Div6" style="text-align: left;" runat="server">
                                    <asp:Label ID="lblPurchaseSummary" Visible="false" runat="server" />
                                </div>
                                </td>
                                </tr>                             
                            <tr><td>                 
                           <asp:DataGrid ID="dgConfirmedSalesData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true"
                                            OnItemCommand="dgConfirmedSalesData_Command" OnDeleteCommand="dgConfirmedSalesData_DeleteCommand" OnEditCommand="dgConfirmedSalesData_EditCommand"
                                            OnCancelCommand="dgConfirmedSalesData_CancelCommand" OnUpdateCommand="dgConfirmedSalesData_UpdateCommand" OnItemDataBound="dgConfirmedSalesData_ItemDataBound" GridLines="none" Width="99%" >
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgConfirmedSalesData.CurrentPageIndex) * dgConfirmedSalesData.PageSize)  %>'>
                                                        </asp:Label>
                                                        <input type="hidden" id="hidConfirmedSalesID" runat="server" value='<%# Eval("FileID")%>' />                                                       
                                                        .</ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Account Code">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
                                                            CausesValidation="false"><span><%# Eval("CustomerAccountCode")%></span></asp:LinkButton>                                                        
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewAccountCode" runat="server" Columns="8" MaxLength="15" CssClass="textbox"
                                                            onfocus="select();" />
                                                        <asp:RequiredFieldValidator ID="rfvNewAccountCode" runat="server" ControlToValidate="txtNewAccountCode"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="ConfirmedSales" />
                                                        <asp:LinkButton ID="lnkFindAccount" runat="server" CommandName="FindProduct" EnableViewState="true"
                                                            CssClass="FindButt" OnClientClick="return SearchAccount(this);"><IMG height="16" src="../../images/icons/FindItem.gif" align="absMiddle"></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Customer Name">
                                                    <ItemTemplate>
                                                        <%# Eval("CustomerName")%>                                                       
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditAccountName" runat="server" Columns="20"
                                                            MaxLength="50" Text='<%# Eval("CustomerName") %>' />
                                                        <asp:RequiredFieldValidator ID="rfvEditAccountName" runat="server" ControlToValidate="txtEditAccountName"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewAccountName" runat="server" Columns="20" MaxLength="50"
                                                            CssClass="textbox" onfocus="select();" onchange="this.value = this.value.toUpperCase()"  />
                                                        <asp:RequiredFieldValidator ID="rfvNewAccountName" runat="server" ControlToValidate="txtNewAccountName"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="ConfirmedSales" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Customer <br />PO No." HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <%# Eval("SONo")%>                                                       
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditSONo" runat="server" Columns="10"
                                                            MaxLength="50" Text='<%# Eval("SONo") %>'  />
                                                        <asp:RequiredFieldValidator ID="rfvEditSONo" runat="server" ControlToValidate="txtEditSONo"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewSONo" runat="server" Columns="10" MaxLength="50" CssClass="textbox" />
                                                        <asp:RequiredFieldValidator ID="rfvNewSONo" runat="server" ControlToValidate="txtNewSONo"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="ConfirmedSales" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Customer <br />PO Date" HeaderStyle-Width="130px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSODate" runat="server">
												    <%# Eval("SODate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("SODate", "{0: dd-MMM-yyyy}")%>
                                                    </asp:Label>                                                       
                                                    </ItemTemplate>
                                                    <EditItemTemplate>                                                                                                        
                                                        <asp:TextBox runat="server" ID="txtEditSODate" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox" Text=' <%# Eval("SODate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("SODate", "{0: dd-MMM-yyyy}")%>'> </asp:TextBox>
                                                    <asp:Image ID="imgCalendartxtEditSODate" runat="server" ImageUrl="../../images/imgCalendar.gif" onclick="showCalendar2(this, 'imgCalendartxtEditSODate', 'txtEditSODate', 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0" />  
                                                    
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtNewSODate" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                                    <asp:Image ID="imgCalendarNewSODate" runat="server" ImageUrl="../../images/imgCalendar.gif" onclick="showCalendar2(this, 'imgCalendarNewSODate', 'txtNewSODate', 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0" />  
                                                    <asp:RequiredFieldValidator ID="rfvNewSODate" runat="server" ControlToValidate="txtNewSODate"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="ConfirmedSales" />
                                                    <asp:RegularExpressionValidator ID="regexpNewSODate" runat="server" ErrorMessage="*" ControlToValidate="txtNewSODate" ValidationExpression="(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/\d{4}" ValidationGroup="ConfirmedSales"/>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Required Date" HeaderStyle-Width="130px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequiredDate" runat="server">
												    <%# Eval("RequiredDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("RequiredDate", "{0: dd-MMM-yyyy}")%>
                                                    </asp:Label>                                                       
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtEditRequiredDate" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox" Text='<%# Eval("RequiredDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("RequiredDate", "{0: dd-MMM-yyyy}")%>'></asp:TextBox>
                                                    <asp:Image ID="imgCalendarEditRequiredDate" runat="server" ImageUrl="../../images/imgCalendar.gif" onclick="showCalendar2(this, 'imgCalendarEditRequiredDate', 'txtEditRequiredDate', 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0" />  
                                                    
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtNewRequiredDate" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                                    <asp:Image ID="imgCalendarNewRequiredDate" runat="server" ImageUrl="../../images/imgCalendar.gif" onclick="showCalendar2(this, 'imgCalendarNewRequiredDate', 'txtNewRequiredDate', 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0" />  

                                                    <asp:RequiredFieldValidator ID="rfvNewRequiredDate" runat="server" ControlToValidate="txtNewRequiredDate"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="ConfirmedSales" />
                                                    
                                                    
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                 
                                                <asp:TemplateColumn HeaderText="Attachment">
                                                    <ItemTemplate>                                                       
                                                       <asp:LinkButton ID="linkName" runat="server" Text='<%# Eval("FileName")%>' CommandArgument='<%#Eval("FileNameEncrypted")%>' CommandName="Load" EnableViewState="true"
                                                            CausesValidation="false"></asp:LinkButton>                                                       
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                       <asp:LinkButton ID="linkNameEdit" runat="server" Text='<%# Eval("FileName")%>' CommandArgument='<%#Eval("FileNameEncrypted")%>' CommandName="Load" EnableViewState="true"
                                                            CausesValidation="false"></asp:LinkButton> 
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                    <asp:FileUpload CssClass="textbox" ID="FileUpload1" runat="server" Width="150px" onchange="btnUploadClick()"  />                                                    
                                                    <asp:Label ID="lblUpload" runat="server" Text=""></asp:Label>
                                                    
                                                                                                         
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>                                               
                                                                                               
                                                <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="true"
                                                            CausesValidation="false" CssClass="DeleteButt" OnClientClick="btnActionClick()"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="true"
                                                            ValidationGroup="valGrpEditRow" CssClass="SaveButt" OnClientClick="btnActionClick()"><span>Save</span></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="true"
                                                            CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>                                                                                                           
                                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="true"
                                                            ValidationGroup="ConfirmedSales" CssClass="NewButt" ><span>Add</span></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkBlank" runat="server" EnableViewState="true" ValidationGroup="Blank" CommandName="Blank"></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                        </asp:DataGrid>
                            </td></tr>
                            </table>                                                            
                        </ContentTemplate>   
                   </asp:UpdatePanel>  
            
                    </ContentTemplate>
                 </ajaxToolkit:TabPanel>                    
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Vendor">
                <ContentTemplate>
                   <asp:UpdatePanel ID="upVendorInformation" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>  
                            <table class="tTable1" cellspacing="5" cellpadding="5" width="99%">
                            <tr>                                
                                <td class="tbLabel">
                                Purchase Remarks</td>
                                <td>
                                    :</td>
                                <td >
                                <asp:TextBox runat="server" ID="txtVendorRemarks" TextMode="MultiLine" Width="90%"
                                        Height="50" onfocus="select();" EnableViewState="true" onchange="this.value = this.value.toUpperCase()" MaxLength="500"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revMessageBoardContentsVendor" ControlToValidate ="txtVendorRemarks" Text="Exceeding 500 characters" ValidationExpression="^[\s\S]{0,500}$" runat="server" />
                                </td>
                            </tr>  
                            <tr>
                            <td colspan="3">
                            <div id="Div1" style="text-align: left;" runat="server">
                                <asp:Label ID="lblVendorSummary" Visible="false" runat="server" />
                            </div>
                            </td>
                            </tr> 
                            <tr>
                            <td colspan="3">
                            <asp:DataGrid ID="dgVendorData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true"
                                            OnItemCommand="dgVendorData_Command" OnDeleteCommand="dgVendorData_DeleteCommand" OnEditCommand="dgVendorData_EditCommand"
                                            OnCancelCommand="dgVendorData_CancelCommand" OnUpdateCommand="dgVendorData_UpdateCommand"
                                            OnItemDataBound="dgVendorData_ItemDataBound" GridLines="none" Width="99%" >
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgVendorData.CurrentPageIndex) * dgVendorData.PageSize)  %>'>
                                                        </asp:Label>
                                                        <input type="hidden" id="hidVendorID" runat="server" value='<%# Eval("VendorID")%>' /> 
                                                        <input type="hidden" id="hidSupplierID" runat="server" value='<%# Eval("MRSupplierID")%>' /> 
                                                        .</ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Vendor">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkVendorEdit" runat="server" CommandName="Edit" EnableViewState="true"
                                                            CausesValidation="false"><span><%# Eval("VendorName") %></span></asp:LinkButton>                                                 
                                                                                                                                                                
                                                    </ItemTemplate>                                                    
                                                    <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtNewVendor" Columns="15" onfocus="select();" CssClass="textbox" MaxLength="50"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNewVendor" runat="server" ControlToValidate="txtNewVendor"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Vendor" />
                                                    <asp:LinkButton ID="lnkFindAccount" runat="server" CommandName="FindProduct" EnableViewState="true"
                                                            CssClass="FindButt" OnClientClick="return SearchSupplier(this);"><IMG height="16" src="../../images/icons/FindItem.gif" align="absMiddle"></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn> 
                                                <asp:TemplateColumn HeaderText="Attn To">
                                                    <ItemTemplate>
                                                        <%# Eval("VendorContact") %>                                                                                                      
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtEditAttnTo" Columns="15" MaxLength="50" onfocus="select();" CssClass="textbox" Text ='<%# Eval("VendorContact") %>'></asp:TextBox>                                                    
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtNewAttnTo" Columns="15" onfocus="select();" CssClass="textbox" MaxLength="50"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNewAttnTo" runat="server" ControlToValidate="txtNewAttnTo"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Vendor" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Tel">
                                                    <ItemTemplate>
                                                        <%# Eval("VendorTel") %>                                                                                                         
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtEditTel"  MaxLength="50" Columns="10" onfocus="select();" CssClass="textbox" Text = '<%# Eval("VendorTel") %>'></asp:TextBox>                                                    
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtNewTel" MaxLength="50" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                                    
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Fax">
                                                    <ItemTemplate>
                                                        <%# Eval("VendorFax") %>                                                                                                      
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtEditFax" MaxLength="50" Columns="10" onfocus="select();" CssClass="textbox" Text ='<%# Eval("VendorFax") %>'></asp:TextBox>                                                    
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtNewFax" MaxLength="50" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                                    
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Email">
                                                    <ItemTemplate>
                                                        <%# Eval("VendorEmail") %>                                                                                                        
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtEditEmail" MaxLength="50" Columns="15" onfocus="select();" CssClass="textbox" Text = '<%# Eval("VendorEmail") %>'></asp:TextBox>                                                    
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtNewEmail" MaxLength="50" Columns="15" onfocus="select();" CssClass="textbox"></asp:TextBox>                                                                                                       
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>                                                
                                                
                                                                                               
                                                <asp:TemplateColumn ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="true"
                                                            CausesValidation="false" CssClass="DeleteButt" OnClientClick="btnActionClick()"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton><br />
                                                         <asp:LinkButton ID="lnlDeletePermanently" runat="server" CommandName="DeletePermanently" EnableViewState="true" CommandArgument='<%#Eval("MRSupplierID")%>' 
                                                            CausesValidation="false" CssClass="DeleteButt" OnClientClick="btnActionClick()"><span>&nbsp;&nbsp;Del Permanently</span></asp:LinkButton>  
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="true"
                                                            ValidationGroup="valGrpEditRow" CssClass="SaveButt" OnClientClick="btnActionClick()"><span>Save</span></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="true"
                                                            CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="true"
                                                            ValidationGroup="Vendor" CssClass="NewButt" ><span>Add</span></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                        </asp:DataGrid>
                            </td>
                            </tr>
                           <tr>
                           <td colspan="3" align="right">
                            <asp:Button ID="btnConfirmVendor" runat="server" Text="Confirm Vendor Information" Visible="false" CssClass="button" OnClick="btnConfirmVendorInfo_Click"></asp:Button>
                           </td>
                           </tr> 
                            </table>
                        </ContentTemplate>
                   </asp:UpdatePanel> 
                   </ContentTemplate>
                   </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Product">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="upProductInformation" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="tTable1" cellspacing="5" cellpadding="5" width="99%">
                            <tr>
                            <td colspan="3">
                            <div id="Div2" style="text-align: left;" runat="server">
                                <asp:Label ID="lblProductSummary" Visible="false" runat="server" />
                            </div>
                            </td>
                            </tr>
                            <tr>
                            <td colspan="3">
                            <asp:DataGrid ID="dgProductData" runat="server" AutoGenerateColumns="false" ShowFooter="true" DataKeyField="DetailNo"
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true"
                                            OnItemCommand="dgProductData_Command" OnDeleteCommand="dgProductData_DeleteCommand" OnEditCommand="dgProductData_EditCommand"
                                            OnCancelCommand="dgProductData_CancelCommand" OnUpdateCommand="dgProductData_UpdateCommand"
                                            OnItemDataBound="dgProductData_ItemDataBound" GridLines="none" Width="99%" >
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgProductData.CurrentPageIndex) * dgProductData.PageSize)  %>'>
                                                        </asp:Label>
                                                        <input type="hidden" id="hidProductID" runat="server" value='<%# Eval("DetailNo")%>' />                                                        
                                                        .   
                                                                                                           
                                                        </ItemTemplate>
                                                        
                                                        
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="Product Code">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
                                                            CausesValidation="false"><span>
                                                                <asp:Label ID="lblProdCode" runat="server" Text='<%# Eval("ProdCode")%>'></asp:Label></span></asp:LinkButton><br />
                                                                <asp:Image ID="imgMagnify" runat="server" ImageUrl="../../images/icons/box_closed.png" /><br />
                                                            <asp:Label ID="lblNewProdCode" runat="server" Text='<%# Eval("NewProdCode")%>'></asp:Label>
                                                             <ajaxToolkit:PopupControlExtender ID="PopupControlExtender1" runat="server" PopupControlID="Panel1"
                                                            TargetControlID="imgMagnify" DynamicContextKey='<%# Eval("CoyID").ToString() + ";" + Eval("ProdCode").ToString() %>'
                                                            DynamicControlID="Panel1" DynamicServiceMethod="GetDynamicContent" Position="Right">
                                                        </ajaxToolkit:PopupControlExtender>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditProductCode" runat="server" Columns="12" MaxLength="15" CssClass="textbox" onfocus="select();" onchange="this.value = this.value.toUpperCase()" Text='<%# Eval("ProdCode")%>' OnTextChanged="txtEditProductCode_OnTextChanged" AutoPostBack="true" /><br />
                                                        <asp:Image ID="imgMagnify" runat="server" ImageUrl="../../images/icons/box_closed.png" />
                                                             <ajaxToolkit:PopupControlExtender ID="PopupControlExtender1" runat="server" PopupControlID="Panel1"
                                                            TargetControlID="imgMagnify" DynamicContextKey='<%# Eval("CoyID").ToString() + ";" + Eval("ProdCode").ToString() %>'
                                                            DynamicControlID="Panel1" DynamicServiceMethod="GetDynamicContent" Position="Right">
                                                        </ajaxToolkit:PopupControlExtender>
                                                        <asp:LinkButton ID="lnkFindProduct" runat="server" CommandName="FindProduct" EnableViewState="true"
                                                            CssClass="FindButt" OnClientClick="return SearchProduct(this,'txtEditProductCode');"><IMG height="16" src="../../images/icons/FindItem.gif" align="absMiddle"></asp:LinkButton><br />
                                                            <asp:Label ID="lblNewProdCode" runat="server">New ProdCode :</asp:Label><br />  
                                                            <asp:TextBox ID="txtEditNewProdCode" runat="server" Columns="12" MaxLength="15" CssClass="textbox" onfocus="select();" onchange="this.value = this.value.toUpperCase()" Text='<%# Eval("NewProdCode")%>' OnTextChanged="txtEditNewProdCode_OnTextChanged" AutoPostBack="true" />
                                                            <asp:HiddenField ID="hidEditPMUSERID" runat="server" Value='<%# Eval("PMUserID", "{0:f0}")%>' />
                                                            <asp:HiddenField ID="hidEditPHUSERID" runat="server" Value='<%# Eval("PHUserID", "{0:f0}")%>' />
                                                            <asp:HiddenField ID="hidEditPH2USERID" runat="server" Value='<%# Eval("PH2UserID", "{0:f0}")%>' />
                                                            <asp:HiddenField ID="hidEditPH3USERID" runat="server" Value='<%# Eval("PH3UserID", "{0:f0}")%>' />
                                                            <asp:HiddenField ID="hidEditOnHand" runat="server" Value='<%# Eval("OnHandQty", "{0:f2}")%>' />
                                                            <asp:HiddenField ID="hidEditOnOrder" runat="server" Value='<%# Eval("OnOrderQty", "{0:f2}")%>' />
                                                            <asp:HiddenField ID="hidEditOnPO" runat="server" Value='<%# Eval("OnPOQty", "{0:f2}")%>' />                                                                                                    
                                                    </EditItemTemplate>                                                    
                                                    <FooterTemplate>
                                                        <a name="ProductAddNew"></a>
                                                        <asp:TextBox ID="txtNewProductCode" runat="server" Columns="12" MaxLength="15" CssClass="textbox" onfocus="select();" onchange="this.value = this.value.toUpperCase()" OnTextChanged="txtNewProductCode_OnTextChanged" AutoPostBack="true" /><br />                                                                                                                           
                                                            <asp:Image ID="imgMagnify" runat="server" ImageUrl="../../images/icons/box_closed.png" />
                                                            <asp:LinkButton ID="lnkFindProduct" runat="server" CommandName="FindProduct" EnableViewState="true"
                                                            CssClass="FindButt" OnClientClick="return SearchProduct(this,'txtNewProductCode');"><IMG height="16" src="../../images/icons/FindItem.gif" align="absMiddle"></asp:LinkButton>
                                                            <ajaxToolkit:PopupControlExtender ID="PopupControlExtender1" runat="server" PopupControlID="Panel1"
                                                                TargetControlID="imgMagnify" DynamicContextKey=''
                                                                DynamicControlID="Panel1" DynamicServiceMethod="GetDynamicContent" Position="Right">
                                                            </ajaxToolkit:PopupControlExtender>
                                                            <br /><asp:Label ID="lblNewProductCode" runat="server" Text="New ProdCode : " Visible = "false"></asp:Label><br /><asp:TextBox ID="txtNewProdCode" runat="server" Visible="false" Columns="12" MaxLength="15" CssClass="textbox" onfocus="select();" onchange="this.value = this.value.toUpperCase()" OnTextChanged="txtNewProdCode_OnTextChanged" AutoPostBack="true"></asp:TextBox><br /> 
                                                            <asp:HiddenField ID="hidOnHand" runat="server" />
                                                            <asp:HiddenField ID="hidOnOrder" runat="server" />
                                                            <asp:HiddenField ID="hidOnPO" runat="server" />                                                           
                                                            <input type="hidden" id="hidPMUSERID" runat="server" value='' />
                                                            <input type="hidden" id="hidPHUSERID" runat="server" value='' />
                                                            <input type="hidden" id="hidPH2USERID" runat="server" value='' />
                                                            <input type="hidden" id="hidPH3USERID" runat="server" value='' />
                                                            <asp:RequiredFieldValidator ID="rfvNewProductCode" runat="server" ControlToValidate="txtNewProductCode"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Product" />
                                                        
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Product Description">
                                                    <ItemTemplate>
                                                        <%# Eval("ProdName")%>  
                                                        <asp:LinkButton ID="lnkEditDescription" runat="server" EnableViewState="true" CssClass="EditButt"
                                                            OnClientClick='<%# "return EditDescription(" + Eval("DetailNo").ToString() + ");" %>'
                                                            Visible=false>Details</asp:LinkButton>                                                                                                                    
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditProductDescription" runat="server" Columns="25" MaxLength="50" Text='<%# Eval("ProdName") %>' />  
                                                        <asp:RequiredFieldValidator ID="rfvEditProductDescription" runat="server" ControlToValidate="txtEditProductDescription"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProdRow" />
                                                        <br />
                                                        <asp:Label ID="lblEditPM" runat="server" Text="PM : " Visible = "false"></asp:Label><asp:DropDownList ID="ddlEditPM" runat="Server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlEditPM_SelectedIndexChanged" Visible="false" /><br />                                                       
                                                        <asp:Label ID="lblEditPH" runat="server" Text="PH : " Visible="false"></asp:Label><asp:DropDownList ID="ddlEditPH" runat="Server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlEditPH_SelectedIndexChanged" Visible="false" /><br />
                                                        <asp:Label ID="lblEditPH3" runat="server" Text="PH : " Visible="false"></asp:Label><asp:DropDownList ID="ddlEditPH3" runat="Server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlEditPH3_SelectedIndexChanged" Visible="false" /><br />   
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewProductDescription" runat="server" Columns="25" MaxLength="50"
                                                            CssClass="textbox" onfocus="select();" onchange="this.value = this.value.toUpperCase()" AutoPostBack="true" />
                                                            <asp:RequiredFieldValidator ID="rfvNewProductDescription" runat="server" ControlToValidate="txtNewProductDescription"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Product" />
                                                        <br />                                                        
                                                        <asp:Label ID="lblNewPM" runat="server" Text="PM : " Visible = "false"></asp:Label><asp:DropDownList ID="ddlNewPM" runat="Server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlNewPM_SelectedIndexChanged" Visible="false" /><br />                                                       
                                                        <asp:Label ID="lblNewPH" runat="server" Text="PH : " Visible="false"></asp:Label><asp:DropDownList ID="ddlNewPH" runat="Server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlNewPH_SelectedIndexChanged" Visible="false" /><br />
                                                        <asp:Label ID="lblNewPH3" runat="server" Text="PH : " Visible="false"></asp:Label><asp:DropDownList ID="ddlNewPH3" runat="Server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlNewPH3_SelectedIndexChanged" Visible="false" /><br />
                                                        
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="UOM" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <%# Eval("UOM")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlEditUOM">
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hidUOM" runat="server" Value='<%# Eval("UOM")%>' />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlNewUOM">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn> 
                                                <asp:TemplateColumn HeaderText="Sales Qty" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                       <%# Eval("ConfirmedOrderQty", "{0:f2}")%>  
                                                    </ItemTemplate>                                                    
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditQuantity" runat="server" Columns="5" Text='<%# Eval("ConfirmedOrderQty", "{0:f2}") %>' AutoPostBack="true" />
                                                        <asp:RequiredFieldValidator ID="rfvEditQuantity" runat="server" ControlToValidate="txtEditQuantity"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProdRow" /><asp:RangeValidator
                                                                ID="rgEditQuantity" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditQuantity"
                                                                Type="Double" ValidationGroup="valGrpEditProdRow" /></EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewSalesQty" runat="server" Columns="5" />
                                                        <asp:RequiredFieldValidator ID="rfvNewQuantity" runat="server" ControlToValidate="txtNewSalesQty"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Product" /><asp:RangeValidator
                                                                ID="rgNewQuantity" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewSalesQty"
                                                                Type="Double" ValidationGroup="Product" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Stk <br />Qty" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockQty" runat="server" Text='<%# Eval("ForStockingQty", "{0:f2}")%>'></asp:Label> 
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditStkQty" runat="server" Columns="5" Text='<%# Eval("ForStockingQty", "{0:f2}") %>' />
                                                        <asp:RequiredFieldValidator ID="rfvEditStkQuantity" runat="server" ControlToValidate="txtEditStkQty"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProdRow" /><asp:RangeValidator
                                                                ID="rgEditStkQty" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditStkQty"
                                                                Type="Double" ValidationGroup="valGrpEditProdRow" /></EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewStkQty" runat="server" Columns="5" />
                                                        <asp:RequiredFieldValidator ID="rfvNewStkQty" runat="server" ControlToValidate="txtNewStkQty"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Product" /><asp:RangeValidator
                                                                ID="rgNewStkQty" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewStkQty"
                                                                Type="Double" ValidationGroup="Product" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Ord <br />Qty" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                       <asp:Label ID="lblOrderQty" runat="server" Text='<%# Eval("OrderQty", "{0:f2}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditOrderQty" runat="server" Columns="5" Text='<%# Eval("OrderQty", "{0:f2}")%>'/>                                                        
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtOrderQty" runat="server" Columns="5" />
                                                       
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="SP" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <%# Eval("SellingCurrency")%> <%# Eval("UnitSellingPrice", "{0:f5}")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlEditSellingCurrency">
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hidSellingCurrency" runat="server" Value='<%# Eval("SellingCurrency")%>' />
                                                        <asp:TextBox CssClass="textbox" ID="txtEditUnitPrice" runat="server" Columns="5"
                                                            MaxLength="10" Text='<%# Eval("UnitSellingPrice", "{0:f5}") %>' />
                                                        <asp:RequiredFieldValidator ID="rfvEditUnitPrice" runat="server" ControlToValidate="txtEditUnitPrice"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProdRow" /><asp:RangeValidator
                                                                ID="rgEditUnitPrice" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditUnitPrice"
                                                                Type="Double" ValidationGroup="valGrpEditProdRow" />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlNewSellingCurrency">
                                                        </asp:DropDownList>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewUnitPrice" runat="server" Columns="5" MaxLength="10" />
                                                        <asp:RequiredFieldValidator ID="rfvNewUnitPrice" runat="server" ControlToValidate="txtNewUnitPrice"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Product" /><asp:RangeValidator
                                                                ID="rgNewUnitPrice" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewUnitPrice"
                                                                Type="Double" ValidationGroup="Product" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="Unit PP" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPurchaseCurrency" runat="server" Text='<%# Eval("PurchaseCurrency")%>'></asp:Label><asp:Label ID="lblUnitPP" runat="server" Text='<%# Eval("UnitPurchasePrice", "{0:f5}") %>'></asp:Label>
                                                        <br /> 
                                                        <asp:LinkButton ID="lnkPrice" runat="server" CommandName="ViewPrice" CommandArgument='<%# Eval("ProdCode")%>' EnableViewState="true"><asp:Image ID="imgPrice" runat="server" ImageUrl="../../images/icons/marked_price.png" /></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlEditPPCurrency">
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hidPurchaseCurrency" runat="server" Value='<%# Eval("PurchaseCurrency")%>' />
                                                        <asp:TextBox CssClass="textbox" ID="txtEditUnitPurchasePrice" runat="server" Columns="5"
                                                            MaxLength="10" Text='<%# Eval("UnitPurchasePrice", "{0:f5}") %>' />
                                                        <asp:RequiredFieldValidator ID="rfvEditUnitPurchasePrice" runat="server" ControlToValidate="txtEditUnitPurchasePrice"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProdRow" /><asp:RangeValidator
                                                                ID="rgEditUnitPurchasePrice" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditUnitPurchasePrice"
                                                                Type="Double" ValidationGroup="valGrpEditProdRow" />                                                         
                                                        <asp:LinkButton ID="lnkPrice" runat="server" CommandName="ViewPrice" CommandArgument='<%# Eval("ProdCode")%>' EnableViewState="true"><asp:Image ID="imgPrice" runat="server" ImageUrl="../../images/icons/marked_price.png" /></asp:LinkButton>
                                                        <br />  
                                                        <asp:Label ID="lblEditReason" runat="server" Visible="false">Reason:</asp:Label><br />
                                                        <asp:TextBox CssClass="textbox" ID="txtEditReason" runat="server" Columns="5" MaxLength="100" Visible='<%# !Eval("Reason").ToString().Equals("")%>' Text='<%# Eval("Reason")%>' />                                                             
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlNewPPCurrency">
                                                        </asp:DropDownList>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewUnitPurchasePrice" runat="server" Columns="5" MaxLength="10"
                                                            AutoPostBack="true" OnTextChanged="PurchasePrice_OnTextChanged" />
                                                        <asp:RequiredFieldValidator ID="rfvNewUnitPurchasePrice" runat="server" ControlToValidate="txtNewUnitPurchasePrice"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Product" /><asp:RangeValidator
                                                                ID="rgNewUnitPurchasePrice" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewUnitPurchasePrice"
                                                                Type="Double" ValidationGroup="Product" />
                                                        <asp:LinkButton ID="lnkPrice" runat="server" CommandName="ViewPrice" CommandArgument='<%# Eval("ProdCode")%>' EnableViewState="true"><asp:Image ID="imgPrice" runat="server" ImageUrl="../../images/icons/marked_price.png" /></asp:LinkButton>
                                                        <br />
                                                        <asp:Label ID="lblReason" runat="server" Visible="false">Reason:</asp:Label><br />
                                                        <asp:TextBox CssClass="textbox" ID="txtReason" runat="server" Columns="5" Visible="false" MaxLength="100" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                                                                
                                                <asp:TemplateColumn HeaderText="Total PP" HeaderStyle-Width="80px">
                                                    <ItemTemplate>                                                        
                                                        <asp:Label ID="lblTotalPP" runat="server" Text='<%# string.Concat(Eval("PurchaseCurrency")," ",Convert.ToDouble(Eval("UnitPurchasePrice", "{0:f5}")) * Convert.ToDouble(Eval("OrderQty", "{0:f5}"))) %>'></asp:Label>                                                        
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblEditTotalPP" runat="server" Text='<%# Convert.ToDouble(Eval("UnitPurchasePrice", "{0:f5}")) * Convert.ToDouble(Eval("OrderQty", "{0:f5}")) %>'></asp:Label>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblNewTotalPrice" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>   
                                                                                            
                                                <asp:TemplateColumn ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="true"
                                                            CausesValidation="false" CssClass="DeleteButt" OnClientClick="btnActionClick()"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="true"
                                                             ValidationGroup="valGrpEditProdRow" CssClass="SaveButt"><span>Save</span></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="true"
                                                            CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="true"
                                                            ValidationGroup="Product" CssClass="NewButt"><span>Add</span></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                        </asp:DataGrid> 
                            </td>
                            </tr>
                            <div runat="server" id="GrandTotalPurchase">
                                 <tr>
                                    
                                    <td align="right" style="font-weight: bold; width: 84%">
                                        Sub Total :
                                    </td>
                                    <td align="left" style="font-weight: bold; width: 6%">
                                        <asp:Label runat="server" ID="lblCurrency1" Text="" /></td>
                                    <td align="left" style="font-weight: bold;width: 10%">
                                        <asp:Label runat="server" ID="lblSubTotal" /></td>
                                </tr>
                                <tr>
                                     <td align="right" style="font-weight: bold; width: 84%">
                                        Discount : 
                                     </td>
                                     <td align="left" style="font-weight: bold; width: 6%">
                                        <asp:Label runat="server" ID="lblCurrency3" Text="" /></td>
                                     <td>
                                        <asp:TextBox runat="server" ID="txtDiscount" Columns="6" CssClass="textbox" Text="0" AutoPostBack="true" OnTextChanged="txtDiscount_OnTextChanged"></asp:TextBox>
                                        
                                     </td>
                                        
                                </tr> 
                                <tr>
                                <td align="right" style="font-weight: bold; width: 84%">
                                    Tax Type
                                    <asp:DropDownList runat="server" ID="ddlTaxType" CssClass="dropdownlist" OnSelectedIndexChanged="ddlTaxType_SelectedIndexChanged"
                                        AutoPostBack="true" /><asp:TextBox runat="server" ID="txtTaxRate" Columns="6" CssClass="textbox"
                                            contentEditable="false" />
                                    :
                                </td>
                                <td align="left" style="font-weight: bold; width: 6%">
                                    <asp:Label runat="server" ID="lblCurrency2" Text="" /></td>
                                <td align="left" style="font-weight: bold;width: 10%">
                                    <asp:Label runat="server" ID="lblTaxAmount" /></td>
                                </tr>
                                
                                <tr>
                                <td align="right" style="font-weight: bold; width: 84%">
                                        Grand Total :
                                    </td>
                                    <td align="left" style="font-weight: bold; width: 6%">
                                        <asp:Label runat="server" ID="lblCurrency4" Text="" /></td>
                                    <td align="left" style="font-weight: bold;width: 10%">
                                        <asp:Label runat="server" ID="lblGrandTotal" /></td>
                                </tr> 
                                              
                               
                            </div>
                            </table> 
                            
                            
            
            <table style="margin-left: 8px;" class="tInfoTable" width="100%" >
            <tr>
            <td style="color:#3C3C3C; font-weight: bold; font-size:10px" width="50%">            
                <div id="Div4" style="text-align: left;" runat="server">
                <ul>
                <li>Stk Qty - Stock Quantity</li>
                <li>Ord Qty - Order Quantity</li>  
                <li>Wt. Cost - Weighted Cost</li>  
                <li>SP - Selling Price</li>
                                 
                </ul>
                </div>
            </td>
            <td style="color:#3C3C3C; font-weight: bold; font-size:10px" width="50%">            
            <div id="Div5" style="text-align: left;" runat="server">
            <ul>    
            
            <li>PP - Purchase Price</li> 
            <li>PM - Product Manager</li>  
            <li>PH - Product Head</li> 
                     
            </ul>
            </div>
            </td> 
            </tr>
       </table>                       
                        </ContentTemplate>
                   </asp:UpdatePanel>
                    </ContentTemplate>
                   </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel4" HeaderText="Delivery">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="upDelivery" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="tTable1" cellspacing="5" cellpadding="5" width="99%">
                            <tr>
                                <td class="tbLabel">
                               Delivery Remarks</td>
                                <td>
                                    :</td>
                                <td >
                                <asp:TextBox runat="server" ID="txtPurchasingRemarks" TextMode="MultiLine" Width="90%"
                                        Height="50" onfocus="select();" EnableViewState="true" onchange="this.value = this.value.toUpperCase()" MaxLength="800"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revMessageBoardContents" ControlToValidate ="txtPurchasingRemarks" Text="Exceeding 800 characters" ValidationExpression="^[\s\S]{0,800}$" runat="server" /></td>
                            </tr> 
                            <tr>
                            <td colspan="3">
                            <div id="Div3" style="text-align: left;" runat="server">
                                <asp:Label ID="lblDeliverySummary" Visible="false" runat="server" />
                            </div>
                            </td>
                            </tr>
                            <tr>
                            <td colspan="3">
                            <asp:DataGrid ID="dgDeliveryData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true"
                                            OnItemCommand="dgDeliveryData_Command" OnDeleteCommand="dgDeliveryData_DeleteCommand" OnEditCommand="dgDeliveryData_EditCommand"
                                            OnCancelCommand="dgDeliveryData_CancelCommand" OnUpdateCommand="dgDeliveryData_UpdateCommand"
                                            OnItemDataBound="dgDeliveryData_ItemDataBound" GridLines="none" Width="100%" >
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6" HeaderStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgDeliveryData.CurrentPageIndex) * dgDeliveryData.PageSize)  %>'>
                                                        </asp:Label>
                                                        <input type="hidden" id="hidDeliveryID" runat="server" value='<%# Eval("DeliveryNo")%>' />
                                                    .</ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="PO No" HeaderStyle-Width="12%">
                                                    <ItemTemplate>                                                        
                                                        <asp:LinkButton ID="lnkPONo" runat="server" Text='<%# Eval("PONo") %>'></asp:LinkButton> 
                                                    </ItemTemplate>            
                                                                                          
                                                    <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtNewPO" Columns="10" onfocus="select();" CssClass="textbox" MaxLength="20"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNewPO" runat="server" ControlToValidate="txtNewPO"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Delivery" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Purchaser" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPurchaser" runat="server">
												    <%# Eval("Purchaser")%>
                                                    </asp:Label>                                                       
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="PO Date" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPODate" runat="server">
												    <%# Eval("PODate", "{0: dd-MMM-yyyy}")%>
                                                    </asp:Label>                                                       
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="CRD" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCRD" runat="server">
												    <%# Eval("CRD").ToString().Equals("1/01/1900 12:00:00 AM") ? "" : Eval("CRD", "{0: dd-MMM-yyyy}")%>
                                                    </asp:Label>                                                       
                                                    </ItemTemplate>
                                                    <EditItemTemplate>                                                                                                        
                                                        <asp:TextBox runat="server" ID="txtEditCRD" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox" Text=' <%# Eval("CRD").ToString().Equals("1/01/1900 12:00:00 AM") ? "" : Eval("CRD", "{0: dd-MMM-yyyy}")%>'> </asp:TextBox>
                                                    <asp:Image ID="imgCalendarEditCRD" runat="server" ImageUrl="../../images/imgCalendar.gif" onclick="showCalendar2(this, 'imgCalendarEditCRD', 'txtEditCRD', 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0" />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtNewCRD" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                                    <asp:Image ID="imgCalendarNewCRD" runat="server" ImageUrl="../../images/imgCalendar.gif" onclick="showCalendar2(this, 'imgCalendarNewCRD', 'txtNewCRD', 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="ETD" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblETD" runat="server">
												    <%# Eval("ETD").ToString().Equals("1/01/1900 12:00:00 AM") ? "" : Eval("ETD", "{0: dd-MMM-yyyy}")%>
                                                    </asp:Label>                                                       
                                                    </ItemTemplate>
                                                    <EditItemTemplate>                                                                                                        
                                                        <asp:TextBox runat="server" ID="txtEditETD" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox" Text=' <%# Eval("ETD").ToString().Equals("1/01/1900 12:00:00 AM") ? "" : Eval("ETD", "{0: dd-MMM-yyyy}")%>'> </asp:TextBox>
                                                    <asp:Image ID="imgCalendarEditETD" runat="server" ImageUrl="../../images/imgCalendar.gif" onclick="showCalendar2(this, 'imgCalendarEditETD', 'txtEditETD', 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0" />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtNewETD" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                                    <asp:Image ID="imgCalendarNewETD" runat="server" ImageUrl="../../images/imgCalendar.gif" onclick="showCalendar2(this, 'imgCalendarNewETD', 'txtNewETD', 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="ETA" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblETA" runat="server">
												    <%# Eval("ETA").ToString().Equals("1/01/1900 12:00:00 AM") ? "" : Eval("ETA", "{0: dd-MMM-yyyy}")%>
                                                    </asp:Label>                                                       
                                                    </ItemTemplate>
                                                    <EditItemTemplate>                                                                                                        
                                                        <asp:TextBox runat="server" ID="txtEditETA" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox" Text=' <%# Eval("ETA").ToString().Equals("1/01/1900 12:00:00 AM") ? "" : Eval("ETA", "{0: dd-MMM-yyyy}")%>'> </asp:TextBox>
                                                    <asp:Image ID="imgCalendarEditETA" runat="server" ImageUrl="../../images/imgCalendar.gif" onclick="showCalendar2(this, 'imgCalendarEditETA', 'txtEditETA', 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0" />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtNewETA" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                                    <asp:Image ID="imgCalendarNewETA" runat="server" ImageUrl="../../images/imgCalendar.gif" onclick="showCalendar2(this, 'imgCalendarNewETA', 'txtNewETA', 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>                                                
                                                <asp:TemplateColumn HeaderText="GRN Info." HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGRNNo" runat="server"></asp:Label>
                                                        <asp:HiddenField ID="hidGRNNo" runat="server" Value='<%# Eval("GRNNo").ToString() %>' />
                                                        <asp:HiddenField ID="hidGRNTrnNo" runat="server" Value='<%# Eval("GRNTrnNo").ToString() %>' />                                                                                                                                                                                                   
                                                    </ItemTemplate>  
                                                    <EditItemTemplate>                                                                                                        
                                                        <%# Eval("GRNNo")%>
                                                    </EditItemTemplate>                                                                                                      
                                                    <FooterTemplate>
                                                                                                            
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>                          
                                                <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%"
                                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                    <ItemTemplate>
                                                         <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                                CausesValidation="false" CssClass="EditButt"><span>&nbsp;&nbsp;Edit</span></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="true"
                                                            CausesValidation="false" CssClass="DeleteButt" OnClientClick="btnActionClick()"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="true"
                                                            ValidationGroup="valGrpEditRow" CssClass="SaveButt" OnClientClick="btnActionClick()"><span>Save</span></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="true"
                                                            CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="true"
                                                            ValidationGroup="Delivery" CssClass="NewButt"><span>Add</span></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                        </asp:DataGrid>
                            </td>
                            </tr>
                            </table>
                            
                            <table style="margin-left: 8px;" class="tInfoTable" width="90%" >
                            <tr>
                                <td style="color:#3C3C3C; font-weight: bold; font-size:10px" width="29%">            
                                    <div id="Div7" style="text-align: left;" runat="server">
                                    <ul>
                                    <li>CRD - Cargo Ready Date</li>
                                    <li>ETD - Estimated Time of Delivery</li>
                                    <li>ETA - Estimated Time of Arrival at Leeden Warehouse</li>           
                                    </ul>
                                    </div>
                                </td>
                            </tr>
                            </table>
            
                            
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>    
                </ajaxToolkit:TabPanel>  
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel5" HeaderText="Attachment">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="upAttachment" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                          
                          <table class="tTable1" cellspacing="2" cellpadding="2" width="99%">
                            <tr>
                                <td>
                                <div id="Div8" style="text-align: left;" runat="server">
                                    <asp:Label ID="lblAttachmentSummary" Visible="false" runat="server" />
                                </div>
                                </td>
                             </tr>  
                                                        
                             <tr><td>
                                <asp:DataGrid ID="dgAttachmentData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true"
                                            OnItemCommand="dgAttachmentData_Command" OnDeleteCommand="dgAttachmentData_DeleteCommand" OnEditCommand="dgAttachmentData_EditCommand"
                                            OnCancelCommand="dgAttachmentData_CancelCommand" OnUpdateCommand="dgAttachmentData_UpdateCommand" OnItemDataBound="dgAttachmentData_ItemDataBound" GridLines="none" Width="100%" >
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgAttachmentData.CurrentPageIndex) * dgAttachmentData.PageSize)  %>'>
                                                        </asp:Label>
                                                        <input type="hidden" id="hidFileID" runat="server" value='<%# Eval("FileID")%>' />                                                       
                                                        .</ItemTemplate>
                                                </asp:TemplateColumn>                                                
                                                <asp:TemplateColumn HeaderText="File Name" ItemStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <%# Eval("FileDisplayName")%>                                                       
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditFileName" runat="server" Columns="50"
                                                            MaxLength="50" Text='<%# Eval("FileDisplayName") %>'  />
                                                        <asp:RequiredFieldValidator ID="rfvEditFileName" runat="server" ControlToValidate="txtEditFileName"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewFileName" runat="server" Columns="50" MaxLength="50" CssClass="textbox" />
                                                        <asp:RequiredFieldValidator ID="rfvNewFileName" runat="server" ControlToValidate="txtNewFileName"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGroupAttachment" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>                                                                                                 
                                                <asp:TemplateColumn HeaderText="Attachment">
                                                    <ItemTemplate>                                                       
                                                       <asp:LinkButton ID="linkName" runat="server" Text='<%# Eval("FileName")%>' CommandArgument='<%#Eval("FileNameEncrypted")%>' CommandName="Load" EnableViewState="true"
                                                            CausesValidation="false"></asp:LinkButton>                                                       
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                       <asp:LinkButton ID="linkNameEdit" runat="server" Text='<%# Eval("FileName")%>' CommandArgument='<%#Eval("FileNameEncrypted")%>' CommandName="Load" EnableViewState="true"
                                                            CausesValidation="false"></asp:LinkButton> 
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                    <asp:FileUpload CssClass="textbox" ID="FileUpload1" runat="server" Width="200px" onchange="btnUploadAttachmentClick()"  />                                                    
                                                    <asp:Label ID="lblUpload" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>                         
                                                <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="true"
                                                            CausesValidation="false" CssClass="DeleteButt" OnClientClick="btnActionClick()"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="true"
                                                            ValidationGroup="valGrpEditRow" CssClass="SaveButt" OnClientClick="btnActionClick()"><span>Save</span></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="true"
                                                            CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>                                                                                                           
                                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="true"
                                                            ValidationGroup="valGroupAttachment" CssClass="NewButt" ><span>Add</span></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkBlank" runat="server" EnableViewState="true" ValidationGroup="Blank" CommandName="Blank"></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                        </asp:DataGrid>                             
                                </td>
                             </tr>
                          </table>    
                        </ContentTemplate>
                        </asp:UpdatePanel>
                     </ContentTemplate>
                </ajaxToolkit:TabPanel>                                                              
                                                                                    
            </ajaxToolkit:TabContainer>
            
            
            </div>
               
               <table style="text-align: left; width: 100%; margin-left:8px" cellspacing="5" cellpadding="5">    
               <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblCreatedBy"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblModifiedBy"></asp:Label></td>
                            </tr>    
               <div id="routingTable" runat="server" visible="false">            
               <tr style="background-color: #D3D1CF; font-weight:bold; margin-top: 10px;">
               <td style="padding:4px;">
               Routing Information
               </td>
               </tr>
               </div>
               <tr>
               <td>
               <asp:DataGrid ID="dgMRFormApproval" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"   
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true" OnItemDataBound="dgMRFormApproval_ItemDataBound"
                                            GridLines="none" Width="100%" OnPageIndexChanged="dgMRFormApproval_PageIndexChanged">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6" ItemStyle-Width="5px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgMRFormApproval.CurrentPageIndex) * dgMRFormApproval.PageSize)  %>'>
                                                        </asp:Label>
                                                        <asp:HiddenField ID="hidCurrentLevel" runat="server" Value='<%# Eval("IsCurrentLevel") %>' />
                                                        .</ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Status" HeaderStyle-Wrap="true" ItemStyle-Width="180px">
                                                    <ItemTemplate>
                                                        <%# Eval("Statusname") %>  <%# ((Eval("RoutedDate").ToString() == "") && ((Eval("Status").ToString() == "A") || (Eval("Status").ToString() == "R"))) ? " (High Level)" : ""%>
                                                    </ItemTemplate> 
                                                </asp:TemplateColumn> 
                                                <asp:TemplateColumn HeaderText="Approver / Person In Charge" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("Approver") %> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Date Routed" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("RoutedDate").ToString().Equals("") ? "-" : Eval("RoutedDate", "{0: dd-MMM-yyyy H:mm:ss}")%>                                                                                                          
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Date Action" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("ActionDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("ActionDate", "{0: dd-MMM-yyyy H:mm:ss}")%>                                                                                                      
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                
                                                <asp:TemplateColumn HeaderText="Reason">
                                                    <ItemTemplate>
                                                       <%# Eval("Reason") %>                                    
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                            <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                        </asp:DataGrid>  
               </td>
               </tr>                
               </table>   
                <p></p>
                    
                      <table style="text-align: right; width: 100%" cellspacing="5" cellpadding="5">
                        <tr>
                            <td>                                                               
                                <asp:Label ID="lblAssignedPurchaser" runat="server" Visible="false">Assigned Purchaser : </asp:Label><asp:DropDownList CssClass="dropdownlist" ID="ddlPurchaser" runat="Server" DataTextField="PurchaserName" DataValueField="PurchaserName" Visible="false" />
                                
                                <asp:Button runat="server" ID="btnSubmitForApproval" Text="Submit for Approval" CssClass="button" OnClick="btnSubmitForApproval_Click" Visible="false" />
                                <asp:Button runat="server" ID="btnApprove" Text="Approve" CssClass="button" OnClick="btnApprove_Click" Visible="false" />
                                <asp:Button runat="server" ID="btnReject" Text="Reject" CssClass="button" Visible="false" />
                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="button" Visible="false" />
                                <asp:Button runat="server" ID="btnUndoCancel" Text="Undo Cancellation" CssClass="button" OnClick="btnUndoCancel_Click" Visible="false" />
                                <asp:Button runat="server" ID="btnHighLevelApprove" Text="Approve (High Level)" CssClass="button" Visible="false" />
                                <asp:Button runat="server" ID="btnHighLevelReject" Text="Reject (High Level)" CssClass="button" Visible="false" />
                                <asp:Button runat="server" ID="btnHighLevelCancel" Text="Cancel (High Level)" CssClass="button" Visible="false" />
                                <asp:Button runat="server" ID="btnDuplicate" Text="Duplicate" OnClick="btnDuplicate_Click" CssClass="button" Visible=False />
                                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="button" OnClick="btnSave_Click" OnClientClick="btnSaveClick(this)" />
                            </td>
                        </tr>
                      </table>   
                     
              <table style="text-align: right; width: 100%" cellspacing="5" cellpadding="5">
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlReport" runat="server" CssClass="dropdownlist">
                        </asp:DropDownList>  
                        <asp:LinkButton ID="lnkPrintReport" OnClick="GenerateReport" runat="server" Text="Print"
                    CssClass="button" ToolTip="Please click to print report." CausesValidation="False"><img id="img4" alt="" src="../../images/icons/printIcon.gif" align="top" border="0" /></asp:LinkButton>              
                        <asp:LinkButton ID="lnkPrintPDF" OnClick="GeneratePDFReport" runat="server" Text="Print PDF"
                            CssClass="button" ToolTip="Please click to print PDF report." CausesValidation="False"><img id="img1" alt="" src="../../images/icons/pdf.png" align="top" border="0" /></asp:LinkButton> 
                    </td>
                </tr>
            </table> 
              
            
            <table align="center" style="margin-left: auto; margin-right: auto;">
                        <tr align="center"><td><div id="importing" style="visibility:hidden"><span style="font-style:italic;">Importing ...</span></div></td></tr>
                        <tr><td>
                        <div id="showbar" style="font-size:8pt;padding:2px;border:solid #D8D8D8 1px;visibility:hidden">
                        <span id="progress1">&nbsp; &nbsp;</span>
                        <span id="progress2">&nbsp; &nbsp;</span>
                        <span id="progress3">&nbsp; &nbsp;</span>
                        <span id="progress4">&nbsp; &nbsp;</span>
                        <span id="progress5">&nbsp; &nbsp;</span>
                        <span id="progress6">&nbsp; &nbsp;</span>
                        <span id="progress7">&nbsp; &nbsp;</span>
                        <span id="progress8">&nbsp; &nbsp;</span>
                        <span id="progress9">&nbsp; &nbsp;</span>        
                        </div>
                        </td></tr>
                        </table> 
                
                
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnReject" PopupControlID="PNL" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground"/>
                                                    <asp:Panel ID="PNL" runat="server" style="display:none; width:400px; background-color:White; border-width:2px; border-color:Black; border-style:solid; padding:20px;">
                                                   
                                                    <table>
                                                    <tr>
                                                    <td colspan="2" class="tbLabel">Please key in reject reason</td><td>:</td>
                                                    <td>
                                                    </tr>
                                                    <tr>
                                                    <td colspan="2">
                                                    <asp:TextBox runat="server" ID="txtRejectReason" onfocus="select();" Columns="60" TextMode="MultiLine" CssClass="textbox"></asp:TextBox>
                                                    </td>                                            
                                                    </tr></table>
                                                                                                                                      
                                                    <br />
                                                        <div style="text-align:center;">
                                                            <asp:Button CssClass="button" ID="ButtonOk" runat="server" Text="Submit" OnCommand="RejectCommand" />
                                                            <asp:Button CssClass="button" ID="ButtonCancel" runat="server" Text="Cancel" />
                                                        </div>
                                                        
                                                                          
                                                    </asp:Panel>
                                                    
                                                    
                                                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnHighLevelApprove" PopupControlID="PNL2" CancelControlID="btnCancel2" BackgroundCssClass="modalBackground"/>
                                                    <asp:Panel ID="PNL2" runat="server" style="display:none; width:400px; background-color:White; border-width:2px; border-color:Black; border-style:solid; padding:20px;">
                                                        <table>
                                                        <tr>
                                                        <td colspan="2" class="tbLabel">Please key in the reason for approving this MR</td><td>:</td>
                                                        <td>
                                                        </tr>
                                                        <tr>
                                                        <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtApproveReason" onfocus="select();" Columns="60" TextMode="MultiLine" CssClass="textbox"></asp:TextBox>
                                                        </td>                                            
                                                        </tr>
                                                        </table>
                                                                                                                                      
                                                    <br />
                                                        <div style="text-align:center;">
                                                            <asp:Button CssClass="button" ID="btnOK2" runat="server" Text="Submit" OnCommand="ApproveCommand" />
                                                            <asp:Button CssClass="button" ID="btnCancel2" runat="server" Text="Cancel" />
                                                        </div>
                                                    
                                                    
                                                    </asp:Panel>
                                                    
                                                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="btnCancel" PopupControlID="PNL3" CancelControlID="btnCancel3" BackgroundCssClass="modalBackground"/>
                                                    <asp:Panel ID="PNL3" runat="server" style="display:none; width:400px; background-color:White; border-width:2px; border-color:Black; border-style:solid; padding:20px;">
                                                        <table>
                                                        <tr>
                                                        <td colspan="2" class="tbLabel">Please key in cancellation reason</td><td>:</td>
                                                        <td>
                                                        </tr>
                                                        <tr>
                                                        <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtCancellationReason" onfocus="select();" Columns="60" TextMode="MultiLine" CssClass="textbox"></asp:TextBox>
                                                        </td>                                            
                                                        </tr>
                                                        </table>
                                                                                                                                      
                                                    <br />
                                                        <div style="text-align:center;">
                                                            <asp:Button CssClass="button" ID="btnOK3" runat="server" Text="Submit" OnCommand="CancelCommand" />
                                                            <asp:Button CssClass="button" ID="btnCancel3" runat="server" Text="Cancel" />
                                                        </div>
                                                    
                                                    
                                                    </asp:Panel>
                                                   
            </ContentTemplate> 
            </asp:UpdatePanel>
            
            <asp:Panel ID="Panel1" runat="server">
            </asp:Panel>
            <div class="TABCOMMAND">
            <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <ul>
                        <li>&nbsp;</li>
                    </ul>
                    <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
            
            
</asp:Content>    