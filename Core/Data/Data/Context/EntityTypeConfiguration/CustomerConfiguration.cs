namespace SAC.Stock.Data.Context.EntityTypeConfiguration
{
    using Domain.CustomerContext;
    using System.Data.Entity.ModelConfiguration;
    internal class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {            
            this.HasRequired(p => p.Person).WithOptional(b => b.Customer);            
        }
    }
}
