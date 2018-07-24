<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="AccountManagement.aspx.cs" Inherits="GMSWeb.Debtors.Debtors.AccountManagement" Title="Customer Info" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">




    <script type="text/javascript">



        function changeButtonClass() {
            // code examples from above
            //document.getElementById('btnTrigger').className = 'crm_button_selected';

            alert('abc');
        }



        function clientActiveTabChanged(sender, args) {
        }

        function progress_update(a) {

            document.getElementById('loading_' + a).style.visibility = 'visible';

        }

        function progress_stop() {

            document.getElementById('loading' + a).style.visibility = 'hidden';

        }



        function confirm_delete() {
            if (confirm("Are you sure you want to delete this item?") == true)
                return true;
            else
                return false;
        }

        function confirm_upgrade() {
            if (confirm("Are you sure you want to upgrade this prospect? ") == true)
                return true;
            else
                return false;
        }

    </script>

    <h1 class="page-header">Customer Info</h1>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="AutoCompleteAccountCode.asmx" />
            <asp:ServiceReference Path="AutoCompleteProductGroupName.asmx" />
            <asp:ServiceReference Path="AutoCompleteProductName.asmx" />
        </Services>
    </asp:ScriptManager>

    <input type="hidden" id="hidAccountCode" runat="server" />

    <asp:UpdatePanel ID="upOutter" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnLoadParticular" />
            <asp:AsyncPostBackTrigger ControlID="btnTrigger" />
            <asp:AsyncPostBackTrigger ControlID="btnLoadCommRecord" />
            <asp:AsyncPostBackTrigger ControlID="btnLoadAttachment" />
            <asp:AsyncPostBackTrigger ControlID="btnLoadFinance" />
            <asp:AsyncPostBackTrigger ControlID="btnLoadSales" />
            <asp:AsyncPostBackTrigger ControlID="btnLoadCollection" />
            <asp:AsyncPostBackTrigger ControlID="btnLoadOutstandingPayment" />
            <asp:AsyncPostBackTrigger ControlID="btnLoadPurchase" />
            <asp:AsyncPostBackTrigger ControlID="btnLoadOthers" />
        </Triggers>

        <ContentTemplate>
            <h3>
            <asp:Label ID="lblAccountCode" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblAccountName" runat="server"></asp:Label>
                </h3>
           <div class="well">
             <div class="btn-group">
                <input class="btn btn-default active" id="btnLoadParticular" value="Particulars" runat="server" type="button" onserverclick="btnLoadParticular_Click">
                <input class="btn btn-default" id="btnTrigger" value="Contacts" runat="server" type="button" onserverclick="btnTrigger_Click">
                <input class="btn btn-default" id="btnLoadCommRecord" value="Communication" runat="server" type="button" onserverclick="btnLoadCommRecord_Click">
                <input class="btn btn-default" id="btnLoadAttachment" value="Attachments" runat="server" type="button" onserverclick="btnLoadAttachment_Click">
                <input class="btn btn-default" id="btnLoadFinance" value="Financial Attachments" runat="server" type="button" onserverclick="btnLoadFinance_Click">
                <input class="btn btn-default" id="btnLoadSales" value="Sales" runat="server" type="button" onserverclick="btnLoadSales_Click">
                <input class="btn btn-default" id="btnLoadCollection" value="Collections" runat="server" type="button" onserverclick="btnLoadCollection_Click">
                <input class="btn btn-default" id="btnLoadOutstandingPayment" value="Debts" runat="server" type="button" onserverclick="btnLoadOutstandingPayment_Click">
                <input class="btn btn-default" id="btnLoadPurchase" value="Purchases" runat="server" type="button" onserverclick="btnLoadPurchase_Click">
                <input class="btn btn-default" id="btnLoadOthers" value="Others" runat="server" type="button" onserverclick="btnLoadOthers_Click">
            </div>
            </div>
          

                <!--Start : Particulars Panel-->
                <asp:UpdatePanel ID="upParticular" runat="server" UpdateMode="Conditional" Visible="true">
                    <ContentTemplate>
                        <div class="panel panel-primary">
                            <div class="panel-body">
                                <p class="desc"><asp:Label ID="lblParticularsMsg" runat="server"></asp:Label></p>
                                <div class="row">
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Customer Code
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:TextBox runat="server" ID="txtAccountCode" MaxLength="10" Columns="12" onfocus="select();"
                                                CssClass="form-control"></asp:TextBox>
                                            <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                                                TargetControlID="txtAccountCode" ServicePath="AutoCompleteAccountCode.asmx"
                                                ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                                            </ajaxToolkit:AutoCompleteExtender>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Customer Name
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:TextBox runat="server" ID="txtAccountName" MaxLength="50" Columns="30" onfocus="select();"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Group
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:DropDownList CssClass="form-control" ID="ddlGroup" runat="Server"
                                                DataTextField="AccountGroupName" DataValueField="AccountGroupCode" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Type
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:DropDownList CssClass="form-control" ID="ddlType" runat="Server"
                                                DataTextField="ClassName" DataValueField="ClassID" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Sales Person
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:DropDownList ID="ddlSalesman" runat="Server" CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Credit Term
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtCreditTerm" runat="server" Columns="5" CssClass="form-control" />
                                                <span class="input-group-addon">Days</span>
                                            </div>
                                            <asp:CompareValidator ID="cvCreditTerm" runat="server" ErrorMessage="*"
                                                Display="Dynamic" ControlToValidate="txtCreditTerm" Type="Integer" Operator="DataTypeCheck"
                                                ValidationGroup="valGrpNewRow" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Default Currency
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:DropDownList runat="server" ID="ddlCurrency" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Credit Limit
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:TextBox ID="txtCreditLimit" runat="server" Columns="20" CssClass="form-control" />
                                            <asp:CompareValidator ID="cvCreditLimit" runat="server" ErrorMessage="*"
                                                Display="Dynamic" ControlToValidate="txtCreditLimit" Type="Double" Operator="DataTypeCheck"
                                                ValidationGroup="valGrpNewRow" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Credit Used on DO
                                        </label>
                                        <label class="col-sm-8 control-label text-left">
                                            <asp:Label runat="server" ID="lblCreditUsedOnDO"></asp:Label>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Original Credit Limit
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:TextBox ID="txtOriginalCreditLimit" runat="server" Columns="20" CssClass="form-control" />
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*"
                                                Display="Dynamic" ControlToValidate="txtOriginalCreditLimit" Type="Double" Operator="DataTypeCheck"
                                                ValidationGroup="valGrpNewRow" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Outstanding
                                        </label>
                                        <label class="col-sm-8 control-label text-left">
                                            <asp:Label runat="server" ID="lblOutstanding"></asp:Label>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Excess
                                        </label>
                                        <label class="col-sm-8 control-label text-left">
                                            <asp:Label runat="server" ID="lblExcess"></asp:Label>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Grade
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:DropDownList CssClass="form-control" ID="ddlGrade" runat="Server"
                                                DataTextField="GradeName" DataValueField="GradeCode" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Industry
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:DropDownList CssClass="form-control" ID="ddlIndustry" runat="Server"
                                                DataTextField="Name" DataValueField="Name" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Territory
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:DropDownList CssClass="form-control" ID="ddlTerritory" runat="Server" DataTextField="TerritoryName" DataValueField="TerritoryName" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Address1
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:TextBox runat="server" ID="txtAddress1" MaxLength="40" Columns="40" onfocus="select();"
                                                CssClass="form-control" TabIndex="1"></asp:TextBox>
                                        </div>
                                        <label class="col-sm-4 control-label text-left">
                                            Address2
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:TextBox runat="server" ID="txtAddress2" MaxLength="40" Columns="40" onfocus="select();"
                                                CssClass="form-control" TabIndex="2"></asp:TextBox>
                                        </div>
                                        <label class="col-sm-4 control-label text-left">
                                            Address3
                                        </label>
                                        <div class="col-sm-8 control-label text-left">

                                            <asp:TextBox runat="server" ID="txtAddress3" MaxLength="40" Columns="40" onfocus="select();"
                                                CssClass="form-control" TabIndex="2"></asp:TextBox>
                                        </div>
                                        <label class="col-sm-4 control-label text-left">
                                            Address4
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:TextBox runat="server" ID="txtAddress4" MaxLength="40" Columns="40" onfocus="select();"
                                                CssClass="form-control" TabIndex="2"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Postal Code
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:TextBox ID="txtPostalCode" runat="server" Columns="20" MaxLength="6"
                                                CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Website
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:TextBox runat="server" ID="txtWebsite" MaxLength="50" Columns="40" onfocus="select();"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Facebook
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:TextBox runat="server" ID="txtFacebook" MaxLength="50" Columns="40" onfocus="select();"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Remarks
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3"
                                                Columns="40" MaxLength="600" CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-4 control-label text-left">
                                            Is Active
                                        </label>
                                        <div class="col-sm-8 control-label text-left">
                                            <div class="checkbox">
                                                <asp:CheckBox ID="chkAcctActive" runat="server" Checked Text="&nbsp;" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="control-label text-left">
                                            <asp:Label runat="server" ID="lblCreatedBy"></asp:Label>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="control-label text-left">
                                            <asp:Label runat="server" ID="lblModifiedBy"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer clearfix">
                                <asp:Button ID="btnUpdateParticular" Text="Update" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                                    OnClick="btnUpdateParticular_Click"></asp:Button>
                                <asp:Button ID="btnUpgradeToCustomer" Text="Upgrade To Customer" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10" Visible="false"
                                    OnClick="btnUpgradeToCustomer_Click" OnClientClick="return confirm_upgrade();"></asp:Button>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnUpdateParticular" />
                        <asp:AsyncPostBackTrigger ControlID="btnUpgradeToCustomer" />
                    </Triggers>
                </asp:UpdatePanel>
                <!--End : Particulars Panel-->
                
                <!--Start : Contact Panel-->
                <asp:UpdatePanel ID="upContact" runat="server" UpdateMode="Conditional" Visible="True">
                    <ContentTemplate>
                        <div class="modal fade" id="contact-modal" style="display: none;">
                            <div class="modal-dialog">
	                            <div class="modal-content">
	                                <div class="modal-header">
	                                    <h4 class="modal-title"><asp:Label ID="lblContact" runat="server"></asp:Label></h4>           
	                                </div>
		                            <div class="modal-body form-horizontal">
		                                <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                First Name
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox ID="txtFirstName" runat="server" Columns="50" MaxLength="50" CssClass="form-control"
                                                    onfocus="select();" /><asp:RequiredFieldValidator ID="rfvCourseTitle" runat="server"
                                                        ControlToValidate="txtFirstName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
                                                <input type="hidden" id="hidContactID1" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Last Name
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox ID="txtLastName" runat="server" Columns="50" MaxLength="50" CssClass="form-control"
                                                onfocus="select();" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="txtLastName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Salutation
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:DropDownList ID="ddlSalutation" runat="server" DataTextField="Name"
                                                    DataValueField="Name" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Designation
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox ID="txtDesignation" runat="server" Columns="30" MaxLength="50" CssClass="form-control"
                                                    onfocus="select();" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                    ControlToValidate="txtDesignation" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Office Phone
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtOfficePhone" runat="server" Columns="30" MaxLength="50" CssClass="form-control"
                                                    onfocus="select();" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                    ControlToValidate="txtOfficePhone" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Mobile Phone
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox ID="txtMobilePhone" runat="server" Columns="30" MaxLength="50" CssClass="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Fax
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox ID="txtFax" runat="server" Columns="30" MaxLength="50"
                                                    CssClass="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Email
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox ID="txtEmail" runat="server" Columns="30" MaxLength="50"
                                                    CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                    ControlToValidate="txtEmail" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
<%--                                                <asp:RegularExpressionValidator ID="regexpName" runat="server" ErrorMessage="Invalid Format" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*" ValidationGroup="valGrpNewRow" />--%>
<%--                                                --%>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Remarks
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox ID="txtContactRemarks" runat="server" TextMode="MultiLine" Rows="3"
                                                    Columns="40" MaxLength="400" CssClass="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Is Active
                                            </label>
                                            <div class="col-sm-8">
                                               <div class="checkbox">
                                                    <asp:CheckBox ID="chkIsActive" runat="server" Checked Text=" "/>
                                                </div>
                                            </div>
                                        </div>
                                   </div>
                                   <div class="modal-footer">
                                       <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                                        <asp:Button CssClass="btn btn-primary" ID="ButtonOk" runat="server" Text="Submit" OnCommand="AddUpdateContact"
                                            ValidationGroup="valGrpNewContact" />
                                    </div>
                                </div>
                            </div>
                        </div>
                       
                        <div class="panel panel-primary">
                            <div class="panel-body no-padding ">
                                <p class="desc"> <span style="color: Red; size: 7px; font-style: italic;">
                                    <asp:Label ID="lblContactsMsg" runat="server"></asp:Label></span></p>
                                <div class="panel panel-primary m-b-0 no-border">
                                    <div class="panel-heading">

                                        <h4 class="panel-title">
                                            <i class="ti-align-justify"></i>
                                            <asp:Label ID="lblContactsSummary" Visible="false" runat="server" />
                                        </h4>
                                    </div>
                                    <div class="table-responsive">
                                        <asp:DataGrid ID="dgContacts" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                            DataKeyField="ContactID" GridLines="none" OnItemDataBound="dgContacts_ItemDataBound" OnDeleteCommand="dgContacts_DeleteCommand" OnItemCommand="dgContacts_Command" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgContacts_PageIndexChanged" EnableViewState="true" OnSortCommand="SortContact" AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                                    <ItemTemplate>
                                                        <%# (Container.ItemIndex + 1) + ((dgContacts.CurrentPageIndex) * dgContacts.PageSize)%>
                                                        <input type="hidden" id="hidContactID" runat="server" value='<%# Eval("ContactID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Name" SortExpression="FirstName" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFirstName" runat="server">
                                                            <asp:LinkButton ID="lnkEditContact" runat="server" CommandName="EditContact"><%# Eval("FirstName")%>&nbsp;<%# Eval("LastName")%></asp:LinkButton>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Sal." SortExpression="Salutation" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <%# Eval("Salutation")%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="Designation" SortExpression="Designation" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <%# Eval("Designation")%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Office No." SortExpression="OfficePhone" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <%# Eval("OfficePhone")%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Mobile No." SortExpression="MobilePhone" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <%# Eval("MobilePhone")%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Email" SortExpression="Email" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <%# Eval("Email")%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Is Active" SortExpression="IsActive">
                                                    <ItemTemplate>
                                                        <%# (Eval("IsActive").Equals(System.DBNull.Value)) ? "No" : ((bool)Eval("IsActive")) ? "Yes" : "No"%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                                                    </ItemTemplate>

                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" />
                                            <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer clearfix">
                                <asp:Button ID="btnAddNewContact" Text="Add Contact" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right" OnClick="btnAddNewContact_Click"  />
                            </div>
                        <asp:Button runat="server" ID="HiddenForModalAddNewContact" Style="display: none" />
                      </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--End : Contact Panel-->

                <!--Start : Communication Panel-->
                <asp:UpdatePanel ID="upCommunication" runat="server" UpdateMode="Conditional" Visible="True">
                    <ContentTemplate>
                        <div class="modal fade" id="communication-modal" style="display: none;">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title">
                                            <asp:Label ID="lblCommunication" runat="server"></asp:Label></h4>
                                    </div>
                                    <div class="modal-body form-horizontal">
                                        <input type="hidden" id="hidCommID1" runat="server" />
                                        <span style="color: Red; size: 7px; font-style: italic;">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label></span>

                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Description
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtDescription" runat="server" Columns="40" MaxLength="1000" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewCommunication" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Date From
                                            </label>
                                            <div class="col-sm-4">
                                                <div class="input-group date">
                                                    <asp:TextBox runat="server" ID="txtDateFrom" MaxLength="10" Columns="10" CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="ti-calendar"></i>
                                                    </span>
                                                </div>
                                                <asp:RequiredFieldValidator ID="rfvDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewCommunication" />
                                                <asp:CompareValidator ID="cvDateFrom" runat="server" ErrorMessage="Invalid Date"
                                                    ControlToValidate="txtDateFrom" Type="Date" Display="Dynamic" ValidationGroup="valGrpNewCommunication"
                                                    Operator="DataTypeCheck"></asp:CompareValidator>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="input-group timepicker">
                                                    <asp:TextBox runat="server" ID="txtDateFromTime" MaxLength="5" Columns="10" CssClass="form-control"
                                                        ToolTip="Time format should be 00:00" data-provide="timepicker" data-show-meridian="false"></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="ti-timer"></i>
                                                    </span>
                                                </div>

                                                <asp:RequiredFieldValidator ID="rfvateFromTime" runat="server" ControlToValidate="txtDateFromTime"
                                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewCommunication" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Date To
                                            </label>

                                            <div class="col-sm-4">
                                                <div class="input-group date">
                                                    <asp:TextBox runat="server" ID="txtDateTo" MaxLength="10" Columns="10" CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="ti-calendar"></i>
                                                    </span>
                                                </div>
                                                <asp:RequiredFieldValidator ID="rfvDateTo" runat="server" ControlToValidate="txtDateTo"
                                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewCommunication" />
                                                <asp:CompareValidator ID="cvDateTo" runat="server" ErrorMessage="Invalid Date" ControlToValidate="txtDateTo"
                                                    Type="Date" Display="Dynamic" ValidationGroup="valGrpNewCommunication" Operator="DataTypeCheck"></asp:CompareValidator>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="input-group timepicker">
                                                    <asp:TextBox runat="server" ID="txtDateToTime" MaxLength="5" Columns="10" CssClass="form-control"
                                                        data-provide="timepicker" data-show-meridian="false" ToolTip="Time format should be 00:00" ></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="ti-timer"></i>
                                                    </span>
                                                </div>
                                                <asp:RequiredFieldValidator ID="rfvDateToTime" runat="server" ControlToValidate="txtDateToTime"
                                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewCommunication" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Type
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:DropDownList ID="ddlCommType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="Visit">Visit</asp:ListItem>
                                                    <asp:ListItem Value="Call">Call</asp:ListItem>
                                                    <asp:ListItem Value="Complaint">Complaint</asp:ListItem>
                                                    <asp:ListItem Value="Email">Email</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Status
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:DropDownList ID="ddlCommStatus" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="Follow up">Follow up</asp:ListItem>
                                                    <asp:ListItem Value="Closed">Closed</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Comment
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="2"
                                                    Columns="40" MaxLength="1000" CssClass="form-control" />
                                            </div>
                                        </div>
                                     
                                        <div id="Div8" style="text-align: left;" runat="server">
                                            <asp:Label ID="lblCommCommentRecordSummary" Visible="false" runat="server" />
                                        </div>
                                                  
                                        <asp:DataGrid ID="dgCommRecordComment" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            DataKeyField="CommID" GridLines="none" OnItemDataBound="dgCommRecordComment_ItemDataBound" OnDeleteCommand="dgCommRecordComment_DeleteCommand" OnItemCommand="dgCommRecordComment_Command" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgCommRecordComment_PageIndexChanged" EnableViewState="true" OnEditCommand="dgCommRecordComment_EditCommand" OnUpdateCommand="dgCommRecordComment_UpdateCommand">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No">
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# (Container.ItemIndex + 1) + ((dgCommRecordComment.CurrentPageIndex) * dgCommRecordComment.PageSize)%>
                                                        <input type="hidden" id="CommentID" runat="server" value='<%# Eval("CommentID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>


                                                <asp:TemplateColumn HeaderText="Comment" HeaderStyle-Wrap="false">

                                                    <ItemTemplate>
                                                        <span style="color: Green; size: 7px; font-style: italic;">Created By <%# Eval("CreatedByName")%> on <%# Eval("CreatedDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("CreatedDate", "{0: dd/MM/yyyy HH:mm}")%></span><br />
                                                        <%# Eval("Comment")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditComment" runat="server" TextMode="MultiLine" Rows="2"
                                                            Columns="40" MaxLength="1000" CssClass="textarea" Text='<%# Eval("Comment")%>' />
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">

                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEditCommComment" runat="server" CommandName="Edit" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Edit"><i class="ti-pencil"></i> </asp:LinkButton>						      							               								             								        
											                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                                            ValidationGroup="valGrpEditRow" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Edit"><i class="ti-check"></i> </asp:LinkButton>	
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                                            CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Edit"><i class="ti-close"></i> </asp:LinkButton>	
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>


                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                            <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                        </asp:DataGrid>

                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                        <asp:Button CssClass="btn btn-primary" ID="btnAddUpdateCommunication" runat="server" Text="Submit" OnCommand="AddUpdateCommunication"
                                            ValidationGroup="valGrpNewCommunication" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-primary">
                            <div class="panel-body  no-padding">
                                <p class="desc">
                                    <span style="color: Red; size: 7px; font-style: italic;">
                                    <asp:Label ID="lblCommunicationMsg" runat="server"></asp:Label></span>
                                </p>
                                <asp:Button runat="server" ID="HiddenForModalCommunicationComment" Style="display: none" />
                             
                                <div class="panel panel-primary m-b-0 no-border">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <i class="ti-align-justify"></i>
                                            <asp:Label ID="lblCommRecordSummary" Visible="false" runat="server" />
                                        </h4>
                                    </div>
                                    <div class="table-responsive">
                                        <asp:DataGrid ID="dgCommRecord" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            DataKeyField="CommID" GridLines="none" OnItemDataBound="dgCommRecord_ItemDataBound" OnDeleteCommand="dgCommRecord_DeleteCommand" OnItemCommand="dgCommRecord_Command" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgCommRecord_PageIndexChanged" EnableViewState="true" OnSortCommand="SortCommunication" AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                                    <ItemTemplate>
                                                        <%# (Container.ItemIndex + 1) + ((dgCommRecord.CurrentPageIndex) * dgCommRecord.PageSize)%>
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
                                                        <asp:LinkButton ID="lnkEditCommunication" runat="server" CommandName="EditCommunication"  CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Edit"><i class="ti-pencil"></i></asp:LinkButton>
								                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false"  CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" />
                                            <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer clearfix">
                                <asp:Button ID="btnAddCommunication" Text="Add Communication" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right" OnClick="AddCommunication_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--End : Communication Panel-->


                <!--Start : Attachement Panel-->
                <asp:UpdatePanel ID="upAttachment" runat="server" UpdateMode="Conditional" Visible="true">
                    <ContentTemplate>
                        <div class="panel panel-primary">
                            <div class="panel-body no-padding">
                                <div class="container-fluid m-t-20">
                                    <div class="form-horizontal">
                                        <div class="form-group col-6 col-md-6">
                                            <label class="col-sm-4 control-label text-left">
                                                <asp:Label CssClass="tbLabel" ID="lblFileName" runat="server">Title</asp:Label>
                                            </label>
                                            <div class="col-sm-8">
                                                  <asp:TextBox runat="server" ID="txtFileName" MaxLength="100" Columns="45" onfocus="select();" CssClass="form-control"></asp:TextBox>
                                                  <asp:RequiredFieldValidator ID="rfvFileName" runat="server" ControlToValidate="txtFileName" ErrorMessage="*" Display="dynamic" ValidationGroup="attachment" />          
                                            </div>
                                        </div>
                                        <div class="form-group col-6 col-md-6">
                                            <label class="col-sm-4 control-label text-left">
                                                <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">Location</asp:Label>
                                            </label>
                                            <div class="col-sm-8">
                                                <div class="input-group">
                                                    <input type="text" class="form-control" readonly>
                                                     <label class="input-group-btn">
                                                        <span class="btn btn-primary btn-upload">
                                                            <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                                            <asp:FileUpload CssClass="form-control hidden" ID="FileUpload1" runat="server" />
                                                        </span>
                                                    </label>
                                                </div>
                                                <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="FileUpload1"
                                                    ErrorMessage="*" Display="dynamic" ValidationGroup="attachment" />       
                                            </div>
                                        </div>
                                     </div>
                               </div>
                                <span style="color: Red; size: 7px; font-style: italic;">
                                    <asp:Label ID="lblAttachmentMsg" runat="server"></asp:Label></span>
                            
                                <div class="panel panel-primary m-b-0 no-border">
                                    <div class="panel-heading">

                                        <h4 class="panel-title">
                                            <i class="ti-align-justify"></i>
                                            <asp:Label ID="lblAttachmentSummary" Visible="false" runat="server" />
                                        </h4>
                                    </div>
                                    <div class="table-responsive">
                                        <asp:DataGrid ID="dgAttachment" runat="server" AutoGenerateColumns="false"
                                    GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                                    PageSize="20" OnPageIndexChanged="dgAttachment_PageIndexChanged" EnableViewState="true" OnSortCommand="SortAttachment" AllowSorting="true"
                                    OnItemDataBound="dgAttachment_ItemDataBound" OnDeleteCommand="dgAttachment_DeleteCommand" OnItemCommand="dgAttachment_Command">
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="No">
                                            <ItemTemplate>
                                                <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize) %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Title" SortExpression="DocumentName" HeaderStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkName" runat="server" Text='<%# Eval("DocumentName")%>' CommandName="Load" ForeColor="#005DAA" CommandArgument='<%#Eval("FileName")%>'></asp:LinkButton>
                                                <input type="hidden" id="hidDocumentID" runat="server" value='<%# Eval("FileName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Uploaded Date" SortExpression="CreatedDate" HeaderStyle-Wrap="false">
                                            <ItemTemplate>
                                                <%# Eval("CreatedDate", "{0:dd/MM/yyyy}")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Uploaded By" SortExpression="userrealname" HeaderStyle-Wrap="false" >
                                            <ItemTemplate>
                                                <%# Eval("userrealname")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <HeaderStyle CssClass="tHeader" />
                                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                </asp:DataGrid>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer clearfix">
                                <asp:Button ID="btnUpload" Text="Upload" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                                    OnClick="btnUpload_Click" ValidationGroup="attachment"></asp:Button>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUpload" />
                    </Triggers>
                </asp:UpdatePanel>
                <!--End : Attachement Panel-->

                <!--Start : Finance Panel-->
                <asp:UpdatePanel ID="upFinance" runat="server" UpdateMode="Conditional" Visible="true">
                    <ContentTemplate>
                        <div class="panel panel-primary">
                            <div class="panel-body no-padding">
                                <div runat="server" id="divFinanceAttachment" >
                                    <div class="container-fluid m-t-20">
                                    <div class="form-horizontal">
                                        <div class="form-group col-lg-6">
                                            <label class="col-sm-4 control-label text-left">
                                                 <asp:Label CssClass="tbLabel" ID="lblType" runat="server">Type</asp:Label>
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:DropDownList CssClass="form-control" ID="ddlAttachmentType" runat="server" DataTextField="FinanceAttachmentTypeName" DataValueField="FinanceAttachmentTypeName" AutoPostBack="true" OnSelectedIndexChanged="ddlAttachmentType_SelectedIndexChanged" />
                                            </div>
                                        </div>
                                        <div class="form-group col-lg-6">
                                            <label class="col-sm-4 control-label text-left">
                                                <asp:Label CssClass="tbLabel" ID="lblAttachmentLocation" runat="server">Location</asp:Label>
                                            </label>
                                            <div class="col-sm-8">
                                                <div class="input-group">
                                                    <input type="text" class="form-control" readonly>
                                                     <label class="input-group-btn">
                                                        <span class="btn btn-primary btn-upload">
                                                            <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                                            <asp:FileUpload CssClass="form-control hidden" ID="FileUploadFinance" runat="server" />
                                                        </span>
                                                    </label>
                                                </div>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="FileUploadFinance"
                                                    ErrorMessage="*" Display="dynamic" ValidationGroup="financeAttachment" />
                                            </div>
                                        </div>
                                        <div class="form-group col-lg-6">
                                            <label class="col-sm-4 control-label text-left">
                                                <asp:Label CssClass="tbLabel" ID="lblPeriod" runat="server">Period From (Mon-Year)</asp:Label>
                                            </label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList CssClass="form-control" ID="ddlPeriodMonthFrom" runat="server" DataTextField="Month" DataValueField="Month" Enabled="false" />
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:DropDownList CssClass="form-control" ID="ddlPeriodYearFrom" runat="server" DataTextField="Year" DataValueField="Year" Enabled="false" />
                                            </div>
                                        </div>
                                        <div class="form-group col-lg-6">
                                            <label class="col-sm-4 control-label text-left">
                                                <asp:Label CssClass="tbLabel" ID="Label1" runat="server">Period To (Mon-Year)</asp:Label>
                                            </label>
                                            <div class="col-sm-4">
                                                 <asp:DropDownList CssClass="form-control" ID="ddlPeriodMonthTo" runat="server" DataTextField="Month" DataValueField="Month" Enabled="false" />
                                            </div>
                                            <div class="col-sm-4">
                                                  <asp:DropDownList CssClass="form-control" ID="ddlPeriodYearTo" runat="server" DataTextField="Year" DataValueField="Year" Enabled="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                 </div>
                                <span style="color: Red; size: 7px; font-style: italic;">
                                        <asp:Label ID="lblFinanceAttachmentMsg" runat="server"></asp:Label></span>
                                
                                <div class="panel panel-primary no-margin">
                                        <div class="panel-heading">
                                            <div class="panel-heading-btn">
                                                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                                            </div>
                                            <h4 class="panel-title">
                                                <i class="ti-align-justify"></i>
                                                <asp:Label ID="lblFinanceAttachmentSummary" Visible="false" runat="server" />
                                            </h4>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:DataGrid ID="dgFinanceAttachment" runat="server" AutoGenerateColumns="false"
                                                GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                                                PageSize="20" OnPageIndexChanged="dgFinanceAttachment_PageIndexChanged" EnableViewState="true" OnSortCommand="SortFinanceAttachment" AllowSorting="true"
                                                OnItemDataBound="dgFinanceAttachment_ItemDataBound" OnDeleteCommand="dgFinanceAttachment_DeleteCommand" OnItemCommand="dgFinanceAttachment_Command" OnEditCommand="dgFinanceAttachment_EditCommand" OnCancelCommand="dgFinanceAttachment_CancelCommand" OnUpdateCommand="dgFinanceAttachment_UpdateCommand">
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="No">
                                                        <ItemTemplate>
                                                            <%# (Container.ItemIndex + 1) + ((dgFinanceAttachment.CurrentPageIndex) * dgFinanceAttachment.PageSize) %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Type" SortExpression="Type" HeaderStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <%# Eval("Type")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:HiddenField ID="hidType" runat="server" Value='<%# Eval("Type")%>' />
                                                            <asp:DropDownList CssClass="dropdownlist" ID="ddlEditType" runat="Server" DataTextField="FinanceAttachmentTypeName" DataValueField="FinanceAttachmentTypeName" AutoPostBack="true" OnSelectedIndexChanged="ddlEditType_SelectedIndexChanged" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Period From : To" HeaderStyle-Wrap="false" >
                                                        <ItemTemplate>
                                                            <%# string.IsNullOrEmpty(Eval("PeriodYearFrom").ToString())  ? "NIL" : new DateTime(Convert.ToInt32(Eval("PeriodYearFrom")), Convert.ToInt32(Eval("PeriodMonthFrom")), 1).ToString("MMM-yyyy")+ " : "  + new DateTime(Convert.ToInt32(Eval("PeriodYearTo")), Convert.ToInt32(Eval("PeriodMonthTo")), 1).ToString("MMM-yyyy")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:HiddenField ID="hidYearFrom" runat="server" Value='<%# Eval("PeriodYearFrom")%>' />
                                                            <asp:HiddenField ID="hidMonthFrom" runat="server" Value='<%# Eval("PeriodMonthFrom")%>' />
                                                            <asp:HiddenField ID="hidYearTo" runat="server" Value='<%# Eval("PeriodYearTo")%>' />
                                                            <asp:HiddenField ID="hidMonthTo" runat="server" Value='<%# Eval("PeriodMonthTo")%>' />

                                                            <asp:Panel ID="pnl" runat="server">
                                                                Fm:
                                                                <asp:DropDownList CssClass="dropdownlist" ID="ddlEditMonthFrom" runat="Server" DataTextField="Month" DataValueField="Month" />
                                                                :
											    <asp:DropDownList CssClass="dropdownlist" ID="ddlEditYearFrom" runat="Server" DataTextField="Year" DataValueField="Year" />
                                                                To:
                                                                <asp:DropDownList CssClass="dropdownlist" ID="ddlEditMonthTo" runat="Server" DataTextField="Month" DataValueField="Month" />
                                                                :
											    <asp:DropDownList CssClass="dropdownlist" ID="ddlEditYearTo" runat="Server" DataTextField="Year" DataValueField="Year" />
                                                            </asp:Panel>
                                                        </EditItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Title" SortExpression="DocumentName" HeaderStyle-Wrap="false" >
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="linkName" runat="server" Text='<%# Eval("DocumentName")%>' CommandName="Load" ForeColor="#005DAA" CommandArgument='<%#Eval("DocumentName")%>'></asp:LinkButton>

                                                            <input type="hidden" id="hidDocumentID" runat="server" value='<%# Eval("ID")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>


                                                    <asp:TemplateColumn HeaderText="Uploaded Date" SortExpression="CreatedDate" HeaderStyle-Wrap="false" >
                                                        <ItemTemplate>
                                                            <%# Eval("CreatedDate", "{0:dd/MM/yyyy}")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>

                                                    <asp:TemplateColumn HeaderText="Uploaded By" SortExpression="userrealname" HeaderStyle-Wrap="false" >
                                                        <ItemTemplate>
                                                            <%# Eval("userrealname")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false" CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Edit"><i class="ti-pencil"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete">
                                    <i class="ti-trash"></i> </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="true" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="true" CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                                                        </EditItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <HeaderStyle CssClass="tHeader" />
                                                <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                            </asp:DataGrid>
                                        </div>
                                    </div>  
                               

                            </div>
                            <div class="panel-footer clearfix">
                                    <asp:Button ID="btnUploadFinance" Text="Upload" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                                            OnClick="btnUploadFinance_Click" ValidationGroup="financeAttachment"></asp:Button>
                            </div>
                         </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUploadFinance" />
                    </Triggers>
                </asp:UpdatePanel>
                <!--End : Finance Panel-->

                <!-- Start : Sale Panel -->
                <asp:UpdatePanel ID="upSales" runat="server" UpdateMode="Conditional" Visible="true">
                    <ContentTemplate>
                        <div class="panel panel-primary">
                            <div class="panel-body no-padding">
                                 <div class="container-fluid m-t-20">
                                    <div class="form-horizontal">
                                        <div class="form-group col-6 col-md-6">
                                            <label class="col-sm-4 control-label text-left">
                                                Trn Date From
                                            </label>
                                            <div class="col-sm-8">
                                                <div class="input-group date">
                                                    <asp:TextBox runat="server" ID="trnDateFrom" MaxLength="10" Columns="10" onfocus="select();"
                                                        CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="ti-calendar"></i>
                                                    </span>
                                                </div>
                                           
                                            </div>
                                        </div>
                                        <div class="form-group col-6 col-md-6">
                                            <label class="col-sm-4 control-label text-left">
                                                    Trn Date To
                                            </label>
                                            <div class="col-sm-8">
                                                <div class="input-group date">
                                                     <asp:TextBox runat="server" ID="trnDateTo" MaxLength="10" Columns="10" onfocus="select();"
                                                        CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="ti-calendar"></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group col-6 col-md-6">
                                            <label class="col-sm-4 control-label text-left">
                                                   Product Code
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox runat="server" ID="txtProductCode" MaxLength="20" Columns="20" onfocus="select();"
                                                        CssClass="form-control" placeholder="e.g. B1110535616"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group col-6 col-md-6">
                                            <label class="col-sm-4 control-label text-left">
                                                   Product Name
                                    
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox runat="server" ID="txtProductName" MaxLength="50" Columns="20" onfocus="select();"
                                                    CssClass="form-control" placeholder="e.g. BLUE-TIG 5356"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group col-6 col-md-6">
                                            <label class="col-sm-4 control-label text-left">
                                                   Product Group Code
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox runat="server" ID="txtProductGroup" MaxLength="50" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. B11"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group col-6 col-md-6">
                                            <label class="col-sm-4 control-label text-left">
                                                   Product Group Name
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox runat="server" ID="txtProductGroupName" MaxLength="50" Columns="20" onfocus="select();"
                                                    CssClass="form-control" placeholder="e.g. BLUEMETALS"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group col-6 col-md-6">
                                            <label class="col-sm-4 control-label text-left">
                                                   Ref. Number
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox runat="server" ID="txtInvoiceNumber" MaxLength="20" Columns="20" onfocus="select();" CssClass="form-control" placeholder="e.g. 4456"> </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-primary no-margin">
                                        <div class="panel-heading">
                                            <h4 class="panel-title">
                                                <i class="ti-align-justify"></i>
                                                <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                                            </h4>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false"
                                                GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                                                PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true" OnSortCommand="SortData" AllowSorting="true">
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="No">
                                                        <ItemTemplate>
                                                            <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Trn Date" HeaderStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <%# Eval("TrnDate", "{0:dd/MM/yyyy}")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Ref No" SortExpression="TrnNo" HeaderStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <a href="#" title="Invoice Detail" onclick='ViewInvoice("<%#  Eval("TrnNo")%>","<%# Eval("TrnType")%>","<%# Eval("DBVersion")%>");return false;'><%# Eval("TrnNo")%></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Invoice No" SortExpression="DONo" HeaderStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDONo" runat="server" Width="70px">
																    <%# Eval("DONo")%>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Customer Code" SortExpression="AccountCode" HeaderStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server">
												    <%# Eval("AccountCode")%>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Customer Name" SortExpression="AccountName" HeaderStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server">
												    <%# Eval("AccountName")%>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="DB Version" SortExpression="DBVersion" HeaderStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDBVersion" runat="server">
												    <%# Eval("DBVersion")%>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <HeaderStyle CssClass="tHeader" />
                                                <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                            </asp:DataGrid>
                                        </div>
                            </div>
                                <div class="panel-footer clearfix">
                                    <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-l-10"
                                        OnClick="btnSearch_Click"></asp:Button>
                                     <asp:Button ID="btnThisYear" Text="This Year" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right m-l-10"
                                                OnClick="btnThisYearSales_Click"></asp:Button>
				                    <asp:Button ID="btnPrevMthSales" Text="Previous Month" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right m-l-10"
                                        OnClick="btnPreviousMonthSales_Click"></asp:Button>
				                    <asp:Button ID="btnCurrentMthSales" Text="Current Month" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right m-l-10"
                                        OnClick="btnCurrentMonthSales_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- End : Sale Panel -->
                
                <!-- Start : Collection Panel -->
                <asp:UpdatePanel ID="upCollection" runat="server" UpdateMode="Conditional" Visible="true">
                    <ContentTemplate>
                        <div class="panel panel-primary">
                            <div class="panel-body no-padding">
                                <div class="container-fluid m-t-20">
                                    <div class="form-horizontal">
                                        <div class="row">
                                            <div class="form-group col-lg-6 col-md-6">
                                                <label class="col-sm-4 control-label text-left">
                                                    Receipt Date From
                                                </label>
                                                <div class="col-sm-8">
                                                    <div class="input-group date">
                                                        <asp:TextBox runat="server" ID="txtReceiptFrom" MaxLength="10" Columns="10" onfocus="select();"
                                                            CssClass="form-control"></asp:TextBox>
                                                        <span class="input-group-addon">
                                                            <i class="ti-calendar"></i>
                                                        </span>
                                                    </div>
                                           
                                                </div>
                                            </div>
                                            <div class="form-group col-lg-6 col-md-6">
                                            <label class="col-sm-4 control-label text-left">
                                                Receipt Date To
                                            </label>
                                            <div class="col-sm-8">
                                                <div class="input-group date">
                                                   <asp:TextBox runat="server" ID="txtReceiptTo" MaxLength="10" Columns="10" onfocus="select();"
                                                        CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="ti-calendar"></i>
                                                    </span>
                                                </div>
                                           
                                            </div>
                                        </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group col-lg-6 col-md-6">
                                                <label class="col-sm-4 control-label text-left">
                                                    Receipt No
                                                </label>
                                                <div class="col-sm-8">
                                                   <asp:TextBox runat="server" ID="txtReceiptNo" MaxLength="50" Columns="20" onfocus="select();"
                                                        CssClass="form-control" placeholder="e.g. 123"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group col-lg-6 col-md-6">
                                                <label class="col-sm-4 control-label text-left">
                                                    Invoice No
                                                </label>
                                                <div class="col-sm-8">
                                                   <asp:TextBox runat="server" ID="txtInvoiceNo" MaxLength="20" Columns="20" onfocus="select();"
                                                        CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            
                                <div class="panel panel-primary no-margin">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <i class="ti-align-justify"></i>
                                            <asp:Label ID="lblCollectionSummary" Visible="false" runat="server" />
                                        </h4>
                                    </div>
                                    <div class="table-responsive">
                                            <asp:DataGrid ID="dgCollections" runat="server" AutoGenerateColumns="false"
                                                        GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                                                        PageSize="20" OnPageIndexChanged="dgCollections_PageIndexChanged" EnableViewState="true" OnSortCommand="SortCollections" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateColumn ItemStyle-Width="10px" ItemStyle-Wrap="True">
                                                                <HeaderTemplate>
                                                                    </td>
					    <td width="500" colspan="4" align="center">Receipt</td>
                                                                    <td width="500" colspan="4" align="center" style="background-color: #DADADB">Invoice</td>
                                                                    <td width="100" align="center">&nbsp;</td>

                                                                    </tr>
				    <td align="Center" height="10px" style="background-color: #EDF3FF">&nbsp;</td>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# (Container.ItemIndex + 1) + ((dgCollections.CurrentPageIndex) * dgCollections.PageSize)%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Receipt Date" SortExpression="RECEIPT_TrnDate" >
                                                                <ItemTemplate>
                                                                    <%# Eval("Receipt_TrnDate", "{0:dd/MM/yyyy}")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Ref No." SortExpression="allcdocno" >
                                                                <ItemTemplate>
                                                                    <%# Eval("allcdocno")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Curr" SortExpression="RECEIPT_Currency" >
                                                                <ItemTemplate>
                                                                    <%# Eval("RECEIPT_Currency")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Receipt Amount" SortExpression="Receipt_Amount" >
                                                                <ItemTemplate>
                                                                    <%# string.Format("{0:0,0.00}", Eval("Receipt_Amount"))%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Invoice Date" SortExpression="SALES_TrnDate" >
                                                                <ItemTemplate>
                                                                    <%# Eval("SALES_TrnDate", "{0:dd/MM/yyyy}")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Invoice No" SortExpression="DoNo" HeaderStyle-Wrap="false" >
                                                                <ItemTemplate>
                                                                    <%# Eval("DoNo")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Curr" SortExpression="SALES_Currency" HeaderStyle-Wrap="false">
                                                                <ItemTemplate>
                                                                    <%# Eval("SALES_Currency")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Invoice Amount" SortExpression="Sales_Amount" >
                                                                <ItemTemplate>
                                                                    <%# string.Format("{0:0,0.00}", Eval("Sales_Amount"))%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Days" HeaderStyle-Wrap="false" >
                                                                <ItemTemplate>
                                                                    <%# Eval("DayDiff")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <HeaderStyle CssClass="tHeader" />
                                                        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                                    </asp:DataGrid>
                                    </div>
                                </div>
                            </div>
                               
                            <div class="panel-footer clearfix">
                                <asp:Button ID="btnCollections" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                                    OnClick="btnCollections_Click"></asp:Button>
                                <asp:Button ID="btnThisYearCollections" Text="This Year" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right m-l-10"
                                    OnClick="btnThisYearCollections_Click"></asp:Button>
                                <asp:Button ID="btnPreviousMonthCollections" Text="Previous Month" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right m-l-10"
                                    OnClick="btnPreviousMonthCollections_Click"></asp:Button>
                                <asp:Button ID="btnCurrentMonth" Text="Current Month" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right m-l-10"
                                    OnClick="btnCurrentMonthCollections_Click"></asp:Button>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- End : Collection Panel -->

                <!-- Start : Debts Panel -->
                <asp:UpdatePanel ID="upOutstanding" runat="server" UpdateMode="Conditional" Visible="true">
                    <ContentTemplate>
                       
                                    <asp:Panel ID="pnlOutstandingPaymentProgress" runat="server">
                                        <table width="10%">
                                            <tr align="center">
                                                <td>
                                                    <img src="../../images/dg_loading.gif" /></td>
                                                <td>Loading...</td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                        <div class="panel panel-primary no-margin">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    <i class="ti-align-justify"></i>
                                    <asp:Label ID="lblOutstandingPaymentsSummary" Visible="false" runat="server" />
                                </h4>
                            </div>
                            <div class="panel panel-body no-padding">
                                <div class="table-responsive">
                                    <asp:DataGrid ID="dgOutstanding" runat="server" AutoGenerateColumns="false"
                                        GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                                        PageSize="20" OnPageIndexChanged="dgOutstanding_PageIndexChanged" EnableViewState="true" OnSortCommand="SortOutstanding" AllowSorting="true">
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
                                        <HeaderStyle CssClass="tHeader" />
                                        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                    </asp:DataGrid>
                                </div>
                            </div>
                            <div class="panel-footer clearfix">
                                <asp:Button ID="Greater180DaysOutstanding" Text="> 180 Days" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right m-l-10"
                                    OnClick="btnSearchGreater180DaysOutstanding_Click"></asp:Button>
                                <asp:Button ID="Greater90DaysOutstanding" Text="> 90 Days" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right m-l-10"
                                    OnClick="btnSearchGreater90DaysOutstanding_Click"></asp:Button>
                                <asp:Button ID="btnAllOutstanding" Text="ALL" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right m-l-10"
                                    OnClick="btnSearchAllOutstanding_Click"></asp:Button>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- End : Debts Panel -->
                    
                <!-- Start : Purchase Panel -->
                <asp:UpdatePanel ID="upPurchase" runat="server" UpdateMode="Conditional" Visible="true">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="HiddenForModalAddNewPurchase" Style="display: none" />
                        <div class="modal fade" id="purchase-modal" style="display: none;">
                            <div class="modal-dialog">
	                            <div class="modal-content">
	                                <div class="modal-header">
	                                    <h4 class="modal-title"><asp:Label ID="lblPurchase" runat="server"></asp:Label></h4>           
	                                </div>
		                            <div class="modal-body form-horizontal">
		                                <input type="hidden" id="hidPurchaseID" runat="server" />
		                                <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Supplier
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtSupplier" runat="server" Columns="50" MaxLength="50" CssClass="form-control"
                                                    onfocus="select();" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                    ControlToValidate="txtSupplier" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewPurchase" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                               Industry
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:DropDownList ID="ddlPurchaseIndustry" runat="server" DataTextField="Name"
                                                    DataValueField="IndustryID" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Product Group
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtPG" runat="server" Columns="30" MaxLength="100" CssClass="form-control"
                                                    onfocus="select();" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                    ControlToValidate="txtPG" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewPurchase" />
                                                <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2" ID="AutoCompleteExtender2"
                                                    TargetControlID="txtPG" ServicePath="AutoCompleteProductGroupName.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                                                </ajaxToolkit:AutoCompleteExtender>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                               Product Name
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox ID="txtPN" runat="server" Columns="50" MaxLength="50" CssClass="form-control"
                                                    onfocus="select();" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                        ControlToValidate="txtPN" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewPurchase" />
                                                <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx3" ID="AutoCompleteExtender3"
                                                    TargetControlID="txtPN" ServicePath="AutoCompleteProductName.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                                                </ajaxToolkit:AutoCompleteExtender>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                UOM
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:DropDownList ID="ddlUOM" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                               Quantity
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox ID="txtQuantity" runat="server" Columns="3" MaxLength="3" CssClass="form-control" />
                                                <asp:CompareValidator ID="cvNoOfMonthsBonded" runat="server" ErrorMessage="*"
                                                    Display="Dynamic" ControlToValidate="txtQuantity" Type="Integer" Operator="DataTypeCheck"
                                                    ValidationGroup="valGrpNewRow" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                               Contract End Date
                                            </label>
                                            <div class="col-sm-8">
                                               <div class="input-group date">
                                                   <asp:TextBox runat="server" ID="contractEndDate" MaxLength="10" Columns="10" 
                                                        CssClass="form-control"></asp:TextBox>
                                                   <span class="input-group-addon">
                                                       <i class="ti-calendar"></i>
                                                   </span>
                                               </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label text-left">
                                                Remarks
                                            </label>
                                            <div class="col-sm-8">
                                               <asp:TextBox ID="txtPurchaseRemarks" runat="server" TextMode="MultiLine" Rows="3"
                                                     Columns="40" MaxLength="400" CssClass="form-control" />
                                            </div>
                                        </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                                    <asp:Button CssClass="btn btn-primary" ID="btnAddUpdatePurchase" runat="server" Text="Submit" OnCommand="AddUpdatePurchase" ValidationGroup="valGrpNewPurchase" />
                                </div>
                            </div>
                        </div>
                        </div>
                        <asp:Label ID="lblPurchaseMsg" runat="server"></asp:Label>
                        <div class="panel panel-primary no-margin">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    <i class="ti-align-justify"></i>
                                    <asp:Label ID="lblPurchasesSumary" Visible="false" runat="server" />
                                </h4>
                            </div>
                            <div class="table-responsive">
                                <asp:DataGrid ID="dgPurchases" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                    DataKeyField="PurchaseID" GridLines="none" OnItemDataBound="dgPurchases_ItemDataBound" OnDeleteCommand="dgPurchases_DeleteCommand" OnItemCommand="dgPurchases_Command"
                                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgPurchases_PageIndexChanged" EnableViewState="true" OnSortCommand="SortPurchase" AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="No">
                                            <ItemTemplate>
                                                <%# (Container.ItemIndex + 1) + ((dgPurchases.CurrentPageIndex) * dgPurchases.PageSize)%>
                                                <input type="hidden" id="hidID" runat="server" value='<%# Eval("PurchaseID")%>' />
                                            </ItemTemplate>

                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Supplier" SortExpression="Supplier" HeaderStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplier" runat="server">
                                                    <asp:LinkButton ID="lnkEditPurchase" runat="server" CommandName="EditPurchase" Text='<%# Eval( "Supplier" )%>'
                                                        CausesValidation="false"></asp:LinkButton>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Industry" SortExpression="industryName" HeaderStyle-Wrap="false">
                                            <ItemTemplate>
                                                <%# Eval("industryName")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Product Group" SortExpression="ProductGroup" HeaderStyle-Wrap="false">
                                            <ItemTemplate>
                                                <%# Eval("ProductGroup")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Product Name" SortExpression="ProductName" HeaderStyle-Wrap="false">
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
                                        <asp:TemplateColumn HeaderText="Contract End Date" SortExpression="ContractEndDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContractEndDate" runat="server">
											    <%# Eval("ContractEndDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("ContractEndDate", "{0: dd-MMM-yyyy}")%>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete">
                                    <i class="ti-trash"></i> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <HeaderStyle CssClass="tHeader" />
                                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                </asp:DataGrid>
                            </div>
                            <div class="panel-footer clearfix">
                                <asp:Button ID="btnAddPurchase" Text="Add Purchase" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                                    OnClick="btnAddPurchase_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- End : Purchase Panel -->
                
                <!-- Start : Others Panel -->
                <asp:UpdatePanel ID="upOthers" runat="server" Visible="true">
                    <ContentTemplate>
                        <div class="panel panel-primary">
                            <div class="panel-body">
                        <div class="row flex-container text-center" style="height:400px">
                            <p class="flex-item">Under construction.</p>
                        </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- End : Others Panel -->

          
        </ContentTemplate>
    </asp:UpdatePanel>




    <div class="TABCOMMAND">
        <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".customer-info-menu").addClass("active expand");
            $(".sub-debtors-search").addClass("active");
        });
    </script>
</asp:Content>
