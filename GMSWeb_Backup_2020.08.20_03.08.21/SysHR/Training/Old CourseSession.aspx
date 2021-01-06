<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old CourseSession.aspx.cs"
    Inherits="GMSWeb.SysHR.Training.CourseSession" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Training - Course Session</title>

    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="ContentBar">
            <h3>
                Training &gt; Course Session</h3>
            List of training course sessions.
            <br />
            <asp:ScriptManager ID="scriptMgr" runat="server" />
            <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                border="1" width="60%">
                <tr>
                    <td class="tbLabel">
                        Course Title</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="searchCourseTitle" MaxLength="80" Columns="50" onfocus="select();"
                            CssClass="textbox"></asp:TextBox></td>
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
                    <td style="width: 10%">
                        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnSearch_Click"></asp:Button><asp:Button ID="btnAdd" Text="Add" EnableViewState="False"
                                runat="server" CssClass="button" OnClick="btnAdd_Click"></asp:Button></td>
                </tr>
            </table>
            <table class="tTable" style="width: 100%">
                <tr>
                    <td>
                        <br />
                        <div id="Div1" style="text-align: left; width: 690px" runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server"></asp:Label>
                        </div>
                        <br />
                        <div class="tTable">
                            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                DataKeyField="CourseID" OnPageIndexChanged="dgData_PageIndexChanged" GridLines="None"
                                CellPadding="5" CssClass="tTable tBorder" AllowPaging="True" PageSize="20"
                                OnDeleteCommand="dgData_DeleteCommand" OnItemDataBound="dgData_ItemDataBound">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)%>
                                            .
                                            <input type="hidden" id="hidCourseSessionID" runat="server" value='<%# Eval("CourseSessionID")%>' />
                                            </ItemTemplate>
                                        <ItemStyle Width="15px" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Course Code">
                                        <ItemTemplate>
                                            <%# Eval( "CourseObject.CourseCode" )%>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Course Title">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCourseTitle" runat="server">
                                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "AddEditSession.aspx?COURSESESSIONID="+Eval("CourseSessionID")%>'><%# Eval("CourseObject.CourseTitle")%></asp:HyperLink>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Course Organizer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrganizer" runat="server">
												   <%# Eval("CourseObject.CourseOrganizerObject.OrganizerName")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Date From" ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDateFrom" runat="server">
												    <%# Eval("DateFrom", "{0: dd-MMM-yyyy HH:mm}")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Date To" ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDateTo" runat="server">
												    <%# Eval("DateTo", "{0: dd-MMM-yyyy HH:mm}")%>
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
