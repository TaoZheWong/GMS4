<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"  CodeBehind="AddProspect.aspx.cs" Inherits="GMSWeb.Debtors.Debtors.AddProspect" Title="Debtor - Add Prospect Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>
<h1>Debtor &gt; Add Prospect</h1>
<asp:ScriptManager ID="sriptmgr1" runat="server">
</asp:ScriptManager>
<p>
<table class="tTable" style="border-collapse: collapse" cellspacing="5" cellpadding="5"
    border="0" width="500px">
    <tr>
        <td class="tbLabel">
            Customer Name</td>
        <td>
            :</td>
        <td colspan="2">
            <asp:TextBox ID="txtCustomerName" runat="server" Columns="50" MaxLength="50" CssClass="textbox"
                onfocus="select();" onchange="this.value = this.value.toUpperCase()" /><asp:RequiredFieldValidator ID="rfvCourseTitle" runat="server" ControlToValidate="txtCustomerName"
                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
        </td>
    </tr>
    <tr>
        <td class="tbLabel">
            Salesman</td>
        <td style="width: 5%">
            :</td>
        <td colspan="2">
            <asp:DropDownList ID="ddlSalesman" runat="Server" DataTextField="SalesPersonName"
                            DataValueField="SalesPersonID" CssClass="dropdownlist" />            
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
        <td style="width: 5%">
            :</td>
        <td colspan="2">
            <asp:DropDownList CssClass="dropdownlist" ID="ddlIndustry" runat="Server" DataTextField="Name" DataValueField="Name" />          
        </td>
    </tr>
    
    <tr>
        <td class="tbLabel">
            Territory</td>
        <td style="width: 5%">
            :</td>
        <td colspan="2">
            <asp:DropDownList CssClass="dropdownlist" ID="ddlCountry" runat="Server" DataTextField="TerritoryName" DataValueField="TerritoryName" />          
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
                ValidationGroup="valGrpNewRow" /></td>
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
                ValidationGroup="valGrpNewRow" /></td>
    </tr>
    <tr>
        <td class="tbLabel">
            Address</td>
        <td>
            :</td>
        <td colspan="2">                  
            <asp:TextBox runat="server" ID="txtAddress1" MaxLength="40" Columns="40" onfocus="select();"
                                            CssClass="textbox" TabIndex="1"></asp:TextBox>
            </td>
    </tr>    
    <tr>
        <td class="tbLabel">
           </td>
        <td>
            </td>
        <td colspan="2">                  
            <asp:TextBox runat="server" ID="txtAddress2" MaxLength="40" Columns="40" onfocus="select();"
                                            CssClass="textbox" TabIndex="2"></asp:TextBox>
            </td>
    </tr>
    <tr>
        <td class="tbLabel">
           </td>
        <td>
            </td>
        <td colspan="2">                  
            <asp:TextBox runat="server" ID="txtAddress3" MaxLength="40" Columns="40" onfocus="select();"
                                            CssClass="textbox" TabIndex="2"></asp:TextBox>
            </td>
    </tr>
    <tr>
        <td class="tbLabel">
           </td>
        <td>
            </td>
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
                CssClass="textbox" /></td>
    </tr>
    
    <tr><td></td><td></td>
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