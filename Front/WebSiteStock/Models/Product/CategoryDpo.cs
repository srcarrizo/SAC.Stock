namespace SAC.Stock.Front.Models.Product
{    
    using System.Collections.Generic;
    public class CategoryDpo
    {
        public int Id { get; set; }
        public string Name { get; set; }                
        public AreaDpo Area { get; set; }
        public virtual ICollection<SubCategoryDpo> SubCategories { get; set; }
    }
}