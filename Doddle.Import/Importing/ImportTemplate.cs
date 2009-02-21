using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Doddle.Import.Importing
{
    /// <summary>
    /// Represents an import template which can be used to import Excel data
    /// </summary>
    public abstract class ImportTemplate
    {
        public abstract IEnumerable<IImportDestinationField> GetFields();

        public void Write(Stream destination)
        {
            string html = GetHtmlTable();

            using (StreamWriter sw = new StreamWriter(destination))
            {
                sw.Write(html);
            }
        }

        private string GetHtmlTable()
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine("<table border='1' cellpadding='0' cellspacing='0'>");
            html.AppendLine("<tr>");

            foreach (IImportDestinationField field in GetFields())
            {
                if (field.IsRequired)
                {
                    html.AppendFormat("<td><b>{0}</b></td>", field.Name);
                }
                else
                {
                    html.AppendFormat("<td>{0}</td>", field.Name);
                }
                
            }

            html.AppendLine("</tr>");
            html.AppendLine("</table>");

            return html.ToString();
        }
    }
}
