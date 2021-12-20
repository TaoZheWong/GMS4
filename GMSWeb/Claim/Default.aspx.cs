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
using GMSWeb.CustomCtrl;
using System.Collections.Generic;
using System.Web.Services;

using CrystalDecisions;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace GMSWeb.Claim
{
    public partial class Default : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;
        protected static ReportDocument crReportDocument;

        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "Sales";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
            }

            Master.setCurrentLink(currentLink);

            LogSession session = base.GetSessionInfo();

            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Add/Edit Claim", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {

                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                            149);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            149);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));


            hidCoyID.Value = session.CompanyId.ToString();
            hidUserID.Value = session.UserId.ToString();
            hidCurrentLink.Value = currentLink;

            if (DateTime.Now > new GMSGeneralDALC().GetDocCloseDate(session.CompanyId, "Claim"))
            {
                btnAdd.Visible = false;
            }
        }

        public class ResponseModel
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public IDictionary<string, object> Params { get; set; }

            public ResponseModel()
            {
                Status = 0; // 0=success; 1=error
                Message = "Success";
            }

            public ResponseModel(int status, string message)
            {
                Status = status;
                Message = message;
            }
        }

        [WebMethod]
        public static ResponseModel GetClaims(short CompanyID, short UserID, short Status, string ClaimDateFrom, string ClaimDateTo, short Condition)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetClaims(CompanyID, UserID, Status, ClaimDateFrom, ClaimDateTo, Condition, ref dsTemp);
                m.Params = new Dictionary<string, object> { { "data", GMSUtil.ToJson(dsTemp, 0) } };
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }

        [WebMethod]
        public static ResponseModel GetApproveRights(int UserID)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetActionAccessRight(UserID, 150, ref dsTemp);
                m.Params = new Dictionary<string, object> { { "data", GMSUtil.ToJson(dsTemp, 0) } };
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }


        [WebMethod]
        public static ResponseModel CreateClaim(short CompanyID, short UserID, string ClaimDate, string Desc)
        {

            var m = new ResponseModel();

            try
            {
                DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(CompanyID, (short)DateTime.Now.Year);
                if (documentNumber == null) //If tbDocumentNumber does not exist
                {
                    documentNumber = new DocumentNumber();
                    documentNumber.CoyID = CompanyID;
                    documentNumber.Year = (short)DateTime.Now.Year;
                    documentNumber.QuotationNo = "0001";
                    documentNumber.ExternalCourseCodePrefix = "E";
                    documentNumber.ExternalCourseCodeNumber = "001";
                    documentNumber.InternalCourseCodePrefix = "I";
                    documentNumber.InternalCourseCodeNumber = "001";
                    documentNumber.OrganizerID = 0;
                    documentNumber.EmployeeCourseRowID = 0;
                    documentNumber.EmployeeID = 0;
                    documentNumber.AttachmentNo = "0001";
                    documentNumber.ProspectNo = "0001";
                    documentNumber.ContactNo = "0001";
                    documentNumber.CommNo = "0001";
                    documentNumber.CommCommentNo = "0001";
                    documentNumber.PurchaseNo = "0001";
                    documentNumber.MRNo = "0001";
                    documentNumber.DocumentNo = "00001";
                    documentNumber.DocumentNoForEmployee = "000001";
                    documentNumber.ProjectNo = "000001";
                    documentNumber.CEID = "0001";
                    documentNumber.CEDetailID = "00001";
                    documentNumber.ItemID = "1";
                    documentNumber.ClaimNo = "0001";
                }

                var ClaimNo = "EC" + DateTime.Now.ToString("yy") + "" + documentNumber.ClaimNo.ToString();

                new GMSGeneralDALC().InsertNewClaim(ClaimNo, CompanyID, UserID, ClaimDate, Desc);

                string nxtStr = ((short)(short.Parse(documentNumber.ClaimNo) + 1)).ToString();
                for (int i = nxtStr.Length; i < documentNumber.ClaimNo.Length; i++)
                {
                    nxtStr = "0" + nxtStr;
                }

                documentNumber.ClaimNo = nxtStr;
                documentNumber.Save();

                m.Message = "Successfully Insert";
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;

        }
    }
}