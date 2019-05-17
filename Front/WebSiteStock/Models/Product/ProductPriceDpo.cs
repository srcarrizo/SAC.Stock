namespace SAC.Stock.Front.Models.Product
{
    using System;    
    public class ProductPriceDpo
    {
        public int Id { get; set; }
        public decimal BuyMayorPrice { get; set; }
        public decimal MayorGainPercent { get; set; }
        public decimal MinorGainPercent { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset? DisabledDate { get; set; }
        public string DisableNote { get; set; }
        public Guid? DeactivatorUserId { get; set; }        
        public ProductDpo Product { get; set; }
    }
}