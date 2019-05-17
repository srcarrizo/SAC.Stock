namespace SAC.Stock.Domain.BoxContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.Domain;
    using SAC.Stock.Service.BoxContext;    
    using System.Linq;    
    using SAC.Stock.Domain.TransactionContext;
    using SAC.Stock.Code;
    using SAC.Stock.Domain.BuyContext;
    using SAC.Stock.Domain.SaleContext;
    using SAC.Stock.Service.BuyContext;
    using SAC.Stock.Service.SaleContext;
    using SAC.Stock.Service.TransactionContext;

    internal class BoxService 
    {
        private readonly IDataContext context;

        public BoxService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Box, int> ViewBox
        {
            get
            {
                return context.GetView<Box, int>();
            }
        }
        public Box GetBox(int boxId)
        {
            return ViewBox.Get(boxId);
        }

        public Box AddBox(BoxDto boxInfo)
        {
            var box = NewBox(boxInfo);
            context.ApplyChanges();
            return box;
        }

        private Box NewBox(BoxDto boxInfo)
        {                        
            var newbox = new Box
            {                 
                OpenDate = boxInfo.OpenDate,
                OpenNote = boxInfo.OpenNote,     
                CloseDate = boxInfo.CloseDate,
                CloseNote = boxInfo.CloseNote, 
                Withdrawal = boxInfo.Withdrawal,
                Detail = boxInfo.Detail.Select(d => d.AdaptToBoxDetail()).ToList(),
                Transactions = boxInfo.Transactions?.Select(d =>d.AdaptToTransaction()).ToList()
            };

            newbox.GenerateNewIdentity();
            ViewBox.Add(newbox);
            return newbox;
        }

        public Box OpenCloseBox(BoxDto boxInfo)
        {
            var box = ProcessBox(boxInfo);            
            context.ApplyChanges();
            return box;
        }
        
        private Box ProcessBox(BoxDto boxInfo)
        {
            Box box;
            var latestBox = GetLatestBox();
            if (latestBox == null)
            {
                if (!boxInfo.OpenDate.HasValue)
                {
                    throw BusinessRulesCode.BoxOpeningWithoutOpenDate.NewBusinessException();
                }

                if (boxInfo.OpenNote == null)
                {
                    throw BusinessRulesCode.BoxOpeningWithoutOpenNote.NewBusinessException();
                }

                boxInfo.CloseDate = null;
                boxInfo.CloseNote = null;
                box = NewBox(boxInfo);

                return box;
            }

            if (latestBox.OpenDate.HasValue && !latestBox.CloseDate.HasValue)
            {
                if (!boxInfo.CloseDate.HasValue)
                {
                    throw BusinessRulesCode.BoxClosingingWithoutCloseDate.NewBusinessException();
                }

                if (boxInfo.CloseNote == null)
                {
                    throw BusinessRulesCode.BoxClosingWithoutClosingNote.NewBusinessException();
                }

                boxInfo.OpenDate = null;
                boxInfo.OpenNote = null;
                box = NewBox(boxInfo);

                var saleSrv = new SaleService(context);
                var buySrv = new BuyService(context);
                var tSrv = new TransactionService(context);

                var sales = saleSrv.QuerySaleNotBoxed().ToList();
                var buys = buySrv.QueryBuyNotBoxed().ToList();
                var transactions = tSrv.QueryTransactionNotBoxed().ToList();

                foreach (var sale in sales)
                {
                    saleSrv.ModifySaleForBox(sale.Id, box);
                }

                foreach (var buy in buys)
                {
                    buySrv.ModifyBuyForBox(buy.Id, box);
                }

                foreach (var t in transactions)
                {
                    tSrv.ModifyTransactionForBox(t.Id, box);
                }

                return box;
            }

            if (!latestBox.OpenDate.HasValue && latestBox.CloseDate.HasValue)
            {
                if (!boxInfo.OpenDate.HasValue)
                {
                    throw BusinessRulesCode.BoxOpeningWithoutOpenDate.NewBusinessException();
                }

                if (boxInfo.OpenNote == null)
                {
                    throw BusinessRulesCode.BoxOpeningWithoutOpenNote.NewBusinessException();
                }

                boxInfo.CloseDate = null;
                boxInfo.CloseNote = null;
                box = NewBox(boxInfo);

                return box;
            }

            if (!boxInfo.OpenDate.HasValue)
            {
                throw BusinessRulesCode.BoxOpeningWithoutOpenDate.NewBusinessException();
            }

            if (boxInfo.OpenNote == null)
            {
                throw BusinessRulesCode.BoxOpeningWithoutOpenNote.NewBusinessException();
            }

            boxInfo.CloseDate = null;
            boxInfo.CloseNote = null;
            box = NewBox(boxInfo);
          
            return box;
        }

        public Box ModifyBox(BoxDto boxInfo)
        {
            var box = UpdateBox(boxInfo);
            context.ApplyChanges();

            return box;
        }

        private Box UpdateBox(BoxDto boxInfo)
        {
            var box = GetBox(boxInfo.Id);
            if (box == null)
            {
                throw BusinessRulesCode.BoxNotExists.NewBusinessException();
            }

            box.CloseDate = boxInfo.CloseDate;
            box.CloseNote = boxInfo.CloseNote;
            box.DeactivateDate = boxInfo.DeactivateDate;
            box.DeactivateNote = boxInfo.DeactivateNote;
            box.OpenDate = boxInfo.OpenDate;
            box.OpenNote = boxInfo.OpenNote;
            box.Detail = boxInfo.Detail.Select(d => d.AdaptToBoxDetail()).ToList();
            box.Transactions = boxInfo.Transactions?.Select(t => t.AdaptToTransaction()).ToList();

            ViewBox.Modify(box);
            return box;
        }

        public Box GetLatestBox()
        {            
            return ViewBox.GetAll().Where(b => !b.DeactivateDate.HasValue).OrderByDescending(b => b.Id).FirstOrDefault();
        }

        public BoxSalesBuysTransactionsDto GetLatestBoxWithSalesBuysTransaction()
        {
            var buySrv = new BuyService(context);
            var saleSrv = new SaleService(context);
            var tSrv = new TransactionService(context);
            
            var sales = saleSrv.QuerySaleNotBoxed().ToList();
            var preSales = saleSrv.QueryPreSale().ToList();
            var buys = buySrv.QueryBuyNotBoxed().ToList();
            var transactions = tSrv.QueryTransactionNotBoxed().ToList();

            var salesTotal = from sale in sales
                             from detail in sale.Detail
                             select new { Total = detail.Price != 0 ? 
                             (double)detail.Price * detail.Amount : sale.MayorMinorSale ? 
                             ((((double)detail.ProductPrice.BuyMayorPrice * (double)detail.ProductPrice.MayorGainPercent) / 100) + (double)detail.ProductPrice.BuyMayorPrice) * detail.Amount :
                             ((((double)detail.ProductPrice.BuyMayorPrice * (double)detail.ProductPrice.MinorGainPercent) / 100) + (double)detail.ProductPrice.BuyMayorPrice) * detail.Amount };

            var preSalesTotal = from presale in preSales
                                from detail in presale.Detail
                                select new { Total = detail.Price != 0 ? 
                                (double)detail.Price * detail.Amount : presale.MayorMinorSale ?
                                ((((double)detail.ProductPrice.BuyMayorPrice * (double)detail.ProductPrice.MayorGainPercent) / 100) + (double)detail.ProductPrice.BuyMayorPrice) * detail.Amount :
                                ((((double)detail.ProductPrice.BuyMayorPrice * (double)detail.ProductPrice.MinorGainPercent) / 100) + (double)detail.ProductPrice.BuyMayorPrice) * detail.Amount };

            var buysTotal = from buy in buys
                             from detail in buy.Detail
                             select new { Total = detail.Price != 0 ? (double)detail.Price * detail.Amount : (double)detail.Product.ProductPrices.OrderByDescending(p => p.Id).FirstOrDefault().BuyMayorPrice * detail.Amount };

            var transactionTotalIn = from t in transactions
                                   from detail in t.Detail
                                   where t.TransactionTypeInOut
                                   select new { TotalIn = detail.Bill.BillUnitType.IsDecimal ? ((decimal)detail.Amount * detail.Bill.Value) / 100 : detail.Amount * detail.Bill.Value };

            var transactionTotalOut = from t in transactions
                                   from detail in t.Detail
                                   where !t.TransactionTypeInOut
                                   select new { TotalOut = detail.Bill.BillUnitType.IsDecimal ? ((decimal)detail.Amount * detail.Bill.Value) / 100 : detail.Amount * detail.Bill.Value };

            var box = GetLatestBox();

            var result = new BoxSalesBuysTransactionsDto
            {
                Box = box.AdaptBoxToDto(),
                UnprocessedBuys = new BuysTotalCountDto
                {
                    Buys = buys.Select(b => b.AdaptToBuyDto()).ToList(),
                    Total = (decimal)buysTotal.Sum(b => b.Total),                    
                },
                UnprocessedSales = new SalesTotalCountDto
                {
                    Sales = sales.Select(s => s.AdaptToSaleDto()).ToList(),
                    Total = (decimal)salesTotal.Sum(s => s.Total),
                    PreSales = preSales.Select(p => p.AdaptToSaleDto()).ToList(),
                    PreSaleTotal = (decimal)preSalesTotal.Sum(s => s.Total)
                },
                UnprocessedTransactions = new TransactionsTotalDto
                {
                    Transactions = transactions.Select(t => t.AdaptTransactionToDto()).ToList(),
                    TotalIn = transactionTotalIn.Sum(t => t.TotalIn),
                    TotalOut = transactionTotalOut.Sum(t => t.TotalOut)
                }                
            };

            return result;
        }
    }
}
