namespace SAC.Stock.Front.InitializeDatabase.BaseInitializer.Provider
{    
    using System.Configuration;
    using System.IO;    

    public static class ProviderImporter
    {
        public static void Run()
        {            
            var pathToProviderFile = ConfigurationManager.AppSettings["PathToProviderFile"];         
            if (!File.Exists(pathToProviderFile))
            {
                return;
            }

            (new ImportProvider()).Run(pathToProviderFile);            
        }
    }
}