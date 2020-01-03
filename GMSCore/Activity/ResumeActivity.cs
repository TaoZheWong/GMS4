using System;
using System.Collections.Generic;
using System.Text;

using GMSCore.Entity;
using GMSCore.Exceptions;
using Wilson.ORMapper;

namespace GMSCore.Activity
{
    public class ResumeActivity : ActivityBase
    {
        #region Constructor
        public ResumeActivity()
        {
        }
        #endregion

        public ResultType CreateResumeUpload(ref ResumeUpload upload, LogSession session)
        {
            if (upload == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!upload.IsValid())
                return ResultType.MainDataNotValid;
            try
            {
                upload.Save();
                return ResultType.Ok;
            }
            catch
            {
                return ResultType.DuplicatedData;
            }
        }

        public ResumeUpload RetrieveResumeUploadByID(string fileID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat("{0}={1}",helper.GetFieldName("ResumeUpload.FileID"),
                                        helper.CleanValue(fileID));

            return ResumeUpload.RetrieveFirst(stb.ToString());
        }

        public ResultType DeleteResumeUpload(string ID, LogSession session)
        {
            if (ID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            ResumeUpload upload = RetrieveResumeUploadByID(ID);
            if (upload == null)
                return ResultType.Error;

            upload.Delete();
            return ResultType.Ok;
        }
    }
}
