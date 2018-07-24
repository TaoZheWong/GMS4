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
    public class SalesPersonMappingActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public SalesPersonMappingActivity()
        {
        }
        #endregion

        #region RetrieveSalesPersonMappingByCoyIDSalesPersonMasterNameSalesPersonID
        public SalesPersonMapping RetrieveSalesPersonMappingByCoyIDSalesPersonMasterNameSalesPersonID(short coyId, string salesPersonMasterName, string salesPersonID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesPersonMapping.CoyID"),
                                helper.CleanValue(coyId));
            stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("SalesPersonMapping.SalesPersonMasterName"),
                                helper.CleanValue(salesPersonMasterName));
            stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("SalesPersonMapping.SalesPersonID"),
                                helper.CleanValue(salesPersonMasterName));

            return SalesPersonMapping.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region DeleteSalesPersonMappingByCoyIDSalesPersonMasterName
        public ResultType DeleteSalesPersonMappingBySalesPersonMasterID(short salesPersonMasterID, LogSession session)
        {
            if (session == null)
                throw new NullSessionException();

            IList<SalesPersonMapping> lstMapping = new SystemDataActivity().RetrieveAllSalesPersonMappingListBySalesPersonMasterIDSortBySalesPersonID(salesPersonMasterID);
            foreach (SalesPersonMapping sMapping in lstMapping)
            {
                sMapping.Delete();
            }
            return ResultType.Ok;
        }
        #endregion

        #region UpdateSalesPersonMapping
        /// <summary>
        /// Function to update SalesPersonMapping
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a SalesPersonMapping object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateSalesPersonMapping(ref SalesPersonMapping sMappingToUpdate, LogSession session)
        {
            if (sMappingToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!sMappingToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            sMappingToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateSalesPersonMapping
        /// <summary>
        /// Function to create a new SalesPersonMapping
        /// </summary>
        /// <param name="organizerToCreate">Reference to a SalesPersonMapping object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateSalesPersonMapping(ref SalesPersonMapping sMappingToCreate, LogSession session)
        {
            if (sMappingToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!sMappingToCreate.IsValid())
                return ResultType.MainDataNotValid;

            sMappingToCreate.Save();

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

        #region DeleteSalesPersonMapping
        public ResultType DeleteSalesPersonMapping(short coyId,string salesPersonMasterName,string salesPersonID, LogSession session)
        {
            if (coyId == null)
                return ResultType.NullMainData;

            if (salesPersonMasterName == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            SalesPersonMapping sMapping = RetrieveSalesPersonMappingByCoyIDSalesPersonMasterNameSalesPersonID(coyId, salesPersonMasterName, salesPersonID);
            if (sMapping == null)
                return ResultType.Error;

            sMapping.Delete();
            return ResultType.Ok;
        }
        #endregion
    }
}
