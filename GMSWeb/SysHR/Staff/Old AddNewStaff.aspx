<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old AddNewStaff.aspx.cs" Inherits="GMSWeb.HR.Staff.AddNewStaff" %>

<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register Assembly="SharpPieces.Web.Controls.ExtendedDropDownList" Namespace="SharpPieces.Web.Controls"
    TagPrefix="piece" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>List of People - Add New Staff</title>

    <script language="javascript" type="text/javascript" src="/GMS4/scripts/popcalendar.js"></script>
    <script language="javascript" type="text/javascript" >
    function ConfirmSendEmail()
    {
        var n = confirm("Send email to the respective parties for new staff information?");
        if (n)
            document.getElementById("hidConfirmSendEmail").value = "True";
        else
            document.getElementById("hidConfirmSendEmail").value = "False";
    }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="ContentBar">
            <h3>
                List of People &gt; Add New Staff</h3>
            Add a new staff data into system or upload a batch of staff data to the system.
            <br />
            <br />
            <input type="hidden" id="hidConfirmSendEmail" runat="server" value="True" />
            <asp:ScriptManager ID="sriptmgr1" runat="server">
                <Services>
                    <asp:ServiceReference Path="../Training/AutoCompleteEmployeeName.asmx" />
                </Services>
            </asp:ScriptManager>
            <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Width="90%"
                CssClass="ajax__tab_xp">
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Add New Staff">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="updatePanel1" runat="server">
                            <ContentTemplate>
                                <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                                    border="1" width="100%">
                                    <tr>
                                        <td class="tbLabel">
                                            Company</td>
                                        <td style="width: 5%">
                                            :</td>
                                        <td colspan="2">
                                            <piece:ExtendedDropDownList ID="ddlNewCompany" runat="server" CssClass="dropdownlist">
                                            </piece:ExtendedDropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Employee No</td>
                                        <td style="width: 5%">
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNewEmployeeNo" runat="server" Columns="20" MaxLength="15" onfocus="select();"
                                                CssClass="textbox" onchange="this.value = this.value.toUpperCase()" />
                                            <asp:RequiredFieldValidator ID="rfvNewEmployeeNo" runat="server" ControlToValidate="txtNewEmployeeNo"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Name</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNewName" runat="server" Columns="20" MaxLength="40" onfocus="select();"
                                                CssClass="textbox" onchange="this.value = this.value.toUpperCase()" />
                                            <asp:RequiredFieldValidator ID="rfvNewName" runat="server" ControlToValidate="txtNewName"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Department</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNewDepartment" runat="server" Columns="20" MaxLength="20" onfocus="select();"
                                                CssClass="textbox" onchange="this.value = this.value.toUpperCase()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            DOB</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox runat="server" ID="txtNewDOB" MaxLength="10" Columns="10" onfocus="select();"
                                                CssClass="textbox"></asp:TextBox>
                                            <img id="img1" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                height="20" width="17" alt="" align="absMiddle" border="0">
                                            <asp:RequiredFieldValidator ID="rfvNewDOB" runat="server" ControlToValidate="txtNewDOB"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                            <asp:CompareValidator ID="cvNewDOB" runat="server" ErrorMessage="Invalid Date" ControlToValidate="txtNewDOB"
                                                Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Date Joined</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox runat="server" ID="txtNewDateJoined" MaxLength="10" Columns="10" onfocus="select();"
                                                CssClass="textbox"></asp:TextBox>
                                            <img id="imgCalendarEditTo" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                height="20" width="17" alt="" align="absMiddle" border="0">
                                            <asp:RequiredFieldValidator ID="rfvNewDateJoined" runat="server" ControlToValidate="txtNewDateJoined"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                            <asp:CompareValidator ID="cvNewDateJoined" runat="server" ErrorMessage="Invalid Date"
                                                ControlToValidate="txtNewDateJoined" Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow"
                                                Operator="DataTypeCheck"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Designation</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNewDesignation" runat="server" Columns="50" MaxLength="50" onfocus="select();"
                                                CssClass="textbox" onchange="this.value = this.value.toUpperCase()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Qualification</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNewQualification" runat="server" Columns="50" MaxLength="50"
                                                onfocus="select();" CssClass="textbox" onchange="this.value = this.value.toUpperCase()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Grade</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNewGrade" runat="server" Columns="5" MaxLength="5" onfocus="select();"
                                                CssClass="textbox" onchange="this.value = this.value.toUpperCase()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            NRIC</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNewNRIC" runat="server" Columns="15" MaxLength="15" onfocus="select();"
                                                CssClass="textbox" onchange="this.value = this.value.toUpperCase()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Email</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNewEmail" runat="server" Columns="30" MaxLength="50" onfocus="select();"
                                                CssClass="textbox" onchange="this.value = this.value.toLowerCase()" />
                                            <asp:RegularExpressionValidator ID="revNewEmail" ControlToValidate="txtNewEmail"
                                                Text="Invalid Email Address" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                                runat="server" Display="Dynamic" ValidationGroup="valGrpNewRow" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Car Plate</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtCarPlate" runat="server" Columns="10" MaxLength="10" onfocus="select();"
                                                CssClass="textbox" onchange="this.value = this.value.toUpperCase()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Supervisor</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNewSupervisorName" runat="server" Columns="30" MaxLength="40"
                                                autocomplete="off" onfocus="select();" CssClass="textbox" />
                                            <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx" ID="autoComplete2"
                                                TargetControlID="txtNewSupervisorName" ServicePath="../Training/AutoCompleteEmployeeName.asmx"
                                                ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                                            </ajaxToolkit:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Is Unit Head</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:RadioButton ID="rbIsUnitHead" runat="server" Text="Yes" GroupName="IsUnitHead" /><asp:RadioButton
                                                ID="rbIsNotUnitHead" runat="server" Text="No" Checked="true" GroupName="IsUnitHead" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Is Active</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:RadioButton ID="rbIsActive" runat="server" Text="Yes" Checked="true" GroupName="IsActive" /><asp:RadioButton
                                                ID="rbIsInctive" runat="server" Text="No" GroupName="IsActive" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tbLabel">
                                            Photo</td>
                                        <td>
                                            :</td>
                                        <td colspan="2">
                                            <asp:FileUpload ID="FileUpload2" runat="server" /></td>
                                        <td style="width: 10%">
                                            <asp:Button ID="btnAdd" Text="Add" EnableViewState="False" runat="server" CssClass="button"
                                                ValidationGroup="valGrpNewRow" OnClick="btnAdd_Click"  OnClientClick="ConfirmSendEmail()"></asp:Button></td>
                                    </tr>
                                </table>
                                <div class="TABCOMMAND">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <ul>
                                                <li></li>
                                            </ul>
                                            <uctrl:MsgPanel ID="MsgPanel1" runat="server" EnableViewState="false" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Upload By Batch">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="updatePanel2" runat="server">
                            <ContentTemplate>
                                See the following files for example:
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <div class="tblTable">
                                                <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <a href="<%= Request.ApplicationPath %>/Documents/Staff.xls">Staff.xls</a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <piece:ExtendedDropDownList ID="ddlCompany" runat="server">
                                                            </piece:ExtendedDropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 370px">
                                                            <asp:FileUpload ID="FileUpload1" runat="server" Width="255px" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload" CssClass="button"
                                                                OnClick="btnUpload_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <iframe id="IFrame1" frameborder="0" scrolling="YES" runat="Server" width="100%"
                                                                style="display: none"></iframe>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="TABCOMMAND">
                                    <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <ul>
                                                <li></li>
                                            </ul>
                                            <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
        </div>
    </form>
</body>
</html>
