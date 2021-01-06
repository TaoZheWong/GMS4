<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="AddEditCourse.aspx.cs" Inherits="GMSWeb.SysHR.Training.AddEditCourse" Title="Training - Add/Edit Course Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>

<ul class="breadcrumb pull-right">
    <li><a href="#">Training</a></li>
    <li class="active">Add & Edit Course</li>
</ul>
<h1 class="page-header">Add New Course <br />
     <small>Add or edit a training course.</small></h1>

<asp:ScriptManager ID="sriptmgr1" runat="server">
    <Services>
        <asp:ServiceReference Path="AutoCompleteOrganizerName.asmx" />
    </Services>
</asp:ScriptManager>

<div class="panel panel-primary">
    <div class="panel-heading">
        <h4 class="panel-title"><i class="ti-notepad"></i> Add / Edit Course</h4>
    </div>
    <div class="panel-body">
        <div class="form-horizontal m-t-20">

        <div class="form-group">
        <label class="col-sm-3 control-label">
        Course Code 
        <input type="hidden" id="hidCourseID" runat="server" />
        <input type="hidden" id="hidCourseTitle" runat="server" />
        <input type="hidden" id="hidCourseCode" runat="server" />
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtCourseCode" runat="server" Columns="10" MaxLength="10" CssClass="form-control"
            onfocus="select();" onchange="this.value = this.value.toUpperCase()" />
            <p class="text-muted"> (If course code is not specified, a new code will be assigned by the system to the course.)</p>
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Course Title
        </label>
        <div class="col-sm-9">
            <asp:TextBox ID="txtCourseTitle" runat="server" Columns="80" MaxLength="100" CssClass="form-control"
            onfocus="select();" onchange="this.value = this.value.toUpperCase()" /><asp:RequiredFieldValidator ID="rfvCourseTitle" runat="server" ControlToValidate="txtCourseTitle"
            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Course Organizer
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtOrganizerName" runat="server" Columns="80" MaxLength="80" autocomplete="off"
            onfocus="select();" CssClass="form-control" onchange="this.value = this.value.toUpperCase()" />
        <asp:RequiredFieldValidator ID="rfvOrganizerName" runat="server" ControlToValidate="txtOrganizerName"
            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
            TargetControlID="txtOrganizerName" ServicePath="AutoCompleteOrganizerName.asmx"
            ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
            EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
        </ajaxToolkit:AutoCompleteExtender>
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Course Type
        </label>
        <div class="col-sm-9">
            <asp:DropDownList ID="ddlCourseType" runat="server" CssClass="form-control">
            <asp:ListItem Value="I">Internal</asp:ListItem>
            <asp:ListItem Value="E">External</asp:ListItem>
        </asp:DropDownList>
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Require PTJNPTEF
        </label>
        <div class="col-sm-9">
            <div class="checkbox">
                <asp:CheckBox ID="chkRequirePTJNPTEF" runat="server" Checked="false" />
                <label for="<%=chkRequirePTJNPTEF.ClientID %>"></label>
            </div>
        
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Is Bonded
        </label>
        <div class="col-sm-9">
            <div class="checkbox">
                <asp:CheckBox ID="chkIsBonded" runat="server" Checked="false" />
                <label for="<%=chkIsBonded.ClientID %>"></label>
            </div>
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Number of Months Bonded
        </label>
        <div class="col-sm-9">
            <div class="input-group">
                <asp:TextBox ID="txtNoOfMonthsBonded" runat="server" Columns="3" MaxLength="3" CssClass="form-control" />
                <span class="input-group-addon">
                    Month(s)
                </span>
            </div>
        <asp:CompareValidator ID="cvNoOfMonthsBonded" runat="server" ErrorMessage="*"
            Display="Dynamic" ControlToValidate="txtNoOfMonthsBonded" Type="Integer" Operator="DataTypeCheck"
            ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Total Training Hours
        </label>
        <div class="col-sm-9">
            <div class="input-group">
                <asp:TextBox ID="txtTotalTrainingHours" runat="server" Columns="3" MaxLength="3"
                CssClass="form-control" />
                <span class="input-group-addon">
                    Hour(s)
                </span>
            </div>
        <asp:CompareValidator ID="cvTotalTrainingHours" runat="server" ErrorMessage="*"
            Display="Dynamic" ControlToValidate="txtTotalTrainingHours" Type="Double" Operator="DataTypeCheck"
            ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Target Audience
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtTargetAudience" runat="server" Columns="50" MaxLength="100" CssClass="form-control" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Course Objective
        </label>
        <div class="col-sm-9">
            <asp:TextBox ID="txtCourseObjective" runat="server" Columns="80" MaxLength="200"
            CssClass="form-control" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Course Description
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtCourseDescription" runat="server" TextMode="MultiLine" Rows="3"
            Columns="40" MaxLength="200" CssClass="form-control" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Prerequisite
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtPrerequisite" runat="server" Columns="80" MaxLength="200" CssClass="form-control" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Local Course Fee
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtLocalCourseFee" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
            ID="cvLocalCourseFee" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalCourseFee"
            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Local Registration Fee
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtLocalRegistrationFee" runat="server" Columns="7" MaxLength="7"
            CssClass="form-control" /><asp:CompareValidator ID="cvLocalRegistrationFee" runat="server"
                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalRegistrationFee"
                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Local Examination Fee
        </label>
        <div class="col-sm-9">
            <asp:TextBox ID="txtLocalExaminationFee" runat="server" Columns="7" MaxLength="7"
            CssClass="form-control" /><asp:CompareValidator ID="cvLocalExaminationFee" runat="server"
                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalExaminationFee"
                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Local Membership Fee
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtLocalMembershipFee" runat="server" Columns="7" MaxLength="7"
            CssClass="form-control" /><asp:CompareValidator ID="cvLocalMembershipFee" runat="server"
                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalMembershipFee"
                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Local GST
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtLocalGST" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
            ID="cvLocalGST" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalGST"
            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Overseas Flight Cost
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtOverseasFlightCost" runat="server" Columns="7" MaxLength="7"
            CssClass="form-control" /><asp:CompareValidator ID="cvOverseasHotelCost" runat="server"
                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasHotelCost"
                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Overseas Hotel Cost
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtOverseasHotelCost" runat="server" Columns="7" MaxLength="7"
            CssClass="form-control" /><asp:CompareValidator ID="CompareValidator2" runat="server"
                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalMembershipFee"
                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Overseas Transport Cost
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtOverseasTransportCost" runat="server" Columns="7" MaxLength="7"
            CssClass="form-control" /><asp:CompareValidator ID="cvOverseasTransportCost" runat="server"
                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasTransportCost"
                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Overseas Meal Cost
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtOverseasMealCost" runat="server" Columns="7" MaxLength="7"
            CssClass="form-control" /><asp:CompareValidator ID="cvOverseasMealCost" runat="server"
                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasMealCost"
                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Overseas Others
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtOverseasOthers" runat="server" Columns="7" MaxLength="7"
            CssClass="form-control" /><asp:CompareValidator ID="cvOverseasOthers" runat="server"
                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasOthers"
                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Overseas SDF
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtOverseasSDF" runat="server" Columns="7" MaxLength="7"
            CssClass="form-control" /><asp:CompareValidator ID="cvOverseasSDF" runat="server"
                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasSDF"
                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Other Funding 1
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtOtherFunding1" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
            ID="cvOtherFunding1" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOtherFunding1"
            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Other Funding 2
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtOtherFunding2" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
            ID="cvOtherFunding2" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOtherFunding2"
            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group">
        <label class="col-sm-3 control-label">
        Other Funding 3
        </label>
        <div class="col-sm-9">
        <asp:TextBox ID="txtOtherFunding3" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
            ID="cvOtherFunding3" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOtherFunding3"
            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
        </div>
        </div>
        <div class="form-group m-b-10 p-b-2">
        <div class="col-sm-offset-3 col-sm-9">
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
            $(".training-menu").addClass("active expand");
            $(".sub-course").addClass("active");
        });
    </script>
</asp:Content>