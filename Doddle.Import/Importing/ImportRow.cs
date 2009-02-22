using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doddle.Import
{
    public class ImportRow
    {
        private readonly Dictionary<string, object> _columnData = new Dictionary<string, object>();

        public ImportRow(IImportSource source)
        {
            ImportSource = source;
        }

        public int RowNumber { get; set; }
        public IImportSource ImportSource { get; set; }

        public object this[string columnName]
        {
            get
            {
                try
                {
                    if (_columnData[columnName] == DBNull.Value)
                        return null;

                    return _columnData[columnName];
                }
                catch
                {
                    throw new ArgumentOutOfRangeException("columnName", "The import source row does not have a column named " + columnName);
                }
            }
            set
            {
                _columnData[columnName] = value;
            }
        }

    }
}
