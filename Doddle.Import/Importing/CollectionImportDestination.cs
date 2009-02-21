using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Doddle.Import.Importing
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

        public IEnumerable<IImportDestinationField> Fields
        {
            get 
            {
                foreach (PropertyInfo pi in tType.GetProperties())
                {
                    yield return new PropertyImportField(pi);
                }
            }
        }

        public bool SupportsFieldCreation
        {
            get { return false; }
        }

        public void CreateField(string fieldName, string typeName)
        {
            throw new NotSupportedException();
        }

        public void ImportRow(SpreadsheetRow row)
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
