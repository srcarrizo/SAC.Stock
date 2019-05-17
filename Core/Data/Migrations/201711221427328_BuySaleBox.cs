namespace SAC.Stock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuySaleBox : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stk.Buy", "Boxed", c => c.Boolean(nullable: false));
            AddColumn("Stk.Sale", "Boxed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Stk.Sale", "Boxed");
            DropColumn("Stk.Buy", "Boxed");
        }
    }
}
