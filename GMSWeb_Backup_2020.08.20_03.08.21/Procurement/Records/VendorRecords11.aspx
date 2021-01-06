<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorRecords11.aspx.cs" Inherits="GMSWeb.Procurement.Records.VendorRecords11" %>
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
                    <h4 class="panel-title">
                        Document Submission CheckList
                    </h4>    
               </div>              
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                         <div class="form-group col-lg-12 text-left"><b>Note: Please tick according to the type of document you are Submitting.</b></div>                    
                        <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="10" CellSpacing="20">
            <asp:TableHeaderRow  runat="server" Font-Bold="true" >
                <asp:TableHeaderCell>Section</asp:TableHeaderCell>
                <asp:TableHeaderCell>Type Of Document</asp:TableHeaderCell>
                <asp:TableHeaderCell>Please (✓)</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow ID="TableRow1" runat="server">
                <asp:TableCell RowSpan="4">II. Financial Information</asp:TableCell>
                <asp:TableCell>Financial Reports (Past 3yrs report)</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkFinancialReport" runat="server" Checked="false" />
                <label for="<%=chkFinancialReport.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow2" runat="server">     
                <asp:TableCell>Company’s ACRA / Direct credit authorization form</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkCompanyACRA" runat="server" Checked="false" />
                <label for="<%=chkCompanyACRA.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow3" runat="server">
                <asp:TableCell>Company Audit reports</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkAuditReport" runat="server" Checked="false" />
                <label for="<%=chkAuditReport.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
           <asp:TableRow  ID="TableRow4" runat="server">
                <asp:TableCell>Electronic funds Transfer information</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkEFTInformation" runat="server" Checked="false" />
                <label for="<%=chkEFTInformation.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow5" runat="server">
                <asp:TableCell RowSpan="2">III. Company Organisation</asp:TableCell>
                <asp:TableCell>Latest Company’s Organization chart</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkOrganizationChart" runat="server" Checked="false" />
                <label for="<%=chkOrganizationChart.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
           <asp:TableRow  ID="TableRow6" runat="server">
                <asp:TableCell>Company’s latest Head count</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkHeadCount" runat="server" Checked="false" />
                <label for="<%=chkHeadCount.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow7" runat="server">
                <asp:TableCell RowSpan="2">IV. Quality/Safety/Environmental</asp:TableCell>
                <asp:TableCell>Company uncontrolled Quality manual</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkQualityManual" runat="server" Checked="false" />
                <label for="<%=chkQualityManual.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow8" runat="server">
                <asp:TableCell>Company’s accredited certificate (eg..ISO 9001,ISO 14001)</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkAccreditedCertificates" runat="server" Checked="false" />
                <label for="<%=chkAccreditedCertificates.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow  ID="TableRow9" runat="server">
                <asp:TableCell>V. Insurance Coverage</asp:TableCell>
                <asp:TableCell>Company’s Insurance certificates</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkInsuranceCertificate" runat="server" Checked="false" />
                <label for="<%=chkInsuranceCertificate.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
           <asp:TableRow  ID="TableRow10" runat="server">
                <asp:TableCell>VI. Customer and Project Records</asp:TableCell>
                <asp:TableCell>Tabulated details track records</asp:TableCell>
                 <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkTrackRecords" runat="server" Checked="false" />
                <label for="<%=chkTrackRecords.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow11" runat="server">
                <asp:TableCell RowSpan="5">VII. Health, Safety and Environment</asp:TableCell>
                <asp:TableCell>Company’s HSE Policy or Manual</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkHSEPolicy" runat="server" Checked="false" />
                <label for="<%=chkHSEPolicy.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow12" runat="server">        
                <asp:TableCell>Worker’s qualification and training certificates</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkQualificationCertificates" runat="server" Checked="false" />
                <label for="<%=chkQualificationCertificates.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow13" runat="server">        
                <asp:TableCell>Biz Safe Level and certificate</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkSafeLevel" runat="server" Checked="false" />
                <label for="<%=chkSafeLevel.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow14" runat="server">        
                <asp:TableCell>Tabulated details track records </asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkHSETrackRecords" runat="server" Checked="false" />
                <label for="<%=chkHSETrackRecords.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow15" runat="server">        
                <asp:TableCell>Workers Training Records</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkTrainingRecords" runat="server" Checked="false" />
                <label for="<%=chkTrainingRecords.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow16" runat="server">        
                <asp:TableCell RowSpan="4">Other documentation</asp:TableCell>
                <asp:TableCell>Company’s Profile: History back ground</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkCompanyBackground" runat="server" Checked="false" />
                <label for="<%=chkCompanyBackground.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow17" runat="server">        
                <asp:TableCell>Product Information</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkProductInformation" runat="server" Checked="false" />
                <label for="<%=chkProductInformation.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow18" runat="server">        
                <asp:TableCell>Technical information</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkTechnicalInformation" runat="server" Checked="false" />
                <label for="<%=chkTechnicalInformation.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow  ID="TableRow19" runat="server">        
                <asp:TableCell>Brochure</asp:TableCell>
                <asp:TableCell><div class="checkbox"><asp:CheckBox ID="chkBrochure" runat="server" Checked="false" />
                <label for="<%=chkBrochure.ClientID %>"></label></div></asp:TableCell>
            </asp:TableRow>
            <asp:TableFooterRow runat="server">
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Right" Font-Italic="true">  
                </asp:TableCell>
            </asp:TableFooterRow>
        </asp:Table>
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
 

