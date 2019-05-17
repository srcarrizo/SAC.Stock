namespace SAC.Stock.Domain.ProductContext
{
    using Seed.NLayer.Domain;    
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class SubCategory : EntityAutoInc
    {       
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set;}
        public virtual ICollection<Product> Products { get; set; }
    }
}
