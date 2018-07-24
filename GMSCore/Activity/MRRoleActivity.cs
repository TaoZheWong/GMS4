using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using GMSCore;
using GMSCore.Activity;
using System.Collections.Generic;
using System.Text;
using GMSCore.Entity;
using GMSCore.Exceptions;
using Wilson.ORMapper;

namespace GMSCore.Activity
{
    public class MRRoleActivity : ActivityBase    
    {

        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public MRRoleActivity()
        {
        }
        #endregion

        #region RetrievePurchaser
        public MRRole RetrieveMainPurchaser(short coyid)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRRole.CoyID"),
                                helper.CleanValue(coyid));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRRole.Approval"),
                                helper.CleanValue(1));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRRole.Role"),
                               helper.CleanValue("Purchasing"));

            return MRRole.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrievePurchaser
        public MRRole RetrievePurchaser(short coyid, short userId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRRole.CoyID"),
                                helper.CleanValue(coyid));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRRole.UserNumID"),
                                helper.CleanValue(userId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRRole.Role"),
                               helper.CleanValue("Purchasing"));

            return MRRole.RetrieveFirst(stb.ToString());
        }
        #endregion
    }
}
