<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" CodeBehind="Records.aspx.cs" Inherits="GMSWeb.HR.Training.Records" Title="Training - Records" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <h1 class="page-header">Records 
        <br />
        <small>Main page for Records.</small>
    </h1>

    <div class="row flex-container text-center">
     
        <p class="flex-item">
            This page is still under construction. Stay tuned for the upcoming new feature in GMS! For more information, please contact your System Administrator.
        </p>
    
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".training-menu").addClass("active expand");
            $(".sub-records").addClass("active");
        });
    </script>
</asp:Content>
