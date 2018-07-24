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
    public class ParameterActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public ParameterActivity()
        {
        }
        #endregion

        #region RetrieveParameterByParameterId
        public IList<CompanySpecialData> RetrieveParameterByParameterId(short paramterId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("CompanySpecialData.ParameterID"),
                                helper.CleanValue(paramterId));

            return CompanySpecialData.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveParameterByCoyIdParameterId
        public IList<CompanySpecialData> RetrieveParameterByCoyIdParameterId(short CoyId, short parameterId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("CompanySpecialData.CoyID"),
                                helper.CleanValue(CoyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("CompanySpecialData.ParameterID"),
                                helper.CleanValue(parameterId));

            return CompanySpecialData.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveParameterByCSDDate
        public CompanySpecialData RetrieveParameterByCSDDate(short CoyId,
                                                                            short parameterId, DateTime CSDDate)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("CompanySpecialData.CoyID"),
                                helper.CleanValue(CoyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("CompanySpecialData.ParameterID"),
                                helper.CleanValue(parameterId));

            stb.AppendFormat(" AND {0} ", helper.GetExpression("CompanySpecialData.CSDDate", CSDDate, ComparisonOperators.Equals));

            return CompanySpecialData.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region UpdateCompanyParameter
        /// <summary>
        /// Function to update Company Parameter
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a CompanySpecialData object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateCompanyParameter(ref CompanySpecialData parameterToUpdate, LogSession session)
        {
            if (parameterToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!parameterToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            parameterToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateCompanySpecialData
        /// <summary>
        /// Function to create a new CompanySpecialData
        /// </summary>
        /// <param name="parameterToCreate">Reference to a CompanySpecialData object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateCompanySpecialData(ref CompanySpecialData parameterToCreate, LogSession session)
        {
            if (parameterToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!parameterToCreate.IsValid())
                return ResultType.MainDataNotValid;

            parameterToCreate.Save();

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

        #region DeleteSpecialData
        public ResultType DeleteSpecialData(DateTime CSDDate,short parameterId, LogSession session)
        {
            //if (CSDDate == null)
            //    return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            CompanySpecialData specialData = RetrieveParameterByCSDDate(session.CompanyId, parameterId, CSDDate); 
            if (specialData == null)
                return ResultType.Error;

            specialData.Delete();
            return ResultType.Ok;
        }
        #endregion
    }
}
