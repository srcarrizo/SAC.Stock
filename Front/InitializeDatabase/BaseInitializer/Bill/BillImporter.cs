namespace SAC.Stock.Front.InitializeDatabase.BaseInitializer.Bill
{
    using SAC.Seed.Dependency;
    using SAC.Seed.NLayer.Data;
    using SAC.Stock.Code;
    using SAC.Stock.Service.BillContext;
    using System.Collections.Generic;
    using System.Linq;

    public class BillImporter
    {
        public static void Run()
        {            
            var supportSvc = DiContainerFactory.DiContainer().Resolve<IBillApplicationService>();
            var billType = supportSvc.QueryBillUnitType(1, 500, new List<SortInfo>
            {
               new SortInfo
               {
                   Direction = OrderDirection.Asc,
                   Field = SortQuery.BillUnitType.Name
               }
            }, null);

            var coin2 = false;
            var coin3 = false;
            foreach (var bills in CodeConst.BillTable.Values())
            {
                var code = bills.Name.Replace(" ", "");
                var bill = new BillDto
                {                    
                    Code = code,
                    Name = bills.Name,
                    Value = int.Parse(bills.Code)
                };

                bill.BillUnitType = billType.FirstOrDefault(t => t.IsDecimal == bills.Description.Equals("Decimal"));
                if (bill.Name.Equals("1 peso"))
                {
                    bill.BillUnitType = billType.FirstOrDefault(t => t.IsDecimal == false && t.Code.Equals(CodeConst.BillUnitType.CoinInt.Code));
                }

                if (bill.Name.Equals("2 pesos"))
                {
                    if (!coin2)
                    {
                        bill.BillUnitType = billType.FirstOrDefault(c => c.Code.Equals(CodeConst.BillUnitType.CoinInt.Code));
                        bill.Code = bill.Code + "C";
                        coin2 = true;
                    }
                }

                if (bill.Name.Equals("5 pesos"))
                {
                    if (!coin3)
                    {
                        bill.BillUnitType = billType.FirstOrDefault(c => c.Code.Equals(CodeConst.BillUnitType.CoinInt.Code));
                        bill.Code = bill.Code + "C";
                        coin3 = true;
                    }
                }

                supportSvc.AddBill(bill);
            }            
        }       
    }
}
