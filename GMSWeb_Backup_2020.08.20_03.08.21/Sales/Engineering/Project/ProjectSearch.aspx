<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ProjectSearch.aspx.cs" Inherits="GMSWeb.Sales.Engineering.Project.ProjectSearch" Title="Project Search" %>

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

        #resizable {
            width: 150px;
            height: 150px;
            padding: 0.5em;
        }

            #resizable h3 {
                text-align: center;
                margin: 0;
            }
    </style>

    <ul class="breadcrumb pull-right">
        <li><a href="#">Sales</a></li>
        <li><a href="#">Project</a></li>
        <li class="active">Project Information</li>
    </ul>
    <h1 class="page-header">Project Information
    </h1>


    <div class="panel panel-primary">
        <div class="panel-heading">
            <h1 class="panel-title">
                <i class="ti-search"></i>
                Search Filter
            </h1>
        </div>
        <div class="panel-body row">
            <div class="m-t-20">
                <input type="hidden" id="hidCoyID" runat="server" value="" />
                <input type="hidden" id="hidUserID" runat="server" value="" />
                <input type="hidden" id="hidCurrentLink" runat="server" value="" />

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtProjectNo">Project No:</label>
                            <input type="text" class="form-control" id="txtProjectNo" tabindex="1" />
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtPrevProjectNo">Prev Project No:</label>
                            <input type="text" class="form-control" id="txtPrevProjectNo" tabindex="2" />
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtCustomerPO">Customer P/O:</label>
                            <input type="text" class="form-control" id="txtCustomerPO" tabindex="3" />
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtStatusID">Status:</label>
                            <select id="txtStatusID" class="form-control select" tabindex="4">
                                <option value="">All</option>
                            </select>
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtAccountCode">Customer Code:</label>
                            <input type="text" class="form-control" id="txtAccountCode" tabindex="5" />
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtAccountName">Customer Name:</label>
                            <input type="text" class="form-control" id="txtAccountName" tabindex="6" />
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtEngineer">Engineer ID or Name:</label>
                            <input type="text" class="form-control" id="txtEngineer" tabindex="7" />
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtSalesPerson">SalesPerson ID or Name:</label>
                            <input type="text" class="form-control" id="txtSalesPerson" tabindex="8" />
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtCreatedDateFrom">Creation Date From:</label>
                            <div class='input-group date_yyyy_mm_dd' id='datetimepickerTo'>
                                <input type='text' class="form-control" id="txtCreatedDateFrom" tabindex="9" />
                                <span class="input-group-addon">
                                    <i class="ti-calendar"></i>
                                </span>
                            </div>
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtCreatedDateTo">Creation Date To:</label>
                            <div class='input-group date_yyyy_mm_dd' id='Div1'>
                                <input type='text' class="form-control" id="txtCreatedDateTo" tabindex="10" />
                                <span class="input-group-addon">
                                    <i class="ti-calendar"></i>
                                </span>
                            </div>
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtCommencementDateFrom">Commencement Date From:</label>
                            <div class='input-group date_yyyy_mm_dd' id='Div2'>
                                <input type='text' class="form-control" id="txtCommencementDateFrom" tabindex="11" />
                                <span class="input-group-addon">
                                    <i class="ti-calendar"></i>
                                </span>
                            </div>
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtCommencementDateTo">Commencement Date To:</label>
                            <div class='input-group date_yyyy_mm_dd' id='Div3'>
                                <input type='text' class="form-control" id="txtCommencementDateTo" tabindex="12" />
                                <span class="input-group-addon">
                                    <i class="ti-calendar"></i>
                                </span>
                            </div>
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtClosingDateFrom">Closing Date From:</label>
                            <div class='input-group date_yyyy_mm_dd' id='Div4'>
                                <input type='text' class="form-control" id="txtClosingDateFrom" tabindex="13" />
                                <span class="input-group-addon">
                                    <i class="ti-calendar"></i>
                                </span>
                            </div>
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtClosingDateTo">Closing Date To:</label>
                            <div class='input-group date_yyyy_mm_dd' id='Div5'>
                                <input type='text' class="form-control" id="txtClosingDateTo" tabindex="14" />
                                <span class="input-group-addon">
                                    <i class="ti-calendar"></i>
                                </span>
                            </div>
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtIsBillable">Billable:</label>
                            <select class="form-control select" id="txtIsBillable" tabindex="15">
                                <option value="">All</option>
                                <option value="True">Yes</option>
                                <option value="False">No</option>
                            </select>
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label" for="txtIsProgressiveClaim">Progressive Claim:</label>
                            <select class="form-control select" id="txtIsProgressiveClaim" tabindex="16">
                                <option value="">All</option>
                                <option value="True">Yes</option>
                                <option value="False">No</option>
                            </select>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <div class="col-sm-12" align="right">
                <p>
                    <a href="#" class="btn btn-sm btn-primary" id="btnAdd" style="display: none;">Add</a>
                    <a href="#" class="btn btn-primary btn-sm active" id="btnSearch">Search</a>
                </p>
            </div>
        </div>
    </div>
    <table id="tblSearch" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".project-menu").addClass("active expand");
            $(".sub-project-search").addClass("active");
        });
    </script>
    <script src="ProjectSearch.aspx.js" type="text/javascript"></script>
</asp:Content>
