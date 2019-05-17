namespace SAC.Stock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Withdrawal : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stk.Box", "Withdrawal", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Stk.Box", "Withdrawal");
        }
    }
}
