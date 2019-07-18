<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorRecords13.aspx.cs" Inherits="GMSWeb.Procurement.Records.VendorRecords13" %>
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
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left text-underline">
                             <b>6.	Conformity and certification</b> 
                        </label>
                        <div class="col-sm-12">
                            <label class="col-sm-12 control-label text-left text-justify">
                           The Company reserves the right to request for certificates of origin and/or test certificates for the Goods. Such certificates must clearly state the Company’s PO number and the serial number of the Goods.
                        </label>
                            <input type="hidden" id="hidFormID5" runat="server" />
                            
                        </div>
                    </div>            
                   </div>  
                    
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left text-underline">
                             <b>7.	Packing </b> 
                        </label>
                        <div class="col-sm-12">
                        <label class="col-sm-12 control-label text-left text-justify ">
                         For Goods where delivery by shipments or airfreight are required, the PO number and shipping marks must appear on all invoices, bills of lading, packing list, correspondence and other papers in connection with this PO. Net and gross weights of each package in kilograms and pounds must be shown on all copies of invoices and packing lists. Packages are to be numbered with contents of each package shown on packing lists. Invoices must be typewritten with full description of goods without abbreviation and must show terms of purchase. Country of origin must be shown on all invoices and packing lists. A copy of the packing list must be included in each package delivered so that materials can be checked when received at destination.
                        </label>   
                        </div>
                    </div>
                   </div>      
                    
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left text-underline">
                             <b>8.	Liquidated Damages</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                         The liquidated damages for late delivery is 0.5% per day of the PO value and the Supplier shall be liable to pay the Company liquidated damages so far as the Goods remain undelivered subject to a maximum of 15% of the PO value.
                        </label>   
                        </div>
                    </div>
                   </div>   

                     <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left text-underline">
                             <b>9.	Warranties</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                         Unless otherwise agreed, Seller warrants that the goods supplied shall be new, fit for purpose and be free from defect in material, workmanship and design (where applicable) and the goods shall conform strictly with the written specifications of Buyer as supplied to Seller and subject thereto shall be of the best commercial quality.
                        </label>   
                        </div>
                    </div>
                   </div> 

                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left text-underline">
                             <b>10.	Inspection and Acceptance</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                        Buyer shall be provided with reasonable opportunity to examine the goods delivered before acceptance. Seller shall provide Buyer with a minimum of 7 working days prior notice in writing for local Factory Acceptance Test and 14 working days prior notice in writing for Factory Acceptance Test not in Singapore. Buyer reserves the right to reject any goods that are not in accordance with specifications or are damaged or defective. Buyer shall have no obligation to pay for rejected, damaged or defective goods. Rejected goods will be held by Buyer pending Seller’s instructions at Seller’s cost and risk.
                        Buyer also requires Seller to arrange for the rectification or replacement of such non- conforming, damaged or defective goods at Seller’s cost and expense. Where specifications for goods require approval from external certifying body, Seller shall also comply with those requirements.
                        Acknowledgement of delivery or payment for goods prior to inspection shall not be deemed to constitute acceptance of the goods or a waiver of Buyer’s right to reject the same
                        </label>   
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

