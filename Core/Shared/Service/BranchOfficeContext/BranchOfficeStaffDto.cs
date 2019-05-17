namespace SAC.Stock.Service.BranchOfficeContext
{
    using SAC.Seed.NLayer.Application;
    using SAC.Stock.Service.BaseDto;
    using System;    
    internal class BranchOfficeStaffDto : EntityDto<Guid>
    {
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivateNote { get; set; }        
        public StaffDto Staff { get; set; }        
        public BranchOfficeDto BranchOffice { get; set; }
        public string StaffRoleCode { get; set; }        
        public Guid UserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
