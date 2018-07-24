<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="GMSWeb.Sales.Sales.Import" Title="Import" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="breadcrumb pull-right">
        <li><a href="#">Administration</a></li>
        <li class="active">Pricelist</li>
    </ul>
    <h1 class="page-header">Pricelist <small>Non A21 can upload pricelist his/her products using Excel file. See the following for sample Excel file format. </small></h1>

    <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <table class="tTable1" style="margin-left: 8px">
        <tr>
            <td>
                <div>
                    <table cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td colspan="2"><a href="<%= Request.ApplicationPath %>/Documents/Customerlist.xls">Customerlist.xls</a>
                            </td>
                        </tr>


                        <tr>
                            <td>
                                <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">Location</asp:Label></td>
                            <td style="width: 50px">:</td>
                            <td align="left" style="width: 500px">
                                <asp:FileUpload CssClass="textbox" ID="FileUpload1" runat="server" Width="355px" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload" CssClass="button" OnClick="btnUpload_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td style="width: 50px"></td>
                            <td colspan="2">
                                <iframe id="IFrame1" frameborder="0" scrolling="YES" runat="Server" width="100%" style="display: none"></iframe>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td colspan="2">
                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
       
        </table>
        <div class="TABCOMMAND">
            <atlas:UpdatePanel ID="udpMsgUpdater" runat="server" Mode="Always">
                <ContentTemplate>
                    <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                </ContentTemplate>
            </atlas:UpdatePanel>
        </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".administration-menu").addClass("active expand");
            $(".sub-importer").addClass("active");
        });
    </script>
</asp:Content>
