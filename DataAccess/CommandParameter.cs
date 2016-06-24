using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class CommandParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public CommandParameter() { }

        public CommandParameter(string parameterName, object parameterValue)
        {
            this.Name = parameterName;
            this.Value = parameterValue;
        }
    }
}
