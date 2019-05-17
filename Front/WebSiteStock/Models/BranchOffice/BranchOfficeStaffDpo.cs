namespace SAC.Stock.Front.Models.BranchOffice
{
    using System;
    using System.Collections.Generic;

    public class BranchOfficeStaffDpo
    {
        public Guid Id { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivateNote { get; set; }
        public Guid StaffId { get; set; }
        public StaffDpo Staff { get; set; }
        public Guid BranchOfficeId { get; set; }
        public BranchOfficeDpo BranchOffice { get; set; }
        public string StaffRoleCode { get; set; }
        public Guid? UserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public List<string> Roles { get; set; }
    }
}