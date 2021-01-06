<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="PriceList.aspx.cs" Inherits="GMSWeb.Sales.Sales.PriceList" Title="Pricelist" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

<ul class="breadcrumb pull-right">
    <li><a href="#">Administration</a></li>
    <li class="active">Pricelist</li>
</ul>
<h1 class="page-header">Pricelist
</h1>

   <div class="note note-info">
        <h4 class="block"><i class="ti-info-alt"></i> Info! </h4>
        <p>Product Manager can upload pricelist his/her products using Excel file. See the following for sample Excel file format.
        <br /><br />
         <i class="ti-download"></i>
            <asp:LinkButton ID="btnExport" runat="server" CausesValidation="true" CssClass="button" OnClick="btnExport_Click">
                Product Price List 
            </asp:LinkButton>
        </p>
         <br />
    </div>  

<atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />

        <div class="panel panel-primary">
            <div class="panel-heading">
                <div class="panel-heading-btn">
                    <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                </div>
                <h4 class="panel-title">
                    <i class="ti-upload"></i> Upload
                </h4>
            </div>
            <div class="panel-body row">
                <div class="form-horizontal m-t-20">					
					<div class="form-group col-lg-6 col-sm-6">
                        <label class="col-sm-4 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">Location</asp:Label></label>
                        <div class="col-sm-8">
                            <div class="input-group">
                                <input type="text" class="form-control" readonly>
                                 <label class="input-group-btn">
                                    <span class="btn btn-primary btn-upload">
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
                <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload" CssClass="btn btn-primary pull-right" OnClick="btnUpload_Click" />
                 
                <ASP:LINKBUTTON id="LINKBUTTON1" onclick="GenerateReport" RUNAT="server" TEXT="Print"
			        CSSCLASS="btn btn-default pull-right m-r-10" TOOLTIP="Please click to print report." CAUSESVALIDATION="False">
                <i class="ti-printer"></i>    
                </ASP:LINKBUTTON>
                <asp:DropDownList ID="ddlReport" runat="server" CssClass="form-control no-full-width pull-right m-r-10">
			        <asp:ListItem Value="Pricelist">Pricelist</asp:ListItem>
		        </asp:DropDownList>
                
            </div> 
        </div>

        <IFRAME ID=IFrame1 FRAMEBORDER=0 SCROLLING=YES Runat="Server" width=100% Style="display:none"></IFRAME>	
		<asp:Label ID="lblMsg" runat="server"></asp:Label>

        <div class="TABCOMMAND">
			<atlas:UpdatePanel ID="udpMsgUpdater" runat="server" Mode="Always">
				<ContentTemplate>
					<uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
				</ContentTemplate>
			</atlas:UpdatePanel>
		</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".pricing-menu").addClass("active expand");
            $(".sub-pricelist").addClass("active");
        });

        $(document).ready(function () {
            $(".sub-pricelist").addClass("active expand");
        });
    </script>
</asp:Content>