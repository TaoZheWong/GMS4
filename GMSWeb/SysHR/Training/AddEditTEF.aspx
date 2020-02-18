<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old AddEditTEF.aspx.cs" Inherits="GMSWeb.SysHR.Training.AddEditTEF" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <title>Training - Add/Edit Training Evaluation Form</title>
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-timepicker/bootstrap-timepicker.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite.css" rel="stylesheet" />
    <link href="../../new_assets/css/layout.css" rel="stylesheet" />
    <link href="../../new_assets/css/component.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <style>
        body {
            overflow: auto;
        }

         .table>tbody>tr>td {
             border-color: #fff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <div class="container">
             <ul class="breadcrumb pull-right">
                <li class="active">Training</li>
                <li class="active">Add/Edit Training Evaluation Form</li>
             </ul>
            <h1 class="page-header">Training Evaluation Form <br />
                <small>Add or edit a Training Evaluation Form.</small></h1>
            <div class="clearfix"></div> 
            <asp:ScriptManager ID="sriptmgr1" runat="server">
            </asp:ScriptManager>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        SECTION A: ADMINISTRATIVE DATA
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Employee's Name
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span>
                                    <input type="hidden" id="hidRandomID" runat="server" />
                                    <input type="hidden" id="hidEmployeeCourseID" runat="server" />
                                    <asp:Label runat="server" ID="lblEmployeeName"></asp:Label>
                                </label>
                            </div>
                        </div>
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Employee Number
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span>
                                    <asp:Label runat="server" ID="lblEmployeeNumber"></asp:Label>
                                </label>
                            </div>
                        </div>
                       <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Superior's Name
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span>
                                    <asp:Label runat="server" ID="lblSuperiorName"></asp:Label>
                                </label>
                            </div>
                        </div>
                      <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Department
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span>
                                    <asp:Label runat="server" ID="lblDepartment"></asp:Label>
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
                                Course Type
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
                                Course Provider
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span>
                                    <asp:Label runat="server" ID="lblOrganizerName"></asp:Label>
                                </label>
                            </div>
                        </div>
                       <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Course Date
                            </label>
                            <div class="col-sm-8">
                                <label class="control-label ">
                                    <span class="hidden-xs"> : </span>
                                    <asp:Label runat="server" ID="lblCourseDate"></asp:Label>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
          
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        SECTION B: COURSE FEEDBACK
                    </h4>
                </div>
                <div class="panel-body">
                    <table class="table table-condensed m-t-20">
                        <tr >
                            <td width="30%" >
                            </td>
                            <td width="10%" align="center">
                                Strongly Disagree
                            </td>
                            <td width="10%" align="center">
                                Disagree</td>
                            <td width="10%" align="center">
                                Neutral</td>
                            <td width="10%" align="center">
                                Agree</td>
                            <td width="10%" align="center">
                                Strongly Agree</td>
                        </tr>
                        <tr>
                            <td colspan="6" style="font-weight: bolder">
                                Content
                            </td>
                        </tr>
                        <tr>
                            <td>
                                The course content is relevant to my work
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbContentRelevant1" runat="server" GroupName="ContentRelevant" />
                                    <label for="<%=rbContentRelevant1.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbContentRelevant2" runat="server" GroupName="ContentRelevant" />
                                     <label for="<%=rbContentRelevant2.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbContentRelevant3" runat="server" GroupName="ContentRelevant" />
                                     <label for="<%=rbContentRelevant3.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbContentRelevant4" runat="server" GroupName="ContentRelevant" />
                                     <label for="<%=rbContentRelevant4.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbContentRelevant5" runat="server" GroupName="ContentRelevant" />
                                     <label for="<%=rbContentRelevant5.ClientID %>"></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                The course content is well-organized
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbContentWellOrganized1" runat="server" GroupName="ContentWellOrganized" />
                                    <label for="<%=rbContentWellOrganized1.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbContentWellOrganized2" runat="server" GroupName="ContentWellOrganized" />
                                    <label for="<%=rbContentWellOrganized2.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbContentWellOrganized3" runat="server" GroupName="ContentWellOrganized" />
                                    <label for="<%=rbContentWellOrganized3.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbContentWellOrganized4" runat="server" GroupName="ContentWellOrganized" />
                                    <label for="<%=rbContentWellOrganized4.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbContentWellOrganized5" runat="server" GroupName="ContentWellOrganized" />
                                    <label for="<%=rbContentWellOrganized5.ClientID %>"></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="font-weight: bolder">
                                Quality of Teaching
                            </td>
                        </tr>
                        <tr>
                            <td>
                                The course is presented clearly and concisely
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseClear1" runat="server" GroupName="CourseClear" />
                                    <label for="<%=rbCourseClear1.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseClear2" runat="server" GroupName="CourseClear" />
                                    <label for="<%=rbCourseClear2.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseClear3" runat="server" GroupName="CourseClear" />
                                    <label for="<%=rbCourseClear3.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseClear4" runat="server" GroupName="CourseClear" />
                                    <label for="<%=rbCourseClear4.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseClear5" runat="server" GroupName="CourseClear" />
                                    <label for="<%=rbCourseClear5.ClientID %>"></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                The speaker encourages questions and participation
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbEncourageParticipation1" runat="server" GroupName="EncourageParticipation" />
                                    <label for="<%=rbEncourageParticipation1.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbEncourageParticipation2" runat="server" GroupName="EncourageParticipation" />
                                    <label for="<%=rbEncourageParticipation2.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbEncourageParticipation3" runat="server" GroupName="EncourageParticipation" />
                                    <label for="<%=rbEncourageParticipation3.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbEncourageParticipation4" runat="server" GroupName="EncourageParticipation" />
                                    <label for="<%=rbEncourageParticipation4.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbEncourageParticipation5" runat="server" GroupName="EncourageParticipation" />
                                    <label for="<%=rbEncourageParticipation5.ClientID %>"></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                The teaching method is effective
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbMethodEffective1" runat="server" GroupName="MethodEffective" />
                                    <label for="<%=rbMethodEffective1.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbMethodEffective2" runat="server" GroupName="MethodEffective" />
                                    <label for="<%=rbMethodEffective2.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbMethodEffective3" runat="server" GroupName="MethodEffective" />
                                    <label for="<%=rbMethodEffective3.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbMethodEffective4" runat="server" GroupName="MethodEffective" />
                                    <label for="<%=rbMethodEffective4.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbMethodEffective5" runat="server" GroupName="MethodEffective" />
                                    <label for="<%=rbMethodEffective5.ClientID %>"></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="font-weight: bolder">
                                Learning Objective
                            </td>
                        </tr>
                        <tr>
                            <td>
                                The course met my learning objectives
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseMeetObjects1" runat="server" GroupName="CourseMeetObjects" />
                                    <label for="<%=rbCourseMeetObjects1.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseMeetObjects2" runat="server" GroupName="CourseMeetObjects" />
                                    <label for="<%=rbCourseMeetObjects2.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseMeetObjects3" runat="server" GroupName="CourseMeetObjects" />
                                    <label for="<%=rbCourseMeetObjects3.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseMeetObjects4" runat="server" GroupName="CourseMeetObjects" />
                                    <label for="<%=rbCourseMeetObjects4.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseMeetObjects5" runat="server" GroupName="CourseMeetObjects" />
                                    <label for="<%=rbCourseMeetObjects5.ClientID %>"></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                The course met my expectation
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseMeetExpectation1" runat="server" GroupName="CourseMeetExpectation" />
                                    <label for="<%=rbCourseMeetExpectation1.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseMeetExpectation2" runat="server" GroupName="CourseMeetExpectation" />
                                    <label for="<%=rbCourseMeetExpectation2.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseMeetExpectation3" runat="server" GroupName="CourseMeetExpectation" />
                                    <label for="<%=rbCourseMeetExpectation3.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseMeetExpectation4" runat="server" GroupName="CourseMeetExpectation" />
                                    <label for="<%=rbCourseMeetExpectation4.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbCourseMeetExpectation5" runat="server" GroupName="CourseMeetExpectation" />
                                    <label for="<%=rbCourseMeetExpectation5.ClientID %>"></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="font-weight: bolder">
                                Overall Satisfaction
                            </td>
                        </tr>
                        <tr>
                            <td>
                                I am satisfied with the course
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbSatisfiedWithCourse1" runat="server" GroupName="SatisfiedWithCourse" />
                                    <label for="<%=rbSatisfiedWithCourse1.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbSatisfiedWithCourse2" runat="server" GroupName="SatisfiedWithCourse" />
                                    <label for="<%=rbSatisfiedWithCourse2.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbSatisfiedWithCourse3" runat="server" GroupName="SatisfiedWithCourse" />
                                    <label for="<%=rbSatisfiedWithCourse3.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbSatisfiedWithCourse4" runat="server" GroupName="SatisfiedWithCourse" />
                                    <label for="<%=rbSatisfiedWithCourse4.ClientID %>"></label>
                                </div>
                            </td>
                            <td align="center">
                                <div class="radio">
                                    <asp:RadioButton ID="rbSatisfiedWithCourse5" runat="server" GroupName="SatisfiedWithCourse" />
                                    <label for="<%=rbSatisfiedWithCourse5.ClientID %>"></label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
   
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        SECTION C: RECOMMENDATIONS
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                What are the areas you like best?
                            </label>
                            <div class="col-sm-8">
                               <asp:TextBox ID="txtBestArea" runat="server" Columns="100" MaxLength="100" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                What are the areas that need improvement?
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtAreaNeedImprovement" runat="server" Columns="100" MaxLength="100"
                                    CssClass="form-control" />
                            </div>
                        </div>
                         <div class="form-group col-lg-12">
                            <label class="col-sm-4 control-label text-left">
                                Other comments:
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtOtherComments" runat="server" Columns="100" MaxLength="800" 
                                    CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer clearfix">
                    <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                    ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>
                    <asp:Button ID="btnUpdate" Text="Update" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right m-r-10"
                        Visible="false" OnClick="btnUpdate_Click"></asp:Button>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
