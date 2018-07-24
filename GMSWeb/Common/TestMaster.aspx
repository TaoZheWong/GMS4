<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="TestMaster.aspx.cs" Inherits="GMSWeb.Common.TestMaster" Title="Test Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderSideBar" runat="server">
        <h1>Sidebar Menu</h1>
		<ul class="sidemenu">
			<li><a href="index.html">Home</a></li>
			<li><a href="#TemplateInfo">Template Info</a></li>
			<li><a href="#SampleTags">Sample Tags</a></li>
			<li><a href="http://www.styleshout.com/">More Free Templates</a></li>	
			<li><a href="http://www.dreamtemplate.com" title="Web Templates">Web Templates</a></li>
		</ul>
			
		<h1>Sponsors</h1>
		<ul class="sidemenu">
            <li><a href="http://www.dreamtemplate.com" title="Website Templates">DreamTemplate</a></li>
            <li><a href="http://www.themelayouts.com" title="WordPress Themes">ThemeLayouts</a></li>
            <li><a href="http://www.imhosted.com" title="Website Hosting">ImHosted.com</a></li>
            <li><a href="http://www.dreamstock.com" title="Stock Photos">DreamStock</a></li>
            <li><a href="http://www.evrsoft.com" title="Website Builder">Evrsoft</a></li>
            <li><a href="http://www.webhostingwp.com" title="Web Hosting">Web Hosting</a></li>
		</ul>
				
		<h1>Wise Words</h1>
		<p>&quot;To know what you know and know what you don't know is 
		the characteristic of one who knows&quot;</p>
		
		<p class="align-right">- Confucius</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
</asp:Content>
