using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace DataAccess.MSSQL
{
    public class SqlClientDataAccess : DatabaseAccessBase
    {
        public SqlClientDataAccess(string connectionString)
        {
            this._ConnectionString = connectionString;
            Initial();
        }

        protected override void Initial()
        {
            this._Connection = new SqlConnection(this._ConnectionString);
            this._Command = new SqlCommand();
            this._DataAdapter = new SqlDataAdapter();
        }

        protected override void AddParameters(IEnumerable<CommandParameter> parameterCollection)
        {
            this._Command.Parameters.Clear();
            foreach (CommandParameter parameter in parameterCollection)
            {
                this._Command.Parameters.Add(new SqlParameter(parameter.Name, parameter.Value));
            }

        }
    }
}
