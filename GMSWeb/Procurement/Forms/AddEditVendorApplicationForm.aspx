<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="AddEditVendorApplicationForm.aspx.cs" Inherits="GMSWeb.Procurement.Forms.AddEditVendorApplicationForm" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<a name="TemplateInfo"></a>
    
<ul class="breadcrumb pull-right">
    <li><a href="#">Procurement</a></li>
    <li><a href="#">Form</a></li>
    <li class="active">Add/Edit Vendor</li>
</ul>

    <h1 class="page-header">Add Vendor <br />
    <small>Add or edit a vendor application form.</small></h1>

<asp:ScriptManager ID="sriptmgr1" runat="server">
</asp:ScriptManager>

<div class="panel panel-primary">
    <div class="panel-heading">
        <div class="panel-heading-btn">
            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
        </div>
        <h4 class="panel-title">
            <i class="ti-notepad"></i>
            New Vendor Pe-Qualification Form 
        </h4>
    </div>
     <div class="panel-body">
            <div class="form-horizontal m-t-20">
                <div class="form-group">
                <label class="col-sm-1 control-label text-left">
                    Vendor Name
                </label>
                 <div class="col-sm-4">
                    <asp:TextBox ID="txtVendorName" runat="server" Columns="80" MaxLength="80" CssClass="form-control"
                        onfocus="select();" onchange="this.value = this.value.toUpperCase()"
                        /><asp:RequiredFieldValidator ID="rfvVendorName" runat="server"
                            ControlToValidate="txtVendorName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                    <input type="hidden" id="hidVendorID" runat="server" />
                    <input type="hidden" id="hidVendorName" runat="server" />
                     <input type="hidden" id="hidFormID" runat="server" />
                     <input type="hidden" id="hidRandomID" runat="server" />
                </div>
                </div>
                </div>

         <div class="form-horizontal m-t-20">
                <div class="form-group">
                <label class="col-sm-1 control-label text-left">
                    Email
                </label> 
                 <div class="col-sm-4">
                    <asp:TextBox ID="txtEmail" runat="server" Columns="80" MaxLength="80" CssClass="form-control"
                        onfocus="select();" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="txtEmail" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
             
                </div>
                </div>
             </div>

          <%--<div class="form-horizontal m-t-20">
                <d class="form-group">
                <label class="col-sm-3 control-label">
                    Status
                </label>
                 <div class="col-sm-9">
                    <asp:TextBox ID="txtStatus" runat="server" Columns="80" MaxLength="80" CssClass="form-control"
                        onfocus="select();" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="txtStatus" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
             
                </div>
                </div>--%>

         </div>   
    
         <div class="panel-footer clearfix">
              <asp:Button ID="btnSendEmail" Text="Send Email" Visible="False" EnableViewState="False" runat="server" CssClass="pull-right btn btn-warning m-l-5" OnClick="btnSendEmail_Click"></asp:Button>
           <asp:Button ID="btnSubmit" Text="Update" EnableViewState="False" runat="server" CssClass="pull-right btn btn-primary"  OnClick="btnSubmit_Click"></asp:Button>

            <asp:HyperLink ID="lnkVendorEvaluation" Visible="False" runat="server" CssClass="text-info"></asp:HyperLink>
        </div>
      <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
    </ContentTemplate>
</asp:UpdatePanel>
     </div> 
     </asp:Content>

