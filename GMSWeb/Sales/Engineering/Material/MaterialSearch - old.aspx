<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="MaterialSearch - old.aspx.cs" Inherits="GMSWeb.Sales.Engineering.Material.MaterialSearch" Title="Material Search" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="Header1" Src="~/SiteHeader1.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header1 ID="MySiteHeader2" runat="server" EnableViewState="true" />


<script src="MaterialSearch.aspx.js" type="text/javascript"></script>
<div class="panel panel-custom margin-bottom-40">
		<div class="panel-heading">
			<h1 class="panel-title">Sales > Material > Material/Equipment Information</h1>
			<input type="hidden" class="form-control" id="hidAllowChanges" name="hidAllowChanges"/>
		</div>
		
		<div class="panel-body">
            <div id="content">
                <!-- Row 1 -->
                <div class="row">
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label for="txtProdCode">Item Code:</label>
                            <input type="text" class="form-control" id="txtProdCode" name="ProdCode" tabindex="1" />
                        </div> 
                    </div>
                    
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label for="txtItemName">Item Name:</label>
                            <input type="text" class="form-control" id="txtItemName" name="ItemName" tabindex="1" />
                        </div> 
                    </div>
                    
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label for="txtItemCategory">Item Category:</label>
                            <input type="text" class="form-control" id="txtItemCategory" name="ItemCategory" tabindex="2"/>
                        </div>
                    </div>
                    
                    <div class="col-sm-3">    
                        <div class="form-group">
                            <label for="txtSupplierName">Supplier Name:</label>
                           <input type="text" class="form-control" id="txtSupplierName" tabindex="4"/>
                        </div>    
                    </div>
                </div>
                <!-- End Row 1 -->
            </div> 

            <p></p>
             <div class="row">
			    <div class="col-sm-12">
                    <div class="btn-group btn-group-justified">                 
                      <a href="#" class="btn btn-primary" id="btnAdd" data-toggle="modal" data-target="#SaveMaterialRecord"  data-backdrop="static" data-keyboard="false">Create New</a>
                      <a href="#" class="btn btn-primary active" id="btnSearch">Search</a>
                    </div>
                </div>
            </div>
            <p></p>
            <table id="tblMaterial" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
        </div>
    </div>
    <!-- Modal : Hidden Form Input-->
    <div class="modal fade" id="SaveMaterialRecord" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
        <div class="modal-dialog">
            
            <div class="modal-content" >
                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                           <span aria-hidden="true">&times;</span>
                           <span class="sr-only">Close</span>
                    </button>
                    <h4 class="modal-title" id="H4_1">Product Information</h4>
                </div>
                
                <!-- Modal Body -->
                <div class="modal-body" id="MaterialRecordDetail">
                   <form role="form">
                   <!-- Tabs -->
                        <div class="tab-v2">
                            <ul class="nav nav-tabs">
                                <li class="active" id="t1"><a href="#tab1" data-toggle="tab">Information</a></li>
                                <li id="t2"><a href="#tab2" data-toggle="tab">Price History</a></li>
                                <li id="Li1"><a href="#tab3" data-toggle="tab">Attachment</a></li>
                            </ul>
	                        <div class="tab-content">
	                            <!-- Child Tab 1 -->
		                        <div class="tab-pane fade in active" id="tab1">
		                            <div class="container">
		                                <!-- Row 1 -->
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <label for="ProdCode" class="control-label">ProdCode</label>
                                                <input type="text" id="ProdCode" name="ProdCode" maxlength="11" class="form-control"  placeholder=""/>
                                            </div>
                                            <div class="col-sm-6">
                                                <label for="ItemName" class="control-label">Name</label>
                                                <input type="text" id="ItemName" name="ItemName" maxlength="80" class="form-control"  placeholder=""/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                              <div class="form-group">
                                                <label for="ItemCategory">Category</label>
                                                <input type="text" id="ItemCategory" name="ItemCategory" maxlength="80" class="form-control" placeholder=""/>
                                              </div>
                                            </div>
                                            <div class="col-sm-6">
                                              <div class="form-group">
                                                <label for="ItemType">Type</label>
                                                <input type="text" id="ItemType" name="ItemType" class="form-control"/>
                                              </div>
                                            </div>
                                        </div>
                                        <div class="row">     
                                            <div class="col-sm-6">
                                              <div class="form-group">
                                                <label for="ItemMaterial">Material</label>
                                                  <input type="text" id="ItemMaterial" name="ItemMaterial" class="form-control" placeholder=""/>
                                              </div>
                                            </div>
                                            <div class="col-sm-6">
                                              <div class="form-group has-feedback">
                                                <label for="ItemLeadtime">Leadtime (days)</label>
                                                <input type="text" id="ItemLeadtime" name="ItemLeadtime" class="form-control" placeholder="" />
                                              </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                              <div class="form-group has-feedback">
                                                <label for="SupplierCode">Supplier Code</label>
                                                <input type="text" id="SupplierCode" name="SupplierCode" class="form-control" placeholder="" />
                                              </div>
                                            </div>
                                            
                                            <div class="col-sm-6">
                                              <div class="form-group has-feedback">
                                                <label for="SupplierName">Supplier Name</label>
                                                <input type="text" id="SupplierName" name="SupplierName" class="form-control" placeholder="" />
                                              </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                              <div class="form-group has-feedback">
                                                <label for="ItemSize">Size</label>
                                                <input type="text" id="ItemSize" name="ItemSize" class="form-control" placeholder=""/>
                                                <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                                <div class="help-block with-errors"></div>
                                              </div>
                                            </div>
                                            
                                            <div class="col-sm-6">
                                              <div class="form-group has-feedback">
                                                <label for="ItemBrand">Brand</label>
                                                <input type="text" id="ItemBrand" name="ItemBrand" class="form-control" placeholder="" />
                                              </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                              <div class="form-group">
                                                <label for="ItemSize">Currency Code</label>
                                                <input type="hidden" id="CurrencyCode" name="CurrencyCode" class="form-control" placeholder=""/>
                                                <input type="text" id="NewCurrencyCode" name="NewCurrencyCode" class="form-control" placeholder=""/>
                                              </div>
                                            </div>
                                            
                                            <div class="col-sm-6">
                                              <div class="form-group">
                                                <label for="UnitPrice">Current Price</label>
                                                <input type="hidden" id="UnitPrice" name="UnitPrice" class="form-control" placeholder="" />
                                                <input type="text" id="NewUnitPrice" name="NewUnitPrice" class="form-control" placeholder="" />
                                              </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                              <div class="form-group">
                                                <label for="QuotationDate">Quotation Date</label>
                                                <input type="hidden" id="QuotationDate" name="QuotationDate" class="form-control" placeholder=""/>
                                                <div class='input-group date' id='datetimepicker1'>
                                                    <input type="text" id="NewQuotationDate" name="NewQuotationDate" class="form-control" placeholder=""/>
                                                    <span class="input-group-addon">
                                                        <span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                </div>
                                              </div>
                                            </div>
                                            
                                            <div class="col-sm-6">
                                              <div class="form-group">
                                                <label for="QuotationValidity">Quotation Validity (days)</label>
                                                <input type="hidden" id="QuotationValidity" name="QuotationValidity" class="form-control" placeholder="" />
                                                <input type="text" id="NewQuotationValidity" name="NewQuotationValidity" class="form-control" placeholder="" />
                                              </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                              <div class="form-group">
                                                <label for="ItemDescription">Description</label>
                                                <textarea id="ItemDescription" name="ItemDescription" class="form-control" rows="5"></textarea>
                                              </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            
                                            <div class="col-sm-6" id="checkbox">
                                                <div class="form-group">
                                                    <div class="col-sm-12 columns table-bordered" style="padding: 10px 5px 10px 10px;">
                                                    <label for="Checkboxes"><font color="red">* Deactivate item:</font></label> 
                                                        <input type="checkbox" name="IsActive" id="chkIsActive" value="False" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- End Row 6 -->
                                    </div>
		                        </div>
		                        <!-- End Child Tab 1 -->
            		            
		                        <!-- Child Tab 2 -->
		                        <div class="tab-pane fade in" id="tab2">
		                            <table id="tblMaterialPriceList" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
		                        </div>
		                        <!-- End Child Tab 2 -->
		                        
		                        <!-- Child Tab 3 -->
		                        <div class="tab-pane fade in" id="tab3">
		                        <div id="view" style="display:none;">
		                         <input id="input-706" name="kartik-input-706[]" type="file" multiple=false class="file-loading">
                            
                            <script>
                                var $el2 = $("#input-706");
                                // custom footer template for the scenario
                                // the custom tags are in braces
                                var footerTemplate = '<div class="file-thumbnail-footer">\n' +
                                '   <div style="margin:5px 0">\n' +
                                '       <input class="kv-input kv-new form-control input-sm text-center {TAG_CSS_NEW}" value="{caption}" placeholder="Enter caption...">\n' +
                                '       <input class="kv-input kv-init form-control input-sm text-center {TAG_CSS_INIT}" value="{TAG_VALUE}" placeholder="Enter caption...">\n' +    
                                '       <input class="kv-input kv-coyid form-control input-sm text-center {TAG_CSS_NEW}" type="hidden" value="115" placeholder="Enter caption...">\n' +  
                                '   </div>\n' +
                                '   {size}\n' +
                                '   {actions}\n' +
                                '</div>';
                                 
                                $el2.fileinput({
                                    uploadUrl: 'FileUploadHandler.ashx',
                                    uploadAsync: true,
                                    maxFileCount: 5,
                                    overwriteInitial: false,
                                    layoutTemplates: {footer: footerTemplate, size: '<samp><small>({sizeText})</small></samp>'},
                                    previewThumbTags: {
                                        '{TAG_VALUE}': '',        // no value
                                        '{TAG_CSS_NEW}': '',      // new thumbnail input
                                        '{TAG_CSS_INIT}': 'hide'  // hide the initial input                                       
                                    },
                                    showBrowse: false,
                                    showPreview: true,
                                    browseOnZoneClick: true,
                                    uploadExtraData: function() {  // callback example
                                        var out = {}, key, i = 0;
                                        $('.kv-input:visible').each(function() {
                                            $el = $(this);
                                            if($el.hasClass('kv-new'))
                                                key = 'new_' + i;
                                            else if($el.hasClass('kv-category'))
                                                key = 'category_' + i;
                                            else if($el.hasClass('kv-init'))
                                                key = 'init_' + i;
                                            else if($el.hasClass('kv-coyid'))
                                                key = 'coyid_' + i;   
                                            //key = $el.hasClass('kv-new') ? 'new_' + i : 'init_' + i;
                                            out[key] = $el.val();
                                            i++;
                                        });
                                        return out;
                                    }
                                });
                                
                                $('#input-706').on('fileuploaded', function(event, data, previewId, index, jqXHR) {
                                    var temp = data["response"];
                                    SaveMaterialUpload(temp, $('#btnMaterialSave').val());     
                                });
                                </script>
                            <p></p>
                            </div>
    					    <table id="tblAttachment" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>
    					    <iframe id="myDownloaderFrame" style="display:none;" ></iframe>
					    </div>
					    
		                        <!-- End Child Tab 3 -->
	                        </div>
                        </div>
                        <!-- End Tab v2 -->
                   
                        
                   </form>
                </div>
                
                <!-- Modal Footer -->
                <div class="modal-footer" id="btnGroup">
                    <div class="form-group">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="btnMaterialSave" name="Edit" data-dismiss="modal">Save</button>
                    </div>
                </div>
            </div>
        </div>
        
    <!-- dialog window markup -->
    <div id="dialog" title="Your session is about to expire!">
	    <p>
		    <span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 50px 0;"></span>
		    You will be logged off in <span id="dialog-countdown" style="font-weight:bold"></span> seconds.
	    </p>
	    <p>Do you want to continue your session?</p>
    </div>

</asp:Content>