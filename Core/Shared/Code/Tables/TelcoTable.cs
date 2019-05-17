namespace SAC.Stock.Code.Tables
{
    using Seed.CodeTable;
    public class TelcoTable : CodeTable
    {
        public readonly CodeItem Personal = new CodeItem { Code = "Personal", Name = "Personal" };

        public readonly CodeItem Claro = new CodeItem { Code = "Claro", Name = "Claro" };

        public readonly CodeItem Movistar = new CodeItem { Code = "Movistar", Name = "Movistar" };

        public readonly CodeItem Nextel = new CodeItem { Code = "Nextel", Name = "Nextel" };
    }
}
