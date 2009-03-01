using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Doddle.Import.WebControls
{
    public class GridErrorsTemplate : ITemplate
    {
        private GridView _grid;
        public void InstantiateIn(Control container)
        {
            _grid = new GridView();
            _grid.AutoGenerateColumns = false;
            BoundField colField = new BoundField();
            colField.DataField = "FieldName";
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
            _grid.DataSource = DataBinder.Eval(dataItem, "FieldErrors");
        }

    }
}