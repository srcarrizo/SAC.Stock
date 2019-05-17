namespace SAC.Stock.Service.ProductContext
{
    using SAC.Seed.NLayer.Application;
    using System;    
    internal class ProductPriceDto : EntityDto<int>
    {        
        public decimal BuyMayorPrice { get; set; }        
        public decimal MayorGainPercent { get; set; }
        public decimal MinorGainPercent { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset? DisabledDate { get; set; }
        public string DisableNote { get; set; }
        public Guid? DeactivatorUserId { get; set; }        
        public virtual ProductDto Product { get; set; }
    }
}
