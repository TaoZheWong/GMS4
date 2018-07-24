using System;
using System.Collections.Generic;
using System.Text;

namespace GMSCore.Activity
{
    public abstract class ActivityBase : IActivity
    {
        /// <summary>
        /// Gets the Wilson Ormapper's GetHelper object
        /// </summary>
        /// <returns></returns>
        protected Wilson.ORMapper.QueryHelper GetHelper()
        {
            DBManager db = DBManager.GetInstance();
            return db.Engine.QueryHelper;
        }
    }
}
