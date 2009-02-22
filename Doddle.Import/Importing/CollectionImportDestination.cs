using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Doddle.Import
{
    public class CollectionImportDestination<T> : IImportDestination where T : class, new()
    {
        private IList<T> _internalList;
        Type tType = typeof(T);

        public CollectionImportDestination(IList<T> backingList)
        {
            _internalList = backingList;
        }

        public bool FieldExists(string name)
        {
            try
            {
                PropertyInfo pi = tType.GetProperty(name);
                return true;
            }
            catch(ArgumentNullException)
            {
                return false;
            }
        }

        public ImportColumnCollection Fields
        {
            get 
            {
                ImportColumnCollection columns = new ImportColumnCollection();
                foreach (PropertyInfo pi in tType.GetProperties())
                {
                    columns.Add(pi.Name, pi.PropertyType);
                }
                return columns;
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
            T t = new T();

            foreach (PropertyInfo pi in tType.GetProperties())
            {
                object value = row[pi.Name];
                if(value == null)
                    continue;

                Type propType = pi.PropertyType;
                object converted = Convert.ChangeType(value, propType);
                pi.SetValue(t, converted, null);
            }

            _internalList.Add(t);
        }
    }
    
}
