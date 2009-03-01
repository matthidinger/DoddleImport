using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Doddle.Importing.Configuration;

namespace Doddle.Importing
{
    public class FieldValidationErrorCollection : IEnumerable<FieldValidationError>
    {
        private readonly List<FieldValidationError> _internalErrors = new List<FieldValidationError>();

        internal FieldValidationErrorCollection()
        {
            
        }

        internal void Add(IValidationRule rule, string fieldName, params object[] errorMessageValues)
        {
            FieldValidationError error = new FieldValidationError(rule, fieldName, errorMessageValues);
            _internalErrors.Add(error);
        }

        public int Count
        {
            get { return _internalErrors.Count; }
        }

        public FieldValidationError this[int index]
        {
            get { return _internalErrors[index]; }
        }

        public IEnumerator<FieldValidationError> GetEnumerator()
        {
            return _internalErrors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}