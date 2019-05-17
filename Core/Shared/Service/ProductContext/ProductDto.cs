namespace SAC.Stock.Service.ProductContext
{
    using SAC.Seed.NLayer.Application;    
    using System;
    using System.Collections.Generic;

    internal class ProductDto : EntityDto<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }     
        public string Description { get; set; }
        public DateTimeOffset? DisabledDate { get; set; }
        public string DisableNote { get; set; }
        public bool ForSale { get; set; }                        
        public SubCategoryDto SubCategory { get; set; }        
        public virtual ContainerDto Container { get; set; }
        public virtual ICollection<ProductPriceDto> ProductPrices { get; set; }
    }
}
