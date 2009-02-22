using System;
using System.Reflection;

namespace Doddle.Import
{
    public class PropertyImportField : IImportField
    {
        private readonly PropertyInfo _propertyInfo;
        
        public PropertyImportField(PropertyInfo pi)
        {
            _propertyInfo = pi;
        }

        public bool IsRequired
        {
            get { return false; }
        }

        public string Name
        {
            get { return _propertyInfo.Name; }
        }

        public string DataTypeName
        {
            get { return _propertyInfo.PropertyType.ToString(); }
        }

        public Type DataType
        {
            get { return _propertyInfo.PropertyType; }
        }
    }
}