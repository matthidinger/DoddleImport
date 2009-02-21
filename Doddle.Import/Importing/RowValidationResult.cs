namespace Doddle.Import.Importing
{
    public class RowValidationResult
    {
        public SpreadsheetRow Row { get; private set; }
        public bool IsValid { get; set; }
        public ColumnValidationErrorCollection ColumnErrors { get; private set; }

        internal RowValidationResult(SpreadsheetRow row)
        {
            Row = row;
            IsValid = true;
            ColumnErrors = new ColumnValidationErrorCollection();
        }

    }
}