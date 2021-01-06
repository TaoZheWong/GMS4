<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="AddEditPurchase.aspx.cs" Inherits="GMSWeb.Debtors.Debtors.AddEditPurchase" Title="Customer Info - Add/Edit Purchase" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>
<h1>Customer &gt; Add/Edit Customer Purchase</h1>
<p>Create new purchase or edit existing customers' purchase.</p>
<asp:ScriptManager ID="sriptmgr1" runat="server">
<Services>
    <asp:ServiceReference Path="AutoCompleteProductGroupName.asmx" />
    <asp:ServiceReference Path="AutoCompleteProductName.asmx" />
</Services>    
</asp:ScriptManager>
<p>
<table class="tTable" style="border-collapse: collapse" cellspacing="5" cellpadding="5"
border="0" width="70%">
<tr>
    <td class="tbLabel" style="width:15%">
        Supplier</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtSupplier" runat="server" Columns="50" MaxLength="50" CssClass="textbox"
            onfocus="select();" onchange="this.value = this.value.toUpperCase()" AutoPostBack="true"
            /><asp:RequiredFieldValidator ID="rfvCourseTitle" runat="server"
                ControlToValidate="txtSupplier" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
       
         <input type="hidden" id="hidPurchaseID" runat="server" />    
         <input type="hidden" id="hidAccountCode" runat="server" />    
    </td>
</tr>

<tr>
    <td class="tbLabel" style="width:15%">
        Industry</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:DropDownList ID="ddlIndustry" runat="server" DataTextField="Name"
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
        <asp:TextBox ID="txtProductGroup" runat="server" Columns="30" MaxLength="100" CssClass="textbox"
            onfocus="select();" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                ControlToValidate="txtProductGroup" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
            TargetControlID="txtProductGroup" ServicePath="AutoCompleteProductGroupName.asmx"
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
        <asp:TextBox ID="txtProductName" runat="server" Columns="50" MaxLength="50" CssClass="textbox"
            onfocus="select();" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                ControlToValidate="txtProductName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2" ID="AutoCompleteExtender2"
            TargetControlID="txtProductName" ServicePath="AutoCompleteProductName.asmx"
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
                        <img id="imgCalendarEditFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"

                            height="20" width="17" alt="" align="absMiddle" border="0"></td>
</tr>
<tr>
    <td class="tbLabel" style="width:15%">
        Remarks</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3"
                Columns="40" MaxLength="400" CssClass="textarea" /></td>
</tr>
<tr>
    <td></td><td></td>
    <td style="width: 10%">
        <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="button"
            ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>
            
    </td>
</tr>
</table></p>
<div class="TABCOMMAND">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <ul>
                <li></li>
            </ul>
            <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
