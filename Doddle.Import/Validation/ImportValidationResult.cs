using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doddle.Import
{
    public class ImportValidationResult
    {
        public bool IsSourceValid { get; set; }
        public RowValidationResultCollection RowResults { get; private set; }

        public ImportValidationResult()
        {
            IsSourceValid = true;
            RowResults = new RowValidationResultCollection(this);
        }

        public IEnumerable<RowValidationResult> GetInvalidRows()
        {
            return RowResults.Where(r => r.IsRowValid == false);
        }
    }
}
