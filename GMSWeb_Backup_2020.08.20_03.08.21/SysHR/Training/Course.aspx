<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Course.aspx.cs" Inherits="GMSWeb.SysHR.Training.Course" Title="Training - Course Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>

<ul class="breadcrumb pull-right">
    <li><a href="#">Training</a></li>
    <li class="active">Course</li>
</ul>
<h1 class="page-header">Course <br />
    <small>List of training courses.</small>
</h1>

<asp:ScriptManager ID="scriptMgr" runat="server" />
<asp:UpdatePanel ID="udpCourseUpdater" runat="server" UpdateMode="conditional">
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
                <div class="form-horizontal row m-t-20">
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">Course Title</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="searchCourseTitle" MaxLength="80" Columns="50" onfocus="select();" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">Course Type</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlCourseType" runat="server" CssClass="form-control">
                                <asp:ListItem Value="%">-All-</asp:ListItem>
                                <asp:ListItem Value="E">External</asp:ListItem>
                                <asp:ListItem Value="I">Internal</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel-footer clearfix">
                    <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-l-10"
                            OnClick="btnSearch_Click"></asp:Button>
                    <asp:Button ID="btnAdd" Text="Add" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right"
                            OnClick="btnAdd_Click"></asp:Button>
            </div>
        </div>
        
    
        
        
        <div class="panel panel-primary">
            <div class="panel-heading">
                <div class="panel-heading-btn">
                    <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                </div>
                <h4 class="panel-title">
                    <i class="ti-align-justify"></i>
                    <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                </h4>
            </div>
            <div class="table-responsive">
                    <asp:DataGrid ID="dgCourse" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                        DataKeyField="CourseID" OnPageIndexChanged="dgCourse_PageIndexChanged" GridLines="None" 
                        CellPadding="5" CellSpacing="5" CssClass="table table-striped table-hover table-condensed"
                        AllowPaging="True" PageSize="20" >
                        <Columns>
                            <asp:TemplateColumn HeaderText="No">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((dgCourse.CurrentPageIndex) * dgCourse.PageSize)  %>
                                 </ItemTemplate>
                                <ItemStyle/>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Course Code">
                                <ItemTemplate>
                                    <%# Eval( "CourseCode" )%>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Course Title">
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseTitle" runat="server">
							        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "AddEditCourse.aspx?COURSEID="+Eval("CourseID")%>'><%# Eval("CourseTitle")%></asp:HyperLink>
                                    </asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Course Organizer">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrganizer" runat="server">
									<%# Eval("CourseOrganizerObject.OrganizerName")%>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <HeaderStyle CssClass="tHeader" />
                        <FooterStyle CssClass="tFooter" />
                        <PagerStyle Mode="NumericPages" cssClass="grid_pagination"/>
                    </asp:DataGrid>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
<div class="TABCOMMAND">
    <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".training-menu").addClass("active expand");
            $(".sub-course").addClass("active");
        });
    </script>
</asp:Content>