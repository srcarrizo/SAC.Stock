namespace SAC.Stock.Code
{
    public struct SortQuery
    {
        public struct Common
        {            
            public const string Default = "Default";
        }

        public struct Location
        {
            public const string Name = "Name";            
        }

        public struct BillUnitType
        {
            public const string Name = "Name";
        }

        public struct Provider
        {
            public const string LastName = "LastName";
            public const string FirstName = "FirstName";
            public const string Uid = "Uid";
            public const string Email = "Email";            
        }

        public struct Customer
        {
            public const string LastName = "LastName";
            public const string FirstName = "FirstName";
            public const string Uid = "Uid";
            public const string Email = "Email";            
        }

        public struct Container
        {
            public const string Name = "Name";
        }

        public struct Product
        {
            public const string Container = "Container";
            public const string SubCategory = "SubCategory";
            public const string Id = "Id";
            public const string Name = "Name";            
        }

        public struct Area
        {
            public const string Id = "Id";
            public const string Name = "Name";            
        }

        public struct Category
        {
            public const string Area = "Area";            
            public const string Id = "Id";
            public const string Name = "Name";
        }

        public struct SubCategory
        {
            public const string Category = "Category";
            public const string Area = "Area";
            public const string Id = "Id";
            public const string Name = "Name";            
        }

        public struct ProductPrice
        {
            public const string Product = "Product";
            public const string BuyMayorPrice = "BuyMayorPrice";
            public const string MayorGainPercent = "MayorGainPercent";            
            public const string Id = "Id";
            public const string MinorGainPercent = "MinorGainPercent";            
        }


        public struct BranchOffice
        {            
            public const string Name = "Name";
        }

        public struct BranchOfficeStaff
        {
            public const string LastName = "LastName";
            public const string FirstName = "FirstName";
            public const string Uid = "Uid";
            public const string Email = "Email";
            public const string StaffRoleCode = "StaffRoleCode";                      
        }

        public struct Buy
        {
            public const string BuyDate = "BuyDate";            
        }

        public struct Budget
        {
            public const string BudgetDate = "BudgetDate";
        }

        public struct Sale
        {
            public const string SaleDate = "SaleDate";
        }

        public struct Profile
        {
            public const string Id = "Id";
            public const string Code = "Code";
            public const string Scope = "Scope";
            public const string Hierarchy = "Hierarchy";
        }

        public struct RolesComposition
        {
            public const string Id = "Id";
            public const string ProfileId = "ProfileId";
            public const string RoleCode = "RoleCode";
            public const string Hierarchy = "Hierarchy";
            public const string CriticalRole = "CriticalRole";
        }
    }
}
