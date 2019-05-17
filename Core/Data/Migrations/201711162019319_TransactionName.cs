namespace SAC.Stock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionName : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stk.Transaction", "Name", c => c.String(nullable: false));
            AddColumn("Stk.Transaction", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Stk.Transaction", "Description");
            DropColumn("Stk.Transaction", "Name");
        }
    }
}
