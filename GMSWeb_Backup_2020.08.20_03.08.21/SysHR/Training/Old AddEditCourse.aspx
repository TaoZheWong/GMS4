<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old AddEditCourse.aspx.cs"
    Inherits="GMSWeb.SysHR.Training.AddEditCourse" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add/Edit Course</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="ContentBar">
            <h3>
                Training &gt; Add/Edit Course</h3>
            Add or edit a training course.
            <br />
            <br />
            <asp:ScriptManager ID="sriptmgr1" runat="server">
                <Services>
                    <asp:ServiceReference Path="AutoCompleteOrganizerName.asmx" />
                </Services>
            </asp:ScriptManager>
            <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                border="1" width="90%">
                <tr>
                    <td class="tbLabel">
                        Course Code 
                        <input type="hidden" id="hidCourseID" runat="server" />
                        <input type="hidden" id="hidCourseTitle" runat="server" />
                        <input type="hidden" id="hidCourseCode" runat="server" />
                    </td>
                    <td style="width: 5%">
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtCourseCode" runat="server" Columns="10" MaxLength="10" CssClass="textbox"
                            onfocus="select();" onchange="this.value = this.value.toUpperCase()" />
                            <span style="color:Green"> (If course code is not specified, a new code will be assigned by the system to the course.)</span>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Course Title</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtCourseTitle" runat="server" Columns="80" MaxLength="80" CssClass="textbox"
                            onfocus="select();" onchange="this.value = this.value.toUpperCase()" /><asp:RequiredFieldValidator ID="rfvCourseTitle" runat="server" ControlToValidate="txtCourseTitle"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Course Organizer</td>
                    <td style="width: 5%">
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOrganizerName" runat="server" Columns="80" MaxLength="80" autocomplete="off"
                            onfocus="select();" CssClass="textbox" onchange="this.value = this.value.toUpperCase()" />
                        <asp:RequiredFieldValidator ID="rfvOrganizerName" runat="server" ControlToValidate="txtOrganizerName"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                            TargetControlID="txtOrganizerName" ServicePath="AutoCompleteOrganizerName.asmx"
                            ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                        </ajaxToolkit:AutoCompleteExtender>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Course Type</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlCourseType" runat="server" CssClass="dropdownlist">
                            <asp:ListItem Value="I">Internal</asp:ListItem>
                            <asp:ListItem Value="E">External</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Require PTJNPTEF</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:CheckBox ID="chkRequirePTJNPTEF" runat="server" Checked="false" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Is Bonded</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:CheckBox ID="chkIsBonded" runat="server" Checked="false" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Number of Months Bonded</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtNoOfMonthsBonded" runat="server" Columns="3" MaxLength="3" CssClass="textbox" />
                        Month(s)<asp:CompareValidator ID="cvNoOfMonthsBonded" runat="server" ErrorMessage="*"
                            Display="Dynamic" ControlToValidate="txtNoOfMonthsBonded" Type="Integer" Operator="DataTypeCheck"
                            ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Total Training Hours</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtTotalTrainingHours" runat="server" Columns="3" MaxLength="3"
                            CssClass="textbox" />
                        Hour(s)<asp:CompareValidator ID="cvTotalTrainingHours" runat="server" ErrorMessage="*"
                            Display="Dynamic" ControlToValidate="txtTotalTrainingHours" Type="Double" Operator="DataTypeCheck"
                            ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Target Audience</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtTargetAudience" runat="server" Columns="50" MaxLength="50" CssClass="textbox" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Course Objective</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtCourseObjective" runat="server" Columns="80" MaxLength="200"
                            CssClass="textbox" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Course Description</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtCourseDescription" runat="server" TextMode="MultiLine" Rows="3"
                            Columns="40" MaxLength="200" CssClass="textarea" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Prerequisite</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtPrerequisite" runat="server" Columns="80" MaxLength="200" CssClass="textbox" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Local Course Fee</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtLocalCourseFee" runat="server" Columns="7" MaxLength="7" CssClass="textbox" /><asp:CompareValidator
                            ID="cvLocalCourseFee" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalCourseFee"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Local Registration Fee</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtLocalRegistrationFee" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvLocalRegistrationFee" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalRegistrationFee"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Local Examination Fee</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtLocalExaminationFee" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvLocalExaminationFee" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalExaminationFee"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Local Membership Fee</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtLocalMembershipFee" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvLocalMembershipFee" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalMembershipFee"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Local GST</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtLocalGST" runat="server" Columns="7" MaxLength="7" CssClass="textbox" /><asp:CompareValidator
                            ID="cvLocalGST" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalGST"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas Flight Cost</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasFlightCost" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvOverseasHotelCost" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasHotelCost"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas Hotel Cost</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasHotelCost" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="CompareValidator2" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalMembershipFee"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas Transport Cost</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasTransportCost" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvOverseasTransportCost" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasTransportCost"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas Meal Cost</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasMealCost" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvOverseasMealCost" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasMealCost"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas Others</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasOthers" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvOverseasOthers" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasOthers"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas SDF</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasSDF" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvOverseasSDF" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasSDF"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Other Funding 1</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOtherFunding1" runat="server" Columns="7" MaxLength="7" CssClass="textbox" /><asp:CompareValidator
                            ID="cvOtherFunding1" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOtherFunding1"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Other Funding 2</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOtherFunding2" runat="server" Columns="7" MaxLength="7" CssClass="textbox" /><asp:CompareValidator
                            ID="cvOtherFunding2" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOtherFunding2"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Other Funding 3</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOtherFunding3" runat="server" Columns="7" MaxLength="7" CssClass="textbox" /><asp:CompareValidator
                            ID="cvOtherFunding3" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOtherFunding3"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                    <td style="width: 10%">
                        <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="button"
                            ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>
                    </td>
                </tr>
            </table>
            <div class="TABCOMMAND">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <ul>
                            <li></li>
                        </ul>
                        <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
