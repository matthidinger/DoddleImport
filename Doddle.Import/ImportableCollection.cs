using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace Doddle.Importing
{
    /// <summary>
    /// Provides importing functionality to and from a generic collection of items
    /// </summary>
    public class ImportableCollection<T> : IImportSource, IImportDestination where T : class, new()
    {
        private readonly IList<T> _internalList;
        private readonly Type _itemType = typeof(T);

        public ImportableCollection(IList<T> backingList)
        {
            _internalList = backingList;
        }

        public bool FieldExists(string name)
        {
            try
            {
                PropertyInfo pi = _itemType.GetProperty(name);
                return true;
            }
            catch(ArgumentNullException)
            {
                return false;
            }
        }

        private ImportFieldCollection _fields;
        public ImportFieldCollection Fields
        {
            get
            {
                if (_fields == null)
                {
                    _fields = new ImportFieldCollection();
                    foreach (PropertyInfo pi in _itemType.GetProperties())
                    {
                        bool required = !IsNullableType(pi.PropertyType);
                        _fields.Add(pi.Name, pi.PropertyType, false);
                    }
                }

                return _fields;
            }
        }

        public IEnumerable<ImportRow> Rows
        {
            get
            {
                foreach(T item in _internalList)
                {
                    yield return new ImportRow(this, item);
                }
            }
        }

        public object GetFieldDataFromRow(object dataItem, string fieldName)
        {
            try
            {
                PropertyInfo pi = _itemType.GetProperty(fieldName);
                return pi.GetValue(dataItem, null);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool SupportsFieldCreation
        {
            get { return false; }
        }

        public void CreateField(string fieldName, Type dataType)
        {
            throw new NotSupportedException();
        }

        public void ImportRow(ImportRow row)
        {
            T item = new T();

            foreach (PropertyInfo pi in _itemType.GetProperties())
            {
                object value = row[pi.Name];
                if(value == null || value == DBNull.Value)
                    continue;

                Type propType = pi.PropertyType;
                object converted = Convert.ChangeType(value, propType);
                pi.SetValue(item, converted, null);
            }

            _internalList.Add(item);
        }

        private static bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }
    }
}
