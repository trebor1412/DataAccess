using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess
{
    public abstract class DatabaseAccessBase : IDatabaseAccess
    {
        protected IDbConnection _Connection;
        protected IDbCommand _Command;
        protected IDbDataAdapter _DataAdapter;
        protected IDataReader _DataReader;
        protected string _ConnectionString;

        public virtual string ConnectionString
        {
            get { return this._ConnectionString; }
        }

        protected virtual void AddParameters(IEnumerable<CommandParameter> parameterCollection) { }

        protected virtual void Initial() { }

        public void CloseConnection()
        {
            if (this._DataReader != null)
            {
                this._Command.Cancel();
                this._DataReader.Close();
            }
            if (this._Connection.State == ConnectionState.Open)
            {
                this._Connection.Close();
            }
        }

        public void OpenConnection()
        {
            if (this._Connection.State != ConnectionState.Open)
            {
                this._Connection.Open();
            }
        }

        public DataTable QueryWithDataTable(DataAccessCommand command)
        {
            DataSet queryResult = new DataSet();

            this.AddParameters(command.ParameterCollection);
            this._Command.CommandText = command.SqlCommand;
            this._Command.Connection = this._Connection;
            this._DataAdapter.SelectCommand = this._Command;
            try
            {
                this._DataAdapter.Fill(queryResult);
                return queryResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw new CommandExecutionExeception(ex.Message, command);
            }
        }

        public IDataReader QueryWithDataReader(DataAccessCommand command)
        {
            this.AddParameters(command.ParameterCollection);
            this._Command.CommandText = command.SqlCommand;
            this._Command.Connection = this._Connection;

            try
            {
                OpenConnection();
                this._DataReader = this._Command.ExecuteReader();
                return this._DataReader;
            }
            catch (Exception ex)
            {
                throw new CommandExecutionExeception(ex.Message, command);
            }
        }

        public int ExecuteCommand(DataAccessCommand command)
        {
            int excutedCount;
            try
            {
                OpenConnection();
                this._Command.Connection = this._Connection;
                this.AddParameters(command.ParameterCollection);
                this._Command.CommandText = command.SqlCommand;
                excutedCount = this._Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new CommandExecutionExeception(ex.Message, command);
            }
            finally
            {
                this._Connection.Close();
            }
            return excutedCount;
        }

        public int ExecuteTransaction(IEnumerable<DataAccessCommand> commandCollection)
        {
            int excutedCount = 0;
            IDbTransaction transaction;
            try
            {
                OpenConnection();
                transaction = this._Connection.BeginTransaction();
                this._Command.Connection = this._Connection;
                this._Command.Transaction = transaction;
                foreach (DataAccessCommand command in commandCollection)
                {
                    try
                    {
                        this.AddParameters(command.ParameterCollection);
                        this._Command.CommandText = command.SqlCommand;
                        excutedCount += this._Command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new CommandExecutionExeception(ex.Message, command);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this._Connection.Close();
            }
            return excutedCount;
        }
    }
}
