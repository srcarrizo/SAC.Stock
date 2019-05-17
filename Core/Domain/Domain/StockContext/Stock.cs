namespace SAC.Stock.Domain.StockContext
{
    using SAC.Stock.Domain.BranchOfficeContext;
    using SAC.Stock.Domain.BuyContext;
    using SAC.Stock.Domain.SaleContext;
    using Seed.NLayer.Domain;    
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class Stock : EntityAutoInc
    {
        [Required]
        public DateTimeOffset StockDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivatedNote { get; set; }        
        public Guid? BranchOfficeId { get; set; }
        public Guid? BranchOfficeStaffId { get; set; }       
        public Guid? UserId { get; set; }

        public virtual BranchOffice BranchOffice { get; set; }
        public virtual BranchOfficeStaff BranchOfficeStaff { get; set; }
        public virtual ICollection<StockDetail> Detail { get; set; }

        public virtual ICollection<Buy> Buys { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
