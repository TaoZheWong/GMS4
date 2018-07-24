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
    public class EmployeeCourseActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public EmployeeCourseActivity()
        {
        }
        #endregion

        #region RetrieveEmployeeCourseByRowID
        public EmployeeCourse RetrieveEmployeeCourseByRowID(int rowID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("EmployeeCourse.RowID"),
                                helper.CleanValue(rowID));

            return EmployeeCourse.RetrieveFirst(stb.ToString());
        }

        public EmployeeCourse RetrieveEmployeeCourseByCourseSessionIDEmployeeID(int CourseSessionID, short EmployeeID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("EmployeeCourse.CourseSessionID"),
                                helper.CleanValue(CourseSessionID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("EmployeeCourse.EmployeeID"),
                                helper.CleanValue(EmployeeID));     

            return EmployeeCourse.RetrieveFirst(stb.ToString());
        }


        #endregion 

        #region UpdateEmployeeCourse
        /// <summary>
        /// Function to update course taken by an employee
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a EmployeeCourse object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateEmployeeCourse(ref EmployeeCourse eCourseToUpdate, LogSession session)
        {
            if (eCourseToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!eCourseToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            eCourseToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateEmployeeCourse
        /// <summary>
        /// Function to create a new Course taken by an Employee
        /// </summary>
        /// <param name="organizerToCreate">Reference to a EmployeeCourse object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateEmployeeCourse(ref EmployeeCourse eCourseToCreate, LogSession session)
        {
            if (eCourseToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!eCourseToCreate.IsValid())
                return ResultType.MainDataNotValid;

            eCourseToCreate.Save();

            return ResultType.Ok;

            //#region Transaction
            //TransactionManager trans = null;
            //try
            //{
            //    trans = new TransactionManager();

            //    trans.Save(parameterToCreate);

            //    trans.Commit();
            //    parameterToCreate.Resync();
            //    return ResultType.Ok;
            //}
            //catch (Exception)
            //{
            //    if (trans != null)
            //        trans.Rollback();
            //    throw;
            //}
            //finally
            //{
            //    if (trans != null)
            //        trans.Dispose();
            //}
            //#endregion //Transaction

        }
        #endregion

        #region DeleteEmployeeCourse
        public ResultType DeleteEmployeeCourse(int rowID, LogSession session)
        {
            //if (rowID == null)
            //    return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            EmployeeCourse eCourse = RetrieveEmployeeCourseByRowID(rowID);
            if (eCourse == null)
                return ResultType.Error;

            eCourse.Delete();
            return ResultType.Ok;
        }
        #endregion
    }
}
