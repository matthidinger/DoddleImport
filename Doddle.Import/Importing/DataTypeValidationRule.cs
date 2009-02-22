using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doddle.Import
{
    /// <summary>
    /// Validates a spreadsheet for data-type mismatch errors
    /// </summary>
    public class DataTypeValidationRule : IImportRule
    {
        public string RuleDescription
        {
            get { return "Validates the spreadsheet for data-type mismatch errors"; }
        }

        public RowValidationResult Validate(SpreadsheetRow row, IImportDestination target, RowValidationResult result)
        {
            Spreadsheet spreadsheet = row.Spreadsheet;

            foreach (IImportDestinationField targetField in target.Fields)
            {
                if (!spreadsheet.Columns.Contains(targetField.Name))
                    continue;


                object importData = row[targetField.Name];
                if (importData == null || importData == DBNull.Value)
                    continue;

                Type targetType = targetField.DataType;
                try
                {
                    Convert.ChangeType(importData, targetType);
                }
                catch
                {
                    result.IsValid = false;

                    ColumnValidationError error = new ColumnValidationError();
                    error.ColumnName = targetField.Name;
                    error.Message = ""; //ExcelConfig.Importing.ValidationMessages.GetInvalidDataTypeMessage(importData, targetField.DataTypeName);

                    result.ColumnErrors.Add(error);
                }
            }

            return result;
        }
    }
}
