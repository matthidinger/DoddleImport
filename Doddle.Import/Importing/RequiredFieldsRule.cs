using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doddle.Import
{
    /// <summary>
    /// Validates a spreadsheet rows for any missing fields that are required by the Import Target
    /// </summary>
    public class RequiredFieldsRule : IImportRule
    {
        public string RuleDescription
        {
            get { return "Validates the spreadsheet rows for any missing fields that are required by the Import Target (SPList)"; }
        }

        public RowValidationResult Validate(SpreadsheetRow row, IImportDestination destination, RowValidationResult result)
        {
            Spreadsheet spreadsheet = row.Spreadsheet;

            foreach (IImportDestinationField destinationField in destination.Fields)
            {
                if (destinationField.IsRequired)
                {
                    if (!spreadsheet.Columns.Contains(destinationField.Name))
                    {
                        result.IsValid = false;

                        ColumnValidationError error = new ColumnValidationError();
                        error.ColumnName = destinationField.Name;
                        error.Message = ""; // ExcelConfig.Importing.ValidationMessages.GetInvalidColumnMessage(destinationField.Name);

                        result.ColumnErrors.Add(error);

                        continue;
                    }

                    object data = row[destinationField.Name];
                    if (data == null || data == DBNull.Value)
                    {
                        result.IsValid = false;

                        ColumnValidationError error = new ColumnValidationError();
                        error.ColumnName = destinationField.Name;
                        error.Message = ""; //ExcelConfig.Importing.ValidationMessages.GetRequiredFieldEmptyMessage(destinationField.Name);

                        result.ColumnErrors.Add(error);
                    }

                }
            }

            return result;
        }
    }
}
