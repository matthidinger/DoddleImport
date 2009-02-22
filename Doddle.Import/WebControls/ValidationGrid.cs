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
    public class RowNmberTemplate : ITemplate
    {
        private Label _label;
        public void InstantiateIn(Control container)
        {
            _label = new Label();
            _label.DataBinding += new EventHandler(label_DataBinding);
            container.Controls.Add(_label);
        }

        void label_DataBinding(object sender, EventArgs e)
        {
            object dataItem = DataBinder.GetDataItem(_label.NamingContainer);
            _label.Text = DataBinder.Eval(dataItem, "Row.RowNumber").ToString();
        }
    }

    public class GridErrorsTemplate : ITemplate
    {
        private GridView _grid;
        public void InstantiateIn(Control container)
        {
            _grid = new GridView();
            _grid.AutoGenerateColumns = false;
            BoundField colField = new BoundField();
            colField.DataField = "ColumnName";
            colField.HeaderText = "Column Name";

            BoundField messageField = new BoundField();
            messageField.DataField = "Message";
            messageField.HeaderText = "Message";

            _grid.Columns.Add(colField);
            _grid.Columns.Add(messageField);

            _grid.DataBinding += new EventHandler(_grid_DataBinding);
            container.Controls.Add(_grid);
        }

        void _grid_DataBinding(object sender, EventArgs e)
        {
            object dataItem = DataBinder.GetDataItem(_grid.NamingContainer);
            _grid.DataSource = DataBinder.Eval(dataItem, "ColumnErrors");
        }

    }

    [DefaultProperty("ImportDestination")]
    [ToolboxData("<{0}:ImportManager runat=server></{0}:ImportManager>")]
    public class ImportManager : CompositeControl
    {
        [Category("Importing")]
        public IImportDestination ImportDestination { get; set; }

        

        private GridView _validationGrid;
        private Button _validationButton;
        private Button _importButton;
        private FileUpload _fileUpload;

        public event EventHandler<ImportRowEventArgs> RowImporting;
        public event EventHandler<ImportRowEventArgs> RowImported;


        protected void _validationButton_Click(object sender, EventArgs e)
        {
            Spreadsheet spreadsheet = GetSpreadsheet();

            ImportValidator validator = new ImportValidator();
            validator.Rules.Add(new MissingHeadersRule());
            validator.Rules.Add(new RequiredFieldsRule());
            validator.Rules.Add(new DataTypeValidationRule());

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
            
            importer.Import(sheet, ImportDestination);
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            _fileUpload = new FileUpload();


            _validationGrid = new GridView();
            _validationGrid.AutoGenerateColumns = false;
            _validationGrid.EmptyDataText = "Spreadsheet validation was successfull. No errors were found.";
            TemplateField rowField = new TemplateField();
            rowField.ItemTemplate = new RowNmberTemplate();
            
            TemplateField errorsField = new TemplateField();
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
