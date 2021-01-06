<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attachment.aspx.cs" Inherits="GMSWeb.Sales.Engineering.Project.Attachment" %>

<head id="Head1" runat="server">
<script src="Attachment.aspx.js" type="text/javascript"></script>
</head>
<form enctype="multipart/form-data" runat="server">
<body>
<br />
<input id="btnSubmit" class="btn btn-warning" type="submit" value="Upload" runat="server" onserverclick="btnSubmit_ServerClick" />
<p id="addremove"></p>
<table id="tblAttachment" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>

    <div class="row" id='upload'>
            <div class="col-sm-3"><div class="form-group">
                
                <div class='input-group date' id='Div3'>
                    <asp:FileUpload  id="FileUpload" runat="server"></asp:FileUpload >
                    <input type="text" id="txtPrjNo" runat="server" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-upload"></span>
                    </span>
                </div>
            </div>
        </div>
    </div>
</form>
<div id="dialog-confirm1" title="Upload?" >
    <p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>Are you sure?</p>
</div>
</body>