using Doddle.Import.Configuration;
namespace Doddle.Import
{
    public class RowValidationResult
    {
        public ImportRow Row { get; private set; }
        public FieldValidationErrorCollection FieldErrors { get; private set; }

        public bool IsRowValid
        {
            get { return FieldErrors.Count == 0; }
        }

        internal RowValidationResult(ImportRow row)
        {
            Row = row;
            FieldErrors = new FieldValidationErrorCollection();
        }


        public void AddFieldError(IValidationRule rule, string fieldName, params object[] errorMessageValues)
        {
            FieldErrors.Add(rule, fieldName, errorMessageValues);
        }

        internal void FormatValidationMessages(string ruleName, IValidationRule rule)
        {
            string errorMessage = Config.Import.Validation.GetValidationMessage(ruleName) ?? rule.DefaultValidationMessage;

            foreach (FieldValidationError error in FieldErrors)
            {
                if (error.Rule == rule)
                {
                    error.FormatMessage(errorMessage);
                }
            }
        }
    }
}