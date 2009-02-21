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
            return (element as ValidationMessageElement).Name;
        }

        public new string this[string name]
        {
            get
            {
                return ((ValidationMessageElement)base.BaseGet(name)).Value;
            }
        }

    }
}