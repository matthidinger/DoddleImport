//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections;
//using Doddle.Reporting;

//namespace Doddle.Import.Importing
//{
//    /// <summary>
//    /// Generate a report from an Import Validation Result
//    /// </summary>
//    public class ImportValidationReportSource : IReportSource
//    {
//        private ImportValidationResult _result;

//        public ImportValidationReportSource(ImportValidationResult result)
//        {
//            _result = result;
//        }

//        public ReportFieldCollection GetFields()
//        {
//            ReportFieldCollection fields = new ReportFieldCollection();
//            fields.Add("Corrected", typeof(bool));
//            fields.Add("Row", typeof(int));
//            fields.Add("Column", typeof(string));
//            fields.Add("Message", typeof(string));
//            return fields;
//        }

//        public IEnumerable GetItems()
//        {
//            var q = from r in _result.GetInvalidRows()
//                    from c in r.ColumnErrors
//                    select new
//                    {
//                        Corrected = false,
//                        Row = r.Row.RowNumber,
//                        Column = c.ColumnName,
//                        Message = c.Message
//                    };

//            return q;
//        }

//        public object GetFieldValue(object dataItem, string fieldName)
//        {
//            return dataItem.GetProperty<object>(fieldName);
//        }
//    }
//}
