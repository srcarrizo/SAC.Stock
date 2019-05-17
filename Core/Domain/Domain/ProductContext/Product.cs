namespace SAC.Stock.Domain.ProductContext
{
    using Seed.NLayer.Domain;
    using System;    
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Product : EntityAutoInc
    {
        [Required(ErrorMessage = "El [Code de Producto] es requerido.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "El [Nombre de Producto] es requerido.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El [Descripción] es requerido.")]
        public string Description { get; set; }        
        public DateTimeOffset? DisabledDate { get; set; }
        public string DisableNote { get; set; }
        public bool ForSale { get; set; }

        [Required]
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }

        [Required]
        public int ContainerId { get; set; }
        public virtual Container Container { get; set; }

        [Required]
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
    }
}