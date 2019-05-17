namespace SAC.Stock.Domain.PhoneContext
{
    using System.ComponentModel.DataAnnotations;
    using Seed.NLayer.Domain;
    public class Telco : EntityAutoInc
    {        
        [Required(ErrorMessage = "El [Código] es requerido.")]
        [StringLength(100, ErrorMessage = "El [Código] no puede ser mas extenso de 100 caracteres.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "El [Nombre de Producto] es requerido.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El [Descripción] es requerido.")]
        public string Description { get; set; }
    }
}
