namespace SAC.Stock.Front.Models.Product
{
    using System.Collections.Generic;    
    public class ContainerDpo
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public string Description { get; set; }
        public int Amount { get; set; }
        public virtual ICollection<ProductDpo> Products { get; set; }
        public virtual ICollection<ContainerDpo> Containers { get; set; }        
        public ContainerDpo ParentContainer { get; set; }
    }
}