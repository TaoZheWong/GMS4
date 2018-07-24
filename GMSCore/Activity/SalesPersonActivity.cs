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
    public class SalesPersonActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public SalesPersonActivity()
        {
        }
        #endregion

        #region RetrieveSalesPersonByCoyIDSalesPersonID
        public SalesPerson RetrieveSalesPersonByCoyIDSalesPersonID(int coyId, string salesPersonId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesPerson.CoyID"),
                                helper.CleanValue(coyId));
            stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("SalesPerson.SalesPersonID"),
                                helper.CleanValue(salesPersonId));

            return SalesPerson.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region UpdateSalesPerson
        /// <summary>
        /// Function to update SalesPerson
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a SalesPerson object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateSalesPerson(ref SalesPerson salesPersonToUpdate, LogSession session)
        {
            if (salesPersonToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!salesPersonToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            salesPersonToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateSalesPerson
        /// <summary>
        /// Function to create a new SalesPerson
        /// </summary>
        /// <param name="organizerToCreate">Reference to a SalesPerson object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateSalesPerson(ref SalesPerson salesPersonToCreate, LogSession session)
        {
            if (salesPersonToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!salesPersonToCreate.IsValid())
                return ResultType.MainDataNotValid;

            salesPersonToCreate.Save();

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

        #region DeleteSalesPerson
        public ResultType DeleteSalesPerson(int coyId,string salesPersonId, LogSession session)
        {
            if (coyId == null)
                return ResultType.NullMainData;

            if (salesPersonId == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            SalesPerson salesPerson = RetrieveSalesPersonByCoyIDSalesPersonID(coyId, salesPersonId);
            if (salesPerson == null)
                return ResultType.Error;

            salesPerson.Delete();
            return ResultType.Ok;
        }
        #endregion
    }
}
