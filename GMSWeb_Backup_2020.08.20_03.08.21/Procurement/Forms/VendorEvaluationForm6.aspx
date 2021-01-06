<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorEvaluationForm6.aspx.cs" Inherits="GMSWeb.Procurement.Forms.VendorEvaluationForm6" %>

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
                        SECTION E: INSURANCE COVERAGE
                    </h4>
              </div>       
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left">Is your company covered by any insurance schemes/programmes? </label>
                         <div class="form-group col-lg-12"> </div>
                    
                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkInsuranceYes" runat="server" Checked="false" />
                            <label for="<%=chkInsuranceYes.ClientID %>">Yes</label>
                            </div>
                        </div>                             
                       </div>

                         <div class="form-group col-lg-12">       
                         <div class="col-sm-4">
                            <div class="checkbox">
                            <asp:CheckBox ID="chkInsuranceNo" runat="server" Checked="false" />
                            <label for="<%=chkInsuranceNo.ClientID %>">No</label>
                            </div>
                        </div>                             
                       </div>

                        <div class="form-horizontal m-t-20">
                         <label class="col-sm-12 control-label text-left">Please list down the types of insurance scheme/programme available in your company. </label>                      
                         <div class="form-group col-lg-12">       
                                <asp:TextBox TextMode="multiline" ID="txtTypesOfInsuranceSchemes" runat="server" Rows="5" Columns="100" MaxLength="100" CssClass="form-control"/>    
                               <asp:RequiredFieldValidator
							  ID="rfvTypeOfInsuranceSchemes" runat="server" ControlToValidate="txtTypesOfInsuranceSchemes" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
                                <input type="hidden" id="hidFormID5" runat="server" />   
                                <input type="hidden" id="hidTypesOfInsuranceScheme" runat="server" />                     
                       </div>
                   </div>
                   </div>

                       <div class="panel-body row">
                <div class="form-horizontal m-t-20">  
                    <div class="form-group col-lg-12">
                            <label class="col-sm-12 control-label text-left">
                               <p class="text-muted"> (Note: Please attach insurance certificates.) </p>
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
        <div class="panel-heading">
        <h4 class="panel-title">
                        SECTION F: CUSTOMER & PROJECT TRACK RECORDS
                    </h4>
        </div>
        <div class="panel-body">
            <div class="table-responsive">
                <asp:DataGrid ID="dgData1" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    DataKeyField="RecordID" OnCancelCommand="dgData1_CancelCommand" OnEditCommand="dgData1_EditCommand"
                    OnUpdateCommand="dgData1_UpdateCommand" OnItemCommand="dgData1_CreateCommand" GridLines="none"
                    OnItemDataBound="dgData1_ItemDataBound" OnDeleteCommand="dgData1_DeleteCommand"
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData1_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData1.CurrentPageIndex) * dgData1.PageSize)  %>
                                <input type="hidden" id="hidRecordID" runat="server" value='<%# Eval("RecordID")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerlName" runat="server">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                            CausesValidation="false"><span><%# Eval("CustomerName")%></span></asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditCustomerName"
                                   runat="server" Columns="15" MaxLength="15" Text='<%# Eval("CustomerName") %>' />
                                 <asp:RequiredFieldValidator
									ID="rfvEditCustomerName" runat="server" ControlToValidate="txtEditCustomerName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorCustomer1" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewCustomerName" runat="server" Columns="15" MaxLength="15" />
                                <asp:RequiredFieldValidator
									ID="rfvNewCustomerName" runat="server" ControlToValidate="txtNewCustomerName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorCustomer"" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                       
                         <asp:TemplateColumn HeaderText="Project No">
                            <ItemTemplate>
                                <asp:Label ID="lblProjectNo" runat="server">
								  <%# Eval("ProjectNo")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditProjectNo" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("ProjectNo") %>' />
                                  <asp:RequiredFieldValidator
									ID="rfvEditProjectNo" runat="server" ControlToValidate="txtEditProjectNo" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorCustomer1" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewProjectNo" runat="server" Columns="15" MaxLength="50" />
                                 <asp:RequiredFieldValidator
									ID="rfvNewProjectNo" runat="server" ControlToValidate="txtNewProjectNo" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorCustomer" />
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Year Of Completion">
                            <ItemTemplate>
                                <asp:Label ID="lblYearOfCompletion" runat="server">
								  <%# Eval("YearOfCompletion")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditYearOfCompletion" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("YearOfCompletion") %>' />
                                  <asp:RequiredFieldValidator
									ID="rfvEditYearOfCompletion" runat="server" ControlToValidate="txtEditYearOfCompletion" Type="Integer" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorCustomer1" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewYearOfCompletion" runat="server" Columns="15" MaxLength="50" />
                                 <asp:RequiredFieldValidator
									ID="rfvNewYearOfCompletion" runat="server" ControlToValidate="txtNewYearOfCompletion" Type="Integer" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorCustomer"/>
                            </FooterTemplate>
                        </asp:TemplateColumn>


                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                    ValidationGroup="valGrpVendorCustomer1" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                    ValidationGroup="valGrpVendorCustomer" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
                    <FooterStyle CssClass="tFooter" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
            </div>    
            </div>
             <div class="TABCOMMAND">
    <asp:UpdatePanel ID="udpMsgUpdater1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="MsgPanel1" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </div> 
        </div>

          <div id="tbUpload" runat="server">
             <div class="panel panel-primary">
            <div class="panel-heading">
                <div class="panel-heading-btn">
                    <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                </div>
                <h4 class="panel-title">
                    <i class="ti-pencil"></i>Upload File
                </h4>
            </div>
            <div class="panel-body row">
                <div class="form-horizontal m-t-20">  
                    <div class="form-group col-lg-12">
                            <label class="col-sm-12 control-label text-left">
                               <p class="text-muted">(PLEASE ATTACHED A LIST OF LATEST PROJECT RECORD) </p>
                            </label>
                        </div>   
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="Label2" runat="server">Document Name</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName2" runat="server" />
                       <asp:RequiredFieldValidator
							ID="rfvDocumentName2" runat="server" ControlToValidate="txtDocumentName2" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail"/>
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
                       <div class="panel-footer clearfix">
                       <asp:Button ID="btnUpdate" Text="Next" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                        OnClick="btnUpdate_Click"  ValidationGroup="valGrpVendorDetail"></asp:Button>
                       <asp:Button ID="btnBack" Text="Back" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                       OnClick="btnBack_Click"></asp:Button>      
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
 
