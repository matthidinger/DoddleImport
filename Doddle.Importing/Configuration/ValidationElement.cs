using System.Configuration;
using Doddle.Importing;
using System;

namespace Doddle.Importing.Configuration
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
        /// Apply Configuration settings to a validator
        /// </summary>
        /// <param name="validator">The validator to apply configuration to</param>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public void ConfigureValidator(IImportValidator validator)
        {
            foreach (RuleElement ruleDefinition in Rules)
            {
                Type ruleType = Type.GetType(ruleDefinition.Type);

                if (ruleType == null)
                {
                    throw new ConfigurationErrorsException(string.Format("Unable to load type '{0}' for rule '{1}'", ruleDefinition.Type, ruleDefinition.Name));
                }

                if (ruleType.GetInterface("Doddle.Importing.IValidationRule") == null)
                {
                    throw new ConfigurationErrorsException(string.Format("The rule '{0}' does not implement interface 'IValidationRule'", ruleDefinition.Name));
                }

                IValidationRule rule = Activator.CreateInstance(ruleType) as IValidationRule;
                if (rule == null)
                {
                    throw new ConfigurationErrorsException(string.Format("Unable to load type '{0}' for rule '{1}'", ruleDefinition.Type, ruleDefinition.Name));
                }

                validator.Rules.Add(ruleDefinition.Name, rule);
            }
        }

        public string GetValidationMessage(string key)
        {
            return Messages[key];
        }
    }
}