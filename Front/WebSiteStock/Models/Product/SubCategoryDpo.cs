namespace SAC.Stock.Front.Models.Product
{
    using System.Collections.Generic;
    public class SubCategoryDpo
    {
        public int Id { get; set; }
        public string Name { get; set; }                
        public virtual CategoryDpo Category { get; set; }
        public virtual ICollection<ProductDpo> Products { get; set; }
    }
}