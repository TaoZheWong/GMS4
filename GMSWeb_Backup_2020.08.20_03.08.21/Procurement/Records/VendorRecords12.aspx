<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorRecords12.aspx.cs" Inherits="GMSWeb.Procurement.Records.VendorRecords12" %>
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
                             <b>1.	Acceptance of Conditions</b> 
                        </label>
                        <div class="col-sm-12">
                            <label class="col-sm-12 control-label text-left text-justify">
                           Acceptance of the Purchase Order constitutes unconditional acceptance of these terms and conditions of purchase for equipment or products or materials or any component thereof (the Goods) and for services. Unless expressly agreed by Buyer, no addition to or modification of these terms and conditions will be accepted nor will any alternative terms and conditions submitted by Seller be recognized. Buyer’s receipt and acceptance of the goods or services shall not in any way imply its consent or acceptance to any additional or modified terms and conditions proposed by Seller.
                        </label>
                            <input type="hidden" id="hidFormID5" runat="server" />
                            
                        </div>
                    </div>

                        <div class="form-group col-lg-12">
                        <div class="col-sm-12">
                            <label class="col-sm-12 control-label text-left text-justify">
                          For the avoidance of doubt, the General Terms and Conditions of Purchases Order and/or any terms contained in the Purchase Order shall take precedence and/or prevail in the event of any discrepancy or inconsistency with any form of Seller’s Terms and Conditions of Contract.
                        </label>
                            <input type="hidden" id="Hidden3" runat="server" />
                            
                        </div>
                    </div>

                         <div class="form-group col-lg-12">
                        <div class="col-sm-12">
                            <label class="col-sm-12 control-label text-left text-justify">
                         Seller shall be deemed to fully accept, all the terms and conditions contained in the Purchase Order unless objections or clarifications are notified to Buyer in writing within 3 days of the receipt of Purchase Order by the Seller.
                        </label>
                            <input type="hidden" id="Hidden4" runat="server" />
                            
                        </div>
                    </div>             
                   </div>  
                    
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left text-underline">
                             <b>2.	Payment</b> 
                        </label>
                        <div class="col-sm-12">
                            <label class="col-sm-12 control-label text-left text-justify ">
                          The Company shall be responsible for payment of Goods only if ordered on the Company’s PO form. No Goods are to be delivered without a PO. Unless otherwise agreed to between the parties, payment for goods and services accepted shall be made only after 45 days of completion of delivery; and receipt of all supporting documentation and Seller’s invoice.
                        </label>   
                        </div>
                    </div>
                   </div>      
                    
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left text-underline">
                             <b>3.	Prices</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                          Unless otherwise stated in the PO, the prices of the Goods stated in the PO are expressed in Singapore currency and shall be inclusive of the cost of carriage and packing, all necessary taxes (including Goods & Services Tax), duties, royalties and tariffs, if applicable. Prices stated in the Purchase Order shall prevail. Seller hereby undertakes to afford to Buyer any general reduction in prices as may from time to time occur and any discount given by Seller to its other customers.
                        </label>   
                        </div>
                    </div>
                   </div>   

                     <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left text-underline">
                             <b>4.	Delivery and Cancellation</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                         The Supplier shall deliver the Goods to the Company in the correct quantity and specification, properly and securely packed, at the time, place and in the manner specified in the PO (“Delivery Conditions”). Goods delivered which do not meet the specification specified in the PO shall be returned to the Supplier, at the Supplier’s expense. If the Supplier cannot meet the Delivery Conditions, the Supplier shall inform the Company immediately in writing of the earliest possible date the Delivery Conditions can be made. The Company reserves the rights to cancel the PO if the proposed revised date is not acceptable to the Company
                        </label>   
                        </div>
                    </div>
                   </div> 

                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left text-underline">
                             <b>5.	Invoice</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                         Separate invoices are required for each shipment. Payment for goods will be made against presentation of original invoice
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
 
