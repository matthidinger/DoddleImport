using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Doddle.Import
{
    internal static class SpreadsheetLoader
    {
        internal static ImportColumnCollection LoadColumns(Spreadsheet sheet, DbDataReader reader)
        {
            ImportColumnCollection columns = new ImportColumnCollection(sheet);

            if (reader == null)
                return columns;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                ImportColumn column = new ImportColumn();
                column.Name = reader.GetName(i);
                string typeName = reader.GetDataTypeName(i);
                //column.DataTypeName = typeName;
                column.DataType =  Type.GetType(typeName);

                columns.Add(column);
            }

            return columns;
        }

        internal static IEnumerable<ImportRow> LoadRows(Spreadsheet spreadsheet, DbDataReader reader)
        {
            int i = 1;
            while (reader.Read())
            {
                i++;

                if (i == 1)
                    continue;

                ImportRow row = new ImportRow(spreadsheet);
                row.RowNumber = i;
                FillRowColumnData(row, reader);
                yield return row;
            }

        }

        private static void FillRowColumnData(ImportRow row, DbDataReader reader)
        {
            foreach (ImportColumn col in row.ImportSource.Columns)
            {
                try
                {

                    row[col.Name] = reader[col.Name];
                }
                catch
                {
                    row[col.Name] = null;
                }
            }
        }
    }


    //public class SpreadsheetColumnCollection : IEnumerable<SpreadsheetColumn>
    //{
    //    private Dictionary<string, SpreadsheetColumn> _internalDictionary = new Dictionary<string, SpreadsheetColumn>();
    //    private Spreadsheet _spreadsheet;

    //    internal SpreadsheetColumnCollection(Spreadsheet spreadsheet)
    //        : this(spreadsheet, null)
    //    {

    //    }

    //    internal SpreadsheetColumnCollection(Spreadsheet spreadsheet, DbDataReader reader)
    //    {
    //        _spreadsheet = spreadsheet;

    //        if (reader == null)
    //            return;

    //        for (int i = 0; i < reader.FieldCount; i++)
    //        {
    //            SpreadsheetColumn column = new SpreadsheetColumn();
    //            column.Name = reader.GetName(i);
    //            string typeName = reader.GetDataTypeName(i);
    //            column.DataTypeName = typeName;
    //            column.DataType = Type.GetType(typeName);

    //            this.Add(column);
    //        }
    //    }

    //    public void Add(string columnName)
    //    {
    //        Add(columnName, null, null);
    //    }

    //    public void Add(string columnName, Type dataType)
    //    {
    //        Add(columnName, dataType, null);
    //    }

    //    public void Add(string columnName, Type dataType, string dataTypeName)
    //    {
    //        SpreadsheetColumn col = new SpreadsheetColumn(columnName, dataType, dataTypeName);
    //        col.Spreadsheet = _spreadsheet;
    //        Add(col);
    //    }

    //    public void Add(SpreadsheetColumn column)
    //    {
    //        _internalDictionary.Add(column.Name, column);
    //    }

    //    public SpreadsheetColumn this[string name]
    //    {
    //        get
    //        {
    //            return _internalDictionary[name];
    //        }
    //    }

    //    public bool Contains(string name)
    //    {
    //        return _internalDictionary.ContainsKey(name);
    //    }

    //    #region IEnumerable<SpreadsheetColumn> Members

    //    public IEnumerator<SpreadsheetColumn> GetEnumerator()
    //    {
    //        return _internalDictionary.Values.GetEnumerator();
    //    }

    //    #endregion

    //    #region IEnumerable Members

    //    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //    {
    //        return _internalDictionary.Values.GetEnumerator();
    //    }

    //    #endregion
    //}
}