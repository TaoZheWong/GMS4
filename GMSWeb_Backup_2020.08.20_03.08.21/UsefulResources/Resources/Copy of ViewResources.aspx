<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
    Codebehind="ViewResources.aspx.cs" Inherits="GMSWeb.UsefulResources.Resources.ViewResources"
    Title="" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <a name="TemplateInfo"></a>
    <h1><asp:label id="lblPageHeader" runat="server"></asp:label>
        </h1>
    <p>
        Click to view a document. You can also upload a document if you have access for uploading.</p>
        <input id="hidModuleCategoryID" runat="server" type="hidden" />
        <input id="hidModuleCategoryName" runat="server" type="hidden" />
    <table class="tTable1" style="width: 620px; margin-left: 8px">
        <tr valign="top">
            <td style="width: 650px;">
                <asp:Repeater ID="rppCategoryList" runat="server">
                    <HeaderTemplate>
                        <table class="tTable1" width="620px" cellpadding="5" cellspacing="5" border="0">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="tHeader">
                            <td style="padding: 4px;">
                                <a href="javascript:toggleAccessRow(<%# Container.ItemIndex %>);" title="Display/Hide Access Functions">
                                    <img src="<%= Request.ApplicationPath %>/App_Themes/Default/images/checkCloseIcon.gif"
                                        alt="Expand/Hide" name="imgAccessBox_<%# Container.ItemIndex%>" /></a><%# DataBinder.Eval(Container.DataItem, "CategoryName")%>
                            </td>
                        </tr>
                        <tr id="rppToggle_<%# Container.ItemIndex %>">
                            <td>
                                <asp:Repeater ID="rppReportList" runat="server" OnItemCommand="rppReportList_ItemCommand">
                                    <HeaderTemplate>
                                        <table cellpadding="2" cellspacing="1" border="0" width="100%">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="rppRow" runat="server">
                                            <td style="padding-left: 30px; width: 400px">
                                                <%# Container.ItemIndex + 1 %>
                                                . &nbsp; <a href='<%# Request.ApplicationPath + "/Data/Resources/" + Eval("FileName")%>'>
                                                    <%# Eval("DocumentName")%>
                                                    </span></a>
                                            </td>
                                            <td>                                         
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"DocumentID")%>' EnableViewState="true"
                                                    CausesValidation="false" CssClass="DeleteButt" Visible=<%# CanDelete %>
                                                    OnClientClick="return confirm('Are you sure you want to delete this document?')">Delete</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <br />
            </td>
        </tr>
    </table>
    <br />
    <table id="tbUpload" runat="server">
        <tr>
            <td style="width: 15%">
                <asp:Label CssClass="tbLabel" ID="lblYear" runat="server">Category</asp:Label>
            </td>
            <td style="width: 5%">
                :</td>
            <td style="width: 470px">
                <asp:DropDownList CssClass="dropdownlist" ID="ddlDocumentCategory" runat="server"
                    DataTextField="CategoryName" DataValueField="DocumentCategoryID" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label CssClass="tbLabel" ID="lblDocumentName" runat="server">Name</asp:Label></td>
            <td style="width: 5%">
                :</td>
            <td style="width: 470px">
                <asp:TextBox CssClass="textbox" ID="txtDocumentName" runat="server" Width="355px" /></td>
        </tr>
        <tr>
            <td>
                <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">Location</asp:Label></td>
            <td style="width: 5%">
                :</td>
            <td style="width: 470px">
                <asp:FileUpload CssClass="textbox" ID="FileUpload1" runat="server" Width="355px" /></td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
                <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload" CssClass="button"
                    OnClick="btnUpload_Click" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
