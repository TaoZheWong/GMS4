using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;

namespace GMSCore
{
    /// <summary>
    /// The base class for this namespace
    /// </summary>
    public sealed class GMSCoreBase
    {
        #region Constant variables declarations
        /// <summary>
        /// Constant string value of the Culture code
        /// </summary>
        public const string SESSION_CULTURE_CODE = "MyWebCulture";
        /// <summary>
        /// Constant string value of the UI Culture code
        /// </summary>
        public const string SESSION_UICULTURE_CODE = "MyWebUICulture";
        /// <summary>
        /// Constant string to store the Session key.
        /// </summary>
        public const string SESSIONNAME = "MYSessionName";
        #endregion

        #region Static variables reading data from Config Files
        /// <summary>
        /// Static string of the Mapping File
        /// </summary>
        public static readonly string MAPPING_FILE = ConfigurationManager.AppSettings["MAPPING_FILE"];

        /// <summary>
        /// Static string of the Database connection string
        /// </summary>
        public static readonly string DB_CONNECTION_STR = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        /// <summary>
        /// Static string of the AuditTrial Database connection string
        /// </summary>
        public static readonly string AUDITTRAIL_CONNECTIONSTR = ConfigurationManager.ConnectionStrings["AuditTrialServer"].ConnectionString;

        /// <summary>
        /// Static string value of the Database's Name.
        /// </summary>
        public static readonly string DB_NAME = ConfigurationManager.AppSettings["DATABASE_NAME"];

        /// <summary>
        /// Static string value of the Server's Name.
        /// </summary>
        public static readonly string SERVER_NAME = ConfigurationManager.AppSettings["SERVER_NAME"];

        /// <summary>
        /// Static string value of the User's login Name.
        /// </summary>
        public static readonly string USER_LOGIN_NAME = ConfigurationManager.AppSettings["USER_LOGIN_NAME"];

        /// <summary>
        /// Static string value of the User's login PWD.
        /// </summary>
        public static readonly string USER_LOGIN_PWD = ConfigurationManager.AppSettings["USER_LOGIN_PWD"];

        /// <summary>
        /// Static string value of the Archive Database's Name.
        /// </summary>
        public static readonly string ARCHIVE_DB_NAME = ConfigurationManager.AppSettings["ARCHIVE_DATABASE_NAME"];

        /// <summary>
        /// Static string value of the Archive Server's Name.
        /// </summary>
        public static readonly string ARCHIVE_SERVER_NAME = ConfigurationManager.AppSettings["ARCHIVE_SERVER_NAME"];

        /// <summary>
        /// Static string value of the Archive User's login Name.
        /// </summary>
        public static readonly string ARCHIVE_USER_LOGIN_NAME = ConfigurationManager.AppSettings["ARCHIVE_USER_LOGIN_NAME"];

        /// <summary>
        /// Static string value of the Archive User's login PWD.
        /// </summary>
        public static readonly string ARCHIVE_USER_LOGIN_PWD = ConfigurationManager.AppSettings["ARCHIVE_USER_LOGIN_PWD"];

        /// <summary>
        /// Static string value of the Security File
        /// </summary>
        public static readonly string SECURITYFILE = ConfigurationManager.AppSettings["SECURITYFILE"];

        /// <summary>
        /// Static string value of the Documentation Path (Affects Report location)
        /// </summary>
        public static readonly string DOC_PATH = ConfigurationManager.AppSettings["DOC_PATH"];
       
        /// <summary>
        /// Static string value of the Temporary Documentation Path (Affects documents location)
        /// </summary>
        public static readonly string TEMP_DOC_PATH = ConfigurationManager.AppSettings["TEMP_DOC_PATH"];

        /// <summary>
        /// Static Datetime value to signify Default empty / null date.
        /// </summary>
        public static readonly DateTime DEFAULT_NO_DATE = new DateTime(1900, 1, 1);

        /// <summary>
        /// Static string value of the Company Deployment Name (Affects Report location)
        /// </summary>
        public static readonly string CUSTOMER_DEPLOYMENT_NAME = ConfigurationManager.AppSettings["CUSTOMER_DEPLOYMENT_NAME"];

        /// <summary>
        /// Static string value of the Companyinfo
        /// </summary>
        public static readonly string COMPANY_NAME = ConfigurationManager.AppSettings["COMPANY_NAME"];
        public static readonly string COMPANY_ADDRESS = ConfigurationManager.AppSettings["COMPANY_ADDRESS"];
        public static readonly string COMPANY_TEL = ConfigurationManager.AppSettings["COMPANY_TEL"];
        public static readonly string COMPANY_FAX = ConfigurationManager.AppSettings["COMPANY_FAX"];
        public static readonly string COMPANY_EMAIL = ConfigurationManager.AppSettings["COMPANY_EMAIL"];
        public static readonly string COMPANY_GSTNO = ConfigurationManager.AppSettings["COMPANY_GSTNO"];
        public static readonly string COMPANY_CRNO = ConfigurationManager.AppSettings["COMPANY_CRNO"];
        public static readonly string COMPANY_LOGO_PATH = ConfigurationManager.AppSettings["COMPANY_LOGO_PATH"];

        /// <summary>
        /// Static string value of the Document Footer
        /// </summary>
        public static readonly string DOCUMENT_FOOTER = ConfigurationManager.AppSettings["DOCUMENT_FOOTER"];
        #endregion

        #region GetCompanyLogo
        /// <summary>
        /// Static function to retrieve the Company Logo
        /// </summary>
        /// <returns>Byte array of the Image's Binary</returns>
        public static byte[] GetCompanyLogo()
        {
            byte[] buffer = null;

            if (!string.IsNullOrEmpty(GMSCoreBase.COMPANY_LOGO_PATH))
            {
                FileStream fs = null;
                try
                {
                    fs = File.Open(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.COMPANY_LOGO_PATH, FileMode.Open, FileAccess.Read, FileShare.Read);

                    if (fs != null)
                    {
                        buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, (int)fs.Length);
                    }
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }

            return buffer;
        }
        #endregion
    }


    #region Enum values
    /// <summary>
    /// Enum to define the available types of Sorting
    /// </summary>
    public enum SortType
    {
        None = 0,
        Asc = 1,
        Desc = 2
    }

    /// <summary>
    /// Enum to define the types of Security usage
    /// </summary>
    public enum SecurityUsageType
    {
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4
    }


    /// <summary>
    /// Enum to define the System Types
    /// </summary>
    public enum SystemType
    {
        Home = 1,
        HR = 2,
        Products = 3,
        Sales = 4,
        Reports = 5,
        Corporate = 6,
        Finance = 7,
        MIS = 8,
        Admin = 9,
        Organization = 10,
        Suppliers = 11,
        Debtors = 12,
        Communications = 13
    }


    /// <summary>
    /// Enum to define the Returned Result Types
    /// </summary>
    public enum ResultType
    {
        Ok = 0,
        Error = 1,
        NoResult = 2,
        Cannot = 3,
        ConcurrentError = 4,
        SecurityError = 5,
        SystemError = 6,
        DuplicatedData = 7,
        DateTimeContraint = 8,
        MainDataNotValid = 9,
        LogExisted = 10,
        Incomplete = 11,
        NullMainData = 12,
        NoTransactionId = 13,
        NullSupportingData = 14,
        InvalidSupportingDataCount = 15,
        CannotValidateTimeStamp = 16,
        InvalidKey = 17
    }



    /// <summary>
    /// Enum to define the List of Access Items (int value must equal to encore_AccessItem table!!)
    /// </summary>
    public enum AccessItemEMType
    {
        PrintReport = 1,
        GMSUserAccess = 2
    }

    /// <summary>
    /// Enum to define the List of Access Operations (Security access depends on the summation of this enum)
    /// (int value must equal to encore_AccessOperation table!!)
    /// 15 = Add,Edit,Delete,View
    /// 31 = Add,Edit,Delete,View,Print
    /// 415 = Add,Edit,Delete,View,Print,Confirm,Unconfirm
    /// </summary>
    public enum AccessOperationEMType
    {
        None = 0,
        Add = 1,
        Edit = 2,
        Delete = 4,
        View = 8,
        Print = 16
    }


    /// <summary>
    /// Enum to define the List of Company (int value must equal to tbCompany table!!)
    /// </summary>
    public enum CompanyIdType
    {
        AmericanDynamicsPteLtd = 1,
        BluePowerCorporationPteLtd = 2,
        BondflexPteLtd = 3
    }

    /// <summary>
    /// Enum to define the List of ModuleCategory (int value must equal to tbModuleCategory table!!)
    /// </summary>
    public enum ModuleCategoryId
    {
        Corporate = 1,
        Finance = 2,
        HR = 3,
        MIS = 4,
        Admin = 5,
        Organization = 6,
        Products = 7,
        Sales = 8,
        Report = 9
    }


    /// <summary>
    /// Enum to define the List of ModuleID (int value must equal to tbModule table - where ParentModuleID != null!!)
    /// </summary>
    public enum ModuleIdType
    {
        Country = 4,
        Division = 5,
        Company = 6,
        ItemCategory = 7,
        Item = 8,
        Currency = 9,
        Users = 11,
        ChangePassword = 12,
        EditAccessRights = 16,
        ViewForeignExchangeRate = 2,
        UploadBudget = 14,
        UploadAudit = 15,
        ViewReportCategory = 18,
        ModifyReportListing = 19,
        UploadReport = 20,
        ViewReports = 22,
        ItemStructure = 23,
        AddForeignExchangeRate = 24

    }

    
    #endregion


    
}
