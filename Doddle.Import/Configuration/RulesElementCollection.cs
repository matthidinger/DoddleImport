using System.Configuration;

namespace Doddle.Import.Configuration
{
    [ConfigurationCollection(typeof(RuleElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class RulesElementCollection : ConfigurationElementCollection
    {
        public RulesElementCollection()
        {
            AddDefaults();
        }

        private void AddDefaults()
        {
            RuleElement requiredFieldsElement = new RuleElement
            {
                Name = "RequiredFieldsRule",
                Type = "Doddle.Import.RequiredFieldsRule, Doddle.Import, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6f5f0fd458d019c9"
            };

            RuleElement missingHeadersElement = new RuleElement
            {
                Name = "MissingHeadersRule",
                Type = "Doddle.Import.MissingHeadersRule, Doddle.Import, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6f5f0fd458d019c9"
            };

            RuleElement dataTypeElement = new RuleElement
            {
                Name = "DataTypeValidationRule",
                Type = "Doddle.Import.DataTypeValidationRule, Doddle.Import, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6f5f0fd458d019c9"
            };

            BaseAdd(requiredFieldsElement);
            BaseAdd(missingHeadersElement);
            BaseAdd(dataTypeElement);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuleElement) element).Name;
        }

        public new RuleElement this[string name]
        {
            get
            {
                return ((RuleElement)BaseGet(name));
            }
        }
    }
}