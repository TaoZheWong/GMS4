<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="VendorApplicationForm.aspx.cs" Inherits="GMSWeb.Procurement.Forms.VendorApplicationForm" %>
<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register Assembly="SharpPieces.Web.Controls.ExtendedDropDownList" Namespace="SharpPieces.Web.Controls" TagPrefix="piece" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<a name="TemplateInfo"></a>

<ul class="breadcrumb pull-right">
    <li><a href="#">Vendor</a></li>
    <li class="active">Pre-Qualification Form</li>
</ul>
<h1 class="page-header">Procurement<br />
    <small> Vendor Pre-Qualification Form.</small>
</h1>

<asp:ScriptManager ID="scriptMgr" runat="server" />

<div class="panel panel-primary">
    <div class="panel-heading">
         <div class="panel-heading-btn">
            <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
        </div>
        <h4 class="panel-title">
            <i class="ti-search"></i>
            Add/Search filter
        </h4>
    </div>
    <div class="panel-body">
        <div class="form-horizontal form-group-sm row m-t-20">
            <div class="form-group col-lg-4 col-sm-6">
                <label class="col-sm-6 control-label text-left">Vendor Name</label>
                <div class="col-sm-6">
                    <asp:TextBox runat="server" ID="txtNewVendorName" MaxLength="80" Columns="50" onfocus="select();" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="clearfix"></div>
         <%--   <div class="form-group col-lg-4 col-sm-6">
                <label class="col-sm-6 control-label text-left">Date From</label>
                <div class="col-sm-6">
                    <div class="input-group date">
                        <asp:TextBox runat="server" ID="dateFrom" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="datepicker form-control"></asp:TextBox>
						<span class="input-group-addon"><i class="ti-calendar"></i></span>
					</div>
                    
                </div>
            </div>--%>
           <%-- <div class="form-group col-lg-4 col-sm-6">
                <label class="col-sm-6 control-label text-left">Date To</label>
                <div class="col-sm-6">
                    <div class="input-group date">
                        <asp:TextBox runat="server" ID="dateTo" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="datepicker form-control"></asp:TextBox>
                        <span class="input-group-addon"><i class="ti-calendar"></i></span>
                    </div>
                </div>
            </div>--%>
        </div>
    </div>
    <div class="panel-footer clearfix">
        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="pull-right btn btn-primary m-l-5" OnClick="btnSearch_Click"></asp:Button>
        <asp:Button ID="btnAdd" Text="Add" EnableViewState="False" runat="server" CssClass="pull-right btn btn-default" OnClick="btnAdd_Click"></asp:Button>
    </div>
</div>

<div class="panel panel-primary">
     <div class="panel-heading">
        <div class="panel-heading-btn">
            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
        </div>
        <h4 class="panel-title">
            <i class="ti-align-justify"></i> 
            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
        </h4>
    </div>

    <div class="table-responsive">
         <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="False" ShowFooter="True"
        DataKeyField="VendorID" OnPageIndexChanged="dgData_PageIndexChanged" GridLines="None"
        CellPadding="5" Cellspacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="True" PageSize="20"
      >
       <Columns>
            <asp:TemplateColumn HeaderText="No">
                <ItemTemplate>
                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)%>
                    <input type="hidden" id="hidVendorID" runat="server" value='<%# Eval("VendorID")%>' />
                    </ItemTemplate>
            </asp:TemplateColumn>

       <asp:TemplateColumn HeaderText="Vendor Name">
                <ItemTemplate>
                    <asp:Label ID="lblVendorName" runat="server">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "AddEditVendorApplicationForm.aspx?VENDORID="+Eval("VendorID")%>'><%# Eval("CompanyName")%></asp:HyperLink>
                    </asp:Label>
                </ItemTemplate>
                <HeaderStyle Wrap="False" />
            </asp:TemplateColumn>
           <asp:TemplateColumn HeaderText="Email">
                <ItemTemplate>
                    <asp:Label ID="lblEmail" runat="server">
							<%# Eval("Email")%>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
           

       </Columns>
        <HeaderStyle CssClass="tHeader" />
        <FooterStyle CssClass="tFooter" />
        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
         </asp:DataGrid>
    </div>
</div>

    <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".vendor-menu").addClass("active expand");
            $(".sub-Forms").addClass("active");
        });
    </script>
</asp:Content>

