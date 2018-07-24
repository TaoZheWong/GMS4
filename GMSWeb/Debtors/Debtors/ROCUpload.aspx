<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ROCUpload.aspx.cs" Inherits="GMSWeb.Debtors.Debtors.ROCUpload" Title="ROC Upload" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<h1>Customer Info &gt; ROC Upload</h1>
            <p>            
            Search for customer's name to upload ROC.
            </p>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            
            <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0" width="99%">
                            
                            <tr>
                                <td style="width: 5%">
                                    <asp:Label CssClass="tbLabel" ID="Label1" runat="server">Customer Name</asp:Label></td>
				                <td style="width: 1%">:</td>
                                <td class="tbLabel">
                                    <asp:TextBox runat="server" ID="txtAccountName" MaxLength="50" Columns="50" onfocus="select();"
                                                CssClass="textbox"></asp:TextBox>                        
                                    <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                                        OnClick="btnSearch_Click"  ></asp:Button>
                                </td>
                            </tr>
                            
                           
            
         
                        
           
                <tr>
                    <td colspan="3">
                        <div id="Div1" style="text-align: left; " runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable">
                        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" 
                             GridLines="none" CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true"
                             PageSize="300" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true" OnSortCommand="SortData" AllowSorting="true">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                                    .</ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Company Name" SortExpression="CompanyName" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%# Eval("CompanyName")%>
                                                    <input type="hidden" id="hidCoyID" runat="server" value='<%# Eval("CoyID")%>' />
                                                    <input type="hidden" id="hidCustomerCode" runat="server" value='<%# Eval("AccountCode")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Customer Code" SortExpression="AccountCode" HeaderStyle-Wrap="false" ItemStyle-Width="25px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%# Eval("AccountCode")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Customer Name" SortExpression="AccountName" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" >
                                                <ItemTemplate>
                                                    <%# Eval("AccountName")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                         
                                            <asp:TemplateColumn HeaderText="Country" SortExpression="Country" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%# Eval("Country")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Tick" ItemStyle-Width="20px">
                                                <HeaderTemplate>
                                                <ASP:CHECKBOX ID="chkSelectAll" onclick="checkAll('chkUpload', this.checked);" Text=""
														    RUNAT="server"></ASP:CHECKBOX>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkUpload" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                        <AlternatingItemStyle CssClass="tAltRow" />
                                        <FooterStyle CssClass="tFooter" />
                                        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                    </asp:DataGrid>
                        </div>
                    </td>
                </tr>
                
            </table>
            
            
            
            
            <asp:Panel ID="pnlUpload" runat="server" Visible="false">
                <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0" width="99%">
                            <tr>
                                <td style="width: 5%">
                                    <asp:Label CssClass="tbLabel" ID="lblFileName" runat="server">Title</asp:Label></td>
				                <td style="width: 1%">:</td>
                                <td class="tbLabel">
                                    <asp:TextBox runat="server" ID="txtFileName" MaxLength="100" Columns="45" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFileName" runat="server" ControlToValidate="txtFileName" ErrorMessage="*" Display="dynamic" ValidationGroup="attachment" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 5%">
                                    <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">Location</asp:Label></td>
				                <td style="width: 1%">:</td>
                                <td class="tbLabel">
                                <asp:FileUpload CssClass="textbox" ID="FileUpload1" runat="server" Width="300px" />                     
                                <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="FileUpload1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="attachment" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 5%"></td>                                   
				                <td style="width: 1%"></td>
                                <td>
                                <asp:Button ID="btnUpload" Text="Upload" EnableViewState="False" runat="server" CssClass="button"
                                OnClick="btnUpload_Click" ValidationGroup="attachment" ></asp:Button></td>
                            </tr>
                            </table>
            </asp:Panel>
           
            
            
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
</asp:Content>
