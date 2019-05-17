namespace SAC.Stock.Migrations
{    
    using System.Data.Entity.Migrations;
    
    public partial class BudgetDetailPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stk.BudgetDetail", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Stk.BudgetDetail", "Price");
        }
    }
}
