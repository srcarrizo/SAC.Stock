namespace SAC.Stock.Service.BranchOfficeContext
{    
    using System.Collections.Generic;    
    internal class BranchOfficeStaffSaveDto  : BranchOfficeStaffDto
    {
        public BranchOfficeStaffSaveDto()
        {
            Roles = new List<string>();
        }

        public ICollection<string> Roles { get; set; }
    }
}
