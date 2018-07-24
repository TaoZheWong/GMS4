<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="EditProject.aspx.cs" Inherits="GMSWeb.Sales.Engineering.Project.EditProject" Title="Project Information" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <style type="text/css">
        .ui-autocomplete {
            max-height: 150px;
            overflow-y: auto;
            /* prevent horizontal scrollbar */
            overflow-x: hidden;
        }
        /* IE 6 doesn't support max-height
    * we use height instead, but this forces the menu to always be this tall
    */
        * html .ui-autocomplete {
            height: 200px;
        }
    </style>

    <ul class="breadcrumb pull-right">
        <li><a href="#">Project</a></li>
        <li class="active">Project</li>
    </ul>

    <h1 class="page-header">Edit Project</h1>


    <div class="panel panel-primary m-b-20">
        <div class="panel-heading">
            <h1 class="panel-title">Project Information</h1>
        </div>
        <div id="FormSearch" class="panel-body">
            <input type="hidden" id="hidCoyID" runat="server" value="" />
            <input type="hidden" id="hidUserID" runat="server" value="" />
            <input type="hidden" id="hidCurrentLink" runat="server" value="" />
            <div class="row m-t-20">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtProjectNo">Project No:</label>
                        <input type="text" class="form-control" id="txtProjectNo" readonly="readonly" />
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtPrevProjectNo">Previous Project No:</label>
                        <input type="text" class="form-control" id="txtPrevProjectNo" tabindex="1" />
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtCEID">C/E No:</label>
                        <input type="text" class="form-control" id="txtCEID" readonly="readonly" />
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtCEID">C/E Status:</label>
                        <input type="text" class="form-control" id="txtCEStatusName" readonly="readonly" />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtQuotationNo">Quotation No:</label>
                        <input type="text" class="form-control" id="txtQuotationNo" readonly="readonly" />
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtAccountCode">Customer Code:</label>
                        <input type="text" class="form-control" id="txtAccountCode" tabindex="2" />
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtAccountName">Customer Name:</label>
                        <input type="text" class="form-control" id="txtAccountName" readonly="readonly" />
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtStatusID">Status:</label>
                        <select class="form-control" id="txtStatusID" tabindex="4">
                            <option value="1">New</option>
                            <option value="2">Ongoing</option>
                            <option value="3">Completed</option>
                            <option value="4">Closed</option>
                            <option value="X">Cancelled</option>
                        </select>
                    </div>
                </div>


            </div>

            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtSalesPersonID">Sales Person: </label>
                        <input type="text" class="form-control" id="txtSalesPersonName" tabindex="5" />
                        <input type="hidden" class="form-control" id="txtSalesPersonID" />
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtEngineerID">Engineer: </label>
                        <input type="text" class="form-control" id="txtEngineerName" tabindex="6" />
                        <input type="hidden" class="form-control" id="txtEngineerID" />
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtIsBillable">Billable:</label>
                        <select type="text" class="form-control" id="txtIsBillable" tabindex="7">
                            <option value="True">Yes</option>
                            <option value="False">No</option>
                        </select>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtIsProgressiveClaim">Progressive Claim:</label>
                        <select class="form-control" id="txtIsProgressiveClaim" tabindex="8">
                            <option value="True">Yes</option>
                            <option value="False">No</option>
                        </select>
                    </div>
                </div>

            </div>
            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txtRefNo">Ref No:</label>
                        <input type="text" class="form-control" id="txtRefNo" tabindex="3" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Tab v2 -->

        <div class=" panel-tab tab-v2">
            <ul class="nav nav-tabs">
                <li class="active"><a href="#tab1" data-toggle="tab">General</a></li>
                <li id="CE"><a href="#tab2" data-toggle="tab">Cost Estimate</a></li>
                <li id="PT"><a href="#tab3" data-toggle="tab">Project Costing</a></li>
                <%--                <li><a href="#tab4" data-toggle="tab">Maintenance</a></li>--%>
                <li id="Inv"><a href="#tab5" data-toggle="tab">Invoice</a></li>
                <li><a href="#tab6" data-toggle="tab">Attachment</a></li>
            </ul>
            <div class="tab-content">
                <div class="tab-pane fade in active" id="tab1">
                </div>
                <div class="tab-pane fade in" id="tab2"></div>
                <div class="tab-pane fade in" id="tab3"></div>
                <%--				<div class="tab-pane fade in" id="tab4"></div>--%>
                <div class="tab-pane fade in" id="tab5"></div>
                <div class="tab-pane fade in" id="tab6"></div>

                <!--CE Total-->
                <div class="row" id="DivTotal" style="display: none">
                    <table class="table-condensed" width="50%" align="right">
                        <tbody>
                            <tr>
                                <td>Original Grand Total</td>
                                <td>:</td>
                                <td></td>
                                <td>
                                    <input type="text" class="form-control" id="GrandTotal" name="GrandTotal" readonly></td>
                            </tr>
                            <tr>
                                <td>Current Grand Total</td>
                                <td>:</td>
                                <td></td>
                                <td>
                                    <input type="text" class="form-control" id="CurrentGrandTotal" name="GrandTotal" readonly></td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <!--Costing Total-->
                <div class="row" id="DivPJTTotal" style="display: none">
                    <table class="table-condensed" width="50%" align="right">
                        <tbody>
                            <tr>
                                <td>MR SubTotal</td>
                                <td>:</td>
                                <td></td>
                                <td>
                                    <input type="text" class="form-control" id="MRSubTotal" name="MRSubTotal" readonly></td>
                            </tr>
                            <tr>
                                <td>Labor SubTotal</td>
                                <td>:</td>
                                <td></td>
                                <td>
                                    <input type="text" class="form-control" id="LaborSubTotal" name="LaborSubTotal" readonly></td>
                            </tr>
                            <tr>
                                <td>Miscellaneous SubTotal</td>
                                <td>:</td>
                                <td></td>
                                <td>
                                    <input type="text" class="form-control" id="MiscSubTotal" name="MiscSubTotal" readonly></td>
                            </tr>
                            <tr>
                                <td>Project Costing Total</td>
                                <td>:</td>
                                <td></td>
                                <td>
                                    <input type="text" class="form-control" id="PJTTotal" name="PJTTotal" readonly></td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <!--Payment Total-->
                <div class="row" id="DivInvTotal" style="display: none">
                    <table class="table-condensed" width="50%" align="right">
                        <tbody>
                            <tr>
                                <td>Grand Total</td>
                                <td>:</td>
                                <td></td>
                                <td>
                                    <input type="text" class="form-control" id="InvGrandTotal" name="InvGrandTotal" readonly></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <!-- End Tab v2 -->
    </div>



    <div class="modal fade" id="ModalMessage" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <!-- Modal Body -->
                <div class="modal-body">
                    <span id="message" name="message"></span>
                </div>
                <!-- Modal Footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" id="btnClose" name="Close">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script src="EditProject.aspx.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".project-menu").addClass("active expand");
            $(".sub-project-search").addClass("active");
        });
    </script>
</asp:Content>
