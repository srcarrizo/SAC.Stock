namespace SAC.Stock.Domain.ProviderContext
{
    using System;    
    using Seed.NLayer.Domain;
    using PersonContext;
    using System.ComponentModel.DataAnnotations;

    public class Provider : EntityGuid
    {
        [Required(ErrorMessage = "El [Nombre del proveedor] es requerido.")]
        public string Name { get; set; }        
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? DeativatedDate { get; set; }
        public string DeativateNote { get; set; }
        public virtual Person Person { get; set; }
    }
}
