<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="BankReport.aspx.cs" Inherits="GMSWeb.SysFinance.Reports.BankReport" Title="Finance - Report Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<a name="TemplateInfo"></a>

<ul class="breadcrumb pull-right">
    <li><a href="#">Bank Facilities</a></li>
    <li class="active">Bank Reports</li>
</ul>
    <h1 class="page-header">Reports <br /> <small>Click a report to view.</small></h1>


<asp:Repeater id="rppCategoryList" Runat="server">
			<ItemTemplate>
				<div class="panel-group">
					<div class="panel panel-default panel-bordered">
						<div class="panel-heading" id="headingTwo">
							<h4 class="panel-title">
								<a href="#collapse_<%# Container.ItemIndex %>" class="accordion-link" data-toggle="collapse" data-parent="#accordion" aria-expanded="true">
									<%# DataBinder.Eval(Container.DataItem,"Name")%>
								</a>
							</h4>
						</div>
						<div id="collapse_<%# Container.ItemIndex %>" class="panel-collapse collapse in" aria-expanded="true">
							
                      
					
						    <asp:repeater id="rppReportList" runat="server" OnItemCommand="rppReportList_ItemCommand">
							    <headertemplate>
								    <ul class="list-group">
							    </headertemplate>
							    <itemtemplate>
								    <li id="Li1" class="list-group-item" runat="server">
											    <%# Container.ItemIndex + 1 %>. &nbsp;
											    <asp:LinkButton ID="lnkPrintReport" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ReportId")%>' EnableViewState="false"
										        CausesValidation="false"><span><%# Eval("Description")%></span></asp:LinkButton>
									  </li>
							    </itemtemplate>
							    <footertemplate>
						            </ul>
					            </footertemplate>
				            </asp:repeater>
			            </div>
					</div>
				</div>
		    </ItemTemplate>
		</asp:Repeater>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".reports-menu").addClass("active expand");
            $(".sub-bank-report").addClass("active");
        });
    </script>
</asp:Content>