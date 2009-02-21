using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Doddle.Import.Importing
{
    public class RowValidationResultCollection : Collection<RowValidationResult>
    {
        private ImportValidationResult _importResult;

        internal RowValidationResultCollection(ImportValidationResult result)
        {
            _importResult = result;    
        }

        protected override void InsertItem(int index, RowValidationResult item)
        {
            if (item.IsValid == false)
            {
                _importResult.IsSpreadsheetValid = false;
            }
            base.InsertItem(index, item);
        }
    }
}
