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
using System.Transactions;
using GMSCore;
using GMSCore.Activity;
using System.Collections.Generic;
using System.Text;
using GMSCore.Entity;
using GMSCore.Exceptions;
using Wilson.ORMapper;

namespace GMSCore.Activity
{
    public class ApprovalActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public ApprovalActivity()
        {
        }
        #endregion
    
        /*
        public void InsertUpdateInfo(short coyID, short userId, string MRNo, string reason)
        {
            MRFormApproval lastFormApproval = new MRFormApprovalActivity().RetrieveLastFormApproval(coyID, MRNo);

            if ((lastFormApproval.Status == "U") && (lastFormApproval.ApproverUserID.ToString() == userId.ToString()))
            {
                lastFormApproval.Reason = lastFormApproval.Reason + "<br />" + reason;
                lastFormApproval.RoutedDate = DateTime.Now;
                lastFormApproval.ActionDate = DateTime.Now;
                lastFormApproval.RandomID = Guid.NewGuid().ToString();
                lastFormApproval.Save();
            }
            else
            {
                InsertApprovaLevelInfo(coyID, MRNo, 1, userId, "U", reason, "N", "Y", "Y", "N"); 
            }

            int i = 1;
            IList<MRFormApproval> lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalByCoyIDByMRNo(coyID, MRNo);
            foreach (MRFormApproval approve in lstApprove)
            {
                approve.Level = (short)i;
                approve.Save();
                i++;
            }
        }
        */


        // For Submit For Approval
        public void InsertApprovaLevelInfoList(short coyID, short userId, string MRNo, short PMUserId, short PHUserId, short PH3UserId, short alternateParty)
        {
            short purchaserUserId = 0;

            MRRole purchaser = new MRRoleActivity().RetrieveMainPurchaser(coyID);
            //ProductManagerProduct pH3 = new MRFormApprovalActivity().RetrievePH3(coyID);            
            
            if (purchaser != null)
                purchaserUserId = (short)purchaser.UserNumID;

            bool setNextLevel = false;

            if ((userId.ToString() != PMUserId.ToString()) && (userId.ToString() != PHUserId.ToString()) && (userId.ToString() != PH3UserId.ToString()) && (userId.ToString() != purchaserUserId.ToString()))
            {
                InsertApprovaLevelInfo(coyID, MRNo, 1, userId, "C", "-", "N", "Y", "Y", "N", alternateParty);
                if (PMUserId.ToString() != "0")
                {
                    InsertApprovaLevelInfo(coyID, MRNo, 2, PMUserId, "P", "-", "Y", "Y", "N", "N", 0);
                    setNextLevel = true;
                }

                if (PHUserId.ToString() != "0")
                {
                    if (setNextLevel)
                        InsertApprovaLevelInfo(coyID, MRNo, 3, PHUserId, "P", "-", "N", "N", "N", "N", 0);
                    else
                    {
                        InsertApprovaLevelInfo(coyID, MRNo, 3, PHUserId, "P", "-", "Y", "N", "N", "N", 0);
                        setNextLevel = true;
                    }
                }

                if (PH3UserId.ToString() != "0")
                {
                    if (setNextLevel)
                        InsertApprovaLevelInfo(coyID, MRNo, 3, PH3UserId, "P", "-", "N", "N", "N", "N", 0);
                    else
                    {
                        InsertApprovaLevelInfo(coyID, MRNo, 3, PH3UserId, "P", "-", "Y", "N", "N", "N", 0);
                        setNextLevel = true;
                    }
                }

                if (setNextLevel)
                    InsertApprovaLevelInfo(coyID, MRNo, 4, purchaserUserId, "P", "-", "N", "N", "N", "Y", 0);       
                else
                    InsertApprovaLevelInfo(coyID, MRNo, 4, purchaserUserId, "P", "-", "Y", "N", "N", "Y", 0);       

            }
            else if (userId.ToString() == PMUserId.ToString() && userId.ToString() == PHUserId.ToString())
            {
                InsertApprovaLevelInfo(coyID, MRNo, 1, userId, "C", "-", "N", "Y", "Y", "N", alternateParty);

                if (PH3UserId.ToString() != "0")
                {                    
                   InsertApprovaLevelInfo(coyID, MRNo, 3, PH3UserId, "P", "-", "Y", "N", "N", "N", 0);
                   setNextLevel = true;                  
                    
                }

                if (setNextLevel)
                    InsertApprovaLevelInfo(coyID, MRNo, 3, purchaserUserId, "P", "-", "N", "N", "N", "Y", 0);
                else
                    InsertApprovaLevelInfo(coyID, MRNo, 3, purchaserUserId, "P", "-", "Y", "N", "N", "Y", 0);

            }
            else if (userId.ToString() == PMUserId.ToString())
            {
                InsertApprovaLevelInfo(coyID, MRNo, 1, userId, "C", "-", "N", "Y", "Y", "N", alternateParty);

                if (PHUserId.ToString() != "0")
                {
                    InsertApprovaLevelInfo(coyID, MRNo, 2, PHUserId, "P", "-", "Y", "Y", "N", "N", 0);
                    setNextLevel = true;
                }

                if (PH3UserId.ToString() != "0")
                {
                    if (setNextLevel)
                        InsertApprovaLevelInfo(coyID, MRNo, 3, PH3UserId, "P", "-", "N", "N", "N", "N", 0);
                    else
                    {
                        InsertApprovaLevelInfo(coyID, MRNo, 3, PH3UserId, "P", "-", "Y", "N", "N", "N", 0);
                        setNextLevel = true;
                    }
                }

                if (setNextLevel)
                    InsertApprovaLevelInfo(coyID, MRNo, 3, purchaserUserId, "P", "-", "N", "N", "N", "Y", 0);
                else
                    InsertApprovaLevelInfo(coyID, MRNo, 3, purchaserUserId, "P", "-", "Y", "N", "N", "Y", 0);

            }
            else if (userId.ToString() == PHUserId.ToString())
            {
                InsertApprovaLevelInfo(coyID, MRNo, 1, userId, "C", "-", "N", "Y", "Y", "N", alternateParty);
                if (PH3UserId.ToString() != "0")
                {
                    InsertApprovaLevelInfo(coyID, MRNo, 2, PH3UserId, "P", "-", "Y", "Y", "N", "N", 0);
                    setNextLevel = true;
                }

                if (setNextLevel)
                    InsertApprovaLevelInfo(coyID, MRNo, 2, purchaserUserId, "P", "-", "N", "Y", "N", "Y", 0);
                else
                    InsertApprovaLevelInfo(coyID, MRNo, 2, purchaserUserId, "P", "-", "Y", "Y", "N", "Y", 0);
            }
            else if (userId.ToString() == PH3UserId.ToString())
            {
                InsertApprovaLevelInfo(coyID, MRNo, 1, userId, "C", "-", "N", "Y", "Y", "N", alternateParty);
                InsertApprovaLevelInfo(coyID, MRNo, 2, purchaserUserId, "P", "-", "Y", "Y", "N", "Y", 0);
            }
            else if (userId.ToString() == purchaserUserId.ToString())
            {
                InsertApprovaLevelInfo(coyID, MRNo, 1, userId, "C", "-", "N", "Y", "Y", "N", alternateParty);
                InsertApprovaLevelInfo(coyID, MRNo, 2, purchaserUserId, "P", "-", "Y", "Y", "N", "Y", 0);
            }

            int i = 1;
            IList<MRFormApproval> lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalByCoyIDByMRNo(coyID, MRNo);
            foreach (MRFormApproval approve in lstApprove)
            {
                approve.Level = (short)i;
                approve.Save();
                i++;
            }

            
        }

        public string ResubmitInsertApprovaLevelInfoList(short coyID, short userId, string MRNo, short PMUserId, short PHUserId, short PH3UserId, short reSubmitUserId, string reason, string action, bool updatePMPHFromNIL, short alternateParty)
        {
            short purchaserUserId = 0;
            short mainPurchaserUserId = 0;
            short currentLevelUserRank = 0;
            short reSubmitUserRank = 0;
            short initialLevel = 0;
            
            initialLevel = userId;
            try
            {                
                    MRRole mainPurchaser = new MRRoleActivity().RetrieveMainPurchaser(coyID);
                    
                    if (mainPurchaser != null)
                        mainPurchaserUserId = (short)mainPurchaser.UserNumID;

                    MRRole purchaser = new MRRoleActivity().RetrievePurchaser(coyID, reSubmitUserId);

                    if (purchaser != null)
                        purchaserUserId = (short)purchaser.UserNumID;
                    else
                        purchaserUserId = mainPurchaserUserId;

                    //1 - requestor, 2 - PM, 3 - PH, 4 - PH3 ,5 - Purchaser
                    if (userId.ToString() == purchaserUserId.ToString())
                        currentLevelUserRank = 5;
                    else if ((userId.ToString() == PH3UserId.ToString()) && (PH3UserId.ToString() != "0"))
                        currentLevelUserRank = 4;
                    else if (userId.ToString() == PHUserId.ToString())
                        currentLevelUserRank = 3;
                    else if (userId.ToString() == PMUserId.ToString())
                        currentLevelUserRank = 2;
                    else
                        currentLevelUserRank = 1;

                    if (reSubmitUserId.ToString() == purchaserUserId.ToString())
                        reSubmitUserRank = 5;
                    else if ((reSubmitUserId.ToString() == PH3UserId.ToString()) && (PH3UserId.ToString() != "0"))
                        reSubmitUserRank = 4;
                    else if (reSubmitUserId.ToString() == PHUserId.ToString())
                        reSubmitUserRank = 3;
                    else if (reSubmitUserId.ToString() == PMUserId.ToString())
                        reSubmitUserRank = 2;
                    else
                        reSubmitUserRank = 1;

                    if ((reSubmitUserRank < currentLevelUserRank) && (action == "R"))
                        userId = reSubmitUserId;

                    MRFormApproval lastFormApproval = new MRFormApprovalActivity().RetrieveLastFormApproval(coyID, MRNo);

                    if (action == "R")
                    {
                        if ((lastFormApproval.Status == "S") && (lastFormApproval.ApproverUserID.ToString() == reSubmitUserId.ToString()))
                        {
                            string[] reasonInDatabase = lastFormApproval.Reason.ToString().Split(new string[] { "<br />" }, StringSplitOptions.RemoveEmptyEntries);
                            string temp = reasonInDatabase[reasonInDatabase.Length-1];

                            if (temp != reason.Trim())
                                lastFormApproval.Reason = lastFormApproval.Reason + "<br />" + reason;
                            lastFormApproval.RoutedDate = DateTime.Now;
                            lastFormApproval.ActionDate = DateTime.Now;
                            lastFormApproval.RandomID = Guid.NewGuid().ToString();
                            lastFormApproval.Save();

                        }
                        else
                        {
                            InsertApprovaLevelInfo(coyID, MRNo, 1, reSubmitUserId, "S", reason, "N", "Y", "Y", "N", alternateParty);
                        }

                    }
                    else if (action == "U")
                    {
                        if ((lastFormApproval.Status == "U") && (lastFormApproval.ApproverUserID.ToString() == reSubmitUserId.ToString()))
                        {
                            string[] reasonInDatabase = lastFormApproval.Reason.ToString().Split(new string[] { "<br />" }, StringSplitOptions.RemoveEmptyEntries);
                            string temp = reasonInDatabase[reasonInDatabase.Length - 1];

                            if (temp != reason.Trim())
                                lastFormApproval.Reason = lastFormApproval.Reason + "<br />" + reason;
                            lastFormApproval.RoutedDate = DateTime.Now;
                            lastFormApproval.ActionDate = DateTime.Now;
                            lastFormApproval.RandomID = Guid.NewGuid().ToString();
                            lastFormApproval.Save();
                        }
                        else
                        {
                            InsertApprovaLevelInfo(coyID, MRNo, 1, reSubmitUserId, "U", reason, "N", "Y", "Y", "N", alternateParty);
                        }

                    }

                    bool setNextLevel = false;

                    //Insert Future Routing Info

                    if ((userId.ToString() == PMUserId.ToString()) && (userId.ToString() == PHUserId.ToString()) && (PMUserId.ToString() != "0") && (PHUserId.ToString() != "0"))
                    {
                        InsertApprovaLevelInfo(coyID, MRNo, 2, PMUserId, "P", "-", "Y", "Y", "N", "N", 0);

                        if (PH3UserId.ToString() != "0")
                            InsertApprovaLevelInfo(coyID, MRNo, 3, PH3UserId, "P", "-", "N", "N", "N", "N", 0);

                        if (mainPurchaserUserId.ToString() != "0")
                            InsertApprovaLevelInfo(coyID, MRNo, 3, mainPurchaserUserId, "P", "-", "N", "Y", "N", "Y", 0);

                    }
                    else if ((userId.ToString() == PMUserId.ToString()) && (PMUserId.ToString() != "0"))
                    {
                        InsertApprovaLevelInfo(coyID, MRNo, 2, PMUserId, "P", "-", "Y", "Y", "N", "N", 0);
                        if (PHUserId.ToString() != "0")
                            InsertApprovaLevelInfo(coyID, MRNo, 3, PHUserId, "P", "-", "N", "Y", "N", "N", 0);

                        if (PH3UserId.ToString() != "0")
                            InsertApprovaLevelInfo(coyID, MRNo, 3, PH3UserId, "P", "-", "N", "N", "N", "N", 0);

                        if (mainPurchaserUserId.ToString() != "0")
                            InsertApprovaLevelInfo(coyID, MRNo, 3, mainPurchaserUserId, "P", "-", "N", "Y", "N", "Y", 0);

                    }
                    else if ((userId.ToString() == PHUserId.ToString()) && (PHUserId.ToString() != "0"))
                    {
                        InsertApprovaLevelInfo(coyID, MRNo, 2, PHUserId, "P", "-", "Y", "Y", "N", "N", 0);

                        if (PH3UserId.ToString() != "0")
                            InsertApprovaLevelInfo(coyID, MRNo, 3, PH3UserId, "P", "-", "N", "N", "N", "N", 0);

                        if (mainPurchaserUserId.ToString() != "0")
                            InsertApprovaLevelInfo(coyID, MRNo, 3, mainPurchaserUserId, "P", "-", "N", "Y", "N", "Y", 0);
                    }
                    else if ((userId.ToString() == PH3UserId.ToString()) && (PH3UserId.ToString() != "0"))
                    {
                        InsertApprovaLevelInfo(coyID, MRNo, 2, PH3UserId, "P", "-", "Y", "Y", "N", "N", 0);

                        if (mainPurchaserUserId.ToString() != "0")
                            InsertApprovaLevelInfo(coyID, MRNo, 3, mainPurchaserUserId, "P", "-", "N", "Y", "N", "Y", 0);
                    }
                    else if ((userId.ToString() == mainPurchaserUserId.ToString()) && (mainPurchaserUserId.ToString() != "0"))
                    {
                        InsertApprovaLevelInfo(coyID, MRNo, 2, mainPurchaserUserId, "P", "-", "Y", "Y", "N", "Y", 0);
                    }
                    else if (action == "R")
                    {
                        if (PMUserId.ToString() != "0")
                        {
                            InsertApprovaLevelInfo(coyID, MRNo, 2, PMUserId, "P", "-", "Y", "Y", "N", "N", 0);
                            setNextLevel = true;
                        }

                        if (PHUserId.ToString() != "0")
                        {
                            if (setNextLevel)
                                InsertApprovaLevelInfo(coyID, MRNo, 3, PHUserId, "P", "-", "N", "N", "N", "N", 0);
                            else
                            {
                                InsertApprovaLevelInfo(coyID, MRNo, 3, PHUserId, "P", "-", "Y", "N", "N", "N", 0);
                                setNextLevel = true;
                            }
                        }

                        if (PH3UserId.ToString() != "0")
                        {
                            if (setNextLevel)
                                InsertApprovaLevelInfo(coyID, MRNo, 3, PH3UserId, "P", "-", "N", "N", "N", "N", 0);
                            else
                            {
                                InsertApprovaLevelInfo(coyID, MRNo, 3, PH3UserId, "P", "-", "Y", "N", "N", "N", 0);
                                setNextLevel = true;
                            }
                        }

                        if (setNextLevel)
                            InsertApprovaLevelInfo(coyID, MRNo, 4, mainPurchaserUserId, "P", "-", "N", "N", "N", "Y", 0);
                        else
                            InsertApprovaLevelInfo(coyID, MRNo, 4, mainPurchaserUserId, "P", "-", "Y", "N", "N", "Y", 0);

                    }

                    MR mr = new MRActivity().RetrieveMRByMRNo(coyID, MRNo);
                    if (mr != null && action == "R")
                    {
                        mr.StatusID = "F";
                        mr.Save();
                    }

                    int i = 1;
                    IList<MRFormApproval> lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalByCoyIDByMRNo(coyID, MRNo);
                    foreach (MRFormApproval approve in lstApprove)
                    {
                        approve.Level = (short)i;
                        approve.Save();
                        i++;
                    }                  
                    return  "OK";               
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();                
            }

              
        }

        public void InsertApprovaLevelInfo(short coyID, string mrno, short level, short userId, string status, string reason, string isCurrentLevel, string routedDate, string actionDate, string isLastLevel, short alternateParty)
        {
            GMSCore.Entity.MRFormApproval mrfa = new GMSCore.Entity.MRFormApproval();
            mrfa.CoyID = coyID;
            mrfa.MRNo = mrno;
            mrfa.Level = level;
            mrfa.ApproverUserID = userId;
            mrfa.Status = status;
            mrfa.Reason = reason;            
            mrfa.RandomID = Guid.NewGuid().ToString();
            mrfa.IsCurrentLevel = isCurrentLevel;
            mrfa.IsLastLevel = isLastLevel;
            mrfa.CreatedDate = DateTime.Now;
            if (routedDate == "Y")
                mrfa.RoutedDate = DateTime.Now;
            if (actionDate == "Y")
                mrfa.ActionDate = DateTime.Now;
            

            if (isCurrentLevel == "Y")            
                mrfa.IsNew = 1;                      
            
            if (status == "X")           
                mrfa.IsNew = 1;

            if (userId.ToString() != alternateParty.ToString())               
                mrfa.AlternateParty = alternateParty;

            mrfa.Save();
            mrfa.Resync();
        }

        public string ApproveMR(int levelID, string purchaser, short alternateParty)
        {
            MRFormApproval approve = new MRFormApprovalActivity().RetrieveFormApprovalByLevelID(levelID);
            if (approve != null)
            {
                if ((approve.Status.ToString() == "P") && (approve.IsCurrentLevel.ToString() == "Y"))
                {
                    approve.Status = "A";
                    approve.IsCurrentLevel = "N";

                    if (approve.ApproverUserID.ToString() != alternateParty.ToString())
                        approve.AlternateParty = alternateParty;

                    if (approve.IsLastLevel.ToString() == "Y")
                    {
                        approve.IsNew = 1;
                        // Check for Invalid Order Qty
                        IList<MRDetail> lstMRDetail = new MRActivity().RetrieveInvalidOrderQtyMRDetailsByCoyIDMRNo(approve.CoyID, approve.MRNo);
                        if (lstMRDetail.Count > 0)
                        {
                            return "OrderQty Error";
                        }
                                            

                    }
                    else
                    {
                        approve.IsNew = 0;
                    }

                    approve.ActionDate = DateTime.Now;
                    approve.Save();

                    if (approve.IsLastLevel.ToString() == "Y")
                    {
                        MR mr = new MRActivity().RetrieveMRByMRNo(approve.CoyID, approve.MRNo);
                        if (mr != null)
                        {
                            mr.StatusID = "A";
                            mr.Purchaser = purchaser;
                            mr.Save();
                        }
                    }
                    else
                    {

                        short nextlevel = GMSUtil.ToShort(approve.Level + 1);
                        MRFormApproval nextApprover = new MRFormApprovalActivity().RetrieveNextApproverByCoyIDByMRNoByLevel(approve.CoyID, approve.MRNo, nextlevel);
                        if (nextApprover != null)
                        {
                            nextApprover.Status = "P";
                            nextApprover.IsCurrentLevel = "Y";
                            nextApprover.RoutedDate = DateTime.Now;
                            nextApprover.IsNew = 1;
                            nextApprover.Save();
                            nextApprover.Resync();

                        }
                    }
                    return "OK";
                }
                else
                {
                    return "";
                }
                
            }
            else
            {
                return "";
            }
        }

        public string RejectMR(short coyId, string MRNo, int levelID, string reason, short alternateParty)
        { 
            MRFormApproval fa = new MRFormApprovalActivity().RetrieveFormApprovalByLevelID(levelID);
            if ((fa != null) && (fa.Status.ToString() == "P") && (fa.IsCurrentLevel.ToString() == "Y"))
            {
                IList<MRFormApproval> lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalByCoyIDByMRNoByLevelId(coyId, MRNo, levelID);

                if (lstApprove.Count > 0)
                {
                    foreach (MRFormApproval approve in lstApprove)
                    {
                        if (approve.LevelID.ToString() == levelID.ToString() && (approve.Status.ToString() == "P") && (approve.IsCurrentLevel.ToString() == "Y"))
                        {
                            approve.Status = "R";
                            approve.IsCurrentLevel = "N";
                            approve.IsNew = 1;
                            approve.ActionDate = DateTime.Now;
                            approve.Reason = reason.ToString();

                            if (approve.ApproverUserID.ToString() != alternateParty.ToString())
                                approve.AlternateParty = alternateParty;

                            approve.Save();
                        }
                        else if (approve.Status.ToString() == "P")
                        {
                            new MRFormApprovalActivity().DeleteMRApproval(approve.LevelID);
                        }
                    }

                    MR mr = new MRActivity().RetrieveMRByMRNo(coyId, MRNo);
                    if (mr != null)
                    {
                        mr.StatusID = "R";
                        mr.Save();
                    }

                    return "OK";

                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

        }

        public string ReSubmitMR(short CompanyId, short UserId, string MRNo, short PMUserId, short PHUserId, short PH3UserId, int levelId, string reason, string action, bool updatePMPHFromNIL, short alternateParty)
        {
            
                MRFormApproval latest = new MRFormApprovalActivity().RetrieveCurrentLevelPendingFormApproval(CompanyId, MRNo);
                string status = "";

                short currentLevelUserId = 0;

                if (latest != null)
                {
                    IList<MRFormApproval> lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalByCoyIDByMRNoByLevelId(CompanyId, MRNo, latest.LevelID);

                    foreach (MRFormApproval approve in lstApprove)
                    {
                        if (approve.Status.ToString() == "P")
                        {
                            new MRFormApprovalActivity().DeleteMRApproval(approve.LevelID);
                        }

                    }

                    currentLevelUserId = latest.ApproverUserID;
                }
                else
                {
                    currentLevelUserId = 0;
                }

                status = ResubmitInsertApprovaLevelInfoList(CompanyId, currentLevelUserId, MRNo, PMUserId, PHUserId, PH3UserId, UserId, reason, action, updatePMPHFromNIL, alternateParty);

                return status;           
            
        }

        public string HighLevelApproveMR(short coyId, string MRNo, int levelID, string purchaser, string reason, short alternateParty)
        {
            MRFormApproval fa = new MRFormApprovalActivity().RetrieveFormApprovalByLevelID(levelID);
            if ((fa != null) && (fa.Status == "P"))
            {
                IList<MRFormApproval> lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalForByPass(coyId, MRNo, levelID);

                if (lstApprove.Count > 0)
                {

                    foreach (MRFormApproval approve in lstApprove)
                    {
                        if ((approve.LevelID.ToString() == levelID.ToString()) && (approve.Status.ToString() == "P"))
                        {
                            approve.Status = "A";
                            approve.IsCurrentLevel = "N";
                            approve.Reason = reason;

                            if (approve.ApproverUserID.ToString() != alternateParty.ToString())
                                approve.AlternateParty = alternateParty;

                            if (approve.IsLastLevel.ToString() == "Y")
                            {
                                approve.IsNew = 1;
                                // Check for Invalid Order Qty
                                IList<MRDetail> lstMRDetail = new MRActivity().RetrieveInvalidOrderQtyMRDetailsByCoyIDMRNo(approve.CoyID, approve.MRNo);
                                if (lstMRDetail.Count > 0)
                                {
                                    return "OrderQty Error";
                                }

                                
                            }
                            else
                            {
                                approve.IsNew = 0;
                            }

                            approve.ActionDate = DateTime.Now;
                            approve.Save();

                            if (approve.IsLastLevel.ToString() == "Y")
                            {
                                MR mr = new MRActivity().RetrieveMRByMRNo(approve.CoyID, approve.MRNo);
                                if (mr != null)
                                {
                                    mr.StatusID = "A";
                                    mr.Purchaser = purchaser;
                                    mr.Save();
                                }
                            }
                            else
                            {

                                short nextlevel = GMSUtil.ToShort(approve.Level + 1);
                                MRFormApproval nextApprover = new MRFormApprovalActivity().RetrieveNextApproverByCoyIDByMRNoByLevel(approve.CoyID, approve.MRNo, nextlevel);
                                if (nextApprover != null)
                                {
                                    if (nextApprover.Status == "P")
                                    {
                                        nextApprover.IsCurrentLevel = "Y";
                                        nextApprover.RoutedDate = DateTime.Now;
                                        nextApprover.IsNew = 1;
                                        nextApprover.Save();
                                        nextApprover.Resync();
                                    }
                                }
                            }
                        }
                        else
                        {
                            approve.Status = "B";
                            approve.IsCurrentLevel = "N";
                            approve.ActionDate = DateTime.Now;
                            approve.IsNew = 1;
                            approve.Save();
                        }
                    }

                    return "OK";
                }
                else
                {
                    return "";
                }
                                
            }
            else
            {
                return "";
                

            }

        }

        public string HighLevelRejectMR(short coyId, string MRNo, int levelID, string reason, short alternateParty)
        {
            MRFormApproval fa = new MRFormApprovalActivity().RetrieveFormApprovalByLevelID(levelID);
            if ((fa != null) && (fa.Status == "P"))
            {

                IList<MRFormApproval> lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalForByPass(coyId, MRNo, levelID);

                if (lstApprove.Count > 0)
                {
                    foreach (MRFormApproval approve in lstApprove)
                    {
                        if ((approve.LevelID.ToString() == levelID.ToString()) && (approve.Status.ToString() == "P"))
                        {
                            approve.Status = "R";
                            approve.IsCurrentLevel = "N";
                            approve.IsNew = 1;
                            approve.ActionDate = DateTime.Now;
                            approve.Reason = reason.ToString();
                            if (approve.ApproverUserID.ToString() != alternateParty.ToString())
                                approve.AlternateParty = alternateParty;
                            approve.Save();
                        }
                        else
                        {
                            approve.Status = "B";
                            approve.IsCurrentLevel = "N";
                            approve.ActionDate = DateTime.Now;
                            approve.Save();
                        }
                    }

                    lstApprove = lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalByCoyIDByMRNoByLevelId(coyId, MRNo, levelID);

                    foreach (MRFormApproval approve in lstApprove)
                    {
                        if ((approve.LevelID.ToString() != levelID.ToString()) && (approve.Status.ToString() == "P"))
                        {
                            new MRFormApprovalActivity().DeleteMRApproval(approve.LevelID);
                        }
                    }

                    MR mr = new MRActivity().RetrieveMRByMRNo(coyId, MRNo);
                    if (mr != null)
                    {
                        mr.StatusID = "R";
                        mr.Save();
                    }

                    return "OK";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

            
        }

        public void CancelMR(short coyId, short userId, string MRNo, int levelID, string reason, short alternateParty)
        {
            IList<MRFormApproval> lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalByCoyIDByMRNoByLevelId(coyId, MRNo, levelID);

            bool hasApproval = false;

            foreach (MRFormApproval approve in lstApprove)
            {
                hasApproval = true;
                if (approve.Status.ToString() == "P")                
                    new MRFormApprovalActivity().DeleteMRApproval(approve.LevelID);                
            }

            if (hasApproval)
            {
                InsertApprovaLevelInfo(coyId, MRNo, 1, userId, "X", reason, "N", "N", "Y", "N", alternateParty);
                int i = 1;
                lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalByCoyIDByMRNo(coyId, MRNo);
                foreach (MRFormApproval approve in lstApprove)
                {
                    approve.Level = (short)i;
                    approve.Save();
                    i++;
                }
            } 

            MR mr = new MRActivity().RetrieveMRByMRNo(coyId, MRNo);
            if (mr != null)
            {
                if ((mr.StatusID.ToString() == "P") && (mr.StatusID.ToString() == "A") && (mr.StatusID.ToString() == "R"))
                {
                    InsertApprovaLevelInfo(coyId, MRNo, 1, userId, "X", reason, "N", "N", "Y", "N", alternateParty);                    
                }
                mr.StatusID = "X";
                mr.CancelledReason = reason.ToString();
                mr.Save();               
            }         

        }

        public void HighLevelCancelMR(short coyId, short userId, string MRNo, int levelID, string reason, short alternateParty)
        {

            IList<MRFormApproval> lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalForByPass(coyId, MRNo, levelID);

            foreach (MRFormApproval approve in lstApprove)
            {
                if ((approve.LevelID.ToString() == levelID.ToString()))
                    InsertApprovaLevelInfo(approve.CoyID, approve.MRNo, approve.Level, approve.ApproverUserID, "X", reason, "N", "N", "Y", "N", alternateParty);                    
                else
                    InsertApprovaLevelInfo(approve.CoyID, approve.MRNo, approve.Level, approve.ApproverUserID, "B", "-", "N", "N", "Y", "N", alternateParty);                    
                
                new MRFormApprovalActivity().DeleteMRApproval(approve.LevelID);
            }

            lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalByCoyIDByMRNoByLevelId(coyId, MRNo, levelID);

            foreach (MRFormApproval approve in lstApprove)
            {
                if ((approve.LevelID.ToString() != levelID.ToString()) && (approve.Status.ToString() == "P"))               
                    new MRFormApprovalActivity().DeleteMRApproval(approve.LevelID);
                
            }

            MR mr = new MRActivity().RetrieveMRByMRNo(coyId, MRNo);
            if (mr != null)
            {
                mr.StatusID = "X";
                mr.CancelledReason = reason.ToString();
                mr.Save();
            }

            int i = 1;
            lstApprove = new MRFormApprovalActivity().RetrieveListFormApprovalByCoyIDByMRNo(coyId, MRNo);
            foreach (MRFormApproval approve in lstApprove)
            {
                approve.Level = (short)i;
                approve.Save();
                i++;
            }

        }

        public string UndoCancellation(short coyId, short userId, string MRNo, short alternateParty)
        {
            MRFormApproval lastFormApproval = new MRFormApprovalActivity().RetrieveLastFormApproval(coyId, MRNo);
            if (lastFormApproval != null)
            {
                if (lastFormApproval.Status == "X")
                {
                    lastFormApproval.IsNew = 0;
                    lastFormApproval.Save();
                    new ApprovalActivity().InsertApprovaLevelInfo(coyId, MRNo, 1, userId, "Y", "", "N", "N", "Y", "N", alternateParty);

                    MR mr = new MRActivity().RetrieveMRByMRNo(coyId, MRNo);
                    if (mr != null)
                    {
                        mr.StatusID = "D";
                        mr.CancelledReason = "";
                        mr.Save();
                    }
                    return "OK";
                }
                else
                    return "";
            }
            else
            {
                MR mr = new MRActivity().RetrieveMRByMRNo(coyId, MRNo);
                if (mr != null)
                {
                    mr.StatusID = "D";
                    mr.CancelledReason = "";
                    mr.Save();
                }
                return "OK";
            }
            
        }
    }
}
