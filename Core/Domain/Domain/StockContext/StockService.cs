namespace SAC.Stock.Domain.StockContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.Domain;
    using SAC.Stock.Domain.BuyContext;
    using ProductContext;
    using SaleContext;
    using System;    
    using System.Linq;     
    using SAC.Stock.Code;
    using System.Collections.Generic;

    public class StockService
    {
        private readonly IDataContext context;

        public StockService(IDataContext context)
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

        private IDataView<Buy, int> ViewBuy
        {
            get
            {
                return context.GetView<Buy, int>();
            }
        }

        private IDataView<Product, int> ViewProduct
        {
            get
            {
                return context.GetView<Product, int>();
            }
        }

        private IDataView<Stock, int> ViewStock
        {
            get
            {
                return context.GetView<Stock, int>();
            }
        }

        public Stock GetStockByDate(DateTimeOffset stockDate)
        {
            return ViewStock.GetFirst(s => s.StockDate.Date.Equals(stockDate));
        }

        public int CheckStockByProduct(int productId)
        {            
            var (currentStock, totalSales, totalBuys, preSales) = GetCurrentStock();
            var buys = ViewBuy.Query(b => b.Detail.Any(d => d.ProductId == productId)).Where(b => !b.DeactivatedDate.HasValue && !b.StockId.HasValue).ToList();
            var sales = ViewSale.Query(s => s.Detail.Any(d => d.ProductId == productId)).Where(b => !b.DeactivatedDate.HasValue && !b.StockId.HasValue).ToList();

            var buyStock = from buy in buys
                           from detail in buy.Detail
                           where detail.ProductId == productId
                           group detail.Amount by detail.Product into g                           
                           select new { Product = g.Key, Amount = g.Sum() };

            var buysTotal = from pro in buyStock
                            select new { ProductId = pro.Product.Id, Total = pro.Amount * pro.Product.Container.Amount };

            var saleStock = from sale in sales
                            from detail in sale.Detail
                            where detail.ProductId == productId
                            group detail.Amount by detail.Product into g
                            select new { Product = g.Key, Amount = g.Sum() };

            var salesTotal = from pro in saleStock
                             select new { ProductId = pro.Product.Id, SaleTotal = (pro.Amount * -1) };

            var positiveStock = currentStock != null ? from st in currentStock.Detail
                                                       join buy in buysTotal on st.ProductId equals buy.ProductId into join1
                                                       from joined in join1.DefaultIfEmpty()
                                                       select new { st.ProductId, Total = st.Amount + (joined == null ? 0 : joined.Total) } :
                               buysTotal;

            var unionStock = positiveStock.Concat(buysTotal).GroupBy(p => p.ProductId).Select(p => p.First());

            var newStock = from p in unionStock
                           join n in salesTotal on p.ProductId equals n.ProductId into join1
                           from joined in join1.DefaultIfEmpty()
                           select new { p.ProductId, Total = p.Total + (joined == null ? 0 : joined.SaleTotal) };

            var stockProduct = newStock.Where(s => s.ProductId == productId).FirstOrDefault();
            return stockProduct != null ? stockProduct.Total : 0;
        }

        public int GetStockByProduct(int productId)
        {
            var (currentStock, totalSales, totalBuys, preSales) = GetCurrentStock();
            var buys = ViewBuy.Query(b => b.Detail.Any(d => d.ProductId == productId)).Where(b => !b.DeactivatedDate.HasValue && !b.StockId.HasValue).ToList();
            var sales = ViewSale.Query(s => s.Detail.Any(d => d.ProductId == productId)).Where(b => !b.DeactivatedDate.HasValue && !b.StockId.HasValue).ToList();

            var buyStock = from buy in buys
                           from detail in buy.Detail
                           where detail.ProductId == productId
                           group detail.Amount by detail.Product into g
                           select new { Product = g.Key, Amount = g.Sum() };

            var buysTotal = from pro in buyStock
                            select new { ProductId = pro.Product.Id, Total = pro.Amount * pro.Product.Container.Amount };

            var saleStock = from sale in sales
                            from detail in sale.Detail
                            where detail.ProductId == productId
                            group detail.Amount by detail.Product into g
                            select new { Product = g.Key, Amount = g.Sum() };

            var salesTotal = from pro in saleStock
                             select new { ProductId = pro.Product.Id, SaleTotal = (pro.Amount * -1) };

            var positiveStock = currentStock != null ? from st in currentStock.Detail
                                                       join buy in buysTotal on st.ProductId equals buy.ProductId into join1
                                                       from joined in join1.DefaultIfEmpty()
                                                       select new { st.ProductId, Total = st.Amount + (joined == null ? 0 : joined.Total) } :
                               buysTotal;

            var unionStock = positiveStock.Concat(buysTotal).GroupBy(p => p.ProductId).Select(p => p.First());

            var newStock = from p in unionStock
                           join n in salesTotal on p.ProductId equals n.ProductId into join1
                           from joined in join1.DefaultIfEmpty()
                           select new { p.ProductId, Total = p.Total + (joined == null ? 0 : joined.SaleTotal) };
           
            var product = ViewProduct.Get(productId);            
            if (newStock.Where(s => s.ProductId == productId).FirstOrDefault().Total < product.Container.Amount)
            {
                throw BusinessRulesCode.LowStock.NewBusinessException();
            }

            if (newStock.Where(s => s.ProductId == productId).FirstOrDefault().Total == 0)
            {
                throw BusinessRulesCode.NoStock.NewBusinessException();
            }

            var stockProduct = newStock.Where(s => s.ProductId == productId).FirstOrDefault();
            return stockProduct != null ? stockProduct.Total : 0;
        }

        public (Stock, int, int, int) CreateStock()
        {

            var (stock, sales, buy, preSales) = GenerateStock();
            context.ApplyChanges();            

            return (stock, sales, buy, preSales);
        }

        public (Stock, int, int, int)GetCurrentStock()
        {
            var buySrv = new BuyService(context);
            var saleSrv = new SaleService(context);
            var stock = ViewStock.GetAll().OrderByDescending(c => c.StockDate).Where(s => !s.DeactivatedDate.HasValue).FirstOrDefault();            
            var preSale = saleSrv.QueryPreSale().ToList();

            var productPresale = from prs in preSale
                              from detail in prs.Detail
                              group detail.Amount by detail.Product into g
                              select new { Product = g.Key, Amount = g.Sum() };

            if (stock != null)
            {
                var result = from detail in stock.Detail
                             join pres in productPresale on detail.ProductId equals pres.Product.Id into join1
                             from joined in join1.DefaultIfEmpty()
                             select new { Amount = detail.Amount + ((joined == null ? 0 : joined.Amount) * -1), detail.ProductId, detail.Product, detail.Id };

                var currentStock = stock;
                currentStock.Detail = result.Where(d => d.Amount > 0).Select(r => new StockDetail
                {
                    Id = r.Id,
                    Amount = r.Amount,
                    Product = r.Product,
                    ProductId = r.ProductId
                }).ToList();

                return (currentStock, saleSrv.QuerySaleNotStocked().ToList().Count(), buySrv.QueryBuyNotStocked().ToList().Count(), saleSrv.QueryPreSale().ToList().Count());
            }
           
            return (null, saleSrv.QuerySaleNotStocked().ToList().Count(), buySrv.QueryBuyNotStocked().ToList().Count(), saleSrv.QueryPreSale().ToList().Count());
        }

        private (Stock, int, int, int) GenerateStock()
        {
            var buySrv = new BuyService(context);
            var saleSrv = new SaleService(context);
            var currentStock = ViewStock.GetAll().OrderByDescending(c => c.StockDate).Where(s => !s.DeactivatedDate.HasValue).FirstOrDefault();
            var buys = buySrv.QueryBuyNotStocked().ToList();
            var sales = saleSrv.QuerySaleNotStocked().ToList();
            var preSales = saleSrv.QueryPreSale().ToList();

            var productBuys = from buy in buys
                              from detail in buy.Detail
                              group detail.Amount by detail.Product into g
                              select new { Product = g.Key, Amount = g.Sum() };

            var buysTotal = from pro in productBuys
                            select new { ProductId = pro.Product.Id, Total = pro.Amount * pro.Product.Container.Amount };

            var productSales = from sale in sales
                               from detail in sale.Detail
                               group detail.Amount by detail.Product into g
                               select new { Product = g.Key, Amount = g.Sum() };

            var salesTotal = from pro in productSales
                             select new { ProductId = pro.Product.Id, SaleTotal = (pro.Amount * -1) };

            var positiveStock = currentStock != null ? from st in currentStock.Detail
                                                       join buy in buysTotal on st.ProductId equals buy.ProductId into join1
                                                       from joined in join1.DefaultIfEmpty()
                                                       select new { st.ProductId, Total = st.Amount + (joined == null ? 0 : joined.Total) } :
                                buysTotal;

            var unionStock = positiveStock.Concat(buysTotal).GroupBy(p => p.ProductId).Select(p => p.First());

            var newStock = from p in unionStock
                           join n in salesTotal on p.ProductId equals n.ProductId into join1
                           from joined in join1.DefaultIfEmpty()
                           select new { p.ProductId, Total = p.Total + (joined == null ? 0 : joined.SaleTotal) };

            var stock = new Stock
            {
                StockDate = DateTimeOffset.UtcNow
            };

            stock.Detail = newStock.Select(s => new StockDetail
            {
                Amount = s.Total,
                ProductId = s.ProductId
            }).Where(c => c.Amount > 0).ToList();
            
            foreach (var buy in buys)
            {
                buySrv.ModifyBuyForStock(buy.Id, stock);
            }

            foreach (var sale in sales)
            {
                saleSrv.ModifySaleForStock(sale.Id, stock);
            }

            ViewStock.Add(stock);
            return (stock, sales.Count(), buys.Count(), preSales.Count());
        }
    }
}