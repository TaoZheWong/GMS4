<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesDetail.aspx.cs" Inherits="GMSWeb.Sales.Sales.SalesDetail" Title="Sales - Detail" %>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <title>Sales - Detail</title>
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
            <input type="hidden" id="hidTrnNo" runat="server" />
            <input type="hidden" id="hidTrnType" runat="server" />
            <input type="hidden" id="hidSrNo" runat="server" />
            <input type="hidden" id="hidProductCode" runat="server" />
            <input type="hidden" id="hidAccountCode" runat="server" />

            <div class="panel panel-primary m-t-20">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <i class="ti-bar-chart"></i>
                        Sale Detail
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="row">
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Trn Date
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblTrnDate"></asp:Label>
                                </label>
                            </div>
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Ref No.
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblRefNo"></asp:Label>
                                </label>
                            </div>
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Product Code
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblProductCode"></asp:Label>
                                </label>
                            </div>
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Product Name
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblProductName"></asp:Label>
                                </label>
                            </div>
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Product Group (Brand)
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblProductGroup"></asp:Label>
                                </label>
                            </div>
                        </div>
                        <div id="PMRegion1" runat="server" class="row">

                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Unit Price
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblUnitPrice"></asp:Label>
                                </label>
                            </div>
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Unit Cost
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblUnitCost"></asp:Label>
                                </label>
                            </div>
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Quantity
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblQty"></asp:Label>
                                </label>
                            </div>
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Total Sales
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblTotalSales"></asp:Label>
                                </label>
                            </div>
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    Total Cost
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblTotalCost"></asp:Label>
                                </label>
                            </div>
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    GP%
                                </label>
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label runat="server" ID="lblGP"></asp:Label>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
