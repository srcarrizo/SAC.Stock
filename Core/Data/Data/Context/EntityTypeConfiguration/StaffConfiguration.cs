namespace SAC.Stock.Data.Context.EntityTypeConfiguration
{
    using SAC.Stock.Domain.StaffContext;
    using System.Data.Entity.ModelConfiguration;
    public class StaffConfiguration : EntityTypeConfiguration<Staff>
    {
        public StaffConfiguration()
        {
            this.HasRequired(p => p.Person).WithOptional(b => b.Staff);
        }
    }
}
