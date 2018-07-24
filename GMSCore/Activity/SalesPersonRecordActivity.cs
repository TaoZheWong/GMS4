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
    public class SalesPersonRecordActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public SalesPersonRecordActivity()
        {
        }
        #endregion
        #region RetrieveSalesPersonRecordByCoyIDSPYearMonth
        public SalesPersonRecord RetrieveSalesPersonRecordByCoyIDSPYearMonth(short salesPersonMasterID, int year, int month)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat("{0} = {1} ", helper.GetFieldName("SalesPersonRecord.SalesPersonMasterID"),
                                helper.CleanValue(salesPersonMasterID));
            stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("SalesPersonRecord.TbYear"),
                                helper.CleanValue(year));
            stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("SalesPersonRecord.TbMonth"),
                                helper.CleanValue(month));

            return SalesPersonRecord.RetrieveFirst(stb.ToString());
        }
        #endregion 
        #region RetrieveSalesPersonRecordByCoyIDSalesPersonMasterNameYearMonth
        public SalesPersonRecord RetrieveSalesPersonRecordBySalesPersonMasterIDYearMonth(short salesPersonMasterID, short salesPersonMasterUserID, int year, int month)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat("{0} = {1} ", helper.GetFieldName("SalesPersonRecord.SalesPersonMasterID"),
                                helper.CleanValue(salesPersonMasterID));
            stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("SalesPersonRecord.SalesPersonMasterUserID"),
                                helper.CleanValue(salesPersonMasterUserID));
            stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("SalesPersonRecord.TbYear"),
                                helper.CleanValue(year));
            stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("SalesPersonRecord.TbMonth"),
                                helper.CleanValue(month));

            return SalesPersonRecord.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region UpdateSalesPersonRecord
        /// <summary>
        /// Function to update SalesPersonRecord
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a SalesPersonRecord object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateSalesPersonRecord(ref SalesPersonRecord salesPersonRecordToUpdate, LogSession session)
        {
            if (salesPersonRecordToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!salesPersonRecordToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            salesPersonRecordToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateSalesPersonRecord
        /// <summary>
        /// Function to create a new SalesPersonRecord
        /// </summary>
        /// <param name="organizerToCreate">Reference to a SalesPersonRecord object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateSalesPersonRecord(ref SalesPersonRecord salesPersonRecordToCreate, LogSession session)
        {
            if (salesPersonRecordToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!salesPersonRecordToCreate.IsValid())
                return ResultType.MainDataNotValid;

            salesPersonRecordToCreate.Save();
            salesPersonRecordToCreate.Resync();
            return ResultType.Ok;

          
        }
        #endregion

        #region DeleteSalesPersonRecord
        public ResultType DeleteSalesPersonRecord(short salesPersonMasterID, short salesPersonMasterUserID, int year, int month, LogSession session)
        {
            if (year == null)
                return ResultType.NullMainData;

            if (month == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            SalesPersonRecord salesPersonRecord = RetrieveSalesPersonRecordBySalesPersonMasterIDYearMonth(salesPersonMasterID, salesPersonMasterUserID, year, month);
            if (salesPersonRecord == null)
                return ResultType.Error;

            salesPersonRecord.Delete();
            salesPersonRecord.Resync();
            return ResultType.Ok;
        }
        #endregion
    }
}
