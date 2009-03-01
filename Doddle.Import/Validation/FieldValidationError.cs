using System.Collections;
namespace Doddle.Import
{
    public class FieldValidationError
    {
        private object[] _messageValues;

        public FieldValidationError(IValidationRule rule, string fieldName, params object[] messageValues)
        {
            Rule = rule;
            FieldName = fieldName;
            _messageValues = messageValues;
        }

        public IValidationRule Rule { get; set; }
        public string FieldName { get; set; }
        public string Message { get; private set; }

        public void FormatMessage(string messageFormat)
        {
            ArrayList values = new ArrayList();
            values.Add(FieldName);
            values.AddRange(_messageValues);

            Message = string.Format(messageFormat ?? Rule.DefaultValidationMessage, values.ToArray());
        }
    }
}