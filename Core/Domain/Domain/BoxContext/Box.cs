namespace SAC.Stock.Domain.BoxContext
{
    using SAC.Seed.NLayer.Domain;
    using SAC.Stock.Domain.BuyContext;
    using SAC.Stock.Domain.SaleContext;
    using SAC.Stock.Domain.TransactionContext;
    using System;
    using System.Collections.Generic;
    public class Box : EntityAutoInc
    {
        public DateTimeOffset? OpenDate { get; set; }
        public string OpenNote { get; set; }
        public DateTimeOffset? CloseDate { get; set; }
        public string CloseNote { get; set; }
        public DateTimeOffset? DeactivateDate { get; set; }
        public string DeactivateNote { get; set; }
        public decimal? Withdrawal { get; set; }
        public virtual ICollection<BoxDetail> Detail { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Buy> Buys { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
