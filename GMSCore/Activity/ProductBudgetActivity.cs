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
    public class ProductBudgetActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public ProductBudgetActivity()
        {
        }
        #endregion

        #region RetrieveProductBudgetByCoyIDYearDivisionIDProductGroupCode
        public IList<ProductBudget> RetrieveProductBudgetByCoyIDYearDivisionIDProductGroupCode(short coyId, short year, string divisionId, string productGroupCode)
        {
            if (year <= 0)
                return null;

            if (coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ProductBudget.Division"),
                                helper.CleanValue(divisionId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ProductBudget.BudgetYear"),
                                helper.CleanValue(year));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ProductBudget.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ProductBudget.ProductGroupCode"),
                               helper.CleanValue(productGroupCode));

            return ProductBudget.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region DeleteBudgetByCoyIDYearDivisionIDProductGroupCode
        public ResultType DeleteBudgetByCoyIDYearDivisionIDProductGroupCode(short coyId, short year, string divisionID, string productGroupCode)
        {
            if (year <= 0)
                return ResultType.NullMainData;

            if (coyId <= 0)
                return ResultType.NullMainData;

            IList<ProductBudget> lstProductBudget = this.RetrieveProductBudgetByCoyIDYearDivisionIDProductGroupCode(coyId, year, divisionID, productGroupCode);
            foreach (ProductBudget budget in lstProductBudget)
            {
                budget.Delete();
                budget.Resync();
            }

            return ResultType.Ok;
        }
        #endregion

        #region CreateProductBudget
        /// <summary>
        /// Function to create a new Product Budget
        /// </summary>
        /// <param name="budgetToCreate">Reference to a Product Budget object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateProductBudget(ref ProductBudget budgetToCreate, LogSession session)
        {
            if (budgetToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!budgetToCreate.IsValid())
                return ResultType.MainDataNotValid;

            budgetToCreate.Save();

            return ResultType.Ok;
        }
        #endregion
    }
}
