namespace SAC.Stock.Domain.LocationContext
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Seed.NLayer.Domain;

    public class Location : EntityAutoInc
    {       
        [StringLength(100, ErrorMessage = "El [Código] no puede ser mas extenso de 100 caracteres.")]
        public string Code { get; set; }
        
        [Required(ErrorMessage = "El [Nombre] es requerido.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string LocationTypeCode { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public int? ParentLocationId { get; set; }
        public virtual Location ParentLocation { get; set; }               
    }
}
