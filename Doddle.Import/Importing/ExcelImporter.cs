using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace Doddle.Import
{
    public class SpreadsheetImporter
    {
        private IImportValidator _validator;
        public MissingColumnAction MissingColumnAction { get; set; }

        public SpreadsheetImporter()
        {
            UnityContainer container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("doddle/unity");
            section.Containers.Default.Configure(container);


            _validator = container.Resolve<IImportValidator>();

            _validator.Rules.Add(new MissingHeadersRule());
            _validator.Rules.Add(new RequiredFieldsRule());
            _validator.Rules.Add(new DataTypeValidationRule());
        }

        public SpreadsheetImporter(IImportValidator validator)
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

        protected virtual void OnRowImporting(SpreadsheetRow row)
        {
            EventHandler<ImportRowEventArgs> handler = RowImporting;
            if (handler != null)
                handler(this, new ImportRowEventArgs(row));
        }

        protected virtual void OnRowImported(SpreadsheetRow row)
        {
            EventHandler<ImportRowEventArgs> handler = RowImported;
            if (handler != null)
                handler(this, new ImportRowEventArgs(row));
        }

        public ImportValidationResult Validate(Spreadsheet spreadsheet, IImportDestination destination)
        {
            return _validator.Validate(spreadsheet, destination);
        }

        public ImportResult Import(Spreadsheet spreadsheet, IImportDestination destination)
        {
            ImportValidationResult validation = _validator.Validate(spreadsheet, destination);
            if (validation.IsSpreadsheetValid == false)
            {
                throw new ImportValidationException("This spreadsheet did not pass validation and cannot be imported.");
            }

            foreach (SpreadsheetColumn col in spreadsheet.Columns)
            {
                if (!destination.FieldExists(col.Name))
                {
                    if (MissingColumnAction == MissingColumnAction.CreateColumn)
                    {
                        if (destination.SupportsFieldCreation)
                        {
                            destination.CreateField(col.Name, col.DataTypeName);
                        }
                    }
                }
            }

            int rowCount = 0;
            foreach (SpreadsheetRow row in spreadsheet.Rows)
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

        public List<FileStructure> GetFileStructure(Spreadsheet spreadsheet, IImportDestination destination)
        {
            List<FileStructure> structure = new List<FileStructure>();

            if (spreadsheet == null)
                return structure;

            FileStructure s = null;

            foreach (SpreadsheetColumn col in spreadsheet.Columns)
            {
                s = new FileStructure();
                s.ColumnName = col.Name;
                s.DataTypeName = col.DataTypeName;

                if (destination.FieldExists(col.Name))
                {
                    s.ExistsInList = true;
                    s.Action = "Nothing";

                }
                else
                {
                    s.ExistsInList = false;

                    if (MissingColumnAction == MissingColumnAction.CreateColumn)
                        s.Action = "Create field";
                    else
                        s.Action = "Ignore field";
                }

                structure.Add(s);
            }

            return structure;
        }
    }
}