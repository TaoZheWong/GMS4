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
using GMSCore.Entity;
using GMSCore.Activity;
using System.Text;

namespace GMSWeb.SysHR
{
    public partial class Form : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string formOnlineID = Request.Params["FORMONLINEID"];
            string applicantID = Request.Params["APPLICANTID"];
            string approvalID = Request.Params["APPROVALID"];

            if (!string.IsNullOrEmpty(formOnlineID) && !string.IsNullOrEmpty(applicantID))
            {
                FormRandomID randomID = (new FormActivity()).RetrieveFormRandomIDByFormOnlineIDApplicantID(formOnlineID, applicantID);
                if (randomID != null)
                {
                    switch (randomID.FormType)
                    {
                        case "TNF":
                            Response.Redirect("../SysHR/Training/AddEditTNF.aspx?FORMONLINEID=" + formOnlineID + "&APPLICANTID=" + applicantID);
                            return;

                        case "CEF":
                            Response.Redirect("../SysHR/Training/AddEditCEF.aspx?FORMONLINEID=" + formOnlineID + "&APPLICANTID=" + applicantID);
                            return;
                    }
                }
            }

            if (!string.IsNullOrEmpty(formOnlineID) && !string.IsNullOrEmpty(approvalID))
            {
                FormRandomID randomID = (new FormActivity()).RetrieveFormRandomIDByFormOnlineIDApprovalID(formOnlineID, approvalID);
                if (randomID != null)
                {
                    switch (randomID.FormType)
                    {
                        case "TNF":
                            Response.Redirect("../SysHR/Training/AddEditTNF.aspx?FORMONLINEID=" + formOnlineID + "&APPROVALID=" + approvalID);
                            return;

                        case "CEF":
                            Response.Redirect("../SysHR/Training/AddEditCEF.aspx?FORMONLINEID=" + formOnlineID + "&APPROVALID=" + approvalID);
                            return;
                    }
                }
            }

            Response.Redirect("../Common/Message.aspx?MESSAGE=" + "The link has already been processed or expired.");
            return;
        }
    }
}
