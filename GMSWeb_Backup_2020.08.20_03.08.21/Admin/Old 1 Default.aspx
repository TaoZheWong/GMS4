<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Old 1 Default.aspx.cs" Inherits="GMSWeb.Admin.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="SharpPieces.Web.Controls.ExtendedDropDownList" Namespace="SharpPieces.Web.Controls"
    TagPrefix="piece" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Leeden's Group Management System</title>
</head>
   <body>
<div id="wrap"><!-- wrap starts here -->
	<div id="header">
		<form id="companylist" runat="server">
               <table cellpadding="0px" cellspacing="4px" width="100%" border="0">
               <tr>
               <td style="width: 30%">
                    <h1 id="logo">Leeden<span class="gray">GMS</span></h1> 
		            <h2 id="slogan">...the Leeden's Group Management System</h2>
               </td>
               <td align="right">
                   <h2 id="username">Welcome&nbsp;<asp:Label runat="server" ID="lblWelcome"></asp:Label>!&nbsp;&nbsp;|&nbsp;&nbsp;
                   <a id="changePassword" href="../Admin/Accounts/ChangePassword.aspx" target="_blank" title="Change password">Change Your Password Here</a></h2>
               </td>
               </tr>
               <tr>
               <td></td>
               <td align="right">
                        <asp:Label CssClass="toplabel" ID="lblCompany" runat="server" Text="Company :"></asp:Label>
                        <piece:ExtendedDropDownList CssClass="list" ID="ddlCompany" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" >
                        </piece:ExtendedDropDownList></li>       
               </td></tr>
               </table>
          </form>
	</div>
		
	<div id="menu">
	    <ul runat="server" id="linkTag"></ul>
		<!--
		<ul>
			<li id="current"><a href="index.html">Home</a></li>
			<li><a href="index.html">News</a></li>
			<li><a href="index.html">Downloads</a></li>
			<li><a href="index.html">Services</a></li>
			<li><a href="index.html">Support</a></li>
			<li><a href="index.html">About</a></li>			
		</ul>-->		
	</div>					
	
	<div id="sidebar" >
	
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
		
	</div>
		
	<div id="main">
			
		<a name="TemplateInfo"></a>
		<h1>Template Info</h1>
				
        <p><strong>Citrus Island</strong> is a free, W3C-compliant, CSS-based website template
        by <a href="http://www.styleshout.com/">styleshout.com</a>. This work is
        distributed under the <a rel="license" href="http://creativecommons.org/licenses/by/2.5/">
        Creative Commons Attribution 2.5  License</a>, which means that you are free to
        use and modify it for any purpose. All I ask is that you give me credit by including a <strong>link back</strong> to
        <a href="http://www.styleshout.com/">my website</a>.
        </p>

        <p>
        You can find more of my free template designs at <a href="http://www.styleshout.com/">my website</a>.
        For premium commercial designs, you can check out
        <a href="http://www.dreamtemplate.com" title="Website Templates">DreamTemplate.com</a>.
        </p>
				
		<p class="post-footer">
			<a href="index.html" class="readmore">Read more</a>
			<a href="index.html" class="comments">Comments (7)</a>
			<span class="date">Oct 11, 2006</span>	
		</p>
			
		<a name="SampleTags"></a>				
		<h1>Sample Tags</h1>
				
		<h3>Code</h3>				
		<p><code>
		code-sample { <br />
		font-weight: bold;<br />
		font-style: italic;<br />				
		}
		</code></p>	
				
		<h3>Example Lists</h3>
				
		<ol>
			<li>example of</li>
			<li>ordered list</li>								
		</ol>	
							
		<ul>
			<li>example of</li>
			<li>unordered list</li>								
		</ul>				
				
		<h3>Blockquote</h3>			
		<blockquote><p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy 
		nibh euismod tincidunt ut laoreet dolore magna aliquam erat....</p></blockquote>
				
		<h3>Image and text</h3>
		<p><a href="http://getfirefox.com/"><img src="images/firefox-gray.jpg" width="100" height="120" alt="firefox" class="float-left" /></a>
		Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec libero. Suspendisse bibendum. 
		Cras id urna. Morbi tincidunt, orci ac convallis aliquam, lectus turpis varius lorem, eu 
		posuere nunc justo tempus leo. Donec mattis, purus nec placerat bibendum, dui pede condimentum 
		odio, ac blandit ante orci ut diam. Cras fringilla magna. Phasellus suscipit, leo a pharetra 
		condimentum, lorem tellus eleifend magna, eget fringilla velit magna id neque. Curabitur vel urna. 
		In tristique orci porttitor ipsum. Aliquam ornare diam iaculis nibh. Proin luctus, velit pulvinar 
		ullamcorper nonummy, mauris enim eleifend urna, congue egestas elit lectus eu est. 				
		</p>
								
		<h3>Example Form</h3>
		<form action="#">		
			<p>				
			<label>Name</label>
			<input name="dname" value="Your Name" type="text" size="30" />
			<label>Email</label>
			<input name="demail" value="Your Email" type="text" size="30" />
			<label>Your Comments</label>
			<textarea rows="5" cols="5"></textarea>
			<br />	
			<input class="button" type="submit" />		
			</p>		
		</form>				
		<br />						
							
	</div>	
	
</div><!-- wrap ends here -->	
	
<!-- footer starts here -->	
	<div id="footer">
		<div id="footer-content">
		<div id="footer-right">
			<a href="index.html">Home</a> |  	
			<a href="index.html">Site Map</a> |
   		    <a href="index.html">RSS Feed</a> |
            <a href="http://validator.w3.org/check/referer">XHTML</a> |
   		    <a href="http://jigsaw.w3.org/css-validator/check/referer">CSS</a>
		</div>
		
		<div id="footer-left">
			&copy; 2011 Leeden Limited |
			<a href="http://www.bluewebtemplates.com/" title="Website Templates">website templates</a> by <a href="http://www.styleshout.com/">styleshout</a>

		</div>
		</div>	
	</div>
<!-- footer ends here -->	
	
</body>

</html>