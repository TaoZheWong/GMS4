<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorEvaluationForm5.aspx.cs" Inherits="GMSWeb.Procurement.Forms.VendorEvaluationForm5" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Header1" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <title>Vendor - Add/Edit Vendor Evaluation Form</title>
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
                <li class="active">Vendor</li>
                <li class="active">Add/Edit Vendor Evaluation Form</li>
             </ul>
             <h1 class="page-header">Vendor Evaluation Form <br />
                <small>Add or edit a Vendor Evaluation Form.</small></h1>
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
                                    <input type="hidden" id="hidCoyID" runat="server" />
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
                        SECTION D: QUALITY/SAFETY/ENVIRONMENT
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-4 control-label text-left">
                               Number of Personnel in your QA/QC Department
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtQANoOfPersonnel" runat="server" Columns="100" MaxLength="100" CssClass="form-control" type="number"/>
                             <asp:RequiredFieldValidator
							  ID="rfvQANoOfPersonnel" runat="server" ControlToValidate="txtQANoOfPersonnel" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            <input type="hidden" id="hidFormID5" runat="server" />
                            <input type="hidden" id="hidQANoOfPersonnel" runat="server" />
                        </div>
                    </div>

                          <div class="form-group col-lg-12">
                            <label class="col-sm-3 control-label text-left">
                               Main QA Contact Person
                            </label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtQAContactPerson" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                               <asp:RequiredFieldValidator
							  ID="rfvQAContactPerson" runat="server" ControlToValidate="txtQAContactPerson" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                              <label class="col-sm-2 control-label text-left">
                               Designation
                            </label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtQAContactPersonDesignation" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator
							  ID="rfvQAContactPersonDesignation" runat="server" ControlToValidate="txtQAContactPersonDesignation" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                        </div>

                        <div class="form-group col-lg-12">
                            <label class="col-sm-3 control-label text-left">
                               Main HSE Contact Person
                            </label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtHSEContactPerson" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                 <asp:RequiredFieldValidator
							      ID="rfvHSEContactPerson" runat="server" ControlToValidate="txtHSEContactPerson" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                            <label class="col-sm-2 control-label text-left">
                               Designation
                            </label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtHSEContactPersonDesignation" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                 <asp:RequiredFieldValidator
							      ID="rfvHSEContactPersonDesignation" runat="server" ControlToValidate="txtHSEContactPersonDesignation" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                        </div>

                         <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                               Do you have a controlled QA Manual
                            </label>
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkManualYes" runat="server" Checked="false" />
                            <label for="<%=chkManualYes.ClientID %>">Yes</label>
                            </div>
                        </div>
                      <%--   <div class="col-sm-6">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkManualNo" runat="server" Checked="false" />
                            <label for="<%=chkManualNo.ClientID %>">No</label>
                         </div>
                        </div>--%>
                       </div>              
          </div>     
   
            <div class="panel-body row">
                <div class="form-horizontal m-t-20">  
                    <div class="form-group col-lg-12">
                            <label class="col-sm-12 control-label text-left">
                               <p class="text-muted"> (If Yes, please submit an uncontrolled copy) </p>
                            </label>                            
                        </div>   
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="Label1" runat="server">Document Name</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName" runat="server" />
                        </div>
                    </div>
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">File Location</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <div class="input-group">
                                <input type="text" class="form-control" readonly>
                                <label class="input-group-btn">
                                    <span class="btn btn-primary btn-upload" style="font-size:20px">
                                        <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                        <asp:FileUpload CssClass="form-control hidden" ID="FileUpload1" runat="server" />
                                    </span>
                                </label>
                            </div>
                        </div>
                    </div>              
                </div>
            </div>
            <div class="panel-footer clearfix">
                <small class="pull-left">
                      <asp:LinkButton ID="linkfileName" runat="server" OnClick = "btnDownload_Click"></asp:LinkButton>     
                    <asp:Label ID="lblMsg" runat="server" ForeColor="red"></asp:Label></small>
                <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload or Update" CssClass="btn btn-warning pull-right"
                    OnClick="btnUpload_Click" OnClientClick="this.disabled=true; this.value='Updating ...';" UseSubmitBehavior="false" />
            </div>
           </div>      
  </div>

            <div class="panel panel-primary">            
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left">  Is your company accredited with below certificates? (Please tick appropriately)</label>
                         <div class="form-group col-lg-12"> </div>
                    
                         <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                             ISO 9001
                            </label>
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkISO9001" runat="server" Checked="false" />
                            <label for="<%=chkISO9001.ClientID %>"></label>
                            </div>
                        </div>
                             <label class="col-sm-3 control-label text-left">
                             Accredited By
                            </label>
                              <div class="col-sm-3">
                                <asp:TextBox ID="txtISOAccreditedBy" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                <%--  <asp:RequiredFieldValidator
							      ID="rfvISOAccreditedBy" runat="server" ControlToValidate="txtISOAccreditedBy" ErrorMessage="*" Display="dynamic"/>--%>
                            </div>
                       </div>

                        <div class="form-group col-lg-12">      
                            <label class="col-sm-4 control-label text-left">
                             OHSAS 18001
                            </label>
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkOHSAS18001" runat="server" Checked="false" />
                            <label for="<%=chkOHSAS18001.ClientID %>"></label>
                            </div>
                        </div>
                             <label class="col-sm-3 control-label text-left">
                             Accredited By
                            </label>
                              <div class="col-sm-3">
                                <asp:TextBox ID="txtOHSASAccreditedBy" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                <%--  <asp:RequiredFieldValidator
							      ID="rfvOHSASAccreditedBy" runat="server" ControlToValidate="txtOHSASAccreditedBy" ErrorMessage="*" Display="dynamic"/>--%>
                            </div>
                       </div>

                         <div class="form-group col-lg-12">      
                            <label class="col-sm-4 control-label text-left">
                             ISO 14001
                            </label>
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkISO14001" runat="server" Checked="false" />
                            <label for="<%=chkISO14001.ClientID %>"></label>
                            </div>
                        </div>
                             <label class="col-sm-3 control-label text-left">
                             Accredited By
                            </label>
                              <div class="col-sm-3">
                                <asp:TextBox ID="txtISO1AccreditedBy" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                  <%-- <asp:RequiredFieldValidator
							      ID="rfvISO1AccreditedBy" runat="server" ControlToValidate="txtISO1AccreditedBy" ErrorMessage="*" Display="dynamic" />--%>
                            </div>
                       </div>

                         <div class="form-group col-lg-12">      
                            <label class="col-sm-4 control-label text-left">
                             FSSC - 22000
                            </label>
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkFSSC22000" runat="server" Checked="false" />
                            <label for="<%=chkFSSC22000.ClientID %>"></label>
                            </div>
                        </div>
                             <label class="col-sm-3 control-label text-left">
                             Accredited By
                            </label>
                              <div class="col-sm-3">
                                <asp:TextBox ID="txtFSSCAccreditedBy" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                               <%--   <asp:RequiredFieldValidator
							      ID="rfvFSSCAccreditedBy" runat="server" ControlToValidate="txtFSSCAccreditedBy" ErrorMessage="*" Display="dynamic"/>--%>
                            </div>
                       </div>

                        <div class="form-group col-lg-12">      
                            <label class="col-sm-4 control-label text-left">
                             BCA Grade: Please specify level
                            </label>
                              <div class="col-sm-8">
                                <asp:TextBox ID="txtBCAGrade" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                                  <asp:RequiredFieldValidator
							      ID="rfvBCAGrade" runat="server" ControlToValidate="txtBCAGrade" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                            </div>
                       </div>

                        <div class="form-group col-lg-12">      
                            <label class="col-sm-4 control-label text-left">
                             Others: Please specify
                            </label>
                              <div class="col-sm-8">
                                <asp:TextBox ID="txtOtherCertifications" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                            </div>
                       </div>

                    </div>
                    
            <div class="panel-body row">
                <div class="form-horizontal m-t-20">  
                    <div class="form-group col-lg-12">
                            <label class="col-sm-12 control-label text-left">
                               <p class="text-muted"> (Note: Please attach copy of valid certificate.) </p>
                            </label>                            
                        </div>   
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="Label2" runat="server">Document Name</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName2" runat="server"/>
                        </div>
                    </div>
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblLocation2" runat="server">File Location</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <div class="input-group">
                                <input type="text" class="form-control" readonly>
                                <label class="input-group-btn">
                                    <span class="btn btn-primary btn-upload" style="font-size:20px">
                                        <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                        <asp:FileUpload CssClass="form-control hidden" ID="FileUpload2" runat="server" />
                                    </span>
                                </label>
                            </div>
                        </div>
                    </div>              
                </div>
            </div>
            <div class="panel-footer clearfix">
                <small class="pull-left">
                      <asp:LinkButton ID="linkfileName2" runat="server" OnClick = "btnDownload2_Click"></asp:LinkButton>     
                    <asp:Label ID="lblMsg2" runat="server" ForeColor="red"></asp:Label></small>
                <asp:Button ID="btnUpload2" runat="server" CausesValidation="true" Text="Upload or Update" CssClass="btn btn-warning pull-right"
                    OnClick="btnUpload2_Click" OnClientClick="this.disabled=true; this.value='Updating ...';" UseSubmitBehavior="false" />
            </div>
                    
                </div>
            </div>

                    <div class="panel panel-primary">            
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left">Please identify which are the international accredited and classification societies standards which your company have worked with: </label>
                         <div class="form-group col-lg-12"> </div>
                    
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkLloyds" runat="server" Checked="false" />
                            <label for="<%=chkLloyds.ClientID %>">Lloyds</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkTUV" runat="server" Checked="false" />
                            <label for="<%=chkTUV.ClientID %>">TUV</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkABS" runat="server" Checked="false" />
                            <label for="<%=chkABS.ClientID %>">ABS</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkDNV" runat="server" Checked="false" />
                            <label for="<%=chkDNV.ClientID %>">DNV</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkBV" runat="server" Checked="false" />
                            <label for="<%=chkBV.ClientID %>">BV (Bureau Veritas)</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-12">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkWithout" runat="server" Checked="false" />
                            <label for="<%=chkWithout.ClientID %>"> Have not worked with any Accredited classification societies</label>
                            </div>
                        </div>                             
                       </div>

                   </div>
                </div>
            </div>
                  
              <div class="panel-footer clearfix">
                       <asp:Button ID="btnUpdate" Text="Next" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                        OnClick="btnUpdate_Click" ValidationGroup="valGrpVendorDetail"></asp:Button>
                       <asp:Button ID="btnBack" Text="Back" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                       ValidationGroup="valGrpNewRow" OnClick="btnBack_Click"></asp:Button>      
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
 
