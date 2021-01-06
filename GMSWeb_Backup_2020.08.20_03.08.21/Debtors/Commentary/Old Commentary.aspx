<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Old Commentary.aspx.cs" Inherits="GMSWeb.Debtors.Commentary.Commentary" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>List of People - Staff</title>

    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>
    <script language="javascript" type="text/javascript" src="/GMS/scripts/date.js"></script>
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <script language="javascript" type="text/javascript">
			function btnSearch_OnClick()
			{
				var YY = document.all('txtAsOfDate').value.substring(6); 
				if (document.all('txtAsOfDate').value.substring(3,4) == '0') 
					var MM = parseInt(document.all('txtAsOfDate').value.substring(4,5))-1; 
				else 
					var MM = parseInt(document.all('txtAsOfDate').value.substring(3,5))-1; 
				var DD = parseInt(document.all('txtAsOfDate').value.substring(0,2)); 
				var selectedDate = new Date(YY,MM,DD); 
				var tempDate = selectedDate.addMonths(1); 
				var tempDate1 = new Date(tempDate.getYear(),tempDate.getMonth(),1); 
				tempDate1.setDate(tempDate1.getDate() - 1); 
				var lastDay = tempDate1.getDate(); 
				if (DD != lastDay) 
				{
					alert('You must select the last day of the month.'); 
					return false; 
				}
				else 
					return true; 
			}
			
			function EditComment(obj)
            {
                 var hidCommentID = obj.id;
                 hidCommentID = hidCommentID.replace("lnkEditComment1", "hidOwnComment1");
                 hidCommentID = hidCommentID.replace("lnkEditComment2", "hidOwnComment2");
                 var textarea=document.getElementsByTagName("textarea");
			     for(var i=0;i<textarea.length;i++)
			     {
				    if(textarea[i].id.indexOf("txtComment")!=-1)
				    {
    				
					    textarea[i].value = document.getElementById(hidCommentID).value;
				    }
				    update();
			     }
			     
			     var hidAccountCodeID = obj.id;
			     hidAccountCodeID = hidAccountCodeID.replace("lnkEditComment1", "hidAccountCode");
			     hidAccountCodeID = hidAccountCodeID.replace("lnkEditComment2", "hidAccountCode");
			     document.getElementById("hidAccountCode").value = document.getElementById(hidAccountCodeID).value;
			     
			     var hidCurrencyID = obj.id;
			     hidCurrencyID = hidCurrencyID.replace("lnkEditComment1", "hidCurrency");
			     hidCurrencyID = hidCurrencyID.replace("lnkEditComment2", "hidCurrency");
			     document.getElementById("hidCurrency").value = document.getElementById(hidCurrencyID).value;
			     
			     var hidCommentDateID = obj.id;
                 hidCommentDateID = hidCommentDateID.replace("lnkEditComment1", "hidComment1Date");
                 hidCommentDateID = hidCommentDateID.replace("lnkEditComment2", "hidComment2Date");
                 document.getElementById("hidCommentDate").value = document.getElementById(hidCommentDateID).value;
            }
            
            function viewCommentHistory(AccountCode, Currency)
			{			
			    var CoyID = document.getElementById('hidCoyID').value;					
				var url = "CommentaryHistory.aspx?CoyID=" + CoyID + "&AccountCode=" + AccountCode + "&CurrencyCode=" + Currency; 
				showModelessDialog(url,window,"dialogWidth:40;dialogHeight:30");				
				return false;
			}	
			
			function viewDetail(Type, AccountCode, Currency)
			{			
			    var CoyID = document.getElementById('hidCoyID').value;	
			    var AsOfDate = document.getElementById('hidAsOfDate').value;					
				var url = "DebtorDetails.aspx?CoyID=" + CoyID + "&AccountCode=" + 
				            AccountCode + "&CurrencyCode=" + Currency + "&Type=" + Type + "&AsOfDate=" + AsOfDate; 
				//window.open(url,window,"dialogWidth:35;dialogHeight:40");	
				window.open(url,"","width=" + 600 + ",height=" + 600 +",resizable=yes,status=yes,menubar=no,scrollbars=no");			
				return false;
			}	
			
			function print()
			{
			    var reportID = document.getElementById('ddlReport').options[document.getElementById('ddlReport').selectedIndex].value;
				jsOpenOperationalReport('Reports/ReportViewer.aspx?REPORTID=' + reportID);
			}
			
			function update() {
               
               var textarea=document.getElementsByTagName("textarea");
			     for(var i=0;i<textarea.length;i++)
			     {
				    if(textarea[i].id.indexOf("txtComment")!=-1)
				    {
    				document.getElementById('txtCounter').value = textarea[i].value.length;
				    }
			     }
              /* if(document.f.counter.value > limit && old <= limit) {
                 alert('Too much data in the text box!');
                 if(document.styleSheets) {
                   document.f.counter.style.fontWeight = 'bold';
                   document.f.counter.style.color = '#ff0000'; } }
               else if(document.f.counter.value <= limit && old > limit
	               && document.styleSheets ) {
                   document.f.counter.style.fontWeight = 'normal';
                   document.f.counter.style.color = '#000000'; } */
               }
		</script>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnSearch">
        <div id="ContentBar">
            <h3>
                Debtors' &gt; Commentary</h3>
            Display debtor's commentary for the past 2 months based on the date keyed in by user.
            <br />
            <br />
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <table class="tTable"style="BORDER-COLLAPSE: collapse" cellspacing="0" cellpadding="1" 
				border="1" width="80%">
                <tr>
                    <td class="tbLabel">
                        As Of Date</td>
                    <td style="width:5%">
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtAsOfDate" MaxLength="10" Columns="10" onfocus="select();"
                                    CssClass="textbox"></asp:TextBox>
                                <img id="imgCalendarAsOfDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                    height="20" width="17" alt="" align="absMiddle" border="0">
                                    <input type="hidden" id="hidAsOfDate" runat="server"  />
                                    <input type="hidden" id="hidCoyID" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Salesperson</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlSalesperson" runat="Server"  CssClass="dropdownlist" /></td><td style="width:10%">
                        <input type="hidden" id="hidSalesperson" runat="server"  />
                        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnSearch_Click" OnClientClick="return btnSearch_OnClick();" ></asp:Button></td>
                </tr>
            </table>
            <br />
            <table class="tTable" style="width: 1100px">
                <tr>
                    <td>
                        <div id="Div1" style="text-align: left; width: 1100px" runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable">
                        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" 
                             GridLines="none" CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" AllowPaging="true"
                             PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true" Width="98%">
                                        <Columns>
                                        <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                                    .</ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Account Code" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountCode" runat="server">
                                                        <%# Eval("AccountCode")%>
                                                            <input type="hidden" id="hidAccountCode" runat="server" value='<%# Eval("AccountCode")%>' />
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Account Name" HeaderStyle-Wrap="false" ItemStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountName" runat="server">
                                                        <%# Eval("AccountName")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Currency" HeaderStyle-Wrap="false" ItemStyle-Width="4%" ItemStyle-HorizontalAlign="center"> 
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSALES_Currency" runat="server">
                                                        <%# Eval("SALES_Currency")%>
                                                    </asp:Label>
                                                    <input type="hidden" id="hidCurrency" runat="server" value='<%# Eval("SALES_Currency")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="91 - 180 Days" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                <div>
                                                    <asp:Label ID="lblFr91To180" runat="server">
                                                        <%# Eval("Fr91To180", "{0:C}")%>
                                                    </asp:Label></div>
                                                    <a href="#" title="Detail1" onclick='viewDetail(1, "<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>Detail</a>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="> 180 Days" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                <div>
                                                    <asp:Label ID="lblFr180" runat="server">
                                                        <%# Eval("Fr180", "{0:C}")%>
                                                    </asp:Label></div>
                                                    <a href="#" title="Detail1" onclick='viewDetail(2, "<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>Detail</a>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Total Debts" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalAmount" runat="server">
                                                        <%# Eval("TotalAmount", "{0:C}")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Last Payment" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                   
                                                    <div><%# Eval("PaymentRefNo")%></div>
                                                    <div><%# Eval("PaymentDate","{0: dd-MMM-yyyy}")%></div>
                                                        <div><%# Eval("PaymentAmount", "{0:C}")%></div>
                                                 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Previous Month Comment" HeaderStyle-Wrap="false" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                <div>
                                                    <asp:Label ID="lblComment1" runat="server">
                                                        <%# FixCrLf(Eval("Comment1").ToString())%>
                                                    </asp:Label></div>
                                                    <asp:ImageButton ID="lnkEditComment1"
                                                    runat="server" ImageUrl="../../images/Icons/ModifyItem.gif" OnClientClick="EditComment(this)" />
                                                    <input type="hidden" id="hidComment1Date" runat="server" value='<%# Eval("Comment1Date")%>' />
                                                    <input type="hidden" id="hidOwnComment1" runat="server" value='<%# Eval("OwnComment1")%>' />
                                             <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lnkEditComment1"
                                                PopupControlID="PNL" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="This Month Comment" HeaderStyle-Wrap="false" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                <div>
                                                    <asp:Label ID="lblComment2" runat="server">
                                                        <%# FixCrLf(Eval("Comment2").ToString())%>
                                                    </asp:Label></div>
                                                    <asp:ImageButton ID="lnkEditComment2"
                                                    runat="server" ImageUrl="../../images/Icons/ModifyItem.gif" OnClientClick="EditComment(this)" />
                                                    <input type="hidden" id="hidComment2Date" runat="server" value='<%# Eval("Comment2Date")%>' />
                                                    <input type="hidden" id="hidOwnComment2" runat="server" value='<%# Eval("OwnComment2")%>' />
                                             <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkEditComment2"
                                                PopupControlID="PNL" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="View All Comments" HeaderStyle-Wrap="false" ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <a href="#" title="View All Comments" onclick='viewCommentHistory("<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>View</a>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle CssClass="tGroupHeader" HorizontalAlign="Center" />
                                        <AlternatingItemStyle CssClass="tGroupAltRow" />
                                        <FooterStyle CssClass="tGroupFooter" />
                                        <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                    </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
                                   
             <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White;
                border-width: 2px; border-color: Black; border-style: solid; padding: 10px;">
                <div style="text-align: center;">
                    <table>
                        <tr>
                            <td >
                                <b>Comment</b><br /> <i>Characters typed : <asp:TextBox runat="server" ID="txtCounter" 
                                    CssClass="textbox" ReadOnly="true" Columns="3"></asp:TextBox> (Limit:400)</i>
                            </td>
                        </tr>
                        <tr>
                            <td >
                            <asp:TextBox runat="server" ID="txtComment" TextMode="MultiLine" Columns="30" Rows="5" onfocus="select();"
                                    CssClass="textbox" onkeyup="update();"></asp:TextBox>
                                    <input type="hidden" id="hidAccountCode" runat="server" value="" />
                                    <input type="hidden" id="hidCurrency" runat="server" value="" />
                                    <input type="hidden" id="hidCommentDate" runat="server" value="" />
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="ButtonOk" runat="server" Text="Save" OnCommand="EditCommentCommand" />
                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" />
                </div>
            </asp:Panel>
            <br />
           <div style="text-align: left; width: 88%">
                            <asp:DropDownList ID="ddlReport" runat="server" CssClass="dropdownlist">
                                <asp:ListItem Value="158">Debtor's Commentary By Sales Executives</asp:ListItem>
                            </asp:DropDownList>
                            <asp:LinkButton ID="LINKBUTTON1"  OnClientClick="print();return false;" runat="server" Text="Print"
                                CssClass="button" ToolTip="Please click to print report." CausesValidation="False"><img id="img4" alt="" src="../../images/icons/printIcon.gif" align="top" border="0" /></asp:LinkButton>
                        </div>
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