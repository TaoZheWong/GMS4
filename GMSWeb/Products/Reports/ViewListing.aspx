<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ViewListing.aspx.cs" Inherits="GMSWeb.Reports.ViewListing" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <ul class="breadcrumb pull-right">
        <li><a href="#">Listing</a></li>
        <li class="active">Product</li>
    </ul>
    <h1 class="page-header">Listing <br /><small>Click a report to view.</small></h1>


    <asp:Repeater ID="rppCategoryList" runat="server">
        <ItemTemplate>
            <div class="panel-group">
                <div class="panel panel-default panel-bordered">
                    <div class="panel-heading" id="heading">
                        <h4 class="panel-title">
                            <a href="#collapseReport_<%# Container.ItemIndex %>" class="accordion-link collapsed" data-toggle="collapse" data-parent="#accordion" aria-expanded="false">
                                <%# DataBinder.Eval(Container.DataItem,"Name")%>
                            </a>
                        </h4>
                    </div>
                </div>
                <div id="collapseReport_<%# Container.ItemIndex %>" class="panel-collapse collapse in" aria-expanded="false">
                    <ul class="list-group">
                        <asp:Repeater ID="rppReportList" runat="server" OnItemCommand="rppReportList_ItemCommand">
                            <ItemTemplate>
                                <li class="list-group-item" runat="server">
                                    <%# Container.ItemIndex + 1 %>. &nbsp;
													<asp:LinkButton ID="lnkPrintReport" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ReportId")%>' EnableViewState="false"
                                                        CausesValidation="false"><span><%# Eval("Description")%></span></asp:LinkButton>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </ItemTemplate>

    </asp:Repeater>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            $(".administration-menu").addClass("active expand");
            $(".sub-listing").addClass("active");}
        );
    </script>
</asp:Content>

