<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
    Codebehind="UserAccessRights.aspx.cs" Inherits="GMSWeb.Admin.Accounts.UserAccessRights1"
    Title="System Admin - User Access Rights Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <style>
        .update-panel .panel-body {
            padding: 0;
        }
    </style>
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <a name="TemplateInfo"></a>

    <ul class="breadcrumb pull-right">
        <li><a href="#">Accounts</a></li>
        <li><a href="#">Users</a></li>
        <li class="active">Access Rights</li>
    </ul>
    <h1 class="page-header">Access Rights <br />
        <small>Modify user access rights.</small>
    </h1>

    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-user"></i>
                <asp:Label ID="lblUserRealName" runat="server"></asp:Label>
            </h4>
        </div>
        <div class="panel-body">
            <div class="form-horizontal form-group-sm row m-t-20">
                <div class="form-group col-lg-6 col-sm-6">
                    <label class="col-sm-6 control-label text-left">Login ID</label>
                    <div class="col-sm-6">
                        <asp:Label ID="lblLoginID" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="form-group col-lg-6 col-sm-6">
                    <label class="col-sm-6 control-label text-left">
                        Email
                    </label>
                    <div class="col-sm-6">
                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="form-group col-lg-6 col-sm-6">
                    <label class="col-sm-6 control-label text-left">
                        Active
                    </label>
                    <div class="col-sm-6">
                        <asp:Label ID="lblActive" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="form-group col-lg-6 col-sm-6">
                    <label class="col-sm-6 control-label text-left">
                        Company Access Right
                    </label>
                    <div class="col-sm-6">
                        <asp:DropDownList ID="ddlCompany" runat="Server" DataTextField="Name" DataValueField="CoyID"
                            CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" />
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <asp:Button ID="Button1" OnClick="btnSubmit_Click" Text="Update" EnableViewState="False"
                    runat="server" CssClass="btn btn-primary pull-right"></asp:Button>
        </div>
    </div>


    <!--
        Todo : 
        changed from tr row to div.
        row.visible  need to update on code behind
    -->
    <div runat="server" id="trCompany">
    <h4 class="m-b-15">Company Access Rights</h4>
        <div class="row">
            <asp:Repeater ID="rppCompanyList" runat="server" OnPreRender="rppCompanyList_PreRender">
                <ItemTemplate>
                    <div class=" col-sm-12">
                        <div class="panel panel-primary update-panel">
                            <div class="panel-heading">
                                <div class="panel-heading-btn">
                                    <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                                    <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                                </div>
                                <h4 class="panel-title">
                                    <%# DataBinder.Eval(Container.DataItem, "CompanyTitle")%>
                                </h4>
                            </div>
                            <div class="panel-body" style="display: none;">
                                <asp:Repeater ID="rppUserAccessCompany" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-condensed">
                                            <tr bgcolor="#F5F6F7">
                                                <td align="right">
                                                    <i>Access</i>
                                                </td>
                                                <td>
                                                    <div class="checkbox">
                                                        <input type="checkbox" id="chkAllCompany" onclick="CheckAll(this, 'rppUserAccessCompany')" name="chkAll" title="Select All" />
                                                        <label for="chkAllCompany">&nbsp;</label>
                                                    </div>
                                                </td>
                                                <td> </td>
                                                <td align="left">
                                                    <i>Default</i>
                                                </td>
                                              <%--  <td>
                                                    <div class="checkbox">
                                                        <input type="checkbox" id="chkDefaultCompany" onclick="CheckOne(this, 'rppUserAccessCompany')" name="chkOne" title="Select One" />
                                                        <label for="chkDefaultCompany">&nbsp;</label>
                                                    </div>
                                                </td>--%>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="rppRow" runat="server">
                                            <td>
                                                <input type="hidden" name="hidCoyId" id="hidCoyId" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"CoyID")%>' />
                                                <input type="hidden" name="hidName" id="hidName" runat="server" value="Company List" />
                                                <input type="hidden" name="hidDefault" id="hidDefault" runat="server" value="Company List" />
                                                <small>
                                                    <%# DataBinder.Eval(Container.DataItem,"Code") + " - " + DataBinder.Eval(Container.DataItem,"Name")%>
                                                 
                                                </small>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="clearfix"></div>
    </div>
 
    <div runat="server" id="trModCategory">
        <h4 class="m-b-15">Module Category Access Rights</h4>
        <div class="row">
            <asp:Repeater ID="rppModCategory" runat="server" OnPreRender="rppModCategory_PreRender">
                <ItemTemplate>
                    <div class=" col-sm-12">
                        <div class="panel panel-primary update-panel">
                            <div class="panel-heading">
                                <div class="panel-heading-btn">
                                    <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                                    <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                                </div>
                                <h4 class="panel-title">
                                    <%# DataBinder.Eval(Container.DataItem, "ModCategoryTitle")%>
                                </h4>
                            </div>
                            <div class="panel-body" style="display: none;">
                                <asp:Repeater ID="rppUserAccessModCategory" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-condensed">
                                            <tr bgcolor="#F5F6F7">
                                                <td align="right">
                                                 </td>
                                                <td>
                                                    <div class="checkbox">
                                                        <input type="checkbox" id="chkAllModule" onclick="CheckAll(this, 'rppUserAccessModCategory')" name="chkAll" title="Select All" />
                                                        <label for="chkAllModule">&nbsp;</label>
                                                    </div>
                                                </td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="rppRow" runat="server" class="rppUserAccessModCategory">
                                            <td>
                                                <input type="hidden" name="hidModCategoryId" id="hidModCategoryId" runat="server"
                                                    value='<%# DataBinder.Eval(Container.DataItem,"ModuleCategoryID")%>' />
                                                <input type="hidden" name="hidName" id="hidName" runat="server" value="Module Category List" />
                                                <small>
                                                    <%# DataBinder.Eval(Container.DataItem,"Name")%>
                                                </small>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="clearfix"></div>
    </div>

    <div>
        <h4 class="m-b-15">Module Access Rights</h4>
        <div class="row">
             <asp:Repeater ID="rppModule" runat="server" OnPreRender="rppModule_PreRender">
                <ItemTemplate>
                    <div class="col-sm-12">
                        <div class="panel panel-primary update-panel">
                        <div class="panel-heading">
                            <div class="panel-heading-btn">
                                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                            </div>
                            <h4 class="panel-title">
                                <%# DataBinder.Eval(Container.DataItem, "Name")%>
                            </h4>
                        </div>
                        <div class="panel-body" style="display: none;">
                            <table class="table table-condensed">
                                <tbody class="CheckListContent_<%# Regex.Replace(DataBinder.Eval(Container.DataItem,"Name").ToString().Trim(), @"\s+", "")%>">
                                    <tr bgcolor="#F5F6F7">
                                        <td align="right">
                                        </td>
                                        <td>
                                            <div class="checkbox">
                                                <input type="checkbox" id='chkAll_<%# Regex.Replace(DataBinder.Eval(Container.DataItem,"Name").ToString().Trim(), @"\s+", "")%>' onclick="CheckAll(this,'CheckListContent_<%# Regex.Replace(DataBinder.Eval(Container.DataItem,"Name").ToString().Trim(), @"\s+", "")%>    ')" name="chkAll" title="Select All" />
                                                <label for="chkAll_<%# Regex.Replace(DataBinder.Eval(Container.DataItem,"Name").ToString(), @"\s+", "")%>"></label>
                                            </div>
                                        </td>
                                    </tr>
                                    <asp:Repeater ID="rppUserAccessModule" runat="server">
                                    <ItemTemplate>
                                        <tr id="rppRow" runat="server">
                                            <td>
                                                <input type="hidden" name="hidName" id="hidName" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ParentModuleName")%>' />
                                                <small>
                                                    <%# DataBinder.Eval(Container.DataItem, "FunctionName")%>
                                                </small>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <!--Report Access Right-->
    <div>
        <h4 class="m-b-15">Reports Access Rights</h4>
        <div class="row">
            <asp:Repeater ID="rppReportsFunctionList" runat="server" OnPreRender="rppReportsFunctionList_PreRender">
                <ItemTemplate>
                    <div class="col-sm-12">
                        <div class="panel panel-primary update-panel">
                            <div class="panel-heading">
                                <div class="panel-heading-btn">
                                    <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                                    <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                                </div>
                                <h4 class="panel-title">
                                    <%# DataBinder.Eval(Container.DataItem, "Reports")%>
                                </h4>
                            </div>
                            <div class="panel-body" style="display: none;">
                                <asp:Repeater ID="rppReportsSystemFunctions" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-condensed">
                                            <tr bgcolor="#F5F6F7">
                                                <td align="right">
                                                </td>
                                                <td>
                                                    <div class="checkbox">
                                                        <input type="checkbox" id="chkAll_report" onclick="CheckAll(this, 'rppReportsSystemFunctions')" name="chkAll" title="Select All" />
                                                        <label for="chkAll_report"></label>
                                                    </div>
                                                </td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="rppRow" runat="server" class="rppReportsSystemFunctions">
                                            <td>
                                                <input type="hidden" name="hidName" id="hidName" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ReportCategory")%>' />
                                                <small>
                                                    <%# DataBinder.Eval(Container.DataItem,"ReportTitle")%>
                                                </small>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="clearfix"></div>
    </div>

    <!-- Document Access Right-->
    <div runat="server" id="trDocument">
        <h4 class="m-b-15">Documents Access Rights</h4>
        <div class="row">
            <asp:Repeater ID="rppDocumentsFunctionList" runat="server" OnPreRender="rppDocumentsFunctionList_PreRender">
                    <ItemTemplate>
                        <div class="col-sm-12">
                            <div class="panel panel-primary update-panel">
                                <div class="panel-heading">
                                    <div class="panel-heading-btn">
                                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                                        <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                                    </div>
                                    <h4 class="panel-title">
                                        <%# DataBinder.Eval(Container.DataItem, "Documents")%>
                                    </h4>
                                </div>
                                <div class="panel-body" style="display: none;">
                                    <asp:Repeater ID="rppDocumentsSystemFunctions" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-condensed">
                                                <tr bgcolor="#F5F6F7">
                                                    <td align="right">
                                                     </td>
                                                    <td>
                                                        <div class="checkbox">
                                                            <input type="checkbox" id="chkAllDocument" onclick="CheckAll(this, 'rppDocumentsSystemFunctions')" name="chkAll" title="Select All" />
                                                            <label for="chkAllDocument"></label>
                                                        </div>
                                                    </td>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr id="rppRow" runat="server" class="rppDocumentsSystemFunctions">
                                                <td>
                                                    <input type="hidden" name="hidName" id="hidName" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"DocumentCategory")%>' />
                                                    <small>
                                                        <%# DataBinder.Eval(Container.DataItem, "DocumentName")%>
                                                    </small>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
        </div>
        <div class="clearfix"></div>
    </div>

    <div runat="server" id="trDocumentOperation">
        <h4 class="m-b-15">Document Operations Access Rights</h4>
        <div class="row">
            <asp:Repeater ID="rppDocumentOperationAccessModuleCategoryList" runat="server" OnPreRender="rppDocumentOperation_PreRender">
                    <ItemTemplate>
                         <div class="col-sm-12">
                            <div class="panel panel-primary update-panel">
                                <div class="panel-heading">
                                    <div class="panel-heading-btn">
                                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                                        <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                                    </div>
                                    <h4 class="panel-title">
                                        <%# DataBinder.Eval(Container.DataItem, "DocumentsOperation")%>
                                    </h4>
                                </div>
                                <div class="panel-body" style="display: none;">
                                    <asp:Repeater ID="rppDocumentOperationModuleCategory" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-condensed">
                                            <tr bgcolor="#F5F6F7">
                                                <td align="right">
                                                    <i>SelectAll (View)</i></td>
                                                <td>
                                                    <div class='checkbox'>
                                                        <input type="checkbox" id="chkAll_documentOperation" onclick="CheckAll(this,'rppDocumentOperationModuleCategory')" name="chkAll" title="Select All" />
                                                        <label for='chkAll_documentOperation'></label>
                                                    </div>
                                                </td>
                                                        <td align="right">
                                                    <i>SelectAll (Edit)</i></td>
                                                <td>
                                                    <div class="checkbox">
                                                        <input type="checkbox" id="chkAllEdit__documentOperation" onclick="CheckAll(this,'rppDocumentOperationModuleCategoryEdit')" name="chkAll" title="Select All" />
                                                        <label for='chkAllEdit__documentOperation'></label>
                                                    </div>
                                                </td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="rppRow" runat="server">
                                            <td>
                                            <input type="hidden" name="hidName" id="hidName" runat="server" value="Module Category List" />
                                                <small>
                                                    <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                                </small>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
        </div>
        <div class="clearfix"></div>
    </div>

    <div class="TABCOMMAND">
        <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".account-menu").addClass("active expand");
            $(".sub-users").addClass("active");
        });
    </script>
</asp:Content>