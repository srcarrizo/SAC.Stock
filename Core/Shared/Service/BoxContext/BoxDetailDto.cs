namespace SAC.Stock.Service.BoxContext
{
    using SAC.Seed.NLayer.Domain;
    using SAC.Stock.Service.BillContext;
    internal class BoxDetailDto : Entity<int>
    {
        public int Amount { get; set; }
        public BillDto Bill { get; set; }
        public BoxDto Box { get; set; }
    }
}
