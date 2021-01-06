<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEditContact.aspx.cs" Inherits="GMSWeb.Debtors.Debtors.AddEditContact" Title="Customer Info - Add/Edit Contact" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add / Edit Contact</title>   
    
</head>
<body style="background-image:none; text-align: left" >
    <form id="form1" runat="server">
       <div>
       
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <table class="tTable1" style="margin-left: 8px" cellspacing="5" cellpadding="5" border="0"
                width="99%">
                
    <tr>
                            <td colspan="4">
                            <span style="color:Red; size:7px; font-style:italic;"><asp:Label ID="lblContactsMsg" runat="server" ></asp:Label></span>           
                            </td>
                        </tr>               
                
    
    <tr>
    <td class="tbLabel" style="width:20%">
        First Name</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtFirstName" runat="server" Columns="50" MaxLength="50" CssClass="textbox"
            onfocus="select();" /><asp:RequiredFieldValidator ID="rfvCourseTitle" runat="server"
                ControlToValidate="txtFirstName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
       
        <input type="hidden" id="hidContactID1" runat="server" />    
            
    </td>
</tr>

<tr>
    <td class="tbLabel" style="width:20%">
        Last Name</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtLastName" runat="server" Columns="50" MaxLength="50" CssClass="textbox"
            onfocus="select();" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                ControlToValidate="txtLastName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />      
       
    </td>
</tr>

<tr>
    <td class="tbLabel" style="width:20%">
        Salutation</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:DropDownList ID="ddlSalutation" runat="server" DataTextField="Name"
            DataValueField="Name" CssClass="dropdownlist">
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Designation</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtDesignation" runat="server" Columns="30" MaxLength="50" CssClass="textbox"
            onfocus="select();" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                ControlToValidate="txtDesignation" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
    </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Office Phone</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtOfficePhone" runat="server" Columns="30" MaxLength="50" CssClass="textbox"
            onfocus="select();" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                ControlToValidate="txtOfficePhone" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
    </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
       Mobile Phone</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtMobilePhone" runat="server" Columns="30" MaxLength="50" CssClass="textbox" />
    </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Fax</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtFax" runat="server" Columns="30" MaxLength="50"
            CssClass="textbox" />
            
            
         </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Email</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtEmail" runat="server" Columns="30" MaxLength="50"
            CssClass="textbox" />
       <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                ControlToValidate="txtEmail" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewContact" />
       <asp:RegularExpressionValidator ID="regexpName" runat="server" ErrorMessage="Invalid Format" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*" ValidationGroup="valGrpNewRow"/>              
       </td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Remarks</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:TextBox ID="txtContactRemarks" runat="server" TextMode="MultiLine" Rows="3"
                Columns="40" MaxLength="400" CssClass="textarea" /></td>
</tr>
<tr>
    <td class="tbLabel" style="width:20%">
        Is Active</td>
    <td>
        :</td>
    <td colspan="2">
        <asp:CheckBox ID="chkIsActive" runat="server" Checked />
    </td>
</tr>

<tr>
   
    <td colspan="4" align="center">
    <asp:Button runat="server" ID="btnSubmitData" CssClass="button" Text="Submit" OnClick="btnSubmitData_Click" ValidationGroup="valGrpNewContact" />

    </td>
</tr>


          </table>
       </div>
       <br />
    </form>
</body>
</html>
