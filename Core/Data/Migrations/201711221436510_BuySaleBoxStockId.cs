namespace SAC.Stock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuySaleBoxStockId : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stk.Buy", "BoxId", c => c.Int());
            AddColumn("Stk.Buy", "StockId", c => c.Int());
            AddColumn("Stk.Sale", "BoxId", c => c.Int());
            AddColumn("Stk.Sale", "StockId", c => c.Int());
            CreateIndex("Stk.Buy", "BoxId");
            CreateIndex("Stk.Buy", "StockId");
            CreateIndex("Stk.Sale", "BoxId");
            CreateIndex("Stk.Sale", "StockId");
            AddForeignKey("Stk.Buy", "BoxId", "Stk.Box", "Id");
            AddForeignKey("Stk.Sale", "BoxId", "Stk.Box", "Id");
            AddForeignKey("Stk.Buy", "StockId", "Stk.Stock", "Id");
            AddForeignKey("Stk.Sale", "StockId", "Stk.Stock", "Id");
            DropColumn("Stk.Buy", "Stocked");
            DropColumn("Stk.Buy", "Boxed");
            DropColumn("Stk.Sale", "Stocked");
            DropColumn("Stk.Sale", "Boxed");
        }
        
        public override void Down()
        {
            AddColumn("Stk.Sale", "Boxed", c => c.Boolean(nullable: false));
            AddColumn("Stk.Sale", "Stocked", c => c.Boolean(nullable: false));
            AddColumn("Stk.Buy", "Boxed", c => c.Boolean(nullable: false));
            AddColumn("Stk.Buy", "Stocked", c => c.Boolean(nullable: false));
            DropForeignKey("Stk.Sale", "StockId", "Stk.Stock");
            DropForeignKey("Stk.Buy", "StockId", "Stk.Stock");
            DropForeignKey("Stk.Sale", "BoxId", "Stk.Box");
            DropForeignKey("Stk.Buy", "BoxId", "Stk.Box");
            DropIndex("Stk.Sale", new[] { "StockId" });
            DropIndex("Stk.Sale", new[] { "BoxId" });
            DropIndex("Stk.Buy", new[] { "StockId" });
            DropIndex("Stk.Buy", new[] { "BoxId" });
            DropColumn("Stk.Sale", "StockId");
            DropColumn("Stk.Sale", "BoxId");
            DropColumn("Stk.Buy", "StockId");
            DropColumn("Stk.Buy", "BoxId");
        }
    }
}
