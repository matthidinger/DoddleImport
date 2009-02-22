using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Doddle.SharePoint;
using Microsoft.SharePoint;
using System.Transactions;

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

        public ImportColumnCollection Fields
        {
            get
            {
                ImportColumnCollection columns = new ImportColumnCollection();
                foreach (SPField field in _list.GetCustomFields())
                {
                    columns.Add(field.Title, field.FieldValueType, field.Required); 
                }

                return columns;
            }
        }

        public bool SupportsFieldCreation
        {
            get { return true; }
        }

        public void CreateField(string fieldName, Type dataType)
        {
            SPField field = _list.AddField(fieldName, GetDataType(dataType.ToString()), false, false);
            _list.Update();
        }

        public void ImportRow(ImportRow row)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                IImportSource source = row.ImportSource;

                SPListItem item = _list.Items.Add();

                foreach (ImportColumn col in source.Columns)
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
