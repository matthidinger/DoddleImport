using System.Configuration;

namespace Doddle.Importing.Configuration
{
    public sealed class ImportSection : ConfigurationSection
    {
        [ConfigurationProperty("validation")]
        public ValidationElement Validation
        {
            get { return (ValidationElement)this["validation"]; }
        }
    }
}