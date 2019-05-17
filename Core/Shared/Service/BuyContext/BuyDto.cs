namespace SAC.Stock.Service.BuyContext
{
    using SAC.Seed.NLayer.Application;
    using SAC.Stock.Service.BoxContext;
    using SAC.Stock.Service.BranchOfficeContext;
    using SAC.Stock.Service.ProviderContext;
    using SAC.Stock.Service.StockContext;
    using SAC.Stock.Service.TransactionContext;
    using System;
    using System.Collections.Generic;

    internal class BuyDto : EntityDto<int>
    {     
        public DateTimeOffset BuyDate { get; set; }  
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivatedNote { get; set; }                
        public BranchOfficeDto BranchOffice { get; set; }                
        public ProviderDto Provider { get; set; }        
        public BranchOfficeStaffDto BranchOfficeStaff { get; set; }
        public string PaymentTypeCode { get; set; }
        public BoxDto Box { get; set; }
        public StockDto Stock { get; set; }
        public ICollection<BuyDetailDto> Detail { get; set; }
        public ICollection<TransactionDto> Transactions { get; set; }
    }
}
