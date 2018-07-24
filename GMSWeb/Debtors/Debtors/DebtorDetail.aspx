<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="DebtorDetail.aspx.cs" Inherits="GMSWeb.Debtors.Debtors.DebtorDetail" Title="Debtors - Debtor Detail" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<h1>Customer Info &gt; Customer Detail</h1>
            <p>View Customer Details.</p>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <input type="hidden" id="hidAccountCode" runat="server" />
            <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0" width="650px">
                <tr>
                    <td class="tbLabel">
                        Customer Code</td>
                    <td style="width:5%">
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblAccountCode"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Customer Name</td>
                    <td style="width:5%">
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblAccountName"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Sales Person</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblSalesPerson"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Credit Term</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblCreditTerm"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Credit Limit</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblCreditLimit"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Default Currency</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblDefaultCurrency"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Industry</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblIndustry"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Address1</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblAddress1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Address2</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblAddress2"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Address3</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblAddress3"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Address4</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblAddress4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Postal Code</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblPostalCode"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Contact Person</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblContactPerson"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Office Phone</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblOfficePhone"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Mobile Phone</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblMobilePhone"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Fax</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblFax"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Email</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblEmail"></asp:Label>
                    </td>
                </tr>
            </table>
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
