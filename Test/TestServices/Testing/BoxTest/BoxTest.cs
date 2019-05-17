namespace SAC.Stock.Testing.BoxTest
{    
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SAC.Seed.Dependency;
    using SAC.Stock.Service.BoxContext;
    using System;
    using System.Collections.Generic;
    using Service.BillContext;
    using System.Linq;

    [TestClass]
    public class BoxTest
    {
        [TestMethod]
        public void ProcessBox()
        {
            var billServ = NewContainer().Resolve<IBillApplicationService>();
            var boxServ = NewContainer().Resolve<IBoxApplicationService>();
            var bills = billServ.QueryBill();
            var box = boxServ.GetLatestBox();            
            if (box == null || (box != null && box.CloseDate.HasValue))
            {
                box = new BoxDto
                {
                    OpenDate = DateTimeOffset.UtcNow,
                    OpenNote = "Pertura",
                    Detail = new List<BoxDetailDto>
                    {
                        new BoxDetailDto
                        {
                            Amount = 10,
                            Bill = bills.Where(b => b.Value == 100).FirstOrDefault()
                        },
                        new BoxDetailDto
                        {
                            Amount = 5,
                            Bill = bills.Where(b => b.Value == 20).FirstOrDefault()
                        },
                        new BoxDetailDto
                        {
                            Amount = 100,
                            Bill = bills.Where(b => b.Value == 50).FirstOrDefault()
                        },
                        new BoxDetailDto
                        {
                            Amount = 1000,
                            Bill = bills.Where(b => b.BillUnitType.IsDecimal && b.Value == 10).FirstOrDefault()
                        }
                    }
                };
            }
            else
            {
                box.CloseDate = DateTimeOffset.UtcNow;
                box.CloseNote = "Cierre";
            }

            box = boxServ.OpenCloseBox(box);
            Assert.IsNotNull(box);
        }

        [TestMethod]
        public void GetBoxCompleted()
        {
            var boxServ = NewContainer().Resolve<IBoxApplicationService>();
            var box = boxServ.GetLatestBoxWithSalesBuysTransaction();

            Assert.IsNotNull(box);
        }

        private static IDiContainer NewContainer()
        {
            return DiContainerFactory.DiContainer().BeginScope();
        }
    }
}
