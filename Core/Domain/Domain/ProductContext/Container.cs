namespace SAC.Stock.Domain.ProductContext
{
    using Seed.NLayer.Domain;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Container : EntityAutoInc
    {
        [Required(ErrorMessage = "El [Nombre del Contenedor] es requerido.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El [Descripción] es requerido.")]
        public string Description { get; set; }
        public int Amount { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Container> Containers { get; set; }
        public int? ParentContainerId { get; set; }
        public virtual Container ParentContainer { get; set; }
    }
}
