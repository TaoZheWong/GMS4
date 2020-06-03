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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Transactions;
using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Text;
using System.Web.Services;
using AjaxControlToolkit;
using System.Web.Script.Services;

namespace GMSWeb.Products.Products
{
    public partial class AddEditMaterialRequisition : GMSBasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string currentLink = "";
            currentLink = "Products";
            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            if (CheckUserAccessPage(session.CompanyId, session.UserId) == false)
                Response.Redirect(base.UnauthorizedPage(currentLink));


            short CompanyId = 0;
            if (Request.Params["CoyID"] != null && Request.Params["CoyID"].ToString() != "")
            {
                CompanyId = GMSUtil.ToShort(Request.Params["CoyID"]);
            }
            else
            {
                Response.Redirect("MaterialRequisitionSearch.aspx?CurrentLink=" + currentLink);
            }

            hidMRScheme.Value = session.MRScheme;
            string mrno = "";
            if (Request.Params["MRNo"] != null && Request.Params["MRNo"].ToString() != "")
            {
                mrno = Request.Params["MRNo"].ToString();
                DataSet ds1 = new DataSet();
                ds1 = CanUserAccessDocument(CompanyId, session.UserId, mrno);
                if (!(Convert.ToBoolean(ds1.Tables[0].Rows[0]["result"])))
                    Response.Redirect(base.UnauthorizedPage(currentLink));

                GMSCore.Entity.MR mr = GMSCore.Entity.MR.RetrieveByCoyAndMRNo(session.CompanyId, Request.Params["MRNo"].ToString());
                if (string.IsNullOrEmpty(mr.MRScheme))
                    hidMRScheme.Value = session.MRScheme;
                else
                    hidMRScheme.Value = mr.MRScheme;
            }

            hidUserID.Value = session.UserId.ToString();
            string userRole = "";
            userRole = GetUserRole(CompanyId, session.UserId);
            hidUserRole.Value = userRole;
            hidCurrentLink.Value = currentLink;
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(session.CompanyId, session.UserId);
            
            hidDimensionL1.Value = session.DimensionL1;
            hidWarehouse.Value = session.DefaultWarehouse;
            //if (session.CompanyId.ToString() == "120")
            //{
            //    DivisionUser du = DivisionUser.RetrieveByKey(session.CompanyId, loginUserOrAlternateParty);
            //    if (du != null)
            //    {
            //        if (du.DivisionID == "GAS")
            //        {
            //            session.MRScheme = "Department";
            //            session.DefaultWarehouse = "G02";
            //            session.DimensionL1 = "GAS";
            //        }
            //        else if (du.DivisionID == "WSD")
            //        {
            //            session.MRScheme = "Product";
            //            session.DefaultWarehouse = "W02";
            //            session.DimensionL1 = "WSD";
            //        }
            //    }
            //}
            MRRole purchaser = new MRRoleActivity().RetrieveMainPurchaser(CompanyId);
            if (purchaser != null)
                hidMainPurchaserUserID.Value = purchaser.UserNumID.ToString();
        }

        public static DataSet CanUserAccessDocument(short CompanyId, short UserId, string mrno)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet ds1 = new DataSet();
            new GMSGeneralDALC().CanUserAccessDocument(CompanyId, "MR", mrno, loginUserOrAlternateParty, ref ds1);
            return ds1;
        }


        public static bool CheckUserAccessPage(short CompanyId, short UserId)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 120);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(CompanyId, loginUserOrAlternateParty,
                                                                           120);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                return false;
            return true;
        }

        public static string GetUserRole(short CompanyId, short UserId)
        {
            string userRole = "";
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet lstUserRole = new DataSet();
            new GMSGeneralDALC().GetMRUserRoleByUserNumIDCoyID(CompanyId, loginUserOrAlternateParty, ref lstUserRole);
            if ((lstUserRole != null) && (lstUserRole.Tables[0].Rows.Count > 0))
            {
                userRole = lstUserRole.Tables[0].Rows[0]["UserRole"].ToString();
            }
            return userRole;
        }

        public static int GetMRFormApprovalLevelID(short CompanyId, string MRNo, short loginUserOrAlternateParty)
        {
            int levelID = 0;
            MRFormApproval latest = new MRFormApprovalActivity().RetrieveLastestFormApproval(CompanyId, MRNo);

            if (latest != null)
            {
                MRFormApproval approve = new MRFormApprovalActivity().RetrieveFormApprovalByCoyIDByUser(CompanyId, MRNo, loginUserOrAlternateParty, latest.LevelID);

                if (approve != null)
                {
                    levelID = approve.LevelID;
                }
                else
                {
                    MRFormApproval requestedApproval = new MRFormApprovalActivity().RetrieveRequestedFormApprovalByCoyIDByUser(CompanyId, MRNo, loginUserOrAlternateParty, latest.LevelID);
                    if (requestedApproval != null)
                        levelID = (short)requestedApproval.LevelID;
                }
            }

            return levelID;
        }

        public static short GetloginUserOrAlternateParty(short CompanyId, short UserID)
        {
            short loginUserOrAlternateParty = UserID;
            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(CompanyId, UserID, "MR", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {

                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
            {
                loginUserOrAlternateParty = UserID;
            }
            return loginUserOrAlternateParty;
        }


        [WebMethod]
        public static List<Dictionary<string, string>> GetMRHeader(short CompanyId, string MRNo)
        {
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref dsTemp); //procAppMaterialRequisitionsByMRNoSelect
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetConfirmedSales(short CompanyId, string MRNo)
        {
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 1);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetVendor(short CompanyId, string MRNo)
        {
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 2);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetProduct(short CompanyId, string MRNo)
        {
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 3);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetDelivery(short CompanyId, string MRNo)
        {
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 4);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetRoutingInfo(short CompanyId, string MRNo)
        {
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 5);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetAttachment(short CompanyId, string MRNo)
        {
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 6);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetAccountList(short CompanyId, short UserId, string account, bool exact, string accounttype)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetAccountList(CompanyId, account, loginUserOrAlternateParty, exact, accounttype, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }


        [WebMethod]
        public static List<Dictionary<string, string>> GetProductList(short CompanyId, string product, string approver1, string approver2, string approver3, string approver4, bool exact)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetProductList(CompanyId, product.Trim(), GMSUtil.ToShort(approver1), GMSUtil.ToShort(approver2), GMSUtil.ToShort(approver3), GMSUtil.ToShort(approver4), exact, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetUOMList(short CompanyId, string uom)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetUOMList(CompanyId, uom, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }


        [WebMethod]
        public static List<Dictionary<string, string>> GetProductGroupList(short CompanyId, string productgroupcode)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetProductGroupList(CompanyId, productgroupcode, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetProductTeamByProductGroup(short CompanyId, string productgroupcode)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetProductTeamByProductGroup(CompanyId, productgroupcode, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }



        [WebMethod]
        public static string Reject(short CompanyId, string MRNo, string Reason, string PageName, short UserID, string CurrentLink, string IsCurrentLevel)
        {
            try
            {
                short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
                int levelID = GetMRFormApprovalLevelID(CompanyId, MRNo, loginUserOrAlternateParty);

                if (IsCurrentLevel == "1")
                    new ApprovalActivity().RejectMR(CompanyId, MRNo, levelID, Reason, UserID);
                else
                    new ApprovalActivity().HighLevelRejectMR(CompanyId, MRNo, levelID, Reason, UserID);

                /*
                MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);
                if (mr != null)
                {
                    WorkFlow WF = new WorkFlow();
                    WF.CompleteWorkItem(WF.GetPendingWorkItem(mr.PIID), UserID, loginUserOrAlternateParty, "REJECT", Reason, false, false, true);
                    mr.StatusID = "R";
                    mr.Save();
                    new GMSGeneralDALC().SendEmailNotificationForMR("Reject", mr.PIID);
                    new GMSGeneralDALC().InsertMRLog(CompanyId, MRNo, UserID, loginUserOrAlternateParty, "Reject-AddEditMaterial");
                }
                */

            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + MRNo;

        }

        [WebMethod]
        public static string Cancel(short CompanyId, string MRNo, string Reason, string PageName, short UserID, string CurrentLink, string IsCurrentLevel)
        {
            try
            {
                short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
                /*
                MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);
                if (mr != null)
                {
                    WorkFlow WF = new WorkFlow();
                    WF.CompleteWorkItem(WF.GetPendingWorkItem(mr.PIID), UserID, loginUserOrAlternateParty, "CANCEL", Reason, false, true, true);
                    mr.StatusID = "X";
                    mr.CancelledReason = Reason;                    
                    mr.Save();

                    new GMSGeneralDALC().SendEmailNotificationForMR("Cancel", mr.PIID);
                    new GMSGeneralDALC().InsertMRLog(CompanyId, MRNo, UserID, loginUserOrAlternateParty, "Cancel-AddEditMaterial");
                }
                */

                int levelID = GetMRFormApprovalLevelID(CompanyId, MRNo, loginUserOrAlternateParty);
                if (IsCurrentLevel == "1")
                    new ApprovalActivity().CancelMR(CompanyId, loginUserOrAlternateParty, MRNo, levelID, Reason, UserID);
                else
                    new ApprovalActivity().HighLevelCancelMR(CompanyId, loginUserOrAlternateParty, MRNo, levelID, Reason, UserID);
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + MRNo;

        }


        [WebMethod]
        public static string Approve(short CompanyId, string MRNo, string Purchaser, string Reason, string PageName, short UserID, string CurrentLink, string IsCurrentLevel)
        {
            try
            {
                short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
                int levelID = GetMRFormApprovalLevelID(CompanyId, MRNo, loginUserOrAlternateParty);

                string status = "";
                if (IsCurrentLevel == "1")
                    status = new ApprovalActivity().ApproveMR(levelID, Purchaser, UserID);
                else
                    status = new ApprovalActivity().HighLevelApproveMR(CompanyId, MRNo, levelID, Purchaser, Reason, UserID);
                if (status != "OK")
                {
                    throw new HttpException(500, "No approval info");
                }


                /*
                MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);
                if (mr != null)
                {
                    
                    WorkFlow WF = new WorkFlow();
                    int pending_approver;
                    pending_approver = WF.CompleteWorkItem(WF.GetPendingWorkItem(mr.PIID), UserID, loginUserOrAlternateParty, "APPROVED", Reason, true, false, true);
                    if (pending_approver == 0)
                    {
                        mr.StatusID = "A";
                        mr.Purchaser = Purchaser;
                    }
                    mr.Save();
                    new GMSGeneralDALC().SendEmailNotificationForMR("Approve", mr.PIID);
                    

                    new GMSGeneralDALC().InsertMRLog(CompanyId, MRNo, UserID, loginUserOrAlternateParty, "APPROVED-AddEditMaterial");
                }
                */

            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + MRNo;


        }

        [WebMethod]
        public static string ConfirmVendor(short CompanyId, string MRNo, string PageName, string CurrentLink, short UserID)
        {
            try
            {
                IList<MRVendor> lstAutoInsertedMRVendor = null;
                lstAutoInsertedMRVendor = new MRActivity().RetrieveAutoInsertedVendorByMRNo(CompanyId, MRNo);

                if (lstAutoInsertedMRVendor.Count > 0)
                {
                    foreach (MRVendor vendor in lstAutoInsertedMRVendor)
                    {
                        vendor.MRSupplierID = null;
                        vendor.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + MRNo;
        }

        [WebMethod]
        public static string SubmitForApproval(short CompanyId, string MRNo, string PageName, short UserID, short MainPurchaserUserID, string CurrentLink)
        {
            try
            {
                short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
                /*
                MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);
                if (mr != null)
                {
                    WorkFlow WF = new WorkFlow();

                    if (mr.PIID == Guid.Empty)
                    {
                        int[] users = new int[] { mr.Requestor, mr.PMUserId, mr.PHUserId, mr.PH3UserId, mr.PH5UserId, MainPurchaserUserID };
                        DataTable dt = new WorkFlowRouting().GenerateRoutingTable_MR(users);
                        Guid piid = WF.InitiateProcessInstance(loginUserOrAlternateParty, "MR", dt);

                        if (piid != Guid.Empty)
                        {
                            WF.CompleteWorkItem(WF.GetPendingWorkItem(piid), UserID, loginUserOrAlternateParty, "SUBMIT", "NEW MR", true, false, true);
                            new GMSGeneralDALC().InsertMRLog(CompanyId, MRNo, UserID, loginUserOrAlternateParty, "SUBMIT-AddEditMaterial");
                        }
                        mr.PIID = piid;
                        
                    }
                    else
                    {
                        WF.CompleteWorkItem(WF.GetPendingWorkItem(mr.PIID), UserID, loginUserOrAlternateParty, "RE-SUBMIT", "RE-SUBMIT MR", true, false, true);
                        new GMSGeneralDALC().InsertMRLog(CompanyId, MRNo, UserID, loginUserOrAlternateParty, "RE-SUBMIT-AddEditMaterial");
                    }
                    mr.StatusID = "F";
                    mr.Save();
                    new GMSGeneralDALC().SendEmailNotificationForMR("Approve", mr.PIID);

                }
                */

                MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);
                new ApprovalActivity().InsertApprovaLevelInfoList(CompanyId, loginUserOrAlternateParty, MRNo, mr.PMUserId, mr.PHUserId, mr.PH3UserId, UserID);

                if (mr != null)
                {
                    mr.StatusID = "F";
                    mr.Save();
                }
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + MRNo;

        }


        [WebMethod]
        public static string DuplicateMR(short CompanyId, string oldMRNo, string PageName, short UserID, string CurrentLink)
        {
            DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(CompanyId, (short)DateTime.Now.Year);
            string mrno = "MR" + DateTime.Now.ToString("yy") + "" + documentNumber.MRNo.ToString();
            try
            {
                new GMSGeneralDALC().DuplicateMR(GMSUtil.ToByte(CompanyId), oldMRNo, mrno, UserID);
                string nxtStr = ((short)(short.Parse(documentNumber.MRNo) + 1)).ToString();
                for (int i = nxtStr.Length; i < documentNumber.MRNo.Length; i++)
                {
                    nxtStr = "0" + nxtStr;
                }
                documentNumber.MRNo = nxtStr;
                documentNumber.Save();
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + mrno;
        }

        [WebMethod]
        public static string SaveMR(Object MRInfo, ArrayList ProductInfo, ArrayList ConfirmedSalesInfo, ArrayList VendorInfo, ArrayList DeliveryInfo, ArrayList AttachmentInfo, string PageName, short CompanyId, short UserID, string CurrentLink, string CurrentMRStatus, string MRScheme, string MRRole)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
            IDictionary idict = (IDictionary)MRInfo;
            string mrno = "";
            string purchaser = "";
            string BudgetCode = "";
            string RefNo = "";
            string ProjectNo = "";
            string sourceid = "";
            string freightmode = "";
            string statusname = "";
            string statusid = "";
            string isconsole = "";
            string consoleDate = "";
            string mrdate = "";
            string requestorremarks = "";
            string orderreason = "";
            string vendorremarks = "";
            string purchasingremarks = "";
            string cancelledreason = "";
            string glcode = "";
            string requestorname = "";
            string requestor = "";
            string pmname = "";
            string pmuserid = "";
            string phname = "";
            string phuserid = "";
            string ph3name = "";
            string ph3userid = "";
            string ph5name = "";
            string ph5userid = "";
            string ismov = "";
            string mov = "";
            string intendedusage = "";
            string mrscheme = "";
            string taxtypeid = "";
            string taxrate = "";
            string discount = "";
            string exchangeRate = "";
            string dimensionL1 = "";
            string warehouse = "";
            string purchaserid = "";
            string othersRemarks = "";
            string dim1 = "";
            string dim2 = "";
            string dim3 = "";
            string dim4 = "";

            List<string> uniqueProductGroup = new List<string>();

            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "mrno")
                    mrno = idict[key].ToString();
                else if (key.ToString() == "purchaser")
                    purchaser = idict[key].ToString();
                else if (key.ToString() == "BudgetCode")
                    BudgetCode = idict[key].ToString();
                else if (key.ToString() == "RefNo")
                    RefNo = idict[key].ToString();
                else if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "sourceid")
                    sourceid = idict[key].ToString();
                else if (key.ToString() == "freightmode")
                    freightmode = idict[key].ToString();
                else if (key.ToString() == "statusname")
                    statusname = idict[key].ToString();
                else if (key.ToString() == "statusid")
                    statusid = idict[key].ToString();
                else if (key.ToString() == "isconsole")
                    isconsole = idict[key].ToString();
                else if (key.ToString() == "consoleDate")
                    consoleDate = idict[key].ToString();
                else if (key.ToString() == "mrdate")
                    mrdate = idict[key].ToString();
                else if (key.ToString() == "requestorremarks")
                    requestorremarks = idict[key].ToString();
                else if (key.ToString() == "orderreason")
                    orderreason = idict[key].ToString();
                else if (key.ToString() == "vendorremarks")
                    vendorremarks = idict[key].ToString();
                else if (key.ToString() == "purchasingremarks")
                    purchasingremarks = idict[key].ToString();
                else if (key.ToString() == "cancelledreason")
                    cancelledreason = idict[key].ToString();
                else if (key.ToString() == "glcode")
                    glcode = idict[key].ToString();
                else if (key.ToString() == "requestorname")
                    requestorname = idict[key].ToString();
                else if (key.ToString() == "requestor")
                    requestor = idict[key].ToString();
                else if (key.ToString() == "pmname")
                    pmname = idict[key].ToString();
                else if (key.ToString() == "pmuserid")
                    pmuserid = idict[key].ToString();
                else if (key.ToString() == "phname")
                    phname = idict[key].ToString();
                else if (key.ToString() == "phuserid")
                    phuserid = idict[key].ToString();
                else if (key.ToString() == "ph3name")
                    ph3name = idict[key].ToString();
                else if (key.ToString() == "ph3userid")
                    ph3userid = idict[key].ToString();
                else if (key.ToString() == "ph5name")
                    ph5name = idict[key].ToString();
                else if (key.ToString() == "ph5userid")
                    ph5userid = idict[key].ToString();
                else if (key.ToString() == "ismov")
                    ismov = idict[key].ToString();
                else if (key.ToString() == "mov")
                    mov = idict[key].ToString();
                else if (key.ToString() == "intendedusage")
                    intendedusage = idict[key].ToString();
                else if (key.ToString() == "mrscheme")
                    mrscheme = idict[key].ToString();
                else if (key.ToString() == "TaxTypeID")
                    taxtypeid = idict[key].ToString();
                else if (key.ToString() == "TaxRate")
                    taxrate = idict[key].ToString();
                else if (key.ToString() == "Discount")
                    discount = idict[key].ToString();
                else if (key.ToString() == "ExchangeRate")
                    exchangeRate = idict[key].ToString();
                else if (key.ToString() == "DimensionL1")
                    dimensionL1 = idict[key].ToString();
                else if (key.ToString() == "Warehouse")
                    warehouse = idict[key].ToString();
                else if (key.ToString() == "purchaserid")
                    purchaserid = idict[key].ToString();
                else if (key.ToString() == "othersRemarks")
                    othersRemarks = idict[key].ToString();
                else if (key.ToString() == "dim1")
                    dim1 = idict[key].ToString();
                else if (key.ToString() == "dim2")
                    dim2 = idict[key].ToString();
                else if (key.ToString() == "dim3")
                    dim3 = idict[key].ToString();
                else if (key.ToString() == "dim4")
                    dim4 = idict[key].ToString();
            }

            int totalUniqueCount = 1;

            if (ProductInfo != null && ProductInfo.Count > 0)
            {
                if (MRScheme == "Product")
                    UpdateProductTeam(CompanyId, ProductInfo);
                uniqueProductGroup = GroupByProductGroup(ProductInfo);
                totalUniqueCount = uniqueProductGroup.Count;
            }
            
            if (mrno == "")
            {
                DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(CompanyId, (short)DateTime.Now.Year);
                if (documentNumber == null) //If tbDocumentNumber does not exist
                {
                    documentNumber = new DocumentNumber();
                    documentNumber.CoyID = CompanyId;
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
                }

                mrno = "MR" + DateTime.Now.ToString("yy") + "" + documentNumber.MRNo.ToString();

                for (int j = 1; j <= totalUniqueCount; j++)
                {
                    if (totalUniqueCount > 1)
                        mrno = "MR" + DateTime.Now.ToString("yy") + "" + documentNumber.MRNo.ToString() + "-" + j.ToString();

                    GMSCore.Entity.MR mr = new GMSCore.Entity.MR();
                    mr.CoyID = CompanyId;
                    mr.MRNo = mrno;
                    mr.SourceID = sourceid;
                    mr.FreightMode = freightmode;
                    mr.ProjectNo = ProjectNo;
                    mr.RefNo = RefNo;
                    mr.BudgetCode = BudgetCode;
                    mr.StatusID = statusid;
                    mr.GLCode = glcode;
                    mr.OthersRemarks = othersRemarks;
                    mr.IntendedUsage = intendedusage;
                    mr.VendorRemarks = vendorremarks;
                    mr.PurchasingRemarks = purchasingremarks;
                    mr.RequestorRemarks = requestorremarks;
                    mr.OrderReason = orderreason;
                    mr.MRDate = GMSUtil.ToDate(mrdate);
                    mr.DimensionL1 = dimensionL1;
                    mr.Warehouse = warehouse;
                    mr.MRScheme = MRScheme;
                    mr.Dim1 = GMSUtil.ToShort(dim1);
                    mr.Dim2 = GMSUtil.ToShort(dim2);
                    mr.Dim3 = GMSUtil.ToShort(dim3);
                    mr.Dim4 = GMSUtil.ToShort(dim4);


                    if (isconsole == "Yes")
                    {
                        mr.IsConsole = true;
                        mr.ConsoleDate = GMSUtil.ToDate(consoleDate); 
                    }
                    else
                    {
                        mr.IsConsole = false;
                        mr.ConsoleDate = GMSUtil.ToDate("");
                    }

                    if (ismov == "Yes")
                    {
                        mr.IsMOV = true;
                        mr.MOV = GMSUtil.ToDecimal(mov);
                    }
                    else
                    {
                        mr.IsMOV = false;
                        mr.MOV = 0;
                    }
                    mr.Requestor = GMSUtil.ToShort(requestor);
                    mr.CreatedBy = UserID;
                    mr.CreatedDate = DateTime.Now;
                    mr.TaxTypeID = taxtypeid;
                    mr.TaxRate = GMSUtil.ToDecimal(taxrate.Trim().TrimEnd('%')) / 100;
                    mr.Discount = GMSUtil.ToDecimal(discount);
                    if (exchangeRate.ToString() == "")
                        mr.ExchangeRate = GMSUtil.ToDecimal(1);
                    else
                        mr.ExchangeRate = GMSUtil.ToDecimal(exchangeRate);
                    // Update Product
                    if (ProductInfo.Count > 0)
                        SaveProduct(mrno, uniqueProductGroup[j - 1].ToString(), ProductInfo, true, mr, mr.SourceID, PageName, CompanyId, MRRole, UserID, CurrentLink, CurrentMRStatus, MRScheme);
                    // Update Confirmed Sales
                    if (ConfirmedSalesInfo.Count > 0)
                        SaveConfirmedSales(mrno, ConfirmedSalesInfo, true, PageName, CompanyId, MRRole, UserID, CurrentLink, CurrentMRStatus, MRScheme);
                    // Update Vendor
                    if (VendorInfo.Count > 0)
                        SaveVendor(mrno, VendorInfo, true, PageName, CompanyId, MRRole, UserID, CurrentLink, CurrentMRStatus, MRScheme);
                    // Update Delivery
                    if (DeliveryInfo.Count > 0)
                        SaveDelivery(mrno, DeliveryInfo, PageName, CompanyId, MRRole, UserID, CurrentMRStatus, MRScheme);
                    //Update Attachment
                    if (AttachmentInfo.Count > 0)
                        SaveAttachment(mrno, AttachmentInfo, PageName, CompanyId, MRRole, UserID, CurrentMRStatus, MRScheme);

                    if (MRScheme != "Product")
                    {
                        mr.PMUserId = GMSUtil.ToShort(pmuserid);
                        mr.PHUserId = GMSUtil.ToShort(phuserid);
                        mr.PH3UserId = GMSUtil.ToShort(ph3userid);
                        mr.PH5UserId = GMSUtil.ToShort(ph5userid);
                    }
                    mr.Save();
                    new GMSGeneralDALC().InsertMRLog(CompanyId, mrno, UserID, loginUserOrAlternateParty, "CREATE-AddEditMaterial");

                }

                string nxtStr = ((short)(short.Parse(documentNumber.MRNo) + 1)).ToString();
                for (int i = nxtStr.Length; i < documentNumber.MRNo.Length; i++)
                {
                    nxtStr = "0" + nxtStr;
                }

                documentNumber.MRNo = nxtStr;
                documentNumber.Save();

                if (totalUniqueCount == 1)
                    return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + mrno;
                else
                    return "MaterialRequisitionSearch.aspx?CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId;
            }
            else
            {
                bool changePMPH = false;
                GMSCore.Entity.MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, mrno);
                mr.SourceID = sourceid;
                mr.FreightMode = freightmode;
                mr.ProjectNo = ProjectNo;
                mr.RefNo = RefNo;
                mr.BudgetCode = BudgetCode;
                mr.StatusID = statusid;
                mr.GLCode = glcode;
                mr.IntendedUsage = intendedusage;
                mr.VendorRemarks = vendorremarks;
                mr.PurchasingRemarks = purchasingremarks;
                mr.RequestorRemarks = requestorremarks;
                mr.OrderReason = orderreason;
                mr.MRDate = GMSUtil.ToDate(mrdate);
                mr.OthersRemarks = othersRemarks;
                mr.Dim1 = GMSUtil.ToShort(dim1);
                mr.Dim2 = GMSUtil.ToShort(dim2);
                mr.Dim3 = GMSUtil.ToShort(dim3);
                mr.Dim4 = GMSUtil.ToShort(dim4);
                if (isconsole == "Yes")
                {
                    mr.IsConsole = true;
                    mr.ConsoleDate = GMSUtil.ToDate(consoleDate);
                }
                else
                {
                    mr.IsConsole = false;
                    mr.ConsoleDate = GMSUtil.ToDate("");
                }

                if (ismov == "Yes")
                {
                    mr.IsMOV = true;
                    mr.MOV = GMSUtil.ToDecimal(mov);
                }
                else
                {
                    mr.IsMOV = false;
                    mr.MOV = 0;
                }

                if (mr.PMUserId != GMSUtil.ToShort(pmuserid))
                    changePMPH = true;

                if (mr.PHUserId != GMSUtil.ToShort(phuserid))
                    changePMPH = true;

                if (mr.PH3UserId != GMSUtil.ToShort(ph3userid))
                    changePMPH = true;

                mr.Requestor = GMSUtil.ToShort(requestor);
                mr.PMUserId = GMSUtil.ToShort(pmuserid);
                mr.PHUserId = GMSUtil.ToShort(phuserid);
                mr.PH3UserId = GMSUtil.ToShort(ph3userid);
                mr.PH5UserId = GMSUtil.ToShort(ph5userid);
                mr.ModifiedBy = loginUserOrAlternateParty;
                mr.ModifiedDate = DateTime.Now;
                mr.Purchaser = purchaserid;
                mr.TaxTypeID = taxtypeid;
                mr.TaxRate = GMSUtil.ToDecimal(taxrate.Trim().TrimEnd('%')) / 100;
                mr.Discount = GMSUtil.ToDecimal(discount);
                if (exchangeRate.ToString() == "")
                    mr.ExchangeRate = GMSUtil.ToDecimal(1);
                else
                    mr.ExchangeRate = GMSUtil.ToDecimal(exchangeRate);
                //mr.Warehouse = warehouse;                
                mr.Save();

                // Update Product
                SaveProduct(mrno, "", ProductInfo, false, mr, mr.SourceID, PageName, CompanyId, MRRole, UserID, CurrentLink, CurrentMRStatus, MRScheme);
                // Update Confirmed Sales
                SaveConfirmedSales(mrno, ConfirmedSalesInfo, false, PageName, CompanyId, MRRole, UserID, CurrentLink, CurrentMRStatus, MRScheme);
                // Update Vendor
                SaveVendor(mrno, VendorInfo, false, PageName, CompanyId, MRRole, UserID, CurrentLink, CurrentMRStatus, MRScheme);
                // Update Delivery
                SaveDelivery(mrno, DeliveryInfo, PageName, CompanyId, MRRole, UserID, CurrentMRStatus, MRScheme);
                //Update Attachment
                SaveAttachment(mrno, AttachmentInfo, PageName, CompanyId, MRRole, UserID, CurrentMRStatus, MRScheme);

                new GMSGeneralDALC().InsertMRLog(CompanyId, mrno, UserID, loginUserOrAlternateParty, "UPDATE-AddEditMaterial");
                int levelID = GetMRFormApprovalLevelID(CompanyId, mrno, loginUserOrAlternateParty);

                if (mr.StatusID == "X" || mr.StatusID == "D" || mr.StatusID == "R" || mr.StatusID == "F")
                {
                    if (changePMPH == true)
                        new ApprovalActivity().ReSubmitMR(CompanyId, loginUserOrAlternateParty, mrno, GMSUtil.ToShort(mr.PMUserId), GMSUtil.ToShort(mr.PHUserId), GMSUtil.ToShort(mr.PH3UserId), levelID, "Header updated.", "R", true, UserID);
                    else
                        new ApprovalActivity().ReSubmitMR(CompanyId, loginUserOrAlternateParty, mrno, GMSUtil.ToShort(mr.PMUserId), GMSUtil.ToShort(mr.PHUserId), GMSUtil.ToShort(mr.PH3UserId), levelID, "Header updated.", "R", false, UserID);

                }
                return "success";
                //return PageName + "&CurrentLink=" + currentLink + "&CoyID=" + CompanyId + "&MRNo=" + mrno;
            }


        }

        public static string Resubmission(string section, string alertMessage, string anchor, short CompanyId, string MRNo, short UserID, string CurrentMRStatus)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
            string message = "";
            string activeTab = "0";
            if (section == "Confirmed Sales updated.")
                activeTab = "0";
            else if (section == "Vendor updated.")
                activeTab = "1";
            else if (section == "Product updated.")
                activeTab = "2";
            else if (section == "Delivery updated.")
                activeTab = "3";

            DataSet ds = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if ((CurrentMRStatus != ds.Tables[0].Rows[0]["StatusID"].ToString()) && ds.Tables[0].Rows[0]["StatusID"].ToString() != "P")
                {
                    message = "Data inconsistency found in the page, please refresh the page.";
                }
            }


            /*
            Principle of Five:
            ---------------------
            R - Header updated.
            R - Confirmed Sales updated.
            U - Vendor updated. If the MR is Rejected and user updated the vendor information, insert R instead of U.  
            R - Product updated.  
            U- Delivery updated.               
            */
            string status = "";
            string action = "U";
            if ((CurrentMRStatus == "A") || (CurrentMRStatus == "P"))
                action = "U";
            else if ((CurrentMRStatus == "R") || (CurrentMRStatus == "F"))
            {
                if ((section == "Delivery updated.") || ((section == "Vendor updated.") && (CurrentMRStatus == "F")))
                    action = "U";
                else
                    action = "R";
            }

            if ((CurrentMRStatus == "A") || (CurrentMRStatus == "R") || (CurrentMRStatus == "F") || (CurrentMRStatus == "P"))
            {
                int levelID = GetMRFormApprovalLevelID(CompanyId, MRNo, loginUserOrAlternateParty);
                status = new ApprovalActivity().ReSubmitMR(CompanyId, loginUserOrAlternateParty, MRNo, GMSUtil.ToShort(ds.Tables[0].Rows[0]["PMUserId"].ToString()), GMSUtil.ToShort(ds.Tables[0].Rows[0]["PHUserId"].ToString()), GMSUtil.ToShort(ds.Tables[0].Rows[0]["PH3UserId"].ToString()), levelID, section, action, false, UserID);
            }

            return message;

        }

        [WebMethod]
        public static string DeleteConfirmedSales(string MRNo, ArrayList ConfirmedSalesInfo, string PageName, string MRRole, short CompanyId, short UserID, string CurrentLink, string CurrentMRStatus)
        {
            DataSet ds = new DataSet();
            IDictionary idict;
            string FileID = "";
            bool StatusChange = false;
            try {
                for (int i = 0; i < ConfirmedSalesInfo.Count; ++i)
                {
                    idict = (IDictionary)ConfirmedSalesInfo[i];
                    foreach (object key in idict.Keys)
                    {
                        if (key.ToString() == "FileID")
                            FileID = idict[key].ToString();
                    }
                    using (TransactionScope tran = new TransactionScope())
                    {
                        ResultType result = new MRActivity().DeleteMRConfirmedSales(CompanyId, FileID);
                        switch (result)
                        {
                            case ResultType.Ok:
                                string message = Resubmission("Confirmed Sales updated.", "Record deleted successfully!", "", CompanyId, MRNo, UserID, CurrentMRStatus);
                                if (message == "")
                                    tran.Complete();
                                else
                                    throw new HttpException(500, message);
                                break;
                            default:
                                throw new HttpException(500, "Processing error of type : " + result.ToString());
                        }
                    }

                    //StatusChange = ResetApproval(CompanyId, MRNo, MRRole, UserID);
                }
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            ds.Clear();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref ds);

            if (StatusChange)
                return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + MRNo;
            else
                return "success";

        }


        [WebMethod]
        public static string SaveConfirmedSales(string MRNo, ArrayList ConfirmedSalesInfo, bool IsNew, string PageName, short CompanyId, string MRRole, short UserID, string CurrentLink, string CurrentMRStatus, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
            DataSet ds = new DataSet();
            IDictionary idict;
            string FileID = "";
            string CustomerAccountCode = "";
            string CustomerName = "";
            string RequiredDate = "";
            string SONo = "";
            string SODate = "";
            string RowStatus = "";
            bool Changes = false;
            bool StatusChange = false;

            try {
                if (ConfirmedSalesInfo != null)
                {
                    for (int i = 0; i < ConfirmedSalesInfo.Count; ++i)
                    {
                        idict = (IDictionary)ConfirmedSalesInfo[i];
                        foreach (object key in idict.Keys)
                        {

                            if (key.ToString() == "FileID")
                                FileID = idict[key].ToString();
                            else if (key.ToString() == "CustomerAccountCode")
                                CustomerAccountCode = idict[key].ToString();
                            else if (key.ToString() == "CustomerAccountName")
                                CustomerName = idict[key].ToString();
                            else if (key.ToString() == "RequiredDate")
                                RequiredDate = idict[key].ToString();
                            else if (key.ToString() == "SONo")
                                SONo = idict[key].ToString();
                            else if (key.ToString() == "SODate")
                                SODate = idict[key].ToString();
                            else if (key.ToString() == "RowStatus")
                                RowStatus = idict[key].ToString();
                        }

                        if (RowStatus == "Insert")
                        {
                            Changes = true;
                            GMSCore.Entity.MRAttachment ma = new GMSCore.Entity.MRAttachment();
                            ma.CoyID = CompanyId;
                            ma.MRNo = MRNo;
                            ma.CustomerAccountCode = CustomerAccountCode;
                            ma.CustomerName = CustomerName;
                            ma.RequiredDate = GMSUtil.ToDate(RequiredDate);
                            ma.SONo = SONo;
                            ma.SODate = GMSUtil.ToDate(SODate);
                            ma.CreatedBy = loginUserOrAlternateParty;
                            ma.CreatedDate = DateTime.Now;
                            ma.Save();
                        }
                        else if (RowStatus == "Modify")
                        {
                            Changes = true;
                            GMSCore.Entity.MRAttachment ma = new MRActivity().RetrieveConfirmedSalesByID(CompanyId, FileID);
                            ma.CustomerAccountCode = CustomerAccountCode;
                            ma.CustomerName = CustomerName;
                            ma.RequiredDate = GMSUtil.ToDate(RequiredDate);
                            ma.SONo = SONo;
                            ma.SODate = GMSUtil.ToDate(SODate);
                            ma.ModifiedBy = loginUserOrAlternateParty;
                            ma.ModifiedDate = DateTime.Now;
                            ma.Save();
                        }
                    }

                    if (!IsNew && Changes)
                    {
                        //StatusChange = ResetApproval(CompanyId, MRNo, MRRole, UserID);
                        using (TransactionScope tran = new TransactionScope())
                        {
                            string message = Resubmission("Confirmed Sales updated.", "Record added/updated successfully!", "", CompanyId, MRNo, UserID, CurrentMRStatus);
                            if (message == "")
                                tran.Complete();
                            else
                                throw new HttpException(500, message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }

            ds.Clear();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref ds);

            if (StatusChange)
                return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + MRNo;
            else
                return "success";
        }

        [WebMethod]
        public static string DeleteVendor(string MRNo, ArrayList VendorInfo, string PageName, string MRRole, short CompanyId, short UserID, string CurrentLink, string CurrentMRStatus)
        {
            DataSet ds = new DataSet();
            IDictionary idict;
            string VendorID = "";
            bool StatusChange = false;
            try {
                for (int i = 0; i < VendorInfo.Count; ++i)
                {
                    idict = (IDictionary)VendorInfo[i];
                    foreach (object key in idict.Keys)
                    {
                        if (key.ToString() == "VendorID")
                            VendorID = idict[key].ToString();
                    }

                    using (TransactionScope tran = new TransactionScope())
                    {
                        ResultType result = new MRActivity().DeleteMRVendor(CompanyId, VendorID);
                        switch (result)
                        {
                            case ResultType.Ok:
                                string message = Resubmission("Vendor updated.", "Record deleted successfully!", "", CompanyId, MRNo, UserID, CurrentMRStatus);
                                if (message == "")
                                    tran.Complete();
                                else
                                    throw new HttpException(500, message);
                                break;
                            default:
                                throw new HttpException(500, "Processing error of type : " + result.ToString());
                        }
                    }
                    //StatusChange = ResetApproval(CompanyId, MRNo, MRRole, UserID);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            ds.Clear();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref ds);

            if (StatusChange)
                return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + MRNo;
            else
                return "success";
        }

        [WebMethod]
        public static string SaveVendor(string MRNo, ArrayList VendorInfo, bool IsNew, string PageName, short CompanyId, string MRRole, short UserID, string CurrentLink, string CurrentMRStatus, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
            DataSet ds = new DataSet();
            IDictionary idict;
            string VendorID = "";
            string VendorName = "";
            string VendorContact = "";
            string VendorTel = "";
            string VendorFax = "";
            string VendorEmail = "";
            string RowStatus = "";
            string VendorCode = "";
            bool Changes = false;
            bool StatusChange = false;

            try
            {
                if (VendorInfo != null)
                {
                    for (int i = 0; i < VendorInfo.Count; ++i)
                    {
                        idict = (IDictionary)VendorInfo[i];
                        foreach (object key in idict.Keys)
                        {
                            if (key.ToString() == "VendorID")
                                VendorID = idict[key].ToString();
                            else if (key.ToString() == "VendorCode")
                                VendorCode = idict[key].ToString();
                            else if (key.ToString() == "VendorName")
                                VendorName = idict[key].ToString();
                            else if (key.ToString() == "VendorContact")
                                VendorContact = idict[key].ToString();
                            else if (key.ToString() == "VendorTel")
                                VendorTel = idict[key].ToString();
                            else if (key.ToString() == "VendorFax")
                                VendorFax = idict[key].ToString();
                            else if (key.ToString() == "VendorEmail")
                                VendorEmail = idict[key].ToString();
                            else if (key.ToString() == "RowStatus")
                                RowStatus = idict[key].ToString();
                        }

                        if (RowStatus == "Insert")
                        {
                            Changes = true;
                            GMSCore.Entity.MRVendor mv = new GMSCore.Entity.MRVendor();
                            mv.CoyID = CompanyId;
                            mv.MRNo = MRNo;
                            mv.VendorName = VendorName;
                            mv.VendorCode = VendorCode;
                            mv.VendorContact = VendorContact;
                            mv.VendorTel = VendorTel;
                            mv.VendorFax = VendorFax;
                            mv.VendorEmail = VendorEmail;
                            mv.CreatedBy = loginUserOrAlternateParty;
                            mv.CreatedDate = DateTime.Now;
                            mv.Save();

                            if (mv.VendorCode != "")
                            {
                                A21Account acct = A21Account.RetrieveByKey(CompanyId, VendorCode);
                                if (acct != null && acct.AccountType == "S")
                                {
                                    TaxType rate = TaxType.RetrieveByKey(CompanyId, acct.GSTType);  
                                    GMSCore.Entity.MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);                                    
                                    mr.TaxTypeID = acct.GSTType;
                                    if (rate != null)
                                        mr.TaxRate = rate.TaxRate;
                                    mr.Save();
                                }
                            }
                        }
                        else if (RowStatus == "Modify")
                        {
                            GMSCore.Entity.MRVendor mv = new MRActivity().RetrieveVendorByID(CompanyId, VendorID);
                            if (mv != null)
                            {
                                Changes = true;
                                mv.VendorName = VendorName;
                                mv.VendorCode = VendorCode;
                                mv.VendorContact = VendorContact;
                                mv.VendorTel = VendorTel;
                                mv.VendorFax = VendorFax;
                                mv.VendorEmail = VendorEmail;
                                mv.ModifiedBy = loginUserOrAlternateParty;
                                mv.ModifiedDate = DateTime.Now;
                                mv.Save();

                                if (mv.VendorCode != "")
                                {
                                    A21Account acct = A21Account.RetrieveByKey(CompanyId, VendorCode);
                                    if (acct != null && acct.AccountType == "S")
                                    {
                                        TaxType rate = TaxType.RetrieveByKey(CompanyId, acct.GSTType);
                                        GMSCore.Entity.MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);
                                        mr.TaxTypeID = acct.GSTType;
                                        if (rate != null)
                                            mr.TaxRate = rate.TaxRate;
                                        mr.Save();
                                    }
                                }

                            }
                        }                        

                        if (MRScheme == "Product")
                        {
                            //insert manual vendor to Supplier List for auto insertion when MR Split
                            DataSet dsProductGroupCode = new DataSet();
                            new GMSGeneralDALC().GetProductGroupCodeByMRNo(CompanyId, MRNo, ref dsProductGroupCode);
                            if (dsProductGroupCode != null && dsProductGroupCode.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < dsProductGroupCode.Tables[0].Rows.Count; k++)
                                {
                                    IList<MRSupplier> lstSupplier = new MRActivity().RetrieveMRSupplierByCoyIDVendorDetails(CompanyId, dsProductGroupCode.Tables[0].Rows[k]["PRODUCTGROUPCODE"].ToString(), VendorName);
                                    if (lstSupplier.Count == 0)
                                    {
                                        if (VendorName != "")
                                        {
                                            GMSCore.Entity.MRSupplier ms = new GMSCore.Entity.MRSupplier();
                                            ms.CoyID = CompanyId;
                                            ms.ProductGroupCode = dsProductGroupCode.Tables[0].Rows[k]["PRODUCTGROUPCODE"].ToString();
                                            ms.AccountName = VendorName;
                                            ms.Contact = VendorName;
                                            ms.Tel = VendorTel;
                                            ms.Fax = VendorFax;
                                            ms.Email = VendorEmail;
                                            ms.CreatedBy = loginUserOrAlternateParty;
                                            ms.CreatedDate = DateTime.Now;
                                            ms.Save();
                                        }
                                    }
                                    else
                                    {
                                        foreach (MRSupplier supplier in lstSupplier)
                                        {
                                            supplier.Contact = VendorContact;
                                            supplier.Tel = VendorTel;
                                            supplier.Fax = VendorFax;
                                            supplier.Email = VendorEmail;
                                            supplier.ModifiedBy = loginUserOrAlternateParty;
                                            supplier.ModifiedDate = DateTime.Now;
                                            supplier.Save();
                                        }

                                    }
                                }
                            }
                        }
                    }

                    if (!IsNew && Changes)
                    {
                        //StatusChange = ResetApproval(CompanyId, MRNo, MRRole, UserID);
                        using (TransactionScope tran = new TransactionScope())
                        {
                            string message = Resubmission("Vendor updated.", "Record added/updated successfully!", "", CompanyId, MRNo, UserID, CurrentMRStatus);
                            if (message == "")
                                tran.Complete();
                            else
                                throw new HttpException(500, message);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            ds.Clear();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref ds);

            if (StatusChange)
                return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + MRNo;
            else
                return "success";
        }

        [WebMethod]
        public static string DeleteDelivery(string MRNo, ArrayList DeliveryInfo, string PageName, string MRRole, short CompanyId, short UserID, string CurrentMRStatus)
        {
            IDictionary idict;
            DataSet ds = new DataSet();
            string DeliveryNo = "";
            try {
                for (int i = 0; i < DeliveryInfo.Count; ++i)
                {
                    idict = (IDictionary)DeliveryInfo[i];
                    foreach (object key in idict.Keys)
                    {
                        if (key.ToString() == "DeliveryNo")
                            DeliveryNo = idict[key].ToString();
                    }

                }
                using (TransactionScope tran = new TransactionScope())
                {
                    ResultType result = new MRActivity().DeleteMRDelivery(CompanyId, DeliveryNo);
                    switch (result)
                    {
                        case ResultType.Ok:
                            IList<MRDelivery> lstMRDelivery = null;
                            lstMRDelivery = new MRActivity().RetrieveMRDeliveryByMRNo(CompanyId, MRNo);
                            if (lstMRDelivery.Count == 0)
                            { // Update MRStatus to Approved if no PO in Delivery
                                GMSCore.Entity.MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);
                                CurrentMRStatus = "A";
                                mr.StatusID = "A";
                                mr.Save();

                            }
                            string message = Resubmission("Delivery updated.", "Record deleted successfully!", "", CompanyId, MRNo, UserID, CurrentMRStatus);
                            if (message == "")
                                tran.Complete();
                            else
                                throw new HttpException(500, message);
                            break;
                        default:
                            throw new HttpException(500, "Processing error of type : " + result.ToString());
                    }
                }
                //updateMRStatus(CompanyId, UserID, MRNo);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            ds.Clear();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref ds);
            return "success";
            //return PageName + "&CurrentLink=" + currentLink + "&CoyID=" + CompanyId + "&MRNo=" + mrno;
        }


        [WebMethod]
        public static string SaveDelivery(string MRNo, ArrayList DeliveryInfo, string PageName, short CompanyId, string MRRole, short UserID, string CurrentMRStatus, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
            DataSet ds = new DataSet();
            IDictionary idict;
            string DeliveryNo = "";
            string PONo = "";
            string ETD = "";
            string ETA = "";
            string CRD = "";
            string RowStatus = "";
            string message = "";

            try
            {
                if (DeliveryInfo != null)
                {
                    for (int i = 0; i < DeliveryInfo.Count; ++i)
                    {
                        idict = (IDictionary)DeliveryInfo[i];
                        foreach (object key in idict.Keys)
                        {
                            if (key.ToString() == "DeliveryNo")
                                DeliveryNo = idict[key].ToString();
                            else if (key.ToString() == "PONo")
                                PONo = idict[key].ToString();
                            else if (key.ToString() == "ETD")
                                ETD = idict[key].ToString();
                            else if (key.ToString() == "ETA")
                                ETA = idict[key].ToString();
                            else if (key.ToString() == "CRD")
                                CRD = idict[key].ToString();
                            else if (key.ToString() == "RowStatus")
                                RowStatus = idict[key].ToString();

                        }

                        if (RowStatus == "Insert")
                        {
                            GMSCore.Entity.MRDelivery existMRDelivery = new MRActivity().RetrieveMRDeliveryPOByMRNo(CompanyId, MRNo, PONo);
                            if (existMRDelivery == null)
                            {
                                GMSCore.Entity.MRDelivery md = new GMSCore.Entity.MRDelivery();
                                md.CoyID = CompanyId;
                                md.MRNo = MRNo;
                                md.PONo = PONo;
                                md.ETD = GMSUtil.ToDate(ETD);
                                md.ETA = GMSUtil.ToDate(ETA);
                                md.CRD = GMSUtil.ToDate(CRD);
                                md.CreatedBy = loginUserOrAlternateParty;
                                md.CreatedDate = DateTime.Now;
                                md.Save();

                                GMSCore.Entity.MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);
                                if (mr.StatusID.ToString() != "C" && mr.StatusID.ToString() != "X")
                                {
                                    mr.StatusID = "P";
                                    mr.ModifiedBy = loginUserOrAlternateParty;
                                    mr.ModifiedDate = DateTime.Now;
                                    mr.Save();
                                }
                            }
                            else
                            {
                                message = "This PO number exist, Please refresh the MR.";
                            }
                        }
                        else if (RowStatus == "Modify")
                        {
                            GMSCore.Entity.MRDelivery md = new MRActivity().RetrieveDeliveryByID(CompanyId, DeliveryNo);
                            md.ETD = GMSUtil.ToDate(ETD);
                            md.ETA = GMSUtil.ToDate(ETA);
                            md.CRD = GMSUtil.ToDate(CRD);
                            md.ModifiedBy = loginUserOrAlternateParty;
                            md.ModifiedDate = DateTime.Now;
                            md.Save();
                        }
                    }
                    if (message == "")
                    {
                        using (TransactionScope tran = new TransactionScope())
                        {
                            message = Resubmission("Delivery updated.", "Record added/updated successfully!", "", CompanyId, MRNo, UserID, CurrentMRStatus);
                            if (message == "")
                                tran.Complete();
                            else
                                throw new HttpException(500, message);


                        }
                    }
                    //updateMRStatus(CompanyId, UserID, MRNo);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            ds.Clear();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref ds);
            return "success";

            //return PageName + "&CurrentLink=" + currentLink + "&CoyID=" + CompanyId + "&MRNo=" + mrno;
        }

        public static void updateMRStatus(short CompanyId, short UserID, string MRNo)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
            IList<MRDelivery> lstMRDelivery = null;
            lstMRDelivery = new MRActivity().RetrieveMRDeliveryByMRNo(CompanyId, MRNo);
            GMSCore.Entity.MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);
            if (lstMRDelivery.Count == 0 && mr.StatusID == "P")
            {
                mr.StatusID = "A";
                mr.ModifiedBy = loginUserOrAlternateParty;
                mr.ModifiedDate = DateTime.Now;
                mr.Save();
            }
            else if (lstMRDelivery.Count > 0 && mr.StatusID == "A")
            {
                mr.StatusID = "P";
                mr.ModifiedBy = loginUserOrAlternateParty;
                mr.ModifiedDate = DateTime.Now;
                mr.Save();
            }
        }


        [WebMethod]
        public static string DeleteAttachment(string MRNo, ArrayList AttachmentInfo, string MRRole, short CompanyId, short UserID, string CurrentMRStatus)
        {
            DataSet ds = new DataSet();
            IDictionary idict;
            string FileID = "";
            try {
                for (int i = 0; i < AttachmentInfo.Count; ++i)
                {
                    idict = (IDictionary)AttachmentInfo[i];
                    foreach (object key in idict.Keys)
                    {
                        if (key.ToString() == "FileID")
                            FileID = idict[key].ToString();
                    }
                    using (TransactionScope tran = new TransactionScope())
                    {
                        ResultType result = new MRActivity().DeleteMRAttachment(CompanyId, FileID);
                        switch (result)
                        {
                            case ResultType.Ok:
                                tran.Complete();
                                break;
                            default:
                                throw new HttpException(500, "Processing error of type : " + result.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            ds.Clear();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref ds);
            return "success";
        }

        [WebMethod]
        public static string SaveAttachment(string MRNo, ArrayList AttachmentInfo, string PageName, short CompanyId, string MRRole, short UserID, string CurrentMRStatus, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
            DataSet ds = new DataSet();
            IDictionary idict;
            string FileID = "";
            string FileDisplayName = "";
            string FileName = "";
            string FileNameEncrypted = "";
            string RowStatus = "";
            string DocumentCategory = "";

            try
            {
                if (AttachmentInfo != null)
                {
                    for (int i = 0; i < AttachmentInfo.Count; ++i)
                    {
                        idict = (IDictionary)AttachmentInfo[i];
                        foreach (object key in idict.Keys)
                        {
                            if (key.ToString() == "FileID")
                                FileID = idict[key].ToString();
                            else if (key.ToString() == "FileDisplayName")
                                FileDisplayName = idict[key].ToString();
                            else if (key.ToString() == "FileName")
                                FileName = idict[key].ToString();
                            else if (key.ToString() == "FileNameEncrypted")
                                FileNameEncrypted = idict[key].ToString();
                            else if (key.ToString() == "FileNameEncrypted")
                                FileNameEncrypted = idict[key].ToString();
                            else if (key.ToString() == "DocumentCategory")
                                DocumentCategory = idict[key].ToString();
                            else if (key.ToString() == "RowStatus")
                                RowStatus = idict[key].ToString();
                        }

                        if (RowStatus == "Insert")
                        {
                            GMSCore.Entity.MRAdditionalAttachment ma = new GMSCore.Entity.MRAdditionalAttachment();
                            ma.CoyID = CompanyId;
                            ma.MRNo = MRNo;
                            ma.FileDisplayName = FileDisplayName;
                            ma.FileName = FileName;
                            ma.FileNameEncrypted = FileName;
                            ma.DocumentCategory = DocumentCategory;
                            ma.CreatedBy = loginUserOrAlternateParty;
                            ma.CreatedDate = DateTime.Now;
                            ma.Save();

                        }
                        else if (RowStatus == "Modify")
                        {
                            GMSCore.Entity.MRAdditionalAttachment ma = new MRActivity().RetrieveMRAttachementByID(CompanyId, FileID);
                            ma.FileDisplayName = FileDisplayName;
                            ma.DocumentCategory = DocumentCategory;
                            ma.ModifiedBy = loginUserOrAlternateParty;
                            ma.ModifiedDate = DateTime.Now;
                            ma.Save();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            ds.Clear();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref ds);
            return "success";
        }

        [WebMethod]
        public static string DeleteProduct(string MRNo, ArrayList ProductInfo, string PageName, string MRRole, short CompanyId, short UserID, string CurrentLink, string CurrentMRStatus)
        {
            DataSet ds = new DataSet();
            IDictionary idict;
            string DetailNo = "";
            bool StatusChange = false;
            try
            {
                for (int i = 0; i < ProductInfo.Count; ++i)
                {
                    idict = (IDictionary)ProductInfo[i];
                    foreach (object key in idict.Keys)
                    {
                        if (key.ToString() == "DetailNo")
                            DetailNo = idict[key].ToString();
                    }

                    using (TransactionScope tran = new TransactionScope())
                    {
                        ResultType result = new MRActivity().DeleteMRProduct(CompanyId, DetailNo);
                        switch (result)
                        {
                            case ResultType.Ok:
                                string message = Resubmission("Product updated.", "Record deleted successfully!", "", CompanyId, MRNo, UserID, CurrentMRStatus);
                                if (message == "")
                                    tran.Complete();
                                else
                                    throw new HttpException(500, message);
                                break;
                            default:
                                throw new HttpException(500, "Processing error of type : " + result.ToString());
                        }
                    }
                    //StatusChange = ResetApproval(CompanyId, MRNo, MRRole, UserID);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            ds.Clear();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref ds);

            if (StatusChange)
                return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + MRNo;
            else
                return "success";

        }

        [WebMethod]
        public static string SaveProduct(string MRNo, string uniqueProductGroup, ArrayList ProductInfo, bool IsNew, MR mr, string Source, string PageName, short CompanyId, string MRRole, short UserID, string CurrentLink, string CurrentMRStatus, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);
            DataSet ds = new DataSet();
            IDictionary idict;
            string DetailNo = "";
            string ProdCode = "";
            string NewProdCode = "";
            string ProdName = "";
            string UOM = "";
            string OnHandQty = "";
            string OnOrderQty = "";
            string OnPOQty = "";
            string ConfirmedOrderQty = "";
            string ForStockingQty = "";
            string OrderQty = "";
            string UnitSellingPrice = "";
            string SellingCurrency = "";
            string UnitPurchasePrice = "";
            string PurchaseCurrency = "";
            string Reason = "";
            string RowStatus = "";
            string KeyToSplit = "";
            string Approver1ID = "";
            string Approver2ID = "";
            string Approver3ID = "";
            string Approver4ID = "";
            string ProductGroupCode = "";
            bool Changes = false;
            bool StatusChange = false;
            bool AllowInsert = true;
            string RecipeNo = "";
            try
            {
                if (ProductInfo != null)
                {
                    bool insertedVendor = false;
                    for (int i = 0; i < ProductInfo.Count; ++i)
                    {
                        idict = (IDictionary)ProductInfo[i];
                        RowStatus = "";
                        foreach (object key in idict.Keys)
                        {
                            if (key.ToString() == "DetailNo")
                                DetailNo = idict[key].ToString();
                            else if (key.ToString() == "ProdCode")
                                ProdCode = idict[key].ToString();
                            else if (key.ToString() == "NewProdCode")
                                NewProdCode = idict[key].ToString();
                            else if (key.ToString() == "ProdName")
                                ProdName = idict[key].ToString();
                            else if (key.ToString() == "UOM")
                                UOM = idict[key].ToString();
                            else if (key.ToString() == "OnHandQty")
                                OnHandQty = idict[key].ToString();
                            else if (key.ToString() == "OnOrderQty")
                                OnOrderQty = idict[key].ToString();
                            else if (key.ToString() == "OnPOQty")
                                OnPOQty = idict[key].ToString();
                            else if (key.ToString() == "ConfirmedOrderQty")
                                ConfirmedOrderQty = idict[key].ToString();
                            else if (key.ToString() == "ForStockingQty")
                                ForStockingQty = idict[key].ToString();
                            else if (key.ToString() == "OrderQty")
                                OrderQty = idict[key].ToString();
                            else if (key.ToString() == "UnitSellingPrice")
                                UnitSellingPrice = idict[key].ToString();
                            else if (key.ToString() == "SellingCurrency")
                                SellingCurrency = idict[key].ToString();
                            else if (key.ToString() == "UnitPurchasePrice")
                                UnitPurchasePrice = idict[key].ToString();
                            else if (key.ToString() == "PurchaseCurrency")
                                PurchaseCurrency = idict[key].ToString();
                            else if (key.ToString() == "ProductReason")
                                Reason = idict[key].ToString();
                            else if (key.ToString() == "Approver1ID")
                                Approver1ID = idict[key].ToString();
                            else if (key.ToString() == "Approver2ID")
                                Approver2ID = idict[key].ToString();
                            else if (key.ToString() == "Approver3ID")
                                Approver3ID = idict[key].ToString();
                            else if (key.ToString() == "Approver4ID")
                                Approver4ID = idict[key].ToString();
                            else if (key.ToString() == "KeyToSplit")
                                KeyToSplit = idict[key].ToString();
                            else if (key.ToString() == "ProductGroupCode")
                                ProductGroupCode = idict[key].ToString();
                            else if (key.ToString() == "RowStatus")
                                RowStatus = idict[key].ToString();
                            else if (key.ToString() == "RecipeNo")
                                RecipeNo = idict[key].ToString();
                        }

                        if ((IsNew && KeyToSplit == uniqueProductGroup) || !IsNew)
                        {
                            if (RowStatus == "Insert")
                            {
                                if (!IsNew && MRNo != "" && MRScheme == "Product" && ProdCode != "00000000000")
                                {
                                    DataSet ds1 = new DataSet();
                                    GMSCore.Entity.MR mr1 = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);
                                    new GMSGeneralDALC().GetProductTeamByProductCode(CompanyId, ProdCode, ref ds1);
                                    if ((ds1 != null) && (ds1.Tables[0].Rows.Count > 0))
                                    {
                                        if (ds1.Tables[0].Rows[0]["pmuserid"].ToString() != mr1.PMUserId.ToString() ||
                                           ds1.Tables[0].Rows[0]["phuserid"].ToString() != mr1.PHUserId.ToString() ||
                                           ds1.Tables[0].Rows[0]["ph3userid"].ToString() != mr1.PH3UserId.ToString() ||
                                           ds1.Tables[0].Rows[0]["ph5userid"].ToString() != mr1.PH5UserId.ToString())
                                            AllowInsert = false;
                                    }
                                }

                                Changes = true;
                                if (AllowInsert)
                                {
                                    GMSCore.Entity.MRDetail md = new GMSCore.Entity.MRDetail();
                                    md.CoyID = CompanyId;
                                    md.MRNo = MRNo;
                                    md.ProdCode = ProdCode;
                                    md.NewProdCode = NewProdCode;
                                    md.ProdName = ProdName;
                                    md.UOM = UOM;
                                    md.OnHandQty = GMSUtil.ToDouble(OnHandQty);
                                    md.OnOrderQty = GMSUtil.ToDouble(OnOrderQty); ;
                                    md.OnPOQty = GMSUtil.ToDouble(OnPOQty);
                                    md.ConfirmedOrderQty = GMSUtil.ToDouble(ConfirmedOrderQty);
                                    md.ForStockingQty = GMSUtil.ToDouble(ForStockingQty);
                                    md.OrderQty = GMSUtil.ToDouble(OrderQty);
                                    md.UnitSellingPrice = GMSUtil.ToDouble(UnitSellingPrice);
                                    md.PurchaseCurrency = PurchaseCurrency;
                                    md.UnitPurchasePrice = GMSUtil.ToDouble(UnitPurchasePrice);
                                    md.SellingCurrency = SellingCurrency;
                                    md.Reason = Reason;
                                    md.RecipeNo = RecipeNo;
                                    md.CreatedBy = loginUserOrAlternateParty;
                                    md.CreatedDate = DateTime.Now;
                                    md.Save();
                                }

                                if (IsNew)
                                {
                                    mr.PMUserId = GMSUtil.ToShort(Approver1ID);
                                    mr.PHUserId = GMSUtil.ToShort(Approver2ID);
                                    mr.PH3UserId = GMSUtil.ToShort(Approver3ID);
                                    mr.PH5UserId = GMSUtil.ToShort(Approver4ID);
                                }

                            }
                            else if (RowStatus == "Modify")
                            {
                                Changes = true;
                                GMSCore.Entity.MRDetail md = new MRActivity().RetrieveMRProductByID(CompanyId, DetailNo);
                                md.ProdCode = ProdCode;
                                md.NewProdCode = NewProdCode;
                                md.ProdName = ProdName;
                                md.UOM = UOM;
                                md.OnHandQty = GMSUtil.ToDouble(OnHandQty);
                                md.OnOrderQty = GMSUtil.ToDouble(OnOrderQty); ;
                                md.OnPOQty = GMSUtil.ToDouble(OnPOQty);
                                md.ConfirmedOrderQty = GMSUtil.ToDouble(ConfirmedOrderQty);
                                md.ForStockingQty = GMSUtil.ToDouble(ForStockingQty);
                                md.OrderQty = GMSUtil.ToDouble(OrderQty);
                                md.UnitSellingPrice = GMSUtil.ToDouble(UnitSellingPrice);
                                md.PurchaseCurrency = PurchaseCurrency;
                                md.UnitPurchasePrice = GMSUtil.ToDouble(UnitPurchasePrice);
                                md.SellingCurrency = SellingCurrency;
                                md.Reason = Reason;
                                md.RecipeNo = RecipeNo;
                                md.CreatedBy = loginUserOrAlternateParty;
                                md.CreatedDate = DateTime.Now;
                                md.Save();
                            }

                            if (!insertedVendor && Source != "Local" && MRNo == "")
                            {
                                IList<MRSupplier> lstSupplier = new MRActivity().RetrieveMRSupplierByCoyIDProductGroupCode(CompanyId, ProductGroupCode);

                                foreach (MRSupplier supplier in lstSupplier)
                                {
                                    GMSCore.Entity.MRVendor mv = new GMSCore.Entity.MRVendor();
                                    mv.CoyID = CompanyId;
                                    mv.MRNo = MRNo;
                                    mv.VendorName = supplier.AccountName.ToString();
                                    mv.VendorContact = supplier.Contact.ToString();
                                    mv.VendorTel = supplier.Tel.ToString();
                                    mv.VendorFax = supplier.Fax.ToString();
                                    mv.VendorEmail = supplier.Email.ToString();
                                    mv.CreatedBy = UserID;
                                    mv.CreatedDate = DateTime.Now;
                                    mv.MRSupplierID = (short)supplier.Id;
                                    mv.Save();
                                }
                                insertedVendor = true;
                            }

                            if (RecipeNo != "") {
                                ImportRecipe(CompanyId, RecipeNo);
                            }
                        }

                        if (!IsNew && Changes)
                        {
                            if (AllowInsert)
                            {
                                using (TransactionScope tran = new TransactionScope())
                                {
                                    string message = Resubmission("Product updated.", "Record added/updated successfully!", "", CompanyId, MRNo, UserID, CurrentMRStatus);
                                    if (message == "")
                                    {
                                        Changes = false;
                                        tran.Complete();                                        
                                    }
                                    else
                                        throw new HttpException(500, message);
                                }
                            }
                            else
                            {
                                string message = "Product update failed";
                                throw new HttpException(500, message);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            ds.Clear();
            new GMSGeneralDALC().GetMaterialRequisitionByMRNo(CompanyId, MRNo, ref ds);

            if (StatusChange)
                return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + CompanyId + "&MRNo=" + MRNo;
            else
                return "success";

        }
        public static bool ResetApproval(short CompanyId, string MRNo, string MRRole, short UserID)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserID);

            MR mr = new MRActivity().RetrieveMRByMRNo(CompanyId, MRNo);
            if (mr.StatusID != "C" && mr.StatusID != "P" && mr.StatusID != "A" && mr.StatusID != "D" && MRRole != "Purchasing")
            {
                WorkFlow WF = new WorkFlow();
                WF.ResetCurrentApproval(mr.PIID, loginUserOrAlternateParty);
                new GMSGeneralDALC().InsertMRLog(CompanyId, MRNo, UserID, loginUserOrAlternateParty, "RESET-AddEditMaterial");

                if (loginUserOrAlternateParty != mr.PMUserId && loginUserOrAlternateParty != mr.PHUserId && loginUserOrAlternateParty != mr.PH3UserId && loginUserOrAlternateParty != mr.PH5UserId)
                {
                    mr.StatusID = "D";
                    mr.Save();
                    return true;
                }
            }
            return false;

        }

        public static void UpdateProductTeam(short CompanyId, ArrayList ProductInfo)
        {
            DataSet ds = new DataSet();
            IDictionary idict;
            string ProdCode = "";

            DataSet ds1 = new DataSet();
            try
            {
                if (ProductInfo != null)
                {
                    for (int i = 0; i < ProductInfo.Count; ++i)
                    {
                        idict = (IDictionary)ProductInfo[i];
                        ProdCode = idict["ProdCode"].ToString();
                        if (ProdCode != "00000000000")
                        {
                            new GMSGeneralDALC().GetProductTeamByProductCode(CompanyId, ProdCode, ref ds1);
                            if ((ds1 != null) && (ds1.Tables[0].Rows.Count > 0))
                            {
                                idict["ProductGroupCode"] = ds1.Tables[0].Rows[0]["ProductGroupCode"].ToString();
                                idict["Approver1ID"] = ds1.Tables[0].Rows[0]["pmuserid"].ToString();
                                idict["Approver2ID"] = ds1.Tables[0].Rows[0]["phuserid"].ToString();
                                idict["Approver3ID"] = ds1.Tables[0].Rows[0]["ph3userid"].ToString();
                                idict["Approver4ID"] = ds1.Tables[0].Rows[0]["ph5userid"].ToString();
                                idict["KeyToSplit"] = ds1.Tables[0].Rows[0]["ProductGroupCode"].ToString() + ds1.Tables[0].Rows[0]["pmuserid"].ToString() + ds1.Tables[0].Rows[0]["phuserid"].ToString() + ds1.Tables[0].Rows[0]["ph3userid"].ToString() + ds1.Tables[0].Rows[0]["ph5userid"].ToString();
                                idict["ProdName"] = ds1.Tables[0].Rows[0]["ProductName"].ToString();
                                ProductInfo[i] = idict;
                                ds1.Clear();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
            }
        }

        public static List<string> GroupByProductGroup(ArrayList ProductInfo)
        {
            IDictionary idict;
            List<string> list = new List<string>();
            int numOfGroup = 0;
            string KeyToSplit = "";

            for (int i = 0; i < ProductInfo.Count; ++i)
            {
                idict = (IDictionary)ProductInfo[i];
                foreach (object key in idict.Keys)
                {
                    if (key.ToString() == "KeyToSplit")
                    {
                        if (!list.Contains(idict[key].ToString()))
                        {
                            list.Add(idict[key].ToString());
                            numOfGroup = numOfGroup + 1;
                        }
                    }
                }
            }
            return list;

        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetCurrencyList()
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetCurrencyList(ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetRequestorAndApproverList(short CompanyId, short UserId)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetRequestorAndApproverList(CompanyId, loginUserOrAlternateParty, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetRequestorList(short CompanyId, short UserId, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetRequestorList(CompanyId, loginUserOrAlternateParty, MRScheme, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetApprover1List(short CompanyId, short UserId, string UserRealName, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetApprover1List(CompanyId, loginUserOrAlternateParty, UserRealName, MRScheme, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetApprover2List(short CompanyId, short UserId, string UserRealName, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetApprover2List(CompanyId, loginUserOrAlternateParty, UserRealName, MRScheme, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetApprover3List(short CompanyId, short UserId, string UserRealName, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetApprover3List(CompanyId, loginUserOrAlternateParty, UserRealName, MRScheme, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetApprover4List(short CompanyId, short UserId, string UserRealName, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetApprover4List(CompanyId, loginUserOrAlternateParty, UserRealName, MRScheme, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }


        [WebMethod]
        public static List<Dictionary<string, string>> GetPurchaserList(short CompanyId)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetPurchaserList(CompanyId, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);

        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetWarehouseList(short CompanyId)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetWarehouseList(CompanyId, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);

        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetMRStatusList(short CompanyId, string Status)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetMRStatusList(CompanyId, Status, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }


        [WebMethod]
        public static List<Dictionary<string, string>> CheckUserAccess(short CompanyId, short UserId, string MRNo, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).CanUserAccessForMR(CompanyId, "MR", MRNo, loginUserOrAlternateParty, MRScheme, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);

        }

        [WebMethod]
        public static List<Dictionary<string, string>> CheckProductCode(short CompanyId, string ProdCode)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).CheckProductCode(CompanyId, ProdCode, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> CheckAccountCode(short CompanyId, string AccountCode)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).CheckAccountCode(CompanyId, AccountCode, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetCompany(short CompanyId)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetCompany(CompanyId, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetConvertedSellingAndPurchasePrice(short CompanyId, string SellingPrice, string SellingCurrency, string PurchasePrice, string PurchaseCurrency)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).ConvertSellingPriceAndPurchasePrice(CompanyId, GMSUtil.ToDouble(SellingPrice), GMSUtil.ToDouble(PurchasePrice), SellingCurrency, PurchaseCurrency, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }
        /*
        [WebMethod]
        public static List<Dictionary<string, string>> GetUserRole(short CompanyId, short UserId)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetMRUserRoleByUserNumIDCoyID(CompanyId, loginUserOrAlternateParty, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }
        */


        [WebMethod]
        public static List<Dictionary<string, string>> GetMRHeaderByMRNo(short CompanyId, string MRNo)
        {
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetMRHeaderByMRNo(CompanyId, MRNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetTaxType(short CompanyId)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetTaxType(CompanyId, false ,ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> CheckUserAccessDocument(short CompanyId, short UserId, string MRNo)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).CanUserAccessDocument(CompanyId, "MR", MRNo, loginUserOrAlternateParty, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> CheckApproverUser(short CompanyId, short UserId, short Seq, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetMaterialRequisitionPMPHRequestorByCoyID(CompanyId, loginUserOrAlternateParty, MRScheme, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, Seq);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> RetrieveAutoInsertedVendorByMRNo(short CompanyId, string MRNo)
        {
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetAutoInsertedVendorByMRNo(CompanyId, MRNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        #region GetDynamicContent
        [WebMethod]
        public static string GetDynamicContent(short CompanyId, string ProdCode, short UserId, string AccountCode)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            short coyId = 1;
            string prodCode = "";
            coyId = CompanyId;
            prodCode = ProdCode;
            bool canAccessProductStatus = false;
            bool isGasDivision = false;
            bool isWeldingDivision = false;

            DataSet ds = new DataSet();
            DataSet ds_lms = new DataSet();
            Company coy = Company.RetrieveByKey(coyId);
            DataSet ds2 = new DataSet();
            GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();

            if (coy.StatusType.ToString() == "H" || coy.StatusType.ToString() == "A")
            {
                if (coy.WebServiceAddress != null && coy.WebServiceAddress.Trim() != "")
                {
                    sc.Url = coy.WebServiceAddress.Trim();
                }
                else
                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                ds = sc.GetProductStockStatus(coyId, prodCode);
                ds2 = sc.GetProductDetailByProductCode(coyId, prodCode);
            }

            // Get ProductStatus From LMS
            if (coy.StatusType.ToString() == "H")
            {
                CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                if (coy.CMSWebServiceAddress != null && coy.CMSWebServiceAddress.Trim() != "")
                {
                    sc1.Url = coy.CMSWebServiceAddress.Trim();
                }
                else
                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                ds_lms = sc1.GetProductStockStatus(prodCode);
                ds2 = sc.GetProductDetailByProductCode(coyId, prodCode);
            }
            else if (coy.StatusType.ToString() == "L")
            {
                CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                if (coy.CMSWebServiceAddress != null && coy.CMSWebServiceAddress.Trim() != "")
                {
                    sc1.Url = coy.CMSWebServiceAddress.Trim();
                }
                else
                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                ds = sc1.GetProductWarehouse(prodCode);
                ds2 = sc1.GetProductDetailByProductCode(prodCode);
                if (ds2 != null && ds.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    isGasDivision = Convert.ToBoolean(ds2.Tables[0].Rows[0]["IsGasDivision"].ToString());
                    isWeldingDivision = Convert.ToBoolean(ds2.Tables[0].Rows[0]["IsWeldingDivision"].ToString());
                }
            }
            else if (coy.StatusType.ToString() == "S")
            {
                string query = "CALL \"AF_API_GET_SAP_STOCK_STATUS\" ('" + prodCode + "', '', '', '', '', '2099-12-31', 'Y')";
                SAPOperation sop = new SAPOperation(coy.SAPURI.ToString(), coy.SAPKEY.ToString(), coy.SAPDB.ToString());
                ds = sop.GET_SAP_QueryData(coy.CoyID, query,
                "ItemCode", "Warehouse", "OnHand", "Committed", "Quantity", "WarehouseName", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
            }

            DataSet ds3 = new DataSet();
            new GMSGeneralDALC().GetPurchaseInfoForProductTeam(coyId, prodCode, loginUserOrAlternateParty, ref ds3);

            DataSet ds4 = new DataSet();
            new GMSGeneralDALC().GetSalesInfoForProductTeam(coyId, prodCode, loginUserOrAlternateParty, ref ds4);

            DataSet ds5 = new DataSet();
            new GMSGeneralDALC().GetSalesInfo(coyId, prodCode, AccountCode, loginUserOrAlternateParty, ref ds5);
                       

            DivisionUser du = DivisionUser.RetrieveByKey(CompanyId, loginUserOrAlternateParty);
            if (du != null)
            {
                if (du.DivisionID == "GAS" && isGasDivision)
                {
                    canAccessProductStatus = true;
                }
                else if (du.DivisionID == "WSD" && isWeldingDivision)
                {
                    canAccessProductStatus = true;
                }
            }
            else
                canAccessProductStatus = true;


            StringBuilder b = new StringBuilder();

            b.Append("<table>");
            b.Append("<tr><td colspan='2'></td></tr>");
                   
            if (canAccessProductStatus && ((ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) || (ds2 != null && ds.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)))
            {
                if (coy.StatusType.ToString() == "H")
                {
                    for (int i = 0; i < ds_lms.Tables[0].Rows.Count; i++)
                    {
                        bool isDupe = false;
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            if (ds_lms.Tables[0].Rows[i][0].ToString() == ds.Tables[0].Rows[j][0].ToString())
                            {
                                ds.Tables[0].Rows[j][2] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][2].ToString()) + GMSUtil.ToDecimal(ds_lms.Tables[0].Rows[i][2].ToString());
                                isDupe = true;
                                break;
                            }
                        }
                        if (!isDupe)
                        {
                            ds.Tables[0].ImportRow(ds_lms.Tables[0].Rows[i]);
                        }
                    }
                }
                b.Append("<tr><td style='width:250px;'><b>Warehouse</b></td>");
                b.Append("<td><b>Quantity</b></td></tr>");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    b.Append("<tr>");
                    b.Append("<td>" + dr["Warehouse"].ToString() + " - " + dr["WarehouseName"].ToString() + "</td>");
                    b.Append("<td>" + dr["Quantity"].ToString() + "</td>");
                    b.Append("</tr>");
                }

                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    b.Append("<tr>");
                    b.Append("<td>On SO</td>");
                    b.Append("<td>" + ds2.Tables[0].Rows[0]["OnOrderQuantity"].ToString() + "</td>");
                    b.Append("</tr>");
                    b.Append("<tr>");
                    b.Append("<td>On PO</td>");
                    b.Append("<td>" + ds2.Tables[0].Rows[0]["OnPOQuantity"].ToString() + "</td>");
                    b.Append("</tr>");
                    b.Append("<tr>");
                    b.Append("<td>On BO</td>");
                    b.Append("<td>" + ds2.Tables[0].Rows[0]["OnBOQuantity"].ToString() + "</td>");
                    b.Append("</tr>");
                }

                if (ds5 != null && ds5.Tables.Count > 0 && ds5.Tables[0].Rows.Count > 0)
                {
                    b.Append("<tr>");
                    b.Append("<td>Last Selling Price (Unit)</td>");
                    b.Append("<td>" + ds5.Tables[0].Rows[0]["Sales"].ToString() + "</td>");
                    b.Append("</tr>");
                }

                if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                {
                    b.Append("<tr><td colspan='2'></td></tr>");
                    b.Append("<tr><td colspan='2'><b>Extra Info</b></td></tr>");
                    b.Append("<tr>");
                    b.Append("<td>Last Purchase Price</td>");
                    b.Append("<td>" + ds3.Tables[0].Rows[0]["Currency"].ToString() + " " + ds3.Tables[0].Rows[0]["LastPurchasePrice"].ToString() + "</td>");
                    b.Append("</tr>");
                    b.Append("<tr>");
                    b.Append("<td>Last Purchase Date</td>");
                    b.Append("<td>" + ds3.Tables[0].Rows[0]["LastPurchaseDate"].ToString() + "</td>");
                    b.Append("</tr>");

                }
                if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
                {
                    b.Append("<tr>");
                    b.Append("<td>Avg Sales Per Month (12 months)</td>");
                    b.Append("<td>" + ds4.Tables[0].Rows[0]["AvgSalesPerMonth"].ToString() + "</td>");
                    b.Append("</tr>");
                }
            }
            else
            {
                b.Append("<tr>");
                b.Append("<td colspan='2'><i>Not Avaiable.</i></td>");
                b.Append("</tr>");
            }

            b.Append("</table>");

            return b.ToString();
        }
        #endregion  

        #region GetGRNInfo
        [WebMethod]
        public static string GetGRNInfo(short CompanyId, string PONo, string MRNo, short UserId)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet ds = new DataSet();
            new GMSGeneralDALC().GetGRNInfoByPO(CompanyId, PONo, loginUserOrAlternateParty, MRNo, ref ds);



            StringBuilder b = new StringBuilder();
            b.Append("<table>");
            b.Append("<tr><td colspan='2'></td></tr>");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                b.Append("<tr><td style='width:150px;'><b>GRN No.</b></td>");
                b.Append("<td><b>GRN Date</b></td>");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    b.Append("<tr>");
                    if (dr["ViewSupplier"].ToString() == "ViewSupplier")
                        b.Append("<td><a href=\'ViewGRN.aspx?MRNo=" + MRNo + "&TrnNo=" + dr["GRNNo"].ToString() + "\' target=\'_blank\'>" + dr["GRNNo"].ToString() + "</a> / " + dr["GRNTrnNo"].ToString() + "</td>");
                    else
                        b.Append("<td><a href=\'ViewGRNWithoutSupplierInfo.aspx?MRNo=" + MRNo + "&TrnNo=" + dr["GRNNo"].ToString() + "\' target=\'_blank\'>" + dr["GRNNo"].ToString() + "</a> / " + dr["GRNTrnNo"].ToString() + "</td>");
                    b.Append("<td>" + dr["GRNDate"].ToString() + "</td>");
                    b.Append("</tr>");
                }


            }
            else
            {
                b.Append("<tr>");
                b.Append("<td colspan='2'><i>Not Avaiable.</i></td>");
                b.Append("</tr>");
            }

            b.Append("</table>");

            return b.ToString();
        }
        #endregion 

        [WebMethod]
        public static List<Dictionary<string, string>> GetNextApprover(short CompanyId, short UserId, short Seq, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            new GMSGeneralDALC().GetNextApproverByLevel(CompanyId, loginUserOrAlternateParty, MRScheme, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, Seq);
        }


        [WebMethod]
        public static string CreatePO(short companyId, string mrNo, string PageName, string CurrentLink)
        {
            try
            {
                Company coy = Company.RetrieveByKey(companyId);

                string lms_IntegrationUserID = System.Web.Configuration.WebConfigurationManager.AppSettings["LMS_IntegrationUserID"].ToString();

                DataSet ds = new DataSet();
                new GMSGeneralDALC().GetMaterialRequisitionByMRNoForSAP(companyId, mrNo, ref ds);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    SAPPurchaseOrder sapPO = new SAPPurchaseOrder();
                    sapPO.GMSDocumentNumber = ds.Tables[0].Rows[0]["MRNo"].ToString();
                    //sapPO.PostingDate = ds.Tables[0].Rows[0]["MRDate"].ToString();                    
                    sapPO.PostingDate = DateTime.Now.ToString("yyyy-MM-dd");
                    sapPO.Vendor = ds.Tables[0].Rows[0]["VendorCode"].ToString();                                 
                    sapPO.DocumentCurency = ds.Tables[0].Rows[0]["CurrencyCode"].ToString();
                    sapPO.DocumentRate = ds.Tables[0].Rows[0]["ExchangeRate"].ToString();
                    sapPO.CreatedBy = lms_IntegrationUserID;
                    sapPO.DocumentDiscount = ds.Tables[0].Rows[0]["Discount"].ToString();
                    sapPO.Remarks = ds.Tables[0].Rows[0]["Remarks"].ToString();
                    sapPO.ContactPerson = ds.Tables[0].Rows[0]["Contact"].ToString();
                    sapPO.GMSCoyID = companyId.ToString();
                    sapPO.Purchaser = ds.Tables[0].Rows[0]["Purchaser"].ToString();
                    sapPO.DeliveryMode = ds.Tables[0].Rows[0]["FreightMode"].ToString();

                    sapPO.SAPPurchaseOrder_Detail = new List<SAPPurchaseOrder_Detail>();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            SAPPurchaseOrder_Detail sapDetail = new SAPPurchaseOrder_Detail();
                            sapDetail.GMSDocumentNumber = ds.Tables[1].Rows[j]["MRNo"].ToString();
                            sapDetail.GMSLineNumber = ds.Tables[1].Rows[j]["DetailNo"].ToString();
                            sapDetail.ProductCode = ds.Tables[1].Rows[j]["ProdCode"].ToString(); 
                            sapDetail.ItemCode = ds.Tables[1].Rows[j]["ProdCode"].ToString();
                            sapDetail.ProductName = ds.Tables[1].Rows[j]["ProdName"].ToString(); 
                            sapDetail.Warehouse = ds.Tables[0].Rows[0]["Warehouse"].ToString();
                            sapDetail.TaxCode = ds.Tables[0].Rows[0]["TaxTypeID"].ToString(); 
                            sapDetail.LineDiscountPercent = "0".ToString();
                            sapDetail.DimensionL1 = ds.Tables[0].Rows[0]["DimensionL1"].ToString(); 
                            sapDetail.DimensionL2 = ds.Tables[0].Rows[0]["DimensionL2"].ToString(); 
                            sapDetail.DimensionL3 = ds.Tables[0].Rows[0]["DimensionL3"].ToString();
                            sapDetail.DimensionL4 = ds.Tables[0].Rows[0]["DimensionL4"].ToString(); 
                            sapDetail.UOMQuantity = ds.Tables[1].Rows[j]["OrderQty"].ToString();
                            sapDetail.UOMPrice = ds.Tables[1].Rows[j]["UnitPurchasePrice"].ToString(); 
                            sapDetail.UOMSales = ds.Tables[1].Rows[j]["UOM"].ToString(); 
                            sapDetail.Requestor = ds.Tables[0].Rows[0]["Requestor"].ToString(); 
                            sapDetail.Approver1 = ds.Tables[0].Rows[0]["Approver1"].ToString(); 
                            sapDetail.Approver2 = ds.Tables[0].Rows[0]["Approver2"].ToString(); 
                            sapDetail.Approver3 = ds.Tables[0].Rows[0]["Approver3"].ToString(); 
                            sapDetail.Approver4 = ds.Tables[0].Rows[0]["Approver4"].ToString(); 
                            sapDetail.GMSMRNo = ds.Tables[0].Rows[0]["MRNo"].ToString();
                            sapDetail.GMSProject = ds.Tables[0].Rows[0]["ProjectNo"].ToString();  
                            sapDetail.ItemName = ds.Tables[1].Rows[j]["ProdName"].ToString();
                            sapPO.SAPPurchaseOrder_Detail.Add(sapDetail);
                        }
                    }
                    SAPOperation sop = new SAPOperation(coy.SAPURI.ToString(), coy.SAPKEY.ToString(), coy.SAPDB.ToString());
                                     
                    sop.PostPO(companyId, "MR", sapPO, false);
                }                
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }

            return PageName + "&CurrentLink=" + CurrentLink + "&CoyID=" + companyId + "&MRNo=" + mrNo;
        }

        [WebMethod]
        public static List<Dictionary<string, string>> CheckData(short CompanyId, short UserId, string MRNo)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).CheckDataForMR(CompanyId, "MR", MRNo, loginUserOrAlternateParty, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);

        }

     

        [WebMethod]
        public static void ImportRecipeInfo(short CompanyId, string recipeno)
        {
            Company coy = Company.RetrieveByKey(CompanyId);
            string RecipeNotFoundMessage = "";
            string RecipeSuccessdMessage = "";
            string message = "";
            List<string> recipeNoList = new List<string>();
            Dictionary<string, string> newDict = new Dictionary<string, string>();

            CMSWebService.CMSWebService sc = new CMSWebService.CMSWebService();
            if (coy.CMSWebServiceAddress != null && coy.CMSWebServiceAddress.Trim() != "")
            {
                
                if (recipeno != "")
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        if (coy.CMSWebServiceAddress != null && coy.CMSWebServiceAddress.Trim() != "")
                        {
                            sc.Url = coy.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/CMS.WebServices/Recipe.asmx";

                        ds = sc.GetRecipe(recipeno);
                        if (ds != null && ds.Tables[0].Rows.Count == 0)
                        {
                            if (RecipeNotFoundMessage == "")
                                RecipeNotFoundMessage = RecipeNotFoundMessage + recipeno.Trim();
                            else
                                RecipeNotFoundMessage = RecipeNotFoundMessage + "," + recipeno.Trim();
                        }

                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (RecipeSuccessdMessage == "")
                                RecipeSuccessdMessage = RecipeSuccessdMessage + recipeno.Trim();
                            else
                                RecipeSuccessdMessage = RecipeSuccessdMessage + "," + recipeno.Trim();
                        }

                        //tbRecipe
                        //if (ds != null && ds.Tables[0].Rows.Count > 0)
                        //{
                        //    if (RecipeSuccessdMessage == "")
                        //        RecipeSuccessdMessage = RecipeSuccessdMessage + RecipeNo.Trim();
                        //    else
                        //        RecipeSuccessdMessage = RecipeSuccessdMessage + "," + RecipeNo.Trim();


                        //    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        //    {
                        //        bool IsStandardLiquidContent;
                        //        if (ds.Tables[0].Rows[j]["IsStandardLiquidContent"].ToString() == "True")
                        //            IsStandardLiquidContent = true;
                        //        else
                        //            IsStandardLiquidContent = false;

                        //        new GMSGeneralDALC().InsertRecipe(GMSUtil.ToByte(CompanyId),
                        //            ds.Tables[0].Rows[j]["RecipeNo"].ToString(),
                        //            ds.Tables[0].Rows[j]["AccountCode"].ToString(),
                        //            DateTime.Parse(ds.Tables[0].Rows[j]["RecipeDate"].ToString()),
                        //            ds.Tables[0].Rows[j]["MixtureType"].ToString(),
                        //            ds.Tables[0].Rows[j]["MolecularUnit"].ToString(),
                        //            GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["CylinderCapacity"].ToString()),
                        //            GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["Temperature"].ToString()),
                        //            GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["RequiredPressure"].ToString()),
                        //            IsStandardLiquidContent,
                        //            GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["LiquidContent"].ToString()),
                        //            ds.Tables[0].Rows[j]["TopPressure"].ToString(),
                        //            ds.Tables[0].Rows[j]["CertificationType"].ToString(),
                        //            ds.Tables[0].Rows[j]["ValveConnection"].ToString(),
                        //            ds.Tables[0].Rows[j]["ValveConnectionType"].ToString(),
                        //            GMSUtil.ToShort(ds.Tables[0].Rows[j]["ShelfLife"].ToString()),
                        //            ds.Tables[0].Rows[j]["SpecialRequirement"].ToString(),
                        //            GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["GasContent"].ToString()),
                        //            GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["Pressure"].ToString()),
                        //            GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["GasPrice"].ToString()),
                        //            GMSUtil.ToShort(ds.Tables[0].Rows[j]["MinLeadTime"].ToString()),
                        //            GMSUtil.ToShort(ds.Tables[0].Rows[j]["MaxLeadTime"].ToString()),
                        //            GMSUtil.ToShort(ds.Tables[0].Rows[j]["TotalComponent"].ToString()),
                        //            ds.Tables[0].Rows[j]["CylinderTypeID"].ToString(),
                        //            ds.Tables[0].Rows[j]["Remarks"].ToString());
                        //    }
                        //}

                        ////tbRecipeDetail
                        //if (ds != null && ds.Tables[1].Rows.Count > 0)
                        //{
                        //    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        //    {
                        //        bool IsBaseGas, RequiredSpecification;

                        //        if (ds.Tables[1].Rows[j]["IsBaseGas"].ToString() == "true")
                        //            IsBaseGas = true;
                        //        else
                        //            IsBaseGas = false;

                        //        if (ds.Tables[1].Rows[j]["RequiredSpecification"].ToString() == "true")
                        //            RequiredSpecification = true;
                        //        else
                        //            RequiredSpecification = false;

                        //        new GMSGeneralDALC().InsertRecipeDetail(
                        //             GMSUtil.ToByte(CompanyId),
                        //             ds.Tables[1].Rows[j]["RecipeNo"].ToString(),
                        //             GMSUtil.ToShort(ds.Tables[1].Rows[j]["DetailNo"].ToString()),
                        //             GMSUtil.ToShort(ds.Tables[1].Rows[j]["ComponentID"].ToString()),
                        //             GMSUtil.ToShort(ds.Tables[1].Rows[j]["ConcentrationUnitID"].ToString()),
                        //             GMSUtil.ToFloat(ds.Tables[1].Rows[j]["RequestedConcentration"].ToString()),
                        //             ds.Tables[1].Rows[j]["RequestedConcentrationUnit"].ToString(),
                        //             GMSUtil.ToFloat(ds.Tables[1].Rows[j]["IdealWeight"].ToString()),
                        //             IsBaseGas,
                        //             RequiredSpecification,
                        //             GMSUtil.ToFloat(ds.Tables[1].Rows[j]["BlendTolerance"].ToString()),
                        //             GMSUtil.ToFloat(ds.Tables[1].Rows[j]["CertificationAccuracy"].ToString())
                        //             );
                        //    }
                        //}

                        //if (RecipeSuccessdMessage != "")
                        //{
                        //    using (TransactionScope tran = new TransactionScope())
                        //    {
                        //        message = Resubmission("Recipe No:"+ RecipeNotFoundMessage + " imported.", "Record added/updated successfully!", "", CompanyId, UserID, CurrentMRStatus);
                        //        if (RecipeNotFoundMessage == "")
                        //            tran.Complete();
                        //        else
                        //            throw new HttpException(500, RecipeNotFoundMessage);


                        //    }
                        //}
                        using (TransactionScope tran = new TransactionScope())
                        {
                            if (RecipeNotFoundMessage != "")
                                throw new HttpException(500, "Recipe No: " + recipeno + " not found in CMS. Please check!");
                            if (RecipeSuccessdMessage != "")
                                tran.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new HttpException(500, ex.Message);
                    }
                }
            }
        }

        public static void ImportRecipe(short CompanyId, string recipeno)
        {
            Company coy = Company.RetrieveByKey(CompanyId);
            string RecipeNotFoundMessage = "";
            string RecipeSuccessdMessage = "";
            string message = "";
            List<string> recipeNoList = new List<string>();
            Dictionary<string, string> newDict = new Dictionary<string, string>();

            CMSWebService.CMSWebService sc = new CMSWebService.CMSWebService();
            if (coy.CMSWebServiceAddress != null && coy.CMSWebServiceAddress.Trim() != "")
            {

                if (recipeno != "")
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        if (coy.CMSWebServiceAddress != null && coy.CMSWebServiceAddress.Trim() != "")
                        {
                            sc.Url = coy.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/CMS.WebServices/Recipe.asmx";

                        ds = sc.GetRecipe(recipeno);
                        if (ds != null && ds.Tables[0].Rows.Count == 0)
                        {
                            if (RecipeNotFoundMessage == "")
                                RecipeNotFoundMessage = RecipeNotFoundMessage + recipeno.Trim();
                            else
                                RecipeNotFoundMessage = RecipeNotFoundMessage + "," + recipeno.Trim();
                        }

                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (RecipeSuccessdMessage == "")
                                RecipeSuccessdMessage = RecipeSuccessdMessage + recipeno.Trim();
                            else
                                RecipeSuccessdMessage = RecipeSuccessdMessage + "," + recipeno.Trim();
                        }

                        //tbRecipe
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (RecipeSuccessdMessage == "")
                                RecipeSuccessdMessage = RecipeSuccessdMessage + recipeno.Trim();
                            else
                                RecipeSuccessdMessage = RecipeSuccessdMessage + "," + recipeno.Trim();


                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                bool IsStandardLiquidContent;
                                if (ds.Tables[0].Rows[j]["IsStandardLiquidContent"].ToString() == "True")
                                    IsStandardLiquidContent = true;
                                else
                                    IsStandardLiquidContent = false;

                                new GMSGeneralDALC().InsertRecipe(GMSUtil.ToByte(CompanyId),
                                    ds.Tables[0].Rows[j]["RecipeNo"].ToString(),
                                    ds.Tables[0].Rows[j]["AccountCode"].ToString(),
                                    DateTime.Parse(ds.Tables[0].Rows[j]["RecipeDate"].ToString()),
                                    ds.Tables[0].Rows[j]["MixtureType"].ToString(),
                                    ds.Tables[0].Rows[j]["MolecularUnit"].ToString(),
                                    GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["CylinderCapacity"].ToString()),
                                    GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["Temperature"].ToString()),
                                    GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["RequiredPressure"].ToString()),
                                    IsStandardLiquidContent,
                                    GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["LiquidContent"].ToString()),
                                    ds.Tables[0].Rows[j]["TopPressure"].ToString(),
                                    ds.Tables[0].Rows[j]["CertificationType"].ToString(),
                                    ds.Tables[0].Rows[j]["ValveConnection"].ToString(),
                                    ds.Tables[0].Rows[j]["ValveConnectionType"].ToString(),
                                    GMSUtil.ToShort(ds.Tables[0].Rows[j]["ShelfLife"].ToString()),
                                    ds.Tables[0].Rows[j]["SpecialRequirement"].ToString(),
                                    GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["GasContent"].ToString()),
                                    GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["Pressure"].ToString()),
                                    GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["GasPrice"].ToString()),
                                    GMSUtil.ToShort(ds.Tables[0].Rows[j]["MinLeadTime"].ToString()),
                                    GMSUtil.ToShort(ds.Tables[0].Rows[j]["MaxLeadTime"].ToString()),
                                    GMSUtil.ToShort(ds.Tables[0].Rows[j]["TotalComponent"].ToString()),
                                    ds.Tables[0].Rows[j]["CylinderTypeID"].ToString(),
                                    ds.Tables[0].Rows[j]["Remarks"].ToString());
                            }
                        }

                        ////tbRecipeDetail
                        if (ds != null && ds.Tables[1].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                bool IsBaseGas, RequiredSpecification;

                                if (ds.Tables[1].Rows[j]["IsBaseGas"].ToString() == "true")
                                    IsBaseGas = true;
                                else
                                    IsBaseGas = false;

                                if (ds.Tables[1].Rows[j]["RequiredSpecification"].ToString() == "true")
                                    RequiredSpecification = true;
                                else
                                    RequiredSpecification = false;

                                new GMSGeneralDALC().InsertRecipeDetail(
                                     GMSUtil.ToByte(CompanyId),
                                     ds.Tables[1].Rows[j]["RecipeNo"].ToString(),
                                     GMSUtil.ToShort(ds.Tables[1].Rows[j]["DetailNo"].ToString()),
                                     GMSUtil.ToShort(ds.Tables[1].Rows[j]["ComponentID"].ToString()),
                                     GMSUtil.ToShort(ds.Tables[1].Rows[j]["ConcentrationUnitID"].ToString()),
                                     GMSUtil.ToFloat(ds.Tables[1].Rows[j]["RequestedConcentration"].ToString()),
                                     ds.Tables[1].Rows[j]["RequestedConcentrationUnit"].ToString(),
                                     GMSUtil.ToFloat(ds.Tables[1].Rows[j]["IdealWeight"].ToString()),
                                     IsBaseGas,
                                     RequiredSpecification,
                                     GMSUtil.ToFloat(ds.Tables[1].Rows[j]["BlendTolerance"].ToString()),
                                     GMSUtil.ToFloat(ds.Tables[1].Rows[j]["CertificationAccuracy"].ToString())
                                     );
                            }
                        }

                        //if (RecipeSuccessdMessage != "")
                        //{
                        //    using (TransactionScope tran = new TransactionScope())
                        //    {
                        //        message = Resubmission("Recipe No:"+ RecipeNotFoundMessage + " imported.", "Record added/updated successfully!", "", CompanyId, UserID, CurrentMRStatus);
                        //        if (RecipeNotFoundMessage == "")
                        //            tran.Complete();
                        //        else
                        //            throw new HttpException(500, RecipeNotFoundMessage);


                        //    }
                        //}
                        using (TransactionScope tran = new TransactionScope())
                        {
                            if (RecipeNotFoundMessage != "")
                                throw new HttpException(500, "Recipe No: " + recipeno + " not found in CMS. Please check!");
                            if (RecipeSuccessdMessage != "")
                                tran.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new HttpException(500, ex.Message);
                    }
                }
            }
        }

        [WebMethod]
        public static IList<CompanyProject> GetDim1(short CompanyId) {
            IList<CompanyProject> lstProject = null;
            lstProject = new SystemDataActivity().RetrieveAllCompanyProjectListByCompanyIDSortByProjectID(CompanyId);
           
            return lstProject;
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetDim2(short CompanyId, short ProjectId) {
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsDepartments = new DataSet();

            dacl.GetDepartmentByDivision(CompanyId, Convert.ToInt16(ProjectId), ref dsDepartments);

            return GMSUtil.ToJson(dsDepartments, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetDim3(short CompanyId, short DepartmentId) {
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsSections = new DataSet();

            dacl.GetCompanySection(CompanyId, Convert.ToInt16(DepartmentId), ref dsSections);

            return GMSUtil.ToJson(dsSections, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetDim4(short CompanyId, short SectionId) {
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsUnits = new DataSet();

            dacl.GetCompanyUnit(CompanyId, Convert.ToInt16(SectionId), ref dsUnits);

            return GMSUtil.ToJson(dsUnits, 0);
        }
    }
}
