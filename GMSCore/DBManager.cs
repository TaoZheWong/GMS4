using System;
using System.Collections.Generic;
using System.Text;

using Wilson.ORMapper;

namespace GMSCore
{
    /// <summary>
    /// DBManager class.
    /// 
    /// Adopts a singleton design. Use GetInstance() to obtain the one and only instance.
    /// </summary>
    public class DBManager
    {
        private static DBManager _manager = new DBManager();
        private ObjectSpace _engine = null;
        private string _databaseName, _serverName, _userLoginName, _userLoginPwd;
        private string _archiveDatabaseName, _archiveServerName, _archiveUserLoginName, _archiveUserLoginPwd;

        /// <summary>
        /// Gets the DBManager's active instance
        /// </summary>
        /// <returns></returns>
        public static DBManager GetInstance()
        {
            return _manager;
        }

        /// <summary>
        /// Access the ObjectSpace object of the ORMapper
        /// </summary>
        public ObjectSpace Engine
        {
            get
            {
                return _engine;
            }
        }

        /// <summary>
        /// String value of the Database Name
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return _databaseName;
            }
        }

        /// <summary>
        /// String value of the Server Name
        /// </summary>
        public string ServerName
        {
            get
            {
                return _serverName;
            }
        }

        /// <summary>
        /// String value of the User login Name
        /// </summary>
        public string UserLoginName
        {
            get
            {
                return _userLoginName;
            }
        }

        /// <summary>
        /// String value of the User login Pwd
        /// </summary>
        public string UserLoginPwd
        {
            get
            {
                return _userLoginPwd;
            }
        }

        /// <summary>
        /// String value of the Database Name
        /// </summary>
        public string ArchiveDatabaseName
        {
            get
            {
                return _archiveDatabaseName;
            }
        }

        /// <summary>
        /// String value of the Server Name
        /// </summary>
        public string ArchiveServerName
        {
            get
            {
                return _archiveServerName;
            }
        }

        /// <summary>
        /// String value of the User login Name
        /// </summary>
        public string ArchiveUserLoginName
        {
            get
            {
                return _archiveUserLoginName;
            }
        }

        /// <summary>
        /// String value of the User login Pwd
        /// </summary>
        public string ArchiveUserLoginPwd
        {
            get
            {
                return _archiveUserLoginPwd;
            }
        }


        /// <summary>
        /// Private constructor
        /// </summary>
        private DBManager()
        {
            string mappingFile = AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.MAPPING_FILE;
            string connectString = GMSCoreBase.DB_CONNECTION_STR;
            string providerType = "MsSql";
            _databaseName = GMSCoreBase.DB_NAME;
            _serverName = GMSCoreBase.SERVER_NAME;
            _userLoginName = GMSCoreBase.USER_LOGIN_NAME;
            _userLoginPwd = GMSCoreBase.USER_LOGIN_PWD;
            _archiveDatabaseName = GMSCoreBase.ARCHIVE_DB_NAME;
            _archiveServerName = GMSCoreBase.ARCHIVE_SERVER_NAME;
            _archiveUserLoginName = GMSCoreBase.ARCHIVE_USER_LOGIN_NAME;
            _archiveUserLoginPwd = GMSCoreBase.ARCHIVE_USER_LOGIN_PWD;

            Provider provider;
            try
            {
                provider = (Provider)System.Enum.Parse(typeof(Provider), providerType, true);
            }
            catch
            {
                provider = Provider.MsSql;
            }

            // Note: Non-Zero Session is needed for Server Applications like Web
            _engine = new ObjectSpace(mappingFile, connectString, provider, 20, 5);
            //engine = new ObjectSpace(mappingFile, connectString, provider);			

        }
    }
}
