<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="GMSWeb.Sales.Default2" Title="Sales Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>
    <h1>Sales</h1>
    <p>Main page for Sales.</p>
    
    <table style="text-align: left; width: 100%; margin-left:8px" cellspacing="5" cellpadding="5"> 
               <tr style="background-color: #DADADB; font-weight:bold; margin-top: 10px;">
               <td style="padding:4px;">
               List of MRs requires your approval / action
               </td>
               </tr>
               
               <tr>
               <td>
               <asp:Label ID="lblSummaryPendingForYourApproval" Visible="false" runat="server" />
               <asp:DataGrid ID="dgPendingForYourApproval" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="5"   
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true" 
                                            GridLines="none" Width="100%" OnPageIndexChanged="dgPendingForYourApproval_PageIndexChanged">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6" ItemStyle-Width="5px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgPendingForYourApproval.CurrentPageIndex) * dgPendingForYourApproval.PageSize)  %>'>
                                                        </asp:Label>
                                                       
                                                        .</ItemTemplate>
                                                </asp:TemplateColumn> 
                                                <asp:TemplateColumn HeaderText="MR No." HeaderStyle-Wrap="true" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "../Products/Products/NewMaterialRequisition.aspx?CurrentLink=Products&MRNo="+Eval("MRNo")%>'><%# Eval("MRNo")%></asp:HyperLink>
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>  
                                                <asp:TemplateColumn HeaderText="MR Date " HeaderStyle-Wrap="true" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <%# Eval("MRDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("MRDate", "{0: dd-MMM-yyyy}")%>                                                                                                      
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>                                             
                                                <asp:TemplateColumn HeaderText="Requestor" HeaderStyle-Wrap="true" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <%# Eval("Requestor")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Product Manager" HeaderStyle-Wrap="true" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <%# Eval("pmname")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Date Routed" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("RoutedDate").ToString().Equals("") ? "-" : Eval("RoutedDate", "{0: dd-MMM-yyyy H:mm:ss}")%>                                                                                                          
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn> 
                                                <asp:TemplateColumn HeaderText="Days" HeaderStyle-Wrap="true" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <%# Eval("diff")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="VendorName" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("VendorName")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Purchaser" HeaderStyle-Wrap="true" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <%# Eval("Purchaser")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>                                              
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                            <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                        </asp:DataGrid> 
                                        </td>
                                        </tr>
                                        </table>
                                        
                                        
                                        <br />
    <table style="text-align: left; width: 100%; margin-left:8px" cellspacing="5" cellpadding="5"> 
               <tr style="background-color: #DADADB; font-weight:bold; margin-top: 10px;">
               <td style="padding:4px;">
               List of MRs that were submitted by you but have been rejected / cancelled for the past 7 days
               </td>
               </tr>
               
               <tr>
               <td>
               <asp:Label ID="lblSummary" Visible="false" runat="server" />
               <asp:DataGrid ID="dgMRFormApproval" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="5"   
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true" 
                                            GridLines="none" Width="100%" OnPageIndexChanged="dgMRFormApproval_PageIndexChanged">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6" ItemStyle-Width="5px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgMRFormApproval.CurrentPageIndex) * dgMRFormApproval.PageSize)  %>'>
                                                        </asp:Label>
                                                       
                                                        .</ItemTemplate>
                                                </asp:TemplateColumn> 
                                                <asp:TemplateColumn HeaderText="MR No." HeaderStyle-Wrap="true" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "../Products/Products/NewMaterialRequisition.aspx?CurrentLink=Sales&MRNo="+Eval("MRNo")%>'><%# Eval("MRNo")%></asp:HyperLink>
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>  
                                                <asp:TemplateColumn HeaderText="MR Date " HeaderStyle-Wrap="true" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <%# Eval("MRDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("MRDate", "{0: dd-MMM-yyyy}")%>                                                                                                      
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>                                             
                                                <asp:TemplateColumn HeaderText="Requestor" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("Requestor")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Product Manager" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("pmname")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Date Action" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("ActionDate").ToString().Equals("") ? "-" : Eval("ActionDate", "{0: dd-MMM-yyyy H:mm:ss}")%>                                                                                                          
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn> 
                                                <asp:TemplateColumn HeaderText="Days" HeaderStyle-Wrap="true" ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <%# Eval("diff")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>                                              
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                            <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                        </asp:DataGrid> 
                                        </td>
                                        </tr>
                                        </table>
                                        
                                        
                                        <br />
                                        <table style="text-align: left; width: 100%; margin-left:8px" cellspacing="5" cellpadding="5"> 
               <tr style="background-color: #DADADB; font-weight:bold; margin-top: 10px;">
               <td style="padding:4px;">
               List of MRs that has failed customer's Required Date
               </td>
               </tr>
               
               <tr>
               <td>
               <asp:Label ID="lblSummaryApprovalManager" Visible="false" runat="server" />
               <asp:DataGrid ID="dgMRFormApprovalManager" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="5"   
                                            CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" EnableViewState="true" 
                                            GridLines="none" Width="100%" OnPageIndexChanged="dgMRFormApprovalManager_PageIndexChanged">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6" ItemStyle-Width="5px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgMRFormApprovalManager.CurrentPageIndex) * dgMRFormApprovalManager.PageSize)  %>'>
                                                        </asp:Label>
                                                       
                                                        .</ItemTemplate>
                                                </asp:TemplateColumn> 
                                                <asp:TemplateColumn HeaderText="MR No." HeaderStyle-Wrap="true" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "../Products/Products/NewMaterialRequisition.aspx?CurrentLink=Sales&MRNo="+Eval("MRNo")%>'><%# Eval("MRNo")%></asp:HyperLink>
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>  
                                                <asp:TemplateColumn HeaderText="MR Date " HeaderStyle-Wrap="true" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <%# Eval("MRDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("MRDate", "{0: dd-MMM-yyyy}")%>                                                                                                      
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>                                             
                                                <asp:TemplateColumn HeaderText="Requestor" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("Requestor")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Account Code" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("customeraccountcode")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                 <asp:TemplateColumn HeaderText="Account Name" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("customername")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Required Date" HeaderStyle-Wrap="true" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%# Eval("requireddate").ToString().Equals("") ? "-" : Eval("requireddate", "{0: dd-MMM-yyyy}")%>                                                                                                          
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn> 
                                                <asp:TemplateColumn HeaderText="Days" HeaderStyle-Wrap="true" ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <%# Eval("diff")%> 
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateColumn>                                              
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                                            <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                        </asp:DataGrid> 
                                        </td>
                                        </tr>
                                        </table>
</asp:Content>
