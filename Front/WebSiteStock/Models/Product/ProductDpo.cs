namespace SAC.Stock.Front.Models.Product
{
    using System;

    public class ProductDpo
    {
        public int Id { get; set; }
        public string Code { get; set; }        
        public string Name { get; set; }        
        public string Description { get; set; }
        public DateTimeOffset? DisabledDate { get; set; }
        public string DisableNote { get; set; }
        public bool ForSale { get; set; }
        public virtual SubCategoryDpo SubCategory { get; set; }        
        public ContainerDpo Container { get; set; }        
        public ProductPriceDpo CurrentProductPrice { get; set; }
    }
}