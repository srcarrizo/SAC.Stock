namespace SAC.Stock.Testing.StockTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SAC.Seed.Dependency;
    using SAC.Stock.Service.StockContext;

    [TestClass]
    public class StockTest
    {
        [TestMethod]
        public void CreateStock()
        {
            var stockSrv = NewContainer().Resolve<IStockApplicationService>();
            var stock = stockSrv.CreateStock();

            Assert.IsNotNull(stock);
        }

        private static IDiContainer NewContainer()
        {
            return DiContainerFactory.DiContainer().BeginScope();
        }
    }
}
