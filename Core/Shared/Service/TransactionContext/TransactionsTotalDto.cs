namespace SAC.Stock.Service.TransactionContext
{    
    using System.Collections.Generic;    
    internal class TransactionsTotalDto
    {
        public List<TransactionDto> Transactions { get; set; }        
        public decimal TotalIn { get; set; }
        public decimal TotalOut { get; set; }
    }
}
