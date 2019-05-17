namespace SAC.Stock.Service.TransactionContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Stock.Domain.TransactionContext;
    using System.Collections.Generic;
    using System.Linq;

    internal class TransactionApplicationService : ITransactionApplicationService
    {
        public IDataContext StockCtx { get; set; }

        public TransactionDto GetTransaction(TransactionDto transactionInfo)
        {
            var svc = new TransactionService(StockCtx);

            return svc.GetTransaction(transactionInfo.Id).AdaptToTransactionDtoComplete();
        }

        public TransactionDto AddTransaction(TransactionDto transactionInfo)
        {
            var svc = new TransactionService(StockCtx);

            return svc.AddTransaction(transactionInfo).AdaptToTransactionDtoComplete();
        }

        public TransactionDto ModifyTransaction(TransactionDto transactionInfo)
        {
            var svc = new TransactionService(StockCtx);

            return svc.ModifyTransaction(transactionInfo).AdaptToTransactionDtoComplete();
        }

        public bool DeleteTransaction(TransactionDto transactionInfo)
        {
            var svc = new TransactionService(StockCtx);
            return svc.DeleteTransaction(transactionInfo);
        }

        public bool DeleteTransactionDetail(TransactionDetailDto transactionDetailInfo)
        {
            var svc = new TransactionService(StockCtx);
            return svc.DeleteTransactionDetail(transactionDetailInfo);
        }

        public ICollection<TransactionDto> QueryTransactionNotBoxed()
        {
            var svc = new TransactionService(StockCtx);
            return svc.QueryTransactionNotBoxed().Select(t => t.AdaptToTransactionDtoComplete()).ToList();
        }

        public ICollection<TransactionDto> QueryAllTransaction()
        {
            var svc = new TransactionService(StockCtx);
            return svc.QueryAllTransaction().Select(t => t.AdaptToTransactionDtoComplete()).ToList();
        }

        public TransactionsTotalDto QueryTransactionNotBoxedWithTotals()
        {
            var svc = new TransactionService(StockCtx);
            var (transaction, tIn, tOut) = svc.QueryTransactionNotBoxedWithTotals();
            var result = new TransactionsTotalDto
            {
                Transactions = transaction.Select(t => t.AdaptTransactionToDto()).ToList(),
                TotalIn = tIn,
                TotalOut = tOut                
            };

            return result;
        }
    }
}
