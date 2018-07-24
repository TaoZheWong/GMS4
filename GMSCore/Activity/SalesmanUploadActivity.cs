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
    
    public class SalesmanUploadActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public SalesmanUploadActivity()
        {
        }
        #endregion


        public ResultType CreateSalesmanUpload(ref SalesmanUpload upload, LogSession session)
        {
            if (upload == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!upload.IsValid())
                return ResultType.MainDataNotValid;

            upload.Save();

            return ResultType.Ok;

        }

        #region RetrieveSalesmanUploadByID
        public SalesmanUpload RetrieveSalesmanUploadByID(string SalesmanUploadID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesmanUpload.DocumentID"),
                               helper.CleanValue(SalesmanUploadID));
            

            return SalesmanUpload.RetrieveFirst(stb.ToString());
        }
        #endregion

       
        public ResultType DeleteSalesmanUpload(string ID, LogSession session)
        {
            if (ID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            SalesmanUpload upload = RetrieveSalesmanUploadByID(ID);
            if (upload == null)
                return ResultType.Error;

            upload.Delete();
            return ResultType.Ok;
        }
       

    }
}
