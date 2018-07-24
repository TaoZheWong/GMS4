<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeCourse.aspx.cs" Inherits="GMSWeb.SysHR.Training.EmployeeCourse" Title="Training - Training Record Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <a name="TemplateInfo"></a>

    <ul class="breadcrumb pull-right">
        <li><a href="#">Training</a></li>
        <li class="active">Search Training Record</li>
    </ul>
    <h1 class="page-header">Records <br />
        <small>List of training courses taken by the employees.</small>
    </h1>

    <asp:ScriptManager ID="sriptmgr1" runat="server">
        <Services>
            <asp:ServiceReference Path="AutoCompleteEmployeeName.asmx" />
        </Services>
    </asp:ScriptManager>

    <asp:UpdatePanel ID="udpEmpCourseUpdater" runat="server" UpdateMode="conditional">
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
                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">Course</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtSearchCourse" runat="server" Columns="50" MaxLength="80" onfocus="select();"
                                    CssClass="form-control" />
                            </div>
                        </div>
                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">Employee Name</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtSearchName" runat="server" Columns="40" MaxLength="40" onfocus="select();"
                                    CssClass="form-control" />
                            </div>
                        </div>

                        <div class="clearfix"></div>

                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">Date From</label>
                            <div class="col-sm-6">
                                <div class="input-group date">
                                    <asp:TextBox runat="server" ID="dateFrom" MaxLength="10" Columns="10" onfocus="select();"
                                        CssClass="datepicker form-control"></asp:TextBox>
                                    <span class="input-group-addon"><i class="ti-calendar"></i></span>
                                </div>
                            </div>
                        </div>

                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">Date To</label>
                            <div class="col-sm-6">
                                <div class="input-group date">
                                    <asp:TextBox runat="server" ID="dateTo" MaxLength="10" Columns="10" onfocus="select();"
                                        CssClass="datepicker form-control"></asp:TextBox>
                                    <span class="input-group-addon"><i class="ti-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">Course Registration</label>
                            <div class="col-sm-6">
                                <div class="radio-inline m-b-3">
                                    <asp:RadioButton ID="rbRegistrationStatusAll" runat="server" Text="All" Checked="true" GroupName="RegistrationStatus" />
                                    <asp:RadioButton ID="rbRegistrationStatusPending" runat="server" Text="Pending" GroupName="RegistrationStatus" />
                                    <asp:RadioButton ID="rbRegistrationStatusApproved" runat="server" Text="Completed" GroupName="RegistrationStatus" />
                                </div>
                            </div>
                        </div>

                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">TEF</label>
                            <div class="col-sm-6">
                                <div class="radio-inline m-b-3">
                                    <asp:RadioButton ID="rbTEFStatusAll" runat="server" Text="All" Checked="true" GroupName="TNFStatus" />
                                    <asp:RadioButton ID="rbTEFStatusPending" runat="server" Text="Pending" GroupName="TNFStatus" />
                                    <asp:RadioButton ID="rbTEFStatusCompleted" runat="server" Text="Completed" GroupName="TNFStatus" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-lg-6 col-sm-6">
                            <label class="col-sm-6 control-label text-left">PTEF</label>
                            <div class="col-sm-6">
                                <div class="radio-inline m-b-3">
                                    <asp:RadioButton ID="rbPTEFStatusAll" runat="server" Text="All" Checked="true" GroupName="CEFStatus" />
                                    <asp:RadioButton ID="rbPTEFStatusPending" runat="server" Text="Pending" GroupName="CEFStatus" />
                                    <asp:RadioButton ID="rbPTEFStatusCompleted" runat="server" Text="Completed" GroupName="CEFStatus" />
                                </div>
                            </div>
                        </div>

                        <div class="clearfix"></div>

                        <div class=" pull-right m-b-5">
                            <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary"
                                OnClick="btnSearch_Click"></asp:Button></td>
                        </div>

                    </div>
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
                    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                        GridLines="none" OnItemDataBound="dgData_ItemDataBound"
                        CellPadding="5" OnDeleteCommand="dgData_DeleteCommand"
                        CellSpacing="5" CssClass="table table-striped table-hover table-condensed" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged"
                        EnableViewState="true">
                        <Columns>
                            <asp:TemplateColumn HeaderText="No">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                    <input type="hidden" id="hidEmployeeCourseID" runat="server" value='<%# Eval("EmployeeCourseID")%>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Employee Name" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpNo2" runat="server">
                                        <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%# "CourseRegistration.aspx?EMPLOYEECOURSEID="+Eval("EmployeeCourseID")+"&FORMTYPE=EC"%>'><%# Eval( "EmployeeObject.Name" )%></asp:HyperLink>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Course" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseTitle" runat="server">
							                   <%# Eval("CourseSessionObject.CourseObject.CourseTitle")%>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Date From" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <%# Eval("CourseSessionObject.DateFrom", "{0: dd-MMM-yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Date To" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <%# Eval("CourseSessionObject.DateTo", "{0: dd-MMM-yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Status" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server">
							                   <%# (Eval("Status").ToString() == "A") ? "Approved" : ((Eval("Status").ToString() == "R") ? "Rejected" : "Pending")%>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="TEF Status" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblTEFStatus" runat="server">
							                   <%# (Eval("TEFList.Count").ToString() != "0") ? ((Eval("TEFList[0].Status").ToString() == "A") ? "Completed" : "Pending") : "N.A" %>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="View TEF" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblTEF" runat="server">
                                        <asp:HyperLink ID="lnkTEF" runat="server" Target="_blank" NavigateUrl='<%# "AddEditTEF.aspx?EMPLOYEECOURSEID="+Eval("EmployeeCourseID")%>' Visible='<%# (Eval("TEFList.Count").ToString() != "0") ? true : false %>'  CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="View">
                                <i class="ti-search"></i> </asp:HyperLink>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="PTEF Status" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPTEFStatus" runat="server">
							                   <%#  (Eval("CourseSessionObject.CourseObject.RequirePTJNPTEF").ToString() == "True") ? ((Eval("ActualValueAfterCourse") == null) ? "Pending" : "Completed") : "N.A"%>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
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
            $(".sub-employee-course").addClass("active");
        });
    </script>
</asp:Content>