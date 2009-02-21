using System;
using System.Runtime.Serialization;

namespace Doddle.Import.Configuration
{
    [Serializable]
    public class SpreadsheetConfigurationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public SpreadsheetConfigurationException()
        {
        }

        public SpreadsheetConfigurationException(string message) : base(message)
        {
        }

        public SpreadsheetConfigurationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SpreadsheetConfigurationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}