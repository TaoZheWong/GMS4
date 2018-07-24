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
    public class CourseOrganizerActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public CourseOrganizerActivity()
        {
        }
        #endregion

        #region RetrieveOrganizerByOrganizerName
        public CourseOrganizer RetrieveOrganizerByOrganizerName(string Name)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("CourseOrganizer.OrganizerName"),
                                helper.CleanValue(Name));

            return CourseOrganizer.RetrieveFirst(stb.ToString());
        }

        public CourseOrganizer RetrieveOrganizerByOrganizerID(short organizerId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("CourseOrganizer.OrganizerID"),
                                helper.CleanValue(organizerId));

            return CourseOrganizer.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region UpdateOrganizer
        /// <summary>
        /// Function to update Course Organizer
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a CompanySpecialData object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateOrganizer(ref CourseOrganizer organizerToUpdate, LogSession session)
        {
            if (organizerToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!organizerToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            organizerToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateOrganizer
        /// <summary>
        /// Function to create a new Course Organizer
        /// </summary>
        /// <param name="organizerToCreate">Reference to a CourseOrganizer object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateOrganizer(ref CourseOrganizer organizerToCreate, LogSession session)
        {
            if (organizerToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!organizerToCreate.IsValid())
                return ResultType.MainDataNotValid;

            organizerToCreate.Save();

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

        #region DeleteOrganizer
        public ResultType DeleteOrganizer(short Id, LogSession session)
        {
            if (session == null)
                throw new NullSessionException();

            CourseOrganizer organizer = RetrieveOrganizerByOrganizerID(Id);
            if (organizer == null)
                return ResultType.Error;

            organizer.Delete();
            return ResultType.Ok;
        }
        #endregion
    }
}
