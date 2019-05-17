namespace SAC.Stock.Front.Models.Transaction
{    
    using System.Collections.Generic;    
    public class TransactionTotalsDpo
    {
        public List<TransactionDpo> Transactions { get; set; }
        public decimal TotalIn { get; set; }
        public decimal TotalOut { get; set; }
    }
}