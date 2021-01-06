<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrainingAttendee.aspx.cs" Inherits="GMSWeb.SysHR.Training.TrainingAttendee" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Training Attendee</title>
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
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="panel panel-primary m-t-20">
                <div class="panel-heading">
                    <h4 class="panel-title">Package Info
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false"
                        GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Name" HeaderStyle-HorizontalAlign="center"
                                ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedDate" runat="server">
					                    <%# Eval("EmployeeObject.Name")%>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Attended" HeaderStyle-HorizontalAlign="center"
                                ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <div class="checkbox">
                                        <asp:CheckBox ID="chkAttended" runat="server" Checked='<%# Eval("Attended")%>' Text=" "/>
                                        <input type="hidden" id="hidEmployeeCourseID" runat="server" value='<%# Eval("EmployeeCourseID")%>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <HeaderStyle CssClass="tHeader" />
                        <AlternatingItemStyle CssClass="tAltRow" />
                        <FooterStyle CssClass="tFooter" />
                    </asp:DataGrid>
                </div>
                <div class="panel-footer clearfix">
                    <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
            OnClick="btnSubmit_Click"></asp:Button><br />
                </div>
            </div>
        </div>
        
        <asp:Label ID="lblMsg" runat="server" />
    </form>
</body>
</html>
