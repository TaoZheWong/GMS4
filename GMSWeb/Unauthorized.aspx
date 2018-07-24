<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Unauthorized.aspx.cs" Inherits="GMSWeb.Unauthorized" Title="Unauthorized Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<TABLE BORDER="0" WIDTH="100%" HEIGHT="100%" ALIGN="center" ID="Table1">
			<TR>
				<TD VALIGN="middle" ALIGN="center">
					<DIV>You are not authorised to view this page. 
						<BR>
						For more information, please contact your System Administrator.
					</DIV>
					<!--
					<BR>
					<INPUT TYPE="button" ONCLICK="history.go(-1);" VALUE="Back" CLASS="button" ID="Button1"
						NAME="Button1">-->
				</TD>
			</TR>
		</TABLE>
</asp:Content>
