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
    public class BudgetActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public BudgetActivity()
        {
        }
        #endregion

        #region RetrieveBudgetForFinanceByYear
        public IList<BudgetForFinance> RetrieveBudgetForFinanceByYear(short year, short coyId, short projectId)
        {
            if (year <= 0)
                return null;

            if (coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
           
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BudgetForFinance.BudgetYear"),
                                helper.CleanValue(year));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.ProjectID"),
                               helper.CleanValue(projectId));

            return BudgetForFinance.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveBudgetForFinanceByYearItemId
        public IList<BudgetForFinance> RetrieveBudgetForFinanceByYearItemId(short itemId, short year, int FYE, short coyId, short projectId, short departmentId, short sectionId , short unitId)
        {
            if (itemId <= 0)
                return null;

            if (year <= 0)
                return null;

            if (coyId <= 0)
                return null;

            if ((FYE != 3) && (FYE != 12))
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BudgetForFinance.ItemID"),
                                helper.CleanValue(itemId));
            if (FYE == 3)
            {
                //Current YEAR, MONTH 4 TO 12
                stb.AppendFormat(" AND (({0} = {1} ", helper.GetFieldName("BudgetForFinance.BudgetYear"),
                                helper.CleanValue(year));
                stb.AppendFormat(" AND {0} IN (4,5,6,7,8,9,10,11,12)) ", helper.GetFieldName("BudgetForFinance.BudgetMonth"));

                //Next YEAR, MONTH 1 TO 3
                stb.AppendFormat(" OR ({0} = {1} ", helper.GetFieldName("BudgetForFinance.BudgetYear"),
                                helper.CleanValue((Int16)(year + 1)));
                stb.AppendFormat(" AND {0} IN (1,2,3))) ", helper.GetFieldName("BudgetForFinance.BudgetMonth"));
            }
            else if (FYE == 12)
            {
                stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.BudgetYear"),
                                helper.CleanValue(year));
            }

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.ProjectID"),
                               helper.CleanValue(projectId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.DepartmentID"),
                              helper.CleanValue(departmentId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.SectionID"),
                               helper.CleanValue(sectionId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.UnitID"),
                              helper.CleanValue(unitId));



            return BudgetForFinance.RetrieveQuery(stb.ToString());
        }

        #endregion

        #region RetrieveBudgetByYearItemStructureId
        public IList<Budget> RetrieveBudgetByYearItemStructureId(short itemStructureId, short year, short coyId,short departmentId)
        {
            if (itemStructureId <= 0)
                return null;

            if (year <= 0)
                return null;

            if (coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Budget.ItemStructureID"),
                                helper.CleanValue(itemStructureId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Budget.BudgetYear"),
                                helper.CleanValue(year));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Budget.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Budget.DepartmentID"),
                               helper.CleanValue(departmentId));

            return Budget.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveBudgetByYearMonthItemStructureId
        public Budget RetrieveBudgetByYearMonthItemStructureId(short itemStructureId, short year, short month, short coyId, short departmentId)
        {
            if (itemStructureId <= 0)
                return null;

            if (year <= 0)
                return null;

            if (month <= 0)
                return null;

            if (coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Budget.ItemStructureID"),
                                helper.CleanValue(itemStructureId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Budget.BudgetYear"),
                                helper.CleanValue(year));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Budget.BudgetMonth"),
                               helper.CleanValue(month));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Budget.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Budget.DepartmentID"),
                               helper.CleanValue(departmentId));

            return Budget.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveBudgetForProductByYearProductGroupCode
        public IList<BudgetForProduct> RetrieveBudgetForProductByYearProductGroupCode(string prodGroupCode, short year, short coyId, string customerType)
        {
            if (prodGroupCode == "")
                return null;

            if (year <= 0)
                return null;

            if (coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BudgetForProduct.ProductGroupCode"),
                                helper.CleanValue(prodGroupCode));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForProduct.BudgetYear"),
                                helper.CleanValue(year));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForProduct.CustomerType"),
                                helper.CleanValue(customerType));            

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForProduct.CoyID"),
                                helper.CleanValue(coyId));

            return BudgetForProduct.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveBudgetForCustomerByYearAccountCode
        public IList<BudgetForCustomer> RetrieveBudgetForCustomerByYearAccountCode(string accountCode, short year, short coyId)
        {
            if (accountCode == "")
                return null;

            if (year <= 0)
                return null;

            if (coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BudgetForCustomer.AccountCode"),
                                helper.CleanValue(accountCode));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForCustomer.BudgetYear"),
                                helper.CleanValue(year));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForCustomer.CoyID"),
                                helper.CleanValue(coyId));

            return BudgetForCustomer.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region DeleteBudgetByYearItemStructureID
        public ResultType DeleteBudgetByYearItemStructureID(short itemStructureId, short year, short coyId, short departmentId)
        {
            if (itemStructureId <= 0)
                return ResultType.NullMainData;

            if (year <= 0)
                return ResultType.NullMainData;

            if (coyId <= 0)
                return ResultType.NullMainData;

            IList<Budget> lstBudget = this.RetrieveBudgetByYearItemStructureId(itemStructureId, year, coyId, departmentId);
            foreach (Budget budget in lstBudget)
            {
                budget.Delete();
                budget.Resync();
            }

            return ResultType.Ok;
        }
        #endregion

        #region DeleteBudgetForFinanceByYearItemID
        public ResultType DeleteBudgetForFinanceByYearItemID(short itemId, short year, int FYE, short coyId, short projectId, short departmentId , short sectionId , short unitId)
        {
            if (itemId <= 0)
                return ResultType.NullMainData;

            if (year <= 0)
                return ResultType.NullMainData;

            if (coyId <= 0)
                return ResultType.NullMainData;

            if (FYE <= 0)
                return ResultType.NullMainData;

            IList<BudgetForFinance> lstBudget = this.RetrieveBudgetForFinanceByYearItemId(itemId, year, FYE, coyId, projectId, departmentId, sectionId, unitId);
            foreach (BudgetForFinance budget in lstBudget)
            {
                budget.Delete();
                budget.Resync();
            }

            return ResultType.Ok;
        }
        #endregion
        
        #region DeleteBudgetForProductByYearProductGroupCode
        public ResultType DeleteBudgetForProductByYearProductGroupCode(string prodGroupCode, short year, short coyId, string customerType)
        {
            if (prodGroupCode == "")
                return ResultType.NullMainData;

            if (year <= 0)
                return ResultType.NullMainData;

            if (coyId <= 0)
                return ResultType.NullMainData;


            IList<BudgetForProduct> lstBudgetForProduct = this.RetrieveBudgetForProductByYearProductGroupCode(prodGroupCode, year, coyId, customerType);

            foreach (BudgetForProduct budget in lstBudgetForProduct)
            {
                budget.Delete();
                budget.Resync();
            }

            return ResultType.Ok;
        }
        #endregion

        #region DeleteBudgetForCustomerByYearAccountCode
        public ResultType DeleteBudgetForCustomerByYearAccountCode(string accountCode, short year, short coyId)
        {
            if (accountCode == "")
                return ResultType.NullMainData;

            if (year <= 0)
                return ResultType.NullMainData;

            if (coyId <= 0)
                return ResultType.NullMainData;


            IList<BudgetForCustomer> lstBudgetForCustomer = this.RetrieveBudgetForCustomerByYearAccountCode(accountCode, year, coyId); 

            foreach (BudgetForCustomer budget in lstBudgetForCustomer)
            {
                budget.Delete();
                budget.Resync();
            }

            return ResultType.Ok;
        }
        #endregion

        #region CreateBudget
        /// <summary>
        /// Function to create a new Budget
        /// </summary>
        /// <param name="budgetToCreate">Reference to a Budget object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateBudget(ref Budget budgetToCreate, LogSession session)
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

        #region CreateBudgetForCustomer
        /// <summary>
        /// Function to create a new Budget For Customer
        /// </summary>
        /// <param name="budgetToCreate">Reference to a Budget object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateBudgetForCustomer(ref BudgetForCustomer budgetToCreate, LogSession session)
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

        #region CreateUpdateBudgetForProduct
        /// <summary>
        /// Function to create a new Budget For Product
        /// </summary>
        /// <param name="budgetToCreate">Reference to a Budget object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateUpdateBudgetForProduct(ref BudgetForProduct budgetToCreate, LogSession session)
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

        #region CreateBudgetForFinance
        /// <summary>
        /// Function to create a new Budget
        /// </summary>
        /// <param name="budgetToCreate">Reference to a Budget object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateBudgetForFinance(ref BudgetForFinance budgetToCreate, LogSession session)
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

        #region RetrieveBudgetForFinanceByItemId
        public BudgetForFinance RetrieveBudgetForFinanceByItemId(short year, short month, short itemId, short coyId)
        {
            if (itemId <= 0)
                return null;

            if (year <= 0)
                return null;

            if (coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BudgetForFinance.ItemID"),
                                helper.CleanValue(itemId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.BudgetYear"),
                                helper.CleanValue(year));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.BudgetMonth"),
                                helper.CleanValue(month));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.CoyID"),
                                helper.CleanValue(coyId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.ProjectID"),-1);

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.SectionID"),-1);
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BudgetForFinance.DepartmentID"), -1);

            return BudgetForFinance.RetrieveFirst(stb.ToString());
        }
        #endregion
    }
}
