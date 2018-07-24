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
    public class ReportsActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public ReportsActivity()
        {
        }
        #endregion


        // Activities for ReportCategory
        #region RetrieveAllReportCategoryListSortBySeqID
        public IList<ReportCategory> RetrieveAllReportCategoryListSortBySeqID()
        {
            QueryHelper helper = base.GetHelper();

            return ReportCategory.RetrieveQuery("", string.Format("{0} ASC", helper.GetFieldName("ReportCategory.SeqID")));
        }

        public IList<ReportCategory> RetrieveAllReportCategoryListSortBySeqID(LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            return ReportCategory.RetrieveQuery("", string.Format("{0} ASC", helper.GetFieldName("ReportCategory.SeqID")));
        }

        public IList<ModuleReportCategory> RetrieveAllModuleReportCategoryListSortBySeqID()
        {
            QueryHelper helper = base.GetHelper();

            return ModuleReportCategory.RetrieveQuery("", string.Format("{0} ASC", helper.GetFieldName("ModuleReportCategory.SeqID")));
        }

        public IList<ModuleReportCategory> RetrieveAllModuleReportCategoryListByModuleCategoryIDSortBySeqID(short moduleCategoryId)
        {
            QueryHelper helper = base.GetHelper();

            return ModuleReportCategory.RetrieveQuery(string.Format("{0} = {1}", helper.GetFieldName("ModuleReportCategory.ModuleCategoryID"), helper.CleanValue(moduleCategoryId)), string.Format("{0} ASC", helper.GetFieldName("ModuleReportCategory.SeqID")));
        }
        #endregion

        #region GetReportCategoryMaxSeqID
        public int GetReportCategoryMaxSeqID()
        {
            DBManager db = DBManager.GetInstance();
            QueryHelper helper = base.GetHelper();

            int iMaxSeqID = GMSUtil.ToInt(
                                    db.Engine.ExecuteScalar(
                                        string.Format("SELECT MAX ( {0} ) FROM {1}",
                                            helper.GetFieldName("ReportCategory.SeqID"),
                                            helper.GetTableName("ReportCategory"))));

            return iMaxSeqID;
        }

        public int GetModuleReportCategoryMaxSeqID()
        {
            DBManager db = DBManager.GetInstance();
            QueryHelper helper = base.GetHelper();

            int iMaxSeqID = GMSUtil.ToInt(
                                    db.Engine.ExecuteScalar(
                                        string.Format("SELECT MAX ( {0} ) FROM {1}",
                                            helper.GetFieldName("ModuleReportCategory.SeqID"),
                                            helper.GetTableName("ModuleReportCategory"))));

            return iMaxSeqID;
        }
        #endregion

        #region RetrieveReportCategoryById
        public ReportCategory RetrieveReportCategoryById(short categoryId, LogSession session)
        {
            if (categoryId <= 0)
                return null;

            if (session == null)
                throw new NullSessionException();

            return ReportCategory.RetrieveByKey(categoryId);
        }

        public ModuleReportCategory RetrieveModuleReportCategoryById(short categoryId, LogSession session)
        {
            if (categoryId <= 0)
                return null;

            if (session == null)
                throw new NullSessionException();

            return ModuleReportCategory.RetrieveByKey(categoryId);
        }
        #endregion

        #region UpdateReportCategory
        /// <summary>
        /// Function to update ReportCategory
        /// </summary>
        /// <param name="reportCategoryToUpdate">Reference to a ReportCategory object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateReportCategory(ref ReportCategory reportCategoryToUpdate, LogSession session)
        {
            if (reportCategoryToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!reportCategoryToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            reportCategoryToUpdate.Save();

            return ResultType.Ok;
        }

        public ResultType UpdateModuleReportCategory(ref ModuleReportCategory reportCategoryToUpdate, LogSession session)
        {
            if (reportCategoryToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!reportCategoryToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            reportCategoryToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateReportCategory
        /// <summary>
        /// Function to create a new ReportCategory
        /// </summary>
        /// <param name="rCategoryToCreate">Reference to a ReportCategory object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateReportCategory(ref ReportCategory rCategoryToCreate, LogSession session)
        {
            if (rCategoryToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!rCategoryToCreate.IsValid())
                return ResultType.MainDataNotValid;

            //if (IsCodeInUsed(billingTermToCreate.Code))
            //    return ResultType.DuplicatedData;

            rCategoryToCreate.Save();

            return ResultType.Ok;
        }

        public ResultType CreateModuleReportCategory(ref ModuleReportCategory rCategoryToCreate, LogSession session)
        {
            if (rCategoryToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!rCategoryToCreate.IsValid())
                return ResultType.MainDataNotValid;

            //if (IsCodeInUsed(billingTermToCreate.Code))
            //    return ResultType.DuplicatedData;

            rCategoryToCreate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region DeleteReportCategory
        public ResultType DeleteReportCategory(short rCategoryIdToDelete, LogSession session)
        {
            if (rCategoryIdToDelete <= 0)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            ReportCategory rCategory = ReportCategory.RetrieveByKey(rCategoryIdToDelete);
            if (rCategory == null)
                return ResultType.Error;

            rCategory.Delete();
            return ResultType.Ok;
        }

        public ResultType DeleteModuleReportCategory(short rCategoryIdToDelete, LogSession session)
        {
            if (rCategoryIdToDelete <= 0)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            ModuleReportCategory rCategory = ModuleReportCategory.RetrieveByKey(rCategoryIdToDelete);
            if (rCategory == null)
                return ResultType.Error;

            rCategory.Delete();
            return ResultType.Ok;
        }
        #endregion



        // Activities for Report
        #region RetrieveAllReportSortBySeqIDName
        public IList<Report> RetrieveAllReportSortBySeqIDName()
        {
            QueryHelper helper = base.GetHelper();

            return Report.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Report.Description")));
        }

        public IList<ModuleReport> RetrieveAllModuleReportSortBySeqIDName()
        {
            QueryHelper helper = base.GetHelper();

            return ModuleReport.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("ModuleReport.Description")));
        }
        #endregion

        #region RetrieveAuditedReportSortBySeqIDName
        public IList<Report> RetrieveAuditedReportSortBySeqIDName()
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Report.ReportCategoryID"),
                                                helper.CleanValue(5));

            return Report.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("Report.FileName")));
        }
        #endregion

        #region RetrieveReportById
        public Report RetrieveReportById(short reportId)
        {
            if (reportId <= 0)
                return null;

            return Report.RetrieveByKey(reportId);
        }

        public ModuleReport RetrieveModuleReportById(short reportId)
        {
            if (reportId <= 0)
                return null;

            return ModuleReport.RetrieveByKey(reportId);
        }
        #endregion

        #region RetrieveReportByCategoryId
        public IList<Report> RetrieveReportByCategoryId(short rCategoryId)
        {
            if (rCategoryId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            ObjectQuery<Report> query;
            query = new ObjectQuery<Report>(string.Format(" {0} = {1} ",
                                                helper.GetFieldName("Report.ReportCategoryID"),
                                                helper.CleanValue(rCategoryId)),
                                            string.Format(" {0} ASC ",
                                                helper.GetFieldName("Report.Description"))
                                            );

            return Report.RetrieveQuery(query);

        }
        #endregion

        #region RetrieveReportCategoryByCategoryName
        public ReportCategory RetrieveReportCategoryByCategoryName(string name)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ReportCategory.Name"),
                              helper.CleanValue(name));
            return ReportCategory.RetrieveFirst(stb.ToString(), string.Format(" {0} ASC", helper.GetFieldName("ReportCategory.Name")));

        }
        #endregion

        public IList<VwReportListing> RetrieveReportByCategoryIdUserAccessId(short rCategoryId, short userId)
        {
            if (rCategoryId <= 0)
                return null;

            if (userId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            ObjectQuery<VwReportListing> query;

            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VwReportListing.ReportCategoryID"),
                                                helper.CleanValue(rCategoryId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("VwReportListing.UserNumID"),
                                                helper.CleanValue(userId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("VwReportListing.IsActive"),
                                                helper.CleanValue(true));

            query = new ObjectQuery<VwReportListing>(stb.ToString(),
                                            string.Format(" {0} ASC ",
                                                helper.GetFieldName("VwReportListing.Description"))
                                            );

            return VwReportListing.RetrieveQuery(query);

        }
        
        public IList<VwReportListingForCompany> RetrieveCompanyReportByCategoryIdUserAccessId(short coyid, short rCategoryId, short userId)
        {
            if (rCategoryId <= 0)
                return null;

            if (userId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            ObjectQuery<VwReportListingForCompany> query;

            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VwReportListingForCompany.ReportCategoryID"),
                                                helper.CleanValue(rCategoryId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("VwReportListingForCompany.UserNumID"),
                                                helper.CleanValue(userId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("VwReportListingForCompany.IsActive"),
                                                helper.CleanValue(true));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("VwReportListingForCompany.CoyID"),
                                                helper.CleanValue(coyid));

            query = new ObjectQuery<VwReportListingForCompany>(stb.ToString(),
                                            string.Format(" {0} ASC ",
                                                helper.GetFieldName("VwReportListingForCompany.Description"))
                                            );

            return VwReportListingForCompany.RetrieveQuery(query);

        }
        

        #region RetrieveReportGroupByCategory
        public IList<Report> RetrieveReportGroupByCategory()
        {
            QueryHelper helper = base.GetHelper();

            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Report.IsActive"),
                                                helper.CleanValue(true));

            ObjectQuery<Report> query;
            query = new ObjectQuery<Report>(stb.ToString(),
                                            string.Format(" {0} ASC, {1} ASC", helper.GetFieldName("Report.ReportCategoryID"), helper.GetFieldName("Report.Description"))
                                            );

            return Report.RetrieveQuery(query);

        }
        #endregion

        #region UpdateReport
        /// <summary>
        /// Function to update Report
        /// </summary>
        /// <param name="reportCategoryToUpdate">Reference to a Report object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateReport(ref Report reportToUpdate, LogSession session)
        {
            if (reportToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!reportToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            reportToUpdate.Save();

            return ResultType.Ok;
        }

        public ResultType UpdateModuleReport(ref ModuleReport reportToUpdate, LogSession session)
        {
            if (reportToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!reportToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            reportToUpdate.Save();

            return ResultType.Ok;
        }

        #endregion

        #region CreateReport
        /// <summary>
        /// Function to create a new Report
        /// </summary>
        /// <param name="rCategoryToCreate">Reference to a Report object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateReport(ref Report reportToCreate, LogSession session)
        {
            if (reportToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!reportToCreate.IsValid())
                return ResultType.MainDataNotValid;

            //billingTermToCreate.Code = billingTermToCreate.Code.ToUpper();

            reportToCreate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region DeleteReport
        public ResultType DeleteReport(short reportIdToDelete, LogSession session)
        {
            if (reportIdToDelete <= 0)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            Report report = Report.RetrieveByKey(reportIdToDelete);
            if (report == null)
                return ResultType.Error;

            report.Delete();
            return ResultType.Ok;
        }

        public ResultType DeleteModuleReport(short reportIdToDelete, LogSession session)
        {
            if (reportIdToDelete <= 0)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            ModuleReport report = ModuleReport.RetrieveByKey(reportIdToDelete);
            if (report == null)
                return ResultType.Error;

            report.Delete();
            return ResultType.Ok;
        }
        #endregion

        

        

        

        
    }
}
