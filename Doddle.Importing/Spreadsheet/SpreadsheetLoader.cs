using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Doddle.Importing
{
    internal static class SpreadsheetLoader
    {
        internal static ImportFieldCollection LoadColumns(Spreadsheet2003 sheet, DbDataReader reader)
        {
            ImportFieldCollection fields = new ImportFieldCollection();

            if (reader == null)
                return fields;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                ImportField field = new ImportField();
                field.Name = reader.GetName(i);
                string typeName = reader.GetDataTypeName(i);
                field.DataType =  Type.GetType(typeName);

                fields.Add(field);
            }

            return fields;
        }

        internal static IEnumerable<ImportRow> LoadRows(Spreadsheet2003 spreadsheet, DbDataReader reader)
        {
            int i = 1;
            while (reader.Read())
            {
                i++;

                if (i == 1)
                    continue;

                ImportRow row = new ImportRow(spreadsheet, null);
                row.RowNumber = i;
                FillRowColumnData(row, reader);
                yield return row;
            }
        }

        internal static void FillRowColumnData(ImportRow row, DbDataReader reader)
        {
            foreach (ImportField col in row.ImportSource.Fields)
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