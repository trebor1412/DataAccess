using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess;
using DataAccess.Oracle;
using System.Data;
using System.Collections.Generic;

namespace DataAccess.Test
{
    [TestClass]
    public class TestOracle
    {        
        private string _ConnectionString = String.Format("User Id={0};Password={1};Data Source={2};",
            "account","password","data source");

        [TestMethod]
        public void TestConnect()
        {
            //arrange
            bool expect = true;
            bool actual = true;
            IDatabaseAccess target = new OracleClientDataAccess(this._ConnectionString);

            //act
            target.OpenConnection();
            target.CloseConnection();

            //assert
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TestQueryDataTable()
        {
            //arrange
            bool expect = true;
            bool actual = true;
            IDatabaseAccess target = new OracleClientDataAccess(this._ConnectionString);
            DataTable result;
            DataAccessCommand command = new DataAccessCommand();

            //act
            command.SqlCommand = "SELECT SYSDATE FROM DUAL";
            result = target.QueryWithDataTable(command);
            actual = (result != null);

            //assert
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TestQueryDataTableWithParameter()
        {
            //arrange
            bool expect = true;
            bool actual = true;
            IDatabaseAccess target = new OracleClientDataAccess(this._ConnectionString);
            DataTable result;
            DataAccessCommand command = new DataAccessCommand();

            //act
            command.SqlCommand = @"
SELECT *  
  FROM TABLE 
 WHERE ID=:ROOM_ID";
            command.AddParameter("ROOM_ID", 150376);

            result = target.QueryWithDataTable(command);
            actual = (result != null);

            //assert
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TestQueryDataTableException()
        {
            //arrange
            bool expect = true;
            bool actual = true;
            IDatabaseAccess target = new OracleClientDataAccess(this._ConnectionString);
            DataTable result;
            DataAccessCommand command = new DataAccessCommand();
            string errMessage = "";

            //act
            command.SqlCommand =
@"SELECT *  
  FROM TABLE 
 WHERE COLUMNNAME=:ROOM_ID";
            command.AddParameter("ggdg", "fdsfsd");
            command.AddParameter("efe", "434");
            command.AddParameter("fe23", "fdr3sfsd");

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

        [TestMethod]
        public void TestQueryDataReaderWithParameter()
        {
            //arrange
            bool expect = true;
            bool actual = true;
            IDatabaseAccess target = new OracleClientDataAccess(this._ConnectionString);
            IDataReader result;
            DataAccessCommand command = new DataAccessCommand();
            List<Chatroom> roomCollection;

            //act
            command.SqlCommand = @"
SELECT *  
  FROM TABLE 
 WHERE COLUMNNAME=:ID";
            command.AddParameter("ID", 150376);
            try
            {
                result = target.QueryWithDataReader(command);
                roomCollection = new List<Chatroom>();
                while (result.Read())
                {
                    roomCollection.Add(
                        new Chatroom(
                            result.GetInt32(result.GetOrdinal("ID")),
                            result.GetDateTime(result.GetOrdinal("ACTDATE")),
                            result.GetString(result.GetOrdinal("FLICODE")),
                            result.GetString(result.GetOrdinal("DPORT")))
                    );
                }

                actual = (roomCollection.Count > 0);
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
        public void TestQueryDatareaderException()
        {
            bool expect = true;
            bool actual = true;
            IDatabaseAccess target = new OracleClientDataAccess(this._ConnectionString);
            IDataReader result;
            DataAccessCommand command = new DataAccessCommand();
            List<Chatroom> roomCollection;
            string errMessage = "";

            //act
            command.SqlCommand = @"
SELECT *  
  FROM TABLE 
 WHERE ID=:ROOM_ID";
            command.AddParameter("ROOM_ID", 150376);
            try
            {
                result = target.QueryWithDataReader(command);
                roomCollection = new List<Chatroom>();
                while (result.Read())
                {
                    roomCollection.Add(
                        new Chatroom(
                            result.GetInt32(result.GetOrdinal("ROOM_ID")),
                            result.GetDateTime(result.GetOrdinal("ACTDATE")),
                            result.GetString(result.GetOrdinal("FLICODE")),
                            result.GetString(result.GetOrdinal("DPORT")))
                    );
                }
            }
            catch (CommandExecutionExeception ex)
            {
                errMessage = ex.DetailErrorMessage;
                actual = (errMessage != "");
            }
            finally
            {
                target.CloseConnection();
            }

            //assert
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TestExecuteCommand()
        {
            //arrange
            bool expect = true;
            bool actual = false;
            IDatabaseAccess target = new OracleClientDataAccess(this._ConnectionString);
            DataAccessCommand command = new DataAccessCommand();
            int dataExecutedCount = 0;

            //act
            command.SqlCommand =
@"UPDATE TABLE
     SET TIME=SYSDATE
   WHERE ID BETWEEN :BEGIN_ID AND :END_ID";
            command.AddParameter("BEGIN_ID", 1668);
            command.AddParameter("END_ID", 1670);

            try
            {
                dataExecutedCount = target.ExecuteCommand(command);
                actual = (dataExecutedCount > 0);
            }
            catch (Exception)
            {
                throw;
            }



            //assert
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TestExecuteCommandException()
        {
            //arrange
            bool expect = true;
            bool actual = false;
            IDatabaseAccess target = new OracleClientDataAccess(this._ConnectionString);
            DataAccessCommand command = new DataAccessCommand();
            string errMessage = "";

            //act
            command.SqlCommand =
@"UPDATE TABLE
     SET TIME=SYSDATE
   WHERE ID BETWEEN :BEGIN_ID AND :END_ID";
            command.AddParameter("BEGIN_ID", 1668);


            try
            {
                target.ExecuteCommand(command);

            }
            catch (CommandExecutionExeception ex)
            {
                errMessage = ex.DetailErrorMessage;
                actual = (errMessage != "");
            }



            //assert
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TestExecuteTransaction()
        {
            //arrange
            bool expect = true;
            bool actual = false;
            IDatabaseAccess target = new OracleClientDataAccess(this._ConnectionString);
            List<DataAccessCommand> commandCollection = new List<DataAccessCommand>();
            DataAccessCommand command;
            int dataExecutedCount = 0;

            //act
            command = new DataAccessCommand();
            command.SqlCommand =
@"UPDATE TABLE
     SET TIME=SYSDATE
   WHERE ID BETWEEN :BEGIN_ID AND :END_ID";
            command.AddParameter("BEGIN_ID", 1668);
            command.AddParameter("END_ID", 1670);

            commandCollection.Add(command);

            command = new DataAccessCommand();
            command.SqlCommand =
@"UPDATE TABLE
     SET TIME=SYSDATE
   WHERE ID BETWEEN :BEGIN_ID AND :END_ID";
            command.AddParameter("BEGIN_ID", 1660);
            command.AddParameter("END_ID", 1665);

            commandCollection.Add(command);

            try
            {
                dataExecutedCount = target.ExecuteTransaction(commandCollection);
                actual = (dataExecutedCount > 0);
            }
            catch (Exception)
            {
                throw;
            }



            //assert
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TestExecuteTransactionException()
        {
            //arrange
            bool expect = true;
            bool actual = false;
            IDatabaseAccess target = new OracleClientDataAccess(this._ConnectionString);
            List<DataAccessCommand> commandCollection = new List<DataAccessCommand>();
            DataAccessCommand command;
            int dataExecutedCount = 0;
            string errMessage = "";

            //act
            command = new DataAccessCommand();
            command.SqlCommand =
@"UPDATE TABLENAME
     SET TIME=SYSDATE
   WHERE ID TWEEN :BEGIN_ID AND :END_ID";
            command.AddParameter("BEGIN_ID", 1668);
            command.AddParameter("END_ID", 1670);

            commandCollection.Add(command);

            command = new DataAccessCommand();
            command.SqlCommand =
@"UPDATE TABLENAME
     SET TIME=SYSDATE
   WHERE ID TWEEN :BEGIN_ID AND :END_ID";
            command.AddParameter("BEGIN_ID", 1660);
            command.AddParameter("END_ID", 1665);

            commandCollection.Add(command);

            try
            {
                dataExecutedCount = target.ExecuteTransaction(commandCollection);
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

    public class Chatroom
    {
        public int RoomID { get; set; }
        public DateTime Actdate { get; set; }
        public string Flicode { get; set; }
        public string Dport { get; set; }


        public Chatroom(int roomID, DateTime actdate, string flicode, string dport)
        {
            this.RoomID = roomID;
            this.Actdate = actdate;
            this.Flicode = flicode;
            this.Dport = dport;
        }
    }

}
