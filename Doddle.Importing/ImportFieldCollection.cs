using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Doddle.Importing
{
    public class ImportFieldCollection : IEnumerable<ImportField>
    {
        private readonly Dictionary<string, ImportField> _internalDictionary = new Dictionary<string, ImportField>();

        public void Add(string fieldName)
        {
            Add(fieldName, null, false);
        }

        public void Add(string fieldName, Type dataType)
        {
            Add(fieldName, dataType, false);
        }

        public void Add(string fieldName, Type dataType, bool isRequired)
        {
            ImportField col = new ImportField(fieldName, dataType, isRequired);
            Add(col);
        }

        public void Add(ImportField field)
        {
            _internalDictionary.Add(field.Name, field);
        }

        public ImportField this[string name]
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


        public IEnumerator<ImportField> GetEnumerator()
        {
            return _internalDictionary.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _internalDictionary.Values.GetEnumerator();
        }
    }
}