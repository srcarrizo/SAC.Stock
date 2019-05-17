namespace SAC.Stock
{
    using Membership.Data.Context;
    using Membership.Data.Migrations;
    using Data.Context;
    using Data.Migrations;
    using System;
    public class Program
    {
        public static void Main()
        {
            try
            {
                var membershipMigration = new MembershipMigration();
                var membershipContext = new MembershipContext();
                membershipMigration.Configure();
                membershipMigration.InitializeDatabase(membershipContext);

                Console.WriteLine("Membership Database migrated OK!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                Console.ReadKey();
            }

            try
            {
                var stockMigration = new StockMigration();
                var stockContext = new StockContext();

                stockMigration.Configure();
                stockMigration.InitializeDatabase(stockContext);
                Console.WriteLine("Stock Database migrated OK!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                Console.ReadKey();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}