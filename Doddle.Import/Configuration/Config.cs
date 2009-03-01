using System.Configuration;

namespace Doddle.Import.Configuration
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