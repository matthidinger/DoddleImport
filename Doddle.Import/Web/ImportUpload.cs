using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Doddle.Import.WebControls
{
    [DefaultProperty("ImportDestination")]
    [DefaultEvent("ImportComplete")]
    [ToolboxData("<{0}:ImportUpload runat=server />")]
    public class ImportUpload : CompositeControl
    {
        private readonly Label _errorLabel = new Label();
        private readonly FileUpload _fileUpload = new FileUpload();
        private readonly Button _importButton = new Button();
        private readonly Button _validationButton = new Button();
        private readonly GridView _validationGrid = new GridView();

        public ImportUpload()
        {
            ValidationMode = ImportValidationMode.Validate;
        }


        [Category("Importing")]
        public IImportDestination ImportDestination { get; set; }

        [Category("Importing")]
        public ImportValidationMode ValidationMode { get; set; }

        [Category("Importing")]
        public event EventHandler<ImportRowEventArgs> RowImporting;

        [Category("Importing")]
        public event EventHandler<ImportRowEventArgs> RowImported;

        [Category("Importing")]
        public event EventHandler<ImportEventArgs> ImportComplete;

        [Category("Importing")]
        public event EventHandler<ValidationEventArgs> ValidationComplete;

        protected virtual void DisplayErrorMessage(string message)
        {
            _errorLabel.Text = message;
            _errorLabel.Visible = true;
        }

        private void InvokeValidationComplete(ValidationEventArgs e)
        {
            EventHandler<ValidationEventArgs> validationCompleteHandler = ValidationComplete;
            if (validationCompleteHandler != null) validationCompleteHandler(this, e);
        }

        private void InvokeImportComplete(ImportEventArgs e)
        {
            EventHandler<ImportEventArgs> importCompleteHandler = ImportComplete;
            if (importCompleteHandler != null) importCompleteHandler(this, e);
        }


        protected void _validationButton_Click(object sender, EventArgs e)
        {
            Spreadsheet spreadsheet = GetSpreadsheet();
            if (spreadsheet == null)
                return;

            ImportValidator validator = new ImportValidator();
            ImportValidationResult result = validator.Validate(spreadsheet, ImportDestination);

            InvokeValidationComplete(new ValidationEventArgs(result));

            _validationGrid.DataSource = result.GetInvalidRows();
            _validationGrid.DataBind();
        }

        private Spreadsheet GetSpreadsheet()
        {
            if (!_fileUpload.HasFile)
            {
                DisplayErrorMessage("Please select a valid file to upload.");
                return null;
            }
            try
            {
                Spreadsheet spreadsheet = new Spreadsheet(_fileUpload.PostedFile.InputStream);
                return spreadsheet;
            }
            catch (Exception)
            {
                DisplayErrorMessage("The selected file is not a valid Excel spreadsheet. Please note that Excel 2008 (.xlsx) files are not supported at this time.");
                return null;
            }
        }

        private void _importButton_Click(object sender, EventArgs e)
        {
            Spreadsheet spreadsheet = GetSpreadsheet();
            if (spreadsheet == null)
                return;

            Importer importer = new Importer();

            importer.RowImporting += RowImporting;
            importer.RowImported += RowImported;

            ImportResult result = importer.Import(spreadsheet, ImportDestination, ValidationMode);
            InvokeImportComplete(new ImportEventArgs(result));
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            _errorLabel.EnableViewState = false;
            _errorLabel.Visible = false;
            Controls.Add(_errorLabel);

            _validationGrid.AutoGenerateColumns = false;
            _validationGrid.EmptyDataText = "Import validation was successfull. No errors were found.";

            TemplateField rowField = new TemplateField();
            rowField.HeaderText = "Row Number";

            rowField.ItemTemplate = new RowNmberTemplate();

            TemplateField errorsField = new TemplateField();
            errorsField.HeaderText = "Errors";
            errorsField.ItemTemplate = new GridErrorsTemplate();

            _validationGrid.Columns.Add(rowField);
            _validationGrid.Columns.Add(errorsField);

            _validationButton.Text = "Validate";
            _validationButton.Click += _validationButton_Click;


            _importButton.Text = "Import";
            _importButton.Click += _importButton_Click;

            Controls.Add(_fileUpload);
            Controls.Add(_validationButton);
            Controls.Add(_importButton);
            Controls.Add(_validationGrid);
        }
    }
}