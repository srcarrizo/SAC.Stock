namespace SAC.Stock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Stk.Address",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Street = c.String(nullable: false),
                        StreetNumber = c.String(nullable: false),
                        Floor = c.String(),
                        Apartment = c.String(),
                        Neighborhood = c.String(),
                        ZipCode = c.String(),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Location", t => t.LocationId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "Stk.Location",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 100),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        LocationTypeCode = c.String(),
                        ParentLocationId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Location", t => t.ParentLocationId)
                .Index(t => t.ParentLocationId);
            
            CreateTable(
                "Stk.Area",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Stk.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        AreaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Area", t => t.AreaId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "Stk.SubCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Category", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "Stk.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        DisabledDate = c.DateTimeOffset(precision: 7),
                        DisableNote = c.String(),
                        ForSale = c.Boolean(nullable: false),
                        SubCategoryId = c.Int(nullable: false),
                        ContainerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Container", t => t.ContainerId)
                .ForeignKey("Stk.SubCategory", t => t.SubCategoryId)
                .Index(t => t.SubCategoryId)
                .Index(t => t.ContainerId);
            
            CreateTable(
                "Stk.Container",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Amount = c.Int(nullable: false),
                        ParentContainerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Container", t => t.ParentContainerId)
                .Index(t => t.ParentContainerId);
            
            CreateTable(
                "Stk.ProductPrice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BuyMayorPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MayorGainPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinorGainPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        UserId = c.Guid(nullable: false),
                        DisabledDate = c.DateTimeOffset(precision: 7),
                        DisableNote = c.String(),
                        DeactivatorUserId = c.Guid(),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Product", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Stk.AttributeValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttributeCode = c.String(),
                        Value = c.String(),
                        PersonId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Person", t => t.PersonId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "Stk.Bill",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 200),
                        Name = c.String(),
                        Value = c.Int(nullable: false),
                        BillUnitTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.BillUnitType", t => t.BillUnitTypeId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.BillUnitTypeId);
            
            CreateTable(
                "Stk.BillUnitType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 200),
                        Name = c.String(nullable: false, maxLength: 200),
                        IsDecimal = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "Stk.Box",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OpenDate = c.DateTimeOffset(precision: 7),
                        OpenNote = c.String(),
                        CloseDate = c.DateTimeOffset(precision: 7),
                        CloseNote = c.String(),
                        DeactivateDate = c.DateTimeOffset(precision: 7),
                        DeactivateNote = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Stk.BoxDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        BillId = c.Int(nullable: false),
                        BoxId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Bill", t => t.BillId)
                .ForeignKey("Stk.Box", t => t.BoxId)
                .Index(t => t.BillId)
                .Index(t => t.BoxId);
            
            CreateTable(
                "Stk.Transaction",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TransactionDate = c.DateTimeOffset(nullable: false, precision: 7),
                        DeactivatedDate = c.DateTimeOffset(precision: 7),
                        DeactivatedNote = c.String(),
                        BranchOfficeId = c.Guid(nullable: false),
                        BranchOfficeStaffId = c.Guid(nullable: false),
                        TransactionTypeInOut = c.Boolean(nullable: false),
                        BuyId = c.Int(),
                        SaleId = c.Int(),
                        BoxId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Box", t => t.BoxId)
                .ForeignKey("Stk.Buy", t => t.BuyId)
                .ForeignKey("Stk.Sale", t => t.SaleId)
                .ForeignKey("Stk.BranchOffice", t => t.BranchOfficeId)
                .ForeignKey("Stk.BranchOfficeStaff", t => t.BranchOfficeStaffId)
                .Index(t => t.BranchOfficeId)
                .Index(t => t.BranchOfficeStaffId)
                .Index(t => t.BuyId)
                .Index(t => t.SaleId)
                .Index(t => t.BoxId);
            
            CreateTable(
                "Stk.BranchOffice",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        AddressId = c.Int(),
                        ActivatedDate = c.DateTimeOffset(precision: 7),
                        DeactivatedDate = c.DateTimeOffset(precision: 7),
                        DeactivateNote = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Address", t => t.AddressId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "Stk.Buy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BuyDate = c.DateTimeOffset(nullable: false, precision: 7),
                        DeactivatedDate = c.DateTimeOffset(precision: 7),
                        DeactivatedNote = c.String(),
                        BranchOfficeId = c.Guid(nullable: false),
                        ProviderId = c.Guid(nullable: false),
                        BranchOfficeStaffId = c.Guid(nullable: false),
                        PaymentTypeCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.BranchOffice", t => t.BranchOfficeId)
                .ForeignKey("Stk.BranchOfficeStaff", t => t.BranchOfficeStaffId)
                .ForeignKey("Stk.Provider", t => t.ProviderId)
                .Index(t => t.BranchOfficeId)
                .Index(t => t.ProviderId)
                .Index(t => t.BranchOfficeStaffId);
            
            CreateTable(
                "Stk.BranchOfficeStaff",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeactivatedDate = c.DateTimeOffset(precision: 7),
                        DeactivateNote = c.String(),
                        StaffId = c.Guid(nullable: false),
                        BranchOfficeId = c.Guid(nullable: false),
                        StaffRoleCode = c.String(),
                        UserId = c.Guid(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.BranchOffice", t => t.BranchOfficeId)
                .ForeignKey("Stk.Staff", t => t.StaffId)
                .Index(t => t.StaffId)
                .Index(t => t.BranchOfficeId);
            
            CreateTable(
                "Stk.Staff",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Person", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "Stk.Person",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        GenderCode = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 200),
                        UidSerie = c.String(nullable: false, maxLength: 100),
                        UidCode = c.String(nullable: false, maxLength: 50),
                        AddressId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Address", t => t.AddressId)
                .Index(t => new { t.UidCode, t.UidSerie }, unique: true, name: "__IX_Person_UidCode_UidSerie")
                .Index(t => t.AddressId);
            
            CreateTable(
                "Stk.Customer",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        DeativatedDate = c.DateTimeOffset(precision: 7),
                        DeativateNote = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Person", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "Stk.Phone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryCode = c.String(nullable: false),
                        AreaCode = c.String(nullable: false),
                        Number = c.String(nullable: false),
                        Mobile = c.Boolean(),
                        Name = c.String(),
                        TelcoId = c.Int(),
                        PersonId = c.Guid(),
                        BranchOfficeId = c.Guid(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Person", t => t.PersonId)
                .ForeignKey("Stk.BranchOffice", t => t.BranchOfficeId)
                .ForeignKey("Stk.Telco", t => t.TelcoId)
                .Index(t => t.TelcoId)
                .Index(t => t.PersonId)
                .Index(t => t.BranchOfficeId);
            
            CreateTable(
                "Stk.Telco",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Stk.Provider",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        DeativatedDate = c.DateTimeOffset(precision: 7),
                        DeativateNote = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Person", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "Stk.BuyDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        BuyId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Buy", t => t.BuyId)
                .ForeignKey("Stk.Product", t => t.ProductId)
                .Index(t => t.BuyId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Stk.Sale",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SaleDate = c.DateTimeOffset(nullable: false, precision: 7),
                        DeactivatedDate = c.DateTimeOffset(precision: 7),
                        DeactivatedNote = c.String(),
                        BranchOfficeId = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        BranchOfficeStaffId = c.Guid(nullable: false),
                        PaymentTypeCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.BranchOffice", t => t.BranchOfficeId)
                .ForeignKey("Stk.BranchOfficeStaff", t => t.BranchOfficeStaffId)
                .ForeignKey("Stk.Customer", t => t.CustomerId)
                .Index(t => t.BranchOfficeId)
                .Index(t => t.CustomerId)
                .Index(t => t.BranchOfficeStaffId);
            
            CreateTable(
                "Stk.SaleDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        SaleId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Product", t => t.ProductId)
                .ForeignKey("Stk.Sale", t => t.SaleId)
                .Index(t => t.SaleId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Stk.TransactionDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Amount = c.Int(nullable: false),
                        BillId = c.Int(nullable: false),
                        TransactionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Bill", t => t.BillId)
                .ForeignKey("Stk.Transaction", t => t.TransactionId)
                .Index(t => t.BillId)
                .Index(t => t.TransactionId);
            
            CreateTable(
                "Stk.Budget",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BudgetDate = c.DateTimeOffset(nullable: false, precision: 7),
                        BranchOfficeId = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        BranchOfficeStaffId = c.Guid(nullable: false),
                        PaymentTypeCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.BranchOffice", t => t.BranchOfficeId)
                .ForeignKey("Stk.BranchOfficeStaff", t => t.BranchOfficeStaffId)
                .ForeignKey("Stk.Customer", t => t.CustomerId)
                .Index(t => t.BranchOfficeId)
                .Index(t => t.CustomerId)
                .Index(t => t.BranchOfficeStaffId);
            
            CreateTable(
                "Stk.BudgetDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        BudgetId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Budget", t => t.BudgetId)
                .ForeignKey("Stk.Product", t => t.ProductId)
                .Index(t => t.BudgetId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Stk.Profile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 200),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Hierarchy = c.Int(nullable: false),
                        Scope = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Stk.RolesComposition",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfileId = c.Int(nullable: false),
                        RoleCode = c.String(),
                        CriticalRole = c.Boolean(nullable: false),
                        Hierarchy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Profile", t => t.ProfileId)
                .Index(t => t.ProfileId);
            
            CreateTable(
                "Stk.Stock",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StockDate = c.DateTimeOffset(nullable: false, precision: 7),
                        DeactivatedDate = c.DateTimeOffset(precision: 7),
                        DeactivatedNote = c.String(),
                        BranchOfficeId = c.Guid(),
                        BranchOfficeStaffId = c.Guid(),
                        UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.BranchOffice", t => t.BranchOfficeId)
                .ForeignKey("Stk.BranchOfficeStaff", t => t.BranchOfficeStaffId)
                .Index(t => t.BranchOfficeId)
                .Index(t => t.BranchOfficeStaffId);
            
            CreateTable(
                "Stk.StockDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        StockId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Stk.Product", t => t.ProductId)
                .ForeignKey("Stk.Stock", t => t.StockId)
                .Index(t => t.ProductId)
                .Index(t => t.StockId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Stk.StockDetail", "StockId", "Stk.Stock");
            DropForeignKey("Stk.StockDetail", "ProductId", "Stk.Product");
            DropForeignKey("Stk.Stock", "BranchOfficeStaffId", "Stk.BranchOfficeStaff");
            DropForeignKey("Stk.Stock", "BranchOfficeId", "Stk.BranchOffice");
            DropForeignKey("Stk.RolesComposition", "ProfileId", "Stk.Profile");
            DropForeignKey("Stk.Phone", "TelcoId", "Stk.Telco");
            DropForeignKey("Stk.BudgetDetail", "ProductId", "Stk.Product");
            DropForeignKey("Stk.BudgetDetail", "BudgetId", "Stk.Budget");
            DropForeignKey("Stk.Budget", "CustomerId", "Stk.Customer");
            DropForeignKey("Stk.Budget", "BranchOfficeStaffId", "Stk.BranchOfficeStaff");
            DropForeignKey("Stk.Budget", "BranchOfficeId", "Stk.BranchOffice");
            DropForeignKey("Stk.TransactionDetail", "TransactionId", "Stk.Transaction");
            DropForeignKey("Stk.TransactionDetail", "BillId", "Stk.Bill");
            DropForeignKey("Stk.Transaction", "BranchOfficeStaffId", "Stk.BranchOfficeStaff");
            DropForeignKey("Stk.Transaction", "BranchOfficeId", "Stk.BranchOffice");
            DropForeignKey("Stk.Transaction", "SaleId", "Stk.Sale");
            DropForeignKey("Stk.SaleDetail", "SaleId", "Stk.Sale");
            DropForeignKey("Stk.SaleDetail", "ProductId", "Stk.Product");
            DropForeignKey("Stk.Sale", "CustomerId", "Stk.Customer");
            DropForeignKey("Stk.Sale", "BranchOfficeStaffId", "Stk.BranchOfficeStaff");
            DropForeignKey("Stk.Sale", "BranchOfficeId", "Stk.BranchOffice");
            DropForeignKey("Stk.Phone", "BranchOfficeId", "Stk.BranchOffice");
            DropForeignKey("Stk.Transaction", "BuyId", "Stk.Buy");
            DropForeignKey("Stk.Buy", "ProviderId", "Stk.Provider");
            DropForeignKey("Stk.BuyDetail", "ProductId", "Stk.Product");
            DropForeignKey("Stk.BuyDetail", "BuyId", "Stk.Buy");
            DropForeignKey("Stk.Buy", "BranchOfficeStaffId", "Stk.BranchOfficeStaff");
            DropForeignKey("Stk.Staff", "Id", "Stk.Person");
            DropForeignKey("Stk.Provider", "Id", "Stk.Person");
            DropForeignKey("Stk.Phone", "PersonId", "Stk.Person");
            DropForeignKey("Stk.Customer", "Id", "Stk.Person");
            DropForeignKey("Stk.AttributeValue", "PersonId", "Stk.Person");
            DropForeignKey("Stk.Person", "AddressId", "Stk.Address");
            DropForeignKey("Stk.BranchOfficeStaff", "StaffId", "Stk.Staff");
            DropForeignKey("Stk.BranchOfficeStaff", "BranchOfficeId", "Stk.BranchOffice");
            DropForeignKey("Stk.Buy", "BranchOfficeId", "Stk.BranchOffice");
            DropForeignKey("Stk.BranchOffice", "AddressId", "Stk.Address");
            DropForeignKey("Stk.Transaction", "BoxId", "Stk.Box");
            DropForeignKey("Stk.BoxDetail", "BoxId", "Stk.Box");
            DropForeignKey("Stk.BoxDetail", "BillId", "Stk.Bill");
            DropForeignKey("Stk.Bill", "BillUnitTypeId", "Stk.BillUnitType");
            DropForeignKey("Stk.Product", "SubCategoryId", "Stk.SubCategory");
            DropForeignKey("Stk.ProductPrice", "ProductId", "Stk.Product");
            DropForeignKey("Stk.Product", "ContainerId", "Stk.Container");
            DropForeignKey("Stk.Container", "ParentContainerId", "Stk.Container");
            DropForeignKey("Stk.SubCategory", "CategoryId", "Stk.Category");
            DropForeignKey("Stk.Category", "AreaId", "Stk.Area");
            DropForeignKey("Stk.Address", "LocationId", "Stk.Location");
            DropForeignKey("Stk.Location", "ParentLocationId", "Stk.Location");
            DropIndex("Stk.StockDetail", new[] { "StockId" });
            DropIndex("Stk.StockDetail", new[] { "ProductId" });
            DropIndex("Stk.Stock", new[] { "BranchOfficeStaffId" });
            DropIndex("Stk.Stock", new[] { "BranchOfficeId" });
            DropIndex("Stk.RolesComposition", new[] { "ProfileId" });
            DropIndex("Stk.BudgetDetail", new[] { "ProductId" });
            DropIndex("Stk.BudgetDetail", new[] { "BudgetId" });
            DropIndex("Stk.Budget", new[] { "BranchOfficeStaffId" });
            DropIndex("Stk.Budget", new[] { "CustomerId" });
            DropIndex("Stk.Budget", new[] { "BranchOfficeId" });
            DropIndex("Stk.TransactionDetail", new[] { "TransactionId" });
            DropIndex("Stk.TransactionDetail", new[] { "BillId" });
            DropIndex("Stk.SaleDetail", new[] { "ProductId" });
            DropIndex("Stk.SaleDetail", new[] { "SaleId" });
            DropIndex("Stk.Sale", new[] { "BranchOfficeStaffId" });
            DropIndex("Stk.Sale", new[] { "CustomerId" });
            DropIndex("Stk.Sale", new[] { "BranchOfficeId" });
            DropIndex("Stk.BuyDetail", new[] { "ProductId" });
            DropIndex("Stk.BuyDetail", new[] { "BuyId" });
            DropIndex("Stk.Provider", new[] { "Id" });
            DropIndex("Stk.Phone", new[] { "BranchOfficeId" });
            DropIndex("Stk.Phone", new[] { "PersonId" });
            DropIndex("Stk.Phone", new[] { "TelcoId" });
            DropIndex("Stk.Customer", new[] { "Id" });
            DropIndex("Stk.Person", new[] { "AddressId" });
            DropIndex("Stk.Person", "__IX_Person_UidCode_UidSerie");
            DropIndex("Stk.Staff", new[] { "Id" });
            DropIndex("Stk.BranchOfficeStaff", new[] { "BranchOfficeId" });
            DropIndex("Stk.BranchOfficeStaff", new[] { "StaffId" });
            DropIndex("Stk.Buy", new[] { "BranchOfficeStaffId" });
            DropIndex("Stk.Buy", new[] { "ProviderId" });
            DropIndex("Stk.Buy", new[] { "BranchOfficeId" });
            DropIndex("Stk.BranchOffice", new[] { "AddressId" });
            DropIndex("Stk.Transaction", new[] { "BoxId" });
            DropIndex("Stk.Transaction", new[] { "SaleId" });
            DropIndex("Stk.Transaction", new[] { "BuyId" });
            DropIndex("Stk.Transaction", new[] { "BranchOfficeStaffId" });
            DropIndex("Stk.Transaction", new[] { "BranchOfficeId" });
            DropIndex("Stk.BoxDetail", new[] { "BoxId" });
            DropIndex("Stk.BoxDetail", new[] { "BillId" });
            DropIndex("Stk.BillUnitType", new[] { "Code" });
            DropIndex("Stk.Bill", new[] { "BillUnitTypeId" });
            DropIndex("Stk.Bill", new[] { "Code" });
            DropIndex("Stk.AttributeValue", new[] { "PersonId" });
            DropIndex("Stk.ProductPrice", new[] { "ProductId" });
            DropIndex("Stk.Container", new[] { "ParentContainerId" });
            DropIndex("Stk.Product", new[] { "ContainerId" });
            DropIndex("Stk.Product", new[] { "SubCategoryId" });
            DropIndex("Stk.SubCategory", new[] { "CategoryId" });
            DropIndex("Stk.Category", new[] { "AreaId" });
            DropIndex("Stk.Location", new[] { "ParentLocationId" });
            DropIndex("Stk.Address", new[] { "LocationId" });
            DropTable("Stk.StockDetail");
            DropTable("Stk.Stock");
            DropTable("Stk.RolesComposition");
            DropTable("Stk.Profile");
            DropTable("Stk.BudgetDetail");
            DropTable("Stk.Budget");
            DropTable("Stk.TransactionDetail");
            DropTable("Stk.SaleDetail");
            DropTable("Stk.Sale");
            DropTable("Stk.BuyDetail");
            DropTable("Stk.Provider");
            DropTable("Stk.Telco");
            DropTable("Stk.Phone");
            DropTable("Stk.Customer");
            DropTable("Stk.Person");
            DropTable("Stk.Staff");
            DropTable("Stk.BranchOfficeStaff");
            DropTable("Stk.Buy");
            DropTable("Stk.BranchOffice");
            DropTable("Stk.Transaction");
            DropTable("Stk.BoxDetail");
            DropTable("Stk.Box");
            DropTable("Stk.BillUnitType");
            DropTable("Stk.Bill");
            DropTable("Stk.AttributeValue");
            DropTable("Stk.ProductPrice");
            DropTable("Stk.Container");
            DropTable("Stk.Product");
            DropTable("Stk.SubCategory");
            DropTable("Stk.Category");
            DropTable("Stk.Area");
            DropTable("Stk.Location");
            DropTable("Stk.Address");
        }
    }
}
