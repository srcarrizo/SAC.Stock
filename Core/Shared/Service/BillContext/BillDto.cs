namespace SAC.Stock.Service.BillContext
{
    using Seed.NLayer.Application;
    public class BillDto : EntityDto<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public BillUnitTypeDto BillUnitType { get; set; }
    }
}
