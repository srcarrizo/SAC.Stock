namespace SAC.Stock.Data.Context.EntityTypeConfiguration
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ProviderContext;
    public class ProviderConfiguration : EntityTypeConfiguration<Provider>
    {
        public ProviderConfiguration()
        {
            this.HasRequired(p => p.Person).WithOptional(b => b.Provider);
        }
    }
}
