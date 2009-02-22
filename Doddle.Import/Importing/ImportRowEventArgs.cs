using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doddle.Import
{
    public class ImportRowEventArgs : EventArgs
    {
        public ImportRowEventArgs(ImportRow row)
        {
            Row = row;
        }
        public ImportRow Row { get; set; }
    }
}
