    <%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
	Codebehind="ProductDetail.aspx.cs" Inherits="GMSWeb.Products.Products.ProductDetail"
	Title="Products - Product Detail" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="breadcrumb pull-right">
        <li><a href="#">Products</a></li>
        <li class="active">Item Detail</li> 
    </ul>
    <h1 class="page-header">Item Detail </h1>

	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>
  
    <style>
        html .upload-container {
            display: inline-block;
            text-align: center;
            padding-top:20px;
            width:100%;
        }
 
        html .upload-container .RadUpload .ruUploadProgress {
            width: 210px;
            display: inline-block;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            vertical-align: top;
        }

        html .upload-container .ruBrowse{
            background-position: 0 -23px;
            width: 79px;
            background-image:url("../../images/ruSprite.png");
        }

        .RadUpload_Bootstrap .ruSelectWrap .ruButton {
           padding:0px 0px;
        }

        html .upload-container .ruFileWrap .ruButtonHover{
            background-position: 100% -23px;
        }
 
        html .upload-container .ruFakeInput {
            width: 200px;
        }

        .RadImageGallery .rigItemBox {
            background-color:lightgrey;
        }

        .RadImageGallery .rigDotList>a{
            padding:0px;
        }
   </style>
    
	<input type="hidden" id="hidProductCode" runat="server" />
    <input type="hidden" id="hidProductGroupCode" runat="server" />
    <input type="hidden" id="hidTrackType" runat="server" />
	
    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                Item Information
            </h4>
        </div>
        <div class="panel-body row">
            <div class="form-vertical m-t-10">
                <div class="form-group col-lg-6">
                    <div class="form-group col-lg-12 col-sm-12 col-md-12">
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                            Item Code
                        </label>
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           <asp:Label runat="server" ID="lblProductCode"></asp:Label>
                        </label>
                    </div>
                    <div class="form-group col-lg-12 col-sm-12 col-md-12">
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                            Item Description
                        </label>
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           <asp:Label runat="server" ID="lblProductName"></asp:Label>
                        </label>
                    </div>
                    <div class="form-group col-lg-12 col-sm-12 col-md-12">
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                            Brand/Product Name
                        </label>
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           <asp:Label runat="server" ID="lblProductGroup"></asp:Label>
                        </label>
                    </div>
                    <div class="form-group col-lg-12 col-sm-12 col-md-12">
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                            Product Manager Name
                        </label>
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           <asp:Label runat="server" ID="lblProductManagerName"></asp:Label>
                        </label>
                    </div>
                    <div class="form-group col-lg-12 col-sm-12 col-md-12">
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                            UOM
                        </label>
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           <asp:Label runat="server" ID="lblUOM"></asp:Label>
                            &nbsp;&nbsp;
                            <a id="lnkViewMultipleUOM" runat="server"  onclick="return ViewMultipleUOM();" href="#">
                                View Conversion Factor</a>
                        </label>
                    </div>
                    <div class="form-group col-lg-12 col-sm-12 col-md-12">
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                            Country Of Origin
                        </label>
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           <asp:Label runat="server" ID="lblCountry"></asp:Label>
                        </label>
                    </div>
                    <div class="form-group col-lg-12 col-sm-12 col-md-12">
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                            Dealer Price
                        </label>
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           <asp:Label runat="server" ID="lblDprice"></asp:Label>
                        </label>
                    </div>
                    <div class="form-group col-lg-12 col-sm-12 col-md-12">
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                            User Price 
                        </label>
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           <asp:Label runat="server" ID="lblUprice"></asp:Label>
                        </label>
                    </div>
                    <div class="form-group col-lg-12 col-sm-12 col-md-12">
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           Retail Price
                        </label>
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           <asp:Label runat="server" ID="lblRprice"></asp:Label>
                        </label>
                    </div>
                     <div class="form-group col-lg-12 col-sm-12 col-md-12">
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           Effective Date
                        </label>
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                           <asp:Label runat="server" ID="lblEffectiveDate"></asp:Label>
                        </label>
                    </div>
                    <div id="PMRegion1" runat="server" class="form-group col-lg-12 col-sm-12 col-md-12">
                            <label class="col-sm-6 col-lg-6 control-label text-left">
                                Weighted Cost
                            </label>
                            <label class="col-sm-6 col-lg-6 control-label text-left">
                               <asp:Label runat="server" ID="lblWeightedCost"></asp:Label>
                            </label>
                    </div>
                    <div class="form-group col-lg-12 col-sm-12 col-md-12">
                        <label class="col-sm-6 col-lg-6 control-label text-left">
                            Remarks
                        </label>
                        <div class="col-sm-6 col-lg-6">
                           <asp:TextBox runat="server" ID="txtRemarks" TextMode="MultiLine" CssClass="form-control" onfocus="select();" onchange="this.value = this.value.toUpperCase()"></asp:TextBox>
				
                        </div>
                    </div>
				    <div class="form-group col-lg-12 col-sm-12">
                        <label class="control-label text-left">
                             <asp:Label ID="lblModifiedDate" runat="server" Text=""></asp:Label>
                        </label>
                    </div>     
                    <div class="form-group col-lg-12 col-sm-12">
                        <label class="control-label text-left">
                             <span style="color:Red; size:7px; font-style:italic;"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></span>
                        </label>
                    </div>
                </div> 
                <div class="form-group col-lg-6">
                    <telerik:RadImageGallery  RenderMode="Lightweight" runat="server" ID="RadImageGallery" DisplayAreaMode="Image" Skin="Bootstrap" OnNeedDataSource="RadImageGallery1_NeedDataSource"
                        DataTitleField="" DataDescriptionField="" DataImageField="ImageData" DataThumbnailField="ThumbnailData" Width="100%" Height="300px" >
                        <ImageAreaSettings Height="300px" ShowDescriptionBox="false" />
                         <ThumbnailsAreaSettings Mode="ImageSlider"  />
                        <ToolbarSettings ShowSlideshowButton="false" Position="Bottom"/>
                    </telerik:RadImageGallery>

                    <div class="upload-container size-narrow" id="PMRegion7" runat="server" >
                        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="RadAsycnUpload1" OnClientValidationFailed="OnClientValidationFailed"
                          OnFileUploaded="RadAsycnUpload1_onFileUploaded"  AllowedFileExtensions="jpg,jpeg,png,gif" MultipleFileSelection="Automatic" MaxFileSize="5097152"/>
                        <asp:Label ID="Label1" runat="server" Text="Only Image files within 5MB size are accepted."></asp:Label>
                        <telerik:RadProgressArea RenderMode="Lightweight" runat="server" ID="RadProgressArea1"  />
                        <br>
                        <asp:LinkButton runat="server" ID="btnUpload" OnClick="btnUpload_Click">Upload Image</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;/&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton runat="server" ID="btnDelete" OnClick="btnDelete_Click">Delete Current Image</asp:LinkButton>
                    </div>
                </div>    
            </div>
        </div>
        <div class="panel-footer clearfix">
              <asp:Button runat="server" ID="btnSave" Text="Update" CssClass="btn btn-primary pull-right" OnClick="btnSave_Click" /><br />
				        
        </div>
    </div>
	
	<div id="PMRegion6" runat="server" class="panel panel-primary" hidden>
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                Selling Price
            </h4>
        </div>
        <div class="panel-body no-padding">
            <div class="table-responsive">
		        <asp:DataGrid ID="dgData4" runat="server" AutoGenerateColumns="false" GridLines="none" 
							        CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" EnableViewState="true">
							        <Columns>
								        <asp:TemplateColumn HeaderText="Country of Origin" HeaderStyle-Wrap="false" ItemStyle-Width="200px">
									        <ItemTemplate>
										        <asp:Label ID="lblCountry" runat="server"><%# Eval("Country")%></asp:Label>
									        </ItemTemplate>
								        </asp:TemplateColumn>
								        <asp:TemplateColumn HeaderText="Dealer Price(S$)" HeaderStyle-Wrap="false" ItemStyle-Width="100px">
									        <ItemTemplate>
										        <asp:Label ID="lblDealerPrice" runat="server"><%# Eval("DealerPrice", "{0:C}")%></asp:Label>
									        </ItemTemplate>
								        </asp:TemplateColumn>
								        <asp:TemplateColumn HeaderText="End User Price(S$)" HeaderStyle-Wrap="false" ItemStyle-Width="100px">
									        <ItemTemplate>
										        <asp:Label ID="lblUserPrice" runat="server"><%# Eval("UserPrice", "{0:C}")%></asp:Label>
									        </ItemTemplate>
								        </asp:TemplateColumn>
								        <asp:TemplateColumn HeaderText="Retail Price(S$)" HeaderStyle-Wrap="false" ItemStyle-Width="100px">
									        <ItemTemplate>
										        <asp:Label ID="lblRetailPrice" runat="server"><%# Eval("RetailPrice", "{0:C}")%></asp:Label>
									        </ItemTemplate>
								        </asp:TemplateColumn>
								        <asp:TemplateColumn HeaderText="Update Date" HeaderStyle-Wrap="false" ItemStyle-Width="200px">
									        <ItemTemplate>
										        <asp:Label ID="lblUpdatedDate" runat="server"><%# Eval("UpdatedDate")%></asp:Label>
									        </ItemTemplate>
								        </asp:TemplateColumn>
								         <asp:TemplateColumn HeaderText="Remarks" HeaderStyle-Wrap="false" ItemStyle-Width="200px">
									        <ItemTemplate>
										        <asp:Label ID="lblRemarks" runat="server"><%# Eval("Remarks")%></asp:Label>
									        </ItemTemplate>
								        </asp:TemplateColumn>
							        </Columns>
							        <HeaderStyle CssClass="tHeader" />
							        <AlternatingItemStyle CssClass="tAltRow" />
							        <FooterStyle CssClass="tFooter" />
							        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
						        </asp:DataGrid>
            </div>
        </div>
	</div>
	
    <div class="row">
        <div id="PMRegion5" runat="server" class="col-lg-4 col-md-12 col-sm-12 col-xs-12">
            <div class="panel panel-primary">
                 <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                    </div>
                    <h4 class="panel-title">
                        Product Location
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive">
                        <asp:DataGrid ID="dgData3" runat="server" AutoGenerateColumns="false" ShowFooter="true"
				    OnItemCommand="dgData3_CreateCommand" GridLines="none"
				    OnItemDataBound="dgData3_ItemDataBound" OnDeleteCommand="dgData3_DeleteCommand"
				    CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" EnableViewState="true">
				    <Columns>
					    <asp:TemplateColumn HeaderText="Location" HeaderStyle-Wrap="false">
						    <ItemTemplate>
							    <asp:Label ID="lblLocation" runat="server">
											    <%# Eval("Location")%>
							    </asp:Label>
							    <input type="hidden" id="hidPLID" runat="server" value='<%# Eval("PLID")%>' />
						    </ItemTemplate>
						    <FooterTemplate>
							    <asp:TextBox CssClass="form-control input-sm" ID="txtNewLocation" runat="server" />
							    <asp:RequiredFieldValidator ID="rfvNewLocation" runat="server" ControlToValidate="txtNewLocation"
								    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
						    </FooterTemplate>
					    </asp:TemplateColumn>
					    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
						    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
						    <ItemTemplate>
							    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
								    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
						    </ItemTemplate>
						    <FooterTemplate>
							    <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
								    ValidationGroup="valGrpNewRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
						    </FooterTemplate>
					    </asp:TemplateColumn>
				    </Columns>
				    <HeaderStyle CssClass="tHeader" />
				    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
			    </asp:DataGrid>
                    </div>
                </div>
            </div>
			
	    </div>
	
	    <div id="PMRegion4" runat="server" class="col-lg-4 col-md-12 col-sm-12 col-xs-12">
            <div class="panel panel-primary">
                 <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                    </div>
                    <h4 class="panel-title">
                        Order Details
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <table class="table table-condensed table-striped">
                        <tr>
                            <td>On SO : <asp:Label runat="server" ID="lblSO"></asp:Label></td>
                            <td>
                                <a id="lnkViewSODetail" onclick="return ViewSODetail();" class="btn btn-default btn-sm pull-right" data-toggle="tooltip" data-placement="top" title="View Detail"><i class="ti-search"></i></a>
                            </td>
                        </tr>
                        <tr>
                            <td>On BO :
                                <asp:Label runat="server" ID="lblBO"></asp:Label>
                            </td>
                            <td>
                                <a id="lnkViewBODetail"  onclick="return ViewBODetail();"
                                    class="btn btn-default btn-sm pull-right" data-toggle="tooltip" data-placement="top" title="View Detail"><i class="ti-search"></i></a>
                            </td>
                        </tr>
                        <tr>
                            <td>On PO :
                                <asp:Label runat="server" ID="lblPO"></asp:Label>
                            </td>
                            <td>
                                <a id="lnkViewPODetail" onclick="return ViewPODetail();" 
                                    class="btn btn-default btn-sm pull-right" data-toggle="tooltip" data-placement="top" title="View Detail"><i class="ti-search"></i></a>
                            </td>
                        </tr>
                        <tr>
                            <td>On TN :
                                <asp:Label runat="server" ID="lblTN"></asp:Label>
                            </td>
                            <td>
                                <a id="lnkViewTNDetail" onclick="return ViewTNDetail();" 
                                 class="btn btn-default btn-sm pull-right" data-toggle="tooltip" data-placement="top" title="View Detail"><i class="ti-search"></i></a>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
		
	</div>

        <div id="PMRegion3" runat="server" class="col-lg-4 col-md-12 col-sm-12 col-xs-12">
        <div class="panel panel-primary">
                <div class="panel-heading">
                <div class="panel-heading-btn">
                    <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                </div>
                <h4 class="panel-title">
                    Stock Status
                </h4>
            </div>
            <div class="panel-body no-padding">
	            <div class="table-responsive">
						<asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" GridLines="none"
							CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" EnableViewState="true">
							<Columns>
								<asp:TemplateColumn HeaderText="Warehouse" HeaderStyle-Wrap="false">
									<ItemTemplate>
										<asp:Label ID="lblWarehouse" runat="server">
														<%# Eval("Warehouse")%>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="WarehouseName" HeaderStyle-Wrap="false">
									<ItemTemplate>
										<asp:Label ID="lblWarehouseName" runat="server">
														<%# Eval("WarehouseName")%>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Quantity" HeaderStyle-Wrap="false">
									<ItemTemplate>
										<asp:Label ID="lblQuantity" runat="server">
                                            <%#  String.Format("{0:0.####}", Convert.ToDecimal(Eval("Quantity"))) %>   
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Batch/Serial" HeaderStyle-Wrap="false">
									<ItemTemplate>
										<a id="lnkViewTNDetail" onclick="return ViewBatchSerial('<%# Eval("Warehouse")%>');" href="#">
										View Detail</a>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="View Detail" HeaderStyle-Wrap="false">
									<ItemTemplate>
										<a id="lnkViewWHDetail" onclick="return ViewWHDetail('<%# Eval("Warehouse")%>');" href="#">
										View Detail</a>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<HeaderStyle CssClass="tHeader" />
							<AlternatingItemStyle CssClass="tAltRow" />
							<FooterStyle CssClass="tFooter" />
							<PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
						</asp:DataGrid>
	            </div>
            </div>
        </div>
	</div>
	
    </div>

    <div id="PMRegion2" runat="server" class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                    </div>
                    <h4 class="panel-title">Stock Movement
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive">
                        <asp:DataGrid ID="dgData2" runat="server" AutoGenerateColumns="false" GridLines="none"
                            CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="true"
                            PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true" OnItemDataBound="dgData2_ItemDataBound">
                            <Columns>
                                <asp:TemplateColumn HeaderText="No" >
                                    <ItemTemplate>
                                        <%# (Container.ItemIndex + 1) + ((dgData2.CurrentPageIndex) * dgData2.PageSize)  %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Trn Date" HeaderStyle-Wrap="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblTrnDate" runat="server">
														<%# Eval("TrnDate", "{0:dd/MM/yyyy}")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Trn Type" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTrnType" runat="server">
														<%# Eval("TrnType")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Trn No." HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTrnNo" runat="server">
														<%# Eval("TrnNo")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Doc No." HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocNo" runat="server">
														<%# Eval("DocNo")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Received Qty" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReceivedQty" runat="server">
														<%# Eval("ReceivedQty")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Issued Qty" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssuedQty" runat="server">
														<%# Eval("IssuedQty")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Warehouse" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWarehouse" runat="server">
														<%# Eval("Warehouse")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Balance Qty" HeaderStyle-Wrap="false">
									<ItemTemplate>
										<asp:Label ID="lblBalanceQty" runat="server">
                                                <%# Eval("BalanceQty")%>
                                        </asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>                                
                                <asp:TemplateColumn HeaderText="Account Name" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountName" runat="server">
														<%# Eval("AccountName")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Remarks" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%--<a href="#" title="View" onclick='viewCommentHistory("<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>View</a>--%>
                                        <%# Eval("Narration")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="DB Version" SortExpression="DBVersion" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDBVersion" runat="server">
														<%# Eval("DBVersion")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                            <HeaderStyle CssClass="tHeader" />
                            <AlternatingItemStyle CssClass="tAltRow" />
                            <FooterStyle CssClass="tFooter" />
                            <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                        </asp:DataGrid>
                    </div>
                </div>
            </div>
        </div>
    </div>
	<div class="TABCOMMAND">
		<asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
			<ContentTemplate>
				<uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".product-menu").addClass("active expand");
            $(".sub-product-search-sale").addClass("active");
        });

        function OnClientValidationFailed(sender, args) {
            var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
            if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
                if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
                    alert("Only Image files allowed!");
                }
                else {
                    alert("Wrong file size!");
                }
            }
            else {
                alert("Only Image files allowed!");
            }
        }

    </script>
</asp:Content>
