using System;

namespace Doddle.Import
{
    /// <summary>
    /// Represents a column in a Spreadsheet
    /// </summary>
    public class SpreadsheetColumn
    {
        public SpreadsheetColumn() { }

        public SpreadsheetColumn(string columnName, string dataTypeName)
            : this(columnName, null, dataTypeName) { }

        public SpreadsheetColumn(string columnName, Type dataType)
            : this(columnName, dataType, null) { }

        public SpreadsheetColumn(string columnName, Type dataType, string dataTypeName)
        {
            Name = columnName;
            DataType = dataType;
            DataTypeName = dataTypeName;
        }

        public Spreadsheet Spreadsheet { get; set; }
        public string Name { get; set; }

        public string DataTypeName { get; set; }
        public Type DataType { get; set; }

    }
}