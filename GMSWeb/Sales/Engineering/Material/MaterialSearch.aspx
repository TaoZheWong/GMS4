<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="MaterialSearch.aspx.cs" Inherits="GMSWeb.Sales.Engineering.Material.MaterialSearch" Title="Material Search" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
<ul class="breadcrumb pull-right">
    <li><a href="#">Sales</a></li>
    <li class="active">Material</li>
</ul>
<h1 class="page-header">Material/Equipment Information
</h1>

<div class="panel panel-primary">
		<div class="panel-heading">
		    <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
		    <i class="ti-search"></i>
                Search filter
			<input type="hidden" class="form-control" id="hidAllowChanges" name="hidAllowChanges"/>
		</div>
		<div class="panel-body row">
		    <div class="m-t-20">
                <input type="hidden" id="hidCoyID" runat="server" value="" />	
		        <input type="hidden" id="hidUserID" runat="server" value="" />
		        <input type="hidden" id="hidCurrentLink" runat="server" value="" />
         
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label" for="txtProdCode">Model No:</label>
                        <input type="text" class="form-control" id="txtModelNo" name="ModelNo" tabindex="1" />
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label" for="txtItemName">Description:</label>
                        <input type="text" class="form-control" id="txtDescription" name="Description" tabindex="2" />
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label" for="txtItemCategory">Item Category:</label>
                        <input type="text" class="form-control" id="txtItemCategory" name="ItemCategory" tabindex="3"/>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label" for="txtSupplierName">Supplier Name:</label>
                        <input type="text" class="form-control" id="txtSupplierName" tabindex="4"/>
                </div>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <a href="#" class="btn btn-primary pull-right" id="btnSearch" tabindex="5">Search</a>                
            <a href="#" class="btn btn-default pull-right m-r-10" id="btnAdd" data-toggle="modal" data-target="#SaveMaterialRecord"  data-backdrop="static" data-keyboard="false">Create New</a>
        </div>
    </div>

    <table id="tblMaterial" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
    

    <!-- Modal : Hidden Form Input-->
    <div class="modal fade" id="SaveMaterialRecord" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
        <div class="modal-dialog modal-lg">
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
                <div class="modal-body no-padding" id="MaterialRecordDetail">
                   <form role="form">
                   <!-- Tabs -->
                        <div class="tab-v2 m-t-10">
                            <ul class="nav nav-tabs">
                                <li class="active" id="t1"><a href="#tab1" data-toggle="tab">Information</a></li>
                                <li id="t2"><a href="#tab2" data-toggle="tab">Price History</a></li>
                                <li id="Li1"><a href="#tab3" data-toggle="tab">Attachment</a></li>
                            </ul>
	                        <div class="tab-content">
	                            <!-- Child Tab 1 -->
		                        <div class="tab-pane fade in active" id="tab1">
		                            <div class="container-fluid">
		                                <!-- Row 1 -->
                                        <div class="row">
                                            <div class="col-sm-12">
                                              <div class="form-group">
                                                <label for="ItemDescription"><sup><font color="red">*</font></sup>Description</label>
                                                <textarea id="ItemDescription" name="ItemDescription" class="form-control" rows="5"></textarea>
                                              </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <label for="Model" class="control-label"><sup><font color="red">*</font></sup>Model</label>
                                                <input type="text" id="ModelNo" name="ModelNo" maxlength="11" class="form-control"  placeholder=""/>
                                            </div>
                                            <div class="col-sm-6">
                                              <div class="form-group">
                                                <label for="ItemCategory"><sup><font color="red">*</font></sup>Category</label>
                                                <input type="text" id="ItemCategory" name="ItemCategory" maxlength="80" class="form-control" placeholder=""/>
                                              </div>
                                            </div>
                                        </div>
                                        <div class="row">     
                                            <div class="col-sm-6">
                                              <div class="form-group">
                                                <label for="ItemMaterial"><sup><font color="red">*</font></sup>Material</label>
                                                  <input type="text" id="ItemMaterial" name="ItemMaterial" class="form-control" placeholder=""/>
                                              </div>
                                            </div>
                                            <div class="col-sm-6">
                                              <div class="form-group has-feedback">
                                                <label for="ItemBrand"><sup><font color="red">*</font></sup>Brand</label>
                                                <input type="text" id="ItemBrand" name="ItemBrand" class="form-control" placeholder="" />
                                              </div>
                                            </div>
                                            
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                              <div class="form-group has-feedback">
                                                <label for="SupplierName"><sup><font color="red">*</font></sup>Supplier Name</label>
                                                <input type="text" id="SupplierName" name="SupplierName" class="form-control" placeholder="" />
                                              </div>
                                            </div>
                                            <div class="col-sm-6">
                                              <div class="form-group has-feedback">
                                                <label for="ItemSize"><sup><font color="red">*</font></sup>Size</label>
                                                <input type="text" id="ItemSize" name="ItemSize" class="form-control" placeholder=""/>
                                                <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                                <div class="help-block with-errors"></div>
                                              </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                              <div class="form-group">
                                                <label for="ItemSize"><sup><font color="red">*</font></sup>Currency</label>
                                                <input type="hidden" id="CurrencyCode" name="CurrencyCode" class="form-control" placeholder=""/>
                                                <input type="text" id="NewCurrencyCode" name="NewCurrencyCode" class="form-control" placeholder=""/>
                                              </div>
                                            </div>
                                            <div class="col-sm-6">
                                              <div class="form-group">
                                                <label for="UnitPrice"><sup><font color="red">*</font></sup>Current Price</label>
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
                                                <div class='input-group date_yyyy_mm_dd' id='datetimepicker1'>
                                                    <input type="text" id="NewQuotationDate" name="NewQuotationDate" class="form-control" placeholder=""/>
                                                    <span class="input-group-addon">
                                                        <i class="ti-calendar"></i>
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
                                            <div class="col-sm-6">
                                              <div class="form-group has-feedback">
                                                <label for="ItemLeadtime">Leadtime (days)</label>
                                                <input type="text" id="ItemLeadtime" name="ItemLeadtime" class="form-control" placeholder="" />
                                              </div>
                                            </div>
                                            <div class="col-sm-6" id="checkbox">
                                                <div class="form-group">
                                                    <div class="col-sm-12 columns table-bordered" style="padding: 10px 5px 10px 10px;">
                                                    <label for="Checkboxes"><font color="red">* Deactivate item:</font></label> 
                                                        <input type="checkbox" name="IsActive" id="chkIsActive" value="False" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
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
                        <button type="button" class="btn btn-primary" id="btnMaterialSave" name="Edit">Save</button>
                    </div>
                </div>
            </div>
        </div>
    </div>    
    <!-- dialog window markup -->

        <div class="modal fade" id="ModalMessage" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    
                    <!-- Modal Body -->
                    <div class="modal-body">
                        <span id="message" name="message"></span>
                    </div> 
                    <!-- Modal Footer -->
                    <div class="modal-footer"> 
                        <button type="button" class="btn btn-primary" id="btnClose" name="Close">Close</button>
                    </div>
                </div>
            </div>
        </div> 
        
        <div class="modal fade" id="confirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
          <div class="modal-dialog">
                <div class="modal-content">
                    
                    <!-- Modal Body -->
                    <div class="modal-body">
                        Are you sure?
                    </div> 
                    <!-- Modal Footer -->
                    <div class="modal-footer"> 
                        <button type="button" data-dismiss="modal" class="btn btn-primary" id="delete">Delete</button>
                        <button type="button" data-dismiss="modal" class="btn">Cancel</button>
                    </div>
                </div>
            </div>
        </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script>
        var $el2 = $("#input-706");
        var CoyID = getCoyID();

        // custom footer template for the scenario
        // the custom tags are in braces
        var footerTemplate = '<div class="file-thumbnail-footer">\n' +
        '   <div style="margin:5px 0">\n' +
        '       <input class="kv-input kv-new form-control input-sm text-center {TAG_CSS_NEW}" value="{caption}" placeholder="Enter caption...">\n' +
        '       <input class="kv-input kv-init form-control input-sm text-center {TAG_CSS_INIT}" value="{TAG_VALUE}" placeholder="Enter caption...">\n' +
        '       <input class="kv-input kv-coyid form-control input-sm text-center {TAG_CSS_NEW}" value="' + CoyID + '" placeholder="Enter caption..." readonly>\n' +
        '   </div>\n' +
        '   {size}\n' +
        '   {actions}\n' +
        '</div>';

        $el2.fileinput({
            uploadUrl: 'FileUploadHandler.ashx',
            uploadAsync: true,
            maxFileCount: 5,
            overwriteInitial: false,
            layoutTemplates: { footer: footerTemplate, size: '<samp><small>({sizeText})</small></samp>' },
            previewThumbTags: {
                '{TAG_VALUE}': '',        // no value
                '{TAG_CSS_NEW}': '',      // new thumbnail input
                '{TAG_CSS_INIT}': 'hide'  // hide the initial input                                       
            },
            showBrowse: false,
            showPreview: true,
            browseOnZoneClick: true,
            uploadExtraData: function () {  // callback example
                var out = {}, key, i = 0;
                $('.kv-input:visible').each(function () {
                    $el = $(this);
                    if ($el.hasClass('kv-new'))
                        key = 'new_' + i;
                    else if ($el.hasClass('kv-category'))
                        key = 'category_' + i;
                    else if ($el.hasClass('kv-init'))
                        key = 'init_' + i;
                    else if ($el.hasClass('kv-coyid'))
                        key = 'coyid_' + i;
                    //key = $el.hasClass('kv-new') ? 'new_' + i : 'init_' + i;
                    out[key] = $el.val();
                    i++;
                });
                return out;
            }
        });

        $('#input-706').on('fileuploaded', function (event, data, previewId, index, jqXHR) {
            var temp = data["response"];
            SaveMaterialUpload(temp, $('#btnMaterialSave').val());
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".project-menu").addClass("active expand");
            $(".sub-material").addClass("active");
        });
    </script>
    <script src="MaterialSearch.aspx.js" type="text/javascript"></script>
</asp:Content>
