using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Doddle.Web.WebControls
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
}