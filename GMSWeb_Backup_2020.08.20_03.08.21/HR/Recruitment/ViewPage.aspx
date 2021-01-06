<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPage.aspx.cs" Inherits="GMSWeb.HR.Recruitment.ViewPage" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Resume</title>
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
      <script src="script.js"></script>
    <script type="text/javascript">
        function zoomin() {
            var myImg = document.getElementById("pdfToImage");
            var currWidth = myImg.clientWidth;
            if (currWidth == 2500) return false;
            else {
                myImg.style.width = (currWidth + 100) + "px";
            }
        }
        function zoomout() {
            var myImg = document.getElementById("pdfToImage");
            var currWidth = myImg.clientWidth;
            if (currWidth == 100) return false;
            else {
                myImg.style.width = (currWidth - 100) + "px";
            }
        }
    </script>
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
		document.onkeydown = function (event)
		{
		    event = (event || window.event);
		    if (event.keyCode == 123 || event.keyCode == 18 ||event.ctrlKey && event.shiftKey && event.keyCode==73)
		    {
		        alert("This function is disabled here");
		        return false;
		    }
		}
		</script>
    <style>
        .btn-circle{
             width: 50px;
            height: 50px;
            padding: 10px 16px;
            font-size: 20px;
            line-height: 1.33;
            border-radius: 50%;
            margin-bottom:10px;
            box-shadow: 1px 1px #888888;
            border: 2px solid black;
            background-color: white;
            color: dodgerblue;
        }

        .info {
          border-color: #2196F3;
          background: white;
          color: dodgerblue;
        }

        .info:hover {
          background: #2196F3;
          color: white;
        }
    </style>
</head>

<body oncontextmenu="return false" >
    <form runat="server">
        <div >
            <div class="btn-group-vertical" style="position:fixed;bottom:5%;right:3%;">
                <button type="button" class="btn info btn-lg btn-circle" onclick="zoomin()" style="border-radius: 50%;"><i class="ti-plus"></i></button>
                <button type="button" class="btn info btn-lg btn-circle" onclick="zoomout()" style="border-radius: 50%;"><i class="ti-minus"></i></button>
            </div> 
            <asp:image ID="pdfToImage" runat="server" ImageUrl="#" style="height:auto;width:100%;" draggable="false" onmousedown="return false"/> 
        </div>
    </form>
</body>
</html>


