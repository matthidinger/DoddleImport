using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doddle.Import
{
    public interface IImportSource
    {
        ImportColumnCollection Columns { get; }
        IEnumerable<ImportRow> Rows { get; }
    }
}
