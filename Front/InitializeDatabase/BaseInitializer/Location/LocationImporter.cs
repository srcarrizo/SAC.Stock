namespace SAC.Stock.Front.InitializeDatabase.BaseInitializer.Location
{
    using SAC.Seed.Dependency;
    using SAC.Stock.Service.LocationContext;
    using System.Configuration;
    using System.IO;
    public static class LocationImporter
    {
        public static void Run()
        {
            var supportSvc = DiContainerFactory.DiContainer().Resolve<ILocationApplicationService>();
            var counties = supportSvc.QueryChildrenLocation();
            if (counties.Count > 0)
            {
                return;
            }

            var pathToCountryFile = ConfigurationManager.AppSettings["PathToCountryFile"];
            var pathToStateFolder = ConfigurationManager.AppSettings["PathToStateFolder"];
            var pathToCityFolder = ConfigurationManager.AppSettings["PathToCityFolder"];
            if (!File.Exists(pathToCountryFile) || !Directory.Exists(pathToStateFolder) || !Directory.Exists(pathToCityFolder))
            {
                return;
            }

            (new ImportCountry()).Run(pathToCountryFile);
            (new ImportState()).Run(pathToStateFolder);
            (new ImportCity()).Run(pathToCityFolder);
        }
    }
}
