<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Common/Site.Master" CodeBehind="PhotoIDUpdate.aspx.cs" Inherits="GMSWeb.SysHR.Staff.PhotoIDUpdate" Title="Photo ID Update"%>

<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
		<li><a href="#">HR Organisation </a></li>
		<li class="active">Photo ID Update</li>
	</ul>
    <h1 class="page-header">
		Photo ID <small>Updates of employees' photos </small>
	</h1>

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        
    </ajaxToolkit:ToolkitScriptManager>
    <style>
        .profileimg {
            margin-left:auto;
            margin-right:auto;
            width: 120px;
            height: 120px;
            overflow: hidden;
            z-index: 10;
            padding: 3px;
            border-radius: 4px;
            background: #fff
        }

        .image-thumbnail-wrapper {
            padding: 25px 0;
            background: #eeeeee;
        }
    </style>
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
		    <div class="form-group col-lg-12 col-md-12 col-sm-12">
                    <div class="col-sm-12 control-label text-left">
                        <label class="control-label">Employee No</label>
                        <asp:TextBox ID="txtEmployeeNo" runat="server" Columns="40" MaxLength="40" autocomplete="off"
                                        onfocus="select();" onChange="changeEmployeeNo(this)" CssClass="form-control" />
                        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2" ID="autoComplete2"
                            TargetControlID="txtEmployeeNo" ServicePath="AutoCompleteEmployeeNo.asmx"
                            ServiceMethod="GetCompletionListByEmployeeNo" MinimumPrefixLength="1" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                        </ajaxToolkit:AutoCompleteExtender>
                        <div style="display: none">
                            <asp:DropDownList ID="ddlEmployee" runat="Server" DataTextField="Name" DataValueField="EmployeeNo" />
                        </div>
                    </div>
		    </div>
	    </div>
    </div>
    <div class="panel-footer clearfix">
        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right" OnClick="btnSearch_Click"></asp:Button>
    </div>
</div>


<div class="panel panel-primary" id="resultList" runat="server" visible="false">
    <div class="panel-body">
        <asp:DataList ID="dlData" runat="server" DataKeyField="EmployeeNo" CssClass="tblTable"
                    OnItemCommand="dlData_OnItemCommand" >       
            <ItemTemplate>
                <input type="hidden" id="hidEmployeeID" runat="server" value='<%# Eval("EmployeeID")%>' />
                        <div class="col-lg-12 panel-body p-t-20" style="background:#fff;">
                            <div class=" col-lg-6 col-sm-12 p-t-20 text-center">
                                <div class="image-thumbnail-wrapper">
                                     <asp:Image ID="Photo1" runat="server" height="300px" Width="225px" CssClass="img-thumbnail" ImageUrl='<%#"MakeThumbnail.aspx?size=small&path=" + Request.ApplicationPath + "/Data/HR/Photo/"+Eval("EmployeeID")+".JPG" +"&t=" + new Random().NextDouble().ToString()  %>' /><br />
                                </div>
                            </div>
                            <div class=" col-lg-6 col-sm-12 text-center">
                                <div class="form-horizontal m-t-20">
                                    <div class="form-group col-lg-12 ">
                                        <label class="col-lg-12 control-label text-left">Employee No: <%# Eval("EmployeeNo") %></label>
                                    </div>
                                     <div class="form-group col-lg-12 ">
                                        <label class="col-lg-12 control-label text-left">Name: <%# Eval("Name") %></label>
                                    </div>
                                    <div class="form-group col-lg-12 ">
                                        <label class="col-lg-12 control-label text-left">Company: <%# Eval("CompanyObject.Name") %></label>
                                    </div>
                                </div>
                                <br>
                                <div class="input-group" style="border: thick">      
                                    <input type="text" class="form-control" readonly  style="border-radius: 0 ; border:none;" placeholder="Select a photo"/>
                                        <label class="input-group-btn">
                                            <span class="btn btn-primary btn-upload" style="border-radius: 0;">
                                                <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                                <asp:FileUpload CssClass="form-control hidden" ID="FileUpload1" runat="server" accept="image/*" onchange="ShowPreview()" />
                                            </span>
                                        </label>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FileUpload1"
                                        ErrorMessage="No Photo Detected." Display="dynamic" ValidationGroup="attachment" />
                            </div>
                        </div>
                        <div class="panel-footer clearfix">
                            <div class=" col-lg-12 col-sm-12 p-t-10 text-center">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" >
                                    <ContentTemplate>
                                        <asp:Button ID="btnUpdate" Text="Upload" EnableViewState="False" runat="server" CommandName="Update" CssClass="btn btn-primary"
                                                     ValidationGroup="attachment" ></asp:Button>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpdate" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <br>
               <asp:Image ID="impPrev" runat="server" Height="200px" class="p-t-20"/> 
        </div>
</div>
</ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnSearch" />
        <asp:AsyncPostBackTrigger ControlID="dlData" />
    </Triggers>
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
            $(".sub-update-photo").addClass("active");
        });
    </script>
    
</asp:Content>