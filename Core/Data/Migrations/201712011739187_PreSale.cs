namespace SAC.Stock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreSale : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stk.Sale", "PreSale", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Stk.Sale", "PreSale");
        }
    }
}
