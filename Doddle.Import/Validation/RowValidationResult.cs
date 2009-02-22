namespace Doddle.Import
{
    public class RowValidationResult
    {
        public ImportRow Row { get; private set; }
        public bool IsValid { get; set; }
        public ColumnValidationErrorCollection ColumnErrors { get; private set; }

        internal RowValidationResult(ImportRow row)
        {
            Row = row;
            IsValid = true;
            ColumnErrors = new ColumnValidationErrorCollection();
        }

    }
}