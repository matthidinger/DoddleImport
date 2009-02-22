using System;
using System.Linq;
using System.Text;

namespace Doddle.Import
{
    /// <summary>
    /// The contact that all Excel Import Rules must implement
    /// </summary>
    public interface IImportRule
    {
        string RuleDescription { get; }
        RowValidationResult Validate(ImportRow sourceRow, IImportDestination destination, RowValidationResult result);
    }
}