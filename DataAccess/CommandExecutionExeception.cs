using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CommandExecutionExeception : Exception
    {
        private string _FormattedErrormessage;
        private const string TextBlockSeparator = "---------------------------------------";
        private const string CRLF = "\r\n";
        private const int ParameterTextLength = 20;

        public string DetailErrorMessage
        {
            get { return this._FormattedErrormessage; }
        }

        public CommandExecutionExeception() { }

        public CommandExecutionExeception(string exMessage, DataAccessCommand dbo)
            : base(exMessage)
        {
            this._FormattedErrormessage =
                string.Format(
                "Exception:" + CRLF +
                TextBlockSeparator + CRLF +
                exMessage +
                TextBlockSeparator + CRLF + CRLF +
                "Command:" + CRLF +
                TextBlockSeparator + CRLF +
                dbo.SqlCommand + CRLF +
                TextBlockSeparator + CRLF + CRLF +
                "Parameters" + CRLF + TextBlockSeparator + CRLF +
                string.Join(CRLF, dbo.ParameterCollection.
                Select(x => ("Name: " + x.Name).PadRight(ParameterTextLength, ' ') + 
                    " , Value: " + x.Value).ToArray()) + CRLF +
                TextBlockSeparator);


        }
    }
}
