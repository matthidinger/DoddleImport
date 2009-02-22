//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data.Common;

//namespace Doddle.Import
//{
//    /// <summary>
//    /// Represents a row of data in a Spreadsheet
//    /// </summary>
//    public class SpreadsheetRow : IImportSourceRow
//    {
//        private readonly Dictionary<string, object> _fieldData = new Dictionary<string, object>();

//        public Spreadsheet Spreadsheet { get; set; }
//        public int RowNumber { get; set; }
//        public object GetFieldData(string fieldName)
//        {
//            try
//            {
//                if (_fieldData[fieldName] == DBNull.Value)
//                    return null;

//                return _fieldData[fieldName];
//            }
//            catch
//            {
//                throw new ArgumentOutOfRangeException("fieldName", "The import source row does not have a column named " + fieldName);
//            }
//        }

//        public void SetFieldData(string fieldName, object value)
//        {
//            _fieldData[fieldName] = value;
//        }

//        public IImportSource Source
//        {
//            get { return Spreadsheet; }
//        }

//        internal SpreadsheetRow(Spreadsheet spreadsheet, DbDataReader reader, int index)
//        {
//            Spreadsheet = spreadsheet;
//            RowNumber = index;
//            FillFieldData(reader);
//        }

//        private void FillFieldData(DbDataReader reader)
//        {
//            foreach (SpreadsheetColumn col in Spreadsheet.Columns)
//            {
//                try
//                {

//                    _fieldData[col.Name] = reader[col.Name];
//                }
//                catch
//                {
//                    _fieldData[col.Name] = null;
//                }
//            }
//        }

//        public object this[string name]
//        {
//            get
//            {
//                return GetFieldData(name);
//            }
//            set
//            {
//                SetFieldData(name, value);
//            }
//        }
//    }
//}