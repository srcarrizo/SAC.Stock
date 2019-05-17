namespace SAC.Stock.Service.ProductContext
{
    using SAC.Seed.NLayer.Application;    
    using System.Collections.Generic;
    internal class CategoryDto : EntityDto<int>
    {                
        public string Name { get; set; }        
        public AreaDto Area { get; set; }        
        public virtual ICollection<SubCategoryDto> SubCategories { get; set; }
    }
}
