using System;
using System.Configuration;

namespace Doddle.Import.Configuration
{
    [ConfigurationCollection(typeof(ValidationMessageElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class ValidationMessagesElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ValidationMessageElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ValidationMessageElement)element).Rule;
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