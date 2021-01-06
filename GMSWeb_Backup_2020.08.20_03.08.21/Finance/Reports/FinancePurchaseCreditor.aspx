<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="FinancePurchaseCreditor.aspx.cs" Inherits="GMSWeb.Finance.Reports.FinancePurchaseCreditor" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<a name="TemplateInfo"></a>

<ul class="breadcrumb pull-right">
    <li class="active">Purchase & Creditors Reports</li>
</ul>
<h1 class="page-header">Reports <small>Click a report to view.</small></h1>
      
<asp:Repeater id="rppCategoryList" Runat="server">
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
				        <asp:repeater id="rppReportList" runat="server" OnItemCommand="rppReportList_ItemCommand">
					        <itemtemplate>
						        <li class="list-group-item" runat="server">
									<%# Container.ItemIndex + 1 %>. &nbsp;
									<asp:LinkButton ID="lnkPrintReport" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ReportId")%>' EnableViewState="false"
									CausesValidation="false"><span><%# Eval("Description")%></span></asp:LinkButton>
						        </li>
					        </itemtemplate>
				        </asp:repeater>
				    </ul>
                </div>
            </div>
	</ItemTemplate>
</asp:Repeater>
			    
			 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".purchase-menu").addClass("active expand");
            $(".sub-purchase").addClass("active");
        });
    </script>
</asp:Content>
