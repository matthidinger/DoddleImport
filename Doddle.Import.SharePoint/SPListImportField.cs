using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Doddle.Import.Importing;

namespace Doddle.Import.SharePoint
{
    public class SPListImportField : IImportDestinationField
    {
        private SPField _spField;

        public SPListImportField(SPField field)
        {
            _spField = field;
        }

        public override bool IsRequired
        {
            get { return _spField.Required; }
        }

        public override string Name
        {
            get { return _spField.Title; }
        }

        public override Type DataType
        {
            get { return _spField.FieldValueType; }
        }

        public override string DataTypeName
        {
            get { return _spField.TypeAsString; }
        }
    }

}
