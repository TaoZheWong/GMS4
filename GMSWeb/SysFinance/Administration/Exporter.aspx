<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Exporter.aspx.cs" Inherits="GMSWeb.SysFinance.Administration.Exporter" Title="Group Finance - Exporter Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <ul class="breadcrumb pull-right">
        <li><a href="#">Administration </a></li>
        <li class="active">Exporter</li>
    </ul>
    <h1 class="page-header">Exporter <br />
        <small>Export data from GMS for importing into DIVA system.</small>
    </h1>
    <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
    <p>1. Export Trial Balance and InterCompany Transactions</p>
    <p></p>
    
    <div class="panel panel-primary">
        <div class="panel-heading">
		    <h4 class="panel-title"> <i class="ti-search"></i> Select the companies from the below list to export its trial balance data into tab-separated txt file.</h4>
	    </div>
        <div class="panel-body">
            <div class="form-horizontal row m-t-20">
		        <div class="form-group col-lg-4 col-sm-6">
			        <label class="col-sm-6 control-label text-left">
			            <asp:Label CssClass="tbLabel" ID="lblYear" runat="server">Year</asp:Label>
			        </label>
			        <div class="col-sm-6">
				        <asp:DropDownList CssClass="form-control" ID="ddlYear" runat="server" DataTextField="Year" DataValueField="Year" />
			        </div>
		        </div>
          
		        <div class="form-group col-lg-4 col-sm-6">
			        <label class="col-sm-6 control-label text-left">
			            <asp:Label CssClass="tbLabel" ID="lblMonth" runat="server">Month</asp:Label>
			        </label>
			        <div class="col-sm-6">
				        <asp:DropDownList CssClass="form-control" ID="ddlMonth" runat="server" DataTextField="Month" DataValueField="Month" />
			        </div>
		        </div>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <asp:Button ID="btnExport" runat="server" CausesValidation="true" Text="Export" CssClass="btn btn-primary pull-right" OnClick="btnExport_Click" />
        </div>
    </div>

     <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
		    <h4 class="panel-title"> <i class="ti-align-justify"></i> Company List</h4>
	    </div>
        <div class="table-responsive">
             <table class="table table-hover table-condensed table-striped table-responsive"> 
            <asp:Repeater ID="rppCompany" runat="server">
                <HeaderTemplate>
                    <tr class="tHeader">
                        <td align="center">SN</td>
                        <td align="center">Code</td>
                        <td align="left">Company</td>
                        <td >
                            <div class="checkbox">
                            <input type="checkbox" id="checkedAll" name="checkedAll" title="Select All" /> 
                             <label for="checkedAll"></label>
                            </div> 
                            
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="rppRow" runat="server">
                        <td align="center"><small><%# Container.ItemIndex + 1 %></small></td>
                        <td align="center"><small><%# DataBinder.Eval(Container.DataItem,"Code")%></small></td>
                        <td>
                            <input type="hidden" name="hidCoyId" id="hidCoyId" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"CoyID")%>' />
                            <input type="hidden" name="hidName" id="hidName" runat="server" value="Company List" />
                            <small>
                                <%# DataBinder.Eval(Container.DataItem,"Name")%>
                            </small>
                        </td>
                        <td>
                            <div class="checkbox">
                                <input type="checkbox" id="CoyID_<%# Container.ItemIndex + 1 %>" name="CoyID" class="checkSingle" value='<%# DataBinder.Eval(Container.DataItem,"CoyID")%>'
                                    title='<%# DataBinder.Eval(Container.DataItem,"Name")%>' />
                                <label for="CoyID_<%# Container.ItemIndex + 1 %>"></label>
                             </div>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        </div>
    </div>
   

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
            $(".sub-exporter").addClass("active");
        });
    </script>
</asp:Content>