namespace SAC.Stock.Service.BuyContext
{
    using SAC.Seed.NLayer.Application;
    using SAC.Stock.Service.ProductContext;
    internal class BuyDetailDto : EntityDto<int>
    {
        public int Amount { get; set; }
        public double Price { get; set; }
        public int BuyId { get; set; }
        public BuyDto Buy { get; set; }        
        public ProductDto Product { get; set; }
    }
}
