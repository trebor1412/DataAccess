using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;

namespace DataAccess.Oracle
{
    public class OracleClientDataAccess : DatabaseAccessBase
    {
        public OracleClientDataAccess(string connectionString)
        {
            this._ConnectionString = connectionString;
            Initial();
        }

        protected override void Initial()
        {
            this._Connection = new OracleConnection(this._ConnectionString);
            this._Command = new OracleCommand();
            this._DataAdapter = new OracleDataAdapter();
        }

        protected override void AddParameters(IEnumerable<CommandParameter> parameterCollection)
        {
            this._Command.Parameters.Clear();
            foreach (CommandParameter parameter in parameterCollection)
            {
                this._Command.Parameters.Add(new OracleParameter(parameter.Name, parameter.Value));
            }

        }
    }
}
