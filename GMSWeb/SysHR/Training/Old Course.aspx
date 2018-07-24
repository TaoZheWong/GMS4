<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old Course.aspx.cs" Inherits="GMSWeb.HR.Training.Course" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Training - Course</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="ContentBar">
            <h3>
                Training &gt; Course</h3>
            List of training courses.
            <br />
            <br />
            <asp:ScriptManager ID="scriptMgr" runat="server" />
            <asp:UpdatePanel ID="udpCourseUpdater" runat="server" UpdateMode="conditional">
                <ContentTemplate>
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
                                Course Type</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlCourseType" runat="server" CssClass="dropdownlist">
                                    <asp:ListItem Value="%">-All-</asp:ListItem>
                                    <asp:ListItem Value="E">External</asp:ListItem>
                                    <asp:ListItem Value="I">Internal</asp:ListItem>
                                </asp:DropDownList></td>
                            <td style="width: 10%">
                                <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                                    OnClick="btnSearch_Click"></asp:Button><asp:Button ID="btnAdd" Text="Add" EnableViewState="False" runat="server" CssClass="button"
                                   OnClick="btnAdd_Click"></asp:Button></td>
                        </tr>
                    </table>
                    <table class="tTable" style="width: 100%">
                        <tr>
                            <td>
                                <br />
                                <div style="text-align: left; width: 690px" runat="server">
                                    <asp:Label ID="lblSearchSummary" Visible="false" runat="server"></asp:Label>
                                </div>
                                <br />
                                <div class="tTable">
                                    <asp:DataGrid ID="dgCourse" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                        DataKeyField="CourseID" OnPageIndexChanged="dgCourse_PageIndexChanged" GridLines="None" 
                                        CellPadding="5" CssClass="tTable tBorder"
                                        AllowPaging="True" PageSize="20">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgCourse.CurrentPageIndex) * dgCourse.PageSize)  %>
                                                    .</ItemTemplate>
                                                <ItemStyle Width="15px" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Course Code">
                                                <ItemTemplate>
                                                    <%# Eval( "CourseCode" )%>
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                                <HeaderStyle Wrap="False" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Course Title">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCourseTitle" runat="server">
										           <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "AddEditCourse.aspx?COURSEID="+Eval("CourseID")%>'><%# Eval("CourseTitle")%></asp:HyperLink>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="250px" />
                                                <HeaderStyle Wrap="False" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Course Organizer">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrganizer" runat="server">
												   <%# Eval("CourseOrganizerObject.OrganizerName")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="250px" />
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
