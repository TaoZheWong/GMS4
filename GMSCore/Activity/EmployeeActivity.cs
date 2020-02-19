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
    public class EmployeeActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public EmployeeActivity()
        {
        }
        #endregion

        #region RetrieveEmployeeByEmployeeID
        public Employee RetrieveEmployeeByEmployeeID(short ID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Employee.EmployeeID"),
                                helper.CleanValue(ID));
           
            return Employee.RetrieveFirst(stb.ToString());
        }

        public Employee RetrieveEmployeeByCoyIDEmployeeID(short coyid, short ID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Employee.EmployeeID"),
                                helper.CleanValue(ID));
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Employee.CoyID"),
                               helper.CleanValue(coyid));

            return Employee.RetrieveFirst(stb.ToString());
        }

        public Employee RetrieveEmployeeByEmployeeName(string name)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Employee.Name"),
                                helper.CleanValue(name));
            stb.AppendFormat(" AND ({0} is null OR {0} <> {1}) ", helper.GetFieldName("Employee.IsInactive"),
                                helper.CleanValue(true));

            return Employee.RetrieveFirst(stb.ToString());
        }

        public Employee RetrieveEmployeeByEmployeeEmail(string email)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Employee.EmailAddress"),
                                helper.CleanValue(email));

            return Employee.RetrieveFirst(stb.ToString());
        }

        public Employee RetrieveUnitHeadByDepartment(string department)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Employee.Department"),
                                helper.CleanValue(department));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Employee.IsUnitHead"),
                                helper.CleanValue(true));

            return Employee.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region UpdateEmployee
        /// <summary>
        /// Function to update Employee
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a Employee object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateEmployee(ref Employee employeeToUpdate, LogSession session)
        {
            if (employeeToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!employeeToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            employeeToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateEmployee
        /// <summary>
        /// Function to create a new Employee
        /// </summary>
        /// <param name="organizerToCreate">Reference to a Employee object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateEmployee(ref Employee employeeToCreate, LogSession session)
        {
            if (employeeToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!employeeToCreate.IsValid())
                return ResultType.MainDataNotValid;

            employeeToCreate.Save();

            return ResultType.Ok;

            //#region Transaction
            //TransactionManager trans = null;
            //try
            //{
            //    trans = new TransactionManager();

            //    trans.Save(parameterToCreate);

            //    trans.Commit();
            //    parameterToCreate.Resync();
            //    return ResultType.Ok;
            //}
            //catch (Exception)
            //{
            //    if (trans != null)
            //        trans.Rollback();
            //    throw;
            //}
            //finally
            //{
            //    if (trans != null)
            //        trans.Dispose();
            //}
            //#endregion //Transaction

        }
        #endregion

        #region DeleteEmployee
        public ResultType DeleteEmployee(short ID, LogSession session)
        {
            //if (ID == null)
            //    return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            Employee employee = RetrieveEmployeeByEmployeeID(ID);
            if (employee == null)
                return ResultType.Error;

            employee.Delete();
            return ResultType.Ok;
        }
        #endregion

        public Employee RetrieveEmployeeByEmployeeNo(string name,string coyid)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Employee.EmployeeNo"),
                                helper.CleanValue(name));
            stb.AppendFormat(" AND ({0} is null OR {0} <> {1}) ", helper.GetFieldName("Employee.IsInactive"),
                                helper.CleanValue(true));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Employee.CoyID"),
                                helper.CleanValue(coyid));

            return Employee.RetrieveFirst(stb.ToString());
        }
    }
}
