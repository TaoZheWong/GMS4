<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPOWithoutSupplierInfo.aspx.cs" Inherits="GMSWeb.Products.Products.ViewPOWithoutSupplierInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View POr Detail</title>   

</head>
<body style="background-image:none; text-align: left" >
    <form id="form1" runat="server">
       <div>
       <h1>
        Products &gt; PO Detail
</h1>
    <p>PO Information: </p>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <input type="hidden" id="hidPOCode" runat="server" />
    <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0"
        width="650px">
        
        <tr>
            <td class="tbLabel">
               PO No.</td>
            <td style="width: 5%">
                :</td>
            <td colspan="2">
                <asp:Label runat="server" ID="lblPONo"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="tbLabel">
                PO Date
            </td>    
            <td>
                :</td>
            <td colspan="2">
                <asp:Label runat="server" ID="lblPODate"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="tbLabel">
                Ref No.</td>
            <td style="width: 5%">
                :</td>
            <td colspan="2">
                <asp:Label runat="server" ID="lblTrnNo"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="tbLabel">
                Purchaser</td>
            <td style="width: 5%">
                :</td>
            <td colspan="2">
                <asp:Label runat="server" ID="lblPurchaser"></asp:Label>
            </td>
        </tr>
        
       
       
        
    </table>
    <br /><br />
    <div style="margin-left: 10px; margin-right: 30px">
        <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Width="104%"
            CssClass="ajax__tab_xp">
            
            <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Product Item">
                <ContentTemplate>
                    <asp:UpdatePanel ID="updatePanel2" runat="server">
                        <ContentTemplate>
                        
                        <table class="tTable1" cellspacing="5" width="100%">
                                <tr>
                                    <td colspan="3">                                        
                                        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="false" OnItemDataBound="dgData_ItemDataBound" 
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true" GridLines="none" Width="100%">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>'>
                                                        </asp:Label>
                                                        .</ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Product Code">
                                                    <ItemTemplate>
                                                        <%# Eval("ProductCode")%>
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Description">
                                                    <ItemTemplate>
                                                        <%# Eval("ProductDescription")%>
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="UOM">
                                                    <ItemTemplate>
                                                        <%# Eval("uom")%>
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Qty">
                                                    <ItemTemplate>                                                        
                                                        <asp:Label runat="server" ID="lblOrderQuantity" Text='<%# Eval("Quantity")%>'></asp:Label>
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>                                               
                                                
                                                
                                                
                                                
                                                
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                            
                                        </asp:DataGrid></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel> 
        </ajaxToolkit:TabContainer>
    </div>
    
    <asp:UpdatePanel ID="updatePanel6" runat="server">
        <ContentTemplate>
            <table width="99%" style="margin-left: 8px" cellspacing="5" cellpadding="5">
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblCreatedBy"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblModifiedBy"></asp:Label></td>
                            </tr>
                            
                        </table>
                    </td>
                   
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="TABCOMMAND">
        
    </div>
       </div>
       <br />
    </form>
</body>
</html>
