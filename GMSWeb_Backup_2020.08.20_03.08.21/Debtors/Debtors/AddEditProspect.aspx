<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="AddEditProspect.aspx.cs" Inherits="GMSWeb.Debtors.Debtors.AddEditProspect" Title="Customer Info - Add Prospect" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>
<ul class="breadcrumb pull-right">
    <li><a href="#">Customer Info</a></li>
    <li class="active">Add Prospect</li>
</ul>
<h1 class="page-header">Add Prospect <small>Create new prospect. </small></h1>

<asp:ScriptManager ID="sriptmgr1" runat="server">    
</asp:ScriptManager>

    <div class="panel panel-primary">
        <div class="panel-heading">
            <h4 class="panel-title"><i class="ti-notepad"></i> Prospect</h4>
        </div>
        <div class="panel-body">
            <div class="form-horizontal m-t-20">
                
              
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Customer Name
            </label>
            <div class="col-sm-9">
                <asp:TextBox ID="txtCustomerName" runat="server" Columns="50" MaxLength="50" CssClass="form-control"
            onfocus="select();" onchange="this.value = this.value.toUpperCase()" AutoPostBack="true"
            /><asp:RequiredFieldValidator ID="rfvCourseTitle" runat="server"
                ControlToValidate="txtCustomerName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
            </div>
        </div>    
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Group
            </label>
            <div class="col-sm-9">
                <asp:DropDownList CssClass="form-control" ID="ddlGroup" runat="Server"
                     DataTextField="AccountGroupName" DataValueField="AccountGroupCode" />
            </div>
        </div>    
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Type
            </label>
            <div class="col-sm-9">
                <asp:DropDownList CssClass="form-control" ID="ddlType" runat="Server"
                                                    DataTextField="ClassName" DataValueField="ClassID" />
            </div>
        </div>    
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Salesman
            </label>
            <div class="col-sm-9">
                <asp:DropDownList ID="ddlSalesman" runat="Server" DataTextField="SalesPersonName"
                            DataValueField="SalesPersonID" CssClass="form-control" />
            </div>
        </div>           
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Default Currency 
            </label>
            <div class="col-sm-9">
                <asp:DropDownList ID="ddlCurrency" runat="Server" CssClass="form-control" />
            </div>
        </div>    
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Industry
            </label>
            <div class="col-sm-9">
                <asp:DropDownList ID="ddlIndustry" runat="Server" CssClass="form-control" DataTextField="Name" DataValueField="Name" />
            </div>
        </div>    
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Territory
            </label>
            <div class="col-sm-9">
                <asp:DropDownList ID="ddlCountry" runat="Server" CssClass="form-control" DataTextField="TerritoryName" DataValueField="TerritoryName" />
            </div>
        </div>    
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Credit Term
            </label>
            <div class="col-sm-9">
                <asp:TextBox ID="txtCreditTerm" runat="server" Columns="20" MaxLength="3" CssClass="form-control" />
                <asp:CompareValidator ID="cvCreditTerm" runat="server" ErrorMessage="*"
                    Display="Dynamic" ControlToValidate="txtCreditTerm" Type="Integer" Operator="DataTypeCheck"
                    ValidationGroup="valGrpNewRow" />      
            </div>
        </div>    
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Credit Limit
            </label>
            <div class="col-sm-9">
                <asp:TextBox ID="txtCreditLimit" runat="server" Columns="20" MaxLength="16" CssClass="form-control" />
                <asp:CompareValidator ID="cvCreditLimit" runat="server" ErrorMessage="*"
                    Display="Dynamic" ControlToValidate="txtCreditLimit" Type="Double" Operator="DataTypeCheck"
                    ValidationGroup="valGrpNewRow" />
            </div>
        </div>    
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Address
            </label>
            <div class="col-sm-9">
                <asp:TextBox runat="server" ID="txtAddress1" MaxLength="40" Columns="40" onfocus="select();"
                                            CssClass="form-control" TabIndex="1"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtAddress2" MaxLength="40" Columns="40" onfocus="select();"
                                                CssClass="form-control" TabIndex="2"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtAddress3" MaxLength="40" Columns="40" onfocus="select();"
                                                CssClass="form-control" TabIndex="2"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtAddress4" MaxLength="40" Columns="40" onfocus="select();"
                                            CssClass="form-control" TabIndex="2"></asp:TextBox>
            </div>
        </div>    
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Postal Code
            </label>
            <div class="col-sm-9">
                 <asp:TextBox ID="txtPostalCode" runat="server" Columns="20" MaxLength="6"
                    CssClass="form-control" />
            </div>
        </div>    
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Website
            </label>
            <div class="col-sm-9">
                <asp:TextBox runat="server" ID="txtWebsite" MaxLength="50" Columns="40" onfocus="select();"
                    CssClass="form-control"></asp:TextBox>
            </div>
        </div>    
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Facebook
            </label>
            <div class="col-sm-9">
                <asp:TextBox runat="server" ID="txtFacebook" MaxLength="50" Columns="40" onfocus="select();"
                    CssClass="form-control"></asp:TextBox>
            </div>
        </div>            
        <div class="form-group">
            <label class="col-sm-3 control-label">
                Remarks
            </label>
            <div class="col-sm-9">
                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3"
                Columns="40" MaxLength="600" CssClass="form-control" />
            </div>
        </div>            
        <div class="form-group">
            <label class="col-sm-3 control-label">
       
            </label>
            <div class="col-sm-9">
                <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="btn btn-primary"
                 ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>
            </div>
        </div>      
        </div>
    </div>
</div>
      
<div class="TABCOMMAND">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".customer-info-menu").addClass("active expand");
            $(".sub-debtors-search").addClass("active");
        });
    </script>
</asp:Content>
