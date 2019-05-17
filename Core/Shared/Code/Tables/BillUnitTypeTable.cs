namespace SAC.Stock.Code.Tables
{
    using Seed.CodeTable;
    public class BillUnitTypeTable : CodeTable
    {
        public readonly CodeItem Bill = new CodeItem { Code = "Bill", Name = "Billete", Description = "Entero" };
        public readonly CodeItem Coin = new CodeItem { Code = "Coin", Name = "Moneda", Description = "Decimal" };
        public readonly CodeItem CoinInt = new CodeItem { Code = "CoinInt", Name = "Moneda", Description = "Entero" };
    }
}
