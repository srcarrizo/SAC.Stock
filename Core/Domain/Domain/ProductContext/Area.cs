namespace SAC.Stock.Domain.ProductContext
{
    using Seed.NLayer.Domain;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Area : EntityAutoInc
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
