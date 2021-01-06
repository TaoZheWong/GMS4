<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPO.aspx.cs" Inherits="GMSWeb.Products.Products.ViewPO" Title="MR - View PO" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>View POr Detail</title>
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-timepicker/bootstrap-timepicker.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite_app.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <style>
        body {
            overflow: auto;
        }
    </style>
</head>
<body class="<%=getIsLargeFont%> <%=getIsOptimizedTable%>">
    <form id="form1" runat="server">
        <div class="container m-t-20">
            <ul class="breadcrumb pull-right">
                <li><a href="#">Products</a></li>
                <li class="active">PO Detail</li>
            </ul>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <input type="hidden" id="hidPOCode" runat="server" />

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        PO Information
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="row m-t-20">
                        <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-6">
                            <label class="control-label text-left"> PO No.</label>
                            <label class="form-control">
                                <asp:Label runat="server" ID="lblPONo"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-6">
                            <label class="control-label text-left"> PO Date</label>
                            <label class="form-control">
                                <asp:Label runat="server" ID="lblPODate"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-6">
                            <label class="control-label text-left"> Ref No.</label>
                            <label class="form-control">
                                <asp:Label runat="server" ID="lblTrnNo"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-6">
                            <label class="control-label text-left">
                                Purchaser
                            </label>
                            <label class="form-control">
                                <asp:Label runat="server" ID="lblPurchaser"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-6">
                            <label class="control-label text-left">
                                Supplier
                            </label>
                            <label class="form-control">
                                <asp:Label runat="server" ID="lblSupplier"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-6">
                            <label class="control-label text-left">
                                Terms
                            </label>
                            <label class="form-control">
                                <asp:Label runat="server" ID="lblTerms"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-6">
                            <label class="control-label text-left">
                                Supplier Name
                            </label>
                            <label class="form-control">
                                <asp:Label runat="server" ID="lblSupplierName"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-6">
                            <label class="control-label text-left">
                                Supplier Address 
                            </label>
                            <label class="form-control">
                                <asp:Label runat="server" ID="lblAdd1"></asp:Label>
                            </label>
                            <label class="form-control">
                                <asp:Label runat="server" ID="lblAdd2"></asp:Label>
                            </label>
                            <label class="form-control">
                                <asp:Label runat="server" ID="lblAdd3"></asp:Label>
                            </label>
                        </div>
                    </div>
                </div>
                <div class="panel-tab">
                    <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Width="100%"
                    CssClass="ajax__tab_xp ajax_tab_custom">

                    <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Product Item">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="updatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="false" OnItemDataBound="dgData_ItemDataBound"
                                        CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" EnableViewState="true" GridLines="none" Width="100%">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Product Code">
                                                <ItemTemplate>
                                                    <%# Eval("ProductCode")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Description">
                                                <ItemTemplate>
                                                    <%# Eval("ProductDescription")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="UOM">
                                                <ItemTemplate>
                                                    <%# Eval("uom")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Qty">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblOrderQuantity" Text='<%# Eval("Quantity")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Unit Price">
                                                <ItemTemplate>
                                                    <%# Eval("Currency")%>
                                                    <asp:Label runat="server" ID="lblUnitPrice" Text='<%# Eval("UnitPrice", "{0:f2}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Amount">
                                                <ItemTemplate>
                                                    <%# Eval("Currency")%>
                                                    <asp:Label runat="server" ID="lblAmount"><%# Convert.ToDouble(Eval("UnitPrice", "{0:f2}")) * Convert.ToDouble(Eval("Quantity", "{0:f2}")) %></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle CssClass="tHeader" />
                                    </asp:DataGrid>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
           
                </div>
                <br />
            </div>




            <asp:UpdatePanel ID="updatePanel6" runat="server">
                <ContentTemplate>
                    <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                    <p>
                        <asp:Label runat="server" ID="lblCreatedBy"></asp:Label>
                    </p>
                    <p>
                        <asp:Label runat="server" ID="lblModifiedBy"></asp:Label>
                    </p>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                    <div class="well">
                        <div class="row">
                            <div class="col-md-8 col-sm-8 text-right">Sub-Total : </div>
                            <div class="col-md-2 col-sm-2 text-right">
                                 <asp:Label runat="server" ID="lblCurrency" Text="SGD" /></td>
                            </div>
                            <div class="col-md-2 col-sm-2 text-right">
                                 <asp:Label runat="server" ID="lblSubTotal" /></td>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-sm-4"></div>
                            <div class="col-md-4 col-sm-4 text-right">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-addon">GST</span>
                                    <asp:TextBox runat="server" ID="txtTaxRate" Columns="6" CssClass="form-control text-right" contentEditable="false" />
                                </div>
                            </div>
                            <div class="col-md-2 col-sm-2 text-right p-t-4">
                                <asp:Label runat="server" ID="lblCurrency2" Text="SGD" />
                            </div>
                            <div class="col-md-2 col-sm-2 text-right p-t-4">
                                 <asp:Label runat="server" ID="lblTaxAmount" /></td>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-8 col-sm-8 text-right p-t-4">
                                Grand Total :
                            </div>
                            <div class="col-md-2 col-sm-2 text-right p-t-4">
                                <asp:Label runat="server" ID="lblCurrency3" Text="SGD" />
                            </div>
                            <div class="col-md-2 col-sm-2 text-right p-t-4">
                                 <asp:Label runat="server" ID="lblGrandTotal" />
                            </div>
                        </div>


                    </div>
                </div>
            </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="TABCOMMAND">
            </div>
        </div>
        <br />
    </form>
</body>
</html>
