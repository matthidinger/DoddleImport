using System;

namespace Doddle.Import
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