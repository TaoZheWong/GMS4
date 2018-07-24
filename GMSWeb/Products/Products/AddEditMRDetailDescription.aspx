<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEditMRDetailDescription.aspx.cs" Inherits="GMSWeb.Products.Products.AddEditMRDetailDescription" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<base target="_self" />
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
    <meta http-equiv="PRAGMA" content="NO-CACHE" />
    <title>Quotation Detail Description</title>
</head>
<body style="background-image:none; text-align:left">
    <form id="form1" runat="server">
    <div>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td >
                            <b>Product Description: </b><br /><br /> <i>Characters typed : <asp:TextBox runat="server" ID="txtCounter" 
                                CssClass="textbox" ReadOnly="true" Columns="3"></asp:TextBox> (Limit:2000)</i><br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="False"
                                AllowPaging="false" showfooter="true" OnItemCommand="dgData_CreateCommand" OnDeleteCommand="dgData_DeleteCommand"
                                OnEditCommand="dgData_EditCommand" OnCancelCommand="dgData_CancelCommand" OnUpdateCommand="dgData_UpdateCommand" CellSpacing="5" CssClass="tTable tBorder" GridLines="None" CellPadding="5" >
                                <Columns>
                                    <asp:TemplateColumn SortExpression="Description" HeaderText="Product Description">
                                        <HeaderStyle></HeaderStyle>
                                        <ItemTemplate>
                                             <%# DataBinder.Eval(Container, "DataItem.Description")%>
                                              <input type="hidden" id="hidDescNo" runat="server" value='<%# Eval("DescNo")%>' />                                              
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtEditDescription" Columns="50" MaxLength="2000" TextMode="MultiLine" Height="50" onfocus="select();"
                                                CssClass="textbox" onkeyup="update(this);" EnableViewState="true" Text='<%# DataBinder.Eval(Container, "DataItem.Description")%>'></asp:TextBox>                                                   
                                              <input type="hidden" id="hidEditDescNo" runat="server" value='<%# Eval("DescNo")%>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox runat="server" ID="txtDescription" Columns="50" MaxLength="2000" TextMode="MultiLine" Height="50" onfocus="select();"
                                                CssClass="textbox" onkeyup="update(this);" EnableViewState="true"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Order">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkGoUp" runat="server" CommandName="GoUp" EnableViewState="true"><IMG height="16" src="../../images/icons/UpArrow.png" align="absMiddle"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkGoDown" runat="server" CommandName="GoDown" EnableViewState="true"><IMG height="16" src="../../images/icons/DownArrow.png" align="absMiddle"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle />
                                        <HeaderStyle Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" 
						                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
						                <ItemTemplate>
						                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
						                        CausesValidation="false" CssClass="EditButt"><span>&nbsp;&nbsp;Edit</span></asp:LinkButton>
						                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
						                        CausesValidation="false" CssClass="DeleteButt"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
						                </ItemTemplate>
						                <EditItemTemplate>
                                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="true" CssClass="SaveButt"><span>Save</span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="true" CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
                                        </EditItemTemplate>
							            <FooterTemplate>
								        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
									        CssClass="NewButt" ><span>Add</span></asp:LinkButton>
							        </FooterTemplate>
						            </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
    </div>
    </form>
</body>
</html>
