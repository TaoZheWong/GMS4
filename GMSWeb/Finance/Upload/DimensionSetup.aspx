<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="DimensionSetup.aspx.cs" Inherits="GMSWeb.Finance.Upload.DimensionSetup" %>

<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<asp:ScriptManager ID="sriptmgr1" runat="server"></asp:ScriptManager>
<a name="TemplateInfo"></a>
<ul class="breadcrumb pull-right">
    <li><a href="#"><asp:Label ID="lblPageHeader" runat="server" /></a></li>
    <li class="active">Dimension Setup</li>
</ul>
<h1 class="page-header">Dimension Setup <br /><small>Setup of dimension short name.</small></h1>

         
   <%--Company Project Setup --%>   
     <div class="panel panel-primary" id="resultList" runat="server" visible="true">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i>
                Company Project
                <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
            </h4>
        </div>
        <div class="panel-body no-padding">
            <div class="table-responsive">
                <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    DataKeyField="ProjectID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                    OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
                     OnDeleteCommand="dgData_DeleteCommand" OnItemDataBound="dgData_ItemDataBound"
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>   
                                <input type="hidden" id="hidProjectID" runat="server" value='<%# Eval("ProjectID")%>' />                            
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Project Name" HeaderStyle-Wrap="false"  SortExpression="ProjectName">
                            <ItemTemplate>  
                                <asp:Label ID="lblProjectName" runat="server">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                            CausesValidation="false"><span><%# Eval("ProjectCodeName")%></span></asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%# Eval("ProjectCodeName")%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div style="float:left; width:40%">
                                     <asp:TextBox CssClass="form-control input-sm" ID="txtNewProjectCode" runat="server" Columns="20" MaxLength="20" Text='<%# Eval("ProjectCode") %>' 
                                         placeholder="New Project Code"/>
                                      <asp:RequiredFieldValidator
									    ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNewProjectCode" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewProjectName" />
                                </div>
                                <div style="float: right; width:50%">
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtNewProjectName" runat="server" Columns="20" MaxLength="20" Text='<%# Eval("ProjectName") %>' 
                                        placeholder="New Project Name"/>
                                  <asp:RequiredFieldValidator
									ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewProjectName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewProjectName" />
                                </div>
                                <div style="clear:both"></div>
                            </FooterTemplate>
                        </asp:TemplateColumn>                        
                       
                         <asp:TemplateColumn HeaderText="Short Name" HeaderStyle-Wrap="false"  SortExpression="ShortName">
                            <ItemTemplate>
                                <asp:Label ID="lblShortName" runat="server">
												       <%# Eval("ShortName")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditShortName" runat="server" Columns="40" MaxLength="20" Text='<%# Eval("ShortName") %>' />
                                  <asp:RequiredFieldValidator
									ID="rfvEditShortName" runat="server" ControlToValidate="txtEditShortName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProjectName" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewShortName" runat="server" Columns="40" MaxLength="20" 
                                    placeholder="New Short Name"/>
                                 <asp:RequiredFieldValidator
									ID="rfvNewEditShortName" runat="server" ControlToValidate="txtNewShortName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewProjectName" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                    ValidationGroup="valGrpEditProjectName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="true"
                                    ValidationGroup="valGrpNewProjectName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
                    <FooterStyle CssClass="tFooter" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
            </div>
        </div>
         <div class="TABCOMMAND">
    <asp:UpdatePanel ID="udpMsgUpdater1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="MsgPanel1" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div> 
    </div>

   <%--Company Department--%>  
     <div class="panel panel-primary" id="Div1" runat="server" visible="true">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i>
                Company Department
                <asp:Label ID="Label1" Visible="false" runat="server" />
            </h4>
        </div>
        <div class="panel-body no-padding">
            <div class="table-responsive">
                <asp:DataGrid ID="dgData2" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    DataKeyField="DepartmentID" OnCancelCommand="dgData2_CancelCommand" OnEditCommand="dgData2_EditCommand"
                    OnUpdateCommand="dgData2_UpdateCommand" OnItemCommand="dgData2_CreateCommand" GridLines="none"
                    OnDeleteCommand="dgData2_DeleteCommand" OnItemDataBound="dgData2_ItemDataBound"
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData2_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData2.CurrentPageIndex) * dgData2.PageSize)  %>   
                                <input type="hidden" id="hidDepartmentID" runat="server" value='<%# Eval("DepartmentID")%>' />                            
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Department Name" HeaderStyle-Wrap="false"  SortExpression="DepartmentName">
                            <ItemTemplate>  
                                <asp:Label ID="lblDepartmentName" runat="server">
                                    <asp:LinkButton ID="lnkEdit2" runat="server" CommandName="Edit" EnableViewState="false"
                                            CausesValidation="false"><span><%# Eval("DepartmentCodeName")%></span></asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%# Eval("DepartmentCodeName")%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div style="float:left; width:40%">
                                     <asp:TextBox CssClass="form-control input-sm" ID="txtNewDepartmentCode" runat="server" Columns="20" MaxLength="20" Text='<%# Eval("DepartmentCode") %>' 
                                         placeholder="New Department Code"/>
                                      <asp:RequiredFieldValidator
									    ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNewDepartmentCode" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewDepartmentName" />
                                </div>
                                <div style="float: right; width:50%">
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtNewDepartmentName" runat="server" Columns="20" MaxLength="20" Text='<%# Eval("DepartmentName") %>' 
                                        placeholder="New Department Name"/>
                                  <asp:RequiredFieldValidator
									ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNewDepartmentName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewDepartmentName" />
                                </div>
                                <div style="clear:both"></div>
                            </FooterTemplate>
                        </asp:TemplateColumn>                        
                       
                         <asp:TemplateColumn HeaderText="Short Name" HeaderStyle-Wrap="false"  SortExpression="ShortName">
                            <ItemTemplate>
                                <asp:Label ID="lblShortName" runat="server">
												       <%# Eval("ShortName")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditShortName2" runat="server" Columns="40" MaxLength="20" Text='<%# Eval("ShortName") %>' />
                                  <asp:RequiredFieldValidator
									ID="rfvEditShortName" runat="server" ControlToValidate="txtEditShortName2" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditDepartmentName" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewShortName2" runat="server" Columns="40" MaxLength="20" />
                                 <asp:RequiredFieldValidator
									ID="rfvNewEditShortName2" runat="server" ControlToValidate="txtNewShortName2" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewDepartmentName" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete2" runat="server" CommandName="Delete" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkSave2" runat="server" CommandName="Update" EnableViewState="false"
                                    ValidationGroup="valGrpEditDepartmentName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel2" runat="server" CommandName="Cancel" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkCreate2" runat="server" CommandName="Create" EnableViewState="true"
                                    ValidationGroup="valGrpNewDepartmentName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
                    <FooterStyle CssClass="tFooter" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
            </div>
        </div>
         <div class="TABCOMMAND">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="MsgPanel2" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div> 
    </div>

   <%--Company Section--%>
    <div class="panel panel-primary" id="Div2" runat="server" visible="true">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i>
                Company Section
                <asp:Label ID="Label2" Visible="false" runat="server" />
            </h4>
        </div>
        <div class="panel-body no-padding">
            <div class="table-responsive">
                <asp:DataGrid ID="dgData3" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    DataKeyField="SectionID" OnCancelCommand="dgData3_CancelCommand" OnEditCommand="dgData3_EditCommand"
                    OnUpdateCommand="dgData3_UpdateCommand" OnItemCommand="dgData3_CreateCommand" GridLines="none"
                    OnItemDataBound="dgData3_ItemDataBound" OnDeleteCommand="dgData3_DeleteCommand"
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData3_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData3.CurrentPageIndex) * dgData3.PageSize)  %>   
                                <input type="hidden" id="hidSectionID" runat="server" value='<%# Eval("SectionID")%>' />                            
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Section Name" HeaderStyle-Wrap="false"  SortExpression="DepartmentName">
                            <ItemTemplate>  
                                <asp:Label ID="lblDepartmentName" runat="server">
                                    <asp:LinkButton ID="lnkEdit3" runat="server" CommandName="Edit" EnableViewState="false"
                                            CausesValidation="false"><span><%# Eval("SectionCodeName")%></span></asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%# Eval("SectionCodeName")%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div style="float:left; width:40%">
                                     <asp:TextBox CssClass="form-control input-sm" ID="txtNewSectionCode" runat="server" Columns="20" MaxLength="20" Text='<%# Eval("SectionCode") %>' 
                                         placeholder="New Section Code"/>
                                      <asp:RequiredFieldValidator
									    ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNewSectionCode" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewSectionName" />
                                </div>
                                <div style="float: right; width:50%">
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtNewSectionName" runat="server" Columns="20" MaxLength="20" Text='<%# Eval("SectionName") %>' 
                                        placeholder="New Section Name"/>
                                  <asp:RequiredFieldValidator
									ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtNewSectionName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewSectionName" />
                                </div>
                                <div style="clear:both"></div>
                            </FooterTemplate>
                        </asp:TemplateColumn>                        
                       
                         <asp:TemplateColumn HeaderText="Short Name" HeaderStyle-Wrap="false"  SortExpression="ShortName">
                            <ItemTemplate>
                                <asp:Label ID="lblShortName" runat="server">
												       <%# Eval("ShortName")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditShortName3" runat="server" Columns="40" MaxLength="20" Text='<%# Eval("ShortName") %>' />
                                  <asp:RequiredFieldValidator
									ID="rfvEditShortName" runat="server" ControlToValidate="txtEditShortName3" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditSectionName" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewShortName3" runat="server" Columns="40" MaxLength="20" />
                                 <asp:RequiredFieldValidator
									ID="rfvNewEditShortName3" runat="server" ControlToValidate="txtNewShortName3" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewSectionName" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete3" runat="server" CommandName="Delete" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkSave3" runat="server" CommandName="Update" EnableViewState="false"
                                    ValidationGroup="valGrpEditSectionName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel3" runat="server" CommandName="Cancel" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkCreate3" runat="server" CommandName="Create" EnableViewState="true"
                                    ValidationGroup="valGrpNewSectionName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
                    <FooterStyle CssClass="tFooter" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
            </div>
        </div>
         <div class="TABCOMMAND">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="MsgPanel3" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div> 
    </div>
     
   <%--Company Unit--%>
    <div class="panel panel-primary" id="Div3" runat="server" visible="true">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i>
                Company Unit
                <asp:Label ID="Label3" Visible="false" runat="server" />
            </h4>
        </div>
        <div class="panel-body no-padding">
            <div class="table-responsive">
                <asp:DataGrid ID="dgData4" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    DataKeyField="UnitID" OnCancelCommand="dgData4_CancelCommand" OnEditCommand="dgData4_EditCommand"
                    OnUpdateCommand="dgData4_UpdateCommand" OnItemCommand="dgData4_CreateCommand" GridLines="none"
                    OnItemDataBound="dgData4_ItemDataBound" OnDeleteCommand="dgData4_DeleteCommand"
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData4_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData4.CurrentPageIndex) * dgData4.PageSize)  %>   
                                <input type="hidden" id="hidUnitID" runat="server" value='<%# Eval("UnitID")%>' />                            
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Unit Name" HeaderStyle-Wrap="false"  SortExpression="UnitName">
                            <ItemTemplate>  
                                <asp:Label ID="lblUnitName" runat="server">
                                    <asp:LinkButton ID="lnkEdit4" runat="server" CommandName="Edit" EnableViewState="false"
                                            CausesValidation="false"><span><%# Eval("UnitCodeName")%></span></asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%# Eval("UnitCodeName")%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div style="float:left; width:40%">
                                     <asp:TextBox CssClass="form-control input-sm" ID="txtNewUnitCode" runat="server" Columns="20" MaxLength="20" Text='<%# Eval("UnitCode") %>' 
                                         placeholder="New Unit Code"/>
                                      <asp:RequiredFieldValidator
									    ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtNewUnitCode" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewUnitName" />
                                </div>
                                <div style="float: right; width:50%">
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtNewUnitName" runat="server" Columns="20" MaxLength="20" Text='<%# Eval("UnitName") %>' 
                                        placeholder="New Unit Name"/>
                                  <asp:RequiredFieldValidator
									ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtNewUnitName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewUnitName" />
                                </div>
                                <div style="clear:both"></div>
                            </FooterTemplate>
                        </asp:TemplateColumn>                        
                       
                         <asp:TemplateColumn HeaderText="Short Name" HeaderStyle-Wrap="false"  SortExpression="ShortName">
                            <ItemTemplate>
                                <asp:Label ID="lblShortName" runat="server">
												       <%# Eval("ShortName")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditShortName4" runat="server" Columns="40" MaxLength="20" Text='<%# Eval("ShortName") %>' />
                                  <asp:RequiredFieldValidator
									ID="rfvEditShortName" runat="server" ControlToValidate="txtEditShortName4" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditUnitName" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewShortName4" runat="server" Columns="40" MaxLength="20" />
                                 <asp:RequiredFieldValidator
									ID="rfvNewEditShortName4" runat="server" ControlToValidate="txtNewShortName4" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewUnitName" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete4" runat="server" CommandName="Delete" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkSave4" runat="server" CommandName="Update" EnableViewState="false"
                                    ValidationGroup="valGrpEditUnitName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel4" runat="server" CommandName="Cancel" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkCreate4" runat="server" CommandName="Create" EnableViewState="true"
                                    ValidationGroup="valGrpNewUnitName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
                    <FooterStyle CssClass="tFooter" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
            </div>
        </div>
         <div class="TABCOMMAND">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="MsgPanel4" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div> 
    </div>
<div class="TABCOMMAND">
    <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div> 

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".administration-menu").addClass("active expand");
            $(".sub-dimension-setup").addClass("active");
        });
    </script>
</asp:Content>
