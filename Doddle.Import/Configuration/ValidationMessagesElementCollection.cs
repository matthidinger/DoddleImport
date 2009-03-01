using System;
using System.Configuration;

namespace Doddle.Import.Configuration
{
    [ConfigurationCollection(typeof(ValidationMessageElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class ValidationMessagesElementCollection : ConfigurationElementCollection
    {
        public ValidationMessagesElementCollection()
        {
            //AddDefaults();
        }

        private void AddDefaults()
        {
            ValidationMessageElement requiredFieldsElement = new ValidationMessageElement
            {
                Rule = "RequiredFieldsRule",
                Message = "Required field '{0}' is missing"
            };

            ValidationMessageElement missingHeadersElement = new ValidationMessageElement
            {
                Rule = "MissingHeadersRule",
                Message = "Required header field '{0}' is missing"
            };

            ValidationMessageElement dataTypeElement = new ValidationMessageElement
            {
                Rule = "DataTypeValidationRule",
                Message = "Unable to convert the value '{0}' to required type '{1}'"
            };

            BaseAdd(requiredFieldsElement);
            BaseAdd(missingHeadersElement);
            BaseAdd(dataTypeElement);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ValidationMessageElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ValidationMessageElement).Rule;
        }

        public new string this[string name]
        {
            get
            {
                foreach (string key in BaseGetAllKeys())
                {
                    if (key == name)
                    {
                        return ((ValidationMessageElement)BaseGet(name)).Message;
                    }
                }
                return null;
            }
        }

    }
}