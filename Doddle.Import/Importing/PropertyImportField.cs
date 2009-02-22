using System;
using System.Reflection;

namespace Doddle.Import
{
    public class PropertyImportField : IImportDestinationField
    {
        private PropertyInfo _propertyInfo;

        public PropertyImportField(PropertyInfo pi)
        {
            _propertyInfo = pi;
        }

        public override bool IsRequired
        {
            get { return false; }
        }

        public override string Name
        {
            get { return _propertyInfo.Name; }
        }

        public override string DataTypeName
        {
            get { return _propertyInfo.PropertyType.ToString(); }
        }

        public override Type DataType
        {
            get { return _propertyInfo.PropertyType; }
        }
    }
}