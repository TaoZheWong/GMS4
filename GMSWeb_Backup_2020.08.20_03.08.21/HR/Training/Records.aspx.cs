using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMSWeb.HR.Training
{
    public partial class Records : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["ModuleID"] != null)
            {
                Master.setCurrentLink(Request.Params["ModuleID"]);
            }
        }
    }
}