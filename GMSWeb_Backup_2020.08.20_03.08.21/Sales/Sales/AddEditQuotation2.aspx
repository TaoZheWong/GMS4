<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
    Codebehind="AddEditQuotation2.aspx.cs" Inherits="GMSWeb.Sales.Sales.AddEditQuotation2"
    Title="Add/Edit Quotation" ValidateRequest="false" %>
<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">


    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <a name="TemplateInfo"></a>
    <h1>
        Quotation &gt; Add/Edit Quotation</h1>
    <p>
        For salesman to manage a quotation.</p>
    <br />
    <asp:ScriptManager ID="sriptmgr1" runat="server">
        <Services>
            <asp:ServiceReference Path="AutoCompletionProduct.asmx" />
        </Services>
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upOutter" runat="server" UpdateMode="Conditional">
        <Triggers>
                   <asp:AsyncPostBackTrigger ControlID="btnLoadInformation" />
                   <asp:AsyncPostBackTrigger ControlID="btnLoadProductItem" />
                   <asp:AsyncPostBackTrigger ControlID="btnLoadPackageItem" />
                   <asp:AsyncPostBackTrigger ControlID="btnLoadTerms" />                   
            </Triggers>
        <ContentTemplate>
    
    <asp:UpdatePanel ID="updatePanel5" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlRevisionNo" />
        </Triggers>
        <ContentTemplate>
            <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0"
                width="99%">
                <tr>
                    <td class="tbLabel">
                        New Customer</td>
                    <td>
                        :</td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkIsNewCustomer" Text="Yes" OnCheckedChanged="chkIsNewCustomer_OnCheckedChanged"
                            AutoPostBack="true" />
                    </td>
                    <td class="tbLabel">
                        Status</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:Label runat="server" ID="lblStatus"></asp:Label></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        New Customer Name</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNewCustomerName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                    </td>
                    <td class="tbLabel">
                        Quotation No.</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:Label runat="server" ID="lblQuotationNo"></asp:Label></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Account Code</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAccountCode" MaxLength="10" Columns="10" AutoPostBack="true"
                            onfocus="select();" CssClass="textbox" OnTextChanged="txtAccountCode_OnTextChanged"></asp:TextBox>
                        <asp:LinkButton ID="lnkFindAccount" runat="server" CommandName="FindAccount" EnableViewState="true"
                            CssClass="FindButt" OnClientClick="return SearchAccount();"><IMG height="16" src="../../images/icons/FindItem.gif" align="absMiddle"></asp:LinkButton>
                        <input type="hidden" id="hidAccountCode" runat="server" /><input type="hidden" id="hidSalesPersonID"
                            runat="server" />
                    </td>
                    <td class="tbLabel" style="width: 15%">
                        Quotation Date</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtQuotationDate" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                        <img id="img2" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                            height="20" width="17" alt="" align="absMiddle" border="0"></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Account Name</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAccountName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="textbox" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="tbLabel">
                        SO Ref No.</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:Label runat="server" ID="lblSORefNo"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Subject</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtSubject" TextMode="MultiLine" onfocus="select();"
                            Width="90%" Height="30"></asp:TextBox>
                    </td>
                    <td class="tbLabel">
                        Salesman</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlSalesman" runat="Server" DataTextField="SalesPersonName"
                            DataValueField="SalesPersonID" CssClass="dropdownlist" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Unsuccessful</td>
                    <td>
                        :</td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkIsUnsuccessful" Text="Yes" OnCheckedChanged="chkIsUnsuccessful_OnCheckedChanged"
                            AutoPostBack="true" /><br />
                        <asp:LinkButton runat="server" ID="lnkCancelUnsuccessful" Text="Cancel Unsuccessful"
                            Visible="false" EnableViewState="true" OnClick="lnkCancelUnsuccessful_Click" />
                    </td>
                    <td class="tbLabel">
                        Revision</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlRevisionNo" runat="Server" DataTextField="Revision" DataValueField="RevisionNo"
                            CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlRevisionNo_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Order Lost Reason</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtOrderLostReason" TextMode="MultiLine" Width="90%"
                            Height="30" onfocus="select();" EnableViewState="true"></asp:TextBox>
                    </td>
                </tr>
            </table>
            
            
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
    
                    <asp:UpdateProgress ID="uppOutter" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="upOutter">
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
    
    <br />
    <div style="margin-left: 10px; margin-right: 30px">
        
        <table>
            <tr>
                <td><input id="btnLoadInformation" class="crm_button" value="Information" runat="server" type="button" onserverclick="btnLoadInformation_Click" ></td>
                <td><input id="btnLoadProductItem" class="crm_button" value="Product Item" runat="server" type="button" onserverclick="btnLoadProductItem_Click" ></td> 
                <td><input id="btnLoadPackageItem" class="crm_button" value="Package Item" runat="server" type="button" onserverclick="btnLoadPackageItem_Click" ></td>
                <td><input id="btnLoadTerms" class="crm_button" value="Terms & Conditions" runat="server" type="button" onserverclick="btnLoadTerms_Click" ></td>                
            </tr>            
        </table>
        
        <div style="padding: 8px; border-color:Gray; border-style:solid; border-color:grey; border-width:1px; margin-left:2px; ">
        
                    <asp:UpdatePanel ID="updatePanel1" runat="server" Visible="false">
                        <ContentTemplate>
                            <table class="tTable1" cellspacing="5" cellpadding="5" border="0" width="99%">
                                <tr>
                                    <td class="tbLabel" rowspan="4" valign="top">
                                        Delivery Address</td>
                                    <td style="width: 5%" rowspan="4" valign="top">
                                        :</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtAddress1" MaxLength="40" Columns="20" onfocus="select();"
                                            CssClass="textbox" TabIndex="1"></asp:TextBox>
                                    </td>
                                    <td class="tbLabel">
                                        Attention To</td>
                                    <td style="width: 5%">
                                        :</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtAttentionTo" MaxLength="40" Columns="20" onfocus="select();"
                                            CssClass="textbox" TabIndex="7"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtAddress2" MaxLength="40" Columns="20" onfocus="select();"
                                            CssClass="textbox" TabIndex="2"></asp:TextBox>
                                    </td>
                                    <td class="tbLabel">
                                        Mobile Phone</td>
                                    <td style="width: 5%">
                                        :</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtMobilePhone" MaxLength="20" Columns="20" onfocus="select();"
                                            CssClass="textbox" TabIndex="8"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtAddress3" MaxLength="40" Columns="20" onfocus="select();"
                                            CssClass="textbox" TabIndex="3"></asp:TextBox>
                                    </td>
                                    <td class="tbLabel">
                                        Office Phone</td>
                                    <td style="width: 5%">
                                        :</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtOfficePhone" MaxLength="20" Columns="20" onfocus="select();"
                                            CssClass="textbox" TabIndex="9"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtAddress4" MaxLength="40" Columns="20" onfocus="select();"
                                            CssClass="textbox" TabIndex="4"></asp:TextBox>
                                    </td>
                                    <td class="tbLabel">
                                        Fax</td>
                                    <td style="width: 5%">
                                        :</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFax" MaxLength="20" Columns="20" onfocus="select();"
                                            CssClass="textbox" TabIndex="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tbLabel">
                                        Customer PO No.</td>
                                    <td style="width: 5%">
                                        :</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtCustomerPONo" MaxLength="15" Columns="20" onfocus="select();"
                                            CssClass="textbox" TabIndex="5"></asp:TextBox>
                                    </td>
                                    <td class="tbLabel">
                                        Email</td>
                                    <td style="width: 5%">
                                        :</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtEmail" MaxLength="40" Columns="20" onfocus="select();"
                                            CssClass="textbox" TabIndex="11"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tbLabel">
                                        Internal Remarks</td>
                                    <td style="width: 5%">
                                        :</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtInternalRemarks" Width="90%" Height="30" TextMode="MultiLine"
                                            onfocus="select();" TabIndex="6"></asp:TextBox>
                                    </td>
                                    <td class="tbLabel">
                                        Currency</td>
                                    <td style="width: 5%">
                                        :</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlCurrency" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                                            AutoPostBack="true" TabIndex="12">
                                        </asp:DropDownList>
                                        <%-- <asp:TextBox runat="server" ID="txtCurrencyRate" MaxLength="20"
                                            Columns="10" onfocus="select();" CssClass="textbox" TabIndex="13" /> --%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tbLabel">
                                        External Remarks</td>
                                    <td style="width: 5%">
                                        :</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtExternalRemarks" Width="90%" Height="30" TextMode="MultiLine"
                                            onfocus="select();" TabIndex="6"></asp:TextBox>
                                    </td>
                                    <td class="tbLabel">
                                    </td>
                                    <td style="width: 5%">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
               
           
                    <asp:UpdatePanel ID="updatePanel2" runat="server" Visible="false">                        
                        <ContentTemplate>                            
                            <asp:Button runat="server" ID="btnHideMSP" Text="Hide MSP" CssClass="button" OnClick="btnHideMSP_Click" />
                            <table class="tTable1" cellspacing="5">
                                <tr>
                                    <td colspan="3">
                                        <input type="hidden" id="hidDetailsCount" runat="server" />
                                        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true"
                                            OnItemCommand="dgData_CreateCommand" OnDeleteCommand="dgData_DeleteCommand" OnEditCommand="dgData_EditCommand"
                                            OnCancelCommand="dgData_CancelCommand" OnUpdateCommand="dgData_UpdateCommand"
                                            OnItemDataBound="dgData_ItemDataBound" GridLines="none" Width="100%" >
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>'>
                                                        </asp:Label>
                                                        .</ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Product Code">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
                                                            CausesValidation="false"><span><%# Eval("ProductCode")%></span></asp:LinkButton>
                                                        <asp:Image ID="imgMagnify" runat="server" ImageUrl="../../images/icons/box_closed.png" />
                                                        <ajaxToolkit:PopupControlExtender ID="PopupControlExtender1" runat="server" PopupControlID="Panel1"
                                                            TargetControlID="imgMagnify" DynamicContextKey='<%# Eval("CoyID").ToString() + ";" + Eval("ProductCode").ToString() %>'
                                                            DynamicControlID="Panel1" DynamicServiceMethod="GetDynamicContent" Position="Right">
                                                        </ajaxToolkit:PopupControlExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewProductCode" runat="server" Columns="12" MaxLength="15" CssClass="textbox"
                                                            onfocus="select();" onchange="this.value = this.value.toUpperCase()" OnTextChanged="txtNewProductCode_OnTextChanged"
                                                            AutoPostBack="true" />
                                                        <asp:RequiredFieldValidator ID="rfvNewProductCode" runat="server" ControlToValidate="txtNewProductCode"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Product" />
                                                        <asp:LinkButton ID="lnkFindProduct" runat="server" CommandName="FindProduct" EnableViewState="true"
                                                            CssClass="FindButt" OnClientClick="return SearchProduct(this);"><IMG height="16" src="../../images/icons/FindItem.gif" align="absMiddle"></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Product Description">
                                                    <ItemTemplate>
                                                        <%# Eval("ProductDescription")%>
                                                        <asp:LinkButton ID="lnkEditDescription" runat="server" EnableViewState="true" CssClass="EditButt"
                                                            OnClientClick='<%# "return EditDescription(" + Eval("SNNo").ToString() + ");" %>'
                                                            Visible='<%#(lblQuotationNo.Text != "" && this.lblStatus.Text.ToUpper() != "UNSUCCESSFUL")%>'>Details</asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditProductDescription" runat="server" Columns="30"
                                                            MaxLength="80" Text='<%# Eval("ProductDescription") %>' />
                                                        <asp:RequiredFieldValidator ID="rfvEditProductDescription" runat="server" ControlToValidate="txtEditProductDescription"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewProductDescription" runat="server" Columns="30" MaxLength="80"
                                                            CssClass="textbox" onfocus="select();" onchange="this.value = this.value.toUpperCase()" AutoPostBack="true" OnTextChanged="txtNewProductDescription_OnTextChanged" />
                                                        <asp:RequiredFieldValidator ID="rfvNewProductDescription" runat="server" ControlToValidate="txtNewProductDescription"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Product" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="UOM" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <%# Eval("UOM")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlEditUOM">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlNewUOM">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Qty">
                                                    <ItemTemplate>
                                                        <%# Eval("Quantity", "{0:f2}")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditQuantity" runat="server" Columns="5" Text='<%# Eval("Quantity", "{0:f2}") %>' />
                                                        <asp:RequiredFieldValidator ID="rfvEditQuantity" runat="server" ControlToValidate="txtEditQuantity"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                                                ID="rgEditQuantity" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditQuantity"
                                                                Type="Double" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewQuantity" runat="server" Columns="5" OnTextChanged="Price_OnTextChanged"
                                                            AutoPostBack="true" />
                                                        <asp:RequiredFieldValidator ID="rfvNewQuantity" runat="server" ControlToValidate="txtNewQuantity"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Product" /><asp:RangeValidator
                                                                ID="rgNewQuantity" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewQuantity"
                                                                Type="Double" ValidationGroup="Product" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="List Price">
                                                    <ItemTemplate>
                                                        <%# Eval("ListPrice", "{0:f2}")%>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblListPrice" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="MSP">
                                                    <ItemTemplate>
                                                        <%# Eval("MinSellingPrice", "{0:f2}")%>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblMinSellingPrice" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Unit Price">
                                                    <ItemTemplate>
                                                        <%# Eval("UnitPrice", "{0:f2}")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditUnitPrice" runat="server" Columns="5"
                                                            MaxLength="10" Text='<%# Eval("UnitPrice", "{0:f2}") %>' />
                                                        <asp:RequiredFieldValidator ID="rfvEditUnitPrice" runat="server" ControlToValidate="txtEditUnitPrice"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                                                ID="rgEditUnitPrice" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditUnitPrice"
                                                                Type="Double" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewUnitPrice" runat="server" Columns="5" MaxLength="10"
                                                            OnTextChanged="Price_OnTextChanged" AutoPostBack="true" />
                                                        <asp:RequiredFieldValidator ID="rfvNewUnitPrice" runat="server" ControlToValidate="txtNewUnitPrice"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Product" /><asp:RangeValidator
                                                                ID="rgNewUnitPrice" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewUnitPrice"
                                                                Type="Double" ValidationGroup="Product" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="GP%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblGP" Text='<%# Eval("GPPercentage")%>'></asp:Label>
                                                        <input type="hidden" id="hidTotalCost" runat="server" value='<%# Eval("TotalCost", "{0:f2}")%>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblGPPercentage" />
                                                        <input type="hidden" id="hidWeightedCost" runat="server" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Total Price">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblTotalPrice" Text='<%# Eval("TotalPrice", "{0:f2}")%>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblNewTotalPrice" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Order" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%# Eval("DSNNo").ToString() + Eval("DOptSNNo").ToString()%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditDSNNo" runat="server" Columns="2" MaxLength="3"
                                                            Text='<%# Eval("DSNNo") %>' Height="10px"/>
                                                        <asp:RequiredFieldValidator ID="rfvEditDSNNo" runat="server" ControlToValidate="txtEditDSNNo"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                                        <asp:CompareValidator ID="cvEditDSNNo" runat="server" ErrorMessage="Not an integer"
                                                            Display="Dynamic" ControlToValidate="txtEditDSNNo" Type="Integer" Operator="DataTypeCheck"
                                                            ValidationGroup="valGrpEditRow" />
                                                        <asp:DropDownList runat="server" ID="ddlEditDOptSNNo">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="true"
                                                            CausesValidation="false" CssClass="DeleteButt"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="true"
                                                            ValidationGroup="valGrpEditRow" CssClass="SaveButt"><span>Save</span></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="true"
                                                            CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="true"
                                                            ValidationGroup="Product" CssClass="NewButt" ><span>Add</span></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                        </asp:DataGrid></td>
                                </tr>
                            </table>
                        </ContentTemplate> 
                       
                    </asp:UpdatePanel>
               
           
                    <asp:UpdatePanel ID="updatePanel4" runat="server" Visible="false">
                        <ContentTemplate>
                            <table class="tTable1" cellspacing="5">
                                <tr>
                                    <td>
                                        <asp:DataGrid ID="dgPackage" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                            GridLines="None" CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" OnItemCommand="dgPackage_CreateCommand"
                                            OnEditCommand="dgPackage_EditCommand" OnDeleteCommand="dgPackage_DeleteCommand"
                                            OnItemDataBound="dgPackage_ItemDataBound" OnCancelCommand="dgPackage_CancelCommand"
                                            OnUpdateCommand="dgPackage_UpdateCommand">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                                    <ItemTemplate>
                                                        <%# (Container.ItemIndex + 1) + ((dgPackage.CurrentPageIndex) * dgPackage.PageSize)%>
                                                        .
                                                        <input type="hidden" id="hidSNNo" runat="server" value='<%# Eval("SNNo")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Product Code">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
                                                            CausesValidation="false"><span><%# Eval("ProductCode")%></span></asp:LinkButton>
                                                        <asp:Image ID="imgMagnify2" runat="server" ImageUrl="../../images/icons/box_closed.png" />
                                                        <ajaxToolkit:PopupControlExtender ID="PopupControlExtender2" runat="server" PopupControlID="Panel1"
                                                            TargetControlID="imgMagnify2" DynamicContextKey='<%# Eval("CoyID").ToString() + ";" + Eval("ProductCode").ToString() %>'
                                                            DynamicControlID="Panel1" DynamicServiceMethod="GetDynamicContent" Position="Right">
                                                        </ajaxToolkit:PopupControlExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewPackageProductCode" runat="server" Columns="10" MaxLength="15"
                                                            CssClass="textbox" onfocus="select();" onchange="this.value = this.value.toUpperCase()"
                                                            OnTextChanged="txtNewPackageProductCode_OnTextChanged" AutoPostBack="true" />
                                                        <asp:RequiredFieldValidator ID="rfvNewPackageProductCode" runat="server" ControlToValidate="txtNewPackageProductCode"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Package" />
                                                        <asp:LinkButton ID="lnkFindPackage" runat="server" CommandName="FindPackage" EnableViewState="true"
                                                            CssClass="FindButt" OnClientClick="return SearchPackage(this);"><IMG height="16" src="../../images/icons/FindItem.gif" align="absMiddle"></asp:LinkButton>
                                                        <input type="hidden" id="hidNewPackageID" runat="server" />
                                                    </FooterTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Package Product Description">
                                                    <ItemTemplate>
                                                        <%# Eval("ProductDescription")%>
                                                        <asp:LinkButton ID="lnkEditPackageDetail" runat="server" EnableViewState="true" CssClass="EditButt"
                                                            OnClientClick='<%# "return EditPackageDescription(" + Eval("SNNo").ToString() + ");" %>'
                                                            Visible='<%#(lblQuotationNo.Text != "" && this.lblStatus.Text.ToUpper() != "UNSUCCESSFUL")%>'>Details</asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditProductDescription" runat="server" Columns="30" MaxLength="80"
                                                            CssClass="textbox" Text='<%# Eval("ProductDescription") %>' />
                                                        <asp:RequiredFieldValidator ID="rfvEditProductDescription" runat="server" ControlToValidate="txtEditProductDescription"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditPackageRow" />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewProductDescription" runat="server" Columns="30" MaxLength="80"
                                                            CssClass="textbox" />
                                                        <asp:RequiredFieldValidator ID="rfvNewProductDescription" runat="server" ControlToValidate="txtNewProductDescription"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Package" />
                                                    </FooterTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="UOM" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <%# Eval("UOM")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlEditUOM">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlNewUOM">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Qty">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("Qty", "{0:f2}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditQuantity" runat="server" Columns="5" MaxLength="10"
                                                            Text='<%# Eval("Qty", "{0:f2}") %>' />
                                                        <asp:RequiredFieldValidator ID="rfvEditQuantity" runat="server" ControlToValidate="txtEditQuantity"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditPackageRow" /><asp:RangeValidator
                                                                ID="rgEditQuantity" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditQuantity"
                                                                Type="Double" ValidationGroup="valGrpEditPackageRow" /></EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewQuantity" runat="server" Columns="5" MaxLength="10" />
                                                        <asp:RequiredFieldValidator ID="rfvNewQuantity" runat="server" ControlToValidate="txtNewQuantity"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Package" /><asp:RangeValidator
                                                                ID="rgNewQuantity" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewQuantity"
                                                                Type="Double" ValidationGroup="Package" />
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Unit Package Price" ItemStyle-ForeColor="green">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblUnitPackagePrice" Text='<%# Eval("UnitPackagePrice", "{0:f2}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="GP%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblGP"></asp:Label>
                                                        <input type="hidden" id="hidUnitPackageCost" runat="server" value='<%# Eval("UnitPackageCost")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Detail">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEditDetail" runat="server" EnableViewState="true" OnClientClick='<%# "return viewDetail("+Eval("SNNo") + ");"%>'
                                                            Visible='<%#(lblQuotationNo.Text != "" && this.lblStatus.Text.ToUpper() != "UNSUCCESSFUL")%>'>Details</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Order" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%# Eval("DSNNo").ToString() + Eval("DOptSNNo").ToString()%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox CssClass="textbox" ID="txtEditDSNNo" runat="server" Columns="2" MaxLength="3"
                                                            Text='<%# Eval("DSNNo") %>'  Height="10px"/>
                                                        <asp:RequiredFieldValidator ID="rfvEditDSNNo" runat="server" ControlToValidate="txtEditDSNNo"
                                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                                        <asp:CompareValidator ID="cvEditDSNNo" runat="server" ErrorMessage="Not an integer"
                                                            Display="Dynamic" ControlToValidate="txtEditDSNNo" Type="Integer" Operator="DataTypeCheck"
                                                            ValidationGroup="valGrpEditRow" />
                                                        <asp:DropDownList runat="server" ID="ddlEditDOptSNNo" >
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                                            CausesValidation="false" CssClass="DeleteButt" ><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                                            ValidationGroup="valGrpEditPackageRow" CssClass="SaveButt" ><span>Save</span></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                                            CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                                            ValidationGroup="Package" CssClass="NewButt"  ><span>Add</span></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                
           
                    <asp:UpdatePanel ID="updatePanel3" runat="server" Visible="false">
                        <ContentTemplate>
                            <table class="tTable1" cellspacing="5">
                                <tr>
                                    <td style="vertical-align: top;">
                                        <asp:DataGrid ID="dgTNC" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            OnItemCommand="dgTNC_CreateCommand" OnDeleteCommand="dgTNC_DeleteCommand" GridLines="none"
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true"
                                            Visible="true">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgTNC.CurrentPageIndex) * dgTNC.PageSize)  %>'>
                                            
                                                        </asp:Label>
                                                        .</ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Name" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval( "Name" )%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNewName" runat="server" Columns="50" MaxLength="200" CssClass="textbox" />
                                                        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2" ID="autoComplete1"
                                                            TargetControlID="txtNewName" ServicePath="AutoCompletionProduct.asmx" ServiceMethod="GetTNCListByName"
                                                            MinimumPrefixLength="1" CompletionInterval="100" EnableCaching="false" CompletionSetCount="50"
                                                            DelimiterCharacters=",">
                                                        </ajaxToolkit:AutoCompleteExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Order">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkGoUp" runat="server" CommandName="GoUp" EnableViewState="true"><IMG height="16" src="../../images/icons/UpArrow.png" align="absMiddle"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkGoDown" runat="server" CommandName="GoDown" EnableViewState="true"><IMG height="16" src="../../images/icons/DownArrow.png" align="absMiddle"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                    <HeaderStyle Wrap="False" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="true"
                                                            CausesValidation="false" CssClass="DeleteButt"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="true"
                                                            CssClass="NewButt" ><span>Add</span></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                            <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                        </asp:DataGrid>
                                    </td>
                                    <td>
                                        <asp:DataGrid ID="dgTNC1" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                            OnItemCommand="dgTNC_CreateCommand" OnDeleteCommand="dgTNC_DeleteCommand" GridLines="none"
                                            CellPadding="5" CellSpacing="5" CssClass="tInfoTable" EnableViewState="true"
                                            Visible="true">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="List Of Terms and Conditions" HeaderStyle-Wrap="false"
                                                    HeaderStyle-Font-Italic="true" ItemStyle-Font-Italic="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval( "Name" )%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tInfoHeader" />
                                            <AlternatingItemStyle CssClass="tInfoAltRow" />
                                            <FooterStyle CssClass="tInfoHeader" />
                                            <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                
        </div>
    </div>
    
    <br />
    <asp:UpdatePanel ID="updatePanel6" runat="server">
        <ContentTemplate>
            <table width="99%" style="margin-left: 8px" cellspacing="5" cellpadding="5">
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblCreatedBy"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblModifiedBy"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DataGrid ID="dgApproval" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                                        ShowFooter="false" GridLines="none" CellPadding="5" CellSpacing="5" OnItemCommand="dgApproval_ItemCommand"
                                        OnItemDataBound="dgApproval_ItemDataBound">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgApproval.CurrentPageIndex) * dgApproval.PageSize)%>
                                                    .</ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Approval/Reject">
                                                <HeaderStyle></HeaderStyle>
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container, "DataItem.Description")%>
                                                    <input type="hidden" id="hidApprovedUserID" runat="server" value='<%# Eval("ApprovedUserID")%>' />
                                                    <input type="hidden" id="hidApprovedAlternateUserID" runat="server" value='<%# Eval("ApprovedAlternateUserID")%>' />
                                                    <input type="hidden" id="hidApprovalStatus" runat="server" value='<%# Eval("ApprovalStatus")%>' />
                                                    <input type="hidden" id="hidApprovalType" runat="server" value='<%# Eval("ApprovalType")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                <ItemTemplate>
                                                    <asp:Button ID="lnkApprove" runat="server" CommandName="Approve" EnableViewState="true"
                                                        CausesValidation="false" Text="Approve" CssClass="button"></asp:Button>
                                                    <asp:Button ID="lnkReject" runat="server" CommandName="Reject" EnableViewState="true"
                                                        CausesValidation="false" Text="Reject" CssClass="button"></asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                        <AlternatingItemStyle CssClass="tAltRow" />
                                        <FooterStyle CssClass="tFooter" />
                                    </asp:DataGrid></td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table>
                            <tr>
                                <td align="right" style="font-weight: bold; width: 84%">
                                    Sub-Total :
                                </td>
                                <td align="center" style="font-weight: bold; width: 6%">
                                    <asp:Label runat="server" ID="lblCurrency" Text="SGD" /></td>
                                <td align="right" style="width: 10%">
                                    <asp:Label runat="server" ID="lblSubTotal" /></td>
                            </tr>
                            <tr>
                                <td align="right" style="font-weight: bold; width: 84%">
                                    GST
                                    <asp:DropDownList runat="server" ID="ddlTaxType" CssClass="dropdownlist" OnSelectedIndexChanged="ddlTaxType_SelectedIndexChanged"
                                        AutoPostBack="true" /><asp:TextBox runat="server" ID="txtTaxRate" Columns="6" CssClass="textbox"
                                            contentEditable="false" />
                                    :
                                </td>
                                <td align="center" style="font-weight: bold; width: 6%">
                                    <asp:Label runat="server" ID="lblCurrency2" Text="SGD" /></td>
                                <td align="right" style="width: 10%">
                                    <asp:Label runat="server" ID="lblTaxAmount" /></td>
                            </tr>
                            <tr>
                                <td align="right" style="font-weight: bold; width: 84%">
                                    Grand Total :
                                </td>
                                <td align="center" style="font-weight: bold; width: 6%">
                                    <asp:Label runat="server" ID="lblCurrency3" Text="SGD" /></td>
                                <td align="right" style="width: 10%">
                                    <asp:Label runat="server" ID="lblGrandTotal" /></td>
                            </tr>
                            <tr>
                                <td align="right" style="font-weight: bold; width: 84%">
                                    GP% :
                                </td>
                                <td align="center" style="font-weight: bold; width: 6%">
                                </td>
                                <td align="right" style="width: 10%">
                                    <asp:Label runat="server" ID="lblGPercentage" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    <table style="text-align: right; width: 100%" cellspacing="5" cellpadding="5">
        <tr>
            <td>
                <asp:Button runat="server" ID="btnApplyDiscount" Text="Apply Discount" CssClass="button" />
                <asp:Button runat="server" ID="btnDuplicate" Text="Duplicate" CssClass="button" OnClientClick="return confirm('A new quotation will be created. Continue?')"
                    OnClick="btnDuplicate_Click" />
                <asp:Button runat="server" ID="btnAddToRevision" Text="Add To Revision" CssClass="button"
                    OnClientClick="return confirm('Are you sure you want to add a new revision?')"
                    OnClick="btnAddToRevision_Click" />
                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="button" OnClick="btnSubmit_Click" />
                <asp:Button runat="server" ID="btnSubmit" Text="Submit For Approval" CssClass="button" OnClick="btnSubmit_Click" OnClientClick="btnActionClick()" />
                <asp:Button runat="server" ID="btnDeleteRevision" Text="Delete Revision" CssClass="button" OnClick="btnDeleteRevision_Click" Visible="false" OnClientClick="btnActionClick()" />
                
            </td>
        </tr>
    </table>
    <asp:UpdatePanel runat="server" id="upReportLinks" updatemode="Conditional" >      
    <ContentTemplate>    
    <table style="text-align: right; width: 100%" cellspacing="5" cellpadding="5">
        <tr>
            <td>
                <asp:DropDownList ID="ddlReport" runat="server" CssClass="dropdownlist">
                </asp:DropDownList>
                <asp:LinkButton ID="lnkPrintReport" OnClick="GenerateReport" runat="server" Text="Print"
                    CssClass="button" ToolTip="Please click to print report." CausesValidation="False"><img id="img4" alt="" src="../../images/icons/printIcon.gif" align="top" border="0" /></asp:LinkButton>
                <asp:LinkButton ID="lnkPrintPDF" OnClick="GeneratePDFReport" runat="server" Text="Print PDF"
                    CssClass="button" ToolTip="Please click to print PDF report." CausesValidation="False"><img id="img1" alt="" src="../../images/icons/pdf.png" align="top" border="0" /></asp:LinkButton>                    
                <asp:LinkButton ID="lnkEmailPDF" OnClick="GenerateEmailPDFReport" runat="server" Text="Email PDF"
                    CssClass="button" ToolTip="Please click to email PDF report." CausesValidation="False" OnClientClick="btnActionClick()"><img id="img3" alt="" src="../../images/icons/mail.png" align="top" border="0" /></asp:LinkButton>  
            </td>
        </tr>
    </table> 
    
    <asp:Button runat="server" ID="HiddenForModalEmail" style="display: none" />    
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="HiddenForModalEmail" PopupControlID="PNL2" BackgroundCssClass="modalBackground" CancelControlID="btnCancel" BehaviorID="btnEmailPopupBehavior" />
    <asp:Panel ID="PNL2" runat="server" style="display:none; width:750px; background-color:White; border-width:2px; border-color:#005DAA; border-style: solid; padding: 10px 20px;" >
    <h1>Email Quotation</h1>    
    <table align="center" style="margin-left: auto; margin-right: auto; margin-top: 0px; margin-bottom: 0px;" >
       
        <tr>
            <td class="tbLabel">
               <img id="img5" alt="" src="../../images/icons/addres16.png" align="top" border="0" />
               To</td>
            <td style="width: 2%">
            :</td>
            <td>                
                <asp:TextBox runat="server" ID="txtTo" Columns="70" onfocus="select();" CssClass="textbox"></asp:TextBox> 
                <asp:RequiredFieldValidator ID="rfvEditEmail" runat="server" ControlToValidate="txtTo"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="eq" />
				<asp:RegularExpressionValidator ID="regexpName" runat="server"     
                                    ErrorMessage="Invalid Format" 
                                    ControlToValidate="txtTo"     
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*" ValidationGroup="eq"/>						    
										    
										      
                
            </td>
        </tr> 
        
        <tr>
        <td></td>
        <td></td>
        <td><span style="color:Green; size:7px; font-style:italic;">e.g. james@leedenlimited.com; wiliam@leedenlimited.com</span></td>
        </tr>
        
        
        
        <tr>
            <td class="tbLabel">
               <img id="img7" alt="" src="../../images/icons/addres16.png" align="top" border="0" />
               Cc</td>
            <td style="width: 2%">
            :</td>
            <td>                
                <asp:TextBox runat="server" ID="txtCC" Columns="70" onfocus="select();" CssClass="textbox"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"     
                                    ErrorMessage="Invalid Format" 
                                    ControlToValidate="txtCC"     
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*" ValidationGroup="eq"/> 
                    
         
            </td>
        </tr>  
        
        
        
        <tr>
            <td class="tbLabel">
               <img id="img8" alt="" src="../../images/icons/addres16.png" align="top" border="0" />
               Bcc</td>
            <td style="width: 2%">
            :</td>
            <td>                
                <asp:TextBox runat="server" ID="txtBCC" Columns="70" onfocus="select();" CssClass="textbox"></asp:TextBox> 
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"     
                                    ErrorMessage="Invalid Format" 
                                    ControlToValidate="txtBCC"     
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*" ValidationGroup="eq" />  
         
            </td>
        </tr>  
        
        
        
        <tr>
            <td class="tbLabel">
                        Subject</td>
            <td style="width: 2%">
            :</td>
            <td>
                <asp:TextBox runat="server" ID="txtEmailSubject" Columns="70" onfocus="select();" CssClass="textbox"></asp:TextBox> 
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmailSubject"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="eq" /> 
            </td>
            
        </tr>    
        
        <tr>
            <td class="tbLabel">
             <img id="img6" alt="" src="../../images/icons/attachment16.png" align="top" border="0" />           Attachment</td>
            <td style="width: 2%">
            :</td>
            <td valign="bottom">                
                <asp:LinkButton ID="lnkView" OnClick="DownloadPDFReport" runat="server" CssClass="button" ToolTip="Download PDF report." CausesValidation="False"> <asp:Label ID="lblFileName" runat="server" Text=""></asp:Label></asp:LinkButton>
                
            </td>
        </tr>
        
        
        <tr align="center">             
        <td colspan = "3"> 
        <br />     
        <FTB:FreeTextBox ID="FTBContent"       
        SupportFolder="/GMS/FreeTextBox/" 
	    JavaScriptLocation="ExternalFile"	    
	    ButtonImagesLocation="ExternalFile"
        runat="server" Text=""
        toolbarlayout="ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu,FontForeColorPicker,FontBackColorsMenu,FontBackColorPicker|Bold,Italic,Underline,Strikethrough|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent"
        Width="750px" Height="250px"
        EnableHtmlMode="false" />
                                                        
        </td>
        </tr>
    </table>
    <br />
        <div style="text-align: center;">
            <asp:Button CssClass="button" ID="btnSend" runat="server" Text="Send" OnCommand="SendEmail" ValidationGroup="eq" OnClientClick="btnEmailClick()" />
            <asp:Button CssClass="button" ID="btnCancel" runat="server" Text="Cancel" />
        </div>
    
        
    </asp:Panel>
    </ContentTemplate>
    <Triggers>
             <asp:AsyncPostBackTrigger controlid="lnkPrintReport" EventName="Click" />
             <asp:AsyncPostBackTrigger controlid="lnkPrintPDF" EventName="Click" />
             <asp:AsyncPostBackTrigger controlid="lnkEmailPDF" EventName="Click" /> 
             <asp:AsyncPostBackTrigger controlid="btnSend" EventName="Click" />   
             <asp:PostBackTrigger ControlID="lnkView"  />

          
    </Triggers> 
    </asp:UpdatePanel>
    
    
    
    <asp:Panel ID="Panel1" runat="server">
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnApplyDiscount"
        PopupControlID="pnlDiscount" BackgroundCssClass="modalBackground" CancelControlID="ButtonCancel" />
    <asp:Panel ID="pnlDiscount" runat="server" Style="display: none; width: 300px; background-color: White;
        border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
        <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0"
                width="99%">
                <tr>
                    <td class="tbLabel">
                        Discount</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox CssClass="textbox" ID="txtDiscountRate" runat="server" Columns="5" MaxLength="10" />
            <asp:RequiredFieldValidator ID="rfvDiscountRate" runat="server" ControlToValidate="txtDiscountRate"
                ErrorMessage="*" Display="dynamic" ValidationGroup="Discount" /><asp:RangeValidator
                    ID="rgDiscountRate" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtDiscountRate"
                    Type="Double" ValidationGroup="Discount" />
                    </td>
                    <td style="width: 5%">
                        by</td>
                    <td>
                        <asp:RadioButtonList ID="rblDiscountType" runat="server" RepeatColumns="1" RepeatLayout="Flow"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" Selected="True">Amount</asp:ListItem>
                                            <asp:ListItem Value="1">Percentage</asp:ListItem>
                                        </asp:RadioButtonList></td>
                </tr>
                </table>
                <br />
        <div style="text-align: center;">
            <asp:Button CssClass="button" ID="ButtonOk" runat="server" Text="Apply" OnCommand="ApplyDiscount" 
                ValidationGroup="Discount" />
            <asp:Button CssClass="button" ID="ButtonCancel" runat="server" Text="Cancel" />
        </div>
    </asp:Panel>
    
    <asp:Button runat="server" ID="HiddenForModalSave" style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenForModalSave" PopupControlID="PNL" BackgroundCssClass="modalBackground" BehaviorID="btnOkPopupBehavior" />
    <asp:Panel ID="PNL" runat="server" style="display:none; width:100px; background-color:#A4A4A4; border-width:0px; padding:0px;">
    <table align="center" style="margin-left: auto; margin-right: auto; margin-top: 0px; margin-bottom: 0px;" >
        <tr align="center">             
        <td>
        <img src = "../../images/loading.gif" />                                                         
        </td>
        </tr>
    </table>
    
    </asp:Panel>
    
     </ContentTemplate>
    </asp:UpdatePanel>
    
    
 
    
</asp:Content>
