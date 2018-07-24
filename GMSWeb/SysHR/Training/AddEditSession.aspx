<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="AddEditSession.aspx.cs" Inherits="GMSWeb.SysHR.Training.AddEditSession" Title="Training - Add/Edit Session Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<a name="TemplateInfo"></a>
    
<ul class="breadcrumb pull-right">
    <li><a href="#">Training</a></li>
    <li><a href="#">Session</a></li>
    <li class="active">Add/Edit Course Session</li>
</ul>
<h1 class="page-header">Add Session <br />
    <small>Add or edit a training course session.</small></h1>

<asp:ScriptManager ID="sriptmgr1" runat="server">
<Services>
    <asp:ServiceReference Path="AutoCompleteCourseTtitle.asmx" />
</Services>
</asp:ScriptManager>

<div class="panel panel-primary">
    <div class="panel-heading">
        <div class="panel-heading-btn">
            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
        </div>
        <h4 class="panel-title">
            <i class="ti-notepad"></i>
            New Session Form
        </h4>
    </div>
    <div class="panel-body">
        <div class="form-horizontal m-t-20">
            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Course Title
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtCourseTitle" runat="server" Columns="80" MaxLength="80" CssClass="form-control"
                        onfocus="select();" onchange="this.value = this.value.toUpperCase()" AutoPostBack="true"
                        OnTextChanged="SetCourseInfo" /><asp:RequiredFieldValidator ID="rfvCourseTitle" runat="server"
                            ControlToValidate="txtCourseTitle" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                    <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                        TargetControlID="txtCourseTitle" ServicePath="AutoCompleteCourseTtitle.asmx"
                        ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
                        EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";" CompletionListCssClass="ui-autocomplete" >
                    </ajaxToolkit:AutoCompleteExtender>
                    <input type="hidden" id="hidCourseSessionID" runat="server" />
                    <input type="hidden" id="hidCourseTitle" runat="server" />
                </div>
            </div>
            

            <div class="form-group">
                <label class="col-sm-3 control-label">
                        Date From
                    </label>
                <div class="col-sm-6">
                    <div class="input-group date">
                        <asp:TextBox runat="server" ID="txtDateFrom" MaxLength="10" Columns="10" CssClass="form-control datepicker"></asp:TextBox>
						<span class="input-group-addon"><i class="ti-calendar"></i></span>
					</div>
                    <asp:RequiredFieldValidator ID="rfvDateFrom" runat="server" ControlToValidate="txtDateFrom"
                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                    <asp:CompareValidator ID="cvDateFrom" runat="server" ErrorMessage="Invalid Date"
                        ControlToValidate="txtDateFrom" Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow"
                        Operator="DataTypeCheck"></asp:CompareValidator>
                   
                </div>
                <div class="col-sm-3">
                     <div class="input-group ">
                         <asp:TextBox runat="server" ID="txtDateFromTime" MaxLength="5" Columns="10" CssClass="form-control" data-provide="timepicker" data-show-meridian="false"
                            ToolTip="Time format should be 00:00"></asp:TextBox> 
                         <span class="input-group-addon"><i class="ti-timer"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvateFromTime" runat="server" ControlToValidate="txtDateFromTime"
                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                </div>
        </div>

        <div class="form-group">
            <label class="col-sm-3 control-label">
                Date To
            </label>
            <div class="col-sm-6">
                <div class="input-group date">
                    <asp:TextBox runat="server" ID="txtDateTo" MaxLength="10" Columns="10" CssClass="form-control datepicker"></asp:TextBox>
                    <span class="input-group-addon"><i class="ti-calendar"></i></span>
				</div>
                <asp:RequiredFieldValidator ID="rfvDateTo" runat="server" ControlToValidate="txtDateTo"
                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                <asp:CompareValidator ID="cvDateTo" runat="server" ErrorMessage="Invalid Date" ControlToValidate="txtDateTo"
                    Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow" Operator="DataTypeCheck"></asp:CompareValidator>
            </div>
            <div class="col-sm-3">
                <div class="input-group">
                    <asp:TextBox runat="server" ID="txtDateToTime" MaxLength="5" Columns="10" CssClass="form-control" data-provide="timepicker" data-show-meridian="false"
                        ToolTip="Time format should be 00:00" ></asp:TextBox>
                    <span class="input-group-addon"><i class="ti-timer"></i></span>
                </div>
                <asp:RequiredFieldValidator ID="rfvDateToTime" runat="server" ControlToValidate="txtDateToTime"
                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
            </div>
        </div>
            
                      
            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Course Language
                </label>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlCourseLanguage" runat="server" DataTextField="LanguageName"
                        DataValueField="LanguageID" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
                    
                   
            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Venue
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtVenue" runat="server" Columns="20" MaxLength="200" CssClass="form-control"
                        onfocus="select();" />
                </div>
            </div>


            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Facilitator
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtFacilitator" runat="server" Columns="20" MaxLength="200" CssClass="form-control"
                        onfocus="select();" />
                </div>
            </div>


            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Local Course Fee
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtLocalCourseFee" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
                        ID="cvLocalCourseFee" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalCourseFee"
                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>


            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Local Registration Fee
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtLocalRegistrationFee" runat="server" Columns="7" MaxLength="7"
                        CssClass="form-control" /><asp:CompareValidator ID="cvLocalRegistrationFee" runat="server"
                            ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalRegistrationFee"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>


            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Local Examination Fee
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtLocalExaminationFee" runat="server" Columns="7" MaxLength="7"
                        CssClass="form-control" /><asp:CompareValidator ID="cvLocalExaminationFee" runat="server"
                            ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalExaminationFee"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>



            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Local Membership Fee
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtLocalMembershipFee" runat="server" Columns="7" MaxLength="7"
                        CssClass="form-control" /><asp:CompareValidator ID="cvLocalMembershipFee" runat="server"
                            ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalMembershipFee"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>
                    
                   
            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Local GST
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtLocalGST" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
                        ID="cvLocalGST" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalGST"
                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>


            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Overseas Flight Cost
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtOverseasFlightCost" runat="server" Columns="7" MaxLength="7"
                        CssClass="form-control" /><asp:CompareValidator ID="cvOverseasFlightCost" runat="server"
                            ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasFlightCost"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>


            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Overseas Hotel Cost
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtOverseasHotelCost" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
                        ID="cvOverseasHotelCost" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasHotelCost"
                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>


            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Overseas Transport Cost
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtOverseasTransportCost" runat="server" Columns="7" MaxLength="7"
                        CssClass="form-control" /><asp:CompareValidator ID="cvOverseasTransportCost" runat="server"
                            ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasTransportCost"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>



            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Overseas Meal Cost
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtOverseasMealCost" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
                        ID="cvLOverseasMealCost" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasMealCost"
                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>


            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Overseas Others
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtOverseasOthers" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
                        ID="cvOverseasOthers" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasOthers"
                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>


            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Overseas SDF
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtOverseasSDF" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
                        ID="cvOverseasSDF" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasSDF"
                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>


            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Other Cost
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtOtherCost" runat="server" Columns="7" MaxLength="7" CssClass="form-control" /><asp:CompareValidator
                        ID="cvOtherCost" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOtherCost"
                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                </div>
            </div>


            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Remarks
                </label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3"
                        Columns="40" MaxLength="200" CssClass="form-control" />
                </div>
            </div>
            
             <div class="form-group">
                <label class="col-sm-3 control-label">
                   
                </label>
                <div class="col-sm-9">
                    <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="btn btn-primary"
                    ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>

                    <asp:Button ID="btnDuplicate" Text="Duplicate" EnableViewState="False" runat="server" CssClass="btn btn-default"
                    OnClick="btnDuplicate_Click" Visible="false"></asp:Button>

                    <asp:Button ID="btnAttendee" Text="Attendees" EnableViewState="False" runat="server" CssClass="btn btn-default"
                    OnClick="btnAttendee_Click" ></asp:Button>
                </div>
            </div>

        </div>
    </div>
    <div class="panel-footer">
        <asp:HyperLink ID="lnkCourseRegistration" runat="server" CssClass="text-info"></asp:HyperLink>
    </div> 
</div> 


<div class="TABCOMMAND">
<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
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