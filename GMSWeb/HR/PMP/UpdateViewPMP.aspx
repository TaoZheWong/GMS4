<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateViewPMP.aspx.cs" Inherits="GMSWeb.HR.PMP.UpdateViewPMP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite.css" rel="stylesheet" />
    <link href="../../new_assets/css/layout.css" rel="stylesheet" />
    <link href="../../new_assets/css/component.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <title>PMP Documents</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <i class="ti-list-ol"></i>
                        <asp:Label ID="lblTitle" runat="server" />
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive">
                        <asp:DataGrid ID="dgDocument" OnItemCommand="dgDocument_ItemCommand" OnItemDataBound="dgDocument_ItemDataBound" OnPageIndexChanged="dgDocument_PageIndexChanged" runat="server"
                            AutoGenerateColumns="false" GridLines="none" ShowFooter="true"
                            CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="True" PageSize="10">
                            <Columns>
                                <asp:TemplateColumn HeaderText="S/N">
                                    <ItemTemplate>
                                        <%# (Container.ItemIndex + 1) + ((dgDocument.CurrentPageIndex) * dgDocument.PageSize)%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Year" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblYear" runat="server"><%# Eval( "Year" )%>  /</asp:Label>
                                        <asp:Label ID="lblNextYear" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList CssClass="form-control input-sm" runat="server" ID="ddlNewYear" DataTextField="Year" DataValueField="Year">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Type" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblType" Text='<%# Eval("Type")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList runat="server" ID="ddlNewType" CssClass="form-control input-sm">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Date Uploaded" HeaderStyle-HorizontalAlign="left"
                                    ItemStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="linkName" runat="server" Text='<%# Eval("DateUploaded")%>' CommandName="Load" ForeColor="#005DAA" CommandArgument='<%#Eval("FileName")%>'></asp:LinkButton>
                                        <input type="hidden" id="hidDocumentID" runat="server" value='<%# Eval("DocumentID")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div class="input-group">
                                            <input type="text" class="form-control input-sm" readonly>
                                             <label class="input-group-btn">
                                                <span class="btn btn-primary btn-upload">
                                                    <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                                    <asp:FileUpload CssClass="form-control hidden" ID="FileUpload1" runat="server" />
                                                </span>
                                            </label>
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("DocumentID")%>' EnableViewState="false"
                                            CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete">
                                <i class="ti-trash"></i> </asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                            ValidationGroup="valGrpNewRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                            <HeaderStyle CssClass="tHeader" />
                            <FooterStyle CssClass="tFooter" />
                            <PagerStyle  CssClass="grid_pagination" Mode="NumericPages" />
                        </asp:DataGrid>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
