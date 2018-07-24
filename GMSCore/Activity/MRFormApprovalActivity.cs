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
    public class MRFormApprovalActivity : ActivityBase   
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public MRFormApprovalActivity()
        {
        }
        #endregion

        #region RetrieveFormApprovalByCoyIDByUser
        public MRFormApproval RetrieveFormApprovalByCoyIDByUser(short coyid, string mrno, short approverUserId, int latest)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                               helper.CleanValue(coyid));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                               helper.CleanValue(mrno));            
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.ApproverUserID"),
                               helper.CleanValue(approverUserId));
            stb.AppendFormat(" AND {0} >= {1} ", helper.GetFieldName("MRFormApproval.LevelID"),
                               helper.CleanValue(latest));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.Status"),
                               helper.CleanValue("P"));

            return MRFormApproval.RetrieveFirst(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("MRFormApproval.LevelID")));
        }
        #endregion



        #region RetrieveFormApprovalByLevelID
        public MRFormApproval RetrieveFormApprovalByLevelID(int levelID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.LevelID"),
                                helper.CleanValue(levelID));            

            return MRFormApproval.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveFormApprovalByCoyIDByUser
        public MRFormApproval RetrieveNextApproverByCoyIDByMRNoByLevel(short coyid, string mrno, short level)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyid));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                                helper.CleanValue(mrno));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.Level"),
                               helper.CleanValue(level));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.Status"),
                               helper.CleanValue("P"));

            return MRFormApproval.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveListFormApprovalByCoyIDByMRNo
        public IList<MRFormApproval> RetrieveListFormApprovalByCoyIDByMRNoByLevelId(short coyid, string mrno, int levelId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyid));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                                helper.CleanValue(mrno));
            stb.AppendFormat(" AND {0} >= {1} ", helper.GetFieldName("MRFormApproval.LevelID"),
                               helper.CleanValue(levelId));

            return MRFormApproval.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("MRFormApproval.LevelID")));
        }
        #endregion

        #region RetrieveListFormApprovalForByPass
        public IList<MRFormApproval> RetrieveListFormApprovalForByPass(short coyid, string mrno, int levelId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyid));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                                helper.CleanValue(mrno));
            stb.AppendFormat(" AND {0} <= {1} ", helper.GetFieldName("MRFormApproval.LevelID"),
                               helper.CleanValue(levelId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.Status"),
                               helper.CleanValue("P"));

            return MRFormApproval.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("MRFormApproval.LevelID")));
        }
        #endregion

        #region RetrieveListFormApprovalByCoyIDByMRNo2
        public IList<MRFormApproval> RetrieveListFormApprovalByCoyIDByMRNo(short coyid, string mrno)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyid));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                                helper.CleanValue(mrno));
           

            return MRFormApproval.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("MRFormApproval.LevelID")));
        }
        #endregion



        #region DeleteMRApproval
        public ResultType DeleteMRApproval(int levelId)
        {
            if (levelId == null)
                return ResultType.NullMainData;

          
            MRFormApproval fa = RetrieveFormApprovalByLevelID(levelId);
            if (fa == null)
                return ResultType.Error;

            fa.Delete();
            return ResultType.Ok;
        }
        #endregion

        #region RetrieveValidFormApproval
        public MRFormApproval RetrieveValidFormApproval(short levelId,short coyId, string mrNo, string randomId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.LevelID"),
                                helper.CleanValue(levelId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                               helper.CleanValue(coyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.RandomID"),
                               helper.CleanValue(randomId));

            return MRFormApproval.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveLastestFormApproval
        public MRFormApproval RetrieveLastestFormApproval(short coyId, string mrNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyId));            
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND ({0} = {1} OR {0} = {2}) ", helper.GetFieldName("MRFormApproval.Status"),
                               helper.CleanValue("S"), helper.CleanValue("C"));

            return MRFormApproval.RetrieveFirst(stb.ToString(), string.Format(" {0} DESC ", helper.GetFieldName("MRFormApproval.LevelID")));
           
        }
        #endregion

        #region RetrieveListFormApprovalForRejectedMR
        public IList<MRFormApproval> RetrieveListFormApprovalForRejectedMR(short coyid, string mrno, short levelId, short latest)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyid));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                                helper.CleanValue(mrno));
            stb.AppendFormat(" AND ({0} <= {1} AND {0} >= {2}) ", helper.GetFieldName("MRFormApproval.LevelID"),
                               helper.CleanValue(levelId), helper.CleanValue(latest));
           

            return MRFormApproval.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("MRFormApproval.LevelID")));
        }
        #endregion

        #region RetrieveLastCancelFormApproval
        public MRFormApproval RetrieveLastCancelFormApproval(short coyId, string mrNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.Status"),
                               helper.CleanValue("X"));

            return MRFormApproval.RetrieveFirst(stb.ToString(), string.Format(" {0} DESC ", helper.GetFieldName("MRFormApproval.LevelID")));

        }
        #endregion

        #region RetrieveFormApprovalByCoyIDByUser
        public MRFormApproval RetrieveRequestedFormApprovalByCoyIDByUser(short coyid, string mrno, short approverUserId, int latest)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                               helper.CleanValue(coyid));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                               helper.CleanValue(mrno));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.ApproverUserID"),
                               helper.CleanValue(approverUserId));
            stb.AppendFormat(" AND {0} >= {1} ", helper.GetFieldName("MRFormApproval.LevelID"),
                               helper.CleanValue(latest));
            stb.AppendFormat(" AND ({0} = {1} OR {0} = {2}) ", helper.GetFieldName("MRFormApproval.Status"),
                              helper.CleanValue("S"), helper.CleanValue("C"));

            return MRFormApproval.RetrieveFirst(stb.ToString(), string.Format(" {0} DESC ", helper.GetFieldName("MRFormApproval.LevelID")));
        }
        #endregion

        #region RetrieveAllFormApprovalByCoyIDByMRNo
        public IList<MRFormApproval> RetrieveAllFormApprovalByCoyIDByMRNo(short coyid, string mrno)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyid));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                                helper.CleanValue(mrno));            

            return MRFormApproval.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("MRFormApproval.LevelID")));
        }
        #endregion

        #region RetrieveLastUndoCancellationFormApproval
        public MRFormApproval RetrieveLastUndoCancellationFormApproval(short coyId, string mrNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.Status"),
                               helper.CleanValue("Y"));

            return MRFormApproval.RetrieveFirst(stb.ToString(), string.Format(" {0} DESC ", helper.GetFieldName("MRFormApproval.LevelID")));

        }
        #endregion


        #region RetrieveCurrentLevelPendingFormApproval
        public MRFormApproval RetrieveCurrentLevelPendingFormApproval(short coyId, string mrNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.Status"),
                               helper.CleanValue("P"));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.IsCurrentLevel"),
                               helper.CleanValue("Y"));

            return MRFormApproval.RetrieveFirst(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("MRFormApproval.LevelID")));

        }
        #endregion


        #region RetrieveValidFormApprovalByRandomID
        public MRFormApproval RetrieveValidFormApprovalByRandomID(int levelId, string randomId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.LevelID"),
                                helper.CleanValue(levelId));            
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.RandomID"),
                               helper.CleanValue(randomId));

            return MRFormApproval.RetrieveFirst(stb.ToString());
        }
        #endregion


        #region RetrieveLastFormApproval
        public MRFormApproval RetrieveLastFormApproval(short coyId, string mrNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                               helper.CleanValue(mrNo));      
            
            return MRFormApproval.RetrieveFirst(stb.ToString(), string.Format(" {0} DESC ", helper.GetFieldName("MRFormApproval.LevelID")));

        }
        #endregion

        #region RetrieveProductTeam
        public ProductManagerProduct RetrieveProductTeam(short coyId, short PMUserId, short PHUserId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ProductManagerProduct.CoyID"),
                                helper.CleanValue(coyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ProductManagerProduct.PMUserID"),
                               helper.CleanValue(PMUserId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ProductManagerProduct.PHUserID"),
                              helper.CleanValue(PHUserId));          

            return ProductManagerProduct.RetrieveFirst(stb.ToString(), string.Format(" {0} DESC ", helper.GetFieldName("ProductManagerProduct.PHUserID")));

        }
        #endregion

        #region RetrievePH3
        public ProductManagerProduct RetrievePH3(short coyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ProductManagerProduct.CoyID"),
                                helper.CleanValue(coyId));
            stb.AppendFormat(" AND {0} > {1} ", helper.GetFieldName("ProductManagerProduct.PH3UserID"),
                               helper.CleanValue(0));

            return ProductManagerProduct.RetrieveFirst(stb.ToString(), string.Format(" {0} DESC ", helper.GetFieldName("ProductManagerProduct.PH3UserID")));

        }
        #endregion



        #region RetrieveFormApprovalByUserIdAndMRNo
        public MRFormApproval RetrieveFormApprovalByUserIdAndMRNo(short coyId, string mrNo, short approverUserId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRFormApproval.CoyID"),
                                helper.CleanValue(coyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRFormApproval.ApproverUserID"),
                               helper.CleanValue(approverUserId));            

            return MRFormApproval.RetrieveFirst(stb.ToString(), string.Format(" {0} DESC ", helper.GetFieldName("MRFormApproval.LevelID")));

        }
        #endregion



        
    }
}
