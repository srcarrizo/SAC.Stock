namespace SAC.Stock.Front.Models.Transaction
{
    using SAC.Stock.Front.Models.Bill;
    using System;    
    public class TransactionDetailDpo
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }        
        public BillDpo Bill { get; set; }        
    }
}