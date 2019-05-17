namespace SAC.Stock.Code
{
    public struct SpecFilter
    {
        public struct Location
        {            
            public const string FullSearch = "FullSearch";

            public const string Country = "Country";

            public const string State = "State";
        }

        public struct BillUnitType
        {
            public const string FullSearch = "FullSearch";

            public const string Name = "Decimal";

            public const string Decimal = "Decimal";
        }

        public struct Provider
        {
            public const string FullSearch = "FullSearch";
            public const string Location = "Location";
            public const string Uid = "Uid";            
        }

        public struct Customer
        {
            public const string FullSearch = "FullSearch";
            public const string Location = "Location";
            public const string Uid = "Uid";            
        }

        public struct Container
        {
            public const string FullSearch = "FullSearch";            
        }

        public struct Product
        {
            public const string FullSearch = "FullSearch";
            public const string Available = "Available";
        }

        public struct Area
        {
            public const string FullSearch = "FullSearch";
        }

        public struct Category
        {
            public const string FullSearch = "FullSearch";
        }

        public struct SubCategory
        {
            public const string FullSearch = "FullSearch";
        }

        public struct ProductPrice
        {
            public const string FullSearch = "FullSearch";
        }

        public struct BranchOfficeStaff
        {
            public const string FullSearch = "FullSearch";
            public const string Name = "Name";
            public const string BranchOfficeId = "BranchOfficeId"; 
        }

        public struct BranchOffice
        {
            public const string FullSearch = "FullSearch";
            public const string Name = "Name";
        }

        public struct Buy
        {
            public const string BuyDate = "BuyDate";
            public const string BuyCustomerUid = "BuyCustomerUid";
            public const string BuyFromDate = "BuyFromDate";
        }

        public struct Budget
        {
            public const string BudgetDate = "BudgetDate";            
        }

        public struct Sale
        {
            public const string SaleDate = "SaleDate";
            public const string SaleCustomerUid = "SaleCustomerUid";
            public const string SaleFromDate = "SaleFromDate";            
        }

        public struct Profile
        {
            public const string Code = "Code";
            public const string Name = "Name";
            public const string Scope = "Scope";
            public const string Hierarchy = "Hierarchy";
            public const string FullSearch = "FullSearch";
        }

        public struct RolesComposition
        {
            public const string ProfileId = "ProfileId";
            public const string RoleCode = "RoleCode";
            public const string CriticalRole = "CriticalRole";
            public const string FullSearch = "FullSearch";
        }
    }
}