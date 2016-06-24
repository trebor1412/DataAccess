using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess.MSSQL;
using System.Data;
using System.Collections.Generic;

namespace DataAccess.Test
{
    [TestClass]
    public class TestMsSql
    {
        private string _ConnectionString = String.Format("User ID={0};Password={1};database={2};server={3};",
                                                 "account", "password", "database", "server");

        [TestMethod]
        public void TestConnect()
        {
            //arrange
            bool expect = true;
            bool actual = true;
            IDatabaseAccess target = new SqlClientDataAccess(this._ConnectionString);

            //act
            target.OpenConnection();
            target.CloseConnection();

            //assert
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TestQueryDataTableWithParameter()
        {
            //arrange
            bool expect = true;
            bool actual = true;
            IDatabaseAccess target = new SqlClientDataAccess(this._ConnectionString);
            DataTable result;
            DataAccessCommand command = new DataAccessCommand();

            //act
            command.SqlCommand =
@"SELECT *             
  FROM tableName
 WHERE columanName=@Value";
            command.AddParameter("Value", "value");
            result = target.QueryWithDataTable(command);
            actual = (result != null);

            //assert
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TestQueryDataReaderWithParameter()
        {
            //arrange
            bool expect = true;
            bool actual = true;
            IDatabaseAccess target = new SqlClientDataAccess(this._ConnectionString);
            IDataReader result;
            DataAccessCommand command = new DataAccessCommand();
            List<Department> employeeCollection;

            //act
            command.SqlCommand =
@"SELECT DOMAINNAME,          
       NAME,                
       FULLNAME             
  FROM v_VoyageRptDepartment
 WHERE DOMAINNAME=@DOMAIN";
            command.AddParameter("DOMAIN", "Evaair");
            try
            {
                result = target.QueryWithDataReader(command);
                employeeCollection = new List<Department>();
                while (result.Read())
                {
                    employeeCollection.Add(
                        new Department(
                            result.GetString(result.GetOrdinal("DOMAINNAME")),
                            result.GetString(result.GetOrdinal("FULLNAME")),
                            result.GetString(result.GetOrdinal("NAME")))
                    );
                }

                actual = (employeeCollection.Count > 0);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                target.CloseConnection();
            }

            //assert
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TestQueryDataTableException()
        {
            //arrange
            bool expect = true;
            bool actual = true;
            IDatabaseAccess target = new SqlClientDataAccess(this._ConnectionString);
            DataTable result;
            DataAccessCommand command = new DataAccessCommand();
            string errMessage = "";

            //act
            command.SqlCommand =
@"SELECT *             
  FROM tableName
 WHERE columanName=@Value";
            command.AddParameter("Value", "value");

            try
            {
                result = target.QueryWithDataTable(command);
            }
            catch (CommandExecutionExeception ex)
            {
                errMessage = ex.DetailErrorMessage;
                actual = (errMessage != "");
            }



            //assert
            Assert.AreEqual(expect, actual);
        }
    }

    public class Department {
        public string DomainName { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }

        public Department(string domainName,string fullName,string name) {
            this.DomainName = domainName;
            this.FullName = fullName;
            this.Name = name;
        }
    }
}
