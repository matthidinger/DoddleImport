using System;
using System.Collections.Generic;
using System.Collections;

namespace Doddle.Import
{
    /// <summary>
    /// Provides importing functionality to and from a basic dictionary
    /// </summary>
    public class ImportableDictionary : IImportSource, IImportDestination
    {
        private readonly List<IDictionary> _internal; 
        public ImportableDictionary()
        {
            _internal = new List<IDictionary>();
            Fields = new ImportFieldCollection();
        }

        public bool FieldExists(string name)
        {
            return Fields.Contains(name);
        }

        public ImportFieldCollection Fields { get; set; }

        public bool SupportsFieldCreation
        {
            get { return true; }
        }

        public void CreateField(string fieldName, Type dataType)
        {
            Fields.Add(fieldName, dataType);
        }

        public void ImportRow(ImportRow row)
        {
            IDictionary newRow = AddRow();
            foreach (ImportField field in Fields)
            {
                newRow[field.Name] = row[field.Name];
            }
        }

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