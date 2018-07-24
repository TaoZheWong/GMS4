using System;
using System.Collections.Generic;
using System.Text;
using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSCore.Exceptions;
using Wilson.ORMapper;

namespace GMSCore.Activity
{
    public class DocumentActivity : ActivityBase 
    {
        #region constructor
        public DocumentActivity()
        {
        }
        #endregion

        #region RetrieveDocumentByDocumentID
        public Document RetrieveDocumentByDocumentID(short DocumentID) 
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Document.DocumentID"),
                                helper.CleanValue(DocumentID));

            return Document.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveDocumentByDocumentNameFileName
        public Document RetrieveDocumentByDocumentNameFileName(string documentName, string fileName)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Document.DocumentName"),
                                helper.CleanValue(documentName));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Document.FileName"),
                                helper.CleanValue(fileName));
   
            return Document.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveDocumentByEmployeeID
        public DocumentForEmployee RetrieveDocumentByEmployeeID(short employeeId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("DocumentForEmployee.EmployeeID"),
                                helper.CleanValue(employeeId));
            
            return DocumentForEmployee.RetrieveFirst(stb.ToString(), string.Format(" {0} DESC ",
                                                    helper.GetFieldName("DocumentForEmployee.FileName")));
        }
        #endregion
    }
}
