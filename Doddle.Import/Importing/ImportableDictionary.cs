using System.Collections.Generic;
using System.Collections;

namespace Doddle.Import
{
    public class ImportableDictionary:IImportSource
    {
        private readonly List<IDictionary> _internal; 
        public ImportableDictionary()
        {
            _internal = new List<IDictionary>();
            Fields = new ImportFieldCollection();
        }

        public ImportFieldCollection Fields { get; set; }

        public IEnumerable<ImportRow> Rows
        {
            get
            {
                foreach (IDictionary dictionary in _internal)
                {
                    yield return new ImportRow(this, dictionary);
                }
            }
        }

        public object GetFieldDataFromRow(object dataItem, string fieldName)
        {
            IDictionary rowData = (IDictionary) dataItem;
            return rowData[fieldName];
        }

        public IDictionary AddRow()
        {
            IDictionary row = new Hashtable();
            _internal.Add(row);
            return row;
        }
    }
}