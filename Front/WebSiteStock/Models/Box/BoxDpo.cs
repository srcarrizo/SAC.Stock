namespace SAC.Stock.Front.Models.Box
{
    using SAC.Stock.Front.Models.Buy;
    using SAC.Stock.Front.Models.Sale;
    using SAC.Stock.Front.Models.Transaction;
    using System;
    using System.Collections.Generic;

    public class BoxDpo
    {
        public int Id { get; set; }
        public DateTimeOffset? OpenDate { get; set; }
        public string OpenNote { get; set; }
        public DateTimeOffset? CloseDate { get; set; }
        public string CloseNote { get; set; }
        public DateTimeOffset? DeactivateDate { get; set; }
        public string DeactivateNote { get; set; }
        public decimal? Withdrawal { get; set; }
        public ICollection<BoxDetailDpo> Detail { get; set; }
        public ICollection<TransactionDpo> Transactions { get; set; }
        public ICollection<SaleDpo> Sales { get; set; }
        public ICollection<BuyDpo> Buys { get; set; }
        public bool OpeningClosing { get; set; }
    }
}