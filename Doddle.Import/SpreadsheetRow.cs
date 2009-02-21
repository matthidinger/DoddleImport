using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace Doddle.Import
{
    /// <summary>
    /// Represents a row of data in a Spreadsheet
    /// </summary>
    public class SpreadsheetRow
    {
        private Dictionary<string, object> _fieldData = new Dictionary<string, object>();

        public Spreadsheet Spreadsheet { get; set; }
        public int RowNumber { get; set; }

        internal SpreadsheetRow(Spreadsheet spreadsheet, DbDataReader reader, int index)
        {
            Spreadsheet = spreadsheet;
            RowNumber = index;
            FillFieldData(reader);
        }

        private void FillFieldData(DbDataReader reader)
        {
            foreach (SpreadsheetColumn col in Spreadsheet.Columns)
            {
                try
                {
                    
                    _fieldData[col.Name] = reader[col.Name];
                }
                catch
                {
                    _fieldData[col.Name] = null;
                }
            }
        }

        public object this[string name]
        {
            get
            {
                try
                {
                    if (_fieldData[name] == DBNull.Value)
                        return null;

                    return _fieldData[name];
                }
                catch
                {
                    throw new ArgumentOutOfRangeException("name", "The spreadsheet row does not have a column named " + name);
                }
            }
            set
            {
                _fieldData[name] = value;
            }
        }
    }
}