<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="CRM1.aspx.cs" Inherits="GMSWeb.Debtors.Debtors.CRM1" Title="Customer Info" ValidateRequest="false" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">




<script type="text/javascript">



function changeButtonClass()
    {
        // code examples from above
        //document.getElementById('btnTrigger').className = 'crm_button_selected';
        
        alert('abc');
    }



function clientActiveTabChanged(sender, args) 
{
}

function progress_update(a) {

document.getElementById('loading_'+a).style.visibility = 'visible';

}

function progress_stop() {

document.getElementById('loading'+a).style.visibility = 'hidden';

}



function confirm_delete()
 {
   if (confirm("Are you sure you want to delete this item?")==true)
     return true;
   else
     return false;
 }
 
 function confirm_upgrade()
 {
   if (confirm("Are you sure you want to upgrade this prospect? ")==true)
     return true;
   else
     return false;
 }

</script>
<h1>Customer Info &gt; <asp:Label ID="lblAccountCode" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblAccountName" runat="server"></asp:Label></h1>
           
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="AutoCompleteAccountCode.asmx" />
                <asp:ServiceReference Path="AutoCompleteProductGroupName.asmx" />
                <asp:ServiceReference Path="AutoCompleteProductName.asmx" />
            </Services>
            </asp:ScriptManager>
             
            
            
              
            
            <input type="hidden" id="hidAccountCode" runat="server" />
            
            
            <p></p>
            <div style="margin-left: 10px; margin-right: 30px">
            
            
            
            
            <asp:UpdatePanel ID="upOutter" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            
            <table>
            <tr>
                <td><input class="crm_button_selected" ID="btnLoadParticular" value="Particulars" runat="server" type="button" onserverclick="btnLoadParticular_Click" ></td>
                <td><input class="crm_button" ID="btnTrigger" value="Contacts" runat="server" type="button" onserverclick="btnTrigger_Click" ></td> 
                <td><input class="crm_button" ID="btnLoadCommRecord" value="Communication" runat="server" type="button" onserverclick="btnLoadCommRecord_Click" ></td>
                <td><input class="crm_button" ID="btnLoadAttachment" value="Attachments" runat="server" type="button" onserverclick="btnLoadAttachment_Click" ></td>
                <td><input class="crm_button" ID="btnLoadFinance" value="Financials Summary" runat="server" type="button" onserverclick="btnLoadFinance_Click" ></td> 
            </tr>
            <tr>
                <td><input class="crm_button" ID="btnLoadSales" value="Sales" runat="server" type="button" onserverclick="btnLoadSales_Click" ></td>
                <td><input class="crm_button" ID="btnLoadCollection" value="Collections" runat="server" type="button" onserverclick="btnLoadCollection_Click" ></td>   
                <td><input class="crm_button" ID="btnLoadOutstandingPayment" value="Outstanding" runat="server" type="button" onserverclick="btnLoadOutstandingPayment_Click" ></td>
                <td><input class="crm_button" ID="btnLoadPurchase" value="Purchases" runat="server" type="button" onserverclick="btnLoadPurchase_Click" ></td>
                <td><input class="crm_button" ID="btnLoadOthers" value="Others" runat="server" type="button" onserverclick="btnLoadOthers_Click" ></td>
            </tr>
            </table>
            
            
            
            <div style="padding: 8px; border-color:Gray; border-style:solid; border-color:grey; border-width:1px; margin-left:2px; width: 740px; ">
                
                    <asp:UpdateProgress ID="uppOutter" runat="server" AssociatedUpdatePanelID="upOutter">
                        <ProgressTemplate>
                           <div align="center">
                            <table width="10%">                             
                             <tr align="center">
                                <td><img src = "../../images/dg_loading.gif" /></td><td> Loading...</td>
                             </tr>
                             </table>
                             </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    
                    
                <asp:UpdatePanel ID="upParticular" runat="server" UpdateMode="Conditional" Visible="false">
                        <ContentTemplate>                 
                           <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0" width="650px">
                <tr>
                    <td colspan= "4">
                    <span style="color:Red; size:7px; font-style:italic;"><asp:Label ID="lblParticularsMsg" runat="server" ></asp:Label></span>           
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Customer Code</td>
                    <td style="width:5%">
                        :</td>
                    <td colspan="2">                        
                        <asp:TextBox runat="server" ID="txtAccountCode" MaxLength="10" Columns="12" onfocus="select();"
                        CssClass="textbox"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
            TargetControlID="txtAccountCode" ServicePath="AutoCompleteAccountCode.asmx"
            ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
            EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                        </ajaxToolkit:AutoCompleteExtender>                        
                        
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Customer Name</td>
                    <td style="width:5%">
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtAccountName" MaxLength="50" Columns="30" onfocus="select();"
                        CssClass="textbox"></asp:TextBox>
                        
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Sales Person</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        
                        <asp:DropDownList ID="ddlSalesman" runat="Server" CssClass="dropdownlist" />   
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Credit Term</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtCreditTerm" runat="server" Columns="20" CssClass="textbox" />
            <asp:CompareValidator ID="cvCreditTerm" runat="server" ErrorMessage="*"
                Display="Dynamic" ControlToValidate="txtCreditTerm" Type="Integer" Operator="DataTypeCheck"
                ValidationGroup="valGrpNewRow" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Credit Limit</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtCreditLimit" runat="server" Columns="20" CssClass="textbox" />
            <asp:CompareValidator ID="cvCreditLimit" runat="server" ErrorMessage="*"
                Display="Dynamic" ControlToValidate="txtCreditLimit" Type="Double" Operator="DataTypeCheck"
                ValidationGroup="valGrpNewRow" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Outstanding</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblOutstanding"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Excess</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblExcess"></asp:Label>
                    </td>
                </tr>
                    
                
                <tr>
                    <td class="tbLabel">
                        Default Currency</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        
                         <asp:DropDownList runat="server" ID="ddlCurrency" CssClass="dropdownlist"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Industry</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        
                        <asp:DropDownList CssClass="dropdownlist" ID="ddlIndustry" runat="Server"
                                                    DataTextField="Name" DataValueField="Name" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Territory</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:DropDownList CssClass="dropdownlist" ID="ddlTerritory" runat="Server" DataTextField="TerritoryName" DataValueField="TerritoryName" />          
                    </td>
                </tr>
                
                <tr>
                    <td class="tbLabel">
                        Address1</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtAddress1" MaxLength="40" Columns="40" onfocus="select();"
                                            CssClass="textbox" TabIndex="1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Address2</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtAddress2" MaxLength="40" Columns="40" onfocus="select();"
                                            CssClass="textbox" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Address3</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtAddress3" MaxLength="40" Columns="40" onfocus="select();"
                                            CssClass="textbox" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Address4</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtAddress4" MaxLength="40" Columns="40" onfocus="select();"
                                            CssClass="textbox" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Postal Code</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtPostalCode" runat="server" Columns="20" MaxLength="6"
                CssClass="textbox" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Grade</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        
                        <asp:DropDownList CssClass="dropdownlist" ID="ddlGrade" runat="Server"
                                                    DataTextField="GradeName" DataValueField="GradeCode" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Website</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtWebsite" MaxLength="50" Columns="40" onfocus="select();"
                        CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>                
                <tr>
                    <td class="tbLabel">
                        Facebook</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtFacebook" MaxLength="50" Columns="40" onfocus="select();"
                        CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="tbLabel">
                        Remarks</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3"
                                Columns="40" MaxLength="600" CssClass="textarea" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Is Active</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:CheckBox ID="chkAcctActive" runat="server" Checked />
                    </td>
                </tr>
                <tr><td></td><td></td>
                 <td>
                        
                        
                        <asp:Button ID="btnUpdateParticular" Text="Update" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnUpdateParticular_Click"></asp:Button>
                        &nbsp;
                        <asp:Button ID="btnUpgradeToCustomer" Text="Upgrade To Customer" EnableViewState="False" runat="server" CssClass="button" Visible="false"
                            OnClick="btnUpgradeToCustomer_Click" OnClientClick="return confirm_upgrade();"></asp:Button>
                            
                    </td>
                </tr>
            </table>
                            
                        </ContentTemplate>
                        <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnUpdateParticular" />
                                <asp:AsyncPostBackTrigger ControlID="btnUpgradeToCustomer" />                                                                 
                        </Triggers>
                        
                    </asp:UpdatePanel>
                
           
                     
                        
                    <asp:UpdatePanel ID="upContact" runat="server" UpdateMode="Conditional" Visible="false">
                        <ContentTemplate> 
                        
                                               
                       
                       
                        <table class="tTable1" style="background-color:White;"> 
                        
                        <tr>
                            <td>
                            <span style="color:Red; size:7px; font-style:italic;"><asp:Label ID="lblContactsMsg" runat="server" ></asp:Label></span>           
                            </td>
                        </tr>                       
                        <tr><td>
                        <asp:Button ID="btnAddNewContact" Text="Add Contact" EnableViewState="False" runat="server" CssClass="button" OnClick="btnAddNewContact_Click" />
                        
                        </td></tr>
                        </table>
                        
                        <br /> 
                        <table class="tTable1" style="background-color:White;">                        
                        <tr>
                        <td>
                        <div id="Div6" style="text-align: left;" runat="server">
                            <asp:Label ID="lblContactsSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable1" width="100%">
                        <asp:DataGrid ID="dgContacts" runat="server" AutoGenerateColumns="false" ShowFooter="false"
								    DataKeyField="ContactID" GridLines="none" OnItemDataBound="dgContacts_ItemDataBound" OnDeleteCommand="dgContacts_DeleteCommand" OnItemCommand="dgContacts_Command" CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgContacts_PageIndexChanged" EnableViewState="true" OnSortCommand="SortContact" AllowSorting="true">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# (Container.ItemIndex + 1) + ((dgContacts.CurrentPageIndex) * dgContacts.PageSize)%>
											    .
											    <input type="hidden" id="hidContactID" runat="server" value='<%# Eval("ContactID")%>' />
											    </ItemTemplate>
									    </asp:TemplateColumn>								    

									    <asp:TemplateColumn HeaderText="Name" SortExpression="FirstName" HeaderStyle-Wrap="false" ItemStyle-Width="50px">										    
										    <ItemTemplate>
										        <asp:Label ID="lblFirstName" runat="server">										        
                                                
                                                <asp:LinkButton ID="lnkEditContact" runat="server" CommandName="EditContact"><%# Eval("FirstName")%>&nbsp;<%# Eval("LastName")%></asp:LinkButton>	
                                                
											    </asp:Label>
										    </ItemTemplate>										    
										    										    
									    </asp:TemplateColumn>
									    
                                        
                                        <asp:TemplateColumn HeaderText="Sal." SortExpression="Salutation" HeaderStyle-Wrap="false" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                
                                               <%# Eval("Salutation")%>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateColumn> 
                                        
                                        <asp:TemplateColumn HeaderText="Designation" SortExpression="Designation" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <%# Eval("Designation")%>                                               
                                            </ItemTemplate>
                                            
                                        </asp:TemplateColumn> 
                                        
                                        <asp:TemplateColumn HeaderText="Office No." SortExpression="OfficePhone" HeaderStyle-Wrap="false" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <%# Eval("OfficePhone")%>                                               
                                            </ItemTemplate>
                                            
                                        </asp:TemplateColumn> 
                                        
                                        <asp:TemplateColumn HeaderText="Mobile No." SortExpression="MobilePhone" HeaderStyle-Wrap="false" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <%# Eval("MobilePhone")%>                                               
                                            </ItemTemplate>
                                            
                                        </asp:TemplateColumn> 
                                        
                                         
                                        
                                        <asp:TemplateColumn HeaderText="Email" SortExpression="Email" HeaderStyle-Wrap="false" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <%# Eval("Email")%>                                               
                                            </ItemTemplate>
                                            
                                        </asp:TemplateColumn> 
                                        <asp:TemplateColumn HeaderText="Is Active" SortExpression="IsActive" ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%# (Eval("IsActive").Equals(System.DBNull.Value)) ? "No" : ((bool)Eval("IsActive")) ? "Yes" : "No"%>                                                    
                                                </ItemTemplate>
                                        </asp:TemplateColumn>                                                  	    
									    
									    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" 
									        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
									        <ItemTemplate>									             								        
									             <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false" CssClass="DeleteButt" ><span>Delete</span></asp:LinkButton>
									        </ItemTemplate>
										    
									    </asp:TemplateColumn>
									    
									    
								    </Columns>
								    <HeaderStyle CssClass="tHeader" />
								    <AlternatingItemStyle CssClass="tAltRow" />
								    <FooterStyle CssClass="tFooter" />
								    <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
							    </asp:DataGrid>
                        </div>
                        </td>
                        </tr>
                        </table>
                        <asp:Button runat="server" ID="HiddenForModalAddNewContact" style="display: none" /> 
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenForModalAddNewContact"
        PopupControlID="pnlDiscount" BackgroundCssClass="modalBackground" CancelControlID="ButtonCancel" />
   
   
                        
                        
    
    <asp:Panel ID="pnlDiscount" runat="server" Style="display: none; width: 550px; background-color: White;
        border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
        
       
    <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0"
                width="99%">
                
    <tr>
                <td style="width: 6%" colspan="3">
                    <span style="font-weight:bold; font-size: 14px;"><asp:Label ID="lblContact" runat="server"></asp:Label></span>
                </td>
                <td>
                    <div style="text-align: right;"> 
                    <asp:Button CssClass="button" ID="ButtonCancel" runat="server" Text="X" />
                    </div>
                </td>
    </tr>
    <tr>
    <td class="tbLabel" style="width:20%">
        First Name</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtFirstName" runat="server" Columns="50" MaxLength="50" CssClass="textbox"
            onfocus="select();" /><asp:RequiredFieldValidator ID="rfvCourseTitle" runat="server"
                ControlToValidate="txtFirstName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
       
        <input type="hidden" id="hidContactID1" runat="server" />    
            
    </td>
</tr>

<tr>
    <td class="tbLabel" style="width:20%">
        Last Name</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtLastName" runat="server" Columns="50" MaxLength="50" CssClass="textbox"
            onfocus="select();" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                ControlToValidate="txtLastName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />      
       
    </td>
</tr>

<tr>
    <td class="tbLabel" style="width:20%">
        Salutation</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:DropDownList ID="ddlSalutation" runat="server" DataTextField="Name"
            DataValueField="Name" CssClass="dropdownlist">
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Designation</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtDesignation" runat="server" Columns="30" MaxLength="50" CssClass="textbox"
            onfocus="select();" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                ControlToValidate="txtDesignation" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
    </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Office Phone</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtOfficePhone" runat="server" Columns="30" MaxLength="50" CssClass="textbox"
            onfocus="select();" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                ControlToValidate="txtOfficePhone" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
    </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
       Mobile Phone</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtMobilePhone" runat="server" Columns="30" MaxLength="50" CssClass="textbox" />
    </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Fax</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtFax" runat="server" Columns="30" MaxLength="50"
            CssClass="textbox" />
            
            
         </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Email</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtEmail" runat="server" Columns="30" MaxLength="50"
            CssClass="textbox" />
       <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                ControlToValidate="txtEmail" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
       <asp:RegularExpressionValidator ID="regexpName" runat="server" ErrorMessage="Invalid Format" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*" ValidationGroup="valGrpNewRow"/>              
       </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Remarks</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtContactRemarks" runat="server" TextMode="MultiLine" Rows="3"
                Columns="40" MaxLength="400" CssClass="textarea" /></td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Is Active</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:CheckBox ID="chkIsActive" runat="server" Checked />
    </td>
</tr>

          </table>
                <br />
        <div style="text-align: center;">
            <asp:Button CssClass="button" ID="ButtonOk" runat="server" Text="Submit" OnCommand="AddUpdateContact" 
                ValidationGroup="valGrpNewContact" />
            
        </div>
    </asp:Panel>
    </ContentTemplate>            
    </asp:UpdatePanel>
            
                    <asp:Panel ID="Panel1" runat="server" UpdateMode="Conditional">
                    </asp:Panel>
                    
                    <asp:UpdatePanel ID="upCommunication" runat="server" UpdateMode="Conditional" Visible="false">
                        <ContentTemplate>
                        
                        
                        <table class="tTable1" style="background-color:White;"> 
                        <tr>
                            <td>
                            <span style="color:Red; size:7px; font-style:italic;"><asp:Label ID="lblCommunicationMsg" runat="server" ></asp:Label></span>           
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <asp:Button ID="btnAddCommunication" Text="Add Communication" EnableViewState="False" runat="server" CssClass="button" OnClick="AddCommunication_Click" />
                            </td>
                        </tr>
                        </table>
                       
                        <asp:Button runat="server" ID="HiddenForModalAddNewCommunication" style="display: none" /> 
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="HiddenForModalAddNewCommunication"
        PopupControlID="pnlCommunication" BackgroundCssClass="modalBackground" CancelControlID="btnCommunicationCancel" />
                        <asp:Panel ID="pnlCommunication" runat="server" Style="display: none; width: 560px; height: 500px; overflow:auto; background-color: White;
                            border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                        <input type="hidden" id="hidCommID1" runat="server" />   
                        
                        
                        
                        
                        
                        <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0" width="99%">
                        
                        <tr>
                                    <td style="width: 100%" colspan="3">                                    
                                    <span style="font-weight:bold; font-size: 14px;"><asp:Label ID="lblCommunication" runat="server"></asp:Label></span><br />
                                    <span style="color:Red; size:7px; font-style:italic;"><asp:Label ID="lblMessage" runat="server" ></asp:Label></span>
                                    </td>
                                    <td>
                                    <div style="text-align: right;">                          
                                            <asp:Button CssClass="button" ID="btnCommunicationCancel" runat="server" Text="X" />
                                                                                       
                                    </div>
                                    
                                    </td>
                        </tr>
                        
                        
                        <tr>
                            <td class="tbLabel" style="width:20%">
                                Description</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtDescription" runat="server" Columns="40" MaxLength="1000" CssClass="textbox" />
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewCommunication" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="tbLabel" style="width:20%">
                                Date From</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="txtDateFrom" MaxLength="10" Columns="10" CssClass="textbox"></asp:TextBox>
                                <img "id="imgCalendarNewFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                    height="20" width="17" alt="" align="absMiddle" border="0" />
                                <asp:RequiredFieldValidator ID="rfvDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewCommunication" />
                                <asp:CompareValidator ID="cvDateFrom" runat="server" ErrorMessage="Invalid Date"
                                    ControlToValidate="txtDateFrom" Type="Date" Display="Dynamic" ValidationGroup="valGrpNewCommunication"
                                    Operator="DataTypeCheck"></asp:CompareValidator>
                                <asp:TextBox runat="server" ID="txtDateFromTime" MaxLength="5" Columns="10" CssClass="textbox"
                                    ToolTip="Time format should be 00:00" onchange="dataSplit(this)"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvateFromTime" runat="server" ControlToValidate="txtDateFromTime"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewCommunication" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tbLabel" style="width:20%">
                                Date To</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="txtDateTo" MaxLength="10" Columns="10" CssClass="textbox"></asp:TextBox>
                                <img id="imgCalendarNewTo" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                    height="20" width="17" alt="" align="absMiddle" border="0" />
                                <asp:RequiredFieldValidator ID="rfvDateTo" runat="server" ControlToValidate="txtDateTo"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewCommunication" />
                                <asp:CompareValidator ID="cvDateTo" runat="server" ErrorMessage="Invalid Date" ControlToValidate="txtDateTo"
                                    Type="Date" Display="Dynamic" ValidationGroup="valGrpNewCommunication" Operator="DataTypeCheck"></asp:CompareValidator>
                                <asp:TextBox runat="server" ID="txtDateToTime" MaxLength="5" Columns="10" CssClass="textbox"
                                    ToolTip="Time format should be 00:00" onchange="dataSplit(this)"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDateToTime" runat="server" ControlToValidate="txtDateToTime"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewCommunication" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="tbLabel" style="width:20%">
                                Type</td>
                            <td>
                                :</td>
                            <td colspan="2"> 
                                <asp:DropDownList ID="ddlCommType" runat="server" CssClass="dropdownlist">
                                    <asp:ListItem Value="Visit">Visit</asp:ListItem>
                                    <asp:ListItem Value="Call">Call</asp:ListItem>
                                    <asp:ListItem Value="Complaint">Complaint</asp:ListItem>
                                    <asp:ListItem Value="Email">Email</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tbLabel" style="width:20%">
                                Status</td>
                            <td>
                                :</td>
                            <td colspan="2"> 
                                <asp:DropDownList ID="ddlCommStatus" runat="server" CssClass="dropdownlist">
                                    <asp:ListItem Value="Follow up">Follow up</asp:ListItem>
                                    <asp:ListItem Value="Closed">Closed</asp:ListItem>                                   
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tbLabel" style="width:20%">
                                Comment</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="2"
                                        Columns="40" MaxLength="1000" CssClass="textarea" /></td>
                        </tr>
                        </table>   
                        <div style="text-align: center;">
                            <asp:Button CssClass="button" ID="btnAddUpdateCommunication" runat="server" Text="Submit" OnCommand="AddUpdateCommunication" 
                                ValidationGroup="valGrpNewCommunication" />                            
                        </div> 
                         
                        <table class="tTable1" style="margin-left: 8px; background-color: White;" cellspacing="5" cellpadding="5" border="0" width="99%">
                        
                        
                        <tr>
                        <td>
                        <div id="Div8" style="text-align: left;" runat="server">
                            <asp:Label ID="lblCommCommentRecordSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable1">
                        <asp:DataGrid ID="dgCommRecordComment" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="CommID" GridLines="none" OnItemDataBound="dgCommRecordComment_ItemDataBound" OnDeleteCommand="dgCommRecordComment_DeleteCommand" OnItemCommand="dgCommRecordComment_Command" CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgCommRecordComment_PageIndexChanged" EnableViewState="true" OnEditCommand="dgCommRecordComment_EditCommand" OnUpdateCommand="dgCommRecordComment_UpdateCommand">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <HeaderTemplate>
											   
										    </HeaderTemplate>
										    <ItemTemplate>
										        
											    <%# (Container.ItemIndex + 1) + ((dgCommRecordComment.CurrentPageIndex) * dgCommRecordComment.PageSize)%>
											    .
											    <input type="hidden" id="CommentID" runat="server" value='<%# Eval("CommentID")%>' />
											</ItemTemplate>
									    </asp:TemplateColumn>  								    
									                                           
                                        
                                        <asp:TemplateColumn HeaderText="Comment" HeaderStyle-Wrap="false" ItemStyle-Width="390px">									    
										    
										    <ItemTemplate>	
										        <span style="color:Green; size:7px; font-style:italic;">Created By <%# Eval("CreatedByName")%> on <%# Eval("CreatedDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("CreatedDate", "{0: dd/MM/yyyy HH:mm}")%></span><br />
       								            <%# Eval("Comment")%>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditComment" runat="server" TextMode="MultiLine" Rows="2"
                                        Columns="40" MaxLength="1000" CssClass="textarea" Text='<%# Eval("Comment")%>' />
											    
										    </EditItemTemplate>						    
									    </asp:TemplateColumn> 
									                                      
									    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" 
									        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
									        
									        <ItemTemplate>	
									             <asp:LinkButton ID="lnkEditCommComment" runat="server" CommandName="Edit" CssClass="EditButt" EnableViewState="false" CausesValidation="false">Edit</asp:LinkButton>&nbsp; 								      							               								             								        
									             <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false" CssClass="DeleteButt" ><span>Delete</span></asp:LinkButton>
									        </ItemTemplate>
									        <EditItemTemplate>
											    <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
												    ValidationGroup="valGrpEditRow" CssClass="SaveButt"><span>Save</span></asp:LinkButton>
											    <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
												    CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
										    </EditItemTemplate>
										    
									    </asp:TemplateColumn>
									    
									    
								    </Columns>
								    <HeaderStyle CssClass="tHeader" />
								    <AlternatingItemStyle CssClass="tAltRow" />
								    <FooterStyle CssClass="tFooter" />
								    <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
							    </asp:DataGrid>
                        </div>                       
                        </td>
                        </tr>
                        </table>
                        
                        </asp:Panel>
                        
                        <asp:Button runat="server" ID="HiddenForModalCommunicationComment" style="display: none" /> 
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="HiddenForModalCommunicationComment"
        PopupControlID="pnlCommunicationComment" BackgroundCssClass="modalBackground" CancelControlID="btnCommunicationCancel" />
                        <asp:Panel ID="pnlCommunicationComment" runat="server" Style="display: none; width: 550px; background-color: White;
                            border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">  
                            
                            
                        </asp:Panel>
                        
                        
                        <table align="center" style="margin-left: auto; margin-right: auto;">
                        <tr align="center"><td><div id="loading_communication" style="visibility:hidden"><span style="font-style:italic; color:Red;">Loading ...</span></div></td></tr>
                        </table> 
                        
                        <table class="tTable1" style="background-color:White;">                         
                        <tr>
                        <td>
                        <div id="Div7" style="text-align: left;" runat="server">
                            <asp:Label ID="lblCommRecordSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable1" width="100%">
                        <asp:DataGrid ID="dgCommRecord" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="CommID" GridLines="none" OnItemDataBound="dgCommRecord_ItemDataBound" OnDeleteCommand="dgCommRecord_DeleteCommand" OnItemCommand="dgCommRecord_Command" CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgCommRecord_PageIndexChanged" EnableViewState="true" OnSortCommand="SortCommunication" AllowSorting="true">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# (Container.ItemIndex + 1) + ((dgCommRecord.CurrentPageIndex) * dgCommRecord.PageSize)%>
											    .
											    <input type="hidden" id="hidCommID" runat="server" value='<%# Eval("CommID")%>' />
											    </ItemTemplate>
									    </asp:TemplateColumn>  
									    
									    <asp:TemplateColumn HeaderText="From Date" SortExpression="FromDateTime" ItemStyle-Width="115px">
                                                <ItemTemplate>                                                   
												    <%# Eval("FromDateTime").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("FromDateTime", "{0: dd/MM/yyyy HH:mm}")%>                                                   
                                                </ItemTemplate>
                                        </asp:TemplateColumn> 
                                        <asp:TemplateColumn HeaderText="To Date" SortExpression="ToDateTime" ItemStyle-Width="115px">
                                                <ItemTemplate>                                                    
												    <%# Eval("ToDateTime").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("ToDateTime", "{0: dd/MM/yyyy HH:mm}")%>                                                    
                                                </ItemTemplate>
                                        </asp:TemplateColumn>  
                                        <asp:TemplateColumn HeaderText="Type" SortExpression="Type" HeaderStyle-Wrap="false" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                
                                               <%# Eval("Type")%>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateColumn> 
                                        <asp:TemplateColumn HeaderText="Status" SortExpression="Status" HeaderStyle-Wrap="false" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                
                                               <%# Eval("Status")%>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateColumn> 
                                        <asp:TemplateColumn HeaderText="Description" SortExpression="Description" HeaderStyle-Wrap="false" ItemStyle-Width="200px">										    
										    <ItemTemplate>
										        <%# Eval("Description")%>
										        <asp:Image ID="imgMagnify" runat="server" ImageUrl="../../images/icons/chat.png" />
                                                <ajaxToolkit:PopupControlExtender ID="PopupControlExtender1" runat="server" PopupControlID="Panel1"
                                                            TargetControlID="imgMagnify" DynamicContextKey='<%# Eval("CoyID").ToString() + ";" + Eval("AccountCode").ToString() + ";" + Eval("CommID").ToString() %>'
                                                            DynamicControlID="Panel1" DynamicServiceMethod="GetDynamicContent" Position="Right">
                                                </ajaxToolkit:PopupControlExtender>
										    </ItemTemplate>						    
									    </asp:TemplateColumn>
									    								    
									            
									    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" 
									        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
									        <ItemTemplate>	
									             <asp:LinkButton ID="lnkEditCommunication" runat="server" CommandName="EditCommunication" CssClass="EditButt">Edit</asp:LinkButton>&nbsp; 
									                   
									             <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false" CssClass="DeleteButt" ><span>Delete</span></asp:LinkButton>
									        </ItemTemplate>
										    
									    </asp:TemplateColumn>
									    
									    
								    </Columns>
								    <HeaderStyle CssClass="tHeader" />
								    <AlternatingItemStyle CssClass="tAltRow" />
								    <FooterStyle CssClass="tFooter" />
								    <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
							    </asp:DataGrid>
                        </div>
                        </td>
                        </tr>
                        </table>
                        
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                
                    

                    <asp:UpdatePanel ID="upAttachment" runat="server" UpdateMode="Conditional" Visible="false">
                        <ContentTemplate>
                            
                            <table class="tTable1" cellspacing="5" cellpadding="5" border="0" width="99%">
                            <tr>
                                <td style="width: 3%">
                                    <asp:Label CssClass="tbLabel" ID="lblFileName" runat="server">Title</asp:Label></td>
				                <td style="width: 1%">:</td>
                                <td class="tbLabel">
                                    <asp:TextBox runat="server" ID="txtFileName" MaxLength="100" Columns="45" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFileName" runat="server" ControlToValidate="txtFileName" ErrorMessage="*" Display="dynamic" ValidationGroup="attachment" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3%">
                                    <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">Location</asp:Label></td>
				                <td style="width: 1%">:</td>
                                <td class="tbLabel">
                                <asp:FileUpload CssClass="textbox" ID="FileUpload1" runat="server" Width="300px" />                     
                                <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="FileUpload1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="attachment" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3%"></td>                                   
				                <td style="width: 1%"></td>
                                <td>
                                <asp:Button ID="btnUpload" Text="Upload" EnableViewState="False" runat="server" CssClass="button"
                                OnClick="btnUpload_Click" ValidationGroup="attachment" ></asp:Button></td>
                            </tr>
                            </table>  
                            
                            <br />
                            
                            <table class="tTable1" style="background-color:White;">
                            
                            <tr>
                            <td>
                            <span style="color:Red; size:7px; font-style:italic;"><asp:Label ID="lblAttachmentMsg" runat="server" ></asp:Label></span>           
                            </td>
                            </tr>
                             
            <tr>
                    <td>
                        <div id="Div2" style="text-align: left;" runat="server">
                            <asp:Label ID="lblAttachmentSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable1">
                        <asp:DataGrid ID="dgAttachment" runat="server" AutoGenerateColumns="false" 
                             GridLines="none" CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true"
                             PageSize="20" OnPageIndexChanged="dgAttachment_PageIndexChanged" EnableViewState="true" OnSortCommand="SortAttachment" AllowSorting="true"
                             OnItemDataBound="dgAttachment_ItemDataBound" OnDeleteCommand="dgAttachment_DeleteCommand" OnItemCommand="dgAttachment_Command" >
                                        <Columns>
                                        <asp:TemplateColumn HeaderText="No">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize) %>
                                                    
                                                    </ItemTemplate>
                                            </asp:TemplateColumn> 
                                            <asp:TemplateColumn HeaderText="Title" SortExpression="DocumentName" HeaderStyle-Wrap="false" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkName" runat="server" Text='<%# Eval("DocumentName")%>' CommandName="Load" ForeColor="#005DAA" CommandArgument='<%#Eval("FileName")%>'></asp:LinkButton> 
                                                    
                                                    <input type="hidden" id="hidDocumentID" runat="server" value='<%# Eval("DocumentID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                         
                                            
                                            
                                            <asp:TemplateColumn HeaderText="Uploaded Date" SortExpression="CreatedDate" HeaderStyle-Wrap="false" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <%# Eval("CreatedDate", "{0:dd/MM/yyyy}")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            
                                            <asp:TemplateColumn HeaderText="Uploaded By" SortExpression="userrealname" HeaderStyle-Wrap="false" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <%# Eval("userrealname")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>  
                                            <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" 
									        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
									        <ItemTemplate>									           							        
									             <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false" CssClass="DeleteButt" ><span>Delete</span></asp:LinkButton>
									        </ItemTemplate>	
									    </asp:TemplateColumn>                                                                                            
                                        </Columns>
                                        <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                        <AlternatingItemStyle CssClass="tAltRow" />
                                        <FooterStyle CssClass="tFooter" />
                                        <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                    </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
                       
          
                        </ContentTemplate>
                            <Triggers>
                                
                                <asp:PostBackTrigger controlid="btnUpload" />                                                                   
                            </Triggers>
                    </asp:UpdatePanel>
                
                    <asp:UpdatePanel ID="upFinance" runat="server" UpdateMode="Conditional" Visible="false">
                        <ContentTemplate>
                            Under construction.
                        </ContentTemplate>
                    </asp:UpdatePanel>
                
           
            
                    <asp:UpdatePanel ID="upSales" runat="server" UpdateMode="Conditional" Visible="false">
                        <ContentTemplate>
                        
                <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0" width="99%">
                <tr>
                    <td class="tbLabel" style="width: 25%">
                        Trn Date From</td>
                    <td style="width: 1%">
                        :</td>
                    <td class="tbLabel" style="width: 25%">
                        <asp:TextBox runat="server" ID="trnDateFrom" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                        <img id="imgCalendarEditFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                            height="20" width="17" alt="" align="absMiddle" border="0"></td>
                    <td class="tbLabel" style="width: 25%">
                        To</td>
                    <td style="width: 1%">
                        :</td>
                    <td class="tbLabel" style="width: 25%">
                        <asp:TextBox runat="server" ID="trnDateTo" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                        <img id="img1" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                            height="20" width="17" alt="" align="absMiddle" border="0">
                    </td>
                </tr>                
                <tr>
                <td class="tbLabel" style="width: 25%">
                    Product Code</td>
                <td style="width: 1%">
                    :</td>
                <td colspan = "4">
                    <asp:TextBox runat="server" ID="txtProductCode" MaxLength="20" Columns="20" onfocus="select();"
                        CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. B1110535616 </span>
                </td>
                
                </tr>
                <tr>
                    <td class="tbLabel" style="width: 25%">
                        Product Name</td>
                    <td style="width: 1%">
                        :</td>
                    <td colspan = "4">
                        <asp:TextBox runat="server" ID="txtProductName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. BLUE-TIG 5356 </span>
                    </td>
                    
                </tr>
                <tr>
                    <td class="tbLabel" style="width: 25%">
                        Product Group Code</td>
                    <td style="width: 1%">
                        :</td>
                    <td colspan = "4">
                        <asp:TextBox runat="server" ID="txtProductGroup" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. B11</span></td>
                </tr>
                <tr>
                    <td class="tbLabel" style="width: 25%">
                        Product Group Name</td>
                    <td style="width: 1%">
                        :</td>
                    <td colspan = "4">
                        <asp:TextBox runat="server" ID="txtProductGroupName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. BLUEMETALS</span></td>
                </tr>
                
                <tr>
                    <td class="tbLabel" style="width: 25%">
                        Ref. Number</td>
                    <td style="width:1%">
                        :</td>
                    <td colspan = "4">
                    <asp:TextBox runat="server" ID="txtInvoiceNumber" MaxLength="20" Columns="20" onfocus="select();"  CssClass="textbox"> </asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. 4456</span></td>
                </tr> 
                <tr>
                    <td class="tbLabel" style="width: 25%"></td>
                    <td style="width:1%"></td>
                    
                    <td colspan = "4" align="right">
                        <asp:Button ID="btnThisYear" Text="This Year" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnThisYearSales_Click" ></asp:Button>&nbsp;
                        <asp:Button ID="btnPrevMthSales" Text="Previous Month" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnPreviousMonthSales_Click" ></asp:Button>&nbsp; 
                        <asp:Button ID="btnCurrentMthSales" Text="Current Month" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnCurrentMonthSales_Click" ></asp:Button>&nbsp; 
                        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnSearch_Click" ></asp:Button></td>
                    
                </tr>
                               
            </table>
            <br />
            
              
             <table class="tTable1" style="margin-left: 8px;background-color:White;">
            <tr>
                    <td>
                        <div id="Div1" style="text-align: left;" runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable1">
                        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" 
                             GridLines="none" CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true"
                             PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true" OnSortCommand="SortData" AllowSorting="true">
                                        <Columns>
                                        <asp:TemplateColumn HeaderText="No">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                                    .</ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Trn Date" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <%# Eval("TrnDate", "{0:dd/MM/yyyy}")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Ref No" SortExpression="TrnNo" HeaderStyle-Wrap="false">
                                                <ItemTemplate>                                                  
                                                    <a href="#" title="Invoice Detail" onclick='ViewInvoice("<%#  Eval("TrnNo")%>","<%# Eval("TrnType")%>");return false;'><%# Eval("TrnNo")%></a>                                                    
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Invoice No" SortExpression="DONo" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDONo" runat="server" Width="70px">
                                                                        <%# Eval("DONo")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Customer Code" SortExpression="AccountCode" HeaderStyle-Wrap="false" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductName" runat="server">
                                                        <%# Eval("AccountCode")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Customer Name" SortExpression="AccountName" HeaderStyle-Wrap="false" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductName" runat="server">
                                                        <%# Eval("AccountName")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                          
                                            
                                                                                                                                                               
                                        </Columns>
                                        <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                        <AlternatingItemStyle CssClass="tAltRow" />
                                        <FooterStyle CssClass="tFooter" />
                                        <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                    </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    

               
                    <asp:UpdatePanel ID="upCollection" runat="server" UpdateMode="Conditional" Visible="false">
                        <ContentTemplate>
                        
                   <table class="tTable1" cellspacing="5" cellpadding="5" border="0" width="99%">
                    <tr>
                    <td class="tbLabel" style="width: 25%">
                        Receipt Date From</td>
                    <td style="width: 1%">
                        :</td>
                    <td class="tbLabel" style="width: 25%">
                        <asp:TextBox runat="server" ID="txtReceiptFrom" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                        <img id="img2" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                            height="20" width="17" alt="" align="absMiddle" border="0"></td>
                    <td class="tbLabel" style="width: 25%">
                        To</td>
                    <td style="width: 1%">
                        :</td>
                    <td class="tbLabel" style="width: 25%">
                        <asp:TextBox runat="server" ID="txtReceiptTo" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                        <img id="img3" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                            height="20" width="17" alt="" align="absMiddle" border="0">
                    </td>
                </tr> 
                
                <tr>
                    <td class="tbLabel" style="width: 25%">
                        Receipt No</td>
                    <td style="width: 1%">
                        :</td>
                    <td colspan="4">
                        <asp:TextBox runat="server" ID="txtReceiptNo" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. 123</span></td>
                </tr>  
               
                <tr>
                    <td class="tbLabel" style="width: 25%">
                         Invoice No</td>
                    <td style="width:1%">
                        :</td>
                    <td colspan="4">
                    <asp:TextBox runat="server" ID="txtInvoiceNo" MaxLength="20" Columns="20" onfocus="select();"
                                    CssClass="textbox"></asp:TextBox></td>
                </tr>   
                
                <tr>
                    <td class="tbLabel" style="width: 25%"></td>
                    <td style="width:1%"></td>                   
                    <td colspan = "4" align="right">
                        <asp:Button ID="btnThisYearCollections" Text="This Year" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnThisYearCollections_Click" ></asp:Button>
                        <asp:Button ID="btnPreviousMonthCollections" Text="Previous Month" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnPreviousMonthCollections_Click" ></asp:Button>
                        <asp:Button ID="btnCurrentMonth" Text="Current Month" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnCurrentMonthCollections_Click" ></asp:Button>     
                        <asp:Button ID="btnCollections" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnCollections_Click" ></asp:Button>
                    </td>
                    
                </tr>             
                                  
                
                 
                </table>     
                        
                    <br />    
                    <table class="tTable1" style="background-color:White;">
                     <tr>
                    <td>
                        <div id="Div4" style="text-align: left;" runat="server">
                            <asp:Label ID="lblCollectionSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable1">
                               <table class="tTable1">
                            <tr>
                                <td><asp:datagrid id="dgCollections" runat="server" AutoGenerateColumns="false" 
                             GridLines="none" CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true"
                             PageSize="20" OnPageIndexChanged="dgCollections_PageIndexChanged" EnableViewState="true" OnSortCommand="SortCollections" AllowSorting="true">
                                    <Columns>
                                    <asp:TemplateColumn ItemStyle-Width="10px" ItemStyle-Wrap="True">
                                    <HeaderTemplate>
                                </td>
                                <td width="500" colspan="4" align="center">Receipt</td>
                                <td width="500" colspan="4" align="center" style="background-color: #DADADB">Invoice</td>
                                <td width="100" align="center">&nbsp;</td>
                                
                            </tr>
                            <td Align="Center" Height="10px" style="background-color: #EDF3FF">&nbsp;</td>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgCollections.CurrentPageIndex) * dgCollections.PageSize)%>
                            </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Receipt Date" SortExpression="RECEIPT_TrnDate" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#EDF3FF" HeaderStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <%# Eval("Receipt_TrnDate", "{0:dd/MM/yyyy}")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                            
                                            <asp:TemplateColumn HeaderText="Ref No." SortExpression="allcdocno" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#EDF3FF" HeaderStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <%# Eval("allcdocno")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Curr" SortExpression="RECEIPT_Currency" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#EDF3FF" HeaderStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <%# Eval("RECEIPT_Currency")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Receipt Amount" SortExpression="Receipt_Amount" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#EDF3FF" HeaderStyle-Font-Bold="true" >
                                                <ItemTemplate>
                                                   <%# string.Format("{0:0,0.00}", Eval("Receipt_Amount"))%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn> 
                                            <asp:TemplateColumn HeaderText="Invoice Date" SortExpression="SALES_TrnDate" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#DADADB" HeaderStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <%# Eval("SALES_TrnDate", "{0:dd/MM/yyyy}")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>   
                                            <asp:TemplateColumn HeaderText="Invoice No" SortExpression="DoNo" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#DADADB" HeaderStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <%# Eval("DoNo")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Curr" SortExpression="SALES_Currency" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#DADADB" HeaderStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <%# Eval("SALES_Currency")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn> 
                                            <asp:TemplateColumn HeaderText="Invoice Amount" SortExpression="Sales_Amount" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#DADADB" HeaderStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                   <%# string.Format("{0:0,0.00}", Eval("Sales_Amount"))%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Days" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#EDF3FF" HeaderStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                   <%# Eval("DayDiff")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                  
                                </columns> 
                                        <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                        <AlternatingItemStyle CssClass="tAltRow" />
                                        <FooterStyle CssClass="tFooter" />
                                        <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                        </asp:datagrid></TD></TR>
                            </table>                   
                                   
                        </div>
                    </td>
                </tr>
            </table>   
            
            
            
            
            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    
               
            
                    <asp:UpdatePanel ID="upOutstanding" runat="server" UpdateMode="Conditional" Visible="false">
                        <ContentTemplate>
                        
                        <table>
                        <tr>
                            <td>
                            <asp:Button ID="btnAllOutstanding" Text="ALL" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnSearchAllOutstanding_Click" ></asp:Button>
                            
                            
                            <asp:Button ID="Greater90DaysOutstanding" Text="> 90 Days" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnSearchGreater90DaysOutstanding_Click" ></asp:Button>
                           
                           
                            <asp:Button ID="Greater180DaysOutstanding" Text="> 180 Days" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnSearchGreater180DaysOutstanding_Click" ></asp:Button>
                            </td>
                        </td>
                        </tr> 
                        </table>
                        <br />
                        <div align="center">
                        <asp:Panel ID="pnlOutstandingPaymentProgress" runat="server">
                             <table width="10%">                             
                             <tr align="center">
                                <td><img src = "../../images/dg_loading.gif" /></td><td> Loading...</td>
                             </tr>
                             </table>
                        </asp:Panel>
                        </div>
                             <table class="tTable1" style="background-color:White;">
            <tr>
                    <td>
                        <div id="Div3" style="text-align: left;" runat="server">
                            <asp:Label ID="lblOutstandingPaymentsSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable1">
                        <asp:DataGrid ID="dgOutstanding" runat="server" AutoGenerateColumns="false" 
                             GridLines="none" CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true"
                             PageSize="20" OnPageIndexChanged="dgOutstanding_PageIndexChanged" EnableViewState="true" OnSortCommand="SortOutstanding" AllowSorting="true"
                             >
                                        <Columns>
                                        <asp:TemplateColumn HeaderText="No">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize) %>
                                                    
                                                    </ItemTemplate>
                                            </asp:TemplateColumn> 
                                            <asp:TemplateColumn HeaderText="Invoice Date" SortExpression="SALES_TrnDate" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <%# Eval("SALES_TrnDate", "{0:dd/MM/yyyy}")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Invoice No" SortExpression="DocNo" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <%# Eval("DocNo")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Invoice Amount" SortExpression="SALES" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                   <%# string.Format("{0:0,0.00}",Eval("SALES"))%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Outstanding Amount" SortExpression="Outstanding" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                   <%# string.Format("{0:0,0.00}", Eval("Outstanding"))%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Days Diff" SortExpression="DayDiff" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <%# Eval("DayDiff")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn HeaderText="SalesPersonName" SortExpression="SalesPersonName" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <%# Eval("SalesPersonName")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                                                                                                  
                                        </Columns>
                                        <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                        <AlternatingItemStyle CssClass="tAltRow" />
                                        <FooterStyle CssClass="tFooter" />
                                        <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                    </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                
                    <asp:UpdatePanel ID="upPurchase" runat="server" UpdateMode="Conditional" Visible="false">
                        <ContentTemplate>
                        
                        <table class="tTable1" style="background-color:White;">
                        <tr>
                            <td>
                            <span style="color:Red; size:7px; font-style:italic;"><asp:Label ID="lblPurchaseMsg" runat="server" ></asp:Label></span>           
                            </td>
                        </tr>
                        <tr><td>
                        <asp:Button ID="btnAddPurchase" Text="Add Purchase" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnAddPurchase_Click"  />
                        </td></tr>
                        </table> 
                        <table align="center" style="margin-left: auto; margin-right: auto;">
                        <tr align="center"><td><div id="loading_purchase" style="visibility:hidden"><span style="font-style:italic; color:Red;">Loading ...</span></div></td></tr>
                        </table> 
                        
                        
                        <table class="tTable1" style="background-color:White;">
                        <tr>
                        <td>
                        <div id="Div5" style="text-align: left;" runat="server">
                            <asp:Label ID="lblPurchasesSumary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable1">
                        <asp:DataGrid ID="dgPurchases" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="PurchaseID" GridLines="none" OnItemDataBound="dgPurchases_ItemDataBound" OnDeleteCommand="dgPurchases_DeleteCommand" OnItemCommand="dgPurchases_Command"
								    CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgPurchases_PageIndexChanged" EnableViewState="true" OnSortCommand="SortPurchase" AllowSorting="true">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# (Container.ItemIndex + 1) + ((dgPurchases.CurrentPageIndex) * dgPurchases.PageSize)%>
											    .
											    <input type="hidden" id="hidID" runat="server" value='<%# Eval("PurchaseID")%>' />
											    </ItemTemplate>
											    
									    </asp:TemplateColumn>
									    

									    <asp:TemplateColumn HeaderText="Supplier" SortExpression="Supplier" HeaderStyle-Wrap="false" ItemStyle-Width="100px">
										    
										    <ItemTemplate>
										        <asp:Label ID="lblSupplier" runat="server">
										        
                                                 <asp:LinkButton ID="lnkEditPurchase" runat="server" CommandName="EditPurchase" Text='<%# Eval( "Supplier" )%>'
                                        CausesValidation="false" ></asp:LinkButton>										          
										           
											    </asp:Label>
										    </ItemTemplate>										    
										    										    
									    </asp:TemplateColumn>				    
									 
                                        <asp:TemplateColumn HeaderText="Industry" SortExpression="industryName" HeaderStyle-Wrap="false" ItemStyle-Width="120px">
                                            <ItemTemplate>
                                                
                                               <%# Eval("industryName")%>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateColumn>
                                        
                                        <asp:TemplateColumn HeaderText="Product Group" SortExpression="ProductGroup" HeaderStyle-Wrap="false" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <%# Eval("ProductGroup")%>
                                               
                                            </ItemTemplate>
                                            
                                        </asp:TemplateColumn>
                                        
                                        <asp:TemplateColumn HeaderText="Product Name" SortExpression="ProductName" HeaderStyle-Wrap="false" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <%# Eval("ProductName")%>
                                               
                                            </ItemTemplate>
                                            
                                        </asp:TemplateColumn>
                                        
                                        <asp:TemplateColumn HeaderText="UOM" SortExpression="UOM" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <%# Eval("UOM")%>
                                                    </ItemTemplate>
                                                   
                                                </asp:TemplateColumn>
									    							    
									    <asp:TemplateColumn HeaderText="Qty" SortExpression="Qty" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <%# Eval("Qty", "{0:f2}")%>
                                                    </ItemTemplate>
                                                    
                                       </asp:TemplateColumn>
                                       <asp:TemplateColumn HeaderText="Contract End Date" SortExpression="ContractEndDate" ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContractEndDate" runat="server">
												    <%# Eval("ContractEndDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("ContractEndDate", "{0: dd-MMM-yyyy}")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                
                                                
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateColumn>	
                                       							    
									    
									    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" 
									        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
									        <ItemTemplate>									             								        
									             <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false" CssClass="DeleteButt" ><span>Delete</span></asp:LinkButton>
									        </ItemTemplate>
										    
									    </asp:TemplateColumn>
									    
									    
								    </Columns>
								    <HeaderStyle CssClass="tHeader" />
								    <AlternatingItemStyle CssClass="tAltRow" />
								    <FooterStyle CssClass="tFooter" />
								    <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
							    </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
            
            
            
            <asp:Button runat="server" ID="HiddenForModalAddNewPurchase" style="display: none" /> 
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenForModalAddNewPurchase"
        PopupControlID="Panel2" BackgroundCssClass="modalBackground" CancelControlID="btnAddCancelPurchase" />
    <asp:Panel ID="Panel2" runat="server" Style="display: none; width: 680px; background-color: White;
        border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
        
        
<table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0"
                width="99%">
    <tr>
                <td style="width: 6%" colspan="3">
                    <span style="font-weight:bold; font-size: 14px;"><asp:Label ID="lblPurchase" runat="server"></asp:Label></span>
                </td>
                <td>
                    <div style="text-align: right;"> 
                    <asp:Button CssClass="button" ID="btnAddCancelPurchase" runat="server" Text="X"  />
                    </div>
                </td>
    </tr>
    <tr>
    <td class="tbLabel" style="width:15%">
        Supplier</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtSupplier" runat="server" Columns="50" MaxLength="50" CssClass="textbox"
            onfocus="select();" /><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                ControlToValidate="txtSupplier" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewPurchase" />
       
         <input type="hidden" id="hidPurchaseID" runat="server" />   
         
    </td>
        </tr>

        <tr>
            <td class="tbLabel" style="width:15%">
                Industry</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:DropDownList ID="ddlPurchaseIndustry" runat="server" DataTextField="Name"
                    DataValueField="IndustryID" CssClass="dropdownlist">
                </asp:DropDownList>
            </td>
        </tr>


        <tr>
            <td class="tbLabel" style="width:15%">
                Product Group</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:TextBox ID="txtPG" runat="server" Columns="30" MaxLength="100" CssClass="textbox"
                    onfocus="select();" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                        ControlToValidate="txtPG" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewPurchase" />
                 <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2" ID="AutoCompleteExtender2"
            TargetControlID="txtPG" ServicePath="AutoCompleteProductGroupName.asmx"
            ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
            EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                        </ajaxToolkit:AutoCompleteExtender>       
            </td>
        </tr>
        <tr>
            <td class="tbLabel" style="width:15%">
                Product Name</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:TextBox ID="txtPN" runat="server" Columns="50" MaxLength="50" CssClass="textbox"
                    onfocus="select();" /><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                        ControlToValidate="txtPN" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewPurchase" />
                  <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx3" ID="AutoCompleteExtender3"
            TargetControlID="txtPN" ServicePath="AutoCompleteProductName.asmx"
            ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
            EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                        </ajaxToolkit:AutoCompleteExtender> 
               
            </td>
        </tr>
        <tr>
            <td class="tbLabel" style="width:15%">
                UOM</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:DropDownList ID="ddlUOM" runat="server" CssClass="dropdownlist">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="tbLabel" style="width:15%">
               Quantity</td>
            <td>
                :</td>
            <td colspan="2">        
                <asp:TextBox ID="txtQuantity" runat="server" Columns="3" MaxLength="3" CssClass="textbox" />
                    <asp:CompareValidator ID="cvNoOfMonthsBonded" runat="server" ErrorMessage="*"
                        Display="Dynamic" ControlToValidate="txtQuantity" Type="Integer" Operator="DataTypeCheck"
                        ValidationGroup="valGrpNewRow" />
            </td>
        </tr>
        <tr>
            <td class="tbLabel" style="width:15%">
            Contract End Date</td>
           <td>
                :</td>
           <td colspan="2"> 
                                <asp:TextBox runat="server" ID="contractEndDate" MaxLength="10" Columns="10" onfocus="select();"
                                    CssClass="textbox"></asp:TextBox>
                                <img id="img4" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"

                                    height="20" width="17" alt="" align="absMiddle" border="0"></td>
        </tr>
        <tr>
            <td class="tbLabel" style="width:15%">
                Remarks</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:TextBox ID="txtPurchaseRemarks" runat="server" TextMode="MultiLine" Rows="3"
                        Columns="40" MaxLength="400" CssClass="textarea" /></td>
        </tr>
    </table>
               
        <div style="text-align: center;">
            <asp:Button CssClass="button" ID="btnAddUpdatePurchase" runat="server" Text="Submit" OnCommand="AddUpdatePurchase" ValidationGroup="valGrpNewPurchase" />
            
        </div>
    </asp:Panel>
            
            
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                
                    <asp:UpdatePanel ID="upOthers" runat="server" Visible="false">
                        <ContentTemplate>
                            Under construction.
                        </ContentTemplate>
                    </asp:UpdatePanel>
                
            </div>
            
            
            </div> 
            </ContentTemplate> 
                               
            </asp:UpdatePanel>        
            
            
            
                  
            <br />
            <div class="TABCOMMAND">
                <asp:UpdatePanel ID="udpMsgUpdater" runat="server"  UpdateMode="Always">
                    <ContentTemplate>
                        <ul>
                            <li>&nbsp;</li>
                        </ul>
                        <uctrl:msgpanel id="PageMsgPanel" runat="server" enableviewstate="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            
            
         
</asp:Content>

