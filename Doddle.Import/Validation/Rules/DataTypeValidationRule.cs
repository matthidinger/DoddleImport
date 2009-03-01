using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doddle.Importing
{
    /// <summary>
    /// Validates an import source for data-type mismatch errors
    /// </summary>
    public class DataTypeValidationRule : IValidationRule
    {
        public string DefaultValidationMessage
        {
            get { return "Unable to convert the source value '{1}' to destination type '{2}'"; }
        }

        public void Validate(ImportRow sourceRow, IImportDestination destination, RowValidationResult rowValidation)
        {
            IImportSource source = sourceRow.ImportSource;

            foreach (ImportField destinationField in destination.Fields)
            {
                if (!source.Fields.Contains(destinationField.Name))
                    continue;


                object importData = sourceRow[destinationField.Name];
                if (importData == null || importData == DBNull.Value)
                    continue;

                Type targetType = destinationField.DataType;
                try
                {
                    Convert.ChangeType(importData, targetType);
                }
                catch
                {
                    rowValidation.AddFieldError(this, destinationField.Name, importData, targetType);
                }
            }
        }
    }
}
