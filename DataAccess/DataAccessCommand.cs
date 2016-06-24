using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    /// <summary>
    /// SQL指令類別
    /// </summary>
    public class DataAccessCommand
    {
        public string SqlCommand { get; set; }
        public List<CommandParameter> ParameterCollection { get; set; }

        public DataAccessCommand()
        {
            this.ParameterCollection = new List<CommandParameter> { };
        }

        public void AddParameter(string parameterName, object parameterValue)
        {
            this.ParameterCollection.Add(new CommandParameter(parameterName, parameterValue));
        }

    }
}
