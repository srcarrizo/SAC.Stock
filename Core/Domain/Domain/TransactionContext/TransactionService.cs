namespace SAC.Stock.Domain.TransactionContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.Domain;
    using SAC.Stock.Code;
    using SAC.Stock.Domain.BoxContext;
    using SAC.Stock.Service.TransactionContext;
    using System;    
    using System.Collections.Generic;
    using System.Linq;    
    public class TransactionService
    {
        private readonly IDataContext context;
        internal TransactionService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Transaction, Guid> ViewTransaction
        {
            get
            {
                return context.GetView<Transaction, Guid>();
            }
        }

        private IDataView<TransactionDetail, Guid> ViewTransactionDetail
        {
            get
            {
                return context.GetView<TransactionDetail, Guid>();
            }
        }

        public Transaction GetTransaction(Guid transactionId)
        {
            return ViewTransaction.Get(transactionId);
        }

        internal Transaction AddTransaction(TransactionDto transactionDto)
        {
            var transaction = NewTransaction(transactionDto);
            context.ApplyChanges();
            return transaction;
        }

        internal Transaction NewTransaction(TransactionDto transactionDto)
        {
            if (transactionDto.BranchOfficeId == null)
            {
                throw BusinessRulesCode.TransactionWithoutBranchOffice.NewBusinessException();
            }

            if (transactionDto.BranchOfficeStaffId == null)
            {
                throw BusinessRulesCode.TransactionWithoutBranchOfficeStaff.NewBusinessException();
            }

            var transaction = transactionDto.AdaptToTransaction();
            transaction.TransactionDate = DateTime.UtcNow;

            transaction.GenerateNewIdentity();
            ViewTransaction.Add(transaction);
            return transaction;
        }

        internal Transaction ModifyTransaction(TransactionDto transactionDto)
        {
            var transaction = UpdateTransaction(transactionDto);
            context.ApplyChanges();
            return transaction;
        }

        internal Transaction UpdateTransaction(TransactionDto transactionDto)
        {
            var transaction = GetTransaction(transactionDto.Id);
            if (transactionDto.BranchOffice == null)
            {
                throw BusinessRulesCode.TransactionDoesNotExist.NewBusinessException();
            }

            if (transactionDto.BranchOffice == null)
            {
                throw BusinessRulesCode.TransactionWithoutBranchOffice.NewBusinessException();
            }

            if (transactionDto.BranchOfficeStaff == null)
            {
                throw BusinessRulesCode.TransactionWithoutBranchOfficeStaff.NewBusinessException();
            }

            TransactionAdapter.AdaptToTransaction(transactionDto, transaction);             
            var TransactiondetailsToDelete = new List<TransactionDetail>();
            foreach (var detail in transaction.Detail)
            {
                var detailDto = transactionDto.Detail.FirstOrDefault(r => r.Id.Equals(detail.Id));
                if (detailDto != null)
                {
                    TransactionAdapter.AdaptToTransactionDetail(detailDto, detail);
                }

                TransactiondetailsToDelete.Add(detail);                
            }

            if (TransactiondetailsToDelete.Count > 0)
            {
                foreach (var detail in TransactiondetailsToDelete)
                {
                    RemoveTransactionDetail(detail);
                }
            }

            foreach (var detailDto in transactionDto.Detail)   
            {
                if (detailDto.Id == Guid.Empty)
                {
                    transaction.Detail.Add(detailDto.AdaptToTransactionDetail());
                }                
            }
           
            return transaction;
        }

        internal bool DeleteTransaction(TransactionDto transactionInfo)
        {
            var result = RemoveTransaction(transactionInfo.AdaptToTransaction());
            context.ApplyChanges();
            return result;
        }

        internal bool DeleteTransactionDetail(TransactionDetailDto detailDto)
        {
            var result = RemoveTransactionDetail(detailDto.AdaptToTransactionDetail());
            context.ApplyChanges();
            return result;
        }

        internal bool RemoveTransactionDetail(TransactionDetail detail)
        {
            ViewTransactionDetail.Remove(detail);

            return true;
        }

        internal IEnumerable<Transaction> QueryTransactionNotBoxed()
        {
            return ViewTransaction.Query(t => !t.BoxId.HasValue && !t.DeactivatedDate.HasValue);
        }

        internal IEnumerable<Transaction> QueryAllTransaction()
        {
            return ViewTransaction.GetAll();
        }

        internal (IEnumerable<Transaction>, decimal, decimal) QueryTransactionNotBoxedWithTotals()
        {
            var transactions = QueryTransactionNotBoxed();

            var totalIn = from t in transactions
                          from detail in t.Detail
                          where t.TransactionTypeInOut
                          select new { TotalIn = detail.Bill.BillUnitType.IsDecimal ? ((decimal)detail.Amount * detail.Bill.Value) / 100 : (decimal)detail.Amount * detail.Bill.Value };

            var totalOut = from t in transactions
                           from detail in t.Detail
                           where !t.TransactionTypeInOut
                           select new { TotalOut = detail.Bill.BillUnitType.IsDecimal ? ((decimal)detail.Amount * detail.Bill.Value) / 100 : (decimal)detail.Amount * detail.Bill.Value };
           
            return (ViewTransaction.Query(t => !t.BoxId.HasValue && !t.DeactivatedDate.HasValue), totalIn.Sum(d => d.TotalIn), totalOut.Sum(d => d.TotalOut));
        }

        public Transaction ModifyTransactionForBox(Guid transactionId, Box box)
        {
            var transaction = ViewTransaction.Get(transactionId);
            if (transaction == null)
            {
                throw BusinessRulesCode.TransactionDoesNotExist.NewBusinessException();
            }

            transaction.Box = box;
            ViewTransaction.Modify(transaction);

            return transaction;
        }

        private bool RemoveTransaction(Transaction transaction)
        {
            ViewTransaction.Remove(transaction);

            return true;
        }
    }
}