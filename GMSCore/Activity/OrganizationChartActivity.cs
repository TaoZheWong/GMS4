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
    public class OrganizationChartActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public OrganizationChartActivity()
        {
        }
        #endregion

        // Activities for Report

        #region RetrieveOrganizationChartById
        public OrganizationChart RetrieveOrganizationChartById(short pageId)
        {
            if (pageId <= 0)
                return null;

            return OrganizationChart.RetrieveByKey(pageId);
        }
        #endregion

        #region CreateOrganizationChart
        /// <summary>
        /// Function to create a new Organization Chart
        /// </summary>
        /// <param name="rOrganizationChartToCreate">Reference to a OrganizationChart object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateOrganizationChart(ref OrganizationChart organizationChartToCreate, LogSession session)
        {
            if (organizationChartToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!organizationChartToCreate.IsValid())
                return ResultType.MainDataNotValid;

            //billingTermToCreate.Code = billingTermToCreate.Code.ToUpper();

            organizationChartToCreate.Save();

            return ResultType.Ok;
        }

        public ResultType CreateOrganizationChartLink(ref OrganizationChartLink organizationChartLinkToCreate, LogSession session)
        {
            if (organizationChartLinkToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!organizationChartLinkToCreate.IsValid())
                return ResultType.MainDataNotValid;

            //billingTermToCreate.Code = billingTermToCreate.Code.ToUpper();

            organizationChartLinkToCreate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region DeleteOrganizationChartLink
        public ResultType DeleteOrganizationChartLink(short linkId, LogSession session)
        {
            if (linkId == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            OrganizationChartLink oChartLink = OrganizationChartLink.RetrieveByKey(linkId);
            if (oChartLink == null)
                return ResultType.Error;

            oChartLink.Delete();
            return ResultType.Ok;
        }
        #endregion

        #region DeleteOrganizationChart
        public ResultType DeleteOrganizationChart(short pageId, LogSession session)
        {
            if (pageId == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            OrganizationChart oChart = OrganizationChart.RetrieveByKey(pageId);
            if (oChart == null)
                return ResultType.Error;

            oChart.Delete();
            return ResultType.Ok;
        }
        #endregion

        #region UpdateOrganizationChartLink
        /// <summary>
        /// Function to update OrganizationChartLink
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a OrganizationChartLink object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateOrganizationChartLink(ref OrganizationChartLink organizationChartLinkToUpdate, LogSession session)
        {
            if (organizationChartLinkToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!organizationChartLinkToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            organizationChartLinkToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion
    }
}
