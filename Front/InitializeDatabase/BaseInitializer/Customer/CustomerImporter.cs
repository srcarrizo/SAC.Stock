namespace SAC.Stock.Front.InitializeDatabase.BaseInitializer.Customer
{    
    using System.Configuration;
    using System.IO;   
    internal class CustomerImporter
    {
        public static void Run()
        {
            var pathToCustomerFile = ConfigurationManager.AppSettings["PathToCustomerFile"];
            if (!File.Exists(pathToCustomerFile))
            {
                return;
            }

            (new ImportCustomer()).Run(pathToCustomerFile);
        }
    }
}
