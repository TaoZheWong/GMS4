<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="SessionTimeOut.aspx.cs" Inherits="GMSWeb.SessionTimeOut" Title="Session Timeout Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<TABLE BORDER="0" WIDTH="100%" HEIGHT="100%" ALIGN="center" ID="Table1">
			<TR>
				<TD VALIGN="middle" ALIGN="center">
					<DIV>Your GMS session has expired.<br />
						Please follow the link below to re-login to GMS.
					</DIV>
					<BR />
					<a href="Default.aspx">GMS Homepage</a>
				</TD>
			</TR>
		</TABLE>
</asp:Content>
