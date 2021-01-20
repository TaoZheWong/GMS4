<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bulletin.aspx.cs" Inherits="GMSWeb.Bulletin" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>List of People - Staff</title>
    <script language="javascript" type="text/javascript" src="/GMS4/scripts/popcalendar.js"></script>
    <script language="javascript" type="text/javascript">
    function addNews()
			{					
				var url = "SysHR/Staff/StaffDetail.aspx?EmployeeID=" + 1; 
				showModelessDialog(url,window,"dialogWidth:42;dialogHeight:25");				
				return false;
			}	
    </script>
</head>
<body>
    
    <form id="form1" runat="server" enableviewstate="true">
        <div id="ContentBar">
            <h3>
                Company &gt; News</h3>
            <br />
            <asp:ScriptManager ID="sriptmgr1" runat="server" EnablePartialRendering="false"/>
            <table class="tTable" style="width: 400px">
                <tr>
                    <td>
                        <div id="Div1" style="text-align: left" runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable">
                               <table><tr><td colspan="3"><ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="0"
                               HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" EnableViewState="true"
                               ContentCssClass="accordionContent" Width="400" FramesPerSecond="40"
                                                     FadeTransitions="false" AutoSize="None" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
                                                        <HeaderTemplate><a href="" class="accordionLink"><%# (Container.DataItemIndex + 1) + (Convert.ToInt16(ViewState["CurrentPageIndex"]) * PageSize)%>. <%# Eval("CreatedDate", "{0: dd-MMM-yyyy}")%> <%# Eval("Title") %> - <%# Eval("CreatedByUsersObject.UserRealName") %></a></HeaderTemplate>
                                                        <ContentTemplate><span><%# Eval("Message") %></span><br /><br />
                                                        <div style="text-align:right; font-size:smaller; color:Red; display:<%# (Eval("ModifiedByUsersObject.UserRealName") != null && !string.IsNullOrEmpty(Eval("ModifiedBy").ToString())) ? "":"none"%>">Edited By <%# Eval("ModifiedByUsersObject.UserRealName")%> At <%# Eval("ModifiedDate") %> <br /><br /></div>
                                                        <div style="text-align:center"><asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" EnableViewState="false" 
                                                            CssClass="EditButt" Enabled='<%# (short) Eval("CreatedBy") == userId %>' ToolTip="Edit this message"><span>&nbsp;&nbsp;Edit</span></asp:LinkButton>&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("MessageID")%>' EnableViewState="true"
                                                            CssClass="DeleteButt" Enabled='<%# (short) Eval("CreatedBy") == userId %>' ToolTip="Delete this message" OnCommand="MyAccordion_ItemCommand"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton></div>
                                                           <ajaxToolkit:ConfirmButtonExtender ID="lnkDeleteCofirm" runat="server"
                                                            TargetControlID="lnkDelete"
                                                            ConfirmText="Are you sure you want to delete this message?" DisplayModalPopupID="ModalPopupExtender3"/>
                                                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="lnkDelete" PopupControlID="PNL3" OkControlID="ButtonOk3" CancelControlID="ButtonCancel3" BackgroundCssClass="modalBackground" />
                                                            <asp:Panel ID="PNL3" runat="server" style="display:none; width:200px; background-color:White; border-width:2px; border-color:Black; border-style:solid; padding:20px;">
                                                                Are you sure you want to delete this message?
                                                                <br /><br />
                                                                <div style="text-align:right;">
                                                                    <asp:Button ID="ButtonOk3" runat="server" Text="OK" />
                                                                    <asp:Button ID="ButtonCancel3" runat="server" Text="Cancel" />
                                                                </div>
                                                            </asp:Panel>
                                                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkEdit" PopupControlID="PNL2" CancelControlID="ButtonCancel2" BackgroundCssClass="modalBackground"/>
                                                            <asp:Panel ID="PNL2" runat="server" style="display:none; width:300px; background-color:White; border-width:2px; border-color:Black; border-style:solid; padding:20px;">
                                                            <table><tr><td>
                                                            Title :
                                                            </td><td>
                                                            <asp:TextBox runat="server" ID="txtEditTitle" Columns="30" Text='<%# Eval("Title") %>'></asp:TextBox><asp:RequiredFieldValidator ID="rfvEditTitle" runat="server" ControlToValidate="txtEditTitle"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                                            <input type="hidden" id="hidMessageID" runat="server" value='<%# Eval("MessageID")%>' />
                                                            </td></tr>
                                                            <tr><td>
                                                            Content :
                                                            </td><td>
                                                                <asp:TextBox ID="txtEditMessage" runat="server" TextMode="MultiLine" Rows="4" Columns="30" Text='<%# Eval("Message") %>'></asp:TextBox><asp:RequiredFieldValidator ID="rfvEditMessage" runat="server" ControlToValidate="txtEditMessage"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                                            </td>
                                                            </tr>
                                                            </table>
                                                                <br /><br />
                                                                <div style="text-align:center;">
                                                                    <asp:Button ID="ButtonOk2" runat="server" OnCommand="MyAccordion_ItemCommand" CommandName="Edit" Text="Save" ValidationGroup="valGrpEditRow"/>
                                                                    <asp:Button ID="ButtonCancel2" runat="server" Text="Cancel" />
                                                                </div>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </ajaxToolkit:Accordion>
                                                    </td>
                                                    </tr><tr><td align="right" style="border-color:#c8dcff; width:45%">
                                                    <asp:LinkButton ID="lnkPrevPage" runat="server" CommandName="ChangePage" CommandArgument="-1"
                                                            CausesValidation="False" OnCommand="ChangePageCommand" >&#060;&#060;Previous
                                                        </asp:LinkButton></td><td  align="center" style="border-color:#c8dcff">&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td><td align="left" style="border-color:#c8dcff; width:45%">
                                                        <asp:LinkButton ID="lnkNextPage" runat="server" CommandName="ChangePage" CommandArgument="1"
                                                            CausesValidation="False" OnCommand="ChangePageCommand">Next>>
                                                        </asp:LinkButton></td>
                                                        </tr>
                                                        </table>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <asp:LinkButton ID="lnkAddNews" runat="server"  CommandName="AddNews" CssClass="NewButt" BackColor="#b3c3ef">Add News</asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lnkAddNews" PopupControlID="PNL" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground"/>
                                                            <asp:Panel ID="PNL" runat="server" style="display:none; width:300px; background-color:White; border-width:2px; border-color:Black; border-style:solid; padding:20px;">
                                                            <table><tr><td>
                                                            Title :
                                                            </td><td>
                                                            <asp:TextBox runat="server" ID="txtNewTitle" Columns="30"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewTitle" runat="server" ControlToValidate="txtNewTitle"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                                            </td></tr>
                                                            <tr><td>
                                                            Content :
                                                            </td><td>
                                                                <asp:TextBox ID="txtNewMessage" runat="server" TextMode="MultiLine" Rows="4" Columns="30"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewMessage" runat="server" ControlToValidate="txtNewMessage"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                                            </td>
                                                            </tr>
                                                            </table>
                                                                <br /><br />
                                                                <div style="text-align:center;">
                                                                    <asp:Button ID="ButtonOk" runat="server" Text="Save" OnCommand="AddNewsCommand" ValidationGroup="valGrpNewRow"/>
                                                                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" />
                                                                </div>
                                                            </asp:Panel>
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
