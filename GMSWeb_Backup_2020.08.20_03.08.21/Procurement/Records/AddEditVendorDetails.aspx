<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="AddEditVendorDetails.aspx.cs" Inherits="GMSWeb.Procurement.Records.AddEditVendorDetails" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>  

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <a name="TemplateInfo"></a>
   <%-- <title>Vendor - Add/Edit Vendor Evaluation Form</title>
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
    </style>  --%>

    <ul class="breadcrumb pull-right">
    <li><a href="#">Procurement</a></li>
    <li><a href="#">Record</a></li>
    <li class="active">Add/Edit Vendor</li>
</ul>

    <h1 class="page-header">Add Vendor <br />
    <small>Add or edit a vendor evaluation form.</small></h1>

        <asp:ScriptManager ID="sriptmgr1" runat="server" EnablePageMethods="true">
                 <Services>
                    <asp:ServiceReference Path="AutoCompleteVendorName.asmx" />
                </Services>
            </asp:ScriptManager>

        <div class="panel panel-primary">     
            <%--   <ul class="breadcrumb pull-right">
                <li class="active">Vendor</li>
                <li class="active">Add/Edit Vendor Evaluation Form</li>
             </ul>
             <h1 class="page-header">Vendor Evaluation Form <br />
                <small>Add or edit a Vendor Evaluation Form.</small></h1>--%>
            <div class="clearfix"></div> 
            <div class="panel panel-primary">
                <div class="panel-heading">
             <div class="panel-heading-btn">
            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
        </div>
                    <h4 class="panel-title">
                        SECTION A: VENDOR'S DETAIL
                    </h4>
                </div>
                 <div class="panel-body">
                     <div class="form-horizontal m-t-20">
                         <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Vendor Name
                            </label>
                   <div class="col-sm-8">
                        <asp:TextBox ID="txtVendorName" runat="server" Columns="80" MaxLength="80" CssClass="form-control"
                            onfocus="select();" onchange="this.value = this.value.toUpperCase()" AutoPostBack="true"
                            OnTextChanged="SetVendor" />
                        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                            TargetControlID="txtVendorName" ServicePath="AutoCompleteVendorName.asmx" ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";" CompletionListCssClass="ui-autocomplete" >
                        </ajaxToolkit:AutoCompleteExtender>
                        <input type="hidden" id="hidFormID1" runat="server" />
                        <input type="hidden" id="hidVendorID1" runat="server" />
                        <input type="hidden" id="hidVendorName" runat="server" />
                    </div>
                            <%--<div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span>
                                    <input type="hidden" id="hidFormID1" runat="server" />
                                    <input type="hidden" id="hidVendorID1" runat="server" />
                                    <input type="hidden" id="hidVendorName" runat="server" />
                                    <asp:Label runat="server" ID="lblVendorName1"></asp:Label>
                                </label>
                            </div>--%>
                        </div>

                        <%-- <div class="form-group col-lg-12">
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
                        </div>--%>
                     </div>
                 </div>
             </div>

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        SECTION B: VENDOR EVALUATION
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                      <%--  <div class="form-group col-lg-12">
                        <label class="col-sm-4 control-label text-left">
                               What type of Ownership ?
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtTypeOfOwnership" runat="server" Columns="100" MaxLength="100" CssClass="form-control" />
                            <input type="hidden" id="hidFormID2" runat="server" />
                            <input type="hidden" id="hidTypeOfOwnership" runat="server" />
                        </div>
                    </div>--%>

                      <%--    <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                What are the nature of your business?
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtNatureOfBusiness" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                            </div>
                        </div>--%>
                   </div>

                     <div class="form-group">
                    <label class="col-sm-3 control-label">
                   
                    </label>
                     <div class="col-sm-9">
                       <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                       ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>
                      <%--<asp:Button ID="btnUpdate" Text="Update" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                         OnClick="btnUpdate_Click"></asp:Button>--%>
                          <asp:Button ID="btnDuplicate" Text="Duplicate" EnableViewState="False" runat="server" CssClass="btn btn-success pull-right m-r-10"
                    OnClick="btnDuplicate_Click" Visible="false"></asp:Button>

         <%--           <asp:Button ID="btnSendEmail" Text="Send Email" EnableViewState="False" runat="server" CssClass="btn btn-warning pull-right m-r-10"
                    OnClick="btnSendEmail_Click" ></asp:Button>--%>
                         </div>
                 </div>

<%--                <div class="panel-footer">


                </div>--%>

                </div>
                
          </div>     
        </div>

     <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/jquery/jquery-1.9.1.min.js"></script> 
    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/jquery/jquery-migrate-1.1.0.min.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/bootstrap/dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" >
         $(document).ready(function () {
             $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
         });

    </script>


    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".vendor-menu").addClass("active expand");
            $(".sub-records").addClass("active");
        });
    </script>
</asp:Content>