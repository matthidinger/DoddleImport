using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace Doddle.SharePoint
{
    public static class SPListExtensions
    {
        public static SPField AddField(this SPList list, string name, SPFieldType type, bool required)
        {
            return AddField(list, name, type, required, true);
        }
        public static SPField AddField(this SPList list, string name, SPFieldType type, bool required, bool addToView)
        {
            return AddField(list, name, type, required, addToView, null);
        }

        public static SPField AddField(this SPList list, string name, SPFieldType type, bool required, bool addToView, StringCollection choices)
        {
            string nameWithoutSpaces = name.Replace(" ", "");
            string fieldName;
            SPField field;

            if (type == SPFieldType.Choice)
            {
                fieldName = list.Fields.Add(nameWithoutSpaces, type, required, false, choices);
            }
            else
            {
                fieldName = list.Fields.Add(nameWithoutSpaces, type, required);
            }

            field = list.Fields[fieldName];
            field.Title = name;

            if (addToView == true)
            {
                //SPView view = list.Views["All Items"];
                SPView view = list.Views[0];
                view.ViewFields.Add(field);
                view.Update();
            }

            field.Update();
            return field;
        }

        public static SPField AddField(this SPList list, string name, string type, bool required, bool addToView)
        {
            string nameWithoutSpaces = name.Replace(" ", "");

            SPField field = list.Fields.CreateNewField(type, name);
            //field.Title = name;
            //field.Required = required;
            list.Fields.Add(field);

            if (addToView == true)
            {
                //SPView view = list.Views["All Items"];
                SPView view = list.Views[0];
                view.ViewFields.Add(field);
                view.Update();
            }

            return field;
        }


        public static SPField SetTitleField(this SPList list, string name)
        {
            string nameWithoutSpaces = name.Replace(" ", "");
            SPField field = list.Fields["Title"];
            field.Title = name;
            field.StaticName = nameWithoutSpaces;
            field.Update();
            return field;
        }

        public static List<SPField> GetCustomFields(this SPListItem item)
        {
            return GetCustomFields(item.ParentList);
        }

        public static List<SPField> GetCustomFields(this SPList list, List<string> customHiddenFields)
        {
            List<SPField> fields = new List<SPField>();
            StringCollection fieldNames = new StringCollection();
            if (customHiddenFields == null)
                customHiddenFields = new List<string>();

            foreach (SPField field in list.Fields)
            {
                if (field.Hidden == false)
                {
                    if (!customHiddenFields.Contains(field.Title) && !fieldNames.Contains(field.Title) && !IsInternalField(field))
                    {
                        fieldNames.Add(field.Title);
                        fields.Add(field);
                    }

                }
            }

            return fields;
        }

        public static List<SPField> GetCustomFields(this SPList list)
        {
            return GetCustomFields(list, null);
        }

        public static bool IsInternalField(this SPField field)
        {
            StringCollection internalNames = new StringCollection
            {
                "ID", 
                "Content Type", 
                "Modified", 
                "Created", 
                "Created By", 
                "Modified By", 
                "Version", 
                "Attachments", 
                "Edit", 
                "Type" 
            };

            return internalNames.Contains(field.Title);
        }
    }
}
