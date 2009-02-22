using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace Doddle.Import
{
    public interface IImportSource
    {
        ImportColumnCollection Columns { get; }
        IEnumerable<ImportRow> Rows { get; }
    }

    public class ImportRow
    {
        private readonly Dictionary<string, object> _columnData = new Dictionary<string, object>();

        public ImportRow(IImportSource source)
        {
            ImportSource = source;
        }

        public int RowNumber { get; set; }
        public IImportSource ImportSource { get; set; }

        public object this[string columnName]
        {
            get
            {
                try
                {
                    if (_columnData[columnName] == DBNull.Value)
                        return null;

                    return _columnData[columnName];
                }
                catch
                {
                    throw new ArgumentOutOfRangeException("columnName", "The import source row does not have a column named " + columnName);
                }
            }
            set
            {
                _columnData[columnName] = value;
            }
        }

    }

    public class Importer
    {
        private readonly IImportValidator _validator;
        private IImportSource _source;

        public MissingColumnAction MissingColumnAction { get; set; }

        public Importer()
        {
            UnityContainer container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("doddle/unity");
            section.Containers.Default.Configure(container);


            _validator = container.Resolve<IImportValidator>();

            _validator.Rules.Add(new MissingHeadersRule());
            _validator.Rules.Add(new RequiredFieldsRule());
            _validator.Rules.Add(new DataTypeValidationRule());
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

        public ImportValidationResult Validate(Spreadsheet spreadsheet, IImportDestination destination)
        {
            return _validator.Validate(spreadsheet, destination);
        }

        public ImportResult Import(IImportSource source, IImportDestination destination)
        {
            ImportValidationResult validation = _validator.Validate(source, destination);
            if (validation.IsSpreadsheetValid == false)
            {
                throw new ImportValidationException("This spreadsheet did not pass validation and cannot be imported.");
            }

            foreach (ImportColumn col in source.Columns)
            {
                if (!destination.FieldExists(col.Name))
                {
                    if (MissingColumnAction == MissingColumnAction.CreateColumn)
                    {
                        if (destination.SupportsFieldCreation)
                        {
                            destination.CreateField(col.Name, col.DataType);
                        }
                    }
                }
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

        public List<FileStructure> GetFileStructure(Spreadsheet spreadsheet, IImportDestination destination)
        {
            List<FileStructure> structure = new List<FileStructure>();

            if (spreadsheet == null)
                return structure;

            FileStructure s = null;

            foreach (ImportColumn col in spreadsheet.Columns)
            {
                s = new FileStructure();
                s.ColumnName = col.Name;
                s.DataTypeName = col.DataType.ToString();

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