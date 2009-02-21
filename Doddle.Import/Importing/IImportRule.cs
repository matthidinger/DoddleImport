using System;
using System.Linq;
using System.Text;

namespace Doddle.Import.Importing
{
    /// <summary>
    /// The contact that all Excel Import Rules must implement
    /// </summary>
    public interface IImportRule
    {
        string RuleDescription { get; }
        RowValidationResult Validate(SpreadsheetRow row, IImportDestination destination, RowValidationResult result);
    }
}