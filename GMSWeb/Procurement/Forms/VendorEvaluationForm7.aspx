<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorEvaluationForm7.aspx.cs" Inherits="GMSWeb.Procurement.Forms.VendorEvaluationForm7" %>
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
                        SECTION G: HEALTH, SAFETY AND ENVIRONMENT
                    </h4>
                     <p>
                         HSE Questionnaire
                     </p>
              </div>       
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left">A. Does your organization have the following policies or management system? </label>
                         <div class="form-group col-lg-12"> </div>
                    
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkPoliciesYes" runat="server" Checked="false" onclick="MutExChkList(this);" />
                            <label for="<%=chkPoliciesYes.ClientID %>">Yes</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkPoliciesNo" runat="server" Checked="false" onclick="MutExChkList(this);"/>
                            <label for="<%=chkPoliciesNo.ClientID %>">No</label>
                            </div>
                        </div>                             
                       </div>

                        <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkPoliciesNA" runat="server" Checked="false" onclick="MutExChkList(this);" />
                            <label for="<%=chkPoliciesNA.ClientID %>">N/A</label>
                            </div>
                        </div>                             
                       </div>
                   </div>
                </div>
            </div>

               <div class="panel panel-primary">                  
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkSafety" runat="server" Checked="false" />
                            <label for="<%=chkSafety.ClientID %>">Safety</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkFoodSafety" runat="server" Checked="false" />
                            <label for="<%=chkFoodSafety.ClientID %>">Food Safety</label>
                            </div>
                        </div>                             
                       </div>


                        <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkHealthEnvironmental" runat="server" Checked="false" />
                            <label for="<%=chkHealthEnvironmental.ClientID %>">Health And Environmental</label>
                            </div>
                        </div>                             
                       </div>
                        <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left"> Comments by Vendor </label>                      
                         <div class="form-group col-lg-12">       
                                <asp:TextBox TextMode="multiline" ID="txtVendorComment1" runat="server" Rows="5" Columns="100" MaxLength="100" CssClass="form-control" />    
                                <input type="hidden" id="hidFormID5" runat="server" />   
                                <input type="hidden" id="hidVendorComment1" runat="server" />                     
                       </div>
                   </div>
                   </div>

                    <div class="panel-body row">
                <div class="form-horizontal m-t-20">  
                    <div class="form-group col-lg-12">
                            <label class="col-sm-12 control-label text-left">
                               <p class="text-muted"> (Note: Please attached a copy of Policy/Manual) </p>
                            </label>                            
                        </div>   
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="Label1" runat="server">Document Name</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName" runat="server"/>
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
                <asp:LinkButton ID="linkfileName" runat="server" OnClick = "btnDownload_Click"></asp:LinkButton>     
                <small class="pull-left">
                    <asp:Label ID="lblMsg" runat="server" ForeColor="red"></asp:Label></small>
                <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload or Update" CssClass="btn btn-warning pull-right"
                    OnClick="btnUpload_Click" OnClientClick="this.disabled=true; this.value='Updating ...';" UseSubmitBehavior="false" />
            </div>
                </div>
            </div>

             <div class="panel panel-primary">                
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left">B. Your company has not been cited for violation of any workplace health and safety including food safety or environment laws or regulation in the past 3 years? </label>
                         <div class="form-group col-lg-12"> </div>
                    
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkViolationYes" runat="server" Checked="false" />
                            <label for="<%=chkViolationYes.ClientID %>">Yes</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkViolationNo" runat="server" Checked="false" />
                            <label for="<%=chkViolationNo.ClientID %>">No</label>
                            </div>
                        </div>                             
                       </div>

                        <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkViolationNA" runat="server" Checked="false" />
                            <label for="<%=chkViolationNA.ClientID %>">N/A</label>
                            </div>
                        </div>                             
                       </div>
                          <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left"> Comments by Vendor </label>                      
                         <div class="form-group col-lg-12">       
                                <asp:TextBox TextMode="multiline" ID="txtVendorComment2" runat="server" Rows="5" Columns="100" MaxLength="100" CssClass="form-control" />      
                                <input type="hidden" id="hidVendorComment2" runat="server" />                     
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
 
