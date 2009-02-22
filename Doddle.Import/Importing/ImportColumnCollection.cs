using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Doddle.Import
{
    public class ImportColumnCollection : IEnumerable<ImportColumn>
    {
        private readonly Dictionary<string, ImportColumn> _internalDictionary = new Dictionary<string, ImportColumn>();
        private IImportSource _source;

        internal ImportColumnCollection(IImportSource source)
        {
            _source = source;
        }

        public void Add(string columnName)
        {
            Add(columnName, null, false);
        }

        public void Add(string columnName, Type dataType)
        {
            Add(columnName, dataType, false);
        }

        public void Add(string columnName, Type dataType, bool isRequired)
        {
            ImportColumn col = new ImportColumn(columnName, dataType, isRequired);
//            col.
            Add(col);
        }

        public void Add(ImportColumn column)
        {
            _internalDictionary.Add(column.Name, column);
        }

        public ImportColumn this[string name]
        {
            get
            {
                return _internalDictionary[name];
            }
        }

        public bool Contains(string name)
        {
            return _internalDictionary.ContainsKey(name);
        }


        public IEnumerator<ImportColumn> GetEnumerator()
        {
            return _internalDictionary.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _internalDictionary.Values.GetEnumerator();
        }
    }
}