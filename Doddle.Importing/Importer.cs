using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;

namespace Doddle.Importing
{
    public class Importer
    {
        private readonly IImportValidator _validator;

        public MissingColumnAction MissingColumnAction { get; set; }

        public Importer()
        {
            _validator = new ImportValidator();
        }

        public Importer(IImportValidator validator)
        {
            _validator = validator;
        }

        /// <summary>
        /// Event fired before a Speadsheet row is imported to the Import Destination, allowing last minute alterations
        /// </summary>
        public event EventHandler<ImportRowEventArgs> RowImporting;

        /// <summary>
        /// Event fired after a Speadsheet row has been imported to the Import Destination
        /// </summary>
        public event EventHandler<ImportRowEventArgs> RowImported;

        protected virtual void OnRowImporting(ImportRow row)
        {
            EventHandler<ImportRowEventArgs> handler = RowImporting;
            if (handler != null)
                handler(this, new ImportRowEventArgs(row));
        }

        protected virtual void OnRowImported(ImportRow row)
        {
            EventHandler<ImportRowEventArgs> handler = RowImported;
            if (handler != null)
                handler(this, new ImportRowEventArgs(row));
        }

        public ImportValidationResult Validate(IImportSource source, IImportDestination destination)
        {
            return _validator.Validate(source, destination);
        }

        public ImportResult Import(IImportSource source, IImportDestination destination)
        {
            return Import(source, destination, ImportValidationMode.Validate);
        }

        public ImportResult Import(IImportSource source, IImportDestination destination, ImportValidationMode validationMode)
        {
            if (validationMode == ImportValidationMode.Validate)
            {
                ImportValidationResult validation = _validator.Validate(source, destination);
                if (validation.IsSourceValid == false)
                {
                    throw new ImportValidationException("This import source did not pass validation and cannot be imported.");
                }
            }

            foreach (ImportField sourceField in source.Fields)
            {
                EnsureFieldExists(destination, sourceField);
            }

            int rowCount = 0;
            foreach (ImportRow row in source.Rows)
            {
                OnRowImporting(row);
                destination.ImportRow(row);
                OnRowImported(row);
                rowCount++;
            }

            ImportResult result = new ImportResult();
            result.ImportedRows = rowCount;

            return result;
        }

        private void EnsureFieldExists(IImportDestination destination, ImportField field)
        {
            if (!destination.FieldExists(field.Name))
            {
                if (MissingColumnAction == MissingColumnAction.CreateColumn)
                {
                    if (destination.SupportsFieldCreation)
                    {
                        destination.CreateField(field.Name, field.DataType);
                    }
                }
            }
        }
    }
}