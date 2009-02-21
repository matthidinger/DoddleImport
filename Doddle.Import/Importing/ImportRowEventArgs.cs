﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doddle.Import.Importing
{
    public class ImportRowEventArgs : EventArgs
    {
        public ImportRowEventArgs(SpreadsheetRow row)
        {
            Row = row;
        }
        public SpreadsheetRow Row { get; set; }
    }
}