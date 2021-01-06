<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old EmployeeCourse.aspx.cs"
    Inherits="GMSWeb.SysHR.Training.EmployeeCourse" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Training - Search Training Record</title>
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />

    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>

    <script language="javascript" type="text/javascript">
function changeEmployeeID (txt)
{
    var ddl = document.getElementById(ddlNewEmployeeID);
    var found = false;
    for( var i=0; i<ddl.options.length; i++)
    {
        if (ddl.options[i].text==txt.value)
        {
            ddl.options[i].selected = true;
            found = true;
        }
    }
    if (found == false)
    {
        alert('Please key in the correct name!');
        ddl.options[0].selected = true;
    }
}
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="ContentBar">
            <h3>
                Training &gt; Search Training Record</h3>
            List of training courses taken by the employees.
            <br />
            <br />
            <asp:ScriptManager ID="sriptmgr1" runat="server">
                <Services>
                    <asp:ServiceReference Path="AutoCompleteEmployeeName.asmx" />
                </Services>
            </asp:ScriptManager>
            <asp:UpdatePanel ID="udpEmpCourseUpdater" runat="server" UpdateMode="conditional">
                <ContentTemplate>
                    <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                        border="1" width="80%">
                        <tr>
                            <td class="tbLabel">
                                Course</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtSearchCourse" runat="server" Columns="50" MaxLength="80" onfocus="select();"
                                    CssClass="textbox" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tbLabel">
                                Employee Name</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtSearchName" runat="server" Columns="40" MaxLength="40" onfocus="select();"
                                    CssClass="textbox" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tbLabel">
                                Date From</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="dateFrom" MaxLength="10" Columns="10" onfocus="select();"
                                    CssClass="textbox"></asp:TextBox>
                                <img id="imgCalendarEditFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                    height="20" width="17" alt="" align="absMiddle" border="0"></td>
                        </tr>
                        <tr>
                            <td class="tbLabel">
                                Date To</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="dateTo" MaxLength="10" Columns="10" onfocus="select();"
                                    CssClass="textbox"></asp:TextBox>
                                <img id="img1" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                    height="20" width="17" alt="" align="absMiddle" border="0"></td>
                        </tr>
                        <tr>
                            <td class="tbLabel">
                                Course Registration</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:RadioButton ID="rbRegistrationStatusAll" runat="server" Text="All" Checked="true" GroupName="RegistrationStatus" />
                                <asp:RadioButton ID="rbRegistrationStatusPending" runat="server" Text="Pending" GroupName="RegistrationStatus" />
                                <asp:RadioButton ID="rbRegistrationStatusApproved" runat="server" Text="Completed" GroupName="RegistrationStatus" />
                        </tr>
                        <tr>
                            <td class="tbLabel">
                                TEF</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:RadioButton ID="rbTEFStatusAll" runat="server" Text="All" Checked="true" GroupName="TNFStatus" />
                                <asp:RadioButton ID="rbTEFStatusPending" runat="server" Text="Pending" GroupName="TNFStatus" />
                                <asp:RadioButton ID="rbTEFStatusCompleted" runat="server" Text="Completed" GroupName="TNFStatus" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tbLabel">
                                PTEF</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:RadioButton ID="rbPTEFStatusAll" runat="server" Text="All" Checked="true" GroupName="CEFStatus" />
                                <asp:RadioButton ID="rbPTEFStatusPending" runat="server" Text="Pending" GroupName="CEFStatus" />
                                <asp:RadioButton ID="rbPTEFStatusCompleted" runat="server" Text="Completed" GroupName="CEFStatus" />
                            </td>
                            <td style="width: 10%">
                                <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                                    OnClick="btnSearch_Click"></asp:Button></td>
                        </tr>
                    </table>
                    <br />
                    <table class="tTable" style="width: 1000px">
                        <tr>
                            <td>
                                <div style="text-align: left; width: 1000px" runat="server">
                                    <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                                </div>
                                <br />
                                <div class="tTable">
                                    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                        GridLines="none" OnItemDataBound="dgData_ItemDataBound"
                                        CellPadding="5" OnDeleteCommand="dgData_DeleteCommand"
                                        CellSpacing="0" CssClass="tTable tBorder" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged"
                                        EnableViewState="true">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                                    .
                                                    <input type="hidden" id="hidEmployeeCourseID" runat="server" value='<%# Eval("EmployeeCourseID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Employee Name" HeaderStyle-Wrap="false" ItemStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpNo2" runat="server">
                                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "CourseRegistration.aspx?EMPLOYEECOURSEID="+Eval("EmployeeCourseID")+"&FORMTYPE=EC"%>'><%# Eval( "EmployeeObject.Name" )%></asp:HyperLink>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Course" HeaderStyle-Wrap="false" ItemStyle-Width="250px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCourseTitle" runat="server">
										           <%# Eval("CourseSessionObject.CourseObject.CourseTitle")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Status" HeaderStyle-Wrap="false" ItemStyle-Width="250px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server">
										           <%# (Eval("Status").ToString() == "A") ? "Approved" : ((Eval("Status").ToString() == "R") ? "Rejected" : "Pending")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="TEF Status" HeaderStyle-Wrap="false" ItemStyle-Width="250px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTEFStatus" runat="server">
										           <%# (Eval("TEFList.Count").ToString() != "0") ? ((Eval("TEFList[0].Status").ToString() == "A") ? "Completed" : "Pending") : "N.A" %>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="View TEF" HeaderStyle-Wrap="false" ItemStyle-Width="250px">
                                                <ItemTemplate>
                                                   <asp:Label ID="lblTEF" runat="server">
                                                        <asp:HyperLink ID="lnkTEF" runat="server" NavigateUrl='<%# "AddEditTEF.aspx?EMPLOYEECOURSEID="+Eval("EmployeeCourseID")%>' Visible='<%# (Eval("TEFList.Count").ToString() != "0") ? true : false %>'>View</asp:HyperLink>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="PTEF Status" HeaderStyle-Wrap="false" ItemStyle-Width="250px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPTEFStatus" runat="server">
										           <%#  (Eval("CourseSessionObject.CourseObject.RequirePTJNPTEF").ToString() == "True") ? ((Eval("ActualValueAfterCourse") == null) ? "Pending" : "Completed") : "N.A"%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                                        CausesValidation="false" CssClass="DeleteButt"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle CssClass="tHeader" />
                                        <AlternatingItemStyle CssClass="tAltRow" />
                                        <FooterStyle CssClass="tFooter" />
                                        <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                    </asp:DataGrid>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <div class="TABCOMMAND">
                <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <ul>
                            <li>&nbsp;</li>
                        </ul>
                        <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
