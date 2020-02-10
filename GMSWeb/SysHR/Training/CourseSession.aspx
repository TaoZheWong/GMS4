<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="CourseSession.aspx.cs" Inherits="GMSWeb.SysHR.Training.CourseSession1" Title="Training - Course Session Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>

<ul class="breadcrumb pull-right">
    <li><a href="#">Training</a></li>
    <li class="active">Course Session</li>
</ul>
<h1 class="page-header">Course Session <br />
    <small> List of training course sessions.</small>
</h1>


<asp:ScriptManager ID="scriptMgr" runat="server" />

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
        <div class="form-horizontal form-group-sm row m-t-20">
            <div class="form-group col-lg-4 col-sm-6">
                <label class="col-sm-6 control-label text-left">Course Title</label>
                <div class="col-sm-6">
                    <asp:TextBox runat="server" ID="searchCourseTitle" MaxLength="80" Columns="50" onfocus="select();" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="clearfix"></div>
            <div class="form-group col-lg-4 col-sm-6">
                <label class="col-sm-6 control-label text-left">Date From</label>
                <div class="col-sm-6">
                    <div class="input-group date">
                        <asp:TextBox runat="server" ID="dateFrom" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="datepicker form-control"></asp:TextBox>
						<span class="input-group-addon"><i class="ti-calendar"></i></span>
					</div>
                    
                </div>
            </div>
            <div class="form-group col-lg-4 col-sm-6">
                <label class="col-sm-6 control-label text-left">Date To</label>
                <div class="col-sm-6">
                    <div class="input-group date">
                        <asp:TextBox runat="server" ID="dateTo" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="datepicker form-control"></asp:TextBox>
                        <span class="input-group-addon"><i class="ti-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel-footer clearfix">
        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="pull-right btn btn-primary m-l-5" OnClick="btnSearch_Click"></asp:Button>
        <asp:Button ID="btnAdd" Text="Add" EnableViewState="False" runat="server" CssClass="pull-right btn btn-default" OnClick="btnAdd_Click"></asp:Button>
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
        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="False" ShowFooter="True"
        DataKeyField="CourseID" OnPageIndexChanged="dgData_PageIndexChanged" GridLines="None"
        CellPadding="5" Cellspacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="True" PageSize="20"
        OnDeleteCommand="dgData_DeleteCommand" OnItemDataBound="dgData_ItemDataBound">
        <Columns>
            <asp:TemplateColumn HeaderText="No">
                <ItemTemplate>
                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)%>
                    <input type="hidden" id="hidCourseSessionID" runat="server" value='<%# Eval("CourseSessionID")%>' />
                    </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Course Code">
                <ItemTemplate>
                    <%# Eval( "CourseObject.CourseCode" )%>
                </ItemTemplate>
                <HeaderStyle Wrap="False" />
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Course Title">
                <ItemTemplate>
                    <asp:Label ID="lblCourseTitle" runat="server">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "AddEditSession.aspx?COURSESESSIONID="+Eval("CourseSessionID")%>'><%# Eval("CourseObject.CourseTitle")%></asp:HyperLink>
                    </asp:Label>
                </ItemTemplate>
                <HeaderStyle Wrap="False" />
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Course Organizer">
                <ItemTemplate>
                    <asp:Label ID="lblOrganizer" runat="server">
							<%# Eval("CourseObject.CourseOrganizerObject.OrganizerName")%>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="QR Code">
                <ItemTemplate>
                    <asp:Label ID="lblQR" runat="server">
                        <asp:HyperLink ID="lnkTEF" runat="server" Target="_blank" NavigateUrl="TEFsignin.aspx" Visible='<%# (Eval("TEFList.Count").ToString() != "0") ? true : false %>'  CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="View">
                            <i class="ti-search"></i> </asp:HyperLink>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Date From" >
                <ItemTemplate>
                    <asp:Label ID="lblDateFrom" runat="server">
							<%# Eval("DateFrom", "{0: dd-MMM-yyyy HH:mm}")%>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Date To" >
                <ItemTemplate>
                    <asp:Label ID="lblDateTo" runat="server">
							<%# Eval("DateTo", "{0: dd-MMM-yyyy HH:mm}")%>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn  HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                        CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete">
                                <i class="ti-trash"></i> </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <HeaderStyle CssClass="tHeader" />
        <FooterStyle CssClass="tFooter" />
        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
    </asp:DataGrid>
    </div>
  
</div>
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
            $(".sub-course-session").addClass("active");
        });
    </script>
</asp:Content>