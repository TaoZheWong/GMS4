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
    public class TempTransferActivity : ActivityBase
    {
        #region Constructor
        public TempTransferActivity()
        {
        }
        #endregion

        #region RetrieveTempTransferByCoyID
        public IList<TempTransfer> RetrieveTempTransferByCoyID(short coyId)
        {
            if (coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("TempTransfer.CoyID"),
                                helper.CleanValue(coyId));

            return TempTransfer.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveAllTempTransfer
        public IList<TempTransfer> RetrieveAllTempTransfer()
        {
            return TempTransfer.RetrieveAll();
        }
        #endregion

        #region DeleteTempTransferByCoyID
        public ResultType DeleteTempTransferByCoyID(short coyId)
        {
            if (coyId <= 0)
                return ResultType.NullMainData;

            IList<TempTransfer> lstTT = this.RetrieveTempTransferByCoyID(coyId); 
            foreach (TempTransfer tt in lstTT)
            {
                tt.Delete(); 
                tt.Resync();
            }

            return ResultType.Ok;
        }
        #endregion

        #region DeleteAllTempTransfer
        public ResultType DeleteAllTempTransfer()
        {
            IList<TempTransfer> lstTT = this.RetrieveAllTempTransfer(); 
            foreach (TempTransfer tt in lstTT)
            {
                tt.Delete(); 
                tt.Resync();
            }

            return ResultType.Ok;
        }
        #endregion

        #region CreateTempTransfer
        public ResultType CreateTempTransfer(ref TempTransfer ttToCreate, LogSession session)
        {
            if (ttToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!ttToCreate.IsValid())
                return ResultType.MainDataNotValid;

            ttToCreate.Save();

            return ResultType.Ok;
        }
        #endregion        

    }
}
