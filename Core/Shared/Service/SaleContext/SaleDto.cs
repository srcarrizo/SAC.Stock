namespace SAC.Stock.Service.SaleContext
{
    using SAC.Seed.NLayer.Application;
    using SAC.Stock.Service.BoxContext;
    using SAC.Stock.Service.BranchOfficeContext;    
    using SAC.Stock.Service.CustomerContext;
    using SAC.Stock.Service.StockContext;
    using SAC.Stock.Service.TransactionContext;
    using System;
    using System.Collections.Generic;
    internal class SaleDto : EntityDto<int>
    {        
        public DateTimeOffset SaleDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivatedNote { get; set; }    
        public BranchOfficeDto BranchOffice { get; set; }        
        public CustomerDto Customer { get; set; }                
        public virtual BranchOfficeStaffDto BranchOfficeStaff { get; set; }
        public string PaymentTypeCode { get; set; }
        public bool PreSale { get; set; }
        public bool MayorMinorSale { get; set; }
        public BoxDto Box { get; set; }
        public StockDto Stock { get; set; }
        public ICollection<SaleDetailDto> Detail { get; set; }
        public ICollection<TransactionDto> Transactions { get; set; }
    }
}
