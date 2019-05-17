namespace SAC.Stock.Domain.StaffContext
{
    using SAC.Seed.NLayer.Domain;
    using SAC.Stock.Domain.BranchOfficeContext;
    using SAC.Stock.Domain.PersonContext;
    using System.Collections.Generic;
    public class Staff : EntityGuid 
    {
        public virtual Person Person { get; set; }
        public virtual ICollection<BranchOfficeStaff> BranchOfficeStaff { get; set; }
    }
}