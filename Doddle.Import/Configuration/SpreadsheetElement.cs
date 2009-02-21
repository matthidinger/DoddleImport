using System.Configuration;

namespace Doddle.Import.Configuration
{
    public class SpreadsheetElement : ConfigurationElement
    {
        [ConfigurationProperty("importing", IsRequired = true)]
        public ImportingElement Importing
        {
            get { return (ImportingElement)this["importing"]; }
        }
    }
}