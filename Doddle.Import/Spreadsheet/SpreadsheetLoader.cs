using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Doddle.Import
{
    internal static class SpreadsheetLoader
    {
        internal static ImportColumnCollection LoadColumns(Spreadsheet sheet, DbDataReader reader)
        {
            ImportColumnCollection columns = new ImportColumnCollection();

            if (reader == null)
                return columns;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                ImportColumn column = new ImportColumn();
                column.Name = reader.GetName(i);
                string typeName = reader.GetDataTypeName(i);
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

        internal static void FillRowColumnData(ImportRow row, DbDataReader reader)
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
}