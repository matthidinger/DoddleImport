using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Doddle.Importing.Configuration;

namespace Doddle.Importing
{
    /// <summary>
    /// Validates import source rows for any missing fields that are required by the import destination
    /// </summary>
    public class RequiredFieldsRule : IValidationRule
    {
        public string DefaultValidationMessage
        {
            get { return "Required field '{0}' is missing or empty"; }
        }

        public void Validate(ImportRow sourceRow, IImportDestination destination, RowValidationResult rowValidation)
        {
            IImportSource source = sourceRow.ImportSource;

            foreach (ImportField destinationField in destination.Fields)
            {
                if (destinationField.IsRequired)
                {
                    if (!source.Fields.Contains(destinationField.Name))
                    {
                        rowValidation.AddFieldError(this, destinationField.Name);
                        continue;
                    }

                    object data = sourceRow[destinationField.Name];
                    if (data == null || data == DBNull.Value)
                    {
                        rowValidation.AddFieldError(this, destinationField.Name);
                    }
                }
            }
        }
    }
}
