<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="AddNewStaff.aspx.cs" Inherits="GMSWeb.SysHR.Staff.AddNewStaff" Title="Training - Add New Staff Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register Assembly="SharpPieces.Web.Controls.ExtendedDropDownList" Namespace="SharpPieces.Web.Controls"
    TagPrefix="piece" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">HR Organisation </a></li>
        <li class="active">Add Employee</li>
    </ul>
    <h1 class="page-header">Add Employee
        <br />
        <small>Add a new employee data into system or upload a batch of employee data to the system.</small>
    </h1>

    <input type="hidden" id="hidConfirmSendEmail" runat="server" value="True" />
    <asp:ScriptManager ID="sriptmgr1" runat="server">
        <Services>
            <asp:ServiceReference Path="../Training/AutoCompleteEmployeeName.asmx" />
        </Services>
    </asp:ScriptManager>
     <div class="note note-info">
        <h4 class="block"><i class="ti-info-alt"></i> Info! </h4>
        <p>For upload by batch see the following files for example:
            <a href="<%= Request.ApplicationPath %>/Documents/Staff.xls">Staff.xls</a>
         </p>
    </div>
    <div class="row">
        <div class="col-lg-6 col-sm-12">
            <asp:UpdatePanel ID="updatePanel1" runat="server">
                <ContentTemplate>
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="panel-heading-btn">
                                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                            </div>
                            <h4 class="panel-title">
                                <i class="ti-pencil"></i>
                                Add New Staff
                            </h4>
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal form-group-sm row m-t-20">
                                <div runat="server" id="trCompany" class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Company</label>
                                    <div class="col-sm-8">
                                        <piece:ExtendedDropDownList ID="ddlNewCompany" runat="server" CssClass="form-control">
                                        </piece:ExtendedDropDownList>
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Employee No</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtNewEmployeeNo" runat="server" Columns="20" MaxLength="15" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                        <asp:RequiredFieldValidator ID="rfvNewEmployeeNo" runat="server" ControlToValidate="txtNewEmployeeNo"
                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Name</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtNewName" runat="server" Columns="20" MaxLength="40" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                        <asp:RequiredFieldValidator ID="rfvNewName" runat="server" ControlToValidate="txtNewName"
                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Department</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtNewDepartment" runat="server" Columns="20" MaxLength="20" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">DOB</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="txtNewDOB" MaxLength="10" Columns="10" onfocus="select();"
                                            CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvNewDOB" runat="server" ControlToValidate="txtNewDOB"
                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                        <asp:CompareValidator ID="cvNewDOB" runat="server" ErrorMessage="Invalid Date" ControlToValidate="txtNewDOB"
                                            Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Date Joined</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="txtNewDateJoined" MaxLength="10" Columns="10" onfocus="select();"
                                            CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvNewDateJoined" runat="server" ControlToValidate="txtNewDateJoined"
                                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                        <asp:CompareValidator ID="cvNewDateJoined" runat="server" ErrorMessage="Invalid Date"
                                            ControlToValidate="txtNewDateJoined" Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow"
                                            Operator="DataTypeCheck"></asp:CompareValidator>
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Designation</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtNewDesignation" runat="server" Columns="50" MaxLength="50" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Qualification</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtNewQualification" runat="server" Columns="50" MaxLength="50"
                                            onfocus="select();" CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Grade</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtNewGrade" runat="server" Columns="5" MaxLength="5" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">NRIC</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtNewNRIC" runat="server" Columns="15" MaxLength="20" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Email</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtNewEmail" runat="server" Columns="30" MaxLength="50" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toLowerCase()" />
                                        <asp:RegularExpressionValidator ID="revNewEmail" ControlToValidate="txtNewEmail"
                                            Text="Invalid Email Address" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                            runat="server" Display="Dynamic" ValidationGroup="valGrpNewRow" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Car Plate</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtCarPlate" runat="server" Columns="10" MaxLength="10" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Supervisor</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtNewSupervisorName" runat="server" Columns="30" MaxLength="40"
                                            autocomplete="off" onfocus="select();" CssClass="form-control" onchange="var str = this.value.split(' - ');this.value = str[0].replace(/^\s+|\s+$/g, '').toUpperCase();" />
                                        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx" ID="autoComplete2"
                                            TargetControlID="txtNewSupervisorName" ServicePath="../Training/AutoCompleteEmployeeName.asmx"
                                            ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                                        </ajaxToolkit:AutoCompleteExtender>
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Division</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtDivision" runat="server" Columns="30" MaxLength="50" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Department</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtDepartment" runat="server" Columns="30" MaxLength="50" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Section</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtSection" runat="server" Columns="30" MaxLength="50" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Unit</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtUnit" runat="server" Columns="30" MaxLength="50" onfocus="select();"
                                            CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Is Unit Head</label>
                                    <div class="col-sm-8">
                                        <div class="radio-inline m-b-3">
                                            <asp:RadioButton ID="rbIsUnitHead" runat="server" Text="Yes" GroupName="IsUnitHead" />
                                            <asp:RadioButton  ID="rbIsNotUnitHead" runat="server" Text="No" Checked="true" GroupName="IsUnitHead" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Is Active</label>
                                    <div class="col-sm-8">
                                        <div class="radio-inline m-b-3">
                                            <asp:RadioButton ID="rbIsActive" runat="server" Text="Yes" Checked="true" GroupName="IsActive" />
                                            <asp:RadioButton ID="rbIsInctive" runat="server" Text="No" GroupName="IsActive" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-lg-12 col-sm-12">
                                    <label class="col-sm-4 control-label text-left">Photo</label>
                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <input type="text" class="form-control" readonly>
                                             <label class="input-group-btn">
                                                <span class="btn btn-primary btn-sm btn-upload">
                                                    <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                                    <asp:FileUpload CssClass="form-control hidden" ID="FileUpload2" runat="server" />
                                                </span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer clearfix">
                            <asp:Button ID="btnAdd" Text="Add" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                                ValidationGroup="valGrpNewRow" OnClick="btnAdd_Click" OnClientClick="ConfirmSendEmail()"></asp:Button>
                        </div>
                    </div>
                    </div>
            <div class="TABCOMMAND">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <uctrl:MsgPanel ID="MsgPanel1" runat="server" EnableViewState="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="col-lg-6 col-sm-12">
            <asp:UpdatePanel ID="updatePanel2" runat="server">
                <ContentTemplate>
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="panel-heading-btn">
                                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                            </div>
                            <h4 class="panel-title">
                                <i class="ti-upload"></i>
                                Upload By Batch
                            </h4>
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal form-group-sm row m-t-20">
                                    <div runat="server" id="trCompany2" class="form-group col-lg-12 col-sm-12">
                                            <piece:ExtendedDropDownList CssClass="form-control" ID="ddlCompany" runat="server">
                                            </piece:ExtendedDropDownList>
                                    </div>
                                     <div class="form-group col-lg-12 col-sm-12">
                                        <asp:DropDownList runat="server" ID="ddlType" CssClass="form-control">
                                            <asp:ListItem Text="Basic Employee Info" Value="EmployeeInfo" />
                                            <asp:ListItem Text="Educational Qualification" Value="Qualification" />
                                            <asp:ListItem Text="Employment History" Value="History" />
                                            <asp:ListItem Text="Career Progression" Value="Progression" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-12 col-sm-12">
                                       <div class="input-group">
                                            <input type="text" class="form-control" readonly>
                                             <label class="input-group-btn">
                                                <span class="btn btn-primary btn-sm btn-upload">
                                                    <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                                    <asp:FileUpload CssClass="form-control hidden" ID="FileUpload1" runat="server" />
                                                </span>
                                            </label>
                                        </div>
                                    </div>
                                        
                                   <iframe id="IFrame1" frameborder="0" scrolling="YES" runat="Server" width="100%"
                                                style="display: none"></iframe>
       
                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <div class="panel-footer clearfix">
                              <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload" CssClass="btn btn-primary pull-right"
                                        OnClick="btnUpload_Click" />
                        </div>
                    </div>
                    <div class="TABCOMMAND">
                        <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".list-of-people-menu").addClass("active expand");
            $(".hr-menu").addClass("active expand");
            $(".sub-add-staff").addClass("active");
        });
    </script>
</asp:Content>
