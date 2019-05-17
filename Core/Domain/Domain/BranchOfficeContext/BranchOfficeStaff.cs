namespace SAC.Stock.Domain.BranchOfficeContext
{
    using SAC.Seed.NLayer.Domain;
    using SAC.Stock.Domain.StaffContext;
    using System;
    public class BranchOfficeStaff : EntityGuid
    {
        public DateTimeOffset? DeactivatedDate { get; set; }

        public string DeactivateNote { get; set; }

        public Guid StaffId { get; set; }

        public virtual Staff Staff { get; set; }

        public Guid BranchOfficeId { get; set; }

        public virtual BranchOffice BranchOffice { get; set; }

        public string StaffRoleCode { get; set; }

        public Guid? UserId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}