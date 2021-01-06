<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CourseRegistration.aspx.cs"
    Inherits="GMSWeb.SysHR.Training.CourseRegistration" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Course Registration Page</title>
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport" />
    <meta name="theme-color" content="#007ee5" />

    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-timepicker/bootstrap-timepicker.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite_app.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <style>
        body {
            overflow: auto;
        }
    </style>

    <script language="javascript" type="text/javascript">
    function changeEmployeeID (txt)
    {
        var ddl = document.getElementById("ddlEmployee");
        var str = txt.value.split(' - ');
        txt.value = str[0].replace(/^\s+|\s+$/g, '').toUpperCase();
        var found = false;
        for( var i=0; i<ddl.options.length; i++)
        {
            if (ddl.options[i].text.toUpperCase() == txt.value.toUpperCase())
            {
                ddl.options[i].selected = true;
                found = true;
            }
        }
        if (found == false)
        {
            alert('Please key in the correct name!');
            ddl.options[0].selected = true;
        }
    }
    
    function jsToggleLayer1(e) {
		if(e){ 
			var a = document.getElementById("trObjectiveNotAchievedRemark1");
			var b = document.getElementById("trObjectiveNotAchievedRemark2");
			
			if( a )
				a.style.display = (e.id=="rbIsObjectiveAchievedAfterCourse" )? "none":"";
			if( b )
				b.style.display = (e.id=="rbIsObjectiveAchievedAfterCourse" )? "none":"";
		}
	}
	function jsToggleLayer2(e) {
		if(e){ 
		    var a = document.getElementById("trActionPlanNotCompletedRemark1");
			var b = document.getElementById("trActionPlanNotCompletedRemark2");
			
			if( a )
				a.style.display = (e.id=="rbIsActionPlanCompletedAfterCourse" )? "none":"";	
			if( b )
				b.style.display = (e.id=="rbIsActionPlanCompletedAfterCourse" )? "none":"";
				
		}
	}
    </script>

</head>
<body  >
    <form id="form1" runat="server">
         <div class="container">
            <a name="TemplateInfo"></a>
             
             <ul class="breadcrumb">
                <li class="active" runat="server" id="PageHeader">
                    Training &gt; Course Registration</li>
             </ul>

            <asp:ScriptManager ID="sriptmgr1" runat="server">
                <Services>
                    <asp:ServiceReference Path="AutoCompleteEmployeeName.asmx" />
                </Services>
            </asp:ScriptManager>

            <div runat="server" id="divStaffDetails" visible="false" class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        Staff Details
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Employee No.
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span><asp:Label runat="server" ID="lblEmployeeNo"></asp:Label>
                                </label>
                            </div>
                        </div>
                         <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Name
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span><asp:Label runat="server" ID="lblEmployeeName"></asp:Label>
                                </label>
                            </div>
                        </div>
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Department
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span><asp:Label runat="server" ID="lblDepartment"></asp:Label>
                                </label>
                            </div>
                        </div>   
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Designation
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span><asp:Label runat="server" ID="lblDesignation"></asp:Label>
                                </label>
                            </div>
                        </div>
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Supervisor
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span><asp:Label runat="server" ID="lblSupervisor"></asp:Label>
                                </label>
                            </div>
                        </div>  
                    </div>
                </div>
            </div>
             
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        Course Details
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Course Code
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span> 
                                    <asp:Label runat="server" ID="lblCourseCode"></asp:Label>
                                    <input type="hidden" id="hidCourseSessionID" runat="server" />
                                    <input type="hidden" id="hidRandomID" runat="server" />
                                    <input type="hidden" id="hidEmployeeCourseID" runat="server" />
                                </label>
                            </div>
                        </div>
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Course Title
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span> 
                                    <asp:Label runat="server" ID="lblCourseTitle"></asp:Label>
                                </label>
                            </div>
                        </div>
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Organizer
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span> 
                                    <asp:Label runat="server" ID="lblCourseOrganizer"></asp:Label>
                                </label>
                            </div>
                        </div>
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Type
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span> 
                                    <asp:Label runat="server" ID="lblCourseType"></asp:Label>
                                </label>
                            </div>
                        </div> 
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Date and Time
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span> 
                                    <asp:Label runat="server" ID="lblDateAndTime"></asp:Label>
                                </label>
                            </div>
                        </div>   
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Conducting Language
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span> 
                                    <asp:Label runat="server" ID="lblCourseLanguage"></asp:Label>
                                </label>
                            </div>
                        </div>    
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Venue
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span> 
                                    <asp:Label runat="server" ID="lblVenue"></asp:Label>
                                </label>
                            </div>
                        </div>   
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Facilitator
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span> 
                                    <asp:Label runat="server" ID="lblFacilitator"></asp:Label>
                                </label>
                            </div>
                        </div>    
                    </div>
                </div>
             </div>

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        Course Description
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Target Audience
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span><asp:Label runat="server" ID="lblTargetAudience"></asp:Label>
                                </label>
                            </div>
                        </div>
                       <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Course Objective
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label text-left ">
                                    <span class="hidden-xs"> : </span><asp:Label runat="server" ID="lblCourseObjective"></asp:Label>
                                </label>
                            </div>
                        </div>
                       <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Course Description
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label text-left">
                                    <span class="hidden-xs"> : </span><asp:Label runat="server" ID="lblCourseDescription"></asp:Label>
                                </label>
                            </div>
                        </div>
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Pre-requisite
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label text-left">
                                    <span class="hidden-xs"> : </span><asp:Label runat="server" ID="lblPrerequisite"></asp:Label>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        Training Cost
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20 row">
                        <div class="col-md-6 col-sm-6 col-lg-6">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Local Course Fee
                                </label>
                                <div class="col-sm-6">
                                    <label class="control-label ">
                                        <span class="hidden-xs"> : </span>
                                        <asp:Label runat="server" ID="lblLocalCourseFee"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Local Registration Fee
                                </label>
                                <div class="col-sm-6">
                                    <label class="control-label ">
                                        <span class="hidden-xs"> : </span>
                                        <asp:Label runat="server" ID="lblLocalRegistrationFee"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Local Examination Fee
                                </label>
                                <div class="col-sm-6">
                                    <label class="control-label ">
                                        <span class="hidden-xs"> : </span>
                                        <asp:Label runat="server" ID="lblLocalExaminationFee"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Local Membership Fee
                                </label>
                                <div class="col-sm-6">
                                    <label class="control-label ">
                                        <span class="hidden-xs"> : </span>
                                        <asp:Label runat="server" ID="lblLocalMembershipFee"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Local GST
                                </label>
                                <div class="col-sm-6">
                                    <label class="control-label ">
                                        <span class="hidden-xs"> : </span>
                                        <asp:Label runat="server" ID="lblLocalGST"></asp:Label>
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-lg-6">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Overseas Flight Cost
                                </label>
                                <div class="col-sm-6">
                                    <label class="control-label ">
                                        <span class="hidden-xs"> : </span>
                                        <asp:Label runat="server" ID="lblOverseasFlightCost"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Overseas Hotel Cost
                                </label>
                                <div class="col-sm-6">
                                    <label class="control-label ">
                                        <span class="hidden-xs"> : </span>
                                        <asp:Label runat="server" ID="lblOverseasHotelCost"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Overseas Transport Cost
                                </label>
                                <div class="col-sm-6">
                                    <label class="control-label ">
                                        <span class="hidden-xs"> : </span>
                                        <asp:Label runat="server" ID="lblOverseasTransportCost"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Overseas Meal Cost
                                </label>
                                <div class="col-sm-6">
                                    <label class="control-label ">
                                        <span class="hidden-xs"> : </span>
                                        <asp:Label runat="server" ID="lblOverseasMealCost"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Overseas Others
                                </label>
                                <div class="col-sm-6">
                                    <label class="control-label ">
                                        <span class="hidden-xs"> : </span>
                                        <asp:Label runat="server" ID="lblOverseasOthers"></asp:Label>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <br />
            <h3>Registration Details</h3>
           
            <div runat="server" id="preJustification" class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        (You need to fill in all the fields below.)
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20 row">
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Learning Objectives
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtLearningObjectives" runat="server" TextMode="MultiLine" Columns="100"
                                Rows="2" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Action Plan Upon Completion of the Course
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtActionPlan" runat="server" Columns="100" MaxLength="5" TextMode="MultiLine"
                                CssClass="form-control" />
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Action Plan Due Date
                                </label>
                                <div class="col-sm-8">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtActionPlanDueDate" runat="server" MaxLength="10" Columns="10"
                                            CssClass="form-control" />
                                        <span class="input-group-addon">
                                            <i class="ti-calendar"></i>
                                        </span>
                                    </div>
                                     <asp:CompareValidator ID="cvActionPlanDueDate" runat="server" ErrorMessage="Invalid Date"
                                        ControlToValidate="txtActionPlanDueDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow"
                                        Operator="DataTypeCheck"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Purpose of Action Plan
                                </label>
                                <div class="col-sm-8">
                                     <asp:TextBox ID="txtPurposeOfActionPlan" runat="server" Columns="100" MaxLength="100"
                                CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-heading">
                    <h4 class="panel-title">
                        Key Performance Indicators (KPI)
                    </h4>
                </div>
                <div class="panel-body">
                     <div class="form-horizontal m-t-20 row">
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Type of Indicator
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtTypeOfIndicator" runat="server" Columns="20" MaxLength="50" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Current %
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtCurrentValue" runat="server" Columns="10" MaxLength="10" CssClass="form-control" />
                                    <asp:CompareValidator ID="cvCurrentValue" runat="server" ErrorMessage="*" Display="Dynamic"
                                        ControlToValidate="txtCurrentValue" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Expected %
                                </label>
                                <div class="col-sm-8">
                                     <asp:TextBox ID="txtExpectedValue" runat="server" Columns="10" MaxLength="10" CssClass="form-control" />
                                    <asp:CompareValidator ID="cvExpectedValue" runat="server" ErrorMessage="*" Display="Dynamic"
                                        ControlToValidate="txtExpectedValue" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                </div>
                            </div>
                        </div>
                </div>
                </div>
            </div>
            <div runat="server" id="fsAfterCourseEvaluation" class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">After Course Evaluation</h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20 row">
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Is the objective achieved? (Please refer to above on the following questions.)
                                </label>
                                <div class="col-sm-6">
                                    <div class="radio">
                                        <asp:RadioButton ID="rbIsObjectiveAchievedAfterCourse" runat="server" Text="Yes"
                                            Checked="true" GroupName="Achieved" onclick="jsToggleLayer1(this)" />
                                        <asp:RadioButton ID="rbIsObjectiveNotAchievedAfterCourse" runat="server" Text="No"
                                            GroupName="Achieved" onclick="jsToggleLayer1(this)" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left" runat="server" id="trObjectiveNotAchievedRemark1" style="display: none">
                                    Remarks
                                </label>
                                <div class="col-sm-6" runat="server" id="trObjectiveNotAchievedRemark2" style="display: none">
                                    <asp:TextBox ID="txtObjectiveNotAchievedRemark" TextMode="MultiLine" runat="server"
                                CssClass="form-control" Columns="100" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left">
                                    Is the action plan completed
                                </label>
                                <div class="col-sm-6">
                                    <div class="radio">
                                        <asp:RadioButton ID="rbIsActionPlanCompletedAfterCourse" runat="server" Text="Yes"
                                            Checked="true" GroupName="Completed" onclick="jsToggleLayer2(this)" />
                                        <asp:RadioButton ID="rbIsActionPlanNotCompletedAfterCourse" runat="server" Text="No"
                                            GroupName="Completed" onclick="jsToggleLayer2(this)" />
                                    </div>
                                </div>
                            </div>
                        </div>
                       <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left" runat="server" id="trActionPlanNotCompletedRemark1" style="display: none">
                                    Remarks
                                </label>
                                <div class="col-sm-6" runat="server" id="trActionPlanNotCompletedRemark2" style="display: none">
                                    <asp:TextBox ID="txtActionPlanNotCompletedRemark" TextMode="MultiLine" runat="server"
                                        CssClass="form-control" Columns="100" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left" >
                                    Action Plan Extended Due Date
                                </label>
                                <div class="col-sm-6">
                                    <div class="input-group date">
                                        <asp:TextBox ID="txtActionPlanExtendedDueDate" runat="server" MaxLength="10" Columns="10"
                                             CssClass="form-control" />
                                        <span class="input-group-addon">
                                            <i class="ti-calendar"></i>
                                        </span>
                                    </div>
                                    <asp:CompareValidator ID="cvActionPlanExtendedDueDate" runat="server" ErrorMessage="Invalid Date"
                                        ControlToValidate="txtActionPlanExtendedDueDate" Type="Date" Display="Dynamic"
                                        ValidationGroup="valGrpAfterRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-6 control-label text-left" >
                                  KPI Actual %
                                </label>
                                <div class="col-sm-6">
                                   <asp:TextBox ID="txtActualValue" runat="server" Columns="10" MaxLength="10" CssClass="form-control" />
                                    <asp:CompareValidator ID="cvActualValue" runat="server" ErrorMessage="*" Display="Dynamic"
                                        ControlToValidate="txtActualValue" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpAfterRow" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer" runat="server" id="trUpdatePTEF">
                    <asp:Button ID="btnUpdatePTEF" Text="Update" EnableViewState="False" runat="server"
                                CssClass="btn btn-primary" ValidationGroup="valGrpAfterRow" OnClick="UpdatePTEF"></asp:Button>
                </div>
            </div>
            <div runat="server" id="divRegister" class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">Staff Name</h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20 row">
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <div class="col-sm-12 control-label text-left">
                                    <asp:TextBox ID="txtEmployeeName" runat="server" Columns="40" MaxLength="40" autocomplete="off"
                                        onfocus="select();" onChange="changeEmployeeID(this)" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvEmployeeName" runat="server" ControlToValidate="txtEmployeeName"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                    <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2" ID="autoComplete2"
                                        TargetControlID="txtEmployeeName" ServicePath="AutoCompleteEmployeeName.asmx"
                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                                    </ajaxToolkit:AutoCompleteExtender>
                                    <div style="display: none">
                                        <asp:DropDownList ID="ddlEmployee" runat="Server" DataTextField="Name" DataValueField="EmployeeID" /></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:Button ID="btnSubmit" Text="Register Now" EnableViewState="False" runat="server"
                        CssClass="btn btn-primary" ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>
                    <asp:Button ID="btnAdd" Text="Add & Approve" EnableViewState="False" runat="server"
                        CssClass="btn btn-default" ValidationGroup="valGrpNewRow" Visible="false" OnClick="btnAdd_Click"></asp:Button>
                    <asp:Button ID="btnAddSendTEF" Text="Add & Approve & Send TEF" EnableViewState="False" runat="server"
                        CssClass="btn btn-default" ValidationGroup="valGrpNewRow" Visible="false" OnClick="btnAddSendTEF_Click">
                    </asp:Button>
                </div>
            </div>
            <div runat="server" id="divStatus" class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">Status</h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20 row">
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-12 control-label text-left">
                                    <asp:Label runat="server" ID="lblEmployeeCourseStatus"></asp:Label>
                                </label>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-12 control-label text-left" runat="server" id="tdStatus">
                                    <asp:Label runat="server" ID="lblEmployeeCourseApprovalStatus" Style="display: block">Approval Status : </asp:Label>
                                </label>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-12 control-label text-left">
                                    <asp:Label runat="server" ID="lblPending">Item(s) Pending :</asp:Label>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer" runat="server" id="tdPending">
                    <asp:LinkButton runat="server" ID="btnResendPRETR" OnClick="ReSend" Visible="false" CssClass="btn btn-default">Re-send Course Registration Confirmation</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnResendTR" OnClick="ReSend" Visible="false" CssClass="btn btn-default">Re-send Course Registration Approval</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnResendTEF" OnClick="ReSend" Visible="false" CssClass="btn btn-default">Re-send Training Evaluation Form</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnResendPTEF" OnClick="ReSend" Visible="false" CssClass="btn btn-default">Re-send Post Training Evaluation Form</asp:LinkButton>
                    <asp:Button ID="btnChangeApprovalStatus" Text="Change Approval Status" EnableViewState="False" CssClass="btn btn-primary"
                        runat="server" OnClick="ChangeApprovalStatus" OnClientClick="return confirm('Are you sure you want to change the approval status?');">
                    </asp:Button>
                </div>
            </div>
            <div runat="server" id="divApproval" class="panel panel-primary">
                <div class="panel panel-footer">
                    <asp:Button ID="btnApprove" Text="Approve" EnableViewState="False" runat="server"
                        CssClass="btn btn-primary" OnClick="ApproveRegistration"></asp:Button>
                    <asp:Button ID="btnReject" Text="Reject" EnableViewState="False" runat="server" CssClass=" btn btn-default"
                        OnClick="RejectRegistration"></asp:Button>
                </div>
            </div>
            <div runat="server" id="divAdminFunctions" class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">For Admin Input</h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20 row">
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Is Bonded?
                                </label>
                                <div class="col-sm-8">
                                    <div class="checkbox">
                                        <asp:CheckBox ID="chkIsBonded" runat="server" Checked="false" Text="Yes"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Number of Months Bonded
                                </label>
                                <div class="col-sm-8">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtNoOfMonthsBonded" runat="server" Columns="3" MaxLength="3" CssClass="form-control" />
                                        <span class="input-group-addon">
                                            Month(s)
                                        </span>
                                    </div>
                                    <asp:CompareValidator ID="cvNoOfMonthsBonded" runat="server" ErrorMessage="*"
                                        Display="Dynamic" ControlToValidate="txtNoOfMonthsBonded" Type="Integer" Operator="DataTypeCheck"
                                        ValidationGroup="valGrpNewRow" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Bond Contract Location
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtBondContractLocation" runat="server" Columns="50" MaxLength="50"
                                        CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Certificate Location
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtCertificateLocation" runat="server" Columns="50" MaxLength="50"
                                        CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        
                        
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Bond Expired Date
                                </label>
                                <div class="col-sm-8">
                                    <div class="input-group date">
                                        <asp:TextBox ID="txtBondExpiredDate" runat="server" MaxLength="10" Columns="10" CssClass="form-control" />
                                        <span class="input-group-addon">
                                            <i class="ti-calendar"></i>
                                        </span>
                                    </div>
                                    <asp:CompareValidator ID="cvBondExpiredDate" runat="server" ErrorMessage="Invalid Date"
                                        ControlToValidate="txtBondExpiredDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpAdminRow"
                                        Operator="DataTypeCheck"></asp:CompareValidator>
                                </div>
                            </div>
                        </div>
                        
                       
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Licence Expired Date
                                </label>
                                <div class="col-sm-8">
                                    <div class="input-group date">
                                        <asp:TextBox ID="txtLicenceExpiredDate" runat="server" MaxLength="10" Columns="10" CssClass="form-control" />
                                        <span class="input-group-addon">
                                            <i class="ti-calendar"></i>
                                        </span>
                                    </div>
                                    <asp:CompareValidator ID="cvLicenceExpiredDate" runat="server" ErrorMessage="Invalid Date"
                                        ControlToValidate="txtLicenceExpiredDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpAdminRow"
                                        Operator="DataTypeCheck"></asp:CompareValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Payment Location
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtPaymentLocation" runat="server" Columns="50" MaxLength="50" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    SDF
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtSDF" runat="server" Columns="7" MaxLength="7" CssClass="form-control" />
                                    <asp:CompareValidator
                                        ID="cvSDF" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtSDF"
                                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                </div>
                            </div>
                        </div>
                       <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    SRP (Absentee Payroll)
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtSRP" runat="server" Columns="7" MaxLength="7" CssClass="form-control" />
                                    <asp:CompareValidator
                                        ID="cvSRP" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtSRP"
                                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                </div>
                            </div>
                        </div>
                       <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    SDF Application Date
                                </label>
                                <div class="col-sm-8">
                                    <div class="input-group date">
                                        <asp:TextBox ID="txtSDFApplicationDate" runat="server" MaxLength="10" Columns="10" CssClass="form-control" />
                                        <span class="input-group-addon">
                                            <i class="ti-calendar"></i>
                                        </span>
                                    </div>
                                     <asp:CompareValidator ID="cvSDFApplicationDate" runat="server" ErrorMessage="Invalid Date"
                                        ControlToValidate="txtSDFApplicationDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpAdminRow"
                                        Operator="DataTypeCheck"></asp:CompareValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    SDF Application No.
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtSDFApplicationNo" runat="server" Columns="15" MaxLength="15"
                                        CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    SDF Disbursement Email Location
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtSDFDisbursementEmailLocation" runat="server" Columns="50" MaxLength="50"
                            CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                         <div class="col-md-12 col-sm-12 col-lg-12">
                            <div class="form-group col-lg-12">
                                <label class="col-sm-4 control-label text-left">
                                    Remarks
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" CssClass="form-control"
                            Columns="50" />
                                </div>
                            </div>
                        </div>   
                    </div>
                </div>
                
                <div class="panel-heading">
                    <h4 class="panel-title">Admin Functions</h4>
                </div>
                <div class="panel-footer">
                    <asp:Button ID="btnApproveOnBehalf" Text="Approve On Behalf" EnableViewState="False"
                        runat="server" CssClass="btn btn-primary" OnClick="ApproveOnBehalf" OnClientClick="return confirm('Are you sure you want to approve on behalf of the relevant party?');"></asp:Button>
                    <asp:Button ID="btnRejectOnBehalf" Text="Reject On Behalf" EnableViewState="False"
                        runat="server" CssClass="btn-danger" OnClick="RejectOnBehalf" OnClientClick="return confirm('Are you sure you want to reject on behalf of the relevant party?');"></asp:Button>
                    <asp:Button ID="btnUpdate" Text="Update" EnableViewState="False" runat="server" CssClass="btn btn-primary"
                        OnClick="btnUpdate_Click" ValidationGroup="valGrpAdminRow"></asp:Button>
                </div>
            </div>
            
            <div class="TABCOMMAND"></div>
        </div><br />
    </form>

    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/jquery/jquery-1.9.1.min.js"></script> 
    <script type="text/javascript" src="<%= Request.ApplicationPath %>/new_assets/plugins/jquery/jquery-migrate-1.1.0.min.js"></script>
    <script src="<%= Request.ApplicationPath %>/new_assets/plugins/bootstrap/dist/js/bootstrap.min.js"></script>
    <script src="<%= Request.ApplicationPath %>/new_assets/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>
    <script>
         $(document).ready(function () {
             $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
         });

    </script>
</body>
</html>
