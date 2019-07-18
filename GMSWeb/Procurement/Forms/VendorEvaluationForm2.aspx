 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorEvaluationForm2.aspx.cs" Inherits="GMSWeb.Procurement.Forms.VendorEvaluationForm2" %>
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
                        Type of Ownership (Please tick appropriately)
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Proprietorship
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkProprietorship" runat="server" Checked="false" />
                            <label for="<%=chkProprietorship.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>

                          <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Partnership
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkPartnership" runat="server" Checked="false" />
                            <label for="<%=chkPartnership.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>

                       <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Corporation
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkCorporation" runat="server" Checked="false" />
                            <label for="<%=chkCorporation.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>

                    <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Private Limited
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkPrivateLimited" runat="server" Checked="false" />
                            <label for="<%=chkPrivateLimited.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>  
                    <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Public Limited
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkPublicLimited" runat="server" Checked="false" />
                            <label for="<%=chkPublicLimited.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>  

                         <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Others (Please specify)
                         </label>   
                          <div class="col-sm-3">
                      <%--  <div class="checkbox">
                            <asp:CheckBox ID="chkOthers" runat="server" Checked="false" />
                            <label for="<%=chkOthers.ClientID %>"></label>
                        </div>   --%>     
                      </div>
                    </div>  
                        <div class="form-group col-lg-12">       
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtOthersType" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                            </div>
                        </div>
                   
                   </div>
                </div>
        </div>

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        Nature of Business (Please tick appropriately)
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Manufacturer
                         </label>
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkManufacturer" runat="server" Checked="false" />
                            <label for="<%=chkManufacturer.ClientID %>"></label>
                        </div>
                      </div>
                    </div>

                          <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Sub-contractor
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkSubcontractor" runat="server" Checked="false" />
                            <label for="<%=chkSubcontractor.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>

                       <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Agent/Distributor 
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkAgent" runat="server" Checked="false" />
                            <label for="<%=chkAgent.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>

                    <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                            Stockiest
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkStockiest" runat="server" Checked="false" />
                            <label for="<%=chkStockiest.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>  
                    <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Fabricator
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkFabricator" runat="server" Checked="false" />
                            <label for="<%=chkFabricator.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>  

                         <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Trading House
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkTradingHouse" runat="server" Checked="false" />
                            <label for="<%=chkTradingHouse.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>  

                         <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Others (Please specify)
                         </label>       
                          <div class="col-sm-3">
                    <%--    <div class="checkbox">
                            <asp:CheckBox ID="chkOthers1" runat="server" Checked="false" />
                            <label for="<%=chkOthers1.ClientID %>"></label>
                        </div>--%>
                      </div>
                    </div>  

                           <div class="form-group col-lg-12">     
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtOthersNature" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                            </div>
                        </div>
                   
                   </div>
                      <div class="panel-body row">
                <div class="form-horizontal m-t-20">  
                    <div class="form-group col-lg-12">
                            <label class="col-sm-12 control-label text-left">
                               <p class="text-muted"> (If Yes, please attach a copy of the appointment letter from brand that you are representing) </p>
                            </label>                            
                        </div>   
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="Label1" runat="server">Document Name</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName" runat="server"  />
                        </div>
                    </div>
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">File Location</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <div class="input-group">
                                <input type="text" class="form-control" readonly/>
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
                    <asp:LinkButton ID="linkfileName" runat="server" OnClick = "btnDownload_Click"></asp:LinkButton>            
                   <small class="pull-left"><asp:Label ID="lblMsg" runat="server" ForeColor="red" ></asp:Label></small>
                  <%-- <asp:LinkButton ID="linkName" runat="server" Text='<%# Eval("AgentDistributorDocumentName")%>' CommandName="Load" ForeColor="#005DAA" CommandArgument='<%#Eval("AgentDistributorFileName")%>'></asp:LinkButton>--%>
                <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload or Update" CssClass="btn btn-warning pull-right"
                    OnClick="btnUpload_Click" OnClientClick="this.disabled=true; this.value='Updating ...';" UseSubmitBehavior="false" />
            </div>
                </div>
        </div>

            <div class="panel panel-primary">
              <div class="panel-heading">
                    <h4 class="panel-title">
                        Scope of work provide to Leeden NOX (Please tick appropriately)
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Supply of material, equipment
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkSupply" runat="server" Checked="false" />
                            <label for="<%=chkSupply.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>

                          <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Building related construction, renovation, addition and alteration
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkBuilding" runat="server" Checked="false" />
                            <label for="<%=chkBuilding.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>

                       <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                            Process Machinery installation, maintenance or servicing works
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkProcess" runat="server" Checked="false" />
                            <label for="<%=chkProcess.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>

                    <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                            On Site Contractors
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkContractors" runat="server" Checked="false" />
                            <label for="<%=chkContractors.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>  
                    <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Waste Collectors
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkCollectors" runat="server" Checked="false" />
                            <label for="<%=chkCollectors.ClientID %>"></label>
                        </div>
        
                      </div>
                    </div>  

                         <div class="form-group col-lg-12">
                         <label class="col-sm-9 control-label text-left">
                             Transporters
                         </label>
                         
                          <div class="col-sm-3">
                        <div class="checkbox">
                            <asp:CheckBox ID="chkTransporters" runat="server" Checked="false" />
                            <label for="<%=chkTransporters.ClientID %>"></label>
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
