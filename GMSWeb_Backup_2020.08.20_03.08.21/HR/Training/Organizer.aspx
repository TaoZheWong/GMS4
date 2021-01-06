<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Organizer.aspx.cs" Inherits="GMSWeb.HR.Training.Organizer" Title="Training - Organizer" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <h1 class="page-header">Organizer 
        <br />
        <small>Main page for Organizer.</small>
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
            $(".sub-organizer").addClass("active");
        });
    </script>
</asp:Content>
