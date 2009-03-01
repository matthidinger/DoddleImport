using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Doddle.Import
{
    public interface IImportSource
    {
        ImportFieldCollection Fields { get; }
        IEnumerable<ImportRow> Rows { get; }
        object GetFieldDataFromRow(object dataItem, string fieldName);
    }
}
