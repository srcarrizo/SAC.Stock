namespace SAC.Stock.Code.Tables
{
    using Seed.CodeTable;
    public class ApplicationTable: CodeTable
    {
        public readonly CodeItem Stock = new CodeItem { Code = Code.Stock, Name = "Stock", Description = "Control de stock y caja" };
        public struct Code
        {            
            public const string Stock = "SAC.Stock";
        }
    }
}
