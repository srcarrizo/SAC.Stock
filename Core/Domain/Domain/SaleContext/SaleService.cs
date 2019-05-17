namespace SAC.Stock.Domain.SaleContext
{
    using Seed.NLayer.Data;
    using Seed.NLayer.Data.Ordering;
    using Seed.NLayer.Domain;
    using Seed.NLayer.Domain.Specification;
    using Code;
    using TransactionContext;
    using Service.SaleContext;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SAC.Stock.Domain.StockContext;
    using SAC.Stock.Domain.BoxContext;

    internal class SaleService
    {
        private readonly IDataContext context;
        internal SaleService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Sale, int> ViewSale
        {
            get
            {
                return context.GetView<Sale, int>();
            }
        }

        private IDataView<SaleDetail, int> ViewSaleDetail
        {
            get
            {
                return context.GetView<SaleDetail, int>();
            }
        }

        public bool DeletePreSale(SaleDto saleInfo)
        {
            var result = ErasePreSale(saleInfo);
            context.ApplyChanges();
            return result;
        }

        private bool ErasePreSale(SaleDto saleInfo, bool isPreSale = false)
        {
            var sale = GetSale(saleInfo.Id);
            if (sale == null)
            {
                throw BusinessRulesCode.SaleNotExists.NewBusinessException();
            }

            if (isPreSale)
            {
                if (!sale.PreSale)
                {
                    throw BusinessRulesCode.NotPreSale.NewBusinessException();
                }
            }
            
            foreach (var detail in saleInfo.Detail)
            {
                ViewSaleDetail.Remove(detail.Id);
            }
          
            ViewSale.Remove(sale.Id);            
            return true;
        }

        public Sale CompleteSale(SaleDto saleInfo)
        {
            saleInfo.PreSale = false;
            var sale = UpdateSale(saleInfo);
            context.ApplyChanges();

            return sale;
        }

        public Sale GetSale(int SaleId)
        {
            return ViewSale.Get(SaleId);
        }

        public Sale AddSale(SaleDto SaleInfo)
        {
            var Sale = NewSale(SaleInfo);
            context.ApplyChanges();
            return Sale;
        }

        private Sale NewSale(SaleDto saleInfo)
        {
            if (saleInfo.BranchOffice == null)
            {
                throw BusinessRulesCode.SaleWithoutBranchOffice.NewBusinessException();
            }

            if (saleInfo.BranchOfficeStaff == null)
            {
                throw BusinessRulesCode.SaleWithoutBranchOfficeStaff.NewBusinessException();
            }

            if (saleInfo.Customer == null)
            {
                throw BusinessRulesCode.SaleWithoutCustomer.NewBusinessException();
            }            

            var sale = new Sale
            {
                BranchOfficeId = saleInfo.BranchOffice.Id,
                SaleDate = saleInfo.SaleDate,
                BranchOfficeStaffId = saleInfo.BranchOfficeStaff.Id,
                PaymentTypeCode = saleInfo.PaymentTypeCode,  
                PreSale = saleInfo.PreSale,
                CustomerId = saleInfo.Customer.Id,
                MayorMinorSale = saleInfo.MayorMinorSale,                
                Detail = saleInfo.Detail.Select(d => new SaleDetail
                {
                    Amount = d.Amount,
                    Price = d.Price,
                    ProductId = d.Product.Id,
                    ProductPriceId = d.Product.ProductPrices.OrderByDescending(p => p.Id).First().Id
                }).ToList()                
            };

            if ((saleInfo.Transactions != null) && (saleInfo.Transactions.Count > 0))
            {
                if (saleInfo.Transactions.Any(t => t.Detail.Any(d => d.Bill == null)))
                {
                    throw BusinessRulesCode.TransactionWithoutBill.NewBusinessException();
                }
            }

            var transactionSvc = new TransactionService(context);
            sale.Transactions = saleInfo.Transactions?.Select(d => transactionSvc.NewTransaction(d)).ToList();

            ViewSale.Add(sale);
            return sale;
        }

        public Sale ModifySale(SaleDto SaleInfo)
        {
            var Sale = UpdateSale(SaleInfo);
            context.ApplyChanges();
            return Sale;
        }

        private Sale UpdateSale(SaleDto SaleInfo)
        {
            var Sale = GetSale(SaleInfo.Id);
            if (Sale == null)
            {
                throw BusinessRulesCode.SaleNotExists.NewBusinessException();
            }

            if (SaleInfo.BranchOffice == null)
            {
                throw BusinessRulesCode.SaleWithoutBranchOffice.NewBusinessException();
            }

            if (SaleInfo.BranchOfficeStaff == null)
            {
                throw BusinessRulesCode.SaleWithoutBranchOfficeStaff.NewBusinessException();
            }

            if (SaleInfo.Customer == null)
            {
                throw BusinessRulesCode.SaleWithoutCustomer.NewBusinessException();
            }

            Sale.BranchOfficeId = SaleInfo.BranchOffice.Id;
            Sale.SaleDate = SaleInfo.SaleDate;
            Sale.BranchOfficeStaffId = SaleInfo.BranchOfficeStaff.Id;
            Sale.PaymentTypeCode = SaleInfo.PaymentTypeCode;
            Sale.PreSale = SaleInfo.PreSale;
            Sale.MayorMinorSale = SaleInfo.MayorMinorSale;
            Sale.CustomerId = SaleInfo.Customer.Id;
            Sale.DeactivatedDate = SaleInfo.DeactivatedDate;
            Sale.DeactivatedNote = SaleInfo.DeactivatedNote;

            foreach (var detail in SaleInfo.Detail)
            {
                var existingDetail = Sale.Detail.FirstOrDefault(r => r.Id.Equals(detail.Id));
                if (existingDetail != null)
                {
                    existingDetail.Price = detail.Price;
                    existingDetail.Amount = detail.Amount;
                    existingDetail.ProductId = detail.Product.Id;
                    existingDetail.ProductPriceId = detail.Product.ProductPrices.OrderByDescending(d => d.Id).First().Id;
                }
            }

            var detailsToDelete = new List<SaleDetail>();
            foreach (var detail in Sale.Detail)
            {
                var detailSale = SaleInfo.Detail.FirstOrDefault(r => r.Id.Equals(detail.Id));
                if (detailSale != null)
                {
                    continue;
                }

                detailsToDelete.Add(detail);
            }

            if (detailsToDelete.Count > 0)
            {
                foreach (var detail in detailsToDelete)
                {
                    RemoveSaleDetail(detail);
                }
            }

            foreach (var detail in SaleInfo.Detail)
            {
                if (detail.Id == 0)
                {
                    Sale.Detail.Add(new SaleDetail
                    {
                        SaleId = Sale.Id,
                        Amount = detail.Amount,
                        Price = detail.Price,
                        ProductId = detail.Product.Id,
                        ProductPriceId = detail.Product.ProductPrices.OrderByDescending(p => p.Id).First().Id
                    });
                }
            }

            if ((SaleInfo.Transactions != null) && (SaleInfo.Transactions.Count > 0))
            {
                if (SaleInfo.Transactions.Any(t => t.Detail.Any(d => d.Bill == null)))
                {
                    throw BusinessRulesCode.TransactionWithoutBill.NewBusinessException();
                }

                var transactionSvc = new TransactionService(context);
                Sale.Transactions = SaleInfo.Transactions.Select(d => transactionSvc.UpdateTransaction(d)).ToList();
            }

            ViewSale.Modify(Sale);

            return Sale;
        }

        public void RemoveSaleDetail(SaleDetail SaleDetail)
        {
            ViewSaleDetail.Remove(SaleDetail);
        }

        public bool StockSale(int saleId, Stock stock)
        {
            var result = ModifySaleForStock(saleId, stock);
            context.ApplyChanges();
            return result.StockId.HasValue;            
        }

        public bool BoxSale(int saleId, Box box)
        {
            var result = ModifySaleForBox(saleId, box);
            context.ApplyChanges();
            return result.BoxId.HasValue;
        }

        public Sale ModifySaleForStock(int saleId, Stock stock)
        {
            var sale = ViewSale.Get(saleId);
            if (sale == null)
            {
                throw BusinessRulesCode.SaleNotExists.NewBusinessException();
            }

            sale.Stock = stock;
            ViewSale.Modify(sale);

            return sale;
        }

        public Sale ModifySaleForBox(int saleId, Box box)
        {
            var sale = ViewSale.Get(saleId);
            if (sale == null)
            {
                throw BusinessRulesCode.SaleNotExists.NewBusinessException();
            }

            sale.Box = box;
            ViewSale.Modify(sale);

            return sale;
        }

        public IEnumerable<Sale> QuerySaleNotBoxed()
        {
            return ViewSale.Query(b => !b.BoxId.HasValue && !b.DeactivatedDate.HasValue && !b.PreSale);
        }
        public IEnumerable<Sale> QuerySaleNotStocked()
        {
            return ViewSale.Query(b => !b.StockId.HasValue && !b.DeactivatedDate.HasValue && !b.PreSale);
        }
        public IEnumerable<Sale> QuerySaleBoxed()
        {
            return ViewSale.Query(b => b.BoxId.HasValue && !b.DeactivatedDate.HasValue);
        }
        public IEnumerable<Sale> QuerySaleStocked()
        {
            return ViewSale.Query(b => b.StockId.HasValue && !b.DeactivatedDate.HasValue);
        }
        public IEnumerable<Sale> QueryPreSale()
        {
            return ViewSale.Query(b => !b.DeactivatedDate.HasValue && b.PreSale);
        }

        public IEnumerable<Sale> QuerySale(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<Sale>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Sale.SaleDate, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Sale, DateTimeOffset>(b => b.SaleDate, info.Direction));
                }
            }

            return ViewSale.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetSaleSpecification(filterInfo));
        }

        private static Specification<Sale> GetSaleSpecification(IEnumerable<FilterInfo> filterInfo)
        {
            Specification<Sale> spec = new TrueSpecification<Sale>();
            if (filterInfo == null)
            {
                return spec;
            }

            foreach (var info in filterInfo)
            {
                if (info.Spec.Equals(SpecFilter.Sale.SaleDate, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecSaleBySaleDate(info.Value);
                }

                if (info.Spec.Equals(SpecFilter.Sale.SaleFromDate, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecSaleBySaleFromDate(info.Value);
                }

                if (info.Spec.Equals(SpecFilter.Sale.SaleCustomerUid, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecSaleByCustomerId(info.Value);
                }
            }

            return spec;
        }

        private static Specification<Sale> SpecSaleByCustomerId(string value)
        {
            return new DirectSpecification<Sale>(b => !b.DeactivatedDate.HasValue && b.CustomerId.Equals(Guid.Parse(value)));
        }

        private static Specification<Sale> SpecSaleBySaleDate(string value)
        {
            return new DirectSpecification<Sale>(s => s.SaleDate.Equals(DateTimeOffset.Parse(value)));
        }

        private static Specification<Sale> SpecSaleBySaleFromDate(string value)
        {
            return new DirectSpecification<Sale>(s => s.SaleDate >= DateTimeOffset.Parse(value));
        }
    }
}