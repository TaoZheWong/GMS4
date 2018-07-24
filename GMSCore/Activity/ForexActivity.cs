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
    public class ForexActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public ForexActivity()
        {
        }
        #endregion

        #region RetrieveForexHistorySortByCreatedDate
        public IList<ForeignExchangeRate> RetrieveForexHistorySortByCreatedDate(string homeCurrencyCode, string foreignCurrencyCode)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ForeignExchangeRate.HomeCurrencyCode"),
                                helper.CleanValue(homeCurrencyCode));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ForeignExchangeRate.ForeignCurrencyCode"),
                                helper.CleanValue(foreignCurrencyCode));

            //stb.AppendFormat(" AND {0} = 0 ", helper.GetFieldName("ForeignExchangeRate.IsInEffect"),

            //                    helper.CleanValue(foreignCurrencyCode));
            StringBuilder stbSort = new StringBuilder(200);
            stbSort.AppendFormat(" {0} DESC ", helper.GetFieldName("ForeignExchangeRate.CreatedDate"));

            return ForeignExchangeRate.RetrieveQuery(stb.ToString(), stbSort.ToString());
        }

        #endregion

        #region RetrieveForexRateByHomeCurrencyCode
        public IList<ForeignExchangeRate> RetrieveForexRateByHomeCurrencyCode(string homeCurrencyCode)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ForeignExchangeRate.HomeCurrencyCode"),
                                helper.CleanValue(homeCurrencyCode));

            stb.AppendFormat(" AND {0} = 1 ", helper.GetFieldName("ForeignExchangeRate.IsInEffect"));

            return ForeignExchangeRate.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate
        public ForeignExchangeRate RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate(string homeCurrencyCode, 
                                                                            string foreignCurrencyCode, DateTime createdDate)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ForeignExchangeRate.HomeCurrencyCode"),
                                helper.CleanValue(homeCurrencyCode));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ForeignExchangeRate.ForeignCurrencyCode"),
                                helper.CleanValue(foreignCurrencyCode));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ForeignExchangeRate.CreatedDate"),
                                helper.CleanValue(createdDate));

            return ForeignExchangeRate.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveForexRateByCreatedDate
        public IList<ForeignExchangeRate> RetrieveForexRateByCreatedDate(DateTime createdDate)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ForeignExchangeRate.CreatedDate"),
                                helper.CleanValue(createdDate));

            return ForeignExchangeRate.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveForexRateByHomeForeignCurrencyCodeIsInEffect
        public ForeignExchangeRate RetrieveForexRateByHomeForeignCurrencyCodeIsInEffect(string homeCurrencyCode,
                                                                            string foreignCurrencyCode)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ForeignExchangeRate.HomeCurrencyCode"),
                                helper.CleanValue(homeCurrencyCode));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ForeignExchangeRate.ForeignCurrencyCode"),
                                helper.CleanValue(foreignCurrencyCode));

            stb.AppendFormat(" AND {0} = 1 ", helper.GetFieldName("ForeignExchangeRate.IsInEffect"));

            return ForeignExchangeRate.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region UpdateForeignExchangeRate
        /// <summary>
        /// Function to update ForeignExchangeRate
        /// </summary>
        /// <param name="forexToUpdate">Reference to a ForeignExchangeRate object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateForeignExchangeRate(ref ForeignExchangeRate forexToUpdate, LogSession session)
        {
            if (forexToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!forexToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            forexToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateForeignExchangeRate
        /// <summary>
        /// Function to create a new ForeignExchangeRate
        /// </summary>
        /// <param name="forexToCreate">Reference to a ForeignExchangeRate object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateForeignExchangeRate(ref ForeignExchangeRate forexToCreate, ref ForeignExchangeRate existingForexToUpdate, LogSession session)
        {
            if (forexToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!forexToCreate.IsValid())
                return ResultType.MainDataNotValid;


            #region Transaction
            TransactionManager trans = null;
            try
            {
                trans = new TransactionManager();

                if (existingForexToUpdate != null)
                trans.Save(existingForexToUpdate);
                trans.Save(forexToCreate);

                trans.Commit();
                if (existingForexToUpdate != null)
                existingForexToUpdate.Resync();
                forexToCreate.Resync();
                return ResultType.Ok;
            }
            catch (Exception)
            {
                if (trans != null)
                    trans.Rollback();
                throw;
            }
            finally
            {
                if (trans != null)
                    trans.Dispose();
            }
            #endregion //Transaction

        }
        #endregion

        #region DeleteForeignExchangeRate
        public ResultType DeleteForeignExchangeRateByHomeForeignCurrencyCodeCreatedDate(string homeCurrencyCode,
                                                                            string foreignCurrencyCode, DateTime createdDate)
        {
            ForeignExchangeRate fRate = this.RetrieveForexRateByHomeForeignCurrencyCodeCreatedDate(homeCurrencyCode, foreignCurrencyCode, createdDate);
            fRate.Delete();
            fRate.Resync();

            return ResultType.Ok;
        }
        #endregion

        #region DeleteForeignExchangeRateByCreateDate
        public ResultType DeleteForeignExchangeRateByCreatedDate(DateTime createdDate)
        {
            IList<ForeignExchangeRate> lstRate = this.RetrieveForexRateByCreatedDate(createdDate);
            foreach (ForeignExchangeRate fRate in lstRate)
            {
                fRate.Delete();
                fRate.Resync();
            }

            return ResultType.Ok;
        }
        #endregion
    }
}
