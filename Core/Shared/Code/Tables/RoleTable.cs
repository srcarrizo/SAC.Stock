namespace SAC.Stock.Code.Tables
{
    using Seed.CodeTable;
    public class RoleTable : CodeTable
    {
        public readonly CodeItem StockSuperAdmin = new CodeItem
        {
            Code = Code.StockSuperAdmin,
            Name = "Super Administrator",
            Description = "Super Usuario Administrador. "
        };

        public readonly CodeItem BuyManager = new CodeItem
        {
            Code = Code.StockBuyManager,
            Name = "Gestor de Compras",
            Description = "Rol para gestionar compras."
        };

        public readonly CodeItem UserManager = new CodeItem
        {
            Code = Code.StockUserManager,
            Name = "Gestionar usuarios y empleados.",
            Description = "Rol para gestionar usuarios y empleados."
        };

        public readonly CodeItem CustomerManager = new CodeItem
        {
            Code = Code.StockCustomerManager,
            Name = "Gestionar usuarios y empleados.",
            Description = "Rol para gestionar usuarios y empleados."
        };

        public readonly CodeItem ProviderManager = new CodeItem
        {
            Code = Code.StockProviderManager,
            Name = "Gestionar proveedores.",
            Description = "Rol para gestionar proveedores."
        };

        public readonly CodeItem BranchofficeManager = new CodeItem
        {
            Code = Code.StockBranchofficeManager,
            Name = "Gestionar sucursal.",
            Description = "Rol para gestionar sucursales."
        };

        public readonly CodeItem ProductManager = new CodeItem
        {
            Code = Code.StockProductManager,
            Name = "Gestionar producto.",
            Description = "Rol para gestionar productos."
        };

        public readonly CodeItem AreaCategoryManager = new CodeItem
        {
            Code = Code.StockAreaCategoryManager,
            Name = "Gestionar rubro, categorias, subCategorias.",
            Description = "Rol para gestionar rubros, categorias, subCategorias."
        };

        public readonly CodeItem ContainerManager = new CodeItem
        {
            Code = Code.StockContainerManager,
            Name = "Gestionar embases.",
            Description = "Rol para gestionar embases."
        };

        public readonly CodeItem BuysManager = new CodeItem
        {
            Code = Code.StockBuysManager,
            Name = "Gestionar Compras.",
            Description = "Rol para gestionar compras."
        };

        public readonly CodeItem SalesManager = new CodeItem
        {
            Code = Code.StockSalesManager,
            Name = "Gestionar Ventas.",
            Description = "Rol para gestionar ventas."
        };
        
        public readonly CodeItem StockManager = new CodeItem
        {
            Code = Code.StockStockManager,
            Name = "Gestionar Stock.",
            Description = "Rol para gestionar stock."
        };

        public readonly CodeItem BoxManager = new CodeItem
        {
            Code = Code.StockBoxManager,
            Name = "Gestionar Caja.",
            Description = "Rol para gestionar caja."
        };

        public readonly CodeItem TransactionManager = new CodeItem
        {
            Code = Code.StockTransactionManager,
            Name = "Gestionar Gastos Comunes.",
            Description = "Rol para gestionar gastos comunes."
        };

        public readonly CodeItem BudgetManager = new CodeItem
        {
            Code = Code.StockBudgetManager,
            Name = "Gestionar Presupuesto.",
            Description = "Rol para gestionar presupuestos."
        };

        public readonly CodeItem ReportBuySaleViewer = new CodeItem
        {
            Code = Code.StockReportBuySaleViewer,
            Name = "Visor de reporte de Compras / Ventas.",
            Description = "Rol para visualizar el reporte de compras y ventas."
        };

        public readonly CodeItem ReportGainViewer = new CodeItem
        {
            Code = Code.StockReportGainViewer,
            Name = "Visor de reporte de ganancias.",
            Description = "Rol para visualizar el reporte de ganancias."
        };

        public struct Code
        {
            public const string StockSuperAdmin = ApplicationTable.Code.Stock + ".SuperAdmin";
            public const string StockBuyManager = ApplicationTable.Code.Stock + ".BuyManager";            
            public const string StockUserManager = ApplicationTable.Code.Stock + ".UserManager";
            public const string StockCustomerManager = ApplicationTable.Code.Stock + ".CustomerManager";
            public const string StockProviderManager = ApplicationTable.Code.Stock + ".ProviderManager";
            public const string StockBranchofficeManager = ApplicationTable.Code.Stock + ".BranchofficeManager";
            public const string StockProductManager = ApplicationTable.Code.Stock + ".ProductManager";
            public const string StockAreaCategoryManager = ApplicationTable.Code.Stock + ".AreaCategoryManager";
            public const string StockContainerManager = ApplicationTable.Code.Stock + ".ContainerManager";
            public const string StockBuysManager = ApplicationTable.Code.Stock + ".BuysManager";
            public const string StockSalesManager = ApplicationTable.Code.Stock + ".SalesManager";
            public const string StockStockManager = ApplicationTable.Code.Stock + ".StockManager";
            public const string StockBoxManager = ApplicationTable.Code.Stock + ".BoxManager";
            public const string StockTransactionManager = ApplicationTable.Code.Stock + ".TransactionManager";
            public const string StockBudgetManager = ApplicationTable.Code.Stock + ".BudgetManager";
            public const string StockReportBuySaleViewer = ApplicationTable.Code.Stock + ".ReportBuySaleViewer";
            public const string StockReportGainViewer = ApplicationTable.Code.Stock + ".ReportGainViewer";
        }
    }
}