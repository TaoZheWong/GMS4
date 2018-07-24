<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="QuotationSearch.aspx.cs" Inherits="GMSWeb.Sales.Sales.QuotationSearch" Title="Quotation Search" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>
 <ul class="breadcrumb pull-right">
    <li><a href="#">Sales</a></li>
    <li class="active">Quotation</li>
</ul>
<h1 class="page-header">Quotation <br />
     <small>Create new customer quotation or search for existing customers' quotations.</small></h1>

<asp:ScriptManager ID="scriptMgr" runat="server" />
    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-search"></i>
                Search filter
            </h4>
        </div>
        <div class="panel-body">
            <div class="m-t-20">
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Quotation Date From</label>
                        <div class="input-group date">
                            <asp:TextBox runat="server" ID="trnDateFrom" MaxLength="10" Columns="10" onfocus="select();"
                                CssClass="form-control datepicker"></asp:TextBox>
						    <span class="input-group-addon"><i class="ti-calendar"></i></span>
					    </div>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Quotation Date To</label>
                        <div class="input-group date">
                            <asp:TextBox runat="server" ID="trnDateTo" MaxLength="10" Columns="10" onfocus="select();"
                                CssClass="form-control datepicker"></asp:TextBox>
						    <span class="input-group-addon"><i class="ti-calendar"></i></span>
					    </div>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Customer Code</label>
                         <asp:TextBox runat="server" ID="txtCustomerAccountCode" MaxLength="50" Columns="20"
                                onfocus="select();" CssClass="form-control" placeholder="e.g. DLK690"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Customer Name</label>
                        <asp:TextBox runat="server" ID="txtCustomerAccountName" MaxLength="50" Columns="20"
                            onfocus="select();" CssClass="form-control" placeholder="e.g. Keppel"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Product Code</label>
                        <asp:TextBox runat="server" ID="txtProductCode" MaxLength="50" Columns="20"
                            onfocus="select();" CssClass="form-control" placeholder="e.g. B1110535616"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Product Name</label>
                        <asp:TextBox runat="server" ID="txtProductName" MaxLength="50" Columns="20"
                            onfocus="select();" CssClass="form-control" placeholder="e.g. BLUE-TIG 5356"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Salesman</label>
                        <asp:DropDownList ID="ddlSalesman" runat="Server" DataTextField="SalesPersonName"
                            DataValueField="SalesPersonID" CssClass="form-control" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Quotation No.</label>
                        <asp:TextBox runat="server" ID="txtQuotationNo" MaxLength="50" Columns="20"
                            onfocus="select();" CssClass="form-control" placeholder="e.g. 13-0001"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Acknowledge</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlAcknowledge" runat="server">
                            <asp:ListItem Value="%%">ALL</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label">Status</label>
                    <asp:DropDownList ID="ddlQuotationStatus" runat="Server" DataTextField="QuotationStatusName"
                        DataValueField="QuotationStatusID" CssClass="form-control" />
                </div>
                </div>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                OnClick="btnSearch_Click"></asp:Button>&nbsp;<asp:Button ID="btnAdd" Text="Add" EnableViewState="False"
                    runat="server" CssClass="btn btn-default pull-right m-r-10" OnClick="btnAdd_Click"></asp:Button>
        </div>
    </div>
    <div class="panel panel-primary" id="resultList" runat="server" visible="false">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i> 
                <asp:Label ID="lblSearchSummary" Visible="false" runat="server"></asp:Label>
            </h4>
        </div>
        <div class="table-responsive">
            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="False" 
        OnPageIndexChanged="dgData_PageIndexChanged" GridLines="None" AllowSorting="true" OnSortCommand="SortData"
        CellPadding="5" Cellspacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="True" PageSize="20"
        >
        <Columns>
                    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                        <ItemTemplate>
                            <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Quotation No." SortExpression="QuotationNo" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" Target="_self" NavigateUrl='<%# "AddEditQuotation.aspx?QuotationNo="+Eval("QuotationNo")%>'><%# Eval("QuotationNo")%></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Quotation Date" SortExpression="QuotationDate" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("QuotationDate", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                            
                    <asp:TemplateColumn HeaderText="Quotation Status" SortExpression="QuotationStatusName" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("QuotationStatusName")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Customer Name" SortExpression="CustomerName" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                                <%# Eval("CustomerName")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Sales Person" SortExpression="SalesPersonName" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("SalesPersonName")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Grand Total" SortExpression="GrandTotal" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("GrandTotal", "{0:f2}")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Is Acknowledge" SortExpression="IsAcknowledge" HeaderStyle-Wrap="false">
                        <ItemTemplate>                                    
                            <%# ( (bool)Eval( "IsAcknowledge" ) ) ? "Yes" : "No"%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
        </Columns>
        <HeaderStyle CssClass="tHeader" />
        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
    </asp:DataGrid>
        </div>
    </div>

<div class="TABCOMMAND">
<asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
    </ContentTemplate>
</asp:UpdatePanel>
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".sale-menu").addClass("active expand");
            $(".sub-quotation").addClass("active");
        });
    </script>
</asp:Content>