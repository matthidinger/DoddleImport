using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Doddle.Import
{
    /// <summary>
    /// Represents an import template which can be used to import Excel data
    /// </summary>
    public class ImportTemplate
    {
        public IImportDestination ImportDestination { get; set; }

        public ImportTemplate(IImportDestination destination)
        {
            ImportDestination = destination;
        }

        public virtual void Write(Stream destination)
        {
            string html = GetHtmlTable();

            using (StreamWriter sw = new StreamWriter(destination))
            {
                sw.Write(html);
            }
        }

        protected virtual string GetHtmlTable()
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine("<table border='1' cellpadding='0' cellspacing='0'>");
            html.AppendLine("<tr>");

            foreach (ImportField field in ImportDestination.Fields)
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
