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

namespace GMSWeb
{
    public partial class gmscontent : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //LogSession session = base.GetSessionInfo();

            //UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
            //                                                                GMSUtil.ToShort(CompanyId.BluePowerCorporationPteLtd));
            //if (uAccess == null)
            //    Response.Redirect(Request.ApplicationPath + "/Unauthorized.htm");
        }
    }
}
