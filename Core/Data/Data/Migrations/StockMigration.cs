using SAC.Seed.NLayer.Data.EntityFramework;
using SAC.Stock.Data.Context;
using SAC.Stock.Migrations;

namespace SAC.Stock.Data.Migrations
{
    internal class StockMigration : EfMigrateDatabaseToLatestVersion<StockContext, Configuration>
    {
    }
}
