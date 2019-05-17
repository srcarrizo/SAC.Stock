namespace SAC.Stock.Front.WebSiteStock
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.        
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/jquery-ui-1.12.1.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                     "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/angular-material.css",                      
                      "~/Content/ui-grid.css",                                                                
                      "~/Content/loading-bar.css",
                      "~/Content/angucomplete-alt.css",                         
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/moment.min.js",
                      "~/Scripts/angular.js",
                      "~/Scripts/angular-mocks.js",
                      "~/Scripts/angular-messages.js",
                      "~/Scripts/angular-route.js", 
                      "~/Scripts/ui-grid.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/angular-moment.js",
                      "~/Scripts/angular-material.js",
                      "~/Scripts/angular-animate.js",
                      "~/Scripts/angular-aria.js",
                      "~/Scripts/angucomplete-alt.js",
                      "~/Scripts/loading-bar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/StockAngular").Include(
                      "~/Scripts/App/StockApp.js",
                      "~/Scripts/App/BaseServices/SAC.Seed.Services.js",
                      "~/Scripts/App/Directives/Autocomplete.js"));

            bundles.Add(new ScriptBundle("~/bundles/LoginController").Include(
                      "~/Scripts/App/AccountController/LoginController.js"));

            bundles.Add(new ScriptBundle("~/bundles/BuyController").Include(
                      "~/Scripts/App/BuyController/BuyController.js"));

            bundles.Add(new ScriptBundle("~/bundles/SaleController").Include(
                      "~/Scripts/App/SaleController/SaleController.js"));

            bundles.Add(new ScriptBundle("~/bundles/ProviderController").Include(
                      "~/Scripts/App/ProviderController/ProviderController.js"));

            bundles.Add(new ScriptBundle("~/bundles/ProductController").Include(
                      "~/Scripts/App/ProductController/ProductController.js"));

            bundles.Add(new ScriptBundle("~/bundles/BranchOfficeController").Include(
                      "~/Scripts/App/BranchOfficeController/BranchOfficeController.js"));

            bundles.Add(new ScriptBundle("~/bundles/BoxController").Include(
                      "~/Scripts/App/BoxController/BoxController.js"));

            bundles.Add(new ScriptBundle("~/bundles/TransactionController").Include(
                     "~/Scripts/App/TransactionController/TransactionController.js"));

            bundles.Add(new ScriptBundle("~/bundles/StockController").Include(
                     "~/Scripts/App/StockController/StockController.js"));

            bundles.Add(new ScriptBundle("~/bundles/CustomerController").Include(
                     "~/Scripts/App/CustomerController/CustomerController.js"));

            bundles.Add(new ScriptBundle("~/bundles/UserController").Include(
                     "~/Scripts/App/UserController/UserController.js"));

            bundles.Add(new ScriptBundle("~/bundles/AreaCategoryController").Include(
                     "~/Scripts/App/AreaCategoryController/AreaCategoryController.js"));

            bundles.Add(new ScriptBundle("~/bundles/ContainerController").Include(
                     "~/Scripts/App/ContainerController/ContainerController.js"));

            bundles.Add(new ScriptBundle("~/bundles/BudgetController").Include(
                "~/Scripts/App/BudgetController/BudgetController.js"));
        }
    }
}
