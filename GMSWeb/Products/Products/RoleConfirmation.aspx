<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="RoleConfirmation.aspx.cs" Inherits="GMSWeb.Products.Products.RoleConfirmation" Title="Role Confirmation" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<h1>Material Requisition &gt; Confirmation</h1>
           
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0" width="99%">
                <tr>
                <td>
                    
                You appear to have two roles in the system, i.e a Product Manager and a Salesman. Please select one of the following options that is applicable in this application.<br /><br />
                </td>
                </tr>
                <tr>
                <td>
                
                <asp:RadioButton ID="radPM" runat="server" Text="Purchase products that are under my care, i.e as a Product Manager." Checked="true" GroupName="PurchaseGroup" />
                <br />
                <asp:RadioButton ID="radSales" runat="server" Text="Purchase products for confirmed sales and these products are not under my care, i.e as a Salesman." GroupName="PurchaseGroup" />
                
                </td>
                </tr>
                <tr>
                <td>
                <asp:Button ID="btnNext" Text="Next" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnNext_Click" ></asp:Button>
                </td>
                </tr>                
            </table>    
</asp:Content>