<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteHeader1.ascx.cs" Inherits="GMSWeb.SiteHeader1" %>
<meta http-equiv="Pragma" content="no-cache" />
<meta http-equiv="Expires" content="-1" />
<meta http-equiv="Cache-Control" content="no-cache" />
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

<script type="text/javascript">
//<!--
	var sDOMAIN = "<%= Request.ApplicationPath %>";
	

//-->
</script>

<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/jQueryUI-1.11.4/jquery-ui.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/jQueryUI-1.11.4/jquery-ui.min.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/css/bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/css/bootstrap-datetimepicker.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/css/fileinput.min.css" media="screen"/>

<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/DataTables-1.10.12/css/dataTables.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/AutoFill-2.1.2/css/autoFill.bootstrap.min.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/css/buttons.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/ColReorder-1.3.2/css/colReorder.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/FixedColumns-3.2.2/css/fixedColumns.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/FixedHeader-3.1.2/css/fixedHeader.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/KeyTable-2.1.2/css/keyTable.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Responsive-2.1.0/css/responsive.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/RowReorder-1.1.2/css/rowReorder.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Scroller-1.4.2/css/scroller.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Select-1.2.0/css/select.bootstrap.css" media="screen"/>

<script type="text/javascript">
        var ua = window.navigator.userAgent;       
        var msie = ua.indexOf("MSIE ");        
        if (msie > 0) // If Internet Explorer, return version number
        {
            alert("You are using an incompatible web browse. Please use Google Chrome.");            
        }
</script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/jQuery-2.2.3/jquery-2.2.3.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/jQueryUI-1.11.4/jquery-ui.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/bootstrap.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/moment.min.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/bootstrap-datetimepicker.min.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/plugins/canvas-to-blob.min.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/plugins/sortable.min.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/plugins/purify.min.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/fileinput.min.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/plugins/canvas-to-blob.min.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/validator.js"></script>

<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/JSZip-2.5.0/jszip.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/pdfmake-0.1.18/build/pdfmake.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/pdfmake-0.1.18/build/vfs_fonts.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/DataTables-1.10.12/js/jquery.dataTables.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/DataTables-1.10.12/js/dataTables.bootstrap.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/AutoFill-2.1.2/js/dataTables.autoFill.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/AutoFill-2.1.2/js/autoFill.bootstrap.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/dataTables.buttons.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/buttons.bootstrap.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/buttons.colVis.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/buttons.flash.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/buttons.html5.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/buttons.print.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/ColReorder-1.3.2/js/dataTables.colReorder.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/FixedColumns-3.2.2/js/dataTables.fixedColumns.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/FixedHeader-3.1.2/js/dataTables.fixedHeader.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/KeyTable-2.1.2/js/dataTables.keyTable.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Responsive-2.1.0/js/dataTables.responsive.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/RowReorder-1.1.2/js/dataTables.rowReorder.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Scroller-1.4.2/js/dataTables.scroller.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/DataTables-Bootstrap/Select-1.2.0/js/dataTables.select.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/Common.js?v1=5"></script>

<link rel="icon" href="https://gms/gms/favicon.ico" type="image/x-icon" />
<link rel="shortcut icon" href="https://gms/gms/favicon.ico" type="image/x-icon" />





