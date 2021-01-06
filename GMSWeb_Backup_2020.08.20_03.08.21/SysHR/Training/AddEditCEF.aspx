<%@ Page Language="C#" AutoEventWireup="true" Codebehind="AddEditCEF.aspx.cs" Inherits="GMSWeb.SysHR.Training.AddEditCEF" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <title>Training - Add/Edit Training Nomination Form</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="ContentBar">
            <h3>
                Training &gt; Add/Edit Course Evaluation Form</h3>
            Add or edit a Course Evaluation Form.
            <br />
            <br />
            <asp:ScriptManager ID="sriptmgr1" runat="server">
            </asp:ScriptManager>
            <fieldset style="width: 70%">
                <legend style="font-weight: bolder">SECTION A: ADMINISTRATIVE DATA</legend>
                <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                    border="0" width="90%">
                    <tr>
                        <td class="tbLabel">
                            Employee's Name
                            <input type="hidden" id="hidFormID" runat="server" />
                            <input type="hidden" id="hidApprovalLevel" runat="server" />
                            <input type="hidden" id="hidView" runat="server" />
                            <input type="hidden" id="hidIsHR" runat="server" />
                        </td>
                        <td style="width: 5%">
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNewEmployeeName" runat="server" Columns="87" MaxLength="40" CssClass="textbox"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Employee Number</td>
                        <td style="width: 5%">
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNewEmployeeNumber" runat="server" Columns="87" MaxLength="15"
                                CssClass="textbox" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Superior's Name</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNewSuperiorName" runat="server" Columns="87" MaxLength="40" CssClass="textbox"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Department</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNewDepartment" runat="server" Columns="87" MaxLength="20" CssClass="textbox"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Course Title</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNewCourseTitle" runat="server" Columns="87" MaxLength="80" CssClass="textbox"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Course Type</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlNewCourseType" runat="server" CssClass="dropdownlist" Enabled="false">
                                <asp:ListItem>External</asp:ListItem>
                                <asp:ListItem>Internal</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Course Provider</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNewOrganizerName" runat="server" Columns="87" MaxLength="80"
                                CssClass="textbox" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Course Date</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="newDateFrom" runat="server" Columns="10" MaxLength="40" CssClass="textbox"
                                ReadOnly="true" />To
                            <asp:TextBox ID="newDateTo" runat="server" Columns="10" MaxLength="40" CssClass="textbox"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Course Fee</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNewCourseFee" runat="server" Columns="87" MaxLength="10" CssClass="textbox"
                                ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width: 70%">
                <legend style="font-weight: bolder">SECTION B: COURSE FEEDBACK</legend>
                <table>
                    <tr>
                        <td width="30%">
                        </td>
                        <td width="10%" align="center">
                            Strongly Disagree
                        </td>
                        <td width="10%" align="center">
                            Disagree</td>
                        <td width="10%" align="center">
                            Neutral</td>
                        <td width="10%" align="center">
                            Agree</td>
                        <td width="10%" align="center">
                            Strongly Agree</td>
                    </tr>
                    <tr>
                        <td colspan="6" style="font-weight: bolder">
                            Content
                        </td>
                    </tr>
                    <tr>
                        <td>
                            The course content is relevant to my work
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbContentRelevant1" runat="server" GroupName="ContentRelevant" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbContentRelevant2" runat="server" GroupName="ContentRelevant" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbContentRelevant3" runat="server" GroupName="ContentRelevant" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbContentRelevant4" runat="server" GroupName="ContentRelevant" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbContentRelevant5" runat="server" GroupName="ContentRelevant" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            The course content is well-organized
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbContentWellOrganized1" runat="server" GroupName="ContentWellOrganized" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbContentWellOrganized2" runat="server" GroupName="ContentWellOrganized" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbContentWellOrganized3" runat="server" GroupName="ContentWellOrganized" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbContentWellOrganized4" runat="server" GroupName="ContentWellOrganized" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbContentWellOrganized5" runat="server" GroupName="ContentWellOrganized" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="font-weight: bolder">
                            Quality of Teaching
                        </td>
                    </tr>
                    <tr>
                        <td>
                            The course is presented clearly and concisely
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseClear1" runat="server" GroupName="CourseClear" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseClear2" runat="server" GroupName="CourseClear" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseClear3" runat="server" GroupName="CourseClear" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseClear4" runat="server" GroupName="CourseClear" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseClear5" runat="server" GroupName="CourseClear" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            The speaker encourages questions and participation
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbEncourageParticipation1" runat="server" GroupName="EncourageParticipation" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbEncourageParticipation2" runat="server" GroupName="EncourageParticipation" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbEncourageParticipation3" runat="server" GroupName="EncourageParticipation" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbEncourageParticipation4" runat="server" GroupName="EncourageParticipation" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbEncourageParticipation5" runat="server" GroupName="EncourageParticipation" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            The teaching method is effective
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbMethodEffective1" runat="server" GroupName="MethodEffective" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbMethodEffective2" runat="server" GroupName="MethodEffective" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbMethodEffective3" runat="server" GroupName="MethodEffective" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbMethodEffective4" runat="server" GroupName="MethodEffective" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbMethodEffective5" runat="server" GroupName="MethodEffective" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="font-weight: bolder">
                            Learning Objective
                        </td>
                    </tr>
                    <tr>
                        <td>
                            The course met my learning objectives
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseMeetObjects1" runat="server" GroupName="CourseMeetObjects" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseMeetObjects2" runat="server" GroupName="CourseMeetObjects" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseMeetObjects3" runat="server" GroupName="CourseMeetObjects" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseMeetObjects4" runat="server" GroupName="CourseMeetObjects" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseMeetObjects5" runat="server" GroupName="CourseMeetObjects" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            The course met my expectation
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseMeetExpectation1" runat="server" GroupName="CourseMeetExpectation" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseMeetExpectation2" runat="server" GroupName="CourseMeetExpectation" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseMeetExpectation3" runat="server" GroupName="CourseMeetExpectation" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseMeetExpectation4" runat="server" GroupName="CourseMeetExpectation" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbCourseMeetExpectation5" runat="server" GroupName="CourseMeetExpectation" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="font-weight: bolder">
                            Overall Satisfaction
                        </td>
                    </tr>
                    <tr>
                        <td>
                            I am satisfied with the course
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbSatisfiedWithCourse1" runat="server" GroupName="SatisfiedWithCourse" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbSatisfiedWithCourse2" runat="server" GroupName="SatisfiedWithCourse" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbSatisfiedWithCourse3" runat="server" GroupName="SatisfiedWithCourse" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbSatisfiedWithCourse4" runat="server" GroupName="SatisfiedWithCourse" />
                        </td>
                        <td align="center">
                            <asp:RadioButton ID="rbSatisfiedWithCourse5" runat="server" GroupName="SatisfiedWithCourse" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width: 70%">
                <legend style="font-weight: bolder">SECTION C: RECOMMENDATIONS</legend>
                <table>
                    <tr>
                        <td>
                            What are the areas you like best?</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtBestArea" runat="server" Columns="100" MaxLength="100" CssClass="textbox" />
                            <asp:RequiredFieldValidator ID="rfvBestArea" runat="server" ControlToValidate="txtBestArea"
                                ErrorMessage="* This field is compulsory!" Display="dynamic" ValidationGroup="valGrpNewRow" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            What are the areas that need improvement?</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtAreaNeedImprovement" runat="server" Columns="100" MaxLength="100"
                                CssClass="textbox" />
                            <asp:RequiredFieldValidator ID="rfvAreaNeedImprovement" runat="server" ControlToValidate="txtAreaNeedImprovement"
                                ErrorMessage="* This field is compulsory!" Display="dynamic" ValidationGroup="valGrpNewRow" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Other comments:</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtOtherComments" runat="server" Columns="100" MaxLength="100" CssClass="textbox" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width: 70%" runat="server" id="SubmitPanel">
                <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="button"
                    ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>
                <asp:Button ID="btnAccept" Text="Accept" EnableViewState="False" runat="server" CssClass="button"
                    Visible="false" OnClick="btnAccept_Click"></asp:Button>
                <asp:Button ID="btnReject" Text="Reject" EnableViewState="False" runat="server" CssClass="button"
                    Visible="false" OnClick="btnReject_Click"></asp:Button>
            </fieldset>
            <fieldset style="width: 70%" runat="server" id="ApprovalStatusPanel">
                <legend style="font-weight: bolder">APPLICATION STATUS</legend>
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="ApplicationStatus">Applicant Status = New</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Level1Status">Level 1 Approval Status = Not Applicable</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="NotificationStatus"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="trPrintReport">
                        <td>
                            <img id="img4" alt="" src="../../images/icons/printIcon.gif" align="top" border="0">
                            <asp:LinkButton ID="lnkPrint" OnClick="printReport" runat="server" Text="Print" CssClass="button"
                                ToolTip="Please click to print." CausesValidation="False"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </form>
</body>
</html>
