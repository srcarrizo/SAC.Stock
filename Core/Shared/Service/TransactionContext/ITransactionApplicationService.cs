namespace SAC.Stock.Service.TransactionContext
{
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract]
    internal interface ITransactionApplicationService
    {
        [OperationContract]
        TransactionDto GetTransaction(TransactionDto transactionInfo);

        [OperationContract]
        TransactionDto AddTransaction(TransactionDto transactionInfo);

        [OperationContract]
        TransactionDto ModifyTransaction(TransactionDto transactionInfo);

        [OperationContract]
        bool DeleteTransaction(TransactionDto transactionInfo);

        [OperationContract]
        bool DeleteTransactionDetail(TransactionDetailDto transactionDetailInfo);

        [OperationContract]
        ICollection<TransactionDto> QueryTransactionNotBoxed();

        [OperationContract]
        TransactionsTotalDto QueryTransactionNotBoxedWithTotals();

        [OperationContract]
        ICollection<TransactionDto> QueryAllTransaction();
    }
}
