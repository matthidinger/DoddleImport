using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace Doddle.Import.SharePoint
{
    public class SPListImportField : IImportField
    {
        private readonly SPField _spField;

        public SPListImportField(SPField field)
        {
            _spField = field;
        }

        public bool IsRequired
        {
            get { return _spField.Required; }
        }

        public string Name
        {
            get { return _spField.Title; }
        }

        public Type DataType
        {
            get { return _spField.FieldValueType; }
        }

        public string DataTypeName
        {
            get { return _spField.TypeAsString; }
        }
    }

}
