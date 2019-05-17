namespace SAC.Stock.Domain.ProfileContext
{
    using SAC.Seed.NLayer.Domain;    
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;    
    public class Profile : EntityAutoInc
    {
        [Required(ErrorMessage = "El [Código del Perfil] es requerido.")]
        [StringLength(200, ErrorMessage = "El [Código] no puede ser mas extenso de 200 caracteres.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "El [Nombre del Perfil] es requerido.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La [Descripción del Perfil] es requerido.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La [Jerarquía del Perfil] es requerido.")]
        public int Hierarchy { get; set; }

        [Required(ErrorMessage = "El [Ámbito del Perfil] es requerido.")]
        public string Scope { get; set; }

        public virtual ICollection<RolesComposition> RolesComposition { get; set; }
    }
}
