namespace SAC.Stock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Budget_Customer : DbMigration
    {
        public override void Up()
        {
            DropIndex("Stk.Budget", new[] { "CustomerId" });
            AddColumn("Stk.Budget", "NonCustomerName", c => c.String(maxLength: 250));
            AlterColumn("Stk.Budget", "CustomerId", c => c.Guid());
            CreateIndex("Stk.Budget", "CustomerId");
        }
        
        public override void Down()
        {
            DropIndex("Stk.Budget", new[] { "CustomerId" });
            AlterColumn("Stk.Budget", "CustomerId", c => c.Guid(nullable: false));
            DropColumn("Stk.Budget", "NonCustomerName");
            CreateIndex("Stk.Budget", "CustomerId");
        }
    }
}
