namespace SAC.Stock.Front.InitializeDatabase.BaseInitializer.AreaCategorySubCategoryContainerProduct
{    
    using System.Configuration;
    using System.IO;    

    internal class AreaCategorySubCategoryContainerProductImporter
    {
        public static void Run()
        {
            var pathToProductFile = ConfigurationManager.AppSettings["PathToProductFile"];
            if (!File.Exists(pathToProductFile))
            {
                return;
            }

           (new ImportAreaCategorySubCategoryContainerProduct()).Run(pathToProductFile);
        }
    }
}