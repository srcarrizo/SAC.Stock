namespace SAC.Stock.Service.StockContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Stock.Domain.StockContext;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    internal class StockApplicationService : IStockApplicationService
    {
        public IDataContext StockCtx { get; set; }

        public StockDto GetStockByDate(DateTimeOffset stockDate)
        {
            var svc = new StockService(StockCtx);
            return svc.GetStockByDate(stockDate).AdpatToStockDto();
        }

        public StockDto GetCurrentStock()
        {
            var svc = new StockService(StockCtx);
            var (stock, sales, buys, preSales) = svc.GetCurrentStock();
            var stockDto = stock.AdpatToStockDto();
            if (stockDto != null)
            {
                stockDto.UnProcsessedSales = sales;
                stockDto.UnProcsessedBuys = buys;
                stockDto.PreSales = preSales;
            }
            else
            {
                stockDto = new StockDto
                {
                    UnProcsessedSales = sales,
                    UnProcsessedBuys = buys,
                    PreSales = preSales
                };
            }         

            return stockDto;
        }

        public int GetStockByProduct(int productId)
        {
            var svc = new StockService(StockCtx);
            return svc.GetStockByProduct(productId);
        }

        public int CheckStockByProduct(int productId)
        {
            var svc = new StockService(StockCtx);
            return svc.CheckStockByProduct(productId);
        }

        public StockDto CreateStock()
        {
            var svc = new StockService(StockCtx);
            var (stock, sales, buys, preSales) = svc.CreateStock();
            var stockDto = stock.AdpatToStockDto();
            stockDto.ProcsessedSales = sales;
            stockDto.ProcsessedBuys = buys;
            stockDto.PreSales = preSales;

            return stockDto;
        }
    }
}
