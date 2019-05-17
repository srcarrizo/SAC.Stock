namespace SAC.Stock.Data.Context
{
    using System.Data.Entity;
    using Domain.ProductContext;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Seed.NLayer.Data.EntityFramework;
    using EntityTypeConfiguration;
    using Domain.CustomerContext;
    using Domain.LocationContext;
    using Domain.PersonContext;
    using Domain.PhoneContext;
    using Domain.ProviderContext;    
    using Domain.BranchOfficeContext;
    using Domain.SaleContext;
    using Domain.BudgetContext;
    using Domain.StaffContext;
    using Domain.AttributeValueContext;
    using Domain.BillContext;
    using Domain.BuyContext;
    using Domain.TransactionContext;
    using Domain.BoxContext;
    using Domain.ProfileContext;
    using Domain.StockContext;

    internal class StockContext : EfContextBase
    {
        #region AttributeValue
        public virtual DbSet<AttributeValue> AttributeValue { get; set; }
        #endregion

        #region Bill
        public virtual DbSet<Bill> Bill { get; set; }
        public virtual DbSet<BillUnitType> BillUnitType { get; set; }
        #endregion

        #region Box
        public virtual DbSet<Box> Box { get; set; }
        public virtual DbSet<BoxDetail> BoxDetail { get; set; }
        #endregion

        #region BranchOffice
        public virtual DbSet<BranchOffice> BranchOffice { get; set; }
        public virtual DbSet<BranchOfficePhone> BranchOfficePhone { get; set; }
        public virtual DbSet<BranchOfficeStaff> BranchOfficeStaff { get; set; }
        #endregion

        #region Budget
        public virtual DbSet<Budget> Budget { get; set; }
        public virtual DbSet<BudgetDetail> BudgetDetail { get; set; }
        #endregion

        #region Buy
        public virtual DbSet<Buy> Buy { get; set; }
        public virtual DbSet<BuyDetail> BuyDetail { get; set; }
        #endregion

        #region Customer
        public virtual DbSet<Customer> Customer { get; set; }
        #endregion             

        #region Location
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        #endregion

        #region Person
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonPhone> PersonPhone { get; set; }
        public virtual DbSet<PersonAttributeValue> PersonAttributeValue { get; set; }
        #endregion

        #region Phone
        public virtual DbSet<Phone> Phone { get; set; }
        public virtual DbSet<Telco> Telco { get; set; }
        #endregion

        #region Product
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<SubCategory> SubCategory { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductPrice> ProductPrice { get; set; }
        #endregion

        #region Provinder
        public virtual DbSet<Provider> Provider { get; set; }
        #endregion

        #region Sale
        public virtual DbSet<Sale> Sale { get; set; }
        public virtual DbSet<SaleDetail> SaleDetail { get; set; }
        #endregion

        #region Staff
        public virtual DbSet<Staff> Staff { get; set; }
        #endregion

        #region Stock
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<StockDetail> StockDetail { get; set; }        
        #endregion

        #region Transaction
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionDetail> TransactionDetail { get; set; }
        #endregion

        #region Roles
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<RolesComposition> RolesComposition { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {            
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.HasDefaultSchema("Stk");
         
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new ProviderConfiguration());
            modelBuilder.Configurations.Add(new StaffConfiguration());           

            var indexName = this.GetIndexName<Person>(x => new { x.UidCode, x.UidSerie });
            modelBuilder.Entity<Person>()
              .Property(e => e.UidCode)
              .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute(indexName) { Order = 1, IsUnique = true }));

            modelBuilder.Entity<Person>()
              .Property(e => e.UidSerie)
              .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute(indexName) { Order = 2, IsUnique = true }));

            modelBuilder.Entity<BillUnitType>()
              .Property(e => e.Code)
              .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() { Order = 1, IsUnique = true }));

            modelBuilder.Entity<Bill>()
              .Property(e => e.Code)
              .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() { Order = 1, IsUnique = true }));

            base.OnModelCreating(modelBuilder);
        }       
    }
}