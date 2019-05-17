namespace SAC.Stock.Service.ProductContext
{
    using SAC.Seed.NLayer.Application;    
    using System.Collections.Generic;    
    internal class ContainerDto : EntityDto<int>
    {
        public string Name { get; set; }        
        public string Description { get; set; }
        public int Amount { get; set; }        
        public virtual ICollection<ProductDto> Products { get; set; }
        public virtual ICollection<ContainerDto> Containers { get; set; }
        public virtual ContainerDto ParentContainer { get; set; }
    }
}
