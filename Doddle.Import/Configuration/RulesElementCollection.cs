using System.Configuration;

namespace Doddle.Import.Configuration
{
    [ConfigurationCollection(typeof(RuleElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class RulesElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as RuleElement).Name;
        }

        public new RuleElement this[string name]
        {
            get
            {
                return ((RuleElement)base.BaseGet(name));
            }
        }
    }
}