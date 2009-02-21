namespace Doddle.Import.Importing
{
    public interface IImportValidator
    {
        /// <summary>
        /// If true the Validate method will not throw an InvalidOperationException if no rules have been added. Default value: false
        /// </summary>
        bool AllowEmptyRules { get; set; }

        /// <summary>
        /// The collection of rules to verify during validation
        /// </summary>
        ImportRuleCollection Rules { get; }

        /// <summary>
        /// Validate a spreadsheet against an Import Target
        /// </summary>
        /// <param name="spreadsheet">The spreadsheet to be validated</param>
        /// <param name="destination">The import target where the spreadsheet rows will be inserted</param>
        ImportValidationResult Validate(Spreadsheet spreadsheet, IImportDestination destination);
    }
}