using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Doddle.Import.Importing;
using Microsoft.SharePoint;

namespace Doddle.Import.SharePoint
{
    public class SPListImportTemplate : ImportTemplate
    {
        public SPList List { get; set; }

        public SPListImportTemplate(string listName)
        {
            List = SPContext.Current.Web.Lists[listName];
        }

        public SPListImportTemplate(SPList list)
        {
            List = list;
        }

        public override IEnumerable<IImportDestinationField> GetFields()
        {
            // TODO: Get hidden fields from Config
            //List<string> hiddenFields = StatementConfig.EntryLists.HiddenFields;
            
            List<SPField> fields = List.GetCustomFields();

            foreach (SPField field in fields)
            {
                yield return new SPListImportField(field);
            }
        }
    }
}
