<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionTimeoutMain.aspx.cs" Inherits="GMSWeb.SessionTimeoutMain" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head lang="en">
    <meta charset="utf-8" />
    <title>Session Timeout Page</title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport" />
    <meta name="theme-color" content="#007ee5">

    <!-- ================== BEGIN BASE CSS STYLE ================== -->
    <link href="<%= Request.ApplicationPath %>/new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="<%= Request.ApplicationPath %>/new_assets/plugins/DataTables/datatables.min.css" rel="stylesheet" />
    <link href="<%= Request.ApplicationPath %>/new_assets/plugins/bootstrap-timepicker/bootstrap-timepicker.css" rel="stylesheet" />
    <link href="<%= Request.ApplicationPath %>/new_assets/plugins/jquery-ui/themes/black-tie/jquery-ui.min.css" rel="stylesheet" />
    <link href="<%= Request.ApplicationPath %>/new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <link href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/css/fileinput.min.css" media="screen" rel="stylesheet" />
    <link href="<%= Request.ApplicationPath %>/new_assets/css/animate.min.css" rel="stylesheet" />
    <link href="<%= Request.ApplicationPath %>/new_assets/css/style.min.css" rel="stylesheet" />
    <link href="<%= Request.ApplicationPath %>/new_assets/css/overwrite_app.css" rel="stylesheet" />
    <!-- ================== END BASE CSS STYLE ================== -->
</head>
<body style="background-image: none;">
    <form id="form1" runat="server">
        <!-- BEGIN #page-container -->
        <div id="page-container">
            <!-- BEGIN error-page -->
            <div class="error-page">
                <div class="error-icon"><i class="ti-alert"></i></div>
                <h1>Oops!</h1>
                <h3>Your GMS session has expired</h3>
                <p>
                    <br />
                    <br />
                    <br />
                    Please follow the link below to re-login to GMS.
                </p>
                <p>
                    <a href="Default.aspx">GMS Homepage</a>
                </p>
            </div>
            <!-- END error-page -->
        </div>
        <!-- END #page-container -->
    </form>
</body>
</html>
