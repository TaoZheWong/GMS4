<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffDetail.aspx.cs" Inherits="GMSWeb.HR.Staff.StaffDetail" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register Assembly="SharpPieces.Web.Controls.ExtendedDropDownList" Namespace="SharpPieces.Web.Controls"
    TagPrefix="piece" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Listing - Employee Details</title>
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-timepicker/bootstrap-timepicker.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite_app.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <style>
        body {
            overflow: auto;
            background-color: rgb(242, 242, 242);
        }

        .image-thumbnail-wrapper {
            padding: 25px 0;
            background: #eeeeee;
        }

        .panel-primary > .panel-heading {
            border-bottom: none !important;
        }


        .btn {
            padding: 9px 12px;
        }
        @media (min-width: 1200px) {
            .image-thumbnail-wrapper {
                min-height: 500px;
                padding: 30px 15px;
            }
        }
    </style>
    <base target="_self" />
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
    <meta http-equiv="PRAGMA" content="NO-CACHE" />

    <script language="javascript" type="text/javascript" src="/GMS3/scripts/popcalendar.js"></script>
    <script language="javascript" type="text/javascript">
        function ConfirmSendEmail(button) {
            if (document.getElementById("dlData_ctl00_rdEditInactive").checked) {
                var n = confirm("The staff has been set to inactive. Send email to the respective parties?");
                if (n)
                    document.getElementById("hidConfirmSendEmail").value = "True";
                else
                    document.getElementById("hidConfirmSendEmail").value = "False";
            }
        }

        function changeSupervisorID(txt) {
            if (txt.value.replace(/^\s+|\s+$/g, '') != '') {

            }
        }

        function jsOpenOperationalReport(url) {
            jsWinOpen2(url, 795, 580, 'yes');
        }

        function jsWinOpen2(x, w, h, haveScroll) {
            var winLeft = (screen.width - w) / 2;
            var winUp = (screen.height - h) / 2;
            if (!window.focus)
                return true;

            haveScroll = 'yes';

            window.open(x, "", "width=" + w + ",height=" + h + ",top=" + winUp + ",left=" + winLeft + ",resizable=yes,status=yes,menubar=no,scrollbars=" + haveScroll);
        }

    </script>

</head>
<body>
    <form id="form1" runat="server" enableviewstate="true">
        <asp:ScriptManager ID="sriptmgr1" runat="server">
            <Services>
                <asp:ServiceReference Path="../Training/AutoCompleteEmployeeName.asmx" />
            </Services>
        </asp:ScriptManager>
        <input type="hidden" id="hidConfirmSendEmail" runat="server" value="True" />
        <div class="container">
            <div class="panel panel-primary m-t-20">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <i class="ti-user"></i>
                        Employee Detail
                    </h4>
                </div>

                <asp:DataList ID="dlData" runat="server" DataKeyField="EmployeeNo" CssClass="tblTable"
                    RepeatDirection="Horizontal" CellPadding="3" OnEditCommand="dlData_EditCommand"
                    OnItemDataBound="dlData_ItemDataBound" OnCancelCommand="dlData_CancelCommand"
                    OnUpdateCommand="dlData_UpdateCommand" OnDeleteCommand="dlData_DeleteCommand" RepeatLayout="Flow">
                    <ItemTemplate>
                        <div class="panel-body no-padding">
                            <div class=" col-lg-3 col-sm-12 no-padding text-center">
                                <div class="image-thumbnail-wrapper">
                                    <a style="height: 200px; width: 200px;" href='<%#"../../Data/HR/Photo/"+Eval("EmployeeID")+".JPG" +"?t=" + new Random().NextDouble().ToString() %>'
                                        target="_blank">
                                        <asp:Image ID="Photo1" runat="server" CssClass="img-thumbnail" ImageUrl='<%#"MakeThumbnail.aspx?size=large&path=" + Request.ApplicationPath + "/Data/HR/Photo/"+Eval("EmployeeID")+".JPG" +"&t=" + new Random().NextDouble().ToString()  %>' /></a>
                                </div>
                            </div>
                            <div class=" col-lg-9 col-sm-12">
                                <div class="form-horizontal m-t-20">
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Employee No</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("EmployeeNo") %>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Company</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("CompanyObject.Name") %>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Name</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("Name") %>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Department</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("Department") %>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Grade</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("Grade") %>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Designation</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("Designation") %>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Date Joined</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("DateJoined").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("DateJoined", "{0: dd-MMM-yyyy}")%>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Qualification</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("Qualification")%>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">DOB</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("DOB").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("DOB", "{0: dd-MMM-yyyy}")%>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">NRIC</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("Nric")%>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Email</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("EmailAddress")%>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Car Plate</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("CarPlate")%>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Supervisor</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# Eval("SuperiorObject.Name")%>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Is Unit Head</label>
                                        <label class="col-sm-6 p-t-5">
                                            <%# (Eval("IsUnitHead") == null || Eval("IsUnitHead").ToString() != "True") ? "No" : "Yes"%>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">KPI</label>
                                        <label class="col-sm-6 p-t-5">
                                            <span style='<%# (Eval("KPIUploadedBy").ToString() == "0") ? "display:none": ""%>'>
                                                <a href='<%#"../Resources/View.aspx?URL=" + AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\KPI\\" + Eval("EmployeeID") + ".pdf"%>' target="_blank">Uploaded By <%# Eval("KPIUploadedByUsersObject.UserRealName")%> On <%# Eval("KPIUploadedDate","{0: dd/MM/yy}")%></a>
                                            </span>
                                            <span style='<%# (Eval("KPIUploadedBy").ToString() == "0") ? "": "display:none"%>'>Not Available
                                            </span>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Job Description</label>
                                        <label class="col-sm-6 p-t-5">
                                            <span style='<%# (Eval("JDUploadedBy").ToString() == "0") ? "display:none": ""%>'>
                                                <a href='<%#"../Resources/View.aspx?URL=" + AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\JobDescription\\" + Eval("EmployeeID") + ".doc"%>' target="_blank">Uploaded By <%# Eval("JDUploadedByUsersObject.UserRealName")%> On <%# Eval("JDUploadedDate","{0: dd/MM/yy}")%></a>
                                            </span>
                                            <span style='<%# (Eval("JDUploadedBy").ToString() == "0") ? "": "display:none"%>'>Not Available
                                            </span>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">PMP</label>
                                        <label class="col-sm-6 p-t-5">
                                            <asp:PlaceHolder ID="plhPMP" runat="server"></asp:PlaceHolder>
                                        </label>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Staff Detail</label>
                                        <label class="col-sm-6 p-t-5">
                                            <asp:PlaceHolder ID="plhStaffDetail" runat="server"></asp:PlaceHolder>
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer clearfix">
                            <input type="hidden" id="hidEmployeeID2" runat="server" value='<%# Eval("EmployeeID")%>' />
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                CssClass="btn btn-danger pull-right" Enabled="false" ToolTip="Delete this employee"><span>Delete</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" EnableViewState="false"
                                CssClass="btn btn-primary pull-right m-r-10" CommandName="Edit" ToolTip="Edit this employee"><span>Edit</span></asp:LinkButton>
                        </div>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <div class="panel-body no-padding">
                            <input type="hidden" id="hidEmployeeID" runat="server" value='<%# Eval("EmployeeID")%>' />
                            <input type="hidden" id="hidCoyID" runat="server" value='<%# Eval("CoyID")%>' />
                            <div class=" col-lg-3 col-sm-12 no-padding text-center">
                                <div class="image-thumbnail-wrapper">
                                    <asp:Image ID="Photo1" runat="server" CssClass="img-thumbnail" ImageUrl='<%#"MakeThumbnail.aspx?size=large&path=" + Request.ApplicationPath + "/Data/HR/Photo/"+Eval("EmployeeID")+".JPG" +"&t=" + new Random().NextDouble().ToString()  %>' /><br />
                                </div>
                                <div class="input-group" style="border: none">
                                    <input type="text" class="form-control" readonly  style="border-radius: 0 ; border:none;" placeholder="Select a photo"/>
                                        <label class="input-group-btn">
                                        <span class="btn btn-primary btn-upload" style="border-radius: 0;">
                                            <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                            <asp:FileUpload CssClass="form-control hidden" ID="FileUpload1" runat="server" />
                                        </span>
                                    </label>
                                </div>
                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                            </div>
                            <div class=" col-lg-9 col-sm-12">
                                <div class="form-horizontal m-t-20">
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Employee No</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox CssClass="form-control" ID="txtEditEmployeeNo" runat="server" Columns="20" MaxLength="15" onfocus="select();"
                                                onchange="this.value = this.value.toUpperCase()" Text='<%# Eval("EmployeeNo") %>' />
                                            <asp:RequiredFieldValidator ID="rfvEditEmployeeNo" runat="server" ControlToValidate="txtEditEmployeeNo"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Company</label>
                                        <div class="col-sm-6">
                                            <piece:ExtendedDropDownList ID="ddlCompany" runat="server" CssClass="form-control">
                                            </piece:ExtendedDropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Name</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txtEditName" runat="server" Columns="40" MaxLength="40" Text='<%# Eval("Name") %>'
                                                onchange="this.value = this.value.toUpperCase()" onfocus="select();" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="rfvEditName" runat="server" ControlToValidate="txtEditName"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Department</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txtEditDepartment" runat="server" Columns="20" MaxLength="20" Text='<%# Eval("Department") %>'
                                                onchange="this.value = this.value.toUpperCase()" onfocus="select();" CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Grade</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox CssClass="form-control" ID="txtEditGrade" runat="server" Columns="5" MaxLength="5" Text='<%# Eval("Grade") %>'
                                                onchange="this.value = this.value.toUpperCase()" onfocus="select();" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Designation</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox CssClass="form-control" ID="txtEditDesignation" runat="server" Columns="50" MaxLength="50" Text='<%# Eval("Designation") %>'
                                                onchange="this.value = this.value.toUpperCase()" onfocus="select();" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Date Joined</label>
                                        <div class="col-sm-6">
                                            <div class="input-group date">
                                                 <asp:TextBox CssClass="form-control datepicker" runat="server" ID="editDateJoined" MaxLength="10" Columns="10" Text='<%# Eval("DateJoined", "{0: dd/MM/yyyy}") %>'></asp:TextBox>
                                                <span class="input-group-addon"><i class="ti-calendar"></i></span>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvEditDateJoined" runat="server" ControlToValidate="editDateJoined"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                            <asp:CompareValidator ID="cvEditDateJoined" runat="server" ErrorMessage="Invalid Date"
                                                ControlToValidate="editDateJoined" Type="Date" Display="Dynamic" ValidationGroup="valGrpEditRow"
                                                Operator="DataTypeCheck"></asp:CompareValidator>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Qualification</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox CssClass="form-control" ID="txtEditQualification" runat="server" Columns="50" MaxLength="50"
                                                Text='<%# Eval("Qualification") %>' onchange="this.value = this.value.toUpperCase()"
                                                onfocus="select();" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">DOB</label>
                                        <div class="col-sm-6">
                                            <div class="input-group date">
                                                <asp:TextBox CssClass="form-control datepicker" runat="server" ID="editDOB" MaxLength="10" Columns="10" Text='<%# Eval("DOB", "{0: dd/MM/yyyy}") %>'></asp:TextBox>
                                                <span class="input-group-addon"><i class="ti-calendar"></i></span>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvEditDOB" runat="server" ControlToValidate="editDOB"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                            <asp:CompareValidator ID="cvEditDOB" runat="server" ErrorMessage="Invalid Date" ControlToValidate="editDOB"
                                                Type="Date" Display="Dynamic" ValidationGroup="valGrpEditRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">NRIC</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox CssClass="form-control" ID="txtEditNRIC" runat="server" Columns="15" MaxLength="15" Text='<%# Eval("NRIC") %>'
                                                onchange="this.value = this.value.toUpperCase()" onfocus="select();" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Email</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox CssClass="form-control" ID="txtEditEmail" runat="server" Columns="50" MaxLength="50" Text='<%# Eval("EmailAddress") %>'
                                                onchange="this.value = this.value.toLowerCase()" onfocus="select();" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Car Plate</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox CssClass="form-control" ID="txtCarPlate" runat="server" Columns="50" MaxLength="10"
                                                Text='<%# Eval("CarPlate") %>' onchange="this.value = this.value.toUpperCase()"
                                                onfocus="select();" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Supervisor</label>
                                        <div class="col-sm-6">
                                            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="udpEmpCourseUpdater">
                                                <ContentTemplate>
                                                    <asp:TextBox CssClass="form-control" ID="txtEditSupervisorName" runat="server" Columns="30" MaxLength="40"
                                                        autocomplete="off" onfocus="select();" Text='<%# Eval("SuperiorObject.Name") %>' onchange="var str = this.value.split(' - ');this.value = str[0].replace(/^\s+|\s+$/g, '').toUpperCase();" />
                                                    <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx" ID="autoComplete2"
                                                        TargetControlID="txtEditSupervisorName" ServicePath="../Training/AutoCompleteEmployeeName.asmx"
                                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Is Unit Head</label>
                                        <div class="col-sm-6">
                                            <div class="radio-inline m-b-3">
                                                <asp:RadioButton ID="rbEditIsUnitHead" runat="server" Text="Yes" Checked='<%# !(Eval("IsUnitHead") == null || Eval("IsUnitHead").ToString() != "True")%>'
                                                    GroupName="UnitHead" /><asp:RadioButton ID="rbEditIsNotUnitHead" runat="server" Text="No"
                                                        Checked='<%# (Eval("IsUnitHead") == null || Eval("IsUnitHead").ToString() != "True")%>'
                                                        GroupName="UnitHead" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">KPI</label>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <input type="text" class="form-control" readonly>
                                                 <label class="input-group-btn">
                                                    <span class="btn btn-primary btn-upload">
                                                        <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                                        <asp:FileUpload CssClass="form-control hidden" ID="FileUpload2" runat="server" />
                                                    </span>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Job Description</label>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <input type="text" class="form-control" readonly>
                                                 <label class="input-group-btn">
                                                    <span class="btn btn-primary btn-upload">
                                                        <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                                        <asp:FileUpload CssClass="form-control hidden" ID="FileUpload3" runat="server" />
                                                    </span>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Is Active</label>
                                        <div class="col-sm-6">
                                            <div class="radio-inline m-b-3">
                                                <asp:RadioButton ID="rdEditActive" runat="server" Text="Yes" Checked='<%# (!(bool) Eval("IsInactive"))%>'
                                                    GroupName="Active" /><asp:RadioButton ID="rdEditInactive" runat="server" Text="No"
                                                        Checked='<%# ((bool) Eval("IsInactive"))%>' GroupName="Active" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-6 col-sm-6">
                                        <label class="col-sm-6 control-label text-left">Date Resigned</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox CssClass="form-control" runat="server" ID="txtDateResigned" MaxLength="10" Columns="10" Text='<%# Eval("DateResigned", "{0: dd/MM/yyyy}") %>'></asp:TextBox>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Invalid Date" ControlToValidate="txtDateResigned"
                                        Type="Date" Display="Dynamic" ValidationGroup="valGrpEditRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer clearfix">
                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                CausesValidation="false" CssClass="btn btn-default pull-right"><span>Cancel</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                ValidationGroup="valGrpEditRow" CssClass="btn btn-primary pull-right m-r-10" OnClientClick="ConfirmSendEmail(this)"><span>Save</span></asp:LinkButton>
                        </div>

                    </EditItemTemplate>
                </asp:DataList>

            </div>
        </div>
        <div class="TABCOMMAND">
            <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </form>

    <!-- ================== BEGIN BASE JS ================== -->
    <script src="<%= Request.ApplicationPath %>/new_assets/plugins/jquery/jquery-1.9.1.min.js"></script> 
    <script src="<%= Request.ApplicationPath %>/new_assets/plugins/jquery/jquery-migrate-1.1.0.min.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/bootstrap/dist/js/bootstrap.min.js"></script>
    <script src="<%= Request.ApplicationPath %>/new_assets/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>
    <!-- ================== END BASE JS ================== -->

    <script>
        $(document).ready(function () {
            $('.datepicker').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });

        });


        $(function () {
            $(document).on('change', ':file', function () {
                var input = $(this),
                    numFiles = input.get(0).files ? input.get(0).files.length : 1,
                    label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                input.trigger('fileselect', [numFiles, label]);
            });

            $(document).ready(function () {
                $(':file').on('fileselect', function (event, numFiles, label) {

                    var input = $(this).parents('.input-group').find(':text'),
                        log = numFiles > 1 ? numFiles + ' files selected' : label;

                    if (input.length) {
                        input.val(log);
                    } else {
                        if (log) alert(log);
                    }

                });
            });

        });
    </script>
</body>
</html>
