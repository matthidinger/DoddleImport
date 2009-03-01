using System.Configuration;

namespace Doddle.Importing.Configuration
{
    public static class Config
    {
        public static ImportSection Import
        {
            get
            {
                ImportSection section = ConfigurationManager.GetSection("doddleImport") as ImportSection;
                return section ?? new ImportSection();
            }
        }
    }
}