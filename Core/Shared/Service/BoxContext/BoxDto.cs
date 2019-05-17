namespace SAC.Stock.Service.BoxContext
{
    using SAC.Seed.NLayer.Application;
    using SAC.Stock.Service.BuyContext;
    using SAC.Stock.Service.SaleContext;
    using SAC.Stock.Service.TransactionContext;
    using System;
    using System.Collections.Generic;    
    internal class BoxDto : EntityDto<int>
    {
        public DateTimeOffset? OpenDate { get; set; }
        public string OpenNote { get; set; }
        public DateTimeOffset? CloseDate { get; set; }
        public string CloseNote { get; set; }
        public DateTimeOffset? DeactivateDate { get; set; }
        public string DeactivateNote { get; set; }
        public decimal? Withdrawal { get; set; }
        public ICollection<BoxDetailDto> Detail { get; set; }        
        public ICollection<TransactionDto> Transactions { get; set; }
        public ICollection<SaleDto> Sales { get; set; }
        public ICollection<BuyDto> Buys { get; set; }
    }
}
