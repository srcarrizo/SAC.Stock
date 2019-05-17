namespace SAC.Stock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductPriceDetail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Stk.Sale", "ProductPriceId", "Stk.ProductPrice");
            DropIndex("Stk.Sale", new[] { "ProductPriceId" });
            AddColumn("Stk.SaleDetail", "ProductPriceId", c => c.Int(nullable: false));
            CreateIndex("Stk.SaleDetail", "ProductPriceId");
            AddForeignKey("Stk.SaleDetail", "ProductPriceId", "Stk.ProductPrice", "Id");
            DropColumn("Stk.Sale", "ProductPriceId");
        }
        
        public override void Down()
        {
            AddColumn("Stk.Sale", "ProductPriceId", c => c.Int(nullable: false));
            DropForeignKey("Stk.SaleDetail", "ProductPriceId", "Stk.ProductPrice");
            DropIndex("Stk.SaleDetail", new[] { "ProductPriceId" });
            DropColumn("Stk.SaleDetail", "ProductPriceId");
            CreateIndex("Stk.Sale", "ProductPriceId");
            AddForeignKey("Stk.Sale", "ProductPriceId", "Stk.ProductPrice", "Id");
        }
    }
}
