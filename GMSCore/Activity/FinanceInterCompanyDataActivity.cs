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
    public class FinanceInterCompanyDataActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public FinanceInterCompanyDataActivity()
        {
        }
        #endregion

        #region RetrieveFinanceInterCompanyDataByYearMonth
        public IList<FinanceInterCompanyData> RetrieveFinanceInterCompanyDataByYearMonth(short year, short month, short coyId)
        {
            if (year <= 0 || month <= 0 || coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FinanceInterCompanyData.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceInterCompanyData.TbYear"),
                                helper.CleanValue(year));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceInterCompanyData.TbMonth"),
                                helper.CleanValue(month));

            return FinanceInterCompanyData.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region DeleteFinanceInterCompanyDataByYearMonth
        public ResultType DeleteFinanceInterCompanyDataByYearMonth(short year, short month, short coyId)
        {
            if (year <= 0 || month <= 0 || coyId <= 0)
                return ResultType.NullMainData;

            IList<FinanceInterCompanyData> lstFinance = this.RetrieveFinanceInterCompanyDataByYearMonth(year, month, coyId);
            foreach (FinanceInterCompanyData finance in lstFinance)
            {
                finance.Delete();
                finance.Resync(); 
            }

            return ResultType.Ok;
        }
        #endregion                

        #region CreateFinanceInterCompanyData
        public ResultType CreateFinanceInterCompanyData(ref FinanceInterCompanyData financeToCreate, LogSession session)
        {
            if (financeToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!financeToCreate.IsValid())
                return ResultType.MainDataNotValid;

            financeToCreate.Resync(); 
            financeToCreate.Save();

            return ResultType.Ok;
        }
        #endregion
    }
}
