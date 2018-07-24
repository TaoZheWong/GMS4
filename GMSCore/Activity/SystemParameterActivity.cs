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
    public class SystemParameterActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public SystemParameterActivity()
        {
        }
        #endregion

        #region RetrieveAdministratorAlternatePassword
        public SystemParameter RetrieveAdministratorAlternatePassword()
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SystemParameter.ParameterName"),
                                helper.CleanValue("AdministratorAlternatePassword"));
  
            return SystemParameter.RetrieveFirst(stb.ToString());
        }

        public SystemParameter RetrieveSystemParameterByParameterName(string paramterName)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SystemParameter.ParameterName"),
                                helper.CleanValue(paramterName));

            return SystemParameter.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region RetrieveLatestNewsDate
        public SystemParameter RetrieveLatestNewsDate()
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SystemParameter.ParameterName"),
                                helper.CleanValue("LatestNewsDate"));

            return SystemParameter.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveProcessingTrialBalance
        public SystemParameter RetrieveProcessingTrialBalance()
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SystemParameter.ParameterName"),
                                helper.CleanValue("ProcessingTrialBalance"));

            return SystemParameter.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region UpdateProcessingTrialBalance
        public ResultType UpdateProcessingTrialBalance(ref SystemParameter sp, LogSession session)
        {
            if (sp == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!sp.IsValid())
                return ResultType.MainDataNotValid;

            sp.Save();

            return ResultType.Ok;
        }
        #endregion
    }
}
