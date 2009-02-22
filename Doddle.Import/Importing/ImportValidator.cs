using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Doddle.Import
{
    /// <summary>
    /// Used to validate an excel spreadsheet for importing
    /// </summary>
    public class ImportValidator : IImportValidator
    {
        /// <summary>
        /// If true the Validate method will not throw an InvalidOperationException if no rules have been added. Default value: false
        /// </summary>
        public bool AllowEmptyRules { get; set; }

        /// <summary>
        /// The collection of rules to verify during validation
        /// </summary>
        public ImportRuleCollection Rules { get; private set; }

        public ImportValidator()
        {
            AllowEmptyRules = false;
            Rules = new ImportRuleCollection();
        }

        /// <summary>
        /// Validate a spreadsheet against an Import Target
        /// </summary>
        /// <param name="spreadsheet">The spreadsheet to be validated</param>
        /// <param name="destination">The import target where the spreadsheet rows will be inserted</param>
        public ImportValidationResult Validate(Spreadsheet spreadsheet, IImportDestination destination)
        {
            if (Rules.Count == 0 && AllowEmptyRules == false)
                throw new ImportValidationException("Unable to Validate because no Import Rules have been added to the Rules property. If this was intentional set the AllowEmptyRules property to true");

            ImportValidationResult importResult = new ImportValidationResult();

            foreach (SpreadsheetRow row in spreadsheet.Rows)
            {
                RowValidationResult rowResult = new RowValidationResult(row);

                foreach (IImportRule rule in Rules)
                {
                    rule.Validate(row, destination, rowResult);
                }

                importResult.RowResults.Add(rowResult);
            }

            return importResult;
        }
    }
}
