<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="UploadSpecialData.aspx.cs" Inherits="GMSWeb.Finance.Upload.UploadSpecialData" Title="Special Data" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="breadcrumb pull-right">
    <li><a href="#">Administration</a></li>
    <li class="active">Data Setup</li>
</ul>
<h1 class="page-header">Data Setup <small>Enter monthly special data (e.g. HO A&G Allocation %, Tax %, headcount for various departments, special adjustments etc) for formulaes used in the financial reports.</small></h1>

<atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        
<div class="panel panel-primary">
    <div class="panel-heading">
        <div class="panel-heading-btn">
            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
        </div>
        <h4 class="panel-title">
            <i class="ti-search"></i>
            Search filter
        </h4>
    </div>
    <div class="table-responsive">
        <div class="form-horizontal m-t-20">
            <div class="form-group col-lg-4 col-sm-6">
                <label class="col-sm-6 control-label text-left">
                    <asp:Label CssClass="tbLabel" ID="lblYear" runat="server">Year</asp:Label></label>
                <div class="col-sm-6">
                    <asp:DropDownList CssClass="form-control" ID="ddlYear" runat="server" DataTextField="Year" DataValueField="Year" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                        AutoPostBack="true" />
                </div>
            </div>
            <div class="form-group col-lg-4 col-sm-6">
                <label class="col-sm-6 control-label text-left">
                    <asp:Label CssClass="tbLabel" ID="lblMonth" runat="server">Month</asp:Label></label>
                <div class="col-sm-6">
                    <asp:DropDownList CssClass="form-control" ID="ddlMonth" runat="server" DataTextField="Month" DataValueField="Month" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                        AutoPostBack="true" />
                </div>
            </div>
            <div class="form-group col-lg-4 col-sm-6">
                <label class="col-sm-6 control-label text-left">
                    <asp:Label CssClass="tbLabel" ID="lblPurpose" runat="server">Purpose</asp:Label></label>
                <div class="col-sm-6">
                    <asp:DropDownList CssClass="form-control" ID="ddlPurpose" runat="server" DataTextField="SpecialDataPurposeName" DataValueField="SpecialDataPurposeID"
                        OnSelectedIndexChanged="ddl_SelectedIndexChanged" AutoPostBack="true" />
                </div>
            </div>
            <div class="form-group col-lg-4 col-sm-6">
                <label class="col-sm-6 control-label text-left">
                    <asp:Label CssClass="tbLabel" ID="lblCurrentValueLabel" runat="server">Current Value</asp:Label></label>
                <div class="col-sm-6">
                    <asp:Label runat="server" ID="lblCurrentValue" />
                </div>
            </div>
            <div class="form-group col-lg-4 col-sm-6">
                <label class="col-sm-6 control-label text-left">
                    <asp:Label CssClass="tbLabel" ID="lblValue" runat="server">New Value</asp:Label>
                </label>
                <div class="col-sm-6">
                    <asp:TextBox runat="server" ID="txtValue" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvValue" runat="server" ControlToValidate="txtValue"
                        ErrorMessage="*" Display="dynamic" ValidationGroup="Value" />
                    <asp:RangeValidator
                        ID="rgValue" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtValue" Type="Double" ValidationGroup="Value" />
                </div>
            </div>
        </div>

    </div>

    <div class="panel-footer clearfix">
        <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary pull-right" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="Value" />
    </div>
</div>

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
            $(".administration-menu").addClass("active expand");
            $(".sub-data-setup").addClass("active");
        });
    </script>
</asp:Content>
