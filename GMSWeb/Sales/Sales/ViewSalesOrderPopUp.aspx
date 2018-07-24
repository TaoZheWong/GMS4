<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewSalesOrderPopUp.aspx.cs" Inherits="GMSWeb.Sales.Sales.ViewSalesOrderPopUp" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View Sales Order Detail</title>
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
        
        <div class="container">
           
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <input type="hidden" id="hidTrnType" runat="server" />
            <input type="hidden" id="hidSOCode" runat="server" />
            <input type="hidden" id="hidDBVersion" runat="server" />

            <ul class="breadcrumb pull-right" style="display:block">
                <li class="active">Invoice Detail</li>
            </ul>

            <div class="panel panel-primary m-t-20">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-info-alt"></i>Invoice Information
                    </h4>
                </div>
                <div class="panel-body row">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">
                                Invoice No.
                            </label>
                            <label class="col-sm-6 control-label text-left">
                                <asp:Label runat="server" ID="lblDONo"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">
                                Ref.
                            </label>
                            <label class="col-sm-6 control-label text-left">
                                <asp:Label runat="server" ID="lblTrnNo"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">
                                Customer Code
                            </label>
                            <label class="col-sm-6 control-label text-left">
                                <asp:Label runat="server" ID="lblAccountCode"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">
                                Customer Name
                            </label>
                            <label class="col-sm-6 control-label text-left">
                                <asp:Label runat="server" ID="lblAccountName"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">
                                Sales Person
                            </label>
                            <label class="col-sm-6 control-label text-left">
                                <asp:Label runat="server" ID="lblSalesPerson"></asp:Label>
                            </label>
                        </div>
                    </div>
                </div>
                <div class="panel-tab">
                    <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0"
                        CssClass="ajax__tab_xp ajax_tab_custom">
                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Information">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="updatePanel1" runat="server">
                                    <ContentTemplate>
                                         <div class="form-horizontal m-t-20">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="form-group col-lg-6 col-sm-6">
                                                        <label class="col-sm-6 control-label text-left">
                                                            Delivery Address
                                                        </label>
                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                                <asp:TextBox runat="server" ID="txtAddress1" MaxLength="40" Columns="20" onfocus="select();"
                                                                    CssClass="form-control" TabIndex="1"></asp:TextBox>
                                                            </div>
                                                            <div class="form-group">
                                                                   <asp:TextBox runat="server" ID="txtAddress2" MaxLength="40" Columns="20" onfocus="select();"
                                                                        CssClass="form-control" TabIndex="2"></asp:TextBox>
                                                            </div>
                                                            <div class="form-group">
                                                                 <asp:TextBox runat="server" ID="txtAddress3" MaxLength="40" Columns="20" onfocus="select();"
                                                                    CssClass="form-control" TabIndex="3"></asp:TextBox>
                                                            </div>
                                                            <div class="form-group">
                                                                <asp:TextBox runat="server" ID="txtAddress4" MaxLength="40" Columns="20" onfocus="select();"
                                                                     CssClass="form-control" TabIndex="4"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-lg-6 col-sm-6">
                                                        <label class="col-sm-6 control-label text-left">
                                                            Attention To
                                                        </label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox runat="server" ID="txtAttentionTo" MaxLength="40" Columns="20" onfocus="select();"
                                                                CssClass="form-control" TabIndex="7"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-lg-6 col-sm-6">
                                                        <label class="col-sm-6 control-label text-left">
                                                           Mobile Phone
                                                        </label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox runat="server" ID="txtMobilePhone" MaxLength="20" Columns="20" onfocus="select();"
                                                                CssClass="form-control" TabIndex="8"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-lg-6 col-sm-6">
                                                        <label class="col-sm-6 control-label text-left">
                                                           Office Phone
                                                        </label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox runat="server" ID="txtOfficePhone" MaxLength="20" Columns="20" onfocus="select();"
                                                                CssClass="form-control" TabIndex="9"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-lg-6 col-sm-6">
                                                        <label class="col-sm-6 control-label text-left">
                                                           Fax
                                                        </label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox runat="server" ID="txtFax" MaxLength="20" Columns="20" onfocus="select();"
                                                                CssClass="form-control" TabIndex="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-lg-6 col-sm-6">
                                                        <label class="col-sm-6 control-label text-left">
                                                           Narration
                                                        </label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox runat="server" ID="txtNarration" MaxLength="15" Columns="20" onfocus="select();"
                                                                CssClass="form-control" TabIndex="5"></asp:TextBox>
                                            
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-lg-6 col-sm-6">
                                                        <label class="col-sm-6 control-label text-left">
                                                           Email
                                                        </label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox runat="server" ID="txtEmail" MaxLength="40" Columns="20" onfocus="select();"
                                                                CssClass="form-control" TabIndex="11"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                         </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Product Item">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="updatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="false" OnItemDataBound="dgData_ItemDataBound"
                                            CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" EnableViewState="true" GridLines="none" Width="100%">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" >
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>'>
                                                        </asp:Label>
                                                        <input type="hidden" id="hidSrNo" runat="server" value='<%# Eval("SrNo")%>' />
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
                                                        <br />
                                                        <asp:Label runat="server" ID="lblProdDetailDesc"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="Unit Price">
                                                    <ItemTemplate>
                                                        <%# Eval("Currency")%>
                                                        <asp:Label runat="server" ID="lblUnitPrice" Text='<%# Eval("UnitPrice", "{0:f2}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Qty" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblOrderQuantity" Text='<%# Eval("Quantity")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="UOM">
                                                    <ItemTemplate>
                                                        <%# Eval("uom")%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Discount">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblDiscount" Text='<%# Eval("Discount")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <%# Eval("Currency")%>
                                                        <asp:Label runat="server" ID="lblAmount"></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader"/>
                                        </asp:DataGrid>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                    </ajaxToolkit:TabContainer>
                </div>
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
