namespace SAC.Stock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuySaleStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stk.Buy", "Stocked", c => c.Boolean(nullable: false));
            AddColumn("Stk.Sale", "Stocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Stk.Sale", "Stocked");
            DropColumn("Stk.Buy", "Stocked");
        }
    }
}
