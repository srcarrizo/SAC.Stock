namespace SAC.Stock.Service.ProductContext
{
    using SAC.Seed.NLayer.Application;    
    using System.Collections.Generic;    
    internal class AreaDto : EntityDto<int>
    {        
        public string Name { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
    }
}
