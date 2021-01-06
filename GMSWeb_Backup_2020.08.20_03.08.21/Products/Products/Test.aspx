<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="GMSWeb.Products.Products.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View POr Detail</title>   

</head>
<body style="background-image:none; text-align: left" >
    <form id="form1" runat="server">
       <div>
       <h1>Products &gt; PO Detail</h1>
    <p>PO Information: </p>
   
    <input type="hidden" id="hidPOCode" runat="server" />
    <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0"
        width="650px">
        <tr>
        <td class="tbLabel">
                        MR Date From</td>
                    <td>
                        :</td>
                    <td>
                       
                        <asp:TextBox runat="server" ID="trnDateFrom" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                        <img id="imgCalendarEditFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, document.getElementById('ctl00_ContentPlaceHolderMain_trnDateFrom'), 'dd/mm/yyyy', null, 1);" height="20" width="17" alt="" align="absMiddle" border="0">
                        
                        <asp:TextBox runat="server" id="datepicker" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                    </td>
        </tr>
        
        
    </table>
      
      </div> 
      
    </form>
</body>
</html>

