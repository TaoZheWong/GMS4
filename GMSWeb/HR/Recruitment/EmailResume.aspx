<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailResume.aspx.cs" Inherits="GMSWeb.HR.Recruitment.EmailResume" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>Email Resume</title>
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite_app.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <style>
        body {
            overflow:auto;
        }
    </style>    
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="panel panel-primary m-t-20">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <i class="ti-email"></i>
                        Email <small>Send Resume via Email.</small>
                    </h4>
                </div>
                    <asp:Panel ID="pnlParameter" runat="server" CssClass="panel-body">
                            <div class='form-horizontal m-t-20'>
                                <div class="form-group ">
                                    <label class="col-sm-4 control-label text-left">Message recipient: </label>
                                    <div class="col-sm-8">
                                        <asp:textbox id="txtTo" class="form-control" runat="server" />
                                        <asp:RequiredFieldValidator
									    ID="rfvEmailTo" runat="server" ControlToValidate="txtTo" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEmail" />
                                    </div>
                                </div>
                            </div>
                            </br>
                            <div class='form-horizontal m-t-20'>
                                    <div class="form-group ">
                                        <label class="col-sm-4 control-label text-left">CC : </label>
                                        <div class="col-sm-8">
                                            <asp:textbox id="txtCC" class="form-control" runat="server" />
                                            <asp:RequiredFieldValidator
									    ID="rfvEmailCC" runat="server" ControlToValidate="txtCC" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEmail" />
                                        </div>
                                    </div>
                            </div>
                            </br>
                            <div class='form-horizontal m-t-20'>
                                    <div class="form-group ">
                                        <label class="col-sm-4 control-label text-left">Subject : </label>
                                        <div class="col-sm-8">
                                            <asp:textbox id="txtSubject" class="form-control" runat="server" />
                                            <asp:RequiredFieldValidator
									    ID="rfvSubject" runat="server" ControlToValidate="txtSubject" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEmail" />
                                        </div>
                                    </div>
                            </div>
                            <asp:button id="btn_SendMessage" class="btn btn-primary pull-right" ValidationGroup="valGrpEmail" runat="server" onclick="btn_SendMessage_Click" text="Send message" />
                            <p style="color: Red; size: 7px; font-style: italic;">
                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                            </p>
                    </asp:Panel>
            </div>

            <br />
        </div>
        
    </form>

    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/jquery/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/jquery/jquery-migrate-1.1.0.min.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/bootstrap/dist/js/bootstrap.min.js"></script>
</body>
</html>
