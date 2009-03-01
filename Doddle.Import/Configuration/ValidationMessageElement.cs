using System.Configuration;

namespace Doddle.Importing.Configuration
{
    public class ValidationMessageElement : ConfigurationElement
    {
        [ConfigurationProperty("rule", IsKey = true)]
        public string Rule
        {
            get { return (string)this["rule"]; }
            set { this["rule"] = value; }
        }

        [ConfigurationProperty("message")]
        public string Message
        {
            get { return (string)this["message"]; }
            set { this["message"] = value; }
        }
    }
}