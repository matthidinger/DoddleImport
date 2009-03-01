using System;

namespace Doddle.Import
{
    public class ImportEventArgs : EventArgs
    {
        public ImportResult Result { get; private set; }
        public ImportEventArgs(ImportResult result)
        {
            Result = result;
        }
    }
}