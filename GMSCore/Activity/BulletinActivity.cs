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
    public class BulletinActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public BulletinActivity()
        {
        }
        #endregion

        #region UpdateBulletin
        /// <summary>
        /// Function to update bulletin message
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a Bulletin object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateBulletin(ref Bulletin bulletinToUpdate, LogSession session)
        {
            if (bulletinToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!bulletinToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            bulletinToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateBulletin
        /// <summary>
        /// Function to create a new Course
        /// </summary>
        /// <param name="courseToCreate">Reference to a Bulletin object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateBulletin(ref Bulletin bulletinToCreate, LogSession session)
        {
            if (bulletinToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!bulletinToCreate.IsValid())
                return ResultType.MainDataNotValid;

            bulletinToCreate.Save();

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

        #region DeleteBulletin
        public ResultType DeleteBulletin(short messageId, LogSession session)
        {
            if (messageId == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            Bulletin bulletin = Bulletin.RetrieveByKey(messageId);
            if (bulletin == null)
                return ResultType.Error;

            bulletin.Delete();
            return ResultType.Ok;
        }
        #endregion
    }
}
