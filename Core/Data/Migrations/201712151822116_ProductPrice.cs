namespace SAC.Stock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stk.Sale", "ProductPriceId", c => c.Int(nullable: false));
            CreateIndex("Stk.Sale", "ProductPriceId");
            AddForeignKey("Stk.Sale", "ProductPriceId", "Stk.ProductPrice", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Stk.Sale", "ProductPriceId", "Stk.ProductPrice");
            DropIndex("Stk.Sale", new[] { "ProductPriceId" });
            DropColumn("Stk.Sale", "ProductPriceId");
        }
    }
}
