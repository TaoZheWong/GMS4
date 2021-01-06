<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorEvaluationForm1.aspx.cs" Inherits="GMSWeb.Procurement.Forms.VendorEvaluationForm1" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Header1" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <title>Procurement - Vendor Registration Form</title>
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-timepicker/bootstrap-timepicker.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite.css" rel="stylesheet" />
    <link href="../../new_assets/css/layout.css" rel="stylesheet" />
    <link href="../../new_assets/css/component.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <style>
        body {
            overflow: auto;
        }

         .table>tbody>tr>td {
             border-color: #fff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
               <ul class="breadcrumb pull-right">
                <li class="active">Procurement</li>
                <li class="active">Vendor Registration Form</li>
             </ul>
             <h1 class="page-header">Vendor Registration/Pre-Qualification Form <br />
                <small>Please fill in the Vendor Registration Form below.</small></h1>
            <div class="clearfix"></div> 
             <asp:ScriptManager ID="sriptmgr1" runat="server">
            </asp:ScriptManager>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                   <%--     SECTION A: VENDOR'S DETAIL--%>
                    </h4>
                </div>
                 <div class="panel-body">
                     <div class="form-horizontal m-t-20">
                         <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Vendor Name
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span>
                                    <input type="hidden" id="hidFormID4" runat="server" />
                                    <input type="hidden" id="hidVendorID4" runat="server" />
                                    <input type="hidden" id="hidRandomID" runat="server" />
                                    <asp:Label runat="server" ID="lblVendorName1"></asp:Label>
                                </label>
                            </div>
                        </div>

                         <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Email
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span>
                                    <input type="hidden" id="Hidden1" runat="server" />
                                    <asp:Label runat="server" ID="lblEmail1"></asp:Label>
                                </label>
                            </div>
                        </div>
                     </div>
                 </div>
             </div>

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        SECTION A: GENERAL PARTICULARS OF VENDOR
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-4 control-label text-left">
                               Name of Company 
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtCompanyName" runat="server" Columns="100" MaxLength="100" CssClass="form-control" />
                             <asp:RequiredFieldValidator
								ID="rfvCompanyName" runat="server" ControlToValidate="txtCompanyName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            <input type="hidden" id="hidFormID5" runat="server" />
                            <input type="hidden" id="hidCompanyName" runat="server" />
                        </div>
                    </div>

                          <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                               Business Address
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtBusinessAddress" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                 <asp:RequiredFieldValidator
								ID="rfvBusinessAddress" runat="server" ControlToValidate="txtBusinessAddress" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                        </div>

                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                               Company Reg No
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtCompanyRegNo" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator
								ID="rfvCompanyRegNo" runat="server" ControlToValidate="txtCompanyRegNo" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                        </div>

                             <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                               Company Reg Date
                            </label>
                            <div class="col-sm-8">
                                  <div class="input-group date">
                                <asp:TextBox ID="txtCompanyRegDate" runat="server" Columns="10" MaxLength="10"
                                    CssClass="form-control datepicker"></asp:TextBox>  
                                 <span class="input-group-addon">
                                            <i class="ti-calendar"></i>
                                        </span>
                                      </div>
                            
                                 <asp:RequiredFieldValidator ID="rfvCompanyRegDate" runat="server" ControlToValidate="txtCompanyRegDate"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                                 <asp:CompareValidator ID="cvCompanyRegDate" runat="server" ErrorMessage="Invalid Date"
                                        ControlToValidate="txtCompanyRegDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow"
                                        Operator="DataTypeCheck"></asp:CompareValidator>
                                </div>
                            </div>      

                         <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                               GST Reg No
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtGSTRegNo" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator
								ID="rfvGSTRegNo" runat="server" ControlToValidate="txtGSTRegNo" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                            </div>

                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                               Company Tel No
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtCompanyTelNo" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                 <asp:RequiredFieldValidator
								ID="rfvCompanyTelNo" runat="server" ControlToValidate="txtCompanyTelNo" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                        </div>

                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                               Company Fax No
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtCompanyFaxNo" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                 <asp:RequiredFieldValidator
								ID="rfvCompanyFaxNo" runat="server" ControlToValidate="txtCompanyFaxNo" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                        </div>
                      </div>
                   </div>
                </div>
               <%--  <div class="panel-footer clearfix">
                       <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                       ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>
                      <asp:Button ID="btnUpdate" Text="Update" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                         OnClick="btnUpdate_Click"></asp:Button>--%>
                  <%--   <asp:Button ID="btnSendEmail" Text="Send Email" EnableViewState="False" runat="server" CssClass="btn btn-warning pull-right m-r-10"
                         OnClick="btnSendEmail_Click"></asp:Button>
                       <asp:Button ID="btnDuplicate" Text="Duplicate" EnableViewState="False" runat="server" CssClass="btn btn-success pull-right m-r-10"
                         OnClick="btnDuplicate_Click"></asp:Button>--%>

                      <%--  <asp:HyperLink ID="lnkVendorEvaluation" Visible="False" runat="server" CssClass="text-info"></asp:HyperLink>--%>
               <%--  </div>--%>
        

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        Contact Person Details
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-4 control-label text-left">
                               Name
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtContactPersonName" runat="server" Columns="100" MaxLength="100" CssClass="form-control" />
                            <asp:RequiredFieldValidator
								ID="rfvContactPersonName" runat="server" ControlToValidate="txtContactPersonName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                        </div>
                    </div>

                          <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                               Designation
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtContactPersonDesignation" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator
								ID="rfvContactPersonDesignation" runat="server" ControlToValidate="txtContactPersonDesignation" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                        </div>

                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                               Tel No.
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtContactPersonTelNo" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator
								ID="rfvContactPersonTelNo" runat="server" ControlToValidate="txtContactPersonTelNo" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                        </div>

                             <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                               Email
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtContactPersonEmail" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                               <asp:RequiredFieldValidator
								ID="rfvContactPersonEmail" runat="server" ControlToValidate="txtContactPersonEmail" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                                <asp:RegularExpressionValidator ID="revContactPersonEmail" runat="server" ErrorMessage="Invalid email address."  ControlToValidate="txtContactPersonEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic">
                                </asp:RegularExpressionValidator>


                            </div>
                        </div>
                        
                   </div>
                </div>
                 <div class="panel-footer clearfix">
                      <%-- <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                       ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>--%>
                      <asp:Button ID="btnUpdate" Text="Next" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                         OnClick="btnUpdate_Click" ValidationGroup="valGrpVendorDetail"></asp:Button>
                  <%--   <asp:Button ID="btnSendEmail" Text="Send Email" EnableViewState="False" runat="server" CssClass="btn btn-warning pull-right m-r-10"
                         OnClick="btnSendEmail_Click"></asp:Button>
                       <asp:Button ID="btnDuplicate" Text="Duplicate" EnableViewState="False" runat="server" CssClass="btn btn-success pull-right m-r-10"
                         OnClick="btnDuplicate_Click"></asp:Button>--%>

                      <%--  <asp:HyperLink ID="lnkVendorEvaluation" Visible="False" runat="server" CssClass="text-info"></asp:HyperLink>--%>
                 </div>
        </div>
   </div>
    </form>
     <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/jquery/jquery-1.9.1.min.js"></script> 
    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/jquery/jquery-migrate-1.1.0.min.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/bootstrap/dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" >
         $(document).ready(function () {
             $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
         });

    </script>
</body>
</html>
 