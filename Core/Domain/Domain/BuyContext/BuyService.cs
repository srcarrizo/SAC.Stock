namespace SAC.Stock.Domain.BuyContext
{
    using Seed.NLayer.Data;
    using Seed.NLayer.Data.Ordering;
    using Seed.NLayer.Domain;
    using Seed.NLayer.Domain.Specification;
    using Code;    
    using TransactionContext;
    using Service.BuyContext;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SAC.Stock.Domain.StockContext;
    using SAC.Stock.Domain.BoxContext;

    internal class BuyService
    {
        private readonly IDataContext context;
        internal BuyService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Buy, int> ViewBuy
        {
            get
            {
                return context.GetView<Buy, int>();
            }
        }

        private IDataView<BuyDetail, int> ViewBuyDetail
        {
            get
            {
                return context.GetView<BuyDetail, int>();
            }
        }

        public Buy GetBuy(int buyId)
        {
            return ViewBuy.Get(buyId);
        }

        public Buy AddBuy(BuyDto buyInfo)
        {
            var buy = NewBuy(buyInfo);
            context.ApplyChanges();
            return buy;
        }

        private Buy NewBuy(BuyDto buyInfo)
        {
            if (buyInfo.BranchOffice == null)
            {
                throw BusinessRulesCode.BuyWithoutBranchOffice.NewBusinessException();
            }

            if (buyInfo.BranchOfficeStaff == null)
            {
                throw BusinessRulesCode.BuyWithoutBranchOfficeStaff.NewBusinessException();
            }

            if (buyInfo.Provider == null)
            {
                throw BusinessRulesCode.BuyWithoutProvider.NewBusinessException();
            }                         

            var buy = new Buy
            {
                BranchOfficeId = buyInfo.BranchOffice.Id,
                BuyDate = buyInfo.BuyDate,
                BranchOfficeStaffId = buyInfo.BranchOfficeStaff.Id,
                PaymentTypeCode = buyInfo.PaymentTypeCode,                
                ProviderId = buyInfo.Provider.Id,
                Detail = buyInfo.Detail.Select(d => new BuyDetail
                {
                    Amount = d.Amount,
                    Price = d.Price,
                    ProductId = d.Product.Id
                }).ToList()                    
            };

            if ((buyInfo.Transactions != null) && (buyInfo.Transactions.Count > 0))
            {
                if (buyInfo.Transactions.Any(t => t.Detail.Any(d => d.Bill == null)))
                {
                    throw BusinessRulesCode.TransactionWithoutBill.NewBusinessException();
                }
            }

            var transactionSvc = new TransactionService(context);
            buy.Transactions = buyInfo.Transactions?.Select(d => transactionSvc.NewTransaction(d)).ToList();

            ViewBuy.Add(buy);

            return buy;
        }

        public Buy ModifyBuy(BuyDto buyInfo)
        {
            var buy = UpdateBuy(buyInfo);
            context.ApplyChanges();
            return buy;
        }

        private Buy UpdateBuy(BuyDto buyInfo)
        {
            var buy = GetBuy(buyInfo.Id);
            if (buy == null)
            {
                throw BusinessRulesCode.BuyNotExists.NewBusinessException();
            }

            if (buyInfo.BranchOffice == null)
            {
                throw BusinessRulesCode.BuyWithoutBranchOffice.NewBusinessException();
            }

            if (buyInfo.BranchOfficeStaff == null)
            {
                throw BusinessRulesCode.BuyWithoutBranchOfficeStaff.NewBusinessException();
            }

            if (buyInfo.Provider == null)
            {
                throw BusinessRulesCode.BuyWithoutProvider.NewBusinessException();
            }

            buy.BranchOfficeId = buyInfo.BranchOffice.Id;
            buy.BuyDate = buyInfo.BuyDate;
            buy.BranchOfficeStaffId = buyInfo.BranchOfficeStaff.Id;
            buy.PaymentTypeCode = buyInfo.PaymentTypeCode;
            buy.ProviderId = buyInfo.Provider.Id;
            buy.DeactivatedDate = buyInfo.DeactivatedDate;
            buy.DeactivatedNote = buyInfo.DeactivatedNote;
            
            foreach (var detail in buyInfo.Detail)
            {
                var existingDetail = buy.Detail.FirstOrDefault(r => r.Id.Equals(detail.Id));
                if (existingDetail != null)
                {
                    existingDetail.Price = detail.Price;
                    existingDetail.Amount = detail.Amount;
                    existingDetail.ProductId = detail.Product.Id;                    
                }               
            }

            var detailsToDelete = new List<BuyDetail>();
            foreach (var detail in buy.Detail)
            {
                var detailBuy = buyInfo.Detail.FirstOrDefault(r => r.Id.Equals(detail.Id));
                if (detailBuy != null)
                {
                    continue;
                }

                detailsToDelete.Add(detail);
            }

            if (detailsToDelete.Count > 0)
            {
                foreach (var detail in detailsToDelete)
                {
                    RemoveBuyDetail(detail);
                }
            }

            foreach (var detail in buyInfo.Detail)
            {
                if (detail.Id == 0)
                {
                    buy.Detail.Add(new BuyDetail
                    {
                        BuyId = buy.Id,
                        Amount = detail.Amount,
                        Price = detail.Price,
                        ProductId = detail.Product.Id
                    });
                }
            }

            if ((buyInfo.Transactions != null) && (buyInfo.Transactions.Count > 0))
            {
                if (buyInfo.Transactions.Any(t => t.Detail.Any(d => d.Bill == null)))
                {
                    throw BusinessRulesCode.TransactionWithoutBill.NewBusinessException();
                }

                var transactionSvc = new TransactionService(context);
                buy.Transactions = buyInfo.Transactions.Select(d => transactionSvc.UpdateTransaction(d)).ToList();
            }

            ViewBuy.Modify(buy);

            return buy;
        }

        public void RemoveBuyDetail(BuyDetail buyDetail)
        {
            ViewBuyDetail.Remove(buyDetail);
        }

        public bool StockBuy(int buyId, Stock stock)
        {
            var result = ModifyBuyForStock(buyId, stock);
            context.ApplyChanges();
            return result.StockId.HasValue;            
        }

        public bool BoxBuy(int buyId, Box box)
        {
            var result = ModifyBuyForBox(buyId, box);
            context.ApplyChanges();
            return result.BoxId.HasValue;
        }

        public Buy ModifyBuyForStock(int buyId, Stock stock)
        {
            var buy = ViewBuy.Get(buyId);
            if(buy == null)
            {
                throw BusinessRulesCode.BuyNotExists.NewBusinessException();
            }

            buy.Stock = stock;
            ViewBuy.Modify(buy);

            return buy;
        }

        public Buy ModifyBuyForBox(int buyId, Box box)
        {
            var buy = ViewBuy.Get(buyId);
            if (buy == null)
            {
                throw BusinessRulesCode.BuyNotExists.NewBusinessException();
            }

            buy.Box = box;
            ViewBuy.Modify(buy);

            return buy;
        }

        public IEnumerable<Buy> QueryBuyNotStocked()
        {
            return ViewBuy.Query(b => !b.StockId.HasValue && !b.DeactivatedDate.HasValue);
        }

        public IEnumerable<Buy> QueryBuyStocked()
        {
            return ViewBuy.Query(b => b.StockId.HasValue && !b.DeactivatedDate.HasValue);
        }

        public IEnumerable<Buy> QueryBuyNotBoxed()
        {
            return ViewBuy.Query(b => !b.BoxId.HasValue && !b.DeactivatedDate.HasValue);
        }

        public IEnumerable<Buy> QueryBuyBoxed()
        {
            return ViewBuy.Query(b => b.BoxId.HasValue && !b.DeactivatedDate.HasValue);
        }

        public IEnumerable<Buy> QueryBuy(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<Buy>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Buy.BuyDate, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Buy, DateTimeOffset>(b => b.BuyDate, info.Direction));
                }
            }

            return ViewBuy.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetBuySpecification(filterInfo));
        }

        private static Specification<Buy> GetBuySpecification(IEnumerable<FilterInfo> filterInfo)
        {
            Specification<Buy> spec = new TrueSpecification<Buy>();
            if (filterInfo == null)
            {
                return spec;
            }

            foreach (var info in filterInfo)
            {
                if (info.Spec.Equals(SpecFilter.Buy.BuyDate, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecBuyByBuyDate(info.Value);
                }

                if (info.Spec.Equals(SpecFilter.Buy.BuyFromDate, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecBuyByBuyFromDate(info.Value);
                }
              
                if (info.Spec.Equals(SpecFilter.Buy.BuyCustomerUid, StringComparison.InvariantCultureIgnoreCase))
                {                    
                    spec &= SpecBuyByProviderId(info.Value);
                }
            }

            return spec;
        }

        private static Specification<Buy> SpecBuyByProviderId(string value)
        {
            return new DirectSpecification<Buy>(b => !b.DeactivatedDate.HasValue && b.ProviderId.Equals(Guid.Parse(value)));
        }

        private static Specification<Buy> SpecBuyByBuyDate(string value)
        {
            return new DirectSpecification<Buy>(s => s.BuyDate.Equals(DateTimeOffset.Parse(value)));
        }

        private static Specification<Buy> SpecBuyByBuyFromDate(string value)
        {
            return new DirectSpecification<Buy>(s => s.BuyDate >= DateTimeOffset.Parse(value));
        }
    }
}
