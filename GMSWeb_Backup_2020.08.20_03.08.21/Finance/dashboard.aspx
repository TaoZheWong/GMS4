<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="GMSWeb.Finance.dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Finance</title>
</head>
    <FRAMESET BORDER="0" FRAMEBORDER="1" FRAMESPACING="1" COLS="18%,*" ROWS="100%">
		<FRAME NAME="side" SRC="sidebar.aspx"  FRAMEBORDER="0"  SCROLLING="no"
			MARGINWIDTH="0" MARGINHEIGHT="0">
		<FRAME NAME="main" SRC="main.aspx" FRAMEBORDER="0" SCROLLING="auto" MARGINWIDTH="10" MARGINHEIGHT="0"
			BORDERCOLOR="#0135ad">
	</FRAMESET>
	<noframes>
		<body>
			<p>
				This HTML frameset displays multiple Web pages. To view this frameset, 
				use a Web browser that supports HTML 4.0 and later.
			</p>
		</body>
	</noframes>	
</html>