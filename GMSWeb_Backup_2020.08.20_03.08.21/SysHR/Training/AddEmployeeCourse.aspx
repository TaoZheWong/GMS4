<%@ Page Language="C#" AutoEventWireup="true" Codebehind="AddEmployeeCourse.aspx.cs"
    Inherits="GMSWeb.SysHR.Training.AddEmployeeCourse" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Training - Training Record</title>
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="ContentBar">
            <h3>
                Training &gt; Training Record</h3>
            Add or edit a training record.
            <br />
            <br />
            <asp:ScriptManager ID="sriptmgr1" runat="server">
                <Services>
                    <asp:ServiceReference Path="AutoCompleteEmployeeName.asmx" />
                    <asp:ServiceReference Path="AutoCompleteOrganizerName.asmx" />
                    <asp:ServiceReference Path="AutoCompleteCourseTtitle.asmx" />
                </Services>
            </asp:ScriptManager>
            <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                border="1" width="90%">
                <tr>
                    <td class="tbLabel">
                        Employee Name
                        <input type="hidden" id="hidRowID" runat="server" />
                    </td>
                    <td style="width: 5%">
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblEmployeeName"></asp:Label>
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
                        Course Organizer</td>
                    <td style="width: 5%">
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="Label1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Date From</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="Label2"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Date To</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="Label3"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Learning Objectives</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtNewRemarks" runat="server" Columns="40" MaxLength="50" CssClass="textbox" />
                    </td>
                    <td style="width: 10%">
                        <asp:Label runat="server" ID="Label4"></asp:Label>
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
