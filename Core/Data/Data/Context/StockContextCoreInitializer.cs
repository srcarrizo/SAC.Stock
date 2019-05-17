namespace SAC.Stock.Data.Context
{
    using Code;
    using System.Data.Entity.Migrations;
    using Domain.PhoneContext;
    using Domain.BillContext;

    internal class StockContextCoreInitializer
    {
        public static void Seed(StockContext context)
        {
            foreach (var telco in CodeConst.Telco.Values())
            {
                context.Telco.AddOrUpdate(t => t.Code, new Telco { Code = telco.Code, Description = telco.Description, Name = telco.Name });
            }

            foreach (var billType in CodeConst.BillUnitType.Values())
            {                
                context.BillUnitType.AddOrUpdate(t => t.Code, new BillUnitType { Code = billType.Code, Name = billType.Name, IsDecimal = billType.Description.Equals("Decimal") });
            }

            context.SaveChanges();
        }
    }
}