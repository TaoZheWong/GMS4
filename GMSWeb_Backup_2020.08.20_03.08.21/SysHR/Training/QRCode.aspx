<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QRCode.aspx.cs" Inherits="GMSWeb.SysHR.Training.QRCode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Session QR Code</title>
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
  <link rel="stylesheet" href="/css/styles.css" type="text/css"/>
    <style>
        body {
            font-family: Helvetica,Arial,sans-serif;
            font-size: 14px;
            line-height: 22px;
            text-align: center;
        }
    </style>
</head>
<body>
    <div class="page">
        <h1>Session QR Code</h1>
        <div class="results">
	        <p><img id="Qrimage" src="<%=path%>" alt="QR Code" runat="server" /></p>
	        <p><strong>Please scan this QR code to fill in Training Evaluation Form.</strong></p>
        </div>
        <br/>
    </div>
</body>
</html>
