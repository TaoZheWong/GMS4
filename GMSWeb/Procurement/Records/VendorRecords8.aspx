<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorRecords8.aspx.cs" Inherits="GMSWeb.Procurement.Records.VendorRecords8" %>
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
                         <label class="col-sm-12 control-label text-left">C. Has your company had previous experience in performing the scope of job mentioned above? </label>
                         <div class="form-group col-lg-12"> </div>
                    
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-8">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkExperienceYes" runat="server" Checked="false" />
                            <label for="<%=chkExperienceYes.ClientID %>">Yes</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkExperienceNo" runat="server" Checked="false" />
                            <label for="<%=chkExperienceNo.ClientID %>">No</label>
                            </div>
                        </div>                             
                       </div>

                        <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkExperienceNA" runat="server" Checked="false" />
                            <label for="<%=chkExperienceNA.ClientID %>">N/A</label>
                            </div>
                        </div>                             
                       </div>

                       <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left"> Comments by Vendor </label>                      
                         <div class="form-group col-lg-12">       
                                <asp:TextBox TextMode="multiline" ID="txtVendorComment3" runat="server" Rows="5" Columns="100" MaxLength="100" CssClass="form-control" readonly/>    
                                <input type="hidden" id="hidFormID5" runat="server" />   
                                <input type="hidden" id="hidVendorComment3" runat="server" />                     
                       </div>
                   </div>
                   </div>
                       <div class="panel-body row">
                <div class="form-horizontal m-t-20">  
                    <div class="form-group col-lg-12">
                            <label class="col-sm-12 control-label text-left">
                               <p class="text-muted"> (Note: Please attached past reference projects)</p>
                            </label>                            
                        </div>   
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="Label1" runat="server">Document Name</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName" runat="server" Style="text-transform: uppercase;" />
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
                </div>
            </div>   

             <div class="panel panel-primary">                
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left">D. Are your workers qualified in performing the above scope of job mentioned above? </label>
                         <div class="form-group col-lg-12"> </div>
                    
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-8">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkQualificationYes" runat="server" Checked="false" />
                            <label for="<%=chkQualificationYes.ClientID %>">Yes</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkQualificationNo" runat="server" Checked="false" />
                            <label for="<%=chkQualificationNo.ClientID %>">No</label>
                            </div>
                        </div>                             
                       </div>

                        <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkQualificationNA" runat="server" Checked="false" />
                            <label for="<%=chkQualificationNA.ClientID %>">N/A</label>
                            </div>
                        </div>                             
                       </div>
                          <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left"> Comments by Vendor </label>                      
                         <div class="form-group col-lg-12">       
                                <asp:TextBox TextMode="multiline" ID="txtVendorComment4" runat="server" Rows="5" Columns="100" MaxLength="100" CssClass="form-control" readonly/>       
                                       
                       </div>
                   </div>
                   </div>
                         <div class="panel-body row">
                <div class="form-horizontal m-t-20">  
                    <div class="form-group col-lg-12">
                            <label class="col-sm-12 control-label text-left">
                               <p class="text-muted"> (Note: Please attached worker’s qualification and training certificates)</p>
                            </label>                            
                        </div>   
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="Label2" runat="server">Document Name</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName2" runat="server" Style="text-transform: uppercase;" />
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
            </div>
                </div>
            </div>

            <div class="panel panel-primary">                
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left">E. Do you ensure that your employee and sub-contractors undergo statutory-required health safety include food safety and environment training? </label>
                         <div class="form-group col-lg-12"> </div>
                    
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-8">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkStatutoryHSEYes" runat="server" Checked="false" />
                            <label for="<%=chkStatutoryHSEYes.ClientID %>">Yes </label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkStatutoryHSENo" runat="server" Checked="false" />
                            <label for="<%=chkStatutoryHSENo.ClientID %>">No</label>
                            </div>
                        </div>                             
                       </div>

                        <div class="form-group col-lg-12">       
                         <div class="col-sm-2">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkStatutoryHSENA" runat="server" Checked="false" />
                            <label for="<%=chkStatutoryHSENA.ClientID %>">N/A</label>
                            </div>
                        </div>                             
                       </div>
                          <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left"> Comments by Vendor </label>                      
                         <div class="form-group col-lg-12">       
                                <asp:TextBox TextMode="multiline" ID="txtVendorComment5" runat="server" Rows="5" Columns="100" MaxLength="100" CssClass="form-control" readonly/>       
                                       
                       </div>
                   </div>
                   </div>
                          <div class="panel-body row">
                <div class="form-horizontal m-t-20">  
                    <div class="form-group col-lg-12">
                            <label class="col-sm-12 control-label text-left">
                               <p class="text-muted"> (Note: Please attached relevant records if possible) </p>
                            </label>                            
                        </div>   
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="Label3" runat="server">Document Name</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName3" runat="server" Style="text-transform: uppercase;" />
                        </div>
                    </div>
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblLocation3" runat="server">File Location</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <div class="input-group">
                                <input type="text" class="form-control" readonly>
                                <label class="input-group-btn">
                                    <span class="btn btn-primary btn-upload" style="font-size:20px">
                                        <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                        <asp:FileUpload CssClass="form-control hidden" ID="FileUpload3" runat="server" />
                                    </span>
                                </label>
                            </div>
                        </div>
                    </div>              
                </div>
            </div>
            <div class="panel-footer clearfix"> 
                <asp:LinkButton ID="linkfileName3" runat="server" OnClick = "btnDownload3_Click"></asp:LinkButton>   
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
 
