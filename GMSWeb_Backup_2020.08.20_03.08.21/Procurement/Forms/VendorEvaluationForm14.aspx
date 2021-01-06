<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorEvaluationForm14.aspx.cs" Inherits="GMSWeb.Procurement.Forms.VendorEvaluationForm14" %>
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
                        <label class="col-sm-12 control-label text-left text-underline">
                             <b>11.	Title and Risk</b> 
                        </label>
                            <input type="hidden" id="hidFormID5" runat="server" />
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
                        <label class="col-sm-12 control-label text-left text-underline">
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
                        <label class="col-sm-12 control-label text-left text-underline">
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
                        <label class="col-sm-12 control-label text-left text-underline">
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
                        <label class="col-sm-12 control-label text-left text-underline">
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
                        <label class="col-sm-12 control-label text-left text-underline">
                             <b>16.	Governing Law</b> 
                        </label>
                        <div class="col-sm-12">
                      <label class="col-sm-12 control-label text-left text-justify ">
                       This PO shall be governed by the laws of Singapore.
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
