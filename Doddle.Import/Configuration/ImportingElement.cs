using System.Configuration;

namespace Doddle.Import.Configuration
{
    public class ImportingElement : ConfigurationElement
    {
        [ConfigurationProperty("validation")]
        public ValidationElement Validation
        {
            get { return (ValidationElement)this["validation"]; }
        }
    }
}