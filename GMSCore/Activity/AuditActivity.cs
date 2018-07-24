using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSCore.Exceptions;

using Wilson.ORMapper;

namespace GMSCore.Activity
{
    public class AuditActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public AuditActivity()
        {
        }
        #endregion

        #region RetrieveAuditByDateItemStructureID
        public IList<Audit> RetrieveAuditByDateItemStructureID(DateTime auditDate, short coyId, short itemStructureId)
        {
            if (coyId <= 0)
                return null;

            if (itemStructureId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Audit.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Audit.ItemStructureID"),
                                helper.CleanValue(itemStructureId));

            if (auditDate.Ticks != GMSUtil.DEFAULTNODATE.Ticks)
                stb.AppendFormat(" AND {0} ", helper.GetExpression("Audit.AuditDate", auditDate, ComparisonOperators.Equals));

            return Audit.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveAuditByYearItemID
        public IList<FinanceAuditData> RetrieveAuditByYearItemID(short year, short itemId, short coyId)
        {
            if (year <= 0 || itemId <=0 || coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FinanceAuditData.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceAuditData.ItemID"),
                                helper.CleanValue(itemId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("FinanceAuditData.TbYear"),
                                helper.CleanValue(year));
            
            return FinanceAuditData.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region DeleteAuditByYearItemStructureID
        public ResultType DeleteAuditByYearItemStructureID(DateTime auditDate, short itemStructureId, short coyId)
        {
            if (itemStructureId <= 0)
                return ResultType.NullMainData;

            if (coyId <= 0)
                return ResultType.NullMainData;

            IList<Audit> lstAudit = this.RetrieveAuditByDateItemStructureID(auditDate, coyId, itemStructureId);
            foreach (Audit audit in lstAudit)
            {
                audit.Delete();
                audit.Resync();
            }

            return ResultType.Ok;
        }
        #endregion

        #region DeleteAuditByYearItemID
        public ResultType DeleteAuditByYearItemID(short year, short itemId, short coyId)
        {
            if (year <= 0 || itemId <=0 || coyId <= 0)
                return ResultType.NullMainData;

            IList<FinanceAuditData> lstAudit = this.RetrieveAuditByYearItemID(year, itemId, coyId);
            foreach (FinanceAuditData audit in lstAudit)
            {
                audit.Delete();
                audit.Resync();
            }

            return ResultType.Ok;
        }
        #endregion

        #region CreateAudit
        /// <summary>
        /// Function to create a new Audit
        /// </summary>
        /// <param name="auditToCreate">Reference to a Audit object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateAudit(ref FinanceAuditData auditToCreate, LogSession session)
        {
            if (auditToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!auditToCreate.IsValid())
                return ResultType.MainDataNotValid;

            auditToCreate.Save();

            return ResultType.Ok;
        }
        #endregion
    }
}
