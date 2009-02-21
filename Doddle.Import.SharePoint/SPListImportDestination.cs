using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Doddle.Import.Importing;

namespace Doddle.Import.SharePoint
{
    /// <summary>
    /// Represents a SharePoint List import destination
    /// </summary>
    public class SPListImportDestination : IImportDestination
    {
        private SPList _list;


        public SPListImportDestination(SPList list)
        {
            _list = list;
        }

        public bool FieldExists(string name)
        {
            return _list.Fields.ContainsField(name);
        }

        public IEnumerable<IImportDestinationField> Fields
        {
            get
            {
                foreach (SPField field in _list.GetCustomFields())
                {
                    yield return new SPListImportField(field);
                }
            }
        }

        public bool SupportsFieldCreation
        {
            get { return true; }
        }

        public void CreateField(string fieldName, string typeName)
        {
            SPField field = _list.AddField(fieldName, GetDataType(typeName), false, false);
            //SPField field = _list.AddField(fieldName, typeName, false, false);
            _list.Update();
        }


        public void ImportRow(SpreadsheetRow row)
        {
            Spreadsheet spreadsheet = row.Spreadsheet;

            SPListItem item = _list.Items.Add();

            foreach (SpreadsheetColumn col in spreadsheet.Columns)
            {
                if (row[col.Name] != null)
                {
                    if (FieldExists(col.Name))
                    {
                        item[col.Name] = row[col.Name];
                    }
                }
            }

            item.Update();
        }

        private SPFieldType GetDataType(string dataType)
        {
            SPFieldType type = SPFieldType.Text;
            switch (dataType)
            {
                case "DBTYPE_WVARCHAR":
                    type = SPFieldType.Text;
                    break;

                case "DBTYPE_R8":
                    type = SPFieldType.Number;
                    break;

                case "DBTYPE_CY":
                    type = SPFieldType.Currency;
                    break;

                case "DBTYPE_DATE":
                    type = SPFieldType.DateTime;
                    break;

                default:
                    type = SPFieldType.Text;

                    break;
            }

            return type;
        }
    }
}
