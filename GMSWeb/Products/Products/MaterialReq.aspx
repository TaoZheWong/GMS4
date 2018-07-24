<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="MaterialReq.aspx.cs" Inherits="GMSWeb.Products.Products.MaterialReq" Title="Products - Material Requisition" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">



<script language="javascript" type="text/javascript">   

    function OpenNewMR() {
            var url = 'NewMaterialRequisition.aspx'; 
            window.open(url,'','width=900,height=600,resizable=yes,status=yes,menubar=no,scrollbars=yes');	
    }  
    
    function OpenFullScreenWindow() {
            var url = 'MRPurchaseInformation.aspx'; 
            window.open(url,'','width=800,height=600,resizable=yes,status=yes,menubar=no,scrollbars=yes');	
    } 
    
    function update(ctr, len) {
                       var a = parseInt(len);
                       var b = ctr.value;                       
                       if(ctr.value.length > a)
                       {   
                           document.getElementById(ctr.id).value = b.substring(0,a);
                           alert('Max Length allow for this field is ' + len);
                       }
                        
	                   
                   }     
    
      
</script>

<h1>Product &gt; Material Requisition Search</h1>
            <p>Search existing material requisition. </p>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout ="360000">
            </asp:ScriptManager>
            
            <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0" width="99%">
                <tr>
                    <td class="tbLabel">
                        MR Date From</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="trnDateFrom" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                        <img id="imgCalendarEditFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, document.getElementById('ctl00_ContentPlaceHolderMain_trnDateFrom'), 'dd/mm/yyyy', null, 1);" 
                            height="20" width="17" alt="" align="absMiddle" border="0">
                            
                    </td>
                    <td class="tbLabel" style="width: 15%">
                        MR Date To</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="trnDateTo" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                        <img id="img1" src="../../images/imgCalendar.gif" onclick="showCalendar(this, document.getElementById('ctl00_ContentPlaceHolderMain_trnDateTo'), 'dd/mm/yyyy', null, 1);"
                            height="20" width="17" alt="" align="absMiddle" border="0">
                            
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Customer Code</td>
                    <td style="width:5%">
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtAccountCode" MaxLength="20" Columns="20" onfocus="select();"
                                    CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. DLK690</span>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Customer Name</td>
                    <td style="width:5%">
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtAccountName" MaxLength="50" Columns="20" onfocus="select();"
                                    CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. Keppel</span>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Vendor</td>
                    <td style="width:5%">
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtVendor" MaxLength="50" Columns="20" onfocus="select();"
                                    CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. One WildWood</span>
                    </td>
                </tr>
                <tr>
                <td class="tbLabel">
                    Product Code</td>
                <td style="width: 5%">
                    :</td>
                <td >
                    <asp:TextBox runat="server" ID="txtProductCode" MaxLength="20" Columns="20" onfocus="select();"
                        CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. B1110535616 </span>
                </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Product Name</td>
                    <td style="width: 5%">
                        :</td>
                    <td >
                        <asp:TextBox runat="server" ID="txtProductName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. BLUE-TIG 5356 </span>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Product Group Code</td>
                    <td>
                        :</td>
                    <td >
                        <asp:TextBox runat="server" ID="txtProductGroup" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. B11</span></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Product Group Name</td>
                    <td>
                        :</td>
                    <td >
                        <asp:TextBox runat="server" ID="txtProductGroupName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. BLUEMETALS</span></td>
                </tr>
                
                <tr>
                    <td class="tbLabel">
                        Requestor</td>
                    <td>
                        :</td>
                    <td >                       
                       <asp:TextBox runat="server" ID="txtRequestor" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. Eugene</span></td>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                         Product Manager</td>
                    <td>
                        :</td>
                    <td >
                        <asp:DropDownList ID="ddlProductManager" runat="Server" DataTextField="ProductManagerName"
                    DataValueField="ProductManagerUserID" CssClass="dropdownlist" />
                    </td>
                </tr> 
                <tr>
                <td class="tbLabel">
                    MR No.</td>
                <td style="width: 5%">
                    :</td>
                <td >
                    <asp:TextBox runat="server" ID="txtMRNo" MaxLength="20" Columns="20" onfocus="select();"
                        CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. MR1300028 
                </tr>             
                
                <tr>
                    <td class="tbLabel">
                        Status</td>
                    <td style="width:5%">
                        :</td> 
                        <td>                   
                            <asp:DropDownList CssClass="dropdownlist" ID="ddlStatus" runat="Server" DataTextField="StatusName" DataValueField="StatusID" /> 
                        </td>
                    
                </tr> 
                
                <tr>
                <td class="tbLabel">
                    PO No.</td>
                <td style="width: 5%">
                    :</td>
                <td >
                    <asp:TextBox runat="server" ID="txtPONo" MaxLength="20" Columns="20" onfocus="select();"
                        CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. 1300028 
                </tr>
                <tr>
                    <td class="tbLabel">
                        Purchaser</td>
                    <td style="width:5%">
                        :</td> 
                        <td>                   
                            <asp:DropDownList CssClass="dropdownlist" ID="ddlPurchaser" runat="Server" DataTextField="PurchaserName" DataValueField="PurchaserName" />
                     </td>                    
                </tr>
                <tr>
                <td class="tbLabel">
                    Project No.</td>
                <td style="width: 5%">
                    :</td>
                <td >
                    <asp:TextBox runat="server" ID="txtProjectNo" MaxLength="50" Columns="20" onfocus="select();"
                        CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. PROJ1300028 
                </tr> 
                <tr>
                <td class="tbLabel">
                    Ref. No.</td>
                <td style="width: 5%">
                    :</td>
                <td >
                    <asp:TextBox runat="server" ID="txtRefNo" MaxLength="50" Columns="20" onfocus="select();"
                        CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. REF1300028 
                </tr> 
                 <tr>
                <td class="tbLabel">
                    Budget Code</td>
                <td style="width: 5%">
                    :</td>
                <td >
                    <asp:TextBox runat="server" ID="txtBudgetCode" MaxLength="50" Columns="20" onfocus="select();"
                        CssClass="textbox"></asp:TextBox> <span style="color:Green; size:7px; font-style:italic;">e.g. ENG1300028 
                </tr> 
                <tr> 
                 <td class="tbLabel">
                    Requestor Remarks</td>
                <td style="width: 5%">
                    :</td>
                <td>
                    <asp:TextBox runat="server" ID="txtRequestorRemarks" TextMode="MultiLine" Width="300px" Height="40" CssClass="textbox" onkeyup="update(this,'500');"></asp:TextBox>                
                </td>
                
                <tr>
                    <td colspan="6" align="right">
                        
                        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnSearch_Click" ></asp:Button>
                        <%--<asp:Button runat="server" ID="btnImport" Text="Import Product Fm A21" CssClass="button" OnClick="btnImport_Click" Visible="false" OnClientClick ="progress_update()" /></asp:Button>--%>
                        <asp:Button ID="btnAdd" Text="Add" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnAddMR_Click" ></asp:Button>     
                    </td>
                    
                </tr> 
                                                 
            </table> 
            <asp:UpdatePanel ID="upOutter" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <table align="center" style="margin-left: auto; margin-right: auto;">
                        <tr align="center"><td><div id="importing" style="visibility:hidden"><span style="font-style:italic;">Importing ...</span></div></td></tr>
                        <tr><td>
                        <div id="showbar" style="font-size:8pt;padding:2px;border:solid #D8D8D8 1px;visibility:hidden">
                        <span id="progress1">&nbsp; &nbsp;</span>
                        <span id="progress2">&nbsp; &nbsp;</span>
                        <span id="progress3">&nbsp; &nbsp;</span>
                        <span id="progress4">&nbsp; &nbsp;</span>
                        <span id="progress5">&nbsp; &nbsp;</span>
                        <span id="progress6">&nbsp; &nbsp;</span>
                        <span id="progress7">&nbsp; &nbsp;</span>
                        <span id="progress8">&nbsp; &nbsp;</span>
                        <span id="progress9">&nbsp; &nbsp;</span>        
                        </div>
                        </td></tr>
                        </table>
            
            <table style="margin-left: 8px">
                <tr>
                    <td>
                        <div id="Div1" style="text-align: left; " runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="tTable">
                        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" 
                             GridLines="none" CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true"
                             PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" OnItemDataBound="dgData_ItemDataBound" EnableViewState="true" OnSortCommand="SortData" AllowSorting="true">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                                    .
                                                    <input type="hidden" id="hidMRNo" runat="server" value='<%# Eval("MRNo")%>' />
                                                    
                                                    </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="MRNo" SortExpression="MRNo" HeaderStyle-Wrap="false" ItemStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl=""><%# Eval("MRNo")%></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="MRDate" SortExpression="MRDate" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" >
                                                <ItemTemplate>                                                    
                                                    <%# Eval("MRDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("MRDate", "{0: dd-MMM-yyyy}")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Status" SortExpression="StatusID" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="90px"> 
                                                <ItemTemplate>
                                                   <%# Eval("StatusName")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn> 
                                            <asp:TemplateColumn HeaderText="Requestor" SortExpression="StatusID" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"> 
                                                <ItemTemplate>
                                                   <%# Eval("Requestor")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn> 
                                            <asp:TemplateColumn HeaderText="Product Manager" SortExpression="StatusID" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"> 
                                                <ItemTemplate>
                                                   <%# Eval("PMRealName")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn> 
                                            <asp:TemplateColumn HeaderText="Vendor" SortExpression="VendorName" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"> 
                                                <ItemTemplate>
                                                   <%# Eval("VendorName")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn> 
                                            <asp:TemplateColumn HeaderText="Purchaser" SortExpression="Purchaser" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"> 
                                                <ItemTemplate>
                                                   <%# Eval("Purchaser")%>
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
            
            </ContentTemplate>
            </asp:UpdatePanel>
            
            
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
</asp:Content>
