<%@ Page Language="C#" AutoEventWireup="true" Codebehind="OrganizationChart.aspx.cs"
    Inherits="GMSWeb.HR.OrganizationChart.OrganizationChart" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Organization Chart - View</title>

    <script type="text/javascript">
    var first = true;
    function point_it(event){
      pos_x = event.offsetX ? (event.offsetX) : event.pageX - document.getElementById("pointer_div").offsetLeft;
      pos_y = event.offsetY?(event.offsetY):event.pageY - document.getElementById("pointer_div").offsetTop;
      switch(first){
  	    case true:
  	      document.getElementById(txtNewFirstCoordinates).value= pos_x.toString() + ", " + pos_y.toString();
  	      break;
  	    case false:
  	      document.getElementById(txtNewSecondCoordinates).value= pos_x.toString() + ", " + pos_y.toString();
  	      break;
      }
      first = !first;
   }
   
//function showTip(evt,elemID) 
//{
//  var tipWin = document.getElementById(elemID).style;
//  pos_x = event.offsetX ? (event.offsetX) : event.pageX - $("pointer_div").offsetLeft;
//  pos_y = event.offsetY?(event.offsetY):event.pageY - $("pointer_div").offsetTop;
//  tipWin.pixelTop = pos_y + document.body.scrollTop + 82;
//  tipWin.pixelLeft = pos_x + document.body.scrollLeft;
//  tipWin.visibility = "visible";
//}

//function hideTip(elemID) 
//{
//  var tipWin = document.getElementById(elemID).style;
//  tipWin.visibility = "hidden";
//}

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="GroupContentBar">
            <h3>
                Organization Chart &gt; View</h3>
            View HR Organization Chart .
            <br />
            <br />
            <asp:ScriptManager ID="scriptMgr" runat="server" />
            <asp:UpdatePanel ID="udpChartUpdater" runat="server" UpdateMode="conditional">
                <ContentTemplate>
                    <asp:HyperLink ID="btnBack" runat="server" Visible="false">Back To Previous Chart</asp:HyperLink>
                    <br />
                    <br />
                    <div id="pointer_div" style="border-bottom-width: 0px" runat="server">
                        <img src="" runat="server" id="OrChart" usemap="#map1" style="border-width: 0px">
                        <map name="map1" id="map1" runat="server">
                        </map>
                        <span class="tipstyle" id="tip1" style="background-color: Blue; padding: 2px; color: white;
                            position: absolute; visibility: hidden;">View Detail</span>
                    </div>
                    <br />
                    <br />
                    <asp:LinkButton ID="btnUploadLink" runat="server" OnClick="btnUploadLink_Click">Upload New Chart</asp:LinkButton>
                    <asp:LinkButton ID="btnCancelUpload" runat="server" Visible="false" OnClick="btnCancelUpload_Click">Cancel Upload</asp:LinkButton>
                    <div id="UploadPanel" runat="server" style="display: none">
                        <div class="tTable">
                        <br />
                            <table class="tblTable">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblLocation" runat="server">Location</asp:Label></td>
                                    <td>
                                        <asp:FileUpload ID="FileUpload1" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnUpload" CssClass="button" runat="server" CausesValidation="true"
                                            Text="Upload" OnClick="btnUpload_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <br />
                    <asp:LinkButton ID="btnEditLink" runat="server" OnClick="btnEditLink_Click">Edit Linking</asp:LinkButton>
                    <asp:LinkButton ID="btnCancelEdit" runat="server" Visible="false" OnClick="btnCancelEdit_Click">Cancel Edit</asp:LinkButton>
                    <div id="EditPanel" runat="server" style="display:none">
                    
                        <table style="width: 90%">
                            <tr>
                                <td>
                                <br />
                                    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                        OnDeleteCommand="dgData_DeleteCommand" DataKeyField="LinkID" GridLines="None"
                                        OnItemDataBound="dgData_ItemDataBound" OnItemCommand="dgData_CreateCommand" OnCancelCommand="dgData_CancelCommand"
                                        OnEditCommand="dgData_EditCommand" OnUpdateCommand="dgData_UpdateCommand" CellPadding="5"
                                        CssClass="tTable tBorder">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) %>
                                                    <input type="hidden" id="hidLinkID" runat="server" value='<%# Eval("LinkID")%>' />
                                                    .</ItemTemplate>
                                                <ItemStyle Width="15px" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="First Coordinates">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCoords1" runat="server">
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                                            CausesValidation="false"><span><%# Eval( "FirstCoordinates" )%></span></asp:LinkButton>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditFirstCoordinates" runat="server" Columns="10" MaxLength="10"
                                                        Text='<%# Eval("FirstCoordinates") %>' />
                                                    <asp:RequiredFieldValidator ID="rfvEditFirstCoordinates" runat="server" ControlToValidate="txtEditFirstCoordinates"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewFirstCoordinates" runat="server" Columns="10" MaxLength="10" />
                                                    <asp:RequiredFieldValidator ID="rfvNewFirstCoordinates" runat="server" ControlToValidate="txtNewFirstCoordinates"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                                </FooterTemplate>
                                                <ItemStyle Width="80px" />
                                                <HeaderStyle Wrap="False" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Second Coordinates">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCoords2" runat="server">
										        <%# Eval( "SecondCoordinates" )%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditSecondCoordinates" runat="server" Columns="10" MaxLength="10"
                                                        Text='<%# Eval("SecondCoordinates") %>' />
                                                    <asp:RequiredFieldValidator ID="rfvEditSecondCoordinates" runat="server" ControlToValidate="txtEditSecondCoordinates"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewSecondCoordinates" runat="server" Columns="10" MaxLength="10" />
                                                    <asp:RequiredFieldValidator ID="rfvNewSecondCoordinates" runat="server" ControlToValidate="txtNewSecondCoordinates"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                                </FooterTemplate>
                                                <ItemStyle Width="80px" />
                                                <HeaderStyle Wrap="False" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Image">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCourseTitle" runat="server">
										           <%# Eval( "LinkToOrganizationChartObject.ImagePath" )%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:FileUpload ID="newFileUpload" runat="server" />
                                                </FooterTemplate>
                                                <ItemStyle Width="250px" />
                                                <HeaderStyle Wrap="False" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Function">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                                        CausesValidation="false" CssClass="DeleteButt"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                                        ValidationGroup="valGrpEditRow" CssClass="SaveButt"><span>Save</span></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                                        CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                                        ValidationGroup="valGrpNewRow" CssClass="NewButt"><span>Add</span></asp:LinkButton>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle CssClass="tHeader" />
                                        <AlternatingItemStyle CssClass="tAltRow" />
                                        <FooterStyle CssClass="tFooter" />
                                    </asp:DataGrid>
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
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnUpload" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
