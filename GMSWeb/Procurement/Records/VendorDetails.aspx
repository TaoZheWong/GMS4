<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="VendorDetails.aspx.cs" Inherits="GMSWeb.Procurement.Records.VendorDetails" %>
<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register Assembly="SharpPieces.Web.Controls.ExtendedDropDownList" Namespace="SharpPieces.Web.Controls" TagPrefix="piece" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<a name="TemplateInfo"></a>

<ul class="breadcrumb pull-right">
    <li><a href="#">Vendor</a></li>
    <li class="active">Pre-Qualification Form</li>
</ul>
<h1 class="page-header">Procurement<br />
    <small> Vendor Pre-Qualification Form.</small>
</h1>

<asp:ScriptManager ID="scriptMgr" runat="server" />

<div class="panel panel-primary">
    <div class="panel-heading">
         <div class="panel-heading-btn">
            <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
        </div>
        <h4 class="panel-title">
            <i class="ti-search"></i>
            Add/Search filter
        </h4>
    </div>
    <div class="panel-body">
        <div class="form-horizontal form-group-sm row m-t-20">
            <div class="form-group col-lg-12 col-sm-12">
                <label class="col-sm-2 control-label text-left">Vendor Name</label>
                <div class="col-sm-4">
                     <asp:TextBox ID="txtNewVendorName" runat="server" Columns="80" MaxLength="80" CssClass="form-control"
                        onfocus="select();" onchange="this.value = this.value.toUpperCase()"  /><asp:RequiredFieldValidator ID="rfvNewVendorName" runat="server"
                        ControlToValidate="txtNewVendorName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                    <input type="hidden" id="hidNewVendorID" runat="server" />
                    <input type="hidden" id="hidNewVendorName" runat="server" />
                     <input type="hidden" id="hidFormID" runat="server" />
                     <input type="hidden" id="hidRandomID" runat="server" />
                </div>
            </div>
                <div class="form-group col-lg-12 col-sm-12">
                <label class="col-sm-2 control-label text-left">Email</label>
                <div class="col-sm-4">
                 <asp:TextBox ID="txtNewVendorEmail" runat="server" Columns="80" MaxLength="80" CssClass="form-control"
                 onfocus="select();" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                 ControlToValidate="txtNewVendorEmail" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                </div>
            </div>
            </div>
   
    </div>
    </div>
    <div class="panel-footer clearfix">
        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="pull-right btn btn-primary m-l-5" OnClick="btnSearch_Click"></asp:Button>
        <asp:Button ID="btnAdd" Text="Add" EnableViewState="False" runat="server" CssClass="pull-right btn btn-default" OnClick="btnAdd_Click"></asp:Button>
    </div>
         <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
    </ContentTemplate>
</asp:UpdatePanel>

<div class="panel panel-primary">
     <div class="panel-heading">
        <div class="panel-heading-btn">
            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
        </div>
        <h4 class="panel-title">
            <i class="ti-align-justify"></i> 
            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
        </h4>
    </div>

    <div class="table-responsive">
         <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="False" ShowFooter="True"
        DataKeyField="VendorID" OnPageIndexChanged="dgData_PageIndexChanged" GridLines="None" OnItemCommand="dgData_ItemCommand"
        CellPadding="5" Cellspacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="True" PageSize="20"
      >
       <Columns>
            <asp:TemplateColumn HeaderText="No">
                <ItemTemplate>
                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)%>
                    <input type="hidden" id="hidVendorID" runat="server" value='<%# Eval("VendorID")%>' />
                    <input type="hidden" id="hidFormID2" runat="server" value='<%# Eval("FormID")%>' />
                    </ItemTemplate>
            </asp:TemplateColumn>

       <asp:TemplateColumn HeaderText="Vendor Name">
                <ItemTemplate>
                     <input type="hidden" id="hidVendorID2" runat="server" />
                    <asp:Label ID="lblVendorName1" runat="server">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "../Forms/AddEditVendorApplicationForm.aspx?VENDORID="+Eval("VendorID")%>'><%# Eval("VendorObject.CompanyName")%></asp:HyperLink>
                    </asp:Label>
                       
                </ItemTemplate>
                <HeaderStyle Wrap="False" />
            </asp:TemplateColumn>
           <asp:TemplateColumn HeaderText="Email">
                <ItemTemplate>
                    <asp:Label ID="lblEmail1" runat="server">
							<%# Eval("VendorObject.Email")%>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
             <asp:TemplateColumn HeaderText="VEF Status">
                <ItemTemplate>
                    <asp:Label ID="lblVEFStatus" runat="server">
							 <%# (Eval("VEFStatus").ToString() != "0") ? "Completed" : "Pending" %>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
           <asp:TemplateColumn HeaderText="Submission Date"  ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblSubmissionDate" runat="server">
							<%# Eval("VEFStatus").ToString() == "0" ? "-" : Eval("Submissiondate","{0:dd/MM/yyyy}")%>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
           <asp:TemplateColumn HeaderText="View VEF" HeaderStyle-Wrap="false">
                 <ItemTemplate>
                    <asp:Label ID="lblVEF" runat="server">
                      <asp:HyperLink ID="lnkVEF" runat="server" Target="_blank" NavigateUrl='<%# "../Records/VendorRecords1.aspx?FORMID="+Eval("FORMID")%>' CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="View">
                    <%--  <asp:HyperLink ID="HyperLink2" runat="server" Target="_blank" NavigateUrl='<%# "../Forms/VendorEvaluationForm.aspx?VENDORID=" + Eval("VendorObject.VendorID") + "&FORMID=" + Eval("FormID")%>' CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="View"> --%>
                          <i class="ti-search"></i> </asp:HyperLink>
                   </asp:Label>
                 </ItemTemplate>
           </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Status" HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <asp:Label ID="lblApprovedStatus" runat="server">
                        <%--  <%# (Eval("VEFStatus").ToString() == "0") ? "Pending" : ((Eval("VEFStatus").ToString() == "1") ? "Approved" : "Rejected")%>--%>
                          <%# (Eval("ApprovedStatus").ToString() == "0") ? "Pending" : ((Eval("ApprovedStatus").ToString() == "1") ? "Approved" : "Rejected")%>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Evaluation Date"  ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblEvaluationDate" runat="server" >
							<%# Eval("ApprovedStatus").ToString() == "0" ? "-" : Eval("Evaluationdate","{0:dd/MM/yyyy}")%>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
           <asp:TemplateColumn HeaderText="Function" HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <asp:Button ID="btnDuplicate" Text="Duplicate" EnableViewState="False" runat="server" CssClass="btn btn-success m-r-10"
                        CommandName="DuplicateForm"></asp:Button>
                      <asp:HyperLink ID="lnkVendorEvaluation" Visible="False" runat="server" CssClass="text-info"></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateColumn>

       </Columns>
        <HeaderStyle CssClass="tHeader" />
        <FooterStyle CssClass="tFooter" />
        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
         </asp:DataGrid>
    </div>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".vendor-menu").addClass("active expand");
            $(".sub-records").addClass("active");
        });
    </script>
</asp:Content>

