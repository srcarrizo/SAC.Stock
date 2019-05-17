namespace SAC.Stock.Front.WebSiteStock.Models
{
    using SAC.Stock.Code;   
    public static class MenuDefinition
    {
        public static readonly Option[] Options = new[]
                                                {
                                                 new Option
                                                 {
                                                    OptionCode = Code.HomeIndex,
                                                    Text = "Home",
                                                    Action = "Index",
                                                    Controller = "Home",
                                                    //Roles = new[] {
                                                    //    CodeConst.Role.StockSuperAdmin.Code,
                                                    //    CodeConst.Role.AreaCategoryManager.Code,
                                                    //    CodeConst.Role.BoxManager.Code,
                                                    //    CodeConst.Role.BranchofficeManager.Code,
                                                    //    CodeConst.Role.BudgetManager.Code,
                                                    //    CodeConst.Role.BuyManager.Code,
                                                    //    CodeConst.Role.BuysManager.Code,
                                                    //    CodeConst.Role.ContainerManager.Code,
                                                    //    CodeConst.Role.CustomerManager.Code,
                                                    //    CodeConst.Role.ProductManager.Code,
                                                    //    CodeConst.Role.ProviderManager.Code,
                                                    //    CodeConst.Role.ReportBuySaleViewer.Code,
                                                    //    CodeConst.Role.ReportGainViewer.Code,
                                                    //    CodeConst.Role.SalesManager.Code,
                                                    //    CodeConst.Role.StockManager.Code,
                                                    //    CodeConst.Role.TransactionManager.Code,
                                                    //    CodeConst.Role.UserManager.Code
                                                    //}
                                                 },
                                                  new Option
                                                    {
                                                      OptionCode = Code.PersonsBranchOffices,
                                                      Text = "Personas y Sucursales",
                                                      Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.BranchofficeManager.Code,
                                                      CodeConst.Role.UserManager.Code, CodeConst.Role.CustomerManager.Code,
                                                      CodeConst.Role.ProviderManager.Code },
                                                      SubOptions = new Option[]
                                                      {
                                                        new Option
                                                        {
                                                            OptionCode = Code.UserManager,
                                                            Text = "Usuarios / Empleados",
                                                            Action = "Index",
                                                            Controller = "User",
                                                            Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.UserManager.Code }
                                                        },
                                                        new Option
                                                        {
                                                            OptionCode = Code.Customers,
                                                            Text = "Clientes",
                                                            Action = "Index",
                                                            Controller = "Customer",
                                                            Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.CustomerManager.Code }
                                                        },
                                                        new Option
                                                        {
                                                            OptionCode = Code.Providers,
                                                            Text = "Proveedores",
                                                            Action = "Index",
                                                            Controller = "Provider",
                                                            Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.ProviderManager.Code }
                                                        },
                                                        new Option
                                                        {
                                                            OptionCode = Code.Branchoffice,
                                                            Text = "Sucursales",
                                                            Action = "Index",
                                                            Controller = "BranchOffice",
                                                            Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.BranchofficeManager.Code }
                                                        },

                                                      }
                                                  },
                                                  new Option
                                                  {
                                                     OptionCode = Code.ProductManagement,
                                                     Text = "Administracion de Productos",
                                                     Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.AreaCategoryManager.Code,
                                                                     CodeConst.Role.ContainerManager.Code, CodeConst.Role.ProductManager.Code },
                                                     SubOptions = new Option[]
                                                     {
                                                        new Option
                                                        {
                                                            OptionCode = Code.AreaCategory,
                                                            Text = "Rubro / Categorias / SubCategorias",
                                                            Action = "Index",
                                                            Controller = "AreaCategory",
                                                            Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.AreaCategoryManager.Code }
                                                        },
                                                        new Option
                                                        {
                                                            OptionCode = Code.Containers,
                                                            Text = "Contenedores",
                                                            Action = "Index",
                                                            Controller = "Container",
                                                            Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.ContainerManager.Code }
                                                        },
                                                        new Option
                                                        {
                                                            OptionCode = Code.Products,
                                                            Text = "Productos y Precios",
                                                            Action = "Index",
                                                            Controller = "Product",
                                                            Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.ProductManager.Code }
                                                        },
                                                     }
                                                  },
                                                  new Option
                                                    {
                                                      OptionCode = Code.BuysSales,
                                                      Text = "Compras / Ventas",
                                                      Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.BuyManager.Code, CodeConst.Role.SalesManager.Code },
                                                      SubOptions = new Option[]
                                                      {
                                                            new Option
                                                            {
                                                                OptionCode = Code.Buys,
                                                                Text = "Compras",
                                                                Action = "Index",
                                                                Controller = "Buy",
                                                                Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.BuyManager.Code }
                                                            },
                                                            new Option
                                                            {
                                                                OptionCode = Code.Sales,
                                                                Text = "Ventas",
                                                                Action = "Index",
                                                                Controller = "Sale",
                                                                Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.SalesManager.Code }
                                                            }
                                                      }
                                                  },
                                                  new Option
                                                  {
                                                     OptionCode = Code.Administration,
                                                     Text = "Administracion",
                                                     Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.BudgetManager.Code, CodeConst.Role.TransactionManager.Code,
                                                     CodeConst.Role.BoxManager.Code, CodeConst.Role.StockManager.Code},
                                                     SubOptions = new Option[]
                                                     {
                                                        new Option
                                                        {
                                                            OptionCode = Code.Stock,
                                                            Text = "Stock",
                                                            Action = "Index",
                                                            Controller = "Stock",
                                                            Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.StockManager.Code }
                                                        },
                                                        new Option
                                                        {
                                                            OptionCode = Code.Box,
                                                            Text = "Caja",
                                                            Action = "Index",
                                                            Controller = "Box",
                                                            Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.BoxManager.Code }
                                                        },
                                                        new Option
                                                        {
                                                            OptionCode = Code.Transaction,
                                                            Text = "Gastos Comunes",
                                                            Action = "Index",
                                                            Controller = "Transaction",
                                                            Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.TransactionManager.Code }
                                                        },
                                                        new Option
                                                        {
                                                            OptionCode = Code.Budget,
                                                            Text = "Presupuestos",
                                                            Action = "Index",
                                                            Controller = "Budget",
                                                            Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.BudgetManager.Code }
                                                        },
                                                     }
                                                  },
                                                  new Option
                                                  {
                                                     OptionCode = Code.Reports,
                                                     Text = "Reportes",
                                                     Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.ReportBuySaleViewer.Code, CodeConst.Role.ReportGainViewer.Code },
                                                     SubOptions = new Option[]
                                                     {
                                                         new Option
                                                          {
                                                              OptionCode = Code.ReportBuySale,
                                                              Text = "Compras Ventas",
                                                              Action = "Index",
                                                              Controller = "Reporter",
                                                              Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.ReportBuySaleViewer.Code }
                                                          },
                                                          new Option
                                                          {
                                                              OptionCode = Code.ReportGainViewer,
                                                              Text = "Ganancias",
                                                              Action = "Index",
                                                              Controller = "Reporter",
                                                              Roles = new[] { CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.ReportGainViewer.Code }
                                                          },

                                                     }
                                                   }
                                             };
        public struct Code
        {            
            public const string BuysSales = "BuysSales";

            public const string Containers = "Containers";

            public const string AreaCategory = "AreaCategory";  

            public const string ReportBuySale = "ReportBuySale";

            public const string ReportGainViewer = "ReportGainViewer";

            public const string Reports = "Reports";

            public const string Movements = "Movements";

            public const string Transaction = "Transaction";

            public const string Administration = "Administration";

            public const string PersonsBranchOffices = "PersonsBranchOffices";

            public const string Budget = "Budget";

            public const string Branchoffice = "Branchoffice";

            public const string Stock = "Stock";

            public const string UserManager = "UserManager";

            public const string Customers = "Customers";

            public const string Products = "Products";

            public const string Providers = "Providers";

            public const string Sales = "Sales";            

            public const string Buys = "Buys";

            public const string Box = "Box";

            public const string AccountRegister = "AccountRegister";

            public const string HomeIndex = "HomeIndex";

            public const string ProductView = "ProductView";

            public const string ProductManagement = "ProductManagement";

            public const string RegisterProduct = "RegisterProduct";

            public const string OtherOptions = "OtherOptions";

            public const string OtherOptionLocations = "OtherOptionLocations";            

            public const string RolesCompositionManager = "RolesCompositionManager";

            public const string RoleManager = "RoleManager";

            public const string ProfileManager = "ProfileManager";          
        }

        public class Option
        {
            public string OptionCode { get; set; }

            public string Description { get; set; }

            public string ImageUri { get; set; }

            public string ImageToolButtonUri { get; set; }

            public string Text { get; set; }

            public string Controller { get; set; }

            public string Action { get; set; }

            public string[] Roles { get; set; }

            public Option[] SubOptions { get; set; }
        }

    }
}