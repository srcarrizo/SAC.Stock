namespace SAC.Stock.Service.BoxContext
{
    using SAC.Stock.Service.BuyContext;
    using SAC.Stock.Service.SaleContext;
    using SAC.Stock.Service.TransactionContext;        

    internal class BoxSalesBuysTransactionsDto
    {
        public BoxDto Box { get; set; }
        public BuysTotalCountDto UnprocessedBuys {get; set;}
        public SalesTotalCountDto UnprocessedSales { get; set; }
        public TransactionsTotalDto UnprocessedTransactions { get; set; }
    }
}