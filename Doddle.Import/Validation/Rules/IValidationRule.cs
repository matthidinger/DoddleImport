using System;
using System.Linq;
using System.Text;

namespace Doddle.Importing
{
    /// <summary>
    /// The contract to use to create custom import validation rules
    /// </summary>
    public interface IValidationRule
    {
        string DefaultValidationMessage { get; }

        /// <summary>
        /// Validate a row being imported. All errors should be applied to the provided RowValidationResult parameter
        /// </summary>
        /// <param name="sourceRow">The row being sent for validation</param>
        /// <param name="destination">The destination the row is being imported to</param>
        /// <param name="rowValidation">The row validation result that all validation errors should be applied to</param>
        void Validate(ImportRow sourceRow, IImportDestination destination, RowValidationResult rowValidation);
    }
}