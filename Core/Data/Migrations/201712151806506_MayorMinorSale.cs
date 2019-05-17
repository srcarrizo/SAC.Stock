namespace SAC.Stock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MayorMinorSale : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stk.Sale", "MayorMinorSale", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Stk.Sale", "MayorMinorSale");
        }
    }
}
