<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinanceReportViewer.aspx.cs" Inherits="GMSWeb.Reports.Report.FinanceReportViewer" %>

<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=11.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>Print</title>
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-timepicker/bootstrap-timepicker.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" />
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
    <form id="Form1" runat="server">
        <div class="container">
            <div class="panel panel-primary m-t-20">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <i class="ti-bar-chart"></i>
                        Report Parameter <small>Select or enter the report parameters in below.</small>
                    </h4>
                </div>
                    <asp:Panel ID="pnlParameter" runat="server" CssClass="panel-body row"></asp:Panel>
            </div>

            <br />


            <div class="container">
                <asp:Label ID="lblFeedback" runat="server" Text="" EnableViewState="false" />
                <CR:CrystalReportViewer ID="cyReportViewer" runat="server" AutoDataBind="true" DisplayGroupTree="false"
                    EnableDatabaseLogonPrompt="False" EnableDrillDown="false" DisplayToolbar="true"
                    EnableTheming="False" HasSearchButton="true" HasToggleGroupTreeButton="true"
                    PrintMode="ActiveX" HasExportButton="true" />
            </div>
        </div>


    </form>
</body>
</html>
