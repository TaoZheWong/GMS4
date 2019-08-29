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
    public class SystemDataActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public SystemDataActivity()
        {
        }
        #endregion

        // Activities for Country
        #region RetrieveAllCountryListSortBySeqID
        public IList<Country> RetrieveAllCountryListSortBySeqID(LogSession session)
        {
            QueryHelper helper = base.GetHelper();          

            return Country.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Country.SeqID")));
        }

        public IList<Country> RetrieveAllCountryListSortBySeqID()
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = 1 ", helper.GetFieldName("Country.IsActive"));

            return Country.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("Country.SeqID")));
        }

        public IList<AccountGrade> RetrieveAllAccountGrade()
        {
            QueryHelper helper = base.GetHelper();

            return AccountGrade.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("AccountGrade.GradeCode")));
        }

        public IList<AccountClass> RetrieveAllAccountClass()
        {
            QueryHelper helper = base.GetHelper();

            return AccountClass.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("AccountClass.ClassID")));
        }

        public IList<AccountGroup> RetrieveAllAccountGroup()
        {
            QueryHelper helper = base.GetHelper();

            return AccountGroup.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("AccountGroup.AccountGroupCode")));
        }

        public IList<MRStatus> RetrieveAllMRStatus()
        {
            QueryHelper helper = base.GetHelper();

            return MRStatus.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("MRStatus.StatusName")));
        }

        public IList<MRStatus> RetrieveMRStatus()
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} OR {0} = {2} ", helper.GetFieldName("MRStatus.StatusID"), 'A', 'C');

            return MRStatus.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("MRStatus.StatusName")));

            //return MRStatus.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("MRStatus.StatusName")));
        }

        public IList<Country> RetrieveAllCountryListSortWithoutGroup()
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} != {1} ", helper.GetFieldName("Country.CountryID"),
                                helper.CleanValue(9));

            return Country.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("Country.SeqID")));
        }

        public IList<AccountTerritory> RetrieveAllTerritoryList()
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            return AccountTerritory.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("AccountTerritory.Code")));
        }
        #endregion

        #region GetCountryMaxSeqID
        public int GetCountryMaxSeqID()
        {
            DBManager db = DBManager.GetInstance();
            QueryHelper helper = base.GetHelper();

            int iMaxSeqID = GMSUtil.ToInt(
                                    db.Engine.ExecuteScalar(
                                        string.Format("SELECT MAX ( {0} ) FROM {1}",
                                            helper.GetFieldName("Country.SeqID"),
                                            helper.GetTableName("Country"))));

            return iMaxSeqID;
        }
        #endregion

        #region RetrieveCountryById
        public Country RetrieveCountryById(short countryId, LogSession session)
        {
            if (countryId <= 0)
                return null;

            if (session == null)
                throw new NullSessionException();

            return Country.RetrieveByKey(countryId);
        }
        #endregion

        #region UpdateCountry
        /// <summary>
        /// Function to update Country
        /// </summary>
        /// <param name="countryToUpdate">Reference to a Country object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateCountry(ref Country countryToUpdate, LogSession session)
        {
            if (countryToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!countryToUpdate.IsValid())
                return ResultType.MainDataNotValid;


            //billingTermToUpdate.Code = billingTermToUpdate.Code.ToUpper();

            countryToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateCountry
        /// <summary>
        /// Function to create a new Country
        /// </summary>
        /// <param name="countryToCreate">Reference to a Country object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateCountry(ref Country countryToCreate, LogSession session)
        {
            if (countryToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!countryToCreate.IsValid())
                return ResultType.MainDataNotValid;

            //if (IsCodeInUsed(billingTermToCreate.Code))
            //    return ResultType.DuplicatedData;

            //billingTermToCreate.Code = billingTermToCreate.Code.ToUpper();

            countryToCreate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region DeleteCountry
        public ResultType DeleteCountry(short countryIdToDelete, LogSession session)
        {
            if (countryIdToDelete <= 0)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            Country country = Country.RetrieveByKey(countryIdToDelete);
            if (country == null)
                return ResultType.Error;

            country.Delete();
            return ResultType.Ok;
        }
        #endregion

        // Activities for Division
        #region RetrieveAllDivisionListSortBySeqID
        public IList<Division> RetrieveAllDivisionListSortBySeqID(LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            return Division.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Division.SeqID")));
        }

        public IList<Division> RetrieveAllDivisionListSortBySeqID()
        {
            QueryHelper helper = base.GetHelper();

            return Division.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Division.SeqID")));
        }
        #endregion

        #region GetDivisionMaxSeqID
        public int GetDivisionMaxSeqID()
        {
            DBManager db = DBManager.GetInstance();
            QueryHelper helper = base.GetHelper();

            int iMaxSeqID = GMSUtil.ToInt(
                                    db.Engine.ExecuteScalar(
                                        string.Format("SELECT MAX ( {0} ) FROM {1}",
                                            helper.GetFieldName("Division.SeqID"),
                                            helper.GetTableName("Division"))));

            return iMaxSeqID;
        }
        #endregion

        #region RetrieveDivisionById
        public Division RetrieveDivisionById(short divisionId, LogSession session)
        {
            if (divisionId <= 0)
                return null;

            if (session == null)
                throw new NullSessionException();

            return Division.RetrieveByKey(divisionId);
        }
        #endregion

        #region UpdateDivision
        /// <summary>
        /// Function to update Division
        /// </summary>
        /// <param name="divisionToUpdate">Reference to a Division object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateDivision(ref Division divisionToUpdate, LogSession session)
        {
            if (divisionToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!divisionToUpdate.IsValid())
                return ResultType.MainDataNotValid;


            //billingTermToUpdate.Code = billingTermToUpdate.Code.ToUpper();

            divisionToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateDivision
        /// <summary>
        /// Function to create a new Division
        /// </summary>
        /// <param name="divisionToCreate">Reference to a Division object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateDivision(ref Division divisionToCreate, LogSession session)
        {
            if (divisionToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!divisionToCreate.IsValid())
                return ResultType.MainDataNotValid;

            //if (IsCodeInUsed(billingTermToCreate.Code))
            //    return ResultType.DuplicatedData;

            //billingTermToCreate.Code = billingTermToCreate.Code.ToUpper();

            divisionToCreate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region DeleteDivision
        public ResultType DeleteDivision(short divisionIdToDelete, LogSession session)
        {
            if (divisionIdToDelete <= 0)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            Division division = Division.RetrieveByKey(divisionIdToDelete);
            if (division == null)
                return ResultType.Error;

            division.Delete();
            return ResultType.Ok;
        }
        #endregion

        // Activities for Company
        #region RetrieveAllCompanyList()
        public IList<Company> RetrieveAllCompanyList()
        {
            return Company.RetrieveAll();
        }
        #endregion
        #region RetrieveAllAliveCompanyList()
        public IList<Company> RetrieveAllAliveCompanyList()
        {
            // if (session == null)
            //     throw new NullSessionException();

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = 1 ", helper.GetFieldName("Company.IsActive"));
            // stb.AppendFormat(" AND {0} != 1 ", helper.GetFieldName("Company.DivisionID"));

            return Company.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ",
                                                    helper.GetFieldName("Company.Name")));
        }
        #endregion

        #region RetrieveAllAliveCompanyListExceptLeedenGroup()
        public IList<Company> RetrieveAllAliveCompanyListForConsol()
        {
            // if (session == null)
            //     throw new NullSessionException();

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = 1 ", helper.GetFieldName("Company.IsActive"));
            stb.AppendFormat(" AND {0} = 1 ", helper.GetFieldName("Company.IsDIVA"));
            //stb.AppendFormat(" AND {0} <> 'Leeden Group'", helper.GetFieldName("Company.Name"));
            //stb.AppendFormat(" AND {0} NOT IN (3,32,79,84,99)", helper.GetFieldName("Company.CoyID"));
            // stb.AppendFormat(" AND {0} != 1 ", helper.GetFieldName("Company.DivisionID"));

            return Company.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ",
                                                    helper.GetFieldName("Company.Name")));
        }
        #endregion

        public IList<Company> RetrieveAllCompanyListWithMR()
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = 1 ", helper.GetFieldName("Company.IsActive"));
            stb.AppendFormat(" AND {0} is not null ", helper.GetFieldName("Company.MRScheme"));

            return Company.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ",
                                                    helper.GetFieldName("Company.Name")));
        }

        #region RetrieveCompanyListSortByCountryIdDivIdCoyName - commented
        //public IList<Company> RetrieveCompanyListSortByCountryIdDivIdCoyName(LogSession session)
        //{
        //    //QueryHelper helper = base.GetHelper();

        //    //return Company.RetrieveQuery("", string.Format("{0}, {1}, {2} ASC",
        //    //                                    helper.GetFieldName("CountryObject.SeqID"),
        //    //                                    helper.GetFieldName("DivisionObject.SeqID"),
        //    //                                    helper.GetFieldName("Name")
        //    //                                    ));
        //    Company company = new Company();
        //    QueryHelper helper = base.GetHelper();
        //    StringBuilder stb = new StringBuilder(200);

        //    stb.AppendFormat(" ( {0} = {1} AND {0} = {2}  ORDER BY {1}, {2}, {4} )", 
        //                                                company.Id,
        //                                                company.CountryObject.SeqID,
        //                                                company.DivisionObject.SeqID,
        //                                                company.Name );

        //    return Company.RetrieveQuery(stb.ToString());
        //}
        #endregion

        #region RetrieveCompanyListViewSortByCountryIdDivIdCoyName
        public IList<VwCompanyListing> RetrieveCompanyListViewSortByCountryIdDivIdCoyName(LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            return VwCompanyListing.RetrieveQuery("", string.Format(" {0}, {1}, {2} ASC ",
                                                    helper.GetFieldName("VwCompanyListing.CountryID"),
                                                    helper.GetFieldName("VwCompanyListing.DivisionID"),
                                                    helper.GetFieldName("VwCompanyListing.CompanyName")
                                                    ));
        }
        #endregion

        #region RetrieveCompanyById
        public Company RetrieveCompanyById(short coyId, LogSession session)
        {
            if (coyId <= 0)
                return null;

            if (session == null)
                throw new NullSessionException();

            return Company.RetrieveByKey(coyId);
        }
        #endregion

        #region RetrieveCompanyByIdWOSession
        public Company RetrieveCompanyByIdWOSession(short coyId)
        {
            if (coyId <= 0)
                return null;

            return Company.RetrieveByKey(coyId);
        }
        #endregion

        #region RetrieveCompanyByCountryIdDivisionId
        public IList<Company> RetrieveCompanyByCountryIdDivisionId(short countryId, short divisionId)
        {
            if (countryId <= 0)
                return null;

            if (divisionId <= 0)
                return null;

            // if (session == null)
            //     throw new NullSessionException();

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Company.CountryID"), countryId);
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Company.DivisionID"), divisionId);
            stb.AppendFormat(" AND {0} = 1 ", helper.GetFieldName("Company.IsActive"));

            return Company.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveCompanyByCode
        public IList<Company> RetrieveCompanyByCode(string Code)
        {
            if (Code.Equals(""))
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Company.Code"), helper.CleanValue(Code));

            return Company.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveCompanyByCountryId
        public IList<Company> RetrieveCompanyByCountryId(short countryId)
        {
            if (countryId <= 0)
                return null;

            // if (session == null)
            //     throw new NullSessionException();

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Company.CountryID"), countryId);
            stb.AppendFormat(" AND {0} = 1 ", helper.GetFieldName("Company.IsActive"));
            // stb.AppendFormat(" AND {0} != 1 ", helper.GetFieldName("Company.DivisionID"));

            return Company.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ",
                                                    helper.GetFieldName("Company.Name")));
        }
        #endregion

        #region RetrieveCompanyByDivisionId
        public IList<Company> RetrieveCompanyByDivisionId(short divisionId)
        {
            if (divisionId <= 0)
                return null;

            // if (session == null)
            //     throw new NullSessionException();

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Company.DivisionID"), divisionId);

            return Company.RetrieveQuery(stb.ToString());
        }

        public IList<Company> RetrieveCompanyByDivisionIdSortByCountryId(short divisionId)
        {
            if (divisionId <= 0)
                return null;

            // if (session == null)
            //     throw new NullSessionException();

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Company.DivisionID"), divisionId);

            return Company.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("Company.CountryID")));
        }

        public IList<Company> RetrieveCompanyByCountryIdDivisionIdSortByCountryIdAndSeqID(short countryId, short divisionId)
        {
            if (countryId <= 0)
                return null;

            if (divisionId <= 0)
                return null;

            // if (session == null)
            //     throw new NullSessionException();

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Company.CountryID"), countryId);
            stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("Company.DivisionID"), divisionId);
            stb.AppendFormat(" and {0} = 1 ", helper.GetFieldName("Company.IsActive"));

            return Company.RetrieveQuery(stb.ToString(), string.Format(" {0},{1} ASC ", helper.GetFieldName("Company.CountryID"), helper.GetFieldName("Company.SeqID")));
        }
        #endregion

        #region UpdateCompany
        /// <summary>
        /// Function to update Company
        /// </summary>
        /// <param name="divisionToUpdate">Reference to a Company object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateCompany(ref Company companyToUpdate, LogSession session)
        {
            if (companyToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!companyToUpdate.IsValid())
                return ResultType.MainDataNotValid;


            //billingTermToUpdate.Code = billingTermToUpdate.Code.ToUpper();

            companyToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateCompany
        /// <summary>
        /// Function to create a new Company
        /// </summary>
        /// <param name="companyToCreate">Reference to a Company object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateCompany(ref Company companyToCreate, LogSession session)
        {
            if (companyToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!companyToCreate.IsValid())
                return ResultType.MainDataNotValid;

            //if (IsCodeInUsed(billingTermToCreate.Code))
            //    return ResultType.DuplicatedData;

            //billingTermToCreate.Code = billingTermToCreate.Code.ToUpper();

            companyToCreate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region DeleteCompany
        public ResultType DeleteCompany(short coyIdToDelete, LogSession session)
        {
            if (coyIdToDelete <= 0)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            Company company = Company.RetrieveByKey(coyIdToDelete);
            if (company == null)
                return ResultType.Error;

            company.Delete();
            return ResultType.Ok;
        }
        #endregion


        // Activities for Item
        #region RetrieveAllItemList()
        public IList<Item> RetrieveAllItemList()
        {
            return Item.RetrieveAll();
        }
        #endregion

        #region SearchItem
        /// <summary>
        /// Function to search for Item
        /// </summary>
        /// <param name="searchString">string value of the search string</param>
        /// <param name="pageSize">int value of the Page Size</param>
        /// <param name="pageNo">int value of the Page No</param>
        /// <param name="totalCount">[out] int value of the record count</param>
        /// <param name="session">LogSession object</param>
        /// <returns>Generic IList of ExternalParty</returns>
        public IList<Item> SearchItem(int pageSize, int pageNo, out int totalCount,
                                                            LogSession session)
        {
            //Check AccessControl
            //if (session != null)
            //{
            //    if (session.dictAccessItem_AccessOperationEMSum == null)
            //        throw new NullSessionException();

            //    if ((session.GetAccessOperationEMSum(AccessItemEMType.ExternalParty) & AccessOperationEMType.View) == 0)
            //        throw new SecurityException("view External Party.");
            //}

            QueryHelper helper = base.GetHelper();

            return Item.RetrievePage("", helper.GetFieldName("Item.ItemName"),
                                                pageSize, pageNo, out totalCount);
        }
        #endregion

        #region RetrieveItemById
        public Item RetrieveItemById(short itemId, LogSession session)
        {
            if (itemId <= 0)
                return null;

            if (session == null)
                throw new NullSessionException();

            return Item.RetrieveByKey(itemId);
        }

        public Item RetrieveItemById(short itemId)
        {
            if (itemId <= 0)
                return null;

            return Item.RetrieveByKey(itemId);
        }
        #endregion

        #region RetrieveItemByName
        public IList<Item> RetrieveItemByName(string itemName)
        {
            if (itemName.Length <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Item.ItemName"),
                                helper.CleanValue(itemName));

            return Item.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveFinanceItemByName
        public IList<FinanceItem> RetrieveFinanceItemByName(string itemName)
        {
            if (itemName.Length <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FinanceItem.ItemName"),
                                helper.CleanValue(itemName));

            return FinanceItem.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region UpdateItem
        /// <summary>
        /// Function to update Item
        /// </summary>
        /// <param name="itemToUpdate">Reference to a Item object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateItem(ref Item itemToUpdate, LogSession session)
        {
            if (itemToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!itemToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            itemToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateItem
        /// <summary>
        /// Function to create a new Item
        /// </summary>
        /// <param name="itemToCreate">Reference to a Item object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateItem(ref Item itemToCreate, LogSession session)
        {
            if (itemToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!itemToCreate.IsValid())
                return ResultType.MainDataNotValid;

            //if (IsCodeInUsed(billingTermToCreate.Code))
            //    return ResultType.DuplicatedData;

            itemToCreate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region DeleteItem
        public ResultType DeleteItem(short itemIdToDelete, LogSession session)
        {
            if (itemIdToDelete <= 0)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            Item item = Item.RetrieveByKey(itemIdToDelete);
            if (item == null)
                return ResultType.Error;

            item.Delete();
            return ResultType.Ok;
        }
        #endregion


        // Activities for ItemStructure
        #region SearchItemStructure
        /// <summary>
        /// Function to search for Item
        /// </summary>
        /// <param name="searchString">string value of the search string</param>
        /// <param name="pageSize">int value of the Page Size</param>
        /// <param name="pageNo">int value of the Page No</param>
        /// <param name="totalCount">[out] int value of the record count</param>
        /// <param name="session">LogSession object</param>
        /// <returns>Generic IList of ExternalParty</returns>
        public IList<VwItemStructureListing> SearchItemStructure(short purposeId, int pageSize, int pageNo, out int totalCount,
                                                            LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VwItemStructureListing.ItemPurposeID"),
                                                             helper.CleanValue(purposeId));

            //StringBuilder stbSort = new StringBuilder(200);
            //stbSort.AppendFormat(" {0}, {1} ASC ", helper.GetFieldName("VwItemStructureListing.ItemCategoryName"),
            //                                        helper.GetFieldName("VwItemStructureListing.ItemName"));

            return VwItemStructureListing.RetrievePage("", helper.GetFieldName("VwItemStructureListing.ItemCategoryName"),
                                                pageSize, pageNo, out totalCount);
        }
        #endregion

        #region RetrieveItemStructureByPurposeId
        public IList<ItemStructure> RetrieveItemStructureByPurposeId(short purposeId)
        {
            QueryHelper helper = base.GetHelper();

            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ItemStructure.ItemPurposeID"),
                                                             helper.CleanValue(purposeId));


            return ItemStructure.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveItemStructureById
        public ItemStructure RetrieveItemStructureById(short itemStructureId, LogSession session)
        {
            if (itemStructureId <= 0)
                return null;

            if (session == null)
                throw new NullSessionException();

            return ItemStructure.RetrieveByKey(itemStructureId);
        }
        #endregion

        #region RetrieveItemStructureByItemIdPurposeId
        public IList<ItemStructure> RetrieveItemStructureByItemIdPurposeId(short itemId, short itemPurposeId)
        {
            if (itemId <= 0)
                return null;

            if (itemPurposeId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();

            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ItemStructure.ItemID"),
                                                             helper.CleanValue(itemId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ItemStructure.ItemPurposeID"),
                                                             helper.CleanValue(itemPurposeId));



            return ItemStructure.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveItemStructureByItemIDPurposeID
        public IList<ItemStructure> RetrieveItemStructureByItemIDPurposeID(short itemId, short purposeId)
        {
            if (itemId <= 0)
                return null;

            if (purposeId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ItemStructure.ItemID"),
                                helper.CleanValue(itemId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ItemStructure.ItemPurposeID"),
                                helper.CleanValue(purposeId));

            return ItemStructure.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region UpdateItemStructure
        /// <summary>
        /// Function to update ItemStructure
        /// </summary>
        /// <param name="itemStructureToUpdate">Reference to a ItemStructure object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateItemStructure(ref ItemStructure itemStructureToUpdate, LogSession session)
        {
            if (itemStructureToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!itemStructureToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            itemStructureToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateItemStructure
        /// <summary>
        /// Function to create a new ItemStructure
        /// </summary>
        /// <param name="itemStructureToCreate">Reference to a ItemStructure object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateItemStructure(ref ItemStructure itemStructureToCreate, LogSession session)
        {
            if (itemStructureToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!itemStructureToCreate.IsValid())
                return ResultType.MainDataNotValid;

            //if (IsCodeInUsed(billingTermToCreate.Code))
            //    return ResultType.DuplicatedData;

            itemStructureToCreate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region DeleteItemStructure
        public ResultType DeleteItemStructure(short itemStructureIdToDelete, LogSession session)
        {
            if (itemStructureIdToDelete <= 0)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            ItemStructure itemstructure = ItemStructure.RetrieveByKey(itemStructureIdToDelete);
            if (itemstructure == null)
                return ResultType.Error;

            itemstructure.Delete();
            return ResultType.Ok;
        }
        #endregion


        // Activities for ItemPurpose
        #region RetrieveAllItemPurposeListSortByName
        public IList<ItemPurpose> RetrieveAllItemPurposeListSortByName()
        {
            QueryHelper helper = base.GetHelper();

            return ItemPurpose.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("ItemPurpose.ItemPurposeName")));
        }
        #endregion

        #region RetrieveAllItemPurposeListSortByID
        public IList<ItemPurpose> RetrieveAllItemPurposeListSortByID()
        {
            QueryHelper helper = base.GetHelper();

            return ItemPurpose.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("ItemPurpose.ItemPurposeID")));
        }
        #endregion

        #region RetrieveAllFinanceItemPurposeListSortByID
        public IList<ItemPurpose> RetrieveAllFinanceItemPurposeListSortByID()
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ItemPurpose.ItemPurposeName"),
                                helper.CleanValue("P & L"));
            stb.AppendFormat("OR {0} = {1} ", helper.GetFieldName("ItemPurpose.ItemPurposeName"),
                                helper.CleanValue("Listing Of Expenses (S & D)"));
            stb.AppendFormat("OR {0} = {1} ", helper.GetFieldName("ItemPurpose.ItemPurposeName"),
                                helper.CleanValue("Listing Of Expenses (G & A)"));
            stb.AppendFormat("OR {0} = {1} ", helper.GetFieldName("ItemPurpose.ItemPurposeName"),
                                helper.CleanValue("Balance Sheet"));
            stb.AppendFormat("ORDER BY {0}", helper.GetFieldName("ItemPurpose.ItemPurposeID"));
            //stb.AppendFormat("AND {0} ASC ", helper.GetFieldName("ItemPurpose.ItemPurposeID")); 

            return ItemPurpose.RetrieveQuery(stb.ToString());
        }
        #endregion

        // Activities for Currency
        #region RetrieveAllCurrencyListSortByCode
        public IList<Currency> RetrieveAllCurrencyListSortByCode(LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            return Currency.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Currency.CurrencyCode")));
        }

        public IList<Currency> RetrieveAllCurrencyListSortByCode()
        {
            QueryHelper helper = base.GetHelper();

            return Currency.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Currency.CurrencyCode")));
        }
        #endregion

        #region RetrieveCurrencyById
        public Currency RetrieveCurrencyById(string currencyCode, LogSession session)
        {
            if (currencyCode == null)
                return null;

            if (session == null)
                throw new NullSessionException();

            return Currency.RetrieveByKey(currencyCode);
        }
        #endregion

        #region UpdateCurrency
        /// <summary>
        /// Function to update Currency
        /// </summary>
        /// <param name="currencyToUpdate">Reference to a Currency object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateCurrency(ref Currency currencyToUpdate, LogSession session)
        {
            if (currencyToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!currencyToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            currencyToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateCurrency
        /// <summary>
        /// Function to create a new Currency
        /// </summary>
        /// <param name="itemToCreate">Reference to a Currency object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateCurrency(ref Currency currencyToCreate, LogSession session)
        {
            if (currencyToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Add) == 0)
            //    throw new SecurityException("add Billing Term.");

            if (!currencyToCreate.IsValid())
                return ResultType.MainDataNotValid;

            //if (IsCodeInUsed(billingTermToCreate.Code))
            //    return ResultType.DuplicatedData;

            currencyToCreate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region DeleteCurrency
        public ResultType DeleteCurrency(string currencyCodeToDelete, LogSession session)
        {
            if (currencyCodeToDelete == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            Currency currency = Currency.RetrieveByKey(currencyCodeToDelete);
            if (currency == null)
                return ResultType.Error;

            currency.Delete();
            return ResultType.Ok;
        }
        #endregion


        // Activities for ModuleCategory
        #region RetrieveAllModuleCategoryList()
        public IList<ModuleCategory> RetrieveAllModuleCategoryList()
        {
            return ModuleCategory.RetrieveAll();
        }


        public ModuleCategory RetrieveModuleCategoryByName(string name)
        {
            QueryHelper helper = base.GetHelper();
            return ModuleCategory.RetrieveFirst(string.Format(" {0} = {1} ",
                                                helper.GetFieldName("ModuleCategory.Name"),
                                                helper.CleanValue(name)), string.Format(" {0} ASC ",
                                                helper.GetFieldName("ModuleCategory.Name")));
        }
        #endregion

        // Activities for Module
        #region RetrieveAllModuleList()
        public IList<Module> RetrieveAllModuleList()
        {
            return Module.RetrieveAll();
        }
        #endregion

        #region RetrieveAllModuleListingByParentModuleName()
        public IList<VwModuleListing> RetrieveAllModuleListingByParentModuleName(short moduleCategoryId)
        {
            if (moduleCategoryId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            ObjectQuery<VwModuleListing> query;
            query = new ObjectQuery<VwModuleListing>(string.Format(" {0} = {1} ",
                                                helper.GetFieldName("VwModuleListing.ModuleCategoryID"),
                                                helper.CleanValue(moduleCategoryId)),
                                            string.Format(" {0} ASC, {1} ASC ",
                                                helper.GetFieldName("VwModuleListing.ModuleCategoryID"), helper.GetFieldName("VwModuleListing.SeqID"))
                                            );

            return VwModuleListing.RetrieveQuery(query);
        }
        #endregion

        // Activities for Company Parameter
        #region RetrieveAllParameterListSortById

        public IList<CompanyParameter> RetrieveAllParameterListSortById()
        {
            QueryHelper helper = base.GetHelper();

            return CompanyParameter.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("CompanyParameter.ParameterID")));
        }
        #endregion

        #region SearchSpecialData
        /// <summary>
        /// Function to search for Special Data
        /// </summary>
        /// <param name="pageSize">int value of the Page Size</param>
        /// <param name="pageNo">int value of the Page No</param>
        /// <param name="totalCount">[out] int value of the record count</param>
        /// <returns>Generic IList of ExternalParty</returns>
        public IList<CompanySpecialData> SearchSpecialData(short CoyId, short parameterId, int pageSize, int pageNo, out int totalCount)
        {
            //Check AccessControl
            //if (session != null)
            //{
            //    if (session.dictAccessItem_AccessOperationEMSum == null)
            //        throw new NullSessionException();

            //    if ((session.GetAccessOperationEMSum(AccessItemEMType.ExternalParty) & AccessOperationEMType.View) == 0)
            //        throw new SecurityException("view External Party.");
            //}

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("CompanySpecialData.CoyID"),
                                helper.CleanValue(CoyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("CompanySpecialData.ParameterID"),
                                helper.CleanValue(parameterId));

            return CompanySpecialData.RetrievePage(stb.ToString(), string.Format(" {0} ASC", helper.GetFieldName("CompanySpecialData.CSDDate")),
                                                pageSize, pageNo, out totalCount);
        }
        #endregion

        #region SearchAuditedReport
        /// <summary>
        /// Function to search for Audited Report
        /// </summary>
        /// <param name="pageSize">int value of the Page Size</param>
        /// <param name="pageNo">int value of the Page No</param>
        /// <param name="totalCount">[out] int value of the record count</param>
        /// <returns>Generic IList of ExternalParty</returns>
        public IList<Report> SearchAuditedReport(int pageSize, int pageNo, out int totalCount)
        {
            //Check AccessControl
            //if (session != null)
            //{
            //    if (session.dictAccessItem_AccessOperationEMSum == null)
            //        throw new NullSessionException();

            //    if ((session.GetAccessOperationEMSum(AccessItemEMType.ExternalParty) & AccessOperationEMType.View) == 0)
            //        throw new SecurityException("view External Party.");
            //}

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Report.ReportCategoryID"),
                                                helper.CleanValue(5));
            return Report.RetrievePage(stb.ToString(), string.Format(" {0} ASC", helper.GetFieldName("Report.FileName")),
                                                pageSize, pageNo, out totalCount);
        }
        #endregion

        // Activities for HR Module
        #region RetrieveAllOrganizerListSortByOrganizerShortName
        public IList<CourseOrganizer> RetrieveAllOrganizerListSortByOrganizerName(LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            return CourseOrganizer.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("CourseOrganizer.OrganizerName")));
        }

        public IList<CourseOrganizer> RetrieveAllOrganizerListSortByOrganizerName()
        {
            QueryHelper helper = base.GetHelper();

            return CourseOrganizer.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("CourseOrganizer.OrganizerName")));
        }

        public IList<CourseOrganizer> RetrieveAllOrganizerListByOrganizerNameSortByOrganizerName(string organizerName)
        {
            QueryHelper helper = base.GetHelper();

            return CourseOrganizer.RetrieveQuery(string.Format(" {0} like {1}", helper.GetFieldName("CourseOrganizer.OrganizerName"), helper.CleanValue(organizerName)), string.Format(" {0} ASC ", helper.GetFieldName("CourseOrganizer.OrganizerName")));
        }
        #endregion

        #region RetrieveAllCourseListSortByCourseCode
        public IList<Course> RetrieveAllCourseListSortByCourseCode(LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            return Course.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Course.CourseCode")));
        }

        public IList<Course> RetrieveAllCourseListSortByCourseCode()
        {
            QueryHelper helper = base.GetHelper();

            return Course.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Course.CourseCode")));
        }

        public IList<Course> RetrieveAllCourseByCourseTitleCourseTypeListSortByCourseCode(string courseTitle, string courseType)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} like {1} ", helper.GetFieldName("Course.CourseTitle"),
                                helper.CleanValue(courseTitle));

            if (courseType == "E" || courseType == "I")
                stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("Course.CourseType"),
                                helper.CleanValue(courseType));
            //stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("Course.CourseType"),
            //                    helper.CleanValue("E%"));

            return Course.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("Course.CourseID")));
        }

        public IList<CourseSession> RetrieveCourseSessionListByCourseTitleByDateSortByCourseTitleDate(string courseTitle, DateTime dateFrom, DateTime dateTo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder where = new StringBuilder(200);
            where.AppendFormat("CourseObject[CourseTitle like {0}] ", helper.CleanValue(courseTitle));
            if (dateFrom != GMSCoreBase.DEFAULT_NO_DATE.AddYears(3) && dateTo != GMSCoreBase.DEFAULT_NO_DATE.AddYears(3))
                where.AppendFormat("AND DateFrom >= {0} AND DateFrom <= {1}", helper.CleanValue(dateFrom), helper.CleanValue(dateTo));
            OPathQuery<CourseSession> query = new OPathQuery<CourseSession>(where.ToString(), "DateFrom Desc, DateTo Desc");
            return DBManager.GetInstance().Engine.GetObjectSet<CourseSession>(query);
        }
        #endregion

        #region RetrieveAllCourseListSortByCourseTitle
        public IList<Course> RetrieveAllCourseListSortByCourseTitle(LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            return Course.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Course.CourseTitle")));
        }

        public IList<Course> RetrieveAllCourseListSortByCourseTitle()
        {
            QueryHelper helper = base.GetHelper();

            return Course.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Course.CourseTitle")));
        }

        public IList<Course> RetrieveAllCourseListByCourseTitleSortByCourseTitle(string courseTitle)
        {
            QueryHelper helper = base.GetHelper();

            return Course.RetrieveQuery(string.Format(" {0} like {1}", helper.GetFieldName("Course.CourseTitle"), helper.CleanValue(courseTitle)), string.Format(" {0} ASC ", helper.GetFieldName("Course.CourseTitle")));
        }
        #endregion

        #region RetrieveAllVendorListByVendorNameSortByVendorName
        public IList<Vendor> RetrieveAllVendorListByVendorNameSortByVendorName(string vendorName)
        {
            QueryHelper helper = base.GetHelper();

            return Vendor.RetrieveQuery(string.Format(" {0} like {1}", helper.GetFieldName("Vendor.CompanyName"), helper.CleanValue(vendorName)), string.Format(" {0} ASC ", helper.GetFieldName("Vendor.CompanyName")));
        }
       #endregion

        #region RetrieveAllECourseListSortByEmployeeNoCourseCode
        public IList<EmployeeCourse> RetrieveAllECourseListSortByEmployeeNoCourseCode(LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            return EmployeeCourse.RetrieveQuery("", string.Format(" {0} ASC, {1} ASC", helper.GetFieldName("EmployeeCourse.EmployeeNo"), helper.GetFieldName("EmployeeCourse.CourseCode")));
        }

        public IList<EmployeeCourse> RetrieveAllECourseListSortByEmployeeNoCourseCode()
        {
            QueryHelper helper = base.GetHelper();

            return EmployeeCourse.RetrieveQuery("", string.Format(" {0} ASC, {1} ASC", helper.GetFieldName("EmployeeCourse.EmployeeNo"), helper.GetFieldName("EmployeeCourse.CourseCode")));
        }
        #endregion

        #region RetrieveECourseListByCourseCodeByEmployeeNoByDateSortByEmployeeNoCourseCode

        public IList<EmployeeCourse> RetrieveECourseListByCourseCodeByEmployeeNoByDateSortByEmployeeNoCourseCode(string courseCode, string employeeNo, DateTime dateFrom, DateTime dateTo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} like {1} ", helper.GetFieldName("EmployeeCourse.CourseCode"),
                                helper.CleanValue(courseCode));

            stb.AppendFormat(" AND {0} like {1} ", helper.GetFieldName("EmployeeCourse.EmployeeNo"),
                                helper.CleanValue(employeeNo));

            stb.AppendFormat(" AND {0} >= {1} ", helper.GetFieldName("EmployeeCourse.DateFrom"),
                                helper.CleanValue(dateFrom));

            stb.AppendFormat(" AND {0} <= {1} ", helper.GetFieldName("EmployeeCourse.DateFrom"),
                                helper.CleanValue(dateTo));
            return EmployeeCourse.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC, {1} ASC", helper.GetFieldName("EmployeeCourse.EmployeeNo"), helper.GetFieldName("EmployeeCourse.CourseCode")));
        }

        public IList<EmployeeCourse> RetrieveECourseListByCourseByEmployeeByNRICByDateByActiveByCEFormSortByEmployeeNoCourseCode(string course, string employee, string nric, DateTime dateFrom, DateTime dateTo, bool isActive)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder where = new StringBuilder(200);
            where.AppendFormat("CourseObject[CourseTitle like {0}] AND EmployeeObject[Name like {1} ", helper.CleanValue(course), helper.CleanValue(employee));
            if (nric != "%")
                where.AppendFormat("AND NRIC like {0} ", helper.CleanValue(nric));
            if (isActive)
                where.AppendFormat("AND (isnull(IsInactive) OR IsInactive = {0}) ] ", helper.CleanValue(!isActive));
            else
                where.AppendFormat("AND (IsInactive = {0}) ] ", helper.CleanValue(!isActive));
            if (dateFrom != GMSCoreBase.DEFAULT_NO_DATE.AddYears(3) && dateTo != GMSCoreBase.DEFAULT_NO_DATE.AddYears(3))
                where.AppendFormat("AND DateFrom >= {0} AND DateFrom <= {1}", helper.CleanValue(dateFrom), helper.CleanValue(dateTo));
            OPathQuery<EmployeeCourse> query = new OPathQuery<EmployeeCourse>(where.ToString(), "DateFrom DESC, EmployeeObject.EmployeeNo, CourseCode");
            return DBManager.GetInstance().Engine.GetObjectSet<EmployeeCourse>(query);
        }

        public IList<VwEmployeeCourse> RetrieveVECourseListByCourseByEmployeeByNRICByDateByActiveByFormStatusSortByEmployeeNoCourseCode(string course, string employee, string nric, DateTime dateFrom, DateTime dateTo, bool isActive, string tnfStatus, string cefStatus)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder where = new StringBuilder(200);
            where.AppendFormat("CourseObject[CourseTitle like {0}] AND EmployeeObject[Name like {1} ", helper.CleanValue(course), helper.CleanValue(employee));
            if (nric != "%")
                where.AppendFormat("AND NRIC like {0} ", helper.CleanValue(nric));
            if (isActive)
                where.AppendFormat("AND (isnull(IsInactive) OR IsInactive = {0}) ] ", helper.CleanValue(!isActive));
            else
                where.AppendFormat("AND (IsInactive = {0}) ] ", helper.CleanValue(!isActive));
            if (dateFrom != GMSCoreBase.DEFAULT_NO_DATE.AddYears(3) && dateTo != GMSCoreBase.DEFAULT_NO_DATE.AddYears(3))
                where.AppendFormat("AND DateFrom >= {0} AND DateFrom <= {1}", helper.CleanValue(dateFrom), helper.CleanValue(dateTo));
            if (tnfStatus == "O")
                where.AppendFormat("AND TNFStatus != 'A' AND TNFStatus != 'R'");
            else if (tnfStatus != "%")
                where.AppendFormat("AND TNFStatus = {0}", helper.CleanValue(tnfStatus));
            if (cefStatus == "O")
                where.AppendFormat("AND CEFStatus != 'A' AND CEFStatus != 'R'");
            else if (cefStatus != "%")
                where.AppendFormat("AND CEFStatus = {0}", helper.CleanValue(cefStatus));
            OPathQuery<VwEmployeeCourse> query = new OPathQuery<VwEmployeeCourse>(where.ToString(), "DateFrom DESC, EmployeeObject.EmployeeNo, CourseCode");
            return DBManager.GetInstance().Engine.GetObjectSet<VwEmployeeCourse>(query);
        }
        #endregion

        #region RetrieveAllEmployeeListSortByEmployeeName
        public IList<Employee> RetrieveAllEmployeeListSortByEmployeeName(LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            return Employee.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Employee.Name")));
        }

        public IList<Employee> RetrieveAllEmployeeListSortByEmployeeName()
        {
            QueryHelper helper = base.GetHelper();

            return Employee.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Employee.Name")));
        }
        #endregion

        #region RetrieveAllEmployeeListSortByEmployeeNo
        public IList<Employee> RetrieveAllEmployeeListSortByEmployeeNo(LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            return Employee.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Employee.EmployeeNo")));
        }

        public IList<Employee> RetrieveAllEmployeeListSortByEmployeeNo()
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            //stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Employee.IsInactive"), helper.CleanValue(0));
            stb.AppendFormat(" ({0} is null OR {0} <> {1}) ", helper.GetFieldName("Employee.IsInactive"),
                               helper.CleanValue(true));

            return Employee.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("Employee.EmployeeNo")));
        }
        #endregion

        #region RetrieveEmployeeListByEmployeeIDSortByEmployeeName

        public IList<Employee> RetrieveEmployeeListByEmployeeIDSortByEmployeeName(short employeeID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            if (employeeID != 0)
                stb.AppendFormat(" {0} like {1} ", helper.GetFieldName("Employee.EmployeeID"),
                                    helper.CleanValue(employeeID));
            else
                stb.AppendFormat(" {0} like {1} ", helper.GetFieldName("Employee.EmployeeID"),
                                helper.CleanValue("%"));
            return Employee.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC", helper.GetFieldName("Employee.Name")));
        }
        #endregion

        #region RetrieveEmployeeListByEmployeeNameSortByEmployeeName

        public IList<Employee> RetrieveEmployeeListByEmployeeNameSortByEmployeeName(string name)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} like {1} ", helper.GetFieldName("Employee.Name"),
                            helper.CleanValue("%" + name + "%"));
            stb.AppendFormat(" AND ({0} is null OR {0} <> {1}) ", helper.GetFieldName("Employee.IsInactive"),
                                helper.CleanValue(true));
            return Employee.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC", helper.GetFieldName("Employee.Name")));
        }
        #endregion

        #region RetrieveEmployeeListByEmployeeNoByNameByDesignationSortByEmployeeName

        public IList<Employee> RetrieveEmployeeListByEmployeeNoByNameByDesignationByNRICByGradeByActiveSortByEmployeeName(string employeeNo, string name, string designation, string nric, string grade, bool isActive, short coyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} like {1} ", helper.GetFieldName("Employee.EmployeeNo"),
                                helper.CleanValue(employeeNo));
            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("Employee.Name"),
                                helper.CleanValue(name));
            if (coyId != 61)
                stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("Employee.CoyID"),
                                   helper.CleanValue(coyId));
            if (designation != "%")
                stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("Employee.Designation"),
                                    helper.CleanValue(designation));
            if (nric != "%")
                stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("Employee.NRIC"),
                                    helper.CleanValue(nric));
            if (grade != "%")
                stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("Employee.Grade"),
                                    helper.CleanValue(grade));
            if (isActive)
            {
                stb.AppendFormat(" and ({0} is null or {0} = {1}) ", helper.GetFieldName("Employee.IsInactive"), helper.CleanValue(!isActive));
            }
            else
                stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("Employee.IsInactive"),
                                    helper.CleanValue(!isActive));
            return Employee.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC", helper.GetFieldName("Employee.Name")));
        }
        #endregion

        #region RetrieveEmployeeListByDOBSortByEmployeeName

        public IList<Employee> RetrieveEmployeeListByDOBSortByEmployeeName(DateTime dob)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" month({0}) = month({1}) ", helper.GetFieldName("Employee.DOB"),
                                helper.CleanValue(dob));
            stb.AppendFormat(" and day({0}) = day({1}) ", helper.GetFieldName("Employee.DOB"),
                                helper.CleanValue(dob));
            stb.AppendFormat(" and ({0} is null or {0} = {1}) ", helper.GetFieldName("Employee.IsInactive"), helper.CleanValue(false));
            return Employee.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC", helper.GetFieldName("Employee.Name")));
        }
        #endregion

        #region RetrieveEmployeeListByEmployeeNoSortByEmployeeName

        public Employee RetrieveEmployeeListByEmployeeNoSortByEmployeeName(string employeeNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Employee.EmployeeNo"),
                              helper.CleanValue(employeeNo));
            return Employee.RetrieveFirst(stb.ToString(), string.Format(" {0} ASC", helper.GetFieldName("Employee.Name")));
        }
        #endregion

        #region RetrieveEmployeeListByDOBSortByEmployeeName
        public IList<Employee> RetrieveEmployeeListByCoyID(short coyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Employee.CoyID"),
                                helper.CleanValue(coyId));
            return Employee.RetrieveQuery(stb.ToString(), "");
        }
        #endregion

        #region RetrieveEmployeeListByCoyIDAndUserID
        public IList<Employee> RetrieveEmployeeListByCoyIDAndUserName(short coyId, string userName)
        {
            userName = "%" + userName + "%";
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Employee.CoyID"),
                                helper.CleanValue(coyId));
            stb.AppendFormat(" AND (IsInActive IS NULL OR IsInactive = 0)");

            //stb.AppendFormat(" AND {0} LIKE {1} ", helper.GetFieldName("Employee.EmailAddress"),
            //                  helper.CleanValue(userName));
            stb.AppendFormat(" AND SuperiorID IN (SELECT EmployeeID FROM tbEmployee WHERE {0} = {1}", helper.GetFieldName("Employee.CoyID"), helper.CleanValue(coyId));
            stb.AppendFormat(" AND {0} LIKE {1})", helper.GetFieldName("Employee.EmailAddress"), helper.CleanValue(userName));
            stb.AppendFormat(" ORDER BY {0}", helper.GetFieldName("Employee.Name"));
            return Employee.RetrieveQuery(stb.ToString(), "");
        }
        #endregion

        #region RetrieveNotificationPartyListByPurpose

        public IList<NotificationParty> RetrieveNotificationPartyListByPurpose(string purpose)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("NotificationParty.Purpose"),
                                helper.CleanValue(purpose));
            stb.AppendFormat(" and {0} is null ", helper.GetFieldName("NotificationParty.CoyID"));


            return NotificationParty.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC", helper.GetFieldName("NotificationParty.EmployeeID")));
        }
        #endregion


        #region RetrieveNotificationPartyListByPurposeByCoyID

        public IList<NotificationParty> RetrieveNotificationPartyListByPurposeByCoyID(string purpose, short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("NotificationParty.Purpose"),
                                helper.CleanValue(purpose));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("NotificationParty.CoyID"),
                                helper.CleanValue(companyID));

            return NotificationParty.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC", helper.GetFieldName("NotificationParty.EmployeeID")));
        }
        #endregion


        #region RetrieveAllCourseLanguageList
        public IList<CourseLanguage> RetrieveAllCourseLanguageListSortByLanguageName()
        {
            QueryHelper helper = base.GetHelper();

            return CourseLanguage.RetrieveQuery("", string.Format(" {0} ASC", helper.GetFieldName("CourseLanguage.LanguageName")));
        }

        #region RetrieveNotificationPartyListByFormTypeStatus

        public IList<NotificationParty> RetrieveNotificationPartyListByFormTypeStatus(string purpose, string status)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("NotificationParty.Purpose"),
                                helper.CleanValue(purpose));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("NotificationParty.Status"),
                                helper.CleanValue(status));
            return NotificationParty.RetrieveQuery(stb.ToString());
        }
        #endregion

        public IList<EmployeeCourse> RetrieveEmployeeCourseListByCourseByEmployeeByDateByFormStatusSortByDateEmployeeNoCourseCode(string course, string employee, DateTime dateFrom, DateTime dateTo, string registrationStatus)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder where = new StringBuilder(200);
            where.AppendFormat("EmployeeObject[Name like {1}] And CourseSessionObject[CourseObject[CourseTitle like {0}] ", helper.CleanValue(course), helper.CleanValue(employee));
            if (dateFrom != GMSCoreBase.DEFAULT_NO_DATE.AddYears(3) && dateTo != GMSCoreBase.DEFAULT_NO_DATE.AddYears(3))
                where.AppendFormat(" AND DateFrom >= {0} AND DateFrom <= {1}]", helper.CleanValue(dateFrom), helper.CleanValue(dateTo));
            else
                where.AppendFormat(" ]");
            if (registrationStatus == "P" || registrationStatus == "A")
                where.AppendFormat("AND Status = {0}", helper.CleanValue(registrationStatus));
            OPathQuery<EmployeeCourse> query = new OPathQuery<EmployeeCourse>(where.ToString(), "CourseSessionObject.DateFrom DESC, EmployeeObject.EmployeeNo");
            return DBManager.GetInstance().Engine.GetObjectSet<EmployeeCourse>(query);
        }
        #endregion

        //Activities for Bank Facilities
        #region RetrieveAllBankListSortByBankName
        public IList<Bank> RetrieveAllBankListSortByBankName()
        {
            QueryHelper helper = base.GetHelper();

            return Bank.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Bank.BankName")));
        }
        #endregion

        #region RetrieveAllBankListSortByBankCode
        public IList<Bank> RetrieveAllBankListSortByBankCode()
        {
            QueryHelper helper = base.GetHelper();

            return Bank.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Bank.BankCode")));
        }
        #endregion

        #region RetrieveAllBankUtilisationListSortByDateByTransactionNo
        public IList<BankUtilisation> RetrieveAllBankUtilisationListSortByBankDateByTransactionNo()
        {
            QueryHelper helper = base.GetHelper();

            return BankUtilisation.RetrieveQuery("", string.Format(" {0} ASC, {1} ASC ", helper.GetFieldName("BankUtilisation.TransactionDate"), helper.GetFieldName("BankUtilisation.TransactionNo")));
        }
        #endregion

        #region RetrieveBankUtilisationListByNameByTransactionNoByTransactionDateByChequeDateByBankCodeSortByTransactionDateByTransactionNo

        public IList<BankUtilisation> RetrieveBankUtilisationListByCompanyIDByNameByModeByTransactionNoByTransactionDateByChequeDateByBankCOAIDByTypeSortByTransactionDateByTransactionNo(short companyID, string name, string mode, string transactionNO, DateTime tDateFrom, DateTime tDateTo, DateTime cDateFrom, DateTime cDateTo, short bankCOAID, string type)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BankUtilisation.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("BankUtilisation.Name"),
                                helper.CleanValue(name));

            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("BankUtilisation.Mode"),
                                helper.CleanValue(mode));

            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("BankUtilisation.TransactionNo"),
                                helper.CleanValue(transactionNO));

            if (bankCOAID != -1)
                stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("BankUtilisation.BankCOAID"),
                                    helper.CleanValue(bankCOAID));
            stb.AppendFormat(" and {0} >= {1} ", helper.GetFieldName("BankUtilisation.TransactionDate"),
                                helper.CleanValue(tDateFrom));
            stb.AppendFormat(" and {0} <= {1} ", helper.GetFieldName("BankUtilisation.TransactionDate"),
                                helper.CleanValue(tDateTo));
            if (cDateFrom != GMSCoreBase.DEFAULT_NO_DATE.AddYears(3) && cDateTo != GMSCoreBase.DEFAULT_NO_DATE.AddYears(3))
            {
                stb.AppendFormat(" and {0} >= {1} ", helper.GetFieldName("BankUtilisation.ChequeDate"),
                                    helper.CleanValue(cDateFrom));
                stb.AppendFormat(" and {0} <= {1} ", helper.GetFieldName("BankUtilisation.ChequeDate"),
                                    helper.CleanValue(cDateTo));
            }
            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("BankUtilisation.Type"), helper.CleanValue(type));
            stb.AppendFormat(" and {0} is null ", helper.GetFieldName("BankUtilisation.IsOld"));

            return BankUtilisation.RetrieveQuery(stb.ToString(), string.Format(" {0} DESC, {1} DESC", helper.GetFieldName("BankUtilisation.TransactionDate"), helper.GetFieldName("BankUtilisation.TransactionNo")));
        }
        #endregion

        #region RetrieveAllBankAccountListByCompanyIDSortByBankAccount
        public IList<BankAccount> RetrieveAllBankAccountListByCompanyIDSortByBankAccount(short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BankAccount.CoyID"),
                                helper.CleanValue(companyID));

            return BankAccount.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("BankAccount.BankCOA")));
        }
        #endregion



        // Addd By Kim 23July2012
        #region RetrieveAllBankAccountListByMajorBankExclusiveCurrentBank
        public IList<BankAccount> RetrieveAllBankAccountListByMajorBankExclusiveCurrentBank(short companyID, bool isMajorBank, short coaID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BankAccount.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BankAccount.IsMajorBank"),
                                helper.CleanValue(isMajorBank));
            stb.AppendFormat(" AND {0} != {1} ", helper.GetFieldName("BankAccount.COAID"),
                                helper.CleanValue(coaID));

            return BankAccount.RetrieveQuery(stb.ToString());
        }
        #endregion



        #region RetrieveAllCustomerAccountsListByPrefixByCompanyIDSortByAccountName
        public IList<A21Account> RetrieveAllCustomerAccountsListByPrefixByCompanyIDSortByAccountName(string prefix, short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("A21Account.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("A21Account.AccountName"),
                                helper.CleanValue(prefix));

            return A21Account.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("A21Account.AccountName")));
        }
        #endregion

        #region RetrieveAllProductGroupNameListByPrefixByCompanyIDSortByProductGroupName
        public IList<ProductGroup> RetrieveAllProductGroupNameListByPrefixByCompanyIDSortByProductGroupName(string prefix, short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ProductGroup.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("ProductGroup.ProductGroupName"),
                                helper.CleanValue(prefix));

            return ProductGroup.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("ProductGroup.ProductGroupName")));
        }
        #endregion

        #region RetrieveAllProductNameListByPrefixByCompanyIDSortByProductName
        public IList<Product> RetrieveAllProductNameListByPrefixByCompanyIDSortByProductName(string prefix, short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Product.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("Product.ProductName"),
                                helper.CleanValue(prefix));

            return Product.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("Product.ProductName")));
        }
        #endregion




        #region RetrieveAllBankReceiverPayerListByPrefixByCompanyIDSortByName
        public IList<BankReceiverPayer> RetrieveAllBankReceiverPayerListByPrefixByCompanyIDSortByName(string prefix, short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BankReceiverPayer.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("BankReceiverPayer.Name"),
                                helper.CleanValue(prefix));

            return BankReceiverPayer.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("BankReceiverPayer.Name")));
        }
        #endregion

        //Organization Chart
        #region RetrieveOrganzationChartLinkByPageID
        public IList<OrganizationChartLink> RetrieveAllOrganzationChartLinkByPageID(short pageID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("OrganizationChartLink.PageID"),
                                helper.CleanValue(pageID));
            return OrganizationChartLink.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC", helper.GetFieldName("OrganizationChartLink.LinkID")));
        }
        #endregion

        //Bulletin
        #region RetrieveAllMessagesSortByDate

        public IList<Bulletin> RetrieveAllMessagesSortByDate()
        {
            QueryHelper helper = base.GetHelper();
            return Bulletin.RetrieveQuery("", string.Format(" {0} DESC", helper.GetFieldName("Bulletin.CreatedDate")));
        }
        #endregion

        #region RetrieveAllSalesPersonListByCompanyIDSortBySalesPersonID
        public IList<SalesPerson> RetrieveAllSalesPersonListByCompanyIDSortBySalesPersonID(short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesPerson.CoyID"),
                                helper.CleanValue(companyID));

            return SalesPerson.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("SalesPerson.SalesPersonID")));
        }
        #endregion

        #region RetrieveAllSalesPersonListByCompanyIDSortBySalesPersonName
        public IList<SalesPerson> RetrieveAllSalesPersonListByCompanyIDSortBySalesPersonName(short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesPerson.CoyID"),
                                helper.CleanValue(companyID));

            return SalesPerson.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("SalesPerson.SalesPersonName")));
        }
        #endregion

        //SalesPersonRecord
        #region RetrieveAllSalesPersonRecordListByCompanyIDSortByYearMonthSalesPersonMasterName
        public IList<SalesPersonRecord> RetrieveAllSalesPersonRecordListByCompanyIDSortByYearMonthSalesPersonMasterID(short companyID, short year, short month, string GroupType)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesPersonRecord.CoyID"),
                                helper.CleanValue(companyID));

            if (year != 0)
                stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("SalesPersonRecord.TbYear"),
                                helper.CleanValue(year));
            if (month != 0)
                stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("SalesPersonRecord.TbMonth"),
                                helper.CleanValue(month));
            if (GroupType != "All")
                stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("SalesPersonRecord.GroupType"),
                                helper.CleanValue(GroupType));

            return SalesPersonRecord.RetrieveQuery(stb.ToString(), string.Format(" {0} DESC, {1} DESC, {2} ASC ", helper.GetFieldName("SalesPersonRecord.TbYear"), helper.GetFieldName("SalesPersonRecord.TbMonth"), helper.GetFieldName("SalesPersonRecord.SalesPersonMasterID")));
        }
        #endregion

        #region RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterName
        public IList<SalesPersonMaster> RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterName(short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesPersonMaster.CoyID"),
                                helper.CleanValue(companyID));
             
            return SalesPersonMaster.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("SalesPersonMaster.SalesPersonMasterName")));
        }
        #endregion

        #region RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterNameByMode
        public IList<SalesPersonMaster> RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterNameByMode(short companyID, string mode)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesPersonMaster.CoyID"),
                                helper.CleanValue(companyID));
            if (mode == "Team")
            {
                stb.AppendFormat("AND {0} like {1} ", helper.GetFieldName("SalesPersonMaster.SalesPersonMasterName"),
                                helper.CleanValue("%/%"));
            }
            else //"Member"
            {
                stb.AppendFormat("AND {0} not like {1} ", helper.GetFieldName("SalesPersonMaster.SalesPersonMasterName"),
                               helper.CleanValue("%/%"));
            }

            return SalesPersonMaster.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("SalesPersonMaster.SalesPersonMasterName")));
        }
        #endregion

        #region RetrieveAllSalesPersonMappingListByCompanyIDSalesPersonMasterNameSortBySalesPersonID
        public IList<SalesPersonMapping> RetrieveAllSalesPersonMappingListBySalesPersonMasterIDSortBySalesPersonID(short salesPersonMasterID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat("{0} = {1} ", helper.GetFieldName("SalesPersonMapping.SalesPersonMasterID"),
                                helper.CleanValue(salesPersonMasterID));

            return SalesPersonMapping.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("SalesPersonMapping.SalesPersonID")));
        }
        #endregion

        #region RetrieveProductGroupByName
        public IList<ProductGroup> RetrieveProductGroupByCoyIDName(short companyId, string itemName)
        {
            if (itemName.Length <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ProductGroup.ProductGroupName"),
                                helper.CleanValue(itemName));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ProductGroup.CoyID"),
                                helper.CleanValue(companyId));

            return ProductGroup.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveAllSalesPersonByCompanyIDSortBySalesPersonID
        public IList<SalesPerson> RetrieveAllSalesPersonByCompanyIDSortBySalesPersonID(short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesPerson.CoyID"),
                                helper.CleanValue(companyID));

            stb.AppendFormat(" and {0} not like {1} ", helper.GetFieldName("SalesPerson.SalesPersonName"),
                               helper.CleanValue("%DO NOT USE%"));   

            return SalesPerson.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("SalesPerson.SalesPersonID")));
        }
        #endregion


        #region RetrieveAllUserSalesPersonByUserNumIDCoyID
        public IList<SalesPersonUser> RetrieveAllSalesPersonUserByUserNumIDCoyID(short userNumId, short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesPersonUser.UserID"),
                                helper.CleanValue(userNumId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("SalesPersonUser.CoyID"),
                                helper.CleanValue(companyId));

            return SalesPersonUser.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("SalesPersonUser.SalesPersonID")));
        }
        #endregion

        #region RetrieveAllManagerSalesPersonByUserNumIDCoyID
        public IList<ManagerSalesPerson> RetrieveAllManagerSalesPersonByManagerUserIDCoyID(short userNumId, short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ManagerSalesPerson.ManagerUserID"),
                                helper.CleanValue(userNumId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ManagerSalesPerson.CoyID"),
                                helper.CleanValue(companyId));

            return ManagerSalesPerson.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("ManagerSalesPerson.SalesPersonID")));
        }
        #endregion

        //Entertainment Expenses
        #region RetrieveEntertainmentExpenseListByCoyIDByNameByAreaByPaymentDateByOthers

        public IList<EntertainmentExpense> RetrieveEntertainmentExpenseListByNameByAreaByPaymentDateByOthers(short companyID, string name, string area, DateTime tDateFrom, DateTime tDateTo, bool others)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder where = new StringBuilder(200);
            OPathQuery<EntertainmentExpense> query = null;
            if (!others)
            {
                where.AppendFormat("SalesPersonMasterObject[SalesPersonMasterName like {0}] ", helper.CleanValue(name));
                where.AppendFormat("AND Date >= {0} AND Date <= {1} AND Area like {2} AND Others = {3} AND CoyID = {4}", helper.CleanValue(tDateFrom), helper.CleanValue(tDateTo), helper.CleanValue(area), helper.CleanValue(others), helper.CleanValue(companyID));
                query = new OPathQuery<EntertainmentExpense>(where.ToString(), "Date DESC, SalesPersonMasterObject.SalesPersonMasterName");
            }
            else
            {
                where.AppendFormat("Date >= {0} AND Date <= {1} AND Others = {2} AND CoyID = {3}", helper.CleanValue(tDateFrom), helper.CleanValue(tDateTo), helper.CleanValue(others), helper.CleanValue(companyID));
                query = new OPathQuery<EntertainmentExpense>(where.ToString(), "Date DESC");
            }
            return DBManager.GetInstance().Engine.GetObjectSet<EntertainmentExpense>(query);
        }
        #endregion

        //ProductManagerMaster
        #region RetrieveAllProductManagerMasterListByCompanyIDSortByEffectiveDate
        public IList<ProductManagerMaster> RetrieveAllProductManagerMasterListByCompanyIDSortByEffectiveDate(short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ProductManagerMaster.CoyID"),
                                helper.CleanValue(companyID));

            return ProductManagerMaster.RetrieveQuery(stb.ToString(), string.Format(" {0} DESC ", helper.GetFieldName("ProductManagerMaster.EffectiveDate")));
        }
        #endregion

        #region RetrieveAllProductGroupManagerListByCompanyIDSortByProductGroupManagerName
        public IList<ProductGroupManager> RetrieveAllProductGroupManagerListByCompanyIDSortByProductGroupManagerName(short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ProductGroupManager.CoyID"),
                                helper.CleanValue(companyID));

            return ProductGroupManager.RetrieveQuery(stb.ToString(),
                string.Format(" {0} ASC ", helper.GetFieldName("ProductGroupManager.ProductGroupManagerName")));
        }
        #endregion

        //Company Department
        #region RetrieveAllCompanyDepartmentListByCompanyIDSortByDepartmentID
        public IList<CompanyDepartment> RetrieveAllCompanyDepartmentListByCompanyIDSortByDepartmentID(short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("CompanyDepartment.CoyID"),
                                helper.CleanValue(companyID));

            return CompanyDepartment.RetrieveQuery(stb.ToString(),
                string.Format(" {0} ASC ", helper.GetFieldName("CompanyDepartment.DepartmentID")));
        }
        #endregion

        //Company Department
        #region RetrieveAllCompanyDepartmentListByCompanyIDSortByDepartmentName
        public IList<CompanyDepartment> RetrieveAllCompanyDepartmentListByCompanyIDSortByDepartmentName(short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("CompanyDepartment.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} is not null ", helper.GetFieldName("CompanyDepartment.DepartmentName"),
                                helper.CleanValue(true));

            return CompanyDepartment.RetrieveQuery(stb.ToString(),
                string.Format(" {0} ASC ", helper.GetFieldName("CompanyDepartment.DepartmentName")));
        }
        #endregion

        //Company Project
        #region RetrieveAllCompanyProjectListByCompanyIDSortByProjectID
        public IList<CompanyProject> RetrieveAllCompanyProjectListByCompanyIDSortByProjectID(short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("CompanyProject.CoyID"),
                                helper.CleanValue(companyID));

            return CompanyProject.RetrieveQuery(stb.ToString(),
                string.Format(" {0} ASC ", helper.GetFieldName("CompanyProject.ProjectID")));
        }
        #endregion

        //Debtors
        #region RetrieveCommentHistorySortByCommentDate
        public IList<DebtorCommentary> RetrieveCommentHistorySortByCommentDate(short companyID, string accountCode, string currency)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("DebtorCommentary.CoyID"),
                                helper.CleanValue(companyID));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("DebtorCommentary.AccountCode"),
                                helper.CleanValue(accountCode));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("DebtorCommentary.CurrencyCode"),
                                helper.CleanValue(currency));

            //stb.AppendFormat(" AND {0} = 0 ", helper.GetFieldName("ForeignExchangeRate.IsInEffect"),

            //                    helper.CleanValue(foreignCurrencyCode));
            StringBuilder stbSort = new StringBuilder(200);
            stbSort.AppendFormat(" {0} DESC ", helper.GetFieldName("DebtorCommentary.CreatedDate"));

            return DebtorCommentary.RetrieveQuery(stb.ToString(), stbSort.ToString());
        }

        #endregion

        //COA Mapping
        #region RetrieveAllCOAMappingByOldCOAIDSortByOldCOAID
        public IList<ChartOfAccountsMapping> RetrieveAllCOAMappingByOldCOAIDSortByOldCOAID(string oldCOAID, short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ChartOfAccountsMapping.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("ChartOfAccountsMapping.OldCOAID"),
                                helper.CleanValue(oldCOAID));

            return ChartOfAccountsMapping.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("ChartOfAccountsMapping.OldCOAID")));
        }
        #endregion


        #region RetrieveAllCOAMappingByCOAIDSortByOldCOAID
        public IList<ChartOfAccountsMapping> RetrieveAllCOAMappingByCOAIDSortByOldCOAID(string oldCOAID, string newCOAID, short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ChartOfAccountsMapping.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("ChartOfAccountsMapping.OldCOAID"),
                                helper.CleanValue(oldCOAID));
            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("ChartOfAccountsMapping.NewCOAID"),
                                helper.CleanValue(newCOAID));

            return ChartOfAccountsMapping.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("ChartOfAccountsMapping.OldCOAID")));
        }
        #endregion

        //DocumentCategory
        #region RetrieveAllDocumentCategoryByModuleCategoryID
        public IList<DocumentCategory> RetrieveAllDocumentCategoryByModuleCategoryID(short moduleCategoryID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("DocumentCategory.ModuleCategoryID"),
                                helper.CleanValue(moduleCategoryID));

            return DocumentCategory.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("DocumentCategory.SeqID")));
        }
        #endregion

        #region RetrieveAllDocumentCategoryByModuleCategoryID
        public IList<DocumentCategory> RetrieveAllDocumentCategoryByModuleCategoryIDByDocumentCategoryID(short moduleCategoryID, string documentCategoryID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("DocumentCategory.ModuleCategoryID"),
                                helper.CleanValue(moduleCategoryID));

            //String documentcategory = documentCategoryID;
            string[] documents = documentCategoryID.Split(',');

            for (int i = 0; i < documents.Length; i++)
            {
                if (i == 0)
                {
                    stb.AppendFormat(" AND ( {0} = {1} ", helper.GetFieldName("DocumentCategory.DocumentCategoryID"),
                               helper.CleanValue(documents[i]));
                }
                else
                {
                    stb.AppendFormat(" OR {0} = {1} ", helper.GetFieldName("DocumentCategory.DocumentCategoryID"),
                               helper.CleanValue(documents[i]));
                }

            }

            stb.AppendFormat(")");

            return DocumentCategory.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("DocumentCategory.SeqID")));
        }
        #endregion

        #region RetrieveAllDocumentByDocumentCategoryID
        public IList<Document> RetrieveAllDocumentByDocumentCategoryID(short documentCategoryID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Document.DocumentCategoryID"),
                                helper.CleanValue(documentCategoryID));

            return Document.RetrieveQuery(stb.ToString(), string.Format(" {0}, {1} ASC ", helper.GetFieldName("Document.DocumentName"), helper.GetFieldName("Document.SeqID")));
        }
        #endregion

        #region RetrieveAllDocumentBySeqID
        public IList<Document> RetrieveAllDocumentBySeqID(short documentCategoryID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Document.DocumentCategoryID"),
                                helper.CleanValue(documentCategoryID));

            return Document.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("Document.SeqID")));
        }
        #endregion

        #region RetrieveAllDocumentForCompanyBySeqID
        public IList<DocumentForCompany> RetrieveAllDocumentForCompanyBySeqID(short documentCategoryID, short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("DocumentForCompany.DocumentCategoryID"),
                                helper.CleanValue(documentCategoryID));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("DocumentForCompany.CoyID"),
                                helper.CleanValue(companyId));

            return DocumentForCompany.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("DocumentForCompany.SeqID")));
        }
        #endregion

        #region RetrieveAllDocuments
        public IList<Document> RetrieveAllDocuments()
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            return Document.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("Document.DocumentCategoryID")));
        }
        #endregion

        #region RetrieveAllDocumentsByEmployeeID
        public IList<DocumentForEmployee> RetrieveAllDocumentsByEmployeeID(short employeeId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("DocumentForEmployee.EmployeeID"),
                                helper.CleanValue(employeeId));

            return DocumentForEmployee.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("DocumentForEmployee.DocumentID")));
        }
        #endregion

        //Product
        #region RetrieveAllProductListByProdcodeSortByProdCode
        public IList<Product> RetrieveAllProductListByProdcodeSortByProdCode(string prodCode, int companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Product.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat(" and {0} like {1} ", helper.GetFieldName("Product.ProductCode"),
                                helper.CleanValue(prodCode));
            //stb.AppendFormat(" or {0} like {1}) ", helper.GetFieldName("Product.ProductName"),
            //                  helper.CleanValue(prodCode));


            return Product.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("Product.ProductCode")));
        }
        #endregion

        //TaxType
        #region RetrieveAllTaxTypeListByCompanyCode
        public IList<TaxType> RetrieveAllTaxTypeListByCompanyCode(short companyId, bool isCustomerSales)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("TaxType.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("TaxType.IsCustomerSales"),
                                helper.CleanValue(isCustomerSales));

            return TaxType.RetrieveQuery(stb.ToString(), "");
        }
        #endregion

        //Quotation
        #region RetrieveAllQuotationDetailDescriptionListByCompanyCodeQuotationNoSNNo
        public IList<QuotationDetailDescription> RetrieveAllQuotationDetailDescriptionListByCompanyCodeQuotationNoSNNo(short companyId,
                                                                                                string quotationNo, byte snNo, byte revisionNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("QuotationDetailDescription.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationDetailDescription.QuotationNo"),
                                helper.CleanValue(quotationNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationDetailDescription.SNNo"),
                                helper.CleanValue(snNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationDetailDescription.RevisionNo"),
                                helper.CleanValue(revisionNo));

            return QuotationDetailDescription.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("QuotationDetailDescription.SeqID")));
        }
        #endregion

        #region RetrieveAllQuotationDetailListByCompanyCodeQuotationNo
        public IList<QuotationDetail> RetrieveAllQuotationDetailListByCompanyCodeQuotationNo(short companyId,
                                                                                                string quotationNo, byte revisionNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("QuotationDetail.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationDetail.QuotationNo"),
                                helper.CleanValue(quotationNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationDetail.RevisionNo"),
                                helper.CleanValue(revisionNo));

            return QuotationDetail.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("QuotationDetail.SNNo")));
        }
        #endregion

        #region RetrieveTransactionApprovalByRandomID
        public TransactionApproval RetrieveTransactionApprovalByRandomID(string randomID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("TransactionApproval.ApprovalRandomID"),
                                helper.CleanValue(randomID));

            return TransactionApproval.RetrieveFirst(stb.ToString(), "");
        }
        #endregion

        #region RetrieveAllQuotationLimitApprovalByQuotationNo
        public IList<TransactionApproval> RetrieveAllQuotationLimitApprovalByQuotationNo(short companyId,
                                                                                                string quotationNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("TransactionApproval.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("TransactionApproval.TrnType"),
                                helper.CleanValue("QO"));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("TransactionApproval.TrnNo"),
                                helper.CleanValue(quotationNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("TransactionApproval.ApprovalType"),
                                helper.CleanValue("Limit"));

            return TransactionApproval.RetrieveQuery(stb.ToString(), "");
        }
        #endregion

        //Company Special Data
        #region RetrieveAllQuotationLimitApprovalByQuotationNo
        public IList<CompanySpecialDataPurpose> RetrieveAllCompanySpecialDataPurposeByCoyID(short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("CompanySpecialDataPurpose.CoyID"),
                                helper.CleanValue(companyId));

            return CompanySpecialDataPurpose.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("CompanySpecialDataPurpose.SpecialDataPurposeID")));
        }
        #endregion

        //Commission
        #region RetrieveKeyCustomersByCoyID
        public IList<KeyCustomerAverageGP> RetrieveKeyCustomersByCoyID(short companyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("KeyCustomerAverageGP.CoyID"),
                                helper.CleanValue(companyID));
            return KeyCustomerAverageGP.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("KeyCustomerAverageGP.SalesPersonMasterID")));
        }
        #endregion

        //Sales Package
        #region RetrieveAllPackageByCoyID
        public IList<Package> RetrieveAllPackageByCoyID(short coyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Package.CoyID"),
                                helper.CleanValue(coyID));

            return Package.RetrieveQuery(stb.ToString(), string.Format(" {0} DESC ", helper.GetFieldName("Package.PackageID")));
        }
        #endregion

        #region RetrieveAllPackageProductByPackageID
        public IList<PackageProduct> RetrieveAllPackageProductByPackageID(int packageID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("PackageProduct.PackageID"),
                                helper.CleanValue(packageID));

            return PackageProduct.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("PackageProduct.SeqID")));
        }
        #endregion

        #region RetrieveAllPackageDetailByPackageID
        public IList<PackageDetail> RetrieveAllPackageDetailByPackageID(int packageID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("PackageDetail.PackageID"),
                                helper.CleanValue(packageID));

            return PackageDetail.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("PackageDetail.SeqID")));
        }
        #endregion

        #region RetrieveAllPackageByQuotationNo
        public IList<QuotationPackage> RetrieveAllPackageByQuotationNo(string quotationNo, short companyID, byte revisionNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("QuotationPackage.QuotationNo"),
                                helper.CleanValue(quotationNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationPackage.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationPackage.RevisionNo"),
                                helper.CleanValue(revisionNo));

            return QuotationPackage.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("QuotationPackage.DSNNo")));
        }
        #endregion

        #region RetrieveAllPackageProductByQuotationNoSNNo
        public IList<QuotationPackageProduct> RetrieveAllPackageProductByQuotationNoSNNo(short companyId, string quotationNo, byte snNo, byte revisionNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("QuotationPackageProduct.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationPackageProduct.QuotationNo"),
                                helper.CleanValue(quotationNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationPackageProduct.SNNo"),
                                helper.CleanValue(snNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationPackageProduct.RevisionNo"),
                                helper.CleanValue(revisionNo));

            return QuotationPackageProduct.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("QuotationPackageProduct.ProductCode")));
        }
        #endregion

        #region RetrieveAllPackageDetailByQuotationNoSNNo
        public IList<QuotationPackageDetail> RetrieveAllPackageDetailByQuotationNoSNNo(short companyId, string quotationNo, byte snNo, byte revisionNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("QuotationPackageDetail.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationPackageDetail.QuotationNo"),
                                helper.CleanValue(quotationNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationPackageDetail.SNNo"),
                                helper.CleanValue(snNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationPackageDetail.RevisionNo"),
                                helper.CleanValue(revisionNo));

            return QuotationPackageDetail.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("QuotationPackageDetail.SeqID")));
        }
        #endregion

        #region RetrieveAllTNCByQuotationNo
        public IList<QuotationTermsNConditions> RetrieveAllTNCByQuotationNo(short companyId, string quotationNo, byte revisionNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("QuotationTermsNConditions.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationTermsNConditions.QuotationNo"),
                                helper.CleanValue(quotationNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("QuotationTermsNConditions.RevisionNo"),
                                helper.CleanValue(revisionNo));

            return QuotationTermsNConditions.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("QuotationTermsNConditions.DSNNo")));
        }
        #endregion

        // CashFlowProjection
        // Addd By Kim 31Aug2012
        #region RetrieveAllCashFlowProjectionByYearByWeek
        public IList<CashFlowProjection> AllCashFlowProjectionByYearByWeek(short companyID, short year, short week)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("CashFlowProjection.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("CashFlowProjection.TbYear"),
                                helper.CleanValue(year));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("CashFlowProjection.TbWeek"),
                                helper.CleanValue(week));

            return CashFlowProjection.RetrieveQuery(stb.ToString());
        }
        #endregion

        public ResultType CreateCashFlowProjection(ref CashFlowProjection cashFlowProjection, LogSession session)
        {
            if (cashFlowProjection == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!cashFlowProjection.IsValid())
                return ResultType.MainDataNotValid;

            cashFlowProjection.Save();

            return ResultType.Ok;
        }

        #region RetrieveIndustryByName
        public AccountIndustry RetrieveIndustryByName(string name)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("AccountIndustry.Name"),
                                helper.CleanValue(name));
            return AccountIndustry.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveGradeByGradeCode
        public AccountGrade RetrieveGradeByGradeCode(string name)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("AccountGrade.GradeCode"),
                                helper.CleanValue(name));

            return AccountGrade.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveAllIndustry()
        public IList<AccountIndustry> RetrieveAllIndustry()
        {
            return AccountIndustry.RetrieveAll();
        }
        #endregion


        #region RetrieveProductGroupByCoyID
        public IList<ProductGroup> RetrieveProductGroupByCoyID(short companyId)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ProductGroup.CoyID"),
                                helper.CleanValue(companyId));

            return ProductGroup.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveProductByCoyID
        public IList<Product> RetrieveProductByCoyID(short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Product.CoyID"),
                                helper.CleanValue(companyId));

            return Product.RetrieveQuery(stb.ToString());
        }
        #endregion


        #region RetrieveAllFinanceAttachmentTypeSortByID
        public IList<FinanceAttachmentType> RetrieveAllFinanceAttachmentTypeSortByID()
        {
            QueryHelper helper = base.GetHelper();

            return FinanceAttachmentType.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("FinanceAttachmentType.Id")));

        }
        #endregion

        #region RetrieveFinanceAttachmentByAccountName
        public IList<FinanceAttachment> RetrieveFinanceAttachmentByAccountName(string accountname)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FinanceAttachmentType.AccountName"),
                                helper.CleanValue(accountname));

            return FinanceAttachment.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("FinanceAttachmentType.Id")));

        }
        #endregion

        #region RetrieveAllScheduledTaskProductByCompany
        public IList<ScheduledTaskProduct> RetrieveAllScheduledTaskProductByCompany(short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ScheduledTaskProduct.CoyID"),
                                helper.CleanValue(companyId));
            return ScheduledTaskProduct.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("ScheduledTaskProduct.ProductCode")));
        }
        #endregion

        #region RetrieveAllScheduledTaskGRNDetailByCompany
        public IList<ScheduledTaskGRNDetail> RetrieveAllScheduledTaskGRNDetailByCompany(short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ScheduledTaskGRNDetail.CoyID"),
                                helper.CleanValue(companyId));
            return ScheduledTaskGRNDetail.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("ScheduledTaskGRNDetail.ProductCode")));
        }
        #endregion



        public ResultType DeleteScheduledTaskProduct(ScheduledTaskProduct product)
        {
            product.Delete();
            return ResultType.Ok;
        }

        public ResultType DeleteScheduledTaskGRNDetail(ScheduledTaskGRNDetail grnDetail)
        {
            grnDetail.Delete();
            return ResultType.Ok;
        }

        #region RetrieveCompanyByCoyId
        public Company RetrieveCompanyByCoyId(short coyId)
        {
            if (coyId <= 0)
                return null;

            return Company.RetrieveByKey(coyId);
        }
        #endregion


        #region RetrieveAllMRDetailDescriptionListByCompanyCodeMRNoDetailNo
        public IList<MRDetailDescription> RetrieveAllMRDetailDescriptionListByCompanyCodeMRNoDetailNo(short companyId,
                                                                                                string mRNo, int detailNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRDetailDescription.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRDetailDescription.MRNo"),
                                helper.CleanValue(mRNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRDetailDescription.DetailNo"),
                                helper.CleanValue(detailNo));

            return MRDetailDescription.RetrieveQuery(stb.ToString(), string.Format(" {0} ASC ", helper.GetFieldName("MRDetailDescription.SeqID")));
        }
        #endregion

        #region RetrieveCompanyProjectByCountryAndName
        public CompanyProject RetrieveCompanyProjectByCountryAndName(string ProjectName, short CompanyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} AND {2} = {3}", helper.GetFieldName("CompanyProject.ProjectName"),
                                helper.CleanValue(ProjectName), helper.GetFieldName("CompanyProject.CoyID"), helper.CleanValue(CompanyID));

            return CompanyProject.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveCompanyDepartmentByCountryAndName
        public CompanyDepartment RetrieveCompanyDepartmentByCountryAndName(string DepartmentName, short CompanyID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} AND {2} = {3} ", helper.GetFieldName("CompanyDepartment.DepartmentName"),
                                helper.CleanValue(DepartmentName),
                                helper.GetFieldName("CompanyDepartment.CoyID"),
                                helper.CleanValue(CompanyID));

            return CompanyDepartment.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region GetSectionIDByNameAndCountry
        public int GetSectionIDByNameAndCountry(string sectionName, short CompanyID)
        {
            DBManager db = DBManager.GetInstance();
            QueryHelper helper = base.GetHelper();

            int sectionID = GMSUtil.ToInt(
                                    db.Engine.ExecuteScalar(
                                        string.Format("SELECT SectionID FROM tbCompanySection Where CoyID = {0} AND SectionName = '{1}' ",
                                            CompanyID,
                                            sectionName)));

            return sectionID;
        }
        #endregion

        #region GetUnitIDByNameAndCountry
        public int GetUnitIDByNameAndCountry(string unitName, short CompanyID)
        {
            DBManager db = DBManager.GetInstance();
            QueryHelper helper = base.GetHelper();

            int unitID = GMSUtil.ToInt(
                                    db.Engine.ExecuteScalar(
                                        string.Format("SELECT UnitID FROM tbCompanyUnit Where CoyID = {0} AND UnitName = '{1}' ",
                                            CompanyID,
                                            unitName)));

            return unitID;
        }
        #endregion

        #region RetrieveTeamSetupSalesGroup
        public IList<SalesGroup> RetrieveTeamSetupSalesGroup(short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesGroup.CoyID"),
                                helper.CleanValue(companyId));
           
            return SalesGroup.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveTeamSetupSalesTeamByGroupID
        public IList<SalesGroupTeam> RetrieveTeamSetupSalesTeamByGroupID(short companyId, short groupId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesGroupTeam.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("SalesGroupTeam.GroupID"),
                                helper.CleanValue(groupId));

            return SalesGroupTeam.RetrieveQuery(stb.ToString());
        }
        #endregion


        #region RetrieveTeamSetupSalesGroupTeam
        public IList<SalesGroupTeam> RetrieveTeamSetupSalesGroupTeam(short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesGroupTeam.CoyID"),
                                helper.CleanValue(companyId));

            return SalesGroupTeam.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveTeamSetupSalesGroupTeamByTeamID
        public SalesGroupTeam RetrieveTeamSetupSalesGroupTeamByTeamID(short companyId, short teamId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesGroupTeam.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat("AND {0} = {1} ", helper.GetFieldName("SalesGroupTeam.TeamID"),
                                helper.CleanValue(teamId));

            return SalesGroupTeam.RetrieveFirst(stb.ToString(), "");
        }
        #endregion

        #region RetrieveTeamSetupSalesTeamSalesPerson
        public IList<SalesTeamSalesPerson> RetrieveTeamSetupSalesTeamSalesPerson(short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesTeamSalesPerson.CoyID"),
                                helper.CleanValue(companyId));

            return SalesTeamSalesPerson.RetrieveQuery(stb.ToString());
        }
        #endregion


        #region RetrieveTeamSetupSalesTeamSalesPersonByTeamID
        public SalesTeamSalesPerson RetrieveTeamSetupSalesTeamSalesPersonByTeamID(short companyId, string salespersonid)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesTeamSalesPerson.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat("AND {0} = {1} ", helper.GetFieldName("SalesTeamSalesPerson.SalesPersonID"),
                                helper.CleanValue(salespersonid));

            return SalesTeamSalesPerson.RetrieveFirst(stb.ToString(), "");
        }
        #endregion

        #region RetrieveVendorApplicationFormByFormID
        public IList<Vendor> RetrieveVendorApplicationFormByVendorID(short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Vendor.CoyID"),
                                helper.CleanValue(companyId));

            return Vendor.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveVendorDetailsByFormID
        public IList<VendorApplicationForm> RetrieveVendorDetailsByVendorID(short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VendorApplicationForm.CoyID"),
                                helper.CleanValue(companyId));

            return VendorApplicationForm.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveVendorByVendorName
        public Vendor RetrieveVendorByVendorName(string vendorName)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Vendor.CompanyName"),
                                helper.CleanValue(vendorName));

            return Vendor.RetrieveFirst(stb.ToString());
        }
        #endregion
       
        #region
        public VendorApplicationForm RetrieveVendorApplicationFormByVendorID(int vendorID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VendorApplicationForm.VendorID"),
                                helper.CleanValue(vendorID));

            return VendorApplicationForm.RetrieveFirst(stb.ToString());
        }

        #endregion

        #region RetrieveVendorCompanyKeyPersonnel
        public IList<VendorCompanyKeyPersonnel> RetrieveVendorCompanyKeyPersonnel(short companyId, short vendorID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VendorCompanyKeyPersonnel.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat("AND {0} = {1} ", helper.GetFieldName("VendorCompanyKeyPersonnel.VendorID"),
                                helper.CleanValue(vendorID));

            return VendorCompanyKeyPersonnel.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveVendorCompanyKeyPersonnelByPersonnelID
        public VendorCompanyKeyPersonnel RetrieveVendorCompanyKeyPersonnelByPersonnelID(short companyId, short personnelId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VendorCompanyKeyPersonnel.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat("AND {0} = {1} ", helper.GetFieldName("VendorCompanyKeyPersonnel.PersonnelID"),
                                helper.CleanValue(personnelId));

            return VendorCompanyKeyPersonnel.RetrieveFirst(stb.ToString(), "");
        }
        #endregion

        #region RetrieveVendorCustomerProjectRecords
        public IList<VendorCustomerProjectRecords> RetrieveVendorCustomerProjectRecords(short companyId, short vendorId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VendorCustomerProjectRecords.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat("AND {0} = {1} ", helper.GetFieldName("VendorCustomerProjectRecords.VendorID"),
                               helper.CleanValue(vendorId));

            return VendorCustomerProjectRecords.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveVendorCustomerProjectRecordsByRecordID
        public VendorCustomerProjectRecords RetrieveVendorCustomerProjectRecordsByRecordID(short companyId, short recordId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VendorCustomerProjectRecords.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat("AND {0} = {1} ", helper.GetFieldName("VendorCustomerProjectRecords.RecordID"),
                                helper.CleanValue(recordId));

            return VendorCustomerProjectRecords.RetrieveFirst(stb.ToString(), "");
        }
        #endregion
       
        #region
        public IList<VendorApplicationForm> RetrieveVendorListByVendorByEmail(string CompanyName, string Email)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder where = new StringBuilder(200);
            where.AppendFormat("VendorObject[CompanyName like {0}] And VendorObject[Email like {1}] ", helper.CleanValue(CompanyName), helper.CleanValue(Email));
            OPathQuery<VendorApplicationForm> query = new OPathQuery<VendorApplicationForm>(where.ToString(), "VendorObject.CompanyName ASC, VendorObject.Email ASC");
            return DBManager.GetInstance().Engine.GetObjectSet<VendorApplicationForm>(query);
        }
        #endregion

        #region
        public IList<VendorApplicationForm> CheckVendorFormWithRandomID(string randomid, string formid)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder where = new StringBuilder(200);
            where.AppendFormat("RandomID LIKE {0} And FormID = {1} ", helper.CleanValue(randomid), helper.CleanValue(formid));
            OPathQuery<VendorApplicationForm> query = new OPathQuery<VendorApplicationForm>(where.ToString());
            return DBManager.GetInstance().Engine.GetObjectSet<VendorApplicationForm>(query);
        }
        #endregion

        #region RetrieveVendorCompanyPersonnelByVendor
        public IList<VendorCompanyKeyPersonnel> RetrieveVendorCompanyPersonnelByVendor(short companyId, short vendorId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VendorCompanyKeyPersonnel.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat("AND {0} = {1} ", helper.GetFieldName("VendorCompanyKeyPersonnel.VendorID"),
                               helper.CleanValue(vendorId));

            return VendorCompanyKeyPersonnel.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveVendorCustomerProjectRecordsByVendor
        public IList<VendorCustomerProjectRecords> RetrieveVendorCustomerProjectRecordsByVendor(short companyId, short vendorId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VendorCustomerProjectRecords.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat("AND {0} = {1} ", helper.GetFieldName("VendorCustomerProjectRecords.VendorID"),
                               helper.CleanValue(vendorId));

            return VendorCustomerProjectRecords.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveVendorCustomerProjectRecordsByVendor
        public VendorApplicationForm RetrieveVendorApplicationFormIDByRandomID(short companyId, string randomId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("VendorApplicationForm.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat("AND {0} = {1} ", helper.GetFieldName("VendorApplicationForm.RandomID"),
                               helper.CleanValue(randomId));

            return VendorApplicationForm.RetrieveFirst(stb.ToString(), "");
        }
        #endregion

    }

}
