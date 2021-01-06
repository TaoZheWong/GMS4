<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="corporate.aspx.cs" Inherits="GMSWeb.corporate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Ace Dynamics Group Management System</title>
    <script language="javascript">
		function fixSize() 
		{
			document.body.rows = window.frames.header.document.body.scrollHeight + ", *"
		}
		</script>
</head>
    <frameset border="0" frameborder="NO" framespacing="-14" rows="15%,*" cols="100%" onload="fixSize()">
	    <frame name="header" src="corporatebanner.aspx" scrolling="auto" frameborder="no" marginwidth="0" marginheight="0" noresize />
	    <frame runat="server" name="main" src="corporatecontent.aspx" frameborder="no" scrolling="auto" marginwidth="0" marginheight="0" noresize />
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