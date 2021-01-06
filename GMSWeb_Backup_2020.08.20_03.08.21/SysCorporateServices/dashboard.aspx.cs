using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.SysCorporateServices
{
    public partial class dashboard : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../SessionTimeout.htm");
                return;
            }

            UserAccessModuleCategory uAccess = new GMSUserActivity().RetrieveUserAccessModuleCategoryByUserIdModCategoryId(session.UserId,
                                                                            17);
            if (uAccess == null)
                Response.Redirect("../Unauthorized.htm");
        }
    }
}
