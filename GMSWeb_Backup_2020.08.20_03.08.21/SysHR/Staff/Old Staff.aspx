<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old Staff.aspx.cs" Inherits="GMSWeb.HR.Staff.Staff" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>List of People - Staff</title>

    <script language="javascript" type="text/javascript" src="/GMS3/scripts/popcalendar.js"></script>

    <script language="javascript" type="text/javascript">
    function viewEmployeeDetail(EmployeeID)
			{					
				var url = "StaffDetail.aspx?EmployeeID=" + EmployeeID; 
				showModelessDialog(url,window,"dialogWidth:43;dialogHeight:30");				
				return false;
			}	
	function resetList()
	{
	document.getElementById('ddlEmployeeNameList').value = '%';
	document.getElementById('ddlEmployeeNoList').value = '%';
	return false;
	}
    </script>

</head>
<body onload="javascript:document.getElementById('txtSearchName').focus();">
    
    <form id="form1" runat="server" enableviewstate="true" defaultbutton="btnSearch">
        <div id="ContentBar">
            <h3>
                List of People &gt; Staff</h3>
            List of staff details.
            <br />
            <br />
            <asp:ScriptManager ID="sriptmgr1" runat="server" />
    <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="udpEmpCourseUpdater">
            <ContentTemplate>
            <table class="tTable"style="BORDER-COLLAPSE: collapse" cellspacing="0" cellpadding="1" 
				border="1" width="80%">
                <tr>
                    <td class="tbLabel">
                        Employee No</td>
                    <td style="width:5%">
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSearchEmployeeNo" runat="server" Columns="20" MaxLength="30" onfocus="select();" CssClass="textbox" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Name</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSearchName" runat="server" Columns="20" MaxLength="30" onfocus="select();" CssClass="textbox" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        NRIC</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSearchNRIC" runat="server" Columns="20" MaxLength="30" onfocus="select();" CssClass="textbox" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Designation</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSearchDesignation" runat="server" Columns="20" MaxLength="30" onfocus="select();" CssClass="textbox" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Grade</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSearchGrade" runat="server" Columns="10" MaxLength="10" onfocus="select();" CssClass="textbox" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Is Active</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:RadioButton ID="rbIsActive" runat="server" Text="Yes" Checked="true" GroupName="IsActive" /><asp:RadioButton ID="rbIsNotActive" runat="server" Text="No" GroupName="IsActive" /></td><td style="width:10%">
                        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnSearch_Click"></asp:Button></td>
                </tr>
            </table>
            <br />
            <br />
            <table class="tTable" style="width: 1600px">
                <tr>
                    <td>
                        <div id="Div1" style="text-align: left; width: 1250px" runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable">
                                
                                    <asp:DataList ID="dlData" runat="server" DataKeyField="EmployeeNo" CssClass="tblTable"
                                        RepeatDirection="Horizontal" RepeatColumns="2" CellSpacing="8" CellPadding="3"
                                        OnItemCommand="dlData_ItemCommand">
                                        <FooterTemplate>
                                            <asp:Table runat="server" ID="tbl_3" >
                                                <asp:TableRow HorizontalAlign="Center">
                                                    <asp:TableCell  HorizontalAlign="Center" BorderColor="#c8dcff">
                                                        <asp:LinkButton ID="lnkPrevPage" runat="server" CommandName="ChangePage" CommandArgument="-1"
                                                            CausesValidation="False" >&#060;&#060;Previous
                                                        </asp:LinkButton>
                                                    </asp:TableCell>
                                                    <asp:TableCell  HorizontalAlign="Center" BorderColor="#c8dcff">&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Center" BorderColor="#c8dcff">
                                                        <asp:LinkButton ID="lnkNextPage" runat="server" CommandName="ChangePage" CommandArgument="1"
                                                            CausesValidation="False">Next>>
                                                        </asp:LinkButton>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" BorderWidth="0" BackColor="#9eb2ec" />
                                        <ItemTemplate>
                                            <asp:Table ID="tbl_1" Style="border-collapse: collapse" runat="server" BORDER="1"
                                                RULES="all" CellSpacing="0">
                                                <asp:TableRow >
                                                    <asp:TableCell Wrap="False" Width="81" RowSpan="13"><a onclick='<%# "viewEmployeeDetail(" +Eval("EmployeeID")+");return false;" %>' href="#" title="View Staff Detail">
                                                        <asp:Image ID="Photo1" runat="server" ImageUrl='<%#"MakeThumbnail.aspx?size=small&path=" + Request.ApplicationPath + "/Data/HR/Photo/"+Eval("EmployeeID")+".JPG"+"&t=" + new Random().NextDouble().ToString() %>'/></a>
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="100px" Wrap="False" Style="background-color:#c3cff2">
												Employee No
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="10px" Wrap="False" cssclass="tbLabel">
												:
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="200px" Wrap="False" cssclass="tbLabel">
												<%# Eval("EmployeeNo") %>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow Style="background-color:#c3cff2">
                                                    <asp:TableCell Width="100px" Wrap="False">
												Name
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="10px" Wrap="False" cssclass="tbLabel">
												:
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="250px" Wrap="False" cssclass="tbLabel">
                                                    <a onclick='<%# "viewEmployeeDetail(" +Eval("EmployeeID")+");return false;" %>' href="#" title="View Staff Detail">
												<%# Eval("Name") %>
												</a>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow Style="background-color:#c3cff2">
                                                    <asp:TableCell Width="100px" Wrap="False">
												Department
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="10px" Wrap="False" cssclass="tbLabel">
												:
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="250px" Wrap="False" cssclass="tbLabel">
                                                    <%# Eval("Department") %>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow Style="background-color:#c3cff2">
                                                    <asp:TableCell Width="100px" Wrap="False">
												Designation
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="10px" Wrap="False" cssclass="tbLabel">
												:
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="250px" Wrap="False" cssclass="tbLabel">
												<%# Eval("Designation") %>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow Style="background-color:#c3cff2">
                                                    <asp:TableCell Width="100px" Wrap="False">
												Grade
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="10px" Wrap="False" cssclass="tbLabel">
												:
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="250px" Wrap="False" cssclass="tbLabel">
												<%# Eval("Grade") %>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow Style="background-color:#c3cff2">
                                                    <asp:TableCell Width="100px" Wrap="False" >
												Date Joined
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="10px" Wrap="False" cssclass="tbLabel">
												:
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="200px" Wrap="False" cssclass="tbLabel">
												<%# Eval("DateJoined").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("DateJoined", "{0: dd-MMM-yyyy}")%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow Style="background-color:#c3cff2">
                                                    <asp:TableCell Width="100px" Wrap="False">
												Qualification
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="10px" Wrap="False" cssclass="tbLabel">
												:
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="200px" Wrap="False" cssclass="tbLabel">
												<%# Eval("Qualification")%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                
                        </div>
                    </td>
                </tr>
            </table>
            </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <div class="TABCOMMAND">
                <asp:UpdatePanel ID="udpMsgUpdater" runat="server"  UpdateMode="Always">
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
