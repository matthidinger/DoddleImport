using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doddle.Import
{
    /// <summary>
    /// Validates an import source for missing fields that are required by the Import Target
    /// </summary>
    public class MissingHeadersRule : IValidationRule
    {
        public string DefaultValidationMessage
        {
            get { return "Required field '{0}' is missing"; }
        }

        public void Validate(ImportRow row, IImportDestination destination, RowValidationResult rowValidation)
        {
            IImportSource source = row.ImportSource;

            foreach (ImportField destinationField in destination.Fields)
            {
                if (destinationField.IsRequired)
                {
                    if (!source.Fields.Contains(destinationField.Name))
                    {
                        rowValidation.AddFieldError(this, destinationField.Name);
                    }
                }
            }
        }
    }
}
