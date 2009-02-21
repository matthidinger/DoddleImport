using System.Data.Common;
using System.Collections.ObjectModel;

namespace Doddle.Import
{
    public class SpreadsheetRowCollection : Collection<SpreadsheetRow>
    {
        private Spreadsheet _spreadsheet;
        internal SpreadsheetRowCollection(Spreadsheet spreadsheet)
        {
            _spreadsheet = spreadsheet;
        }
    }

    internal static class SpreadsheetRowLoader
    {
        internal static SpreadsheetRowCollection LoadRows(Spreadsheet spreadsheet, DbDataReader reader)
        {
            SpreadsheetRowCollection rows = new SpreadsheetRowCollection(spreadsheet);
            int i = 1;
            while (reader.Read())
            {
                i++;

                if (i == 1)
                    continue;


                SpreadsheetRow row = new SpreadsheetRow(spreadsheet, reader, i);
                rows.Add(row);
            }

            return rows;
        }

    }
}