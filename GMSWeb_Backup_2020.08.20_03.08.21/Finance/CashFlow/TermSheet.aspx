<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master"  CodeBehind="TermSheet.aspx.cs" Inherits="GMSWeb.Finance.CashFlow.TermSheet" Title="Cash Flow - Term Sheet" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <h1 class="page-header">Term Sheet 
        <br />
        <small>Main page for Term Sheet.</small>
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
            $(".cash-flow-menu").addClass("active expand");
            $(".sub-term-sheet").addClass("active");
        });
    </script>
</asp:Content>
