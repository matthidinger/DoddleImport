using System.Configuration;

namespace Doddle.Import.Configuration
{
    public class ValidationMessageElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("value")]
        public string Value
        {
            get { return (string)this["value"]; }
        }
    }
}