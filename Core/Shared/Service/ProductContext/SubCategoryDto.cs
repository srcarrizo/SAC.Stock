namespace SAC.Stock.Service.ProductContext
{
    using SAC.Seed.NLayer.Application;    
    using System.Collections.Generic;    

    internal class SubCategoryDto : EntityDto<int>
    {
        public string Name { get; set; }    
        public CategoryDto Category { get; set; }        
        public virtual ICollection<ProductDto> Products { get; set; }
    }
}
