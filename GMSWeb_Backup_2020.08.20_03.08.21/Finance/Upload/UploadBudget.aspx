<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="UploadBudget.aspx.cs" Inherits="GMSWeb.Finance.Upload.UploadBudget" Title="Finance - Upload Budget Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="breadcrumb pull-right">
        <li><a href="#">Budget</a></li>
        <li class="active">Upload</li>
    </ul>
    <h1 class="page-header">Upload
    <br />
        <small>Upload yearly budget data for different purposes using downloaded Excel files. Figures should be keyed in 000s.
        </small>
    </h1>

        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        
        <div class="panel panel-primary">
            <div class="panel-heading">
                <div class="panel-heading-btn">
                    <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                </div>
                <h4 class="panel-title">
                    <i class="ti-download"></i> Download Template
                </h4>
            </div>
            <div class="panel-body row">
                <div class="m-t-20">
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">
		                    Dimension 1
                        </label>
	                    <asp:DropDownList CssClass="form-control" ID="ddlDim1" runat="server" DataTextField="ProjectName" DataValueField="ProjectID" AutoPostBack ="true" OnSelectedIndexChanged="ddlDim1_SelectedIndexChanged" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">
		                    Dimension 2
                        </label>
	                    <asp:DropDownList CssClass="form-control" ID="ddlDim2" runat="server" AutoPostBack ="true" Enabled="false" OnSelectedIndexChanged="ddlDim2_SelectedIndexChanged"  />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">
		                    Dimension 3
                        </label>
	                    <asp:DropDownList CssClass="form-control" ID="ddlDim3" runat="server" AutoPostBack ="true" Enabled="false"  OnSelectedIndexChanged="ddlDim3_SelectedIndexChanged" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">
		                    Dimension 4
                        </label>
	                    <asp:DropDownList CssClass="form-control" ID="ddlDim4" runat="server" AutoPostBack ="true" Enabled="false"  />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">
		                    Budget Year
                        </label>
	                    <asp:DropDownList CssClass="form-control" ID="templateYear" runat="server"  DataTextField="Year" DataValueField="Year"   />
                    </div>
                </div>
            </div>
            <div class="panel-footer clearfix">
                <asp:Button ID="btnDownload" CssClass="btn btn-primary pull-right" runat="server" CausesValidation="true" Text="Download" OnClick="btnDownload_Click" />
            </div>
        </div>

        <div class="note note-info">
            <h4 class="block"><i class="ti-info-alt"></i> Info! </h4>
            <p>If data has been existed for the year specified below, it will be overwritten. See the following sample files for examples:</p>
            <br />
            <!--
            <ul>
                <li><a href="<%= Request.ApplicationPath %>/Documents/Budget_P&L_Company_COA2016.xls">Budget_P&amp;L_Company_COA2016.xls</a></li>
                <li><a href="<%= Request.ApplicationPath %>/Documents/Budget_P&L_Company.xls">Budget_P&amp;L_Company.xls</a></li>
                <li><a href="<%= Request.ApplicationPath %>/Documents/Budget_P&L_WeldingSales.xls">Budget_P&amp;L_WeldingSales.xls</a></li>
                <li><a href="<%= Request.ApplicationPath %>/Documents/Budget_P&L_ProductUnit.xls">Budget_P&amp;L_ProductUnit.xls</a></li>
                <li><a href="<%= Request.ApplicationPath %>/Documents/Budget_P&L_Department.xls">Budget_P&amp;L_Department.xls</a></li>
                <li><a href="<%= Request.ApplicationPath %>/Documents/Budget_S&D.xls">Budget_S&amp;D.xls</a></li>
                <li><a href="<%= Request.ApplicationPath %>/Documents/Budget_G&A.xls">Budget_G&amp;A.xls</a></li>
                <li><a href="<%= Request.ApplicationPath %>/Documents/Budget_ProductGroup.xls">Budget_ProductGroup.xls</a></li>
                <li><a href="<%= Request.ApplicationPath %>/Documents/Budget_Customer.xls">Budget_Customer.xls</a></li>
            </ul>
            -->
            <p><i class="ti-star"></i> Please upload budget in 000s.</p>
        </div>
    
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
                <div class="m-t-20">
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">
		                     <asp:Label CssClass="tbLabel" ID="lblYear" runat="server">Year</asp:Label>
                        </label>
	                         <asp:DropDownList CssClass="form-control" ID="ddlYear" runat="server" DataTextField="Year" DataValueField="Year" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">
		                    <asp:Label CssClass="tbLabel" ID="lblPurpose" runat="server">Type</asp:Label>
                        </label>
	                        <asp:DropDownList CssClass="form-control" ID="ddlPurpose" runat="server" DataTextField="ItemPurposeName" DataValueField="ItemPurposeID" onchange="SelectPurpose(this)"/>
                    </div>      
                    <!-- 
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">
		                    <asp:Label CssClass="tbLabel" ID="lblProject" runat="server">Project</asp:Label>
                        </label>
	                        <asp:DropDownList CssClass="form-control" ID="ddlProject" runat="server" DataTextField="ProjectName" DataValueField="ProjectID" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">
		                    <asp:Label CssClass="tbLabel" ID="lblDepartment" runat="server">Department</asp:Label>
                        </label>
	                        <asp:DropDownList CssClass="form-control" ID="ddlDepartment" runat="server" DataTextField="DepartmentName" DataValueField="DepartmentID" />
                    </div>
                    -->
                  <%--  <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">
		                    <asp:Label CssClass="tbLabel" ID="lblCustomerType" runat="server">Customer Type</asp:Label>
                        </label>
	                        <asp:DropDownList ID="ddlCustomerType" runat="server" CssClass="form-control" Enabled="false">                                       
                                <asp:ListItem Value="E">External</asp:ListItem>
                                <asp:ListItem Value="I">Interco</asp:ListItem>
                            </asp:DropDownList>
                    </div> --%> 
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">
		                    <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">Browse</asp:Label>
                        </label>
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
            <div class="panel-footer clearfix">
                <asp:Button ID="btnUpload" CssClass="btn btn-primary pull-right" runat="server" CausesValidation="true" Text="Upload" OnClick="btnUpload_Click" />
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
            $(".budget-menu").addClass("active expand");
            $(".sub-upload-budget").addClass("active");
        });
    </script>
</asp:Content>