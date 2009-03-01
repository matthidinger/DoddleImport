using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Doddle.Import.WebControls
{
    [DefaultProperty("ImportDestination")]
    [DefaultEvent("RowImporting")]
    [ToolboxData("<{0}:ImportManager runat=server></{0}:ImportManager>")]
    public class ImportManager : CompositeControl
    {
        public ImportManager()
        {
            ValidationMode = ImportValidationMode.Validate;
        }

        [Category("Importing")]
        public IImportDestination ImportDestination { get; set; }

        [Category("Importing")]
        public ImportValidationMode ValidationMode { get; set; }
        

        private GridView _validationGrid;
        private Button _validationButton;
        private Button _importButton;
        private FileUpload _fileUpload;

        [Category("Importing")]
        public event EventHandler<ImportRowEventArgs> RowImporting;

        [Category("Importing")]
        public event EventHandler<ImportRowEventArgs> RowImported;


        protected void _validationButton_Click(object sender, EventArgs e)
        {
            Spreadsheet spreadsheet = GetSpreadsheet();

            ImportValidator validator = new ImportValidator();
            ImportValidationResult result = validator.Validate(spreadsheet, ImportDestination);

            _validationGrid.DataSource = result.GetInvalidRows();
            _validationGrid.DataBind();
        }

        private Spreadsheet GetSpreadsheet()
        {
            Spreadsheet spreadsheet = new Spreadsheet(_fileUpload.PostedFile.InputStream);
            return spreadsheet;
        }
        
        void _importButton_Click(object sender, EventArgs e)
        {        
            Spreadsheet sheet = GetSpreadsheet();

            Importer importer = new Importer();
            
            importer.RowImporting += this.RowImporting;
            importer.RowImported += this.RowImported;
            
            importer.Import(sheet, ImportDestination, ValidationMode);
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            _fileUpload = new FileUpload();

            _validationGrid = new GridView();
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

            _validationButton = new Button();
            _validationButton.Text = "Validate";
            _validationButton.Click += new EventHandler(_validationButton_Click);

            _importButton = new Button();
            _importButton.Text = "Import";
            _importButton.Click += new EventHandler(_importButton_Click);

            Controls.Add(_fileUpload);
            Controls.Add(_validationButton);
            Controls.Add(_importButton);
            Controls.Add(_validationGrid);
        }
    }
}
