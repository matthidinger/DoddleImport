using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doddle.Import
{
    /// <summary>
    /// Validates a spreadsheet header for missing fields that are required by the Import Target
    /// </summary>
    public class MissingHeadersRule : IImportRule
    {
        public string RuleDescription
        {
            get { return "Validates the spreadsheet header for missing fields that are required by the Import Target (SPList)"; }
        }

        public RowValidationResult Validate(ImportRow row, IImportDestination destination, RowValidationResult result)
        {
            IImportSource source = row.ImportSource;

            foreach (ImportColumn destinationFields in destination.Fields)
            {
                if (destinationFields.IsRequired)
                {
                    if (!source.Columns.Contains(destinationFields.Name))
                    {
                        result.IsValid = false;

                        ColumnValidationError error = new ColumnValidationError();
                        error.ColumnName = destinationFields.Name;
                        error.Message = string.Format("Missing header column '{0}'", destinationFields.Name); 
                        //ExcelConfig.Importing.ValidationMessages.GetMissingHeadersMessage(destinationFields.Name);

                        result.ColumnErrors.Add(error);
                    }
                }
            }

            return result;
        }
    }
}
