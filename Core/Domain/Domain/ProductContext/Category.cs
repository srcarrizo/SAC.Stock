namespace SAC.Stock.Domain.ProductContext
{    
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Seed.NLayer.Domain;

    public class Category : EntityAutoInc
    {        
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
