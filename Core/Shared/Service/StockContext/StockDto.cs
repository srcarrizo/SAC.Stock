namespace SAC.Stock.Service.StockContext
{
    using SAC.Seed.NLayer.Application;
    using SAC.Stock.Service.BranchOfficeContext;
    using SAC.Stock.Service.BuyContext;
    using SAC.Stock.Service.SaleContext;
    using System;
    using System.Collections.Generic;

    internal class StockDto : EntityDto<int>
    {        
        public DateTimeOffset StockDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivatedNote { get; set; }
        public Guid? BranchOfficeId { get; set; }
        public Guid? BranchOfficeStaffId { get; set; }
        public Guid? UserId { get; set; }
        public int ProcsessedBuys { get; set; }
        public int ProcsessedSales { get; set; }
        public int UnProcsessedBuys { get; set; }
        public int UnProcsessedSales { get; set; }
        public int PreSales { get; set; }
        public virtual BranchOfficeDto BranchOffice { get; set; }
        public virtual BranchOfficeStaffDto BranchOfficeStaff { get; set; }
        public virtual ICollection<StockDetailDto> Detail { get; set; }

        public ICollection<SaleDto> Sales { get; set; }
        public ICollection<BuyDto> Buys { get; set; }
    }
}