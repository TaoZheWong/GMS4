<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEditQuotationDetailDescription.aspx.cs" Inherits="GMSWeb.Sales.Sales.AddEditQuotationDetailDescription" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<base target="_self" />
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
    <meta http-equiv="PRAGMA" content="NO-CACHE" />
    <title>Quotation Detail Description</title>
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-timepicker/bootstrap-timepicker.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite_app.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <style>
        body {
            overflow: auto;
        }
    </style>
</head>
<body style="background-image:none; text-align:left">
    <form id="form1" runat="server">
    <div class="container">
        <h5 class="page-header m-t-20">Product Description:</h5>
            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="False"
                AllowPaging="false" showfooter="true" OnItemCommand="dgData_CreateCommand" OnDeleteCommand="dgData_DeleteCommand"
                OnEditCommand="dgData_EditCommand" OnCancelCommand="dgData_CancelCommand" OnUpdateCommand="dgData_UpdateCommand" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" GridLines="None" CellPadding="5" >
                <Columns>
                    <asp:TemplateColumn SortExpression="Description" HeaderText="Product Description">
                        <HeaderStyle></HeaderStyle>
                        <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.Description")%>
                                <input type="hidden" id="hidDescNo" runat="server" value='<%# Eval("DescNo")%>' />                                              
                        </ItemTemplate>
                        <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtEditDescription" Columns="50" MaxLength="2000" TextMode="MultiLine" Height="50" onfocus="select();"
                                CssClass="form-control" onkeyup="update(this);" EnableViewState="true" Text='<%# DataBinder.Eval(Container, "DataItem.Description")%>'></asp:TextBox>                                                   
                                <input type="hidden" id="hidEditDescNo" runat="server" value='<%# Eval("DescNo")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox runat="server" ID="txtDescription" Columns="50" MaxLength="2000" TextMode="MultiLine" Height="50" onfocus="select();"
                                CssClass="form-control" onkeyup="update(this);" EnableViewState="true"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Order">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkGoUp" runat="server" CommandName="GoUp" EnableViewState="true" CssClass="btn btn-default btn-xs"><i class="ti-arrow-up"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkGoDown" runat="server" CommandName="GoDown" EnableViewState="true" CssClass="btn btn-default btn-xs"><i class="ti-arrow-down"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle />
                        <HeaderStyle Wrap="False" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" 
						ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
						<ItemTemplate>
						    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
						        CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Edit"><i class="ti-pencil"></i></asp:LinkButton>
						    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
						        CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i></asp:LinkButton>
						</ItemTemplate>
						<EditItemTemplate>
                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="true" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="true" CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                        </EditItemTemplate>
						<FooterTemplate>
						<asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
							CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
					</FooterTemplate>
					</asp:TemplateColumn>
                </Columns>
                <HeaderStyle CssClass="tHeader"/>
            </asp:DataGrid>
            <i>Characters typed : <asp:TextBox runat="server" ID="txtCounter" 
                CssClass="textbox" ReadOnly="true" Columns="3"></asp:TextBox> (Limit:2000)</i><br /><br />
    </div>
    </form>
</body>
</html>
