namespace SAC.Stock.Code.Tables
{
    using SAC.Seed.CodeTable;
    public class ScopeTable : CodeTable
    {
        public readonly CodeItem SAC = new CodeItem { Code = "SAC", Name = "SAC" };
        public readonly CodeItem BranchOffice = new CodeItem { Code = "BranchOffice", Name = "BranchOffice" };
        public readonly CodeItem Customer = new CodeItem { Code = "Customer", Name = "Customer" };
        public readonly CodeItem Provider = new CodeItem { Code = "Provider", Name = "Provider" };
    }
}
