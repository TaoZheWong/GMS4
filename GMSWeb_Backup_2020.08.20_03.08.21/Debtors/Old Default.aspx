<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Old Default.aspx.cs" Inherits="GMSWeb.Debtors.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Leeden Group Management System</title>
    <script language="javascript">
		function fixSize() 
		{
			document.body.rows = window.frames.header.document.body.scrollHeight + ", *"
		}
    </script>
    <link rel="icon" href="http://sgcitrix/gms/favicon.ico" type="image/x-icon" />
<link rel="shortcut icon" href="http://sgcitrix/gms/favicon.ico" type="image/x-icon" />
</head>
    <frameset frameborder="1" framespacing="-14" rows="94px,*" cols="100%" onload="fixSize()">
	    <frame name="header" src="../common/banner.aspx" scrolling="no" frameborder="1" marginwidth="0" marginheight="0" noresize />
	    <frame name="dashboard" src="dashboard.aspx" frameborder="0" scrolling="auto" marginwidth="0" marginheight="0" noresize />
    </frameset>
	<noframes>
		<body>
			<p>
				This HTML frameset displays multiple Web pages. To view this frameset, 
				use a Web browser that supports HTML 4.0 and later.
			</p>
		</body>
	</noframes>	
</html>