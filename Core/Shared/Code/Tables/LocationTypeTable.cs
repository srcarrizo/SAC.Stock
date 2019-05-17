namespace SAC.Stock.Code.Tables
{
    using Seed.CodeTable;
    public class LocationTypeTable : CodeTable
    {
        public readonly CodeItemAttribute<CodeTable> Country = new CodeItemAttribute<CodeTable> { Code = "Country", Name = "País" };

        public readonly CodeItemAttribute<CodeTable> State = new CodeItemAttribute<CodeTable> { Code = "State", Name = "Provincia" };

        public readonly CodeItemAttribute<CodeTable> City = new CodeItemAttribute<CodeTable> { Code = "City", Name = "Ciudad" };
    }
}
