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
    public class CashFlowProjActivity : ActivityBase
    {
        public CashFlowProjActivity()
        {
        }

         public ResultType CreateCashFlowProjection(ref CashFlowProjectionForWeek cfp, LogSession session)
        {
            if (cfp == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!cfp.IsValid())
                return ResultType.MainDataNotValid;

            cfp.Save();

            return ResultType.Ok;            

        }
    }
}
