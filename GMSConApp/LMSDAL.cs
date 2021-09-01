using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace GMSConApp
{
    class LMSDAL
    {
        public string strcon;
        public SqlConnection cn;
        public SqlCommand cm;
        public DataTable dt;
        public string ex;

        #region SQLConnection
        public LMSDAL(string conn)
        {
            strcon = conn;
        }
        public void CNCMGenerator()
        {
            cn = new SqlConnection(strcon);
            cm = new SqlCommand();
        }
        public void SPexecutor(string strSPName)
        {
            //Store Procedual executor
            cm.CommandTimeout = 90;
            cm.CommandText = strSPName;
            cm.CommandType = CommandType.StoredProcedure;
            cn.Open();
            cm.Connection = cn;
            try
            {
                this.ex = string.Empty;
                cm.ExecuteScalar();
            }
            catch (Exception e)
            {
                this.ex = e.Message;
            }
            finally
            {
                cn.Dispose();
                cm.Dispose();
            }
        }
        public void SPexecutor_dt(string strSPName)
        {
            dt = new DataTable();
            cm.CommandTimeout = 90;
            cm.CommandText = strSPName;
            cm.CommandType = CommandType.StoredProcedure;
            cn.Open();
            cm.Connection = cn;
            try
            {
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    dt.Load(dr);
                    dr.Dispose();
                }
            }
            catch (Exception e)
            {
                this.ex = e.Message;
            }
            finally
            {
                cn.Dispose();
                cm.Dispose();
            }
        }
        #endregion

        public DataTable LMS_Get_CompanyByCode(string Code)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyCode", SqlDbType.NVarChar).Value = Code;
            SPexecutor_dt("procWSCompanySelect");
            return dt;
        }

        public void LMS_Update_ClosePRStatus(string Code, string prno)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyCode", SqlDbType.NVarChar).Value = Code;
            cm.Parameters.Add("@PRNo", SqlDbType.NVarChar).Value = prno;
            SPexecutor("procWSClosePRStatus");
        }

        public void LMS_Update_SAPAddress(string Code,string accountCode,string addresstype, 
            string address1,string address2,string address3,string address4,string postalCode,
            string officePhone, string mobilePhone,string fax,string email,string contactPerson,
            string addressName)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyCode", SqlDbType.NVarChar).Value = Code;
            cm.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            cm.Parameters.Add("@AddressType", SqlDbType.NVarChar).Value = addresstype;
            cm.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = address1;
            cm.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = address2;
            cm.Parameters.Add("@Address3", SqlDbType.NVarChar).Value = address3;
            cm.Parameters.Add("@Address4", SqlDbType.NVarChar).Value = address4;
            cm.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = postalCode;
            cm.Parameters.Add("@OfficePhone", SqlDbType.NVarChar).Value = officePhone;
            cm.Parameters.Add("@MobilePhone", SqlDbType.NVarChar).Value = mobilePhone;
            cm.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = fax;
            cm.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
            cm.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = contactPerson;
            cm.Parameters.Add("@AddressName", SqlDbType.NVarChar).Value = addressName;
            SPexecutor("procWSSAPAddressUpdate");
        }

        public void LMS_Update_SAPProductGroup1(string Code, string productCode, string productName)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyCode", SqlDbType.NVarChar).Value = Code;
            cm.Parameters.Add("@Code", SqlDbType.NVarChar).Value = productCode;
            cm.Parameters.Add("@Name", SqlDbType.NVarChar).Value = productName;
            SPexecutor("procWSSAPProductGroup1Update");
        }
    }
}
