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
using System.Globalization;
using Newtonsoft.Json;

namespace GMSWeb.Claim
{
    public partial class Detail : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;

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
                Response.Redirect(base.SessionTimeOutPage("Sales"));
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
                Response.Redirect(base.UnauthorizedPage("Sales"));

            hidCurrentLink.Value = currentLink;

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
        public static ResponseModel GetActionAccessRight(int UserID)
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
        public static ResponseModel SaveClaimAttachment(int ClaimAttachmentID, int ClaimDetailID, string data)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().SaveClaimAttachment(ClaimAttachmentID,ClaimDetailID, data);
                m.Message = "Save Successful";
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }
        [WebMethod]
        public static ResponseModel DeleteAttachment(int ClaimAttachmentID)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().DeleteClaimDetailAttachment(ClaimAttachmentID);
                m.Message = "Delete Successful";
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }
        
        [WebMethod]
        public static ResponseModel GetClaimAttachment(int ClaimDetailID)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetClaimAttachment(ClaimDetailID, ref dsTemp);
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
        public static ResponseModel GetSelectionCustomerList(string Name , short CompanyID)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetSelectionCustomerList(Name, CompanyID, ref dsTemp);
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
        public static ResponseModel GetCompanyProjectListByCoyID( short CompanyID)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetCompanyProject(CompanyID, ref dsTemp);
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
        public static ResponseModel GetCompanyDepartmentListByCoyIDAndProjectID(short CompanyID, short ProjectID)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetCompanyDepartment(CompanyID, ProjectID, ref dsTemp);
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
        public static ResponseModel GetCompanySectionList(short CompanyID, short DepartmentID)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetCompanySection(CompanyID, DepartmentID, ref dsTemp);
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
        public static ResponseModel GetCompanyUnitList(short CompanyID, short SectionID)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetCompanyUnit(CompanyID, SectionID, ref dsTemp);
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
        public static ResponseModel GetClaim(int ClaimID)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetClaimByID(ClaimID, ref dsTemp);
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
        public static ResponseModel UpdateClaim(int ClaimID, string Description, string ClaimDate, 
            string ClaimantDesig, string SalesPersonID, string NumPplEntertained, string CreateOnBehalf,
            string dim1, string dim2, string dim3, string dim4,
            string Cust1, string Cust2, string Cust3, 
            string Desig1, string Desig2, string Desig3, 
            string Person1, string Person2, string Person3,
            string Phone1, string Phone2, string Phone3
            )
        {
            var m = new ResponseModel();
            
            try
            {
                new GMSGeneralDALC().UpdateClaim(ClaimID, Description, ClaimDate, ClaimantDesig, SalesPersonID, NumPplEntertained, CreateOnBehalf,
                    Convert.ToInt16(dim1), Convert.ToInt16(dim2), Convert.ToInt16(dim3), Convert.ToInt16(dim4),
                    Cust1, Cust2, Cust3,
                    Desig1, Desig2, Desig3,
                    Person1, Person2, Person3,
                    Phone1, Phone2, Phone3
                );

                m.Message = "Claim info update successfully.";
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }
        
        public class Customer {
            public string name { get; set; }
            public string designation { get; set; }
        }

        public class ClaimDetail
        {
            public string id { get; set; }
            public string date { get; set; }
            public string type { get; set; }
            public string currencyCode { get; set; }
            public string remark { get; set; }
            public float currencyRate { get; set; }
            public float amount { get; set; }
            public float amountSGD { get; set; }
            public string chargeto { get; set; }
        }


        [WebMethod]
        public static ResponseModel DetailModel()
        {
            var m = new ResponseModel();

            try
            {
                 var claimDetail = new ClaimDetail();

                 claimDetail.currencyRate = 0;
                 claimDetail.amount = 0;
                 claimDetail.amountSGD = 0;
                 
                 m.Params = new Dictionary<string, object> { { "data",  JsonConvert.SerializeObject(claimDetail)} };
                
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
           

         
        }

        [WebMethod]
        public static ResponseModel GetClaimDetails(int ClaimID, int CompanyID)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetClaimDetails(ClaimID, CompanyID, ref dsTemp);
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
        public static ResponseModel GetEntertainmentType()
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetClaimEntertainmentType(ref dsTemp);
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
        public static ResponseModel CreateClaimDetail(short claimID, string details, short companyID)
        {
            var m = new ResponseModel();

            try
            {
                List<ClaimDetail> claimDetail = JsonConvert.DeserializeObject<List<ClaimDetail>>(details);

                foreach (var detailObj in claimDetail)
                {
                    if (detailObj.id != null)
                    {
                        //Update
                        new GMSGeneralDALC().UpdateClaimDetail(int.Parse(detailObj.id), detailObj.type, detailObj.date, detailObj.remark,detailObj.currencyCode, detailObj.currencyRate, detailObj.amount, detailObj.chargeto);
                    }
                    else {
                        //insert
                        new GMSGeneralDALC().InsertNewClaimDetail(companyID, claimID, detailObj.type, detailObj.date, detailObj.remark,detailObj.currencyCode, detailObj.currencyRate, detailObj.amount, detailObj.chargeto);
                    }
                    
                }

                m.Message = "Claim details saved successfully.";
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }

        [WebMethod]
        public static ResponseModel SubmitClaim(int ClaimID, string Type, int UserID, string RejectRemark)
        {
            var m = new ResponseModel();
            short status = 0;

            try
            {
                if (Type == "Submit")
                    status = 1;
                else if (Type == "Approve")
                    status = 2;
                else if (Type == "Reject")
                    status = 3;
                else if (Type == "Resubmit")
                    status = 4;

                new GMSGeneralDALC().UpdateClaimStatus(ClaimID, UserID, status, RejectRemark);

                m.Message = Type + " Successfully";
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }

        [WebMethod]
        public static ResponseModel DeleteClaimDetail(int ClaimDetailID)
        {
            var m = new ResponseModel();

            try
            {
                new GMSGeneralDALC().DeleteClaimDetail(ClaimDetailID);

                m.Message = "Delete Successfully";
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }

        [WebMethod]
        public static ResponseModel GetSelectionCurrencyList(string DefaultCurrency, string Date, short CompanyID , string Currency)
        {
            var m = new ResponseModel();
            DateTime inputDate;
            try
            {
                if (!Date.Equals(String.Empty))
                    inputDate = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                else
                    inputDate = new DateTime();
                
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetSelectionCurrencyList(DefaultCurrency,Currency, CompanyID, (short)inputDate.Year, (short)inputDate.Month, ref dsTemp);
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
        public static ResponseModel GetClaimSalesPersonID(int CompanyID, int ClaimantID)
        {
            var m = new ResponseModel();
            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetClaimSalesPersonID( CompanyID, ClaimantID, ref dsTemp);
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
        public static ResponseModel DeleteClaim(int ClaimID, int CoyID)
        {
            var m = new ResponseModel();

            try
            {
                new GMSGeneralDALC().DeleteClaim(ClaimID, CoyID);

                m.Message = "Delete Successfully";
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }

        [WebMethod]
        public static Boolean IsLatestTransaction(int CoyID, int ClaimID, string ModifyDate, string Module)
        {
            var m = new ResponseModel();
            Boolean tempResult = new Boolean();

            try
            {
                new GMSGeneralDALC().IsLatestTransaction(CoyID, ClaimID, ModifyDate, Module, ref tempResult);
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }
            
            return tempResult;
        }

        [WebMethod]
        public static ResponseModel CreateOnBehalf(int CoyID, int UserID)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().EntertainmentClaimCreateOnBehalf(CoyID, UserID, ref dsTemp);
                m.Params = new Dictionary<string, object> { { "data", GMSUtil.ToJson(dsTemp, 0) } };
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