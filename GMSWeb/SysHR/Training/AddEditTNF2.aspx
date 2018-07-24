<%@ Page Language="C#" AutoEventWireup="true" Codebehind="AddEditTNF.aspx.cs" Inherits="GMSWeb.SysHR.Training.AddEditTNF" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <title>Training - Add/Edit Training Nomination Form</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="ContentBar">
            <h3>
                Training &gt; Add/Edit Training Nomination Form</h3>
            Add or edit a Training Nomination Form.
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
                            Employee's Designation</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNewDesignation" runat="server" Columns="87" MaxLength="50" CssClass="textbox"
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
                            Superior's Designation</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNewSuperiorDeisignation" runat="server" Columns="87" MaxLength="50"
                                CssClass="textbox" ReadOnly="true" />
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
                    <tr>
                        <td class="tbLabel">
                            Course Code</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNewCourseCode" runat="server" Columns="87" MaxLength="10" CssClass="textbox"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Training Hours</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNewHour" runat="server" Columns="87" MaxLength="10" CssClass="textbox"
                                ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width: 70%">
                <legend style="font-weight: bolder">SECTION B: PRE-COURSE DISCUSSION</legend>
                <fieldset style="width: 95%">
                    <legend style="color: Black; font-weight: bold">Learning Objectives (Skills & Knowledge
                        To Be Learned)</legend>
                    <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                        border="0">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtNewLearningObjectives" TextMode="MultiLine" runat="server" Rows="2"
                                    Columns="100" MaxLength="200" CssClass="textarea" />
                                <asp:RequiredFieldValidator ID="rfvNewLearningObjectives" runat="server" ControlToValidate="txtNewLearningObjectives"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset style="width: 95%">
                    <legend style="color: Black; font-weight: bold">Work Objectives (What, Why, When & How
                        Affect Company's Business Plans)</legend>
                    <table>
                        <tr>
                            <td>
                                1. What to do? (The action plan after the course, e.g. design a relational database)</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtNewWhatToDo" runat="server" Columns="100" MaxLength="100" CssClass="textbox" />
                                <asp:RequiredFieldValidator ID="rfvNewWhatToDo" runat="server" ControlToValidate="txtNewWhatToDo"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                2. Why do it? (The purpose of the action plan, e.g. track the performance profile
                                of my top 10 clients)</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtWhyDoIt" runat="server" Columns="100" MaxLength="100" CssClass="textbox" />
                                <asp:RequiredFieldValidator ID="rfvWhyDoIt" runat="server" ControlToValidate="txtWhyDoIt"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                3. Do it by when? (Time frame to achieve action plan, e.g. within 1 month after
                                course)</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtDoItByWhen" runat="server" Columns="100" MaxLength="100" CssClass="textbox" />
                                <asp:RequiredFieldValidator ID="rfvDoItByWhen" runat="server" ControlToValidate="txtDoItByWhen"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                4. To improve which indicator? (How the action plan aligns with the Company's business
                                plans, e.g. Average margin per client + 10%)</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtImproveWhichIndicator" runat="server" Columns="100" MaxLength="100"
                                    CssClass="textbox" />
                                <asp:RequiredFieldValidator ID="rfvImproveWhichIndicator" runat="server" ControlToValidate="txtImproveWhichIndicator"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </fieldset>
            <fieldset style="width: 70%">
                <legend style="font-weight: bolder">SECTION C: FOR EXTERNAL COURSES ONLY</legend>
                <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                    border="0">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkNewInternalNotHave" runat="server" />
                        </td>
                        <td>
                            Internally we do not have the required expertise or facilities to conduct the training</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkNewExternalAcceptibility" runat="server" />
                        </td>
                        <td>
                            Acceptability of external advisers in this particular circumstances</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkNewAdvantageGained" runat="server" />
                        </td>
                        <td>
                            There are advantages to be gained from the participant(s) interacting with employees
                            of other organisations</td>
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
                            <asp:Label runat="server" ID="Level2Status">Level 2 Approval Status = Not Applicable</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Level3Status">Level 3 Approval Status = Not Applicable</asp:Label>
                        </td>
                    </tr>
                    <tr>
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
