<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorRecords9.aspx.cs" Inherits="GMSWeb.Procurement.Records.VendorRecords9" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Header1" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <title>Procurement -  Vendor Evaluation Form</title>
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
                <li class="active">Vendor Evaluation Form</li>
             </ul>
             <h1 class="page-header">Vendor Evaluation Form <br />
                <small>Approved or rejected a Vendor Evaluation Form.</small></h1>
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
                                    <input type="hidden" id="hidVendorName" runat="server" />
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

                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Status
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span>
                                    <input type="hidden" id="Hidden2" runat="server" />
                                    <asp:Label runat="server" ID="lblStatus1"></asp:Label>
                                </label>
                            </div>
                        </div>
                     </div>
                 </div>
             </div>   

             <div class="panel panel-primary">            
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left">F. Will you be providing a contractor supervisor on site? </label>
                         <div class="form-group col-lg-12"> </div>
                    
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-8">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkContractorYes" runat="server" Checked="false" />
                            <label for="<%=chkContractorYes.ClientID %>">Yes </label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkContractorNo" runat="server" Checked="false" />
                            <label for="<%=chkContractorNo.ClientID %>">No</label>
                            </div>
                        </div>                             
                       </div>

                        <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkContractorNA" runat="server" Checked="false" />
                            <label for="<%=chkContractorNA.ClientID %>">N/A</label>
                            </div>
                        </div>                             
                       </div>

                       <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left"> Comments by Vendor </label>                      
                         <div class="form-group col-lg-12">       
                                <asp:TextBox TextMode="multiline" ID="txtVendorComment6" runat="server" Rows="5" Columns="100" MaxLength="100" CssClass="form-control" readonly/>    
                                <input type="hidden" id="hidFormID5" runat="server" />   
                                <input type="hidden" id="hidVendorComment6" runat="server" />                     
                       </div>
                   </div>
                   </div>
                </div>
            </div>   

             <div class="panel panel-primary">                
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left">G. Do you have a designated SHE representative / Personnel? </label>
                         <div class="form-group col-lg-12"> </div>
                    
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-8">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkRepresentativeYes" runat="server" Checked="false" />
                            <label for="<%=chkRepresentativeYes.ClientID %>">Yes</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkRepresentativeNo" runat="server" Checked="false" />
                            <label for="<%=chkRepresentativeNo.ClientID %>">No</label>
                            </div>
                        </div>                             
                       </div>

                        <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkRepresentativeNA" runat="server" Checked="false" />
                            <label for="<%=chkRepresentativeNA.ClientID %>">N/A</label>
                            </div>
                        </div>                             
                       </div>
                          <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left"> Comments by Vendor </label>                      
                         <div class="form-group col-lg-12">       
                                <asp:TextBox TextMode="multiline" ID="txtVendorComment7" runat="server" Rows="5" Columns="100" MaxLength="100" CssClass="form-control" readonly/>       
                                       
                       </div>
                   </div>
                   </div>
                </div>
            </div>

            <div class="panel panel-primary">                
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left">H. Do you ensure that your employee and sub-contractors undergo statutory-required Medical Examinations? </label>
                         <div class="form-group col-lg-12"> </div>
                    
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-8">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkStatutoryMEYes" runat="server" Checked="false" />
                            <label for="<%=chkStatutoryMEYes.ClientID %>">Yes</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkStatutoryMENo" runat="server" Checked="false" />
                            <label for="<%=chkStatutoryMENo.ClientID %>">No</label>
                            </div>
                        </div>                             
                       </div>

                        <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkStatutoryMENA" runat="server" Checked="false" />
                            <label for="<%=chkStatutoryMENA.ClientID %>">N/A</label>
                            </div>
                        </div>                             
                       </div>
                          <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left"> Comments by Vendor </label>                      
                         <div class="form-group col-lg-12">       
                                <asp:TextBox TextMode="multiline" ID="txtVendorComment8" runat="server" Rows="5" Columns="100" MaxLength="100" CssClass="form-control" ReadOnly/>       
                                       
                       </div>
                   </div>
                   </div>
                </div>
            </div>

              <div class="panel panel-primary">                
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <div class="form-group col-lg-12"><b>HSE and Food Safety Standard Requirement</b></div>
                         <label class="col-sm-12 control-label text-left">It is Leeden’s company policy that all Suppliers/Vendors/Contractors performing work or services within Leeden’s premises are required to abide Leeden’s HSE and Food Safety requirement  </label>
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-8">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkRequirementYes" runat="server" Checked="false" />
                            <label for="<%=chkRequirementYes.ClientID %>">Comply</label>
                            </div>
                        </div>                             
                       </div>
                   </div>
                </div>
            </div>
           
            <div class="panel-footer clearfix">
                       <asp:Button ID="btnUpdate" Text="Next" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                        OnClick="btnUpdate_Click"></asp:Button>
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
 
