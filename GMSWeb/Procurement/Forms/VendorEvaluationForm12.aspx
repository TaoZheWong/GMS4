<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorEvaluationForm12.aspx.cs" Inherits="GMSWeb.Procurement.Forms.VendorEvaluationForm12" %>
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
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left">
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
                        <label class="col-sm-12 control-label text-left">
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
                        <label class="col-sm-12 control-label text-left">
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
                        <label class="col-sm-12 control-label text-left">
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
                        <label class="col-sm-12 control-label text-left">
                             <b>5.	Invoice</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                         Separate invoices are required for each shipment. Payment for goods will be made against presentation of original invoice
                        </label>   
                        </div>
                    </div>
                   </div> 

                      <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left">
                             <b>6.	Conformity and certification</b> 
                        </label>
                        <div class="col-sm-12">
                            <label class="col-sm-12 control-label text-left text-justify">
                           The Company reserves the right to request for certificates of origin and/or test certificates for the Goods. Such certificates must clearly state the Company’s PO number and the serial number of the Goods.
                        </label>
                            <input type="hidden" id="Hidden2" runat="server" />
                            
                        </div>
                    </div>            
                   </div>  
                    
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left">
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
                        <label class="col-sm-12 control-label text-left">
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
                        <label class="col-sm-12 control-label text-left">
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
                        <label class="col-sm-12 control-label text-left">
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

                        <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left">
                             <b>11.	Title and Risk</b> 
                        </label>
                            <input type="hidden" id="Hidden5" runat="server" />
                        <div class="col-sm-12">
                        <label class="col-sm-12 control-label text-left text-justify">
                          Title to the goods shall pass to Buyer upon the earlier of payment, delivery, or acceptance of the goods by Buyer, to the extent of and proportional to the amount of money paid by Buyer for the goods.
                        </label>           
                        </div>
                    </div>

                         <div class="form-group col-lg-12">
                        <div class="col-sm-12">
                        <label class="col-sm-12 control-label text-left text-justify">
                         Risk for loss or damage in the goods shall remain with Seller and shall pass to Buyer on delivery and acceptance of the goods at Buyer’s premises.
                        </label>           
                        </div>
                    </div>            
                   </div>  
                    
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left">
                             <b>12.	Insurance</b> 
                        </label>
                        <div class="col-sm-12">
                            <label class="col-sm-12 control-label text-left text-justify ">
                         Seller shall be responsible at its cost and expense to provide necessary insurance for the goods against loss or damage until delivery and acceptance of the goods by Buyer
                        </label>   
                        </div>
                    </div>
                   </div>      
                    
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left">
                             <b>13.	Intellectual Property Rights</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                         Seller warrants that the goods sold do not infringe the intellectual property rights of any third parties, and Seller indemnifies and holds harmless Buyer, its customers and users of the goods from and against all losses, damages, liability, claims, demands, costs and expenses for any actual or alleged infringement of any patent, registered design, trademark, copyright or other intellectual property rights of third parties, in relation to the goods supplied or any use or resale by Buyer of such goods.
                        </label>   
                        </div>
                    </div>
                   </div>   

                     <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left">
                             <b>14.	Indemnity</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                         Seller shall be absolutely and solely responsible for and fully indemnifies and keeps Buyer indemnified against accidents, injuries, death, damages and losses to any person, property and things as set out below
                        </label>   
                        </div>
                       <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                        •	Persons employed by Seller or any of his subcontractors.  
                        </label>   
                           <label class="col-sm-12 control-label text-left text-justify ">
                        •	All tools, equipment and machinery brought into Buyer’s premises or worksites for the performance of Seller’s scope of work 
                        </label> 
                        </div>
                    </div>
                   </div> 

                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left">
                             <b>15.	Confidentially</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                        All information or document, either in verbal or written form, provided by Buyer which may otherwise be acquired by Seller including but not limited to specification and drawing relating to this Purchase Order shall be treated as confidential information by Seller and shall not be disclosed to any third party without Buyer prior written consent.
                        </label>   
                        </div>
                    </div>
                   </div> 

                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left">
                             <b>16.	Governing Law</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                       This PO shall be governed by the laws of Singapore.
                        </label>   
                        </div>
                    </div>
                   </div>               
          
                  <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left">
                             <b>17.	Arbitration</b> 
                        </label>
                        <div class="col-sm-12">
                            <label class="col-sm-12 control-label text-left text-justify">
                          Any disputes arising out of the purchase order or its performance which cannot be settled amicably by the Buyer and the Seller shall be referred to and finally resolved by arbitration in Singapore in the English language by a sole arbitrator in accordance with the Arbitration Rules of the Singapore International Arbitration Centre for the time being in force which rules are deemed to be incorporated by reference into this Clause
                        </label>
                            <input type="hidden" id="Hidden6" runat="server" />                            
                        </div>
                    </div>
                    
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                        <label class="col-sm-12 control-label text-left">
                             <b>18.	Amended or Additional Terms</b> 
                        </label>
                        <div class="col-sm-12">
                            <label class="col-sm-12 control-label text-left text-justify ">
                          Where Seller and Buyer subsequently enter into a written agreement containing different terms and conditions in respect of the goods, the terms and conditions in respect of that written agreement shall prevail over the terms and conditions herein. 
                        </label>   
                        </div>
                    </div>
                   </div>      
                     
                </div>     
        </div>
      </div>

            
                 <div class="panel-footer clearfix">
                       <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                        OnClick="btnSubmit_Click"></asp:Button>
                       <asp:Button ID="btnBack" Text="Back" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                       ValidationGroup="valGrpNewRow" OnClick="btnBack_Click"></asp:Button>      
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
 
