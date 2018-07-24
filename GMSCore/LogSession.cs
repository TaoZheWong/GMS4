using System;
using System.Collections.Generic;
using System.Text;

namespace GMSCore
{
    /// <summary>
    /// LogSession class
    /// </summary>
    [Serializable]
    public class LogSession
    {
        /// <summary>
        /// Default value of the default company id.
        /// </summary>
        public const int DEFAULT_COMPANYID = 1;

        /// <summary>
        /// Default value of the default Division id
        /// </summary>
        public const int DEFAULT_DIVISIONID = 1;

        /// <summary>
        /// Default value of the default Department id
        /// </summary>
        public const int DEFAULT_DEPARTMENTID = 1;

        /// <summary>
        /// The User's ID
        /// </summary>
        public short UserId = 0;
        /// <summary>
        /// The selected Login Company.
        /// </summary>
        public short CompanyId = 0;

        /// <summary>
        /// The selected Division
        /// </summary>
        public short DivisionId = 0;

        /// <summary>
        /// The selected Country
        /// </summary>
        public short CountryId = 0;

        /// <summary>
        /// The Company's Web Service Address
        /// </summary>
        public string WebServiceAddress = "0.0.0.0";

        /// <summary>
        /// The User's UserName
        /// </summary>
        public string UserName = "";

        /// <summary>
        /// The User's Real Name
        /// </summary>
        public string UserRealName = "";

        /// <summary>
        /// The User's IP Address
        /// </summary>
        public string IPAddress = "0.0.0.0";

        /// <summary>
        /// The User's Last Login Date
        /// </summary>
        public string LastLoginDate = "";

        public bool ToNewsPage = false;

        public string DefaultCurrency = "";

        public string CMSWebServiceAddress = "0.0.0.0";

        public string TBType = "N";

        public short FYE = 12;

        public string DBName = "";

        public string StatusType = "";

        public string LMSParallelRunEndDate = "";

        public bool IsOffline = false;

        public string SAPStartDate = "";      

        public string GASLMSWebServiceAddress = "";

        public string WSDLMSWebServiceAddress = "";       

        public string MRScheme = "";

        public string DefaultWarehouse = "";

        public string DimensionL1 = "";

        public string TableSuffix = "";

        public string SAPURI = "";

        public string SAPKEY = "";

        public string SAPDB = "";

        public List<short> UserAccessModuleCategory = new List<short>();
        
        public IDictionary<AccessItemEMType, AccessOperationEMType> dictAccessItem_AccessOperationEMSum;

        /// <summary>
        /// Get a AccessOperationEMType sum to determine if the Operation is available for this Access Item.
        /// </summary>
        /// <param name="accessItemType">AccessItemEMType enum defining the AccessItem</param>
        /// <returns>AccessOperationEMType enum sum containing the access rights. 0 if not available.</returns>
        public AccessOperationEMType GetAccessOperationEMSum(AccessItemEMType accessItemType)
        {
            if (this.dictAccessItem_AccessOperationEMSum.ContainsKey(accessItemType))
            {
                return this.dictAccessItem_AccessOperationEMSum[accessItemType];
            }
            return AccessOperationEMType.None;
        }

        //SAMPLE USER ROLE CHECKING

        //if (session == null || session.dictAccessItem_AccessOperationEMSum == null)
        //    throw new NullSessionException();

        //if ((session.GetAccessOperationEMSum(AccessItemEMType.InventoryItemBatch) & AccessOperationEMType.View) == 0)
        //    throw new SecurityException("view Item Batch.");
    }
}
