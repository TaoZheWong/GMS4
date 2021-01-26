<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
    Codebehind="AddEditQuotation.aspx.cs" Inherits="GMSWeb.Sales.Sales.AddEditQuotation"
    Title="Add/Edit Quotation" ValidateRequest="false" %>
<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

<script type="text/javascript">

// javascript to add to your aspx page

            function btnUploadAttachmentClick()
            {
                document.getElementById('<%= btnUpdateFileUploadAttachment.ClientID %>').click();
            }


			function CheckOutright(source)
			{
			    if(document.getElementById('ctl00_ContentPlaceHolderMain_ckhIsOutright').checked)
			    {
			        document.getElementById('ctl00_ContentPlaceHolderMain_chkIsCOP').checked = false;	
			    }
				
			}
			
			function CheckCOP(source)
			{
			    if(document.getElementById('ctl00_ContentPlaceHolderMain_chkIsCOP').checked)
			    {			    
			        document.getElementById('ctl00_ContentPlaceHolderMain_ckhIsOutright').checked = false;			      
			            
			    }
			   
			}
		</script>

    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">Quotation</a></li>
        <li class="active">Add/Edit Quotation</li>
    </ul>
    <h1 class="page-header">Add/Edit Quotation <br />
        <small>For salesman to manage a quotation.</small></h1>
  
    <asp:ScriptManager ID="sriptmgr1" runat="server">
        <Services>
            <asp:ServiceReference Path="AutoCompletionProduct.asmx" />
        </Services>
    </asp:ScriptManager>
    
    
    <!--Start Panel-->
    <div class="panel panel-primary">
        <div class="panel-heading">
            <h4 class="panel-title">
                <i class="ti-pencil"></i>  Add/Edit Quatation
            </h4>
        </div>
        <asp:UpdatePanel ID="updatePanel5" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="ddlRevisionNo" />
            </Triggers>
            <ContentTemplate>
                <div class="panel-body">
                        <div class="form-horizontal m-t-20">
                            <div class="row">
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        New Customer
                                    </label>
                                    <div class="col-sm-6">
                                        <div class="checkbox">
                                            <asp:CheckBox runat="server" ID="chkIsNewCustomer" Text="Yes" OnCheckedChanged="chkIsNewCustomer_OnCheckedChanged"
                                            AutoPostBack="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Status
                                    </label>
                                    <label class="col-sm-6 control-label text-left">
                                        <asp:Label runat="server" ID="lblStatus"></asp:Label>
                                    </label>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        New Customer Name
                                    </label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtNewCustomerName" MaxLength="50" onfocus="select();" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Quotation No.
                                    </label>
                                    <label class="col-sm-6 control-label text-left">
                                        <asp:Label runat="server" ID="lblQuotationNo"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                            Account Code
                                    </label>
                                    <div class="col-sm-6">
                                        <div class="input-group">
                                            <asp:TextBox runat="server" ID="txtAccountCode" MaxLength="10" Columns="10" AutoPostBack="true"
                                                onfocus="select();" CssClass="form-control input-sm" OnTextChanged="txtAccountCode_OnTextChanged"></asp:TextBox>
                                            <asp:LinkButton ID="lnkFindAccount" runat="server" CommandName="FindAccount" EnableViewState="true"
                                                CssClass="input-group-addon " OnClientClick="return SearchAccount();"><i class="ti-search"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                    <input type="hidden" id="hidAccountCode" runat="server" />
                                    <input type="hidden" id="hidSalesPersonID" runat="server" />
                                    <input type="hidden" id="hidUserID" runat="server" />
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Quotation Date
                                    </label>
                                    <div class="col-sm-6">
                                        <div class="input-group date">
                                            <asp:TextBox runat="server" ID="txtQuotationDate" MaxLength="10" Columns="10" onfocus="select();"
                                                    CssClass="form-control datepicker"></asp:TextBox>
                                            <span class="input-group-addon">
                                                <i class="ti-calendar"></i>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Account Name
                                    </label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtAccountName" MaxLength="50" Columns="20" onfocus="select();"
                                            CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    SO Ref No.
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblSORefNo"></asp:Label>
                                </label>
                            </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Subject
                                    </label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtSubject" MaxLength="200" TextMode="MultiLine" onfocus="select();"
                                            onkeyup="update(this,'200');" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Salesman
                                    </label>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddlSalesman" runat="Server" DataTextField="SalesPersonName"
                                            DataValueField="SalesPersonID" CssClass="form-control input-sm" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Unsuccessful
                                    </label>
                                    <div class="col-sm-6">
                                        <div class="checkbox">
                                            <asp:CheckBox runat="server" ID="chkIsUnsuccessful" Text="Yes" OnCheckedChanged="chkIsUnsuccessful_OnCheckedChanged"
                                                AutoPostBack="true" />
                                        </div>
                                        <asp:LinkButton runat="server" ID="lnkCancelUnsuccessful" Text="Cancel Unsuccessful"
                                            Visible="false" EnableViewState="true" OnClick="lnkCancelUnsuccessful_Click" />
                                
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                 <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Revision
                                    </label>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddlRevisionNo" runat="Server" DataTextField="Revision" DataValueField="RevisionNo"
                                            CssClass="form-control input-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlRevisionNo_SelectedIndexChanged" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Order Lost Reason
                                    </label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtOrderLostReason" TextMode="MultiLine" onfocus="select();" EnableViewState="true" onkeyup="update(this,'100');" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Acknowledge
                                    </label>
                                    <div class="col-sm-6">
                                        <div class="checkbox">
                                            <asp:CheckBox runat="server" ID="chkAcknowledge" Text="Yes" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Outright
                                </label>
                                <div class="col-sm-6">
                                    <div class="checkbox">
                                        <asp:CheckBox runat="server" ID="ckhIsOutright" Text="Yes" onclick="CheckOutright(this);" />
                                    </div>
                                </div>
                            </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        COP
                                    </label>
                                    <div class="col-sm-6">
                                        <div class="checkbox">
                                            <asp:CheckBox runat="server" ID="chkIsCOP" Text="Yes" onclick="CheckCOP(this);" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        PO No.
                                    </label>
                                    <div class="col-sm-6">
                                            <asp:TextBox runat="server" ID="txtPONo" MaxLength="15" onfocus="select();" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Convert Lab File
                                </label>
                                <div class="col-sm-6">
                                    <div class="checkbox">
                                        <asp:CheckBox runat="server" ID="chkConvertLabFile" Text="Yes" />
                                    </div>
                                </div>
                            </div>
                            </div>
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    
        <asp:UpdatePanel ID="upMsg" runat="server">
            <ContentTemplate>
                <span style="color:Red; size:7px; font-style:italic;"><asp:Label ID="lblFail" runat="server" Text=""></asp:Label></span><br /> 
                <span style="color:Green; size:7px; font-style:italic;"><asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label></span> 
        </ContentTemplate>
        </asp:UpdatePanel>

        <div class="panel-tab">
            <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Width="100%"
        CssClass="ajax__tab_xp ajax_tab_custom">
        <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Information">
            <ContentTemplate>
                <asp:UpdatePanel ID="updatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="form-horizontal m-t-20">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">
                                            Delivery Address
                                        </label>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" ID="txtAddress1" MaxLength="40" Columns="20" onfocus="select();"
                                                        CssClass="form-control input-sm" TabIndex="1"></asp:TextBox>
                                                         <asp:LinkButton ID="lnkAddress" runat="server" EnableViewState="true"
                                                            CssClass="input-group-addon" OnClientClick="return SearchAddress();"><i class="ti-search"></i></asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox runat="server" ID="txtAddress2" MaxLength="40" Columns="20" onfocus="select();"
                                                    CssClass="form-control input-sm" TabIndex="2"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox runat="server" ID="txtAddress3" MaxLength="40" Columns="20" onfocus="select();"
                                                    CssClass="form-control input-sm" TabIndex="3"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox runat="server" ID="txtAddress4" MaxLength="40" Columns="20" onfocus="select();"
                                                    CssClass="form-control input-sm" TabIndex="4"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox runat="server" ID="txtAddressCode" MaxLength="40" Columns="20" onfocus="select();"
                                                    CssClass="form-control input-sm" TabIndex="4"></asp:TextBox><input type="hidden" id="hidAddressID" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">
                                            Attn To
                                        </label>
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" ID="txtAttentionTo" MaxLength="40" Columns="20" onfocus="select();"
                                                CssClass="form-control input-sm" TabIndex="7"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">
                                            Delivery Mode
                                        </label>
                                        <div class="col-sm-6">
                                            <div class="radio">
                                                <asp:RadioButton ID="rbDelivery" runat="server" Text="Delivery" GroupName="Console" Checked />
                                                <asp:RadioButton ID="rbSelfCollect" runat="server" Text="Self-Collect" GroupName="Console" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">
                                            Required Date
                                        </label>
                                        <div class="col-sm-6">
                                            <div class="input-group date">
                                                <asp:TextBox runat="server" ID="txtRequiredDate" MaxLength="10" Columns="10" onfocus="select();"
                                                    CssClass="form-control datepicker"></asp:TextBox>
                                                <span class="input-group-addon"><i class="ti-calendar"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">
                                            Office Phone
                                        </label>
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" ID="txtOfficePhone" MaxLength="20" Columns="20" onfocus="select();"
                                                CssClass="form-control input-sm" TabIndex="9"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Mobile Phone
                                    </label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtMobilePhone" MaxLength="20" Columns="20" onfocus="select();"
                                            CssClass="form-control input-sm" TabIndex="8"></asp:TextBox>
                                    </div>
                                </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">
                                            Contact Person
                                        </label>
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" ID="txtContactPerson" MaxLength="40" Columns="20" onfocus="select();"
                                                CssClass="form-control input-sm" TabIndex="7"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">
                                            Fax
                                        </label>
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" ID="txtFax" MaxLength="20" Columns="20" onfocus="select();"
                                                CssClass="form-control input-sm" TabIndex="10"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">
                                            Ordered By
                                        </label>
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" ID="txtOrderedBy" MaxLength="40" Columns="20" onfocus="select();"
                                                CssClass="form-control input-sm" TabIndex="11"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Email
                                    </label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtEmail" MaxLength="40" Columns="20" onfocus="select();"
                                            CssClass="form-control input-sm" TabIndex="11"></asp:TextBox>
                                    </div>
                                </div>
                                </div>    
                                <div class="row">
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">
                                            Customer PO No.
                                        </label>
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" ID="txtCustomerPONo" MaxLength="15" Columns="20" onfocus="select();"
                                                CssClass="form-control input-sm" TabIndex="5"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">
                                            Currency
                                        </label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList runat="server" ID="ddlCurrency" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                                                AutoPostBack="true" TabIndex="12" CssClass="form-control input-sm">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <%-- <asp:TextBox runat="server" ID="txtCurrencyRate" MaxLength="20"
                                            Columns="10" onfocus="select();" CssClass="form-control input-sm" TabIndex="13" /> --%>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">
                                            Customer SO No.
                                        </label>
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" ID="txtCustomerSONo" MaxLength="15" Columns="20" onfocus="select();"
                                                CssClass="form-control input-sm" TabIndex="5"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                    <label class="col-sm-6 control-label text-left">
                                        Transport Zone
                                    </label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtTransportZone" MaxLength="40" Columns="20" onfocus="select();"
                                            CssClass="form-control input-sm" TabIndex="11"></asp:TextBox>
                                    </div>
                                </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-12 col-sm-12">
                                        <label class="col-sm-3 control-label text-left">
                                            Internal Remarks
                                        </label>
                                        <div class="col-sm-9">
                                            <asp:TextBox runat="server" ID="txtInternalRemarks" CssClass="form-control input-sm" TextMode="MultiLine"
                                                onfocus="select();" TabIndex="6"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-3 control-label text-left">
                                        External Remarks
                                    </label>
                                    <div class="col-sm-9">
                                        <asp:TextBox runat="server" ID="txtExternalRemarks" CssClass="form-control input-sm" TextMode="MultiLine"
                                            onfocus="select();" TabIndex="6"></asp:TextBox>
                                    </div>
                                </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Product Item">
            <ContentTemplate>
                <asp:UpdatePanel ID="updatePanel2" runat="server">                        
                    <ContentTemplate>
                        <div class="container-fluid m-t-20">
                             <asp:Button runat="server" ID="btnHideMSP" Text="Hide MSP" CssClass="btn btn-default btn-sm" OnClick="btnHideMSP_Click" />
                        </div>
                        <input type="hidden" id="hidDetailsCount" runat="server" />
                        <div class="table-responsive">
                            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover m-t-20 m-b-20" EnableViewState="true"
                                OnItemCommand="dgData_CreateCommand" OnDeleteCommand="dgData_DeleteCommand" OnEditCommand="dgData_EditCommand"
                                OnCancelCommand="dgData_CancelCommand" OnUpdateCommand="dgData_UpdateCommand"
                                OnItemDataBound="dgData_ItemDataBound" GridLines="none" >
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>'>
                                            </asp:Label>                                                        
                                         </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Product Code">
                                        <ItemTemplate>
                                            <input type='hidden' value='<%# Eval("CoyID").ToString() + ";" + Eval("ProductCode").ToString() +";" + hidUserID.Value %>' />
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true" CausesValidation="false" title='<%# "Stock Status For "+Eval("ProductCode")%>'>
                                                <span><%# Eval("ProductCode")%>
                                                    <a data-toggle='popover' data-trigger='hover' title='<%# "Stock Status For "+Eval("ProductCode")%>' data-content='' data-html='true' onMouseOver="getProductDetails(this)">
                                                        <span class="glyphicon glyphicon-list-alt"></span>
                                                    </a>
                                                </span>
                                            </asp:LinkButton>
                                            <asp:Image ID="imgMagnify" runat="server" ImageUrl="../../images/icons/box_closed.png" style="display:none"/>
                                                        <ajaxToolkit:PopupControlExtender ID="PopupControlExtender1" runat="server" PopupControlID="Panel1"
                                                            TargetControlID="imgMagnify" DynamicContextKey='<%# Eval("CoyID").ToString() + ";" + Eval("ProductCode").ToString() +";" + hidUserID.Value %>'
                                                            DynamicControlID="Panel1" DynamicServiceMethod="GetDynamicContent" Position="Right">
                                                        </ajaxToolkit:PopupControlExtender>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtNewProductCode" runat="server" Columns="12" MaxLength="15" CssClass="form-control input-sm"
                                                    onfocus="select();" onchange="this.value = this.value.toUpperCase()" OnTextChanged="txtNewProductCode_OnTextChanged"
                                                    AutoPostBack="true" />
                                                <asp:LinkButton ID="lnkFindProduct" runat="server" CommandName="FindProduct" EnableViewState="true"
                                                    CssClass="input-group-addon" OnClientClick="return SearchProduct(this);"><i class="ti-search"></i></asp:LinkButton>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvNewProductCode" runat="server" ControlToValidate="txtNewProductCode"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="Product" />
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
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtEditProductDescription" runat="server" Columns="30"
                                                MaxLength="80" Text='<%# Eval("ProductDescription") %>' />
                                            <asp:RequiredFieldValidator ID="rfvEditProductDescription" runat="server" ControlToValidate="txtEditProductDescription"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewProductDescription" runat="server" Columns="30" MaxLength="80"
                                                CssClass="form-control input-sm" onfocus="select();" onchange="this.value = this.value.toUpperCase()" AutoPostBack="true" OnTextChanged="txtNewProductDescription_OnTextChanged" />
                                            <asp:RequiredFieldValidator ID="rfvNewProductDescription" runat="server" ControlToValidate="txtNewProductDescription"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="Product" />
                                                        
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Recipe No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecipeNo" runat="server" Text='<%# Eval("RecipeNo")%>'></asp:Label>                                                       
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtEditRecipeNo" runat="server" Columns="12"
                                                MaxLength="12" Text='<%# Eval("RecipeNo") %>' OnTextChanged="txtEditRecipeNo_OnTextChanged" AutoPostBack="true" />
                                            </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewRecipeNo" runat="server" Columns="12" MaxLength="12"
                                                CssClass="form-control input-sm" onfocus="select();" onchange="this.value = this.value.toUpperCase()" OnTextChanged="txtNewRecipeNo_OnTextChanged" AutoPostBack="true" />                                                        
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Tag No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTagNo" runat="server" Text='<%# Eval("TagNo")%>'></asp:Label>                                                       
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtEditTagNo" runat="server" Columns="12"
                                                MaxLength="20" Text='<%# Eval("TagNo") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewTagNo" runat="server" Columns="12" MaxLength="20"
                                                CssClass="form-control input-sm" onfocus="select();" onchange="this.value = this.value.toUpperCase()" />                                                        
                                        </FooterTemplate>
                                    </asp:TemplateColumn>  
                                    <asp:TemplateColumn HeaderText="Batch Size">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBatchNo" runat="server" Text='<%# Eval("BatchSize")%>'></asp:Label>                                                       
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtEditBatchNo" runat="server" Columns="5"
                                                MaxLength="12" Text='<%# Eval("BatchSize") %>' />
                                            </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewBatchNo" runat="server" Columns="5" MaxLength="9" CssClass="form-control input-sm" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>    
                                    <asp:TemplateColumn HeaderText="UOM" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <%# Eval("UOM")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlEditUOM" CssClass="form-control input-sm">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList runat="server" ID="ddlNewUOM" CssClass="form-control input-sm">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Qty">
                                        <ItemTemplate>
                                            <%# Eval("Quantity", "{0:f2}")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtEditQuantity" runat="server" Columns="5" Text='<%# Eval("Quantity", "{0:f2}") %>' />
                                            <asp:RequiredFieldValidator ID="rfvEditQuantity" runat="server" ControlToValidate="txtEditQuantity"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                                    ID="rgEditQuantity" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditQuantity"
                                                    Type="Double" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtNewQuantity" runat="server" Columns="5" OnTextChanged="Price_OnTextChanged"
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
                                            <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Eval("UnitPrice", "{0:f4}")%>'></asp:Label>                                                        
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control input-sm m-w-90" ID="txtEditUnitPrice" runat="server" Columns="5"
                                                MaxLength="10" Text='<%# Eval("UnitPrice", "{0:f4}") %>' />
                                            <asp:RequiredFieldValidator ID="rfvEditUnitPrice" runat="server" ControlToValidate="txtEditUnitPrice"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                                    ID="rgEditUnitPrice" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditUnitPrice"
                                                    Type="Double" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                                       
                                        <FooterTemplate>
                                            <asp:TextBox CssClass="form-control input-sm m-w-90" ID="txtNewUnitPrice" runat="server" Columns="5" MaxLength="10"
                                                OnTextChanged="Price_OnTextChanged" AutoPostBack="true" />
                                            <asp:RequiredFieldValidator ID="rfvNewUnitPrice" runat="server" ControlToValidate="txtNewUnitPrice"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="Product" /><asp:RangeValidator
                                                    ID="rgNewUnitPrice" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewUnitPrice"
                                                    Type="Double" ValidationGroup="Product" />
                                                    <input type="hidden" id="hidWeightedCost" runat="server" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="GP%">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblGP" Text='<%# Eval("GPPercentage")%>'></asp:Label>
                                            <input type="hidden" id="hidTotalCost" runat="server" value='<%# Eval("TotalCost", "{0:f2}")%>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblGPPercentage" />
                                                        
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
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtEditDSNNo" runat="server" Columns="2" MaxLength="3"
                                                Text='<%# Eval("DSNNo") %>'/>
                                            <asp:RequiredFieldValidator ID="rfvEditDSNNo" runat="server" ControlToValidate="txtEditDSNNo"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                            <asp:CompareValidator ID="cvEditDSNNo" runat="server" ErrorMessage="Not an integer"
                                                Display="Dynamic" ControlToValidate="txtEditDSNNo" Type="Integer" Operator="DataTypeCheck"
                                                ValidationGroup="valGrpEditRow" />
                                            <asp:DropDownList runat="server" ID="ddlEditDOptSNNo" CssClass="form-control input-sm">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="true"
                                                CausesValidation="false" OnClientClick="btnActionClick()" 
                                                CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="true"
                                                ValidationGroup="valGrpEditRow"  OnClientClick="btnActionClick()"
                                                CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="true"
                                                CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="true"
                                                ValidationGroup="Product"  OnClientClick="btnAddProductClick(this)"
                                                 CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle CssClass="tHeader" />
                                <AlternatingItemStyle CssClass="tAltRow" />
                                <FooterStyle CssClass="tFooter" />
                            </asp:DataGrid>
                        </div>
                    </ContentTemplate> 
                       
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="TabPanel4" HeaderText="Package Item">
            <ContentTemplate>
                <asp:UpdatePanel ID="updatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DataGrid ID="dgPackage" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                            GridLines="None" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover m-t-20" OnItemCommand="dgPackage_CreateCommand"
                            OnEditCommand="dgPackage_EditCommand" OnDeleteCommand="dgPackage_DeleteCommand"
                            OnItemDataBound="dgPackage_ItemDataBound" OnCancelCommand="dgPackage_CancelCommand"
                            OnUpdateCommand="dgPackage_UpdateCommand">
                            <Columns>
                                <asp:TemplateColumn HeaderText="No" >
                                    <ItemTemplate>
                                        <%# (Container.ItemIndex + 1) + ((dgPackage.CurrentPageIndex) * dgPackage.PageSize)%>
                                        <input type="hidden" id="hidSNNo" runat="server" value='<%# Eval("SNNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Product Code">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
                                            CausesValidation="false"><span><%# Eval("ProductCode")%></span></asp:LinkButton>
                                        <asp:Image ID="imgMagnify2" runat="server" ImageUrl="../../images/icons/box_closed.png" />
                                        <ajaxToolkit:PopupControlExtender ID="PopupControlExtender2" runat="server" PopupControlID="Panel1"
                                            TargetControlID="imgMagnify2" DynamicContextKey='<%# Eval("CoyID").ToString() + ";" + Eval("ProductCode").ToString() + ";" + hidUserID.Value %>'
                                            DynamicControlID="Panel1" DynamicServiceMethod="GetDynamicContent" Position="Right">
                                        </ajaxToolkit:PopupControlExtender>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div class="input-group">
                                                <asp:TextBox ID="txtNewPackageProductCode" runat="server" Columns="10" MaxLength="15"
                                                CssClass="form-control input-sm" onfocus="select();" onchange="this.value = this.value.toUpperCase()"
                                                OnTextChanged="txtNewPackageProductCode_OnTextChanged" AutoPostBack="true" />
                                            <span class="input-group-addon">
                                                    <asp:LinkButton ID="lnkFindPackage" runat="server" CommandName="FindPackage" EnableViewState="true"
                                                    CssClass="FindButt" OnClientClick="return SearchPackage(this);">
                                                        <i class="ti-search"></i>
                                                    </asp:LinkButton>
                                                    <input type="hidden" id="hidNewPackageID" runat="server" />
                                            </span>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvNewPackageProductCode" runat="server" ControlToValidate="txtNewPackageProductCode"
                                            ErrorMessage="*" Display="dynamic" ValidationGroup="Package" />
                                                   
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
                                            CssClass="form-control input-sm" Text='<%# Eval("ProductDescription") %>' />
                                        <asp:RequiredFieldValidator ID="rfvEditProductDescription" runat="server" ControlToValidate="txtEditProductDescription"
                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditPackageRow" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewProductDescription" runat="server" Columns="30" MaxLength="80"
                                            CssClass="form-control input-sm" />
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
                                        <asp:DropDownList runat="server" ID="ddlEditUOM" CssClass="form-control input-sm">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList runat="server" ID="ddlNewUOM" CssClass="form-control input-sm">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("Qty", "{0:f2}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox CssClass="form-control input-sm" ID="txtEditQuantity" runat="server" Columns="5" MaxLength="10"
                                            Text='<%# Eval("Qty", "{0:f2}") %>' />
                                        <asp:RequiredFieldValidator ID="rfvEditQuantity" runat="server" ControlToValidate="txtEditQuantity"
                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditPackageRow" /><asp:RangeValidator
                                                ID="rgEditQuantity" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditQuantity"
                                                Type="Double" ValidationGroup="valGrpEditPackageRow" /></EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox CssClass="form-control input-sm" ID="txtNewQuantity" runat="server" Columns="5" MaxLength="10" />
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
                                        <asp:TextBox CssClass="form-control input-sm" ID="txtEditDSNNo" runat="server" Columns="2" MaxLength="3"
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
                                            CausesValidation="false" OnClientClick="btnActionClick()"
                                            CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                            ValidationGroup="valGrpEditPackageRow" OnClientClick="btnActionClick()"
                                            CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                            CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                            ValidationGroup="Package" OnClientClick="btnAddPackageClick(this)" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                            <HeaderStyle CssClass="tHeader" />
                            <AlternatingItemStyle CssClass="tAltRow" />
                            <FooterStyle CssClass="tFooter" />
                        </asp:DataGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>            
        <ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Terms & Conditions">
            <ContentTemplate>
                <asp:UpdatePanel ID="updatePanel3" runat="server">
                    <ContentTemplate>
                        <div class="col-lg-6 col-md-6 no-padding">
                            <asp:DataGrid ID="dgTNC" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                            OnItemCommand="dgTNC_CreateCommand" OnDeleteCommand="dgTNC_DeleteCommand" GridLines="none"
                            CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" EnableViewState="true"
                            Visible="true">
                            <Columns>
                                <asp:TemplateColumn HeaderText="No">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgTNC.CurrentPageIndex) * dgTNC.PageSize)  %>'>
                                            
                                        </asp:Label>
                                     </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Name" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval( "Name" )%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewName" runat="server" Columns="50" MaxLength="200" CssClass="form-control input-sm" />
                                        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2" ID="autoComplete1"
                                            TargetControlID="txtNewName" ServicePath="AutoCompletionProduct.asmx" ServiceMethod="GetTNCListByName"
                                            MinimumPrefixLength="1" CompletionInterval="100" EnableCaching="false" CompletionSetCount="50"
                                            DelimiterCharacters=",">
                                        </ajaxToolkit:AutoCompleteExtender>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Order" ItemStyle-Width="80px" >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkGoUp" runat="server" CommandName="GoUp" EnableViewState="true" CssClass="btn btn-default btn-xs"><i class="ti-arrow-up"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkGoDown" runat="server" CommandName="GoDown" EnableViewState="true" CssClass="btn btn-default btn-xs"><i class="ti-arrow-down"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle />
                                    <HeaderStyle Wrap="False" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="true"
                                            CausesValidation="false"  CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="true"
                                            CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                            <HeaderStyle CssClass="tHeader" />
                            <AlternatingItemStyle CssClass="tAltRow" />
                            <FooterStyle CssClass="tFooter" />
                            <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                        </asp:DataGrid>
                        </div>
                        <div class="col-lg-6 col-md-6 well">
                            <asp:DataGrid ID="dgTNC1" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                OnItemCommand="dgTNC_CreateCommand" OnDeleteCommand="dgTNC_DeleteCommand" GridLines="none"
                                CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed" EnableViewState="true"
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
                                <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                            </asp:DataGrid>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>  
        <ajaxToolkit:TabPanel runat="server" ID="TabPanel5" HeaderText="Attachment">
            <ContentTemplate>
                <asp:UpdatePanel ID="upAttachment" runat="server">
                    <ContentTemplate>
                            <div id="Div8" style="text-align: left;" runat="server">
                                <asp:Label ID="lblAttachmentSummary" Visible="false" runat="server" />
                            </div>
                            <asp:DataGrid ID="dgAttachmentData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                        CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover m-t-20" EnableViewState="true"
                                        OnItemCommand="dgAttachmentData_Command" OnDeleteCommand="dgAttachmentData_DeleteCommand" 
                                        OnItemDataBound="dgAttachmentData_ItemDataBound" GridLines="none" >
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgAttachmentData.CurrentPageIndex) * dgAttachmentData.PageSize)  %>'>
                                                    </asp:Label>
                                                    <input type="hidden" id="hidFileID" runat="server" value='<%# Eval("FileID")%>' />                                                       
                                                 </ItemTemplate>
                                            </asp:TemplateColumn>                                                
                                            <asp:TemplateColumn HeaderText="File Name">
                                                <ItemTemplate>
                                                    <%# Eval("FileDisplayName")%>                                                       
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEditFileName" runat="server" Columns="80"
                                                        MaxLength="50" Text='<%# Eval("FileDisplayName") %>'  />
                                                    <asp:RequiredFieldValidator ID="rfvEditFileName" runat="server" ControlToValidate="txtEditFileName"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewFileName" runat="server" Columns="70" MaxLength="50" CssClass="form-control input-sm" />
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
                                                    <div class="input-group">
                                                        <input type="text" class="form-control input-sm" readonly>
                                                         <label class="input-group-btn">
                                                            <span class="btn btn-primary btn-sm btn-upload">
                                                                <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                                                <asp:FileUpload CssClass="form-control input-sm hidden" ID="FileUpload1" runat="server" onchange="btnUploadAttachmentClick()"  />
                                                            </span>
                                                        </label>
                                                    </div>                                                 
                                                <asp:Label ID="lblUpload" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>                         
                                            <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="true"
                                                        CausesValidation="false" OnClientClick="btnActionClick()" 
                                                        CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="true"
                                                        ValidationGroup="valGrpEditRow" OnClientClick="btnActionClick()"
                                                        CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="true"
                                                        CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                                                </EditItemTemplate>
                                                <FooterTemplate>                                                                                                           
                                                    <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="true"
                                                        ValidationGroup="valGroupAttachment" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkBlank" runat="server" EnableViewState="true" ValidationGroup="Blank" CommandName="Blank"></asp:LinkButton>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle CssClass="tHeader" />
                                        <AlternatingItemStyle CssClass="tAltRow" />
                                        <FooterStyle CssClass="tFooter" />
                                    </asp:DataGrid>                             
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel> 
        <ajaxToolkit:TabPanel runat="server" ID="TabPanel7" HeaderText="Special Conditions" Visible="false">
            <ContentTemplate>
                <asp:UpdatePanel ID="upSpecialCondition" runat="server">
                    <ContentTemplate>
                        <asp:DataGrid ID="dgSpecialCondition" runat="server" AutoGenerateColumns="false" ShowFooter="true" GridLines="none"
                                        CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover m-t-20" EnableViewState="true" Visible="true">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgSpecialCondition.CurrentPageIndex) * dgSpecialCondition.PageSize)  %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Name" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConditionName" runat="server" Text='<%# Eval("ConditionName")%>'>
                                                    </asp:Label>
                                                </ItemTemplate>                                                   
                                            </asp:TemplateColumn>  
                                            <asp:TemplateColumn HeaderText="Value" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtConditionValue" runat="server" Columns="20" MaxLength="500"
                                                        Text='<%# Eval("ConditionValue") %>' />
                                                </ItemTemplate>                                                   
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle CssClass="tHeader" />
                                        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                    </asp:DataGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>                  
    </ajaxToolkit:TabContainer>
        </div>
    
        <div class="panel-body">
            <br />
            <asp:UpdatePanel ID="updatePanel6" runat="server">
                <ContentTemplate>
                   <div class="row">
                        <div class="col-lg-6 col-md-6">
                            <p>
                                <asp:Label runat="server" ID="lblCreatedBy"></asp:Label>
                            </p>
                            <p>
                                 <asp:Label runat="server" ID="lblModifiedBy"></asp:Label>
                            </p>
                            <asp:DataGrid ID="dgApproval" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                                ShowFooter="false" GridLines="none" CellPadding="5" CellSpacing="5" OnItemCommand="dgApproval_ItemCommand"
                                OnItemDataBound="dgApproval_ItemDataBound">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgApproval.CurrentPageIndex) * dgApproval.PageSize)%>
                                        </ItemTemplate>
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
                            </asp:DataGrid>
                        </div>
                        <div class="col-lg-6 col-md-6">
                            <div class="well">
                                 <div class="row">
                                    <div class="col-md-8 col-sm-8 text-right">Sub-Total : </div>
                                    <div class="col-md-2 col-sm-2 text-right">
                                        <asp:Label runat="server" ID="lblCurrency" Text="SGD" />
                                    </div>
                                     <div class="col-md-2 col-sm-2 text-right">
                                          <asp:Label runat="server" ID="lblSubTotal" />
                                     </div>
                                </div>
                                 <div class="row">
                                    <div class="col-lg-5 col-md-5 col-sm-5 text-right">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon">GST</span>
                                            <asp:DropDownList runat="server" ID="ddlTaxType" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlTaxType_SelectedIndexChanged"
                                                AutoPostBack="true" />
                                         </div>
                                    </div>
                                     <div class="col-md-3 col-sm-3">
                                         <asp:TextBox runat="server" ID="txtTaxRate" Columns="6" CssClass="form-control input-sm text-right"
                                                    contentEditable="false" /> 
                                     </div>
                                    <div class="col-md-2 col-sm- text-right" style="padding-top:6px">
                                        <asp:Label runat="server" ID="lblCurrency2" Text="SGD" />
                                    </div>
                                     <div class="col-md-2 col-sm-2 text-right">
                                         <asp:Label runat="server" ID="lblTaxAmount" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-8 col-sm-8 text-right">
                                        Grand Total :
                                    </div>
                                    <div class="col-md-2 col-sm-2 text-right">
                                        <asp:Label runat="server" ID="lblCurrency3" Text="SGD" />
                                    </div>
                                    <div class="col-md-2 col-sm-2 text-right">
                                        <asp:Label runat="server" ID="lblGrandTotal" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-8 col-sm-8 text-right">
                                       GP% :
                                    </div>
                                     <div class="col-md-4 col-sm-4 text-right">
                                         <asp:Label runat="server" ID="lblGPercentage" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
   
        <div class="panel-footer clearfix">
            <div class="pull-right">
              <asp:Button runat="server" ID="btnImportRecipe" Text="Import Recipe" CssClass="btn btn-default" OnClick="btnImportRecipe_Click" Visible="false" />
                <asp:Button runat="server" ID="btnApplyDiscount" Text="Apply Discount" CssClass="btn btn-default" />
                <asp:Button runat="server" ID="btnDuplicate" Text="Duplicate" CssClass="btn btn-default" OnClientClick="return confirm('A new quotation will be created. Continue?')"
                    OnClick="btnDuplicate_Click" />
                <asp:Button runat="server" ID="btnAddToRevision" Text="Add To Revision" CssClass="btn btn-default"
                    OnClientClick="return confirm('Are you sure you want to add a new revision?')"
                    OnClick="btnAddToRevision_Click" />
                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-default" OnClick="btnSubmit_Click" OnClientClick="btnActionClick()" />
                <asp:Button runat="server" ID="btnSubmit" Text="Submit For Approval" CssClass="btn btn-default" OnClick="btnSubmit_Click" OnClientClick="btnActionClick()" />
                <asp:Button runat="server" ID="btnDeleteRevision" Text="Delete Revision" CssClass="btn btn-default" OnClick="btnDeleteRevision_Click" Visible="false" OnClientClick="btnActionClick()" />
            </div>   
        </div>
    </div>
    <!--End Panel-->

    <asp:UpdatePanel runat="server" id="upReportLinks" updatemode="Conditional" >      
        <ContentTemplate>
            <p>  
                <asp:LinkButton ID="lnkPrintReport" OnClick="GenerateReport" runat="server" Text="Print"
                    CssClass="btn btn-default btn-sm pull-right" ToolTip="Please click to print report." CausesValidation="False">
                    <i class="ti-printer"></i>
                </asp:LinkButton>
                <asp:LinkButton ID="lnkPrintPDF" OnClick="GeneratePDFReport" runat="server" Text="Print PDF"
                    CssClass="btn btn-default btn-sm pull-right" ToolTip="Please click to print PDF report." CausesValidation="False">
                    <i class="ti-download"></i> PDF
                </asp:LinkButton>                    
                <asp:LinkButton ID="lnkEmailPDF" OnClick="GenerateEmailPDFReport" runat="server" Text="Email PDF"
                    CssClass="btn btn-default btn-sm pull-right" ToolTip="Please click to email PDF report." CausesValidation="False" OnClientClick="btnActionClick()">
                    <i class="ti-email"></i>    
                </asp:LinkButton>  
                <asp:DropDownList ID="ddlReport" runat="server" CssClass="form-control input-sm no-full-width pull-right">
                </asp:DropDownList>
            </p>
            <div class="clearfix"></div>
    

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
                <asp:TextBox runat="server" ID="txtTo" Columns="70" onfocus="select();" CssClass="form-control input-sm"></asp:TextBox> 
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
                <asp:TextBox runat="server" ID="txtCC" Columns="70" onfocus="select();" CssClass="form-control input-sm"></asp:TextBox>
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
                <asp:TextBox runat="server" ID="txtBCC" Columns="70" onfocus="select();" CssClass="form-control input-sm"></asp:TextBox> 
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
                <asp:TextBox runat="server" ID="txtEmailSubject" Columns="70" onfocus="select();" CssClass="form-control input-sm"></asp:TextBox> 
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
        SupportFolder="/GMS4/FreeTextBox/" 
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
             <asp:PostBackTrigger ControlID="btnUpdateFileUploadAttachment" />
          
    </Triggers> 
            
    </asp:UpdatePanel>
    <div style="display: none">
                 <asp:Button ID="btnUpdateFileUploadAttachment" runat="server" Text="" OnClick="btnUpdateFileUploadAttachment_Click" />  
    </div>
    
    
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
                        <asp:TextBox CssClass="form-control input-sm" ID="txtDiscountRate" runat="server" Columns="5" MaxLength="10" />
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
        <!-- Remove ajax picture
    <table align="center" style="margin-left: auto; margin-right: auto; margin-top: 0px; margin-bottom: 0px;" >
        <tr align="center">             
        <td>
        <img src = "../../images/ajax-loader.gif" />                                                         
        </td>
        </tr>
    </table>
    -->
    </asp:Panel>
    
    
 
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".sale-menu").addClass("active expand");
            $(".sub-quotation").addClass("active");
        });

        function getProductDetails(element) {
            var context = $(element).prev().prev().val();
            //alert(context);
            $.ajax({
                async: true,
                url: "AddEditQuotation.aspx/GetDynamicContent",
                data: JSON.stringify({ "contextKey": context }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    data = data.hasOwnProperty('d') ? data.d : data;
                    $(element).attr('data-content', data);
                    $('[data-toggle="popover"]').popover();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(textStatus);
                    console.log("Product Detail Error");
                }
            });
           
        }
    </script>
</asp:Content>