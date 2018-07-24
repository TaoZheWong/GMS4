using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSCore.Exceptions;

using Wilson.ORMapper;

namespace GMSCore.Activity
{
    public class FormActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public FormActivity()
        {
        }
        #endregion

        #region RetrieveFormRandomIDByFormOnlineIDApplicantID
        public FormRandomID RetrieveFormRandomIDByFormOnlineIDApplicantID(string formOnlineID, string applicantID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FormRandomID.FormOnlineID"),
                                helper.CleanValue(formOnlineID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FormRandomID.ApplicantID"),
                                helper.CleanValue(applicantID));

            return FormRandomID.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region RetrieveFormRandomIDByFormOnlineIDApprovalID
        public FormRandomID RetrieveFormRandomIDByFormOnlineIDApprovalID(string formOnlineID, string approvalID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FormRandomID.FormOnlineID"),
                                helper.CleanValue(formOnlineID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FormRandomID.ApprovalID"),
                                helper.CleanValue(approvalID));

            return FormRandomID.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region RetrieveFormRandomIDByFormTypeFormIDApprovalLevel
        public FormRandomID RetrieveFormRandomIDByFormTypeFormIDApprovalLevel(string formType, int formID, byte approvalLevel)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FormRandomID.FormType"),
                                helper.CleanValue(formType));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FormRandomID.FormID"),
                                helper.CleanValue(formID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FormRandomID.ApprovalLevel"),
                                helper.CleanValue(approvalLevel));

            return FormRandomID.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region RetrieveFormRandomIDListByFormTypeFormID
        public IList<FormRandomID> RetrieveFormRandomIDListByFormTypeFormID(string formType, int formID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FormRandomID.FormType"),
                                helper.CleanValue(formType));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FormRandomID.FormID"),
                                helper.CleanValue(formID));

            return FormRandomID.RetrieveQuery(stb.ToString());
        }
        #endregion 

        #region RetrieveEmployeeCourseByTNFID
        public EmployeeCourse RetrieveEmployeeCourseByTNFID(int TNFID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("EmployeeCourse.TNFID"),
                                helper.CleanValue(TNFID));

            return EmployeeCourse.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region RetrieveFormByFormTypeFormID
        public Form RetrieveFormByFormTypeFormID(string formType, int formID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Form.FormType"),
                                helper.CleanValue(formType));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Form.FormID"),
                                helper.CleanValue(formID));

            return Form.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region RetrieveFormApprovalByFormTypeFormIDApprovalLevel
        public FormApproval RetrieveFormApprovalByFormTypeFormIDApprovalLevel(string formType, int formID, byte approvalLevel)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FormApproval.FormType"),
                                helper.CleanValue(formType));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FormApproval.FormID"),
                                helper.CleanValue(formID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FormApproval.ApprovalLevel"),
                                helper.CleanValue(approvalLevel));

            return FormApproval.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region RetrieveFormApprovalListByFormTypeFormID
        public IList<FormApproval> RetrieveFormApprovalListByFormTypeFormID(string formType, int formID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FormApproval.FormType"),
                                helper.CleanValue(formType));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FormApproval.FormID"),
                                helper.CleanValue(formID));

            return FormApproval.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("FormApproval.ApprovalLevel")));
        }
        #endregion 

        #region RetrieveEmployeeCourseByCEFID
        public EmployeeCourse RetrieveEmployeeCourseByCEFID(int CEFID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("EmployeeCourse.CEFID"),
                                helper.CleanValue(CEFID));

            return EmployeeCourse.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region RetrieveFormRandomIDByFormOnlineIDApprovalID
        public FormApproval RetrieveFormApporvalByRandomID(string randomID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FormApproval.ApprovalRandomID"),
                                helper.CleanValue(randomID));

            return FormApproval.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region RetrievePendingFormApprovalByFormTypeFormID
        public FormApproval RetrievePendingFormApprovalByFormTypeFormID(string formType, int formID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FormApproval.FormType"),
                                helper.CleanValue(formType));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FormApproval.FormID"),
                                helper.CleanValue(formID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FormApproval.ApprovalStatus"),
                                helper.CleanValue("P"));

            return FormApproval.RetrieveFirst(stb.ToString());
        }
        #endregion 
    }
}
