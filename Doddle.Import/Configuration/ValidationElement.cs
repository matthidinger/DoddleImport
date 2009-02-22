using System.Configuration;
using Doddle.Import;
using System;

namespace Doddle.Import.Configuration
{
    public class ValidationElement : ConfigurationElement
    {
        [ConfigurationProperty("rules", IsRequired = true)]
        public RulesElementCollection Rules
        {
            get { return (RulesElementCollection)this["rules"]; }
        }

        [ConfigurationProperty("messages", IsRequired = true)]
        public ValidationMessagesElementCollection Messages
        {
            get { return (ValidationMessagesElementCollection) this["messages"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validator"></param>
        /// <exception cref="SpreadsheetConfigurationException"></exception>
        public void Configure(IImportValidator validator)
        {
            foreach (RuleElement ruleDefinition in Rules)
            {
                Type ruleType = Type.GetType(ruleDefinition.Type);

                if (ruleType == null)
                {
                    throw new SpreadsheetConfigurationException(string.Format("Unable to load type '{0}' for rule '{1}'", ruleDefinition.Type, ruleDefinition.Name));
                }

                if (ruleType.GetInterface("Doddle.Import.IImportRule") == null)
                {
                    throw new SpreadsheetConfigurationException(string.Format("The rule '{0}' does not implement interface 'IImportRule'", ruleDefinition.Name));
                }

                IImportRule rule = Activator.CreateInstance(ruleType) as IImportRule;
                if (rule == null)
                {
                    throw new SpreadsheetConfigurationException(string.Format("Unable to load type '{0}' for rule '{1}'", ruleDefinition.Type, ruleDefinition.Name));
                }

                validator.Rules.Add(rule);
            }
        }
    }
}