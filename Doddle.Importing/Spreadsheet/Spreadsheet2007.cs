using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace Doddle.Importing
{
    public class Spreadsheet2007 : ISpreadsheet
    {
        private readonly ExcelPackage _spreadsheet;
        private readonly ExcelWorksheet _excelWorksheet;

        public Spreadsheet2007(Stream stream)
        {
            _spreadsheet = new ExcelPackage(stream);
            _excelWorksheet = _spreadsheet.Workbook.Worksheets[1];
        }

        #region Implementation of IImportSource

        public ImportFieldCollection Fields
        {
            get
            {
                var fields = new ImportFieldCollection();
                for(int i = 1; i < _excelWorksheet.Dimension.End.Column; i++)
                {
                    var fieldName = _excelWorksheet.Cells[1, i].Value;
                    if (fieldName != null)
                    {
                        var importField = new ImportField();
                        importField.Name = fieldName.ToString();
                        fields.Add(importField);
                    }
                }
                return fields;
            }
        }

        public IEnumerable<ImportRow> Rows
        {
            get
            {
                for(int i = 1; i < _excelWorksheet.Dimension.End.Row; i++)
                {
                    yield return new ImportRow(this, _excelWorksheet.Row(i));
                }
            }
        }

        public object GetFieldDataFromRow(object dataItem, string fieldName)
        {
            var row = dataItem as ExcelRow;
            if (row != null)
            {
                for(int i = 1; i < _excelWorksheet.Dimension.End.Column; i++)
                {
                    var columnValue = _excelWorksheet.Cells[1, i].Value;
                    if (columnValue != null && columnValue.ToString() == fieldName)
                    {
                        return _excelWorksheet.Cells[row.Row, i].Value;
                    }
                }
            }
            return null;
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _spreadsheet.Dispose();
        }

        #endregion

        #region Implementation of ISpreadsheet

        public string[] LoadWorksheets()
        {
            var worksheets = new List<string>();
            foreach (var sheet in _spreadsheet.Workbook.Worksheets)
            {
                worksheets.Add(sheet.Name);
            }
            return worksheets.ToArray();
        }

        #endregion
    }
}