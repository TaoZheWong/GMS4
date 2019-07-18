<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorRecords4.aspx.cs" Inherits="GMSWeb.Procurement.Records.VendorRecords4" %>
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
        <div class="panel-heading">
           <%-- <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i>
                <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
            </h4>--%>
            <h4 class="panel-title">
                        SECTION C: COMPANY ORGANISATION
                    </h4>
        </div>
        <div class="panel-body">
            <div class="table-responsive">
                <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    DataKeyField="PersonnelID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                    OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
                    OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                <input type="hidden" id="hidPersonnelID" runat="server" value='<%# Eval("PersonnelID")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Personnel Name">
                            <ItemTemplate>
                                <asp:Label ID="lblPersonnelName" runat="server">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                            CausesValidation="false"><span><%# Eval("PersonnelName")%></span></asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditPersonnelName"
                                   runat="server" Columns="15" MaxLength="15" Text='<%# Eval("PersonnelName") %>' />
                                 <asp:RequiredFieldValidator
									ID="rfvEditPersonnelName" runat="server" ControlToValidate="txtEditPersonnelName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditPersonnelName" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewPersonnelName" runat="server" Columns="15" MaxLength="15" />
                                <asp:RequiredFieldValidator
									ID="rfvNewPersonnelName" runat="server" ControlToValidate="txtNewPersonnelName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewPersonnelName" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                       
                         <asp:TemplateColumn HeaderText="Designation">
                            <ItemTemplate>
                                <asp:Label ID="lblDesignation" runat="server">
								  <%# Eval("PersonnelDesignation")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditPersonnelDesignation" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("PersonnelDesignation") %>' />
                                  <asp:RequiredFieldValidator
									ID="rfvEditPersonnelDesignation" runat="server" ControlToValidate="txtEditPersonnelDesignation" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditPersonnelDesignation" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewPersonnelDesignation" runat="server" Columns="15" MaxLength="50" />
                                 <asp:RequiredFieldValidator
									ID="rfvNewPersonnelDesignation" runat="server" ControlToValidate="txtNewPersonnelDesignation" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewPersonnelDesignation" />
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Year Of Experience">
                            <ItemTemplate>
                                <asp:Label ID="lblYearOfExperience" runat="server">
								  <%# Eval("PersonnelYearOfExperience")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditPersonnelYearOfExperience" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("PersonnelYearOfExperience") %>' />
                                  <asp:RequiredFieldValidator
									ID="rfvEditPersonnelYearOfExperience" runat="server" ControlToValidate="txtEditPersonnelYearOfExperience" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditPersonnelYearOfExperience" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewPersonnelYearOfExperience" runat="server" Columns="15" MaxLength="50" />
                                 <asp:RequiredFieldValidator
									ID="rfvNewPersonnelYearOfExperience" runat="server" ControlToValidate="txtNewPersonnelYearOfExperience" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewPersonnelYearOfExperience" />
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
                                    ValidationGroup="valGrpEditPersonnelName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                    ValidationGroup="valGrpNewPersonnelName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
                    <FooterStyle CssClass="tFooter" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
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
                               <p class="text-muted"> Note: Please provide a copy of the company latest organization chart and headcount.</p>
                            </label>           
                        </div>   
                    <div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="Label1" runat="server">Document Name</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName" runat="server" Style="text-transform: uppercase;" />
                       <asp:RequiredFieldValidator
							ID="rfvNewDocumentName" runat="server" ControlToValidate="txtDocumentName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpVendorDetail" />
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
            </div>
        </div>
            </div>    
                 <div class="panel-footer clearfix">
                       <asp:Button ID="btnNext" Text="Next" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                        OnClick="btnNext_Click"></asp:Button>
                       <asp:Button ID="btnBack" Text="Back" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                       ValidationGroup="valGrpNewRow" OnClick="btnBack_Click"></asp:Button>      
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

