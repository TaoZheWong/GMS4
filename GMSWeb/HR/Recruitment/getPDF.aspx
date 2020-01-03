<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getPDF.aspx.cs" Inherits="GMSWeb.HR.Recruitment.getPDF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
		document.onmousedown=disableclick;
		status="Right Click Disabled !!!";
		function disableclick(event)
		{
		  if(event.button==2)
		   {
			 alert(status);
			 return false;    
		   }
		}
		</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
