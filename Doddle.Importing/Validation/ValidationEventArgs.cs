using System;

namespace Doddle.Importing
{
    public class ValidationEventArgs : EventArgs
    {
        public ValidationEventArgs(ImportValidationResult result)
        {
            Result = result;
        }

        public ImportValidationResult Result { get; private set; }
    }
}