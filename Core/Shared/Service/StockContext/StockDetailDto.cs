namespace SAC.Stock.Service.StockContext
{
    using SAC.Seed.NLayer.Application;
    using SAC.Stock.Service.ProductContext;

    internal class StockDetailDto : EntityDto<int>
    {        
        public int ProductId { get; set; }     
        public int Amount { get; set; }          
        public ProductDto Product { get; set; }
    }
}