namespace SAC.Stock.Code.Tables
{
    using SAC.Seed.CodeTable;
    public class GenderTable : CodeTable
    {
        public readonly CodeItem Male = new CodeItem { Code = Code.Male, Name = "Masculino" };        
        public readonly CodeItem Female = new CodeItem { Code = Code.Female, Name = "Femenino" };
        public struct Code
        {            
            public const string Male = "Male";
            public const string Female = "Female";
        }
    }
}
