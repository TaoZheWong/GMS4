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
    public class ProcessedTrialBalanceActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public ProcessedTrialBalanceActivity()
        {
        }
        #endregion

        //#region RetrieveProcessedTrialBalanceByYearItemStructureId
        //public IList<ProcessedTrialBalance> RetrieveProcessedTrialBalanceByYearItemStructureId(short itemStructureId, double year, short coyId)
        //{
        //    if (itemStructureId <= 0)
        //        return null;

        //    if (year <= 0)
        //        return null;

        //    if (coyId <= 0)
        //        return null;

        //    QueryHelper helper = base.GetHelper();
        //    StringBuilder stb = new StringBuilder(200);
        //    stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Budget.ItemStructureID"),
        //                        helper.CleanValue(itemStructureId));

        //    stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Budget.BudgetYear"),
        //                        helper.CleanValue(year));

        //    stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Budget.CoyID"),
        //                        helper.CleanValue(coyId));

        //    return ProcessedTrialBalance.RetrieveQuery(stb.ToString());
        //}
        //#endregion

        #region RetrieveProcessedTrialBalanceByYearMonthDepartmentIDItemStructureID
        public ProcessedTrialBalance RetrieveProcessedTrialBalanceByYearMonthDepartmentIDItemStructureID(double year, double month, short coyId, short departmentId, short itemStructureId)
        {
            if (coyId <= 0)
                return null;

            if (itemStructureId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ProcessedTrialBalance.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ProcessedTrialBalance.ItemStructureID"),
                                helper.CleanValue(itemStructureId));

                stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ProcessedTrialBalance.TBYear"),
                                helper.CleanValue(year));

                stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ProcessedTrialBalance.TBMonth"),
                                    helper.CleanValue(month));

                stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ProcessedTrialBalance.DepartmentID"),
                                    helper.CleanValue(departmentId));

            return ProcessedTrialBalance.RetrieveFirst(stb.ToString());
        }
        #endregion

        //#region DeleteProcessedTrialBalanceByYearMonthItemStructureID
        //public ResultType DeleteProcessedTrialBalanceByYearItemStructureID(double year,  short itemStructureId, short coyId)
        //{
        //    if (itemStructureId <= 0)
        //        return ResultType.NullMainData;

        //    if (coyId <= 0)
        //        return ResultType.NullMainData;

        //    IList<ProcessedTrialBalance> lstProcessedTrialBalance = this.RetrieveProcessedTrialBalanceByYearItemStructureId(itemStructureId, year, coyId);
        //    foreach (ProcessedTrialBalance processedTrialBalance in lstProcessedTrialBalance)
        //    {
        //        processedTrialBalance.Delete();
        //        processedTrialBalance.Resync();
        //    }

        //    return ResultType.Ok;
        //}
        //#endregion

        #region CreateProcessedTrialBalance
        /// <summary>
        /// Function to create a new ProcessedTrialBalance
        /// </summary>
        /// <param name="auditToCreate">Reference to a ProcessedTrialBalance object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateProcessedTrialBalance(ref ProcessedTrialBalance processedTrialBalanceToCreate, LogSession session)
        {
            if (processedTrialBalanceToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!processedTrialBalanceToCreate.IsValid())
                return ResultType.MainDataNotValid;

            processedTrialBalanceToCreate.Save();

            return ResultType.Ok;
        }
        #endregion
    }
}
