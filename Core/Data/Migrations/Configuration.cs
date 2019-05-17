namespace SAC.Stock.Migrations
{    
    using System.Data.Entity.Migrations;
    using Data.Context;

    internal sealed class Configuration : DbMigrationsConfiguration<StockContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;            
        }

        protected override void Seed(StockContext context)
        {
           base.Seed(context);
           StockContextCoreInitializer.Seed(context);
        }
    }
}
