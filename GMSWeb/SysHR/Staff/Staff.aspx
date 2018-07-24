<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Staff.aspx.cs" Inherits="GMSWeb.SysHR.Staff.Staff" Title="Training - Staff Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
		<li><a href="#">HR Organisation </a></li>
		<li class="active">Employee Listing</li>
	</ul>
    <h1 class="page-header">
		Staff <small>List of employees.</small>
	</h1>
<asp:ScriptManager ID="sriptmgr1" runat="server" />
<asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="udpEmpCourseUpdater">
<ContentTemplate>
    
<div class="panel panel-primary">
    <div class="panel-heading">
        <div class="panel-heading-btn">
            <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
        </div>
		<h4 class="panel-title">
		    <i class="ti-search"></i>
            Search filter
		</h4>
	</div>
    <div class="panel-body">
        <div class="form-group-sm row m-t-20">
		    <div class="form-group col-lg-3 col-md-6 col-sm-6">
			    <label class="control-label">Employee No</label>
				    <asp:TextBox ID="txtSearchEmployeeNo" runat="server" Columns="20" MaxLength="30" onfocus="select();" CssClass="form-control" />
		    </div>
            <div class="form-group col-lg-3 col-md-6 col-sm-6">
			    <label class="control-label">Name</label>
				    <asp:TextBox ID="txtSearchName" runat="server" Columns="20" MaxLength="30" onfocus="select();" CssClass="form-control" />
		    </div>
           <div class="form-group col-lg-3 col-md-6 col-sm-6">
			    <label class="control-label">NRIC</label>
				    <asp:TextBox ID="txtSearchNRIC" runat="server" Columns="20" MaxLength="30" onfocus="select();" CssClass="form-control" />
		    </div>
            <div class="form-group col-lg-3 col-md-6 col-sm-6">
			    <label class="control-label">Designation</label>
				    <asp:TextBox ID="txtSearchDesignation" runat="server" Columns="20" MaxLength="30" onfocus="select();" CssClass="form-control" />
		    </div>
            <div class="form-group col-lg-3 col-md-6 col-sm-6">
			    <label class="control-label">Grade</label>
				    <asp:TextBox ID="txtSearchGrade" runat="server" Columns="10" MaxLength="10" onfocus="select();" CssClass="form-control" />
		    </div>
            <div class="form-group col-lg-3 col-md-6 col-sm-6">
			    <label class="control-label">Is Active</label>
                    <div class="radio m-b-3">
                        <asp:RadioButton ID="rbIsActive" runat="server" Text="Yes" Checked="true" GroupName="IsActive" />
                        <asp:RadioButton ID="rbIsNotActive" runat="server" Text="No" GroupName="IsActive" />
				    </div>
		    </div>
	    </div>
    </div>
    <div class="panel-footer clearfix">
        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right" OnClick="btnSearch_Click"></asp:Button>
    </div>
</div>


<div class="panel panel-primary" id="resultList" runat="server" visible="false">
    <div class="panel-heading">
        <div class="panel-heading-btn">
            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
        </div>
		<h4 class="panel-title">
		    <i class="ti-align-justify"></i> 
            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
		</h4>
	</div>
    <div class="panel-body">
        <asp:DataList ID="dlData" runat="server" DataKeyField="EmployeeNo" CssClass="row"
            RepeatDirection="Horizontal" OnItemCommand="dlData_ItemCommand" RepeatLayout="Flow" ItemStyle-CssClass="col-lg-4 col-md-6 col-sm-6">
            <HeaderTemplate>
                <ul class="pager m-t-0 m-b-10">
                    <li class="previous"><asp:LinkButton ID="lnkPrevPage2" runat="server" CommandName="ChangePage" CommandArgument="-1"
                                CausesValidation="False"> <i class="ti-angle-left"></i> Previous
                            </asp:LinkButton></li>
                    <li class="next">
                        <asp:LinkButton ID="lnkNextPage2" runat="server" CommandName="ChangePage" CommandArgument="1"
                                CausesValidation="False">Next <i class="ti-angle-right"></i>
                            </asp:LinkButton>
                    </li>
                </ul>
                <div class="clearfix"></div>
            </HeaderTemplate>
            <FooterTemplate>
                <div class="clearfix"></div>
                <ul class="pager m-t-0 m-b-10">
                    <li class="previous"><asp:LinkButton ID="lnkPrevPage" runat="server" CommandName="ChangePage" CommandArgument="-1"
                                CausesValidation="False"> <i class="ti-angle-left"></i> Previous
                            </asp:LinkButton></li>
                    <li class="next">
                        <asp:LinkButton ID="lnkNextPage" runat="server" CommandName="ChangePage" CommandArgument="1"
                                CausesValidation="False">Next <i class="ti-angle-right"></i>
                            </asp:LinkButton>
                        
                    </li>
                </ul>
            </FooterTemplate>
            <FooterStyle />
            <ItemTemplate>
              
                <div class="widget widget-card dynamic text-center" onclick='<%# "viewEmployeeDetail(" +Eval("EmployeeID")+");return false;" %>'>
                    <div class="widget-card-cover">
                        <div class="cover-bg"></div>
                    </div>
                    <div class="widget-card-content inverse-mode profile-header-content">
                        <div class="m-b-10 m-t-10 profile-header-img">
                             <asp:Image ID="Photo1" runat="server" ImageUrl='<%#"MakeThumbnail.aspx?size=small&path=" + Request.ApplicationPath + "/Data/HR/Photo/"+Eval("EmployeeID")+".JPG"+"&t=" + new Random().NextDouble().ToString() %>' />
                        </div>
                        <div class="profile-header-info">
							<h4><%# Eval("Name") %> </h4>
                            <h5><%# Eval("EmployeeNo") %></h5>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="widget-card-content bg-gray p-10">
                        <div class="row">
                             <div class="col-md-6 p-10">
                                <div class="widget-title"><%# Eval("Designation") %></div>
                                <div class="widget-desc"><%# Eval("Department") %> </div>
                            </div>
                             <div class="col-md-6 p-10">
                                <div class="widget-title"><%# Eval("Grade") %></div>
                                <div class="widget-desc">Grade</div>
                            </div>
                        </div>
                        <div class="row">
                           
                            <div class="col-md-6 p-10">
                                <div class="widget-title"><%# Eval("DateJoined").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("DateJoined", "{0: dd-MMM-yyyy}")%></div>
                                <div class="widget-desc">Date Joined</div>
                            </div>
                            <div class="col-md-6 p-10">
                                <div class="widget-title"><%# Eval("Qualification")%></div>
                                <div class="widget-desc">Qualification</div>
                            </div>
                        </div>
                    </div>
                </div>

                <%#(Container.ItemIndex +1)%3 == 0 ? "<span class='clearfix'></span>":""%>
                     

            </ItemTemplate>
        </asp:DataList>

    </div>
</div>



</ContentTemplate>
</asp:UpdatePanel>
<br />
<div class="TABCOMMAND">
    <asp:UpdatePanel ID="udpMsgUpdater" runat="server"  UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".list-of-people-menu").addClass("active expand");
            $(".hr-menu").addClass("active expand");
            $(".sub-staff").addClass("active");
        });
    </script>
</asp:Content>