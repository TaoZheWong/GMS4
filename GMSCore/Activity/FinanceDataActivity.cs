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
    public class FinanceDataActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public FinanceDataActivity()
        {
        }
        #endregion

        #region RetrieveFinanceDataByYearMonthItemID
        public IList<FinanceData> RetrieveFinanceDataByYearMonthItemID(short year, short month, short itemId, short coyId)
        {
            if (year <= 0 || month <= 0 || itemId <=0 || coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FinanceData.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.ProjectID"),-1);
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.DepartmentID"), -1);
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.SectionID"), -1);

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.ItemID"),
                                helper.CleanValue(itemId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.TbYear"),
                                helper.CleanValue(year));
            
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.TbMonth"),
                                helper.CleanValue(month));

            return FinanceData.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region DeleteFinanceDataByYearMonthItemID
        public ResultType DeleteFinanceDataByYearMonthItemID(short year, short month, short itemId, short coyId)
        {
            if (year <= 0 || month <= 0 || itemId <=0 || coyId <= 0)
                return ResultType.NullMainData;

            IList<FinanceData> lstFinance = this.RetrieveFinanceDataByYearMonthItemID(year, month, itemId, coyId);
            foreach (FinanceData finance in lstFinance)
            {
                finance.Delete();
                finance.Resync();
            }

            return ResultType.Ok;
        }
        #endregion

        #region CreateFinanceData
        /// <summary>
        /// Function to create a new FinanceData
        /// </summary>
        /// <param name="financeToCreate">Reference to a FinanceData object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateFinanceData(ref FinanceData financeToCreate, LogSession session)
        {
            if (financeToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!financeToCreate.IsValid())
                return ResultType.MainDataNotValid;

            financeToCreate.Save();

            return ResultType.Ok;
        }
        #endregion

        public FinanceData RetrieveFinanceDataByItemID(short year, short month, short itemId, short coyId)
        {
            if (year <= 0 || month <= 0 || itemId <= 0 || coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FinanceData.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.ProjectID"), -1);
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.DepartmentID"), -1);
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.SectionID"), -1);

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.ItemID"),
                                helper.CleanValue(itemId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.TbYear"),
                                helper.CleanValue(year));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceData.TbMonth"),
                                helper.CleanValue(month));

          
            return FinanceData.RetrieveFirst(stb.ToString());
         }

    }
}
