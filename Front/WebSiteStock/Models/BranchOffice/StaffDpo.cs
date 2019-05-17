namespace SAC.Stock.Front.Models.BranchOffice
{
    using SAC.Stock.Front.Models.Shared;
    using System.Collections.Generic;

    public class StaffDpo
    {
        public PersonDpo Person { get; set; }
        public virtual ICollection<BranchOfficeStaffDpo> BranchOfficeStaff { get; set; }
    }
}