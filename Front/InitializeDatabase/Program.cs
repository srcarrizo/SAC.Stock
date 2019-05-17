namespace SAC.Stock.Front.InitializeDatabase
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;    
    using Membership.Data.Context;
    using Membership.Data.Migrations;
    using Data.Migrations;
    using Data.Context;
    using Infrastructure;

    public class Program
    {
        public static void Main()
        {

            var cmd = new SqlCommand
            {
                Connection =
                            new SqlConnection(
                            ConfigurationManager.ConnectionStrings[@"SAC.Stock.Data.Context.StockContext"].ToString()),
                CommandText =
                            @"USE master; "
                            + "ALTER DATABASE [SAC.Stock.DB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; "
                            + "DROP DATABASE [SAC.Stock.DB]; ",
                CommandType = CommandType.Text
            };
            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // NADA. SI FALLA ES PORQUE NO EXISTE Y PUEDE SEGUIR.
            }

            ContainerConfig.Initialize();

            try
            {
                var membershipMigration = new MembershipMigration();
                var membershipContext = new MembershipContext();
                membershipMigration.Configure();
                membershipMigration.InitializeDatabase(membershipContext);

                BaseInitializer.MembershipContextInitializer.Seed(membershipContext);
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

                BaseInitializer.StockContextInitializer.Seed(stockContext);
                Console.WriteLine("Stock Database migrated OK!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                Console.ReadKey();
            }

            Console.WriteLine(@"El proceso de inicializacion se ejecuto de manera exitosa!!");
            Console.ReadLine();
        }
    }
}