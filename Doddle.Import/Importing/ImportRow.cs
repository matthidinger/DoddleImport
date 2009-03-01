using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doddle.Import
{
    public class ImportRow
    {
        private readonly Dictionary<string, object> _fieldData = new Dictionary<string, object>();
        private readonly object _dataItem;

        public ImportRow(IImportSource source, object dataItem)
        {
            ImportSource = source;
            _dataItem = dataItem;
        }

        public int RowNumber { get; set; }
        public IImportSource ImportSource { get; private set; }

        public object this[string fieldName]
        {
            get
            {
                if (_fieldData.ContainsKey(fieldName))
                    return _fieldData[fieldName];

                return ImportSource.GetFieldDataFromRow(_dataItem, fieldName);
            }
            set { _fieldData[fieldName] = value; }
        }

    }
}
