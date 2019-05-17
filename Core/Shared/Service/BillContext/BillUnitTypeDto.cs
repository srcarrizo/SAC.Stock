namespace SAC.Stock.Service.BillContext
{
    using Seed.NLayer.Application;
    public class BillUnitTypeDto : EntityDto<int>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsDecimal { get; set; }
    }
}
