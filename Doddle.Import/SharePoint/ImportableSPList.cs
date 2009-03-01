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
    /// Provides importing functionality to and from a SharePoint list
    /// </summary>
    public class ImportableSPList : IImportDestination, IImportSource
    {
        private SPList _list;


        public ImportableSPList(SPList list)
        {
            _list = list;
        }

        public bool FieldExists(string name)
        {
            return _list.Fields.ContainsField(name);
        }

        public ImportFieldCollection Fields
        {
            get
            {
                ImportFieldCollection fields = new ImportFieldCollection();
                foreach (SPField field in _list.GetCustomFields())
                {
                    fields.Add(field.Title, field.FieldValueType, field.Required); 
                }

                return fields;
            }
        }

        public IEnumerable<ImportRow> Rows
        {
            get 
            {
                foreach (SPListItem item in _list.Items)
                {
                    yield return new ImportRow(this, item);
                }
            }
        }

        public object GetFieldDataFromRow(object dataItem, string fieldName)
        {
            SPListItem listItem = dataItem as SPListItem;
            if(listItem == null)
                return null;

            return listItem[fieldName];
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

                foreach (ImportField col in source.Fields)
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
