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
    public class SalesPersonMasterActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public SalesPersonMasterActivity()
        {
        }
        #endregion

        #region RetrieveSalesPersonMasterByCoyIDSalesPersonMasterName
        public SalesPersonMaster RetrieveSalesPersonMasterByCoyIDSalesPersonMasterName(int coyId, string salesPersonMasterName)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesPersonMaster.CoyID"),
                                helper.CleanValue(coyId));
            stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("SalesPersonMaster.SalesPersonMasterName"),
                                helper.CleanValue(salesPersonMasterName));

            return SalesPersonMaster.RetrieveFirst(stb.ToString());
        }

        public SalesPersonMaster RetrieveSalesPersonMasterBySalesPersonMasterID(short salesPersonMasterID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat("{0} = {1} ", helper.GetFieldName("SalesPersonMaster.SalesPersonMasterID"),
                                helper.CleanValue(salesPersonMasterID));

            return SalesPersonMaster.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region UpdateSalesPersonMaster
        /// <summary>
        /// Function to update SalesPersonMaster
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a SalesPersonMaster object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateSalesPersonMaster(ref SalesPersonMaster sMasterToUpdate, LogSession session)
        {
            if (sMasterToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!sMasterToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            sMasterToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateSalesPersonMaster
        /// <summary>
        /// Function to create a new SalesPersonMaster
        /// </summary>
        /// <param name="organizerToCreate">Reference to a SalesPersonMaster object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateSalesPersonMaster(ref SalesPersonMaster sMasterToCreate, LogSession session)
        {
            if (sMasterToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!sMasterToCreate.IsValid())
                return ResultType.MainDataNotValid;

            sMasterToCreate.Save();

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

        #region DeleteSalesPersonMaster
        public ResultType DeleteSalesPersonMaster(short salesPersonMasterID, LogSession session)
        {
            if (session == null)
                throw new NullSessionException();

            SalesPersonMaster sMaster = RetrieveSalesPersonMasterBySalesPersonMasterID(salesPersonMasterID);
            if (sMaster == null)
                return ResultType.Error;

            sMaster.Delete();
            return ResultType.Ok;
        }
        #endregion
    }
}
