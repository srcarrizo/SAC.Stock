namespace SAC.Stock.Service.SaleContext
{
    using SAC.Seed.NLayer.Application;
    using SAC.Stock.Service.ProductContext;
    internal class SaleDetailDto : EntityDto<int>
    {
        public int Amount { get; set; }
        public double Price { get; set; }
        public SaleDto Sale { get; set; }
        public ProductDto Product { get; set; }
        public int ProductId { get; set; }
    }
}
