<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="AddEditTEF1.aspx.cs" Inherits="GMSWeb.SysHR.Training.AddEditTEF" Title="Training - Add/Edit TEF Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<h1>Training &gt; Add/Edit Training Evaluation Form</h1>
<p>Add or edit a Training Evaluation Form.</p>
            <asp:ScriptManager ID="sriptmgr1" runat="server">
            </asp:ScriptManager>
            <fieldset style="width: 650px; margin-left: 8px">
                <legend style="font-weight: bolder">SECTION A: ADMINISTRATIVE DATA</legend>
                <table class="tTable" style="border-collapse: collapse" cellspacing="5" cellpadding="5"
                    border="0" width="650px">
                    <tr>
                        <td class="tbLabel">
                            Employee's Name
                            <input type="hidden" id="hidRandomID" runat="server" />
                            <input type="hidden" id="hidEmployeeCourseID" runat="server" />
                        </td>
                        <td style="width: 5%">
                            :</td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblEmployeeName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Employee Number</td>
                        <td style="width: 5%">
                            :</td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblEmployeeNumber"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Superior's Name</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblSuperiorName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Department</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblDepartment"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Course Title</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblCourseTitle"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Course Type</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblCourseType"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Course Provider</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblOrganizerName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbLabel">
                            Course Date</td>
                        <td>
                            :</td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblCourseDate"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width: 650px; margin-left: 8px">
                <legend style="font-weight: bolder">SECTION B: COURSE FEEDBACK</legend>
                <table >
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
            <fieldset style="width: 650px; margin-left: 8px">
                <legend style="font-weight: bolder">SECTION C: RECOMMENDATIONS</legend>
                <table style="width: 650px">
                    <tr>
                        <td>
                            What are the areas you like best?</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtBestArea" runat="server" Columns="100" MaxLength="100" CssClass="textbox" />
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
            </fieldset><br /><br />
            <fieldset style="width: 650px; margin-left: 8px" runat="server" id="SubmitPanel">
                <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="button"
                    ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>
                <asp:Button ID="btnUpdate" Text="Update" EnableViewState="False" runat="server" CssClass="button"
                    Visible="false" OnClick="btnUpdate_Click"></asp:Button>
            </fieldset>
</asp:Content>
