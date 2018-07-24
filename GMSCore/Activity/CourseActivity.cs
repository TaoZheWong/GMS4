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
    public class CourseActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public CourseActivity()
        {
        }
        #endregion

        #region RetrieveCourseByCourseCode
        public Course RetrieveCourseByCourseCode(string courseCode)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Course.CourseCode"),
                                helper.CleanValue(courseCode));

            return Course.RetrieveFirst(stb.ToString());
        }

        public Course RetrieveCourseByCourseTitleByOrganizerID(string courseTitle, short organizerId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Course.CourseTitle"),
                                helper.CleanValue(courseTitle));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Course.OrganizerID"),
                                helper.CleanValue(organizerId));

            return Course.RetrieveFirst(stb.ToString());
        }

        public Course RetrieveCourseByCourseTitle(string courseTitle)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Course.CourseTitle"),
                                helper.CleanValue(courseTitle));

            return Course.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region UpdateCourse
        /// <summary>
        /// Function to update Course
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a Course object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateCourse(ref Course courseToUpdate, LogSession session)
        {
            if (courseToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!courseToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            courseToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateCourse
        /// <summary>
        /// Function to create a new Course
        /// </summary>
        /// <param name="courseToCreate">Reference to a Course object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateCourse(ref Course courseToCreate, LogSession session)
        {
            if (courseToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!courseToCreate.IsValid())
                return ResultType.MainDataNotValid;

            courseToCreate.Save();

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

        #region DeleteCourse
        public ResultType DeleteCourse(String courseCode, LogSession session)
        {
            if (courseCode == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            Course course = RetrieveCourseByCourseCode(courseCode);
            if (course == null)
                return ResultType.Error;

            course.Delete();
            return ResultType.Ok;
        }
        #endregion
    }
}
