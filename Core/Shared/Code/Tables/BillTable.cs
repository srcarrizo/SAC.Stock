namespace SAC.Stock.Code.Tables
{
    using Seed.CodeTable;
    public class BillTable : CodeTable
    {
        public readonly CodeItem Coin1 = new CodeItem { Code = "1", Name = "1 centavo", Description = "Decimal" };
        public readonly CodeItem Coin5 = new CodeItem { Code = "5", Name = "5 centavos", Description = "Decimal" };
        public readonly CodeItem Coin10 = new CodeItem { Code = "10", Name = "10 centavos", Description = "Decimal" };
        public readonly CodeItem Coin25 = new CodeItem { Code = "25", Name = "25 centavos", Description = "Decimal" };
        public readonly CodeItem Coin50 = new CodeItem { Code = "50", Name = "50 centavos", Description = "Decimal" };
        public readonly CodeItem Coin100 = new CodeItem { Code = "1", Name = "1 peso", Description = "Entero" };
        public readonly CodeItem Coin200 = new CodeItem { Code = "2", Name = "2 pesos", Description = "Entero" };
        public readonly CodeItem Coin500 = new CodeItem { Code = "5", Name = "5 pesos", Description = "Entero" };

        public readonly CodeItem Bill2 = new CodeItem { Code = "2", Name = "2 pesos", Description = "Entero" };
        public readonly CodeItem Bill5 = new CodeItem { Code = "5", Name = "5 pesos", Description = "Entero" };
        public readonly CodeItem Bill10 = new CodeItem { Code = "10", Name = "10 pesos", Description = "Entero" };
        public readonly CodeItem Bill20 = new CodeItem { Code = "20", Name = "20 pesos", Description = "Entero" };
        public readonly CodeItem Bill50 = new CodeItem { Code = "50", Name = "50 pesos", Description = "Entero" };
        public readonly CodeItem Bill100 = new CodeItem { Code = "100", Name = "100 pesos", Description = "Entero" };
        public readonly CodeItem Bill200 = new CodeItem { Code = "200", Name = "200 pesos", Description = "Entero" };
        public readonly CodeItem Bill500 = new CodeItem { Code = "500", Name = "500 pesos", Description = "Entero" };
        public readonly CodeItem Bill1000 = new CodeItem { Code = "1000", Name = "1000 pesos", Description = "Entero" };
    } 
}