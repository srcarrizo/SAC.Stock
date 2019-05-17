namespace SAC.Stock.Service.TransactionContext
{
    using SAC.Seed.NLayer.Application;
    using SAC.Stock.Service.BillContext;
    using System;

    internal class TransactionDetailDto : EntityDto<Guid>
    {
        public int Amount { get; set; }
        public BillDto Bill { get; set; }                
        public TransactionDto Transaction { get; set; }
    }
}
