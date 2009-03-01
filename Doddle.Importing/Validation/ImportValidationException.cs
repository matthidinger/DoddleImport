using System;
using System.Runtime.Serialization;

namespace Doddle.Importing
{
    [Serializable]
    public class ImportValidationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ImportValidationException()
        {
        }

        public ImportValidationException(string message) : base(message)
        {
        }

        public ImportValidationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ImportValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}