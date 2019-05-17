namespace SAC.Stock.Domain.CustomerContext
{
    using System;
    using Seed.NLayer.Domain;
    using PersonContext;
    using System.ComponentModel.DataAnnotations;

    public class Customer : EntityGuid
    {
        [Required(ErrorMessage = "El [Nombre del proveedor] es requerido.")]
        public string Name { get; set; }
        public virtual Person Person { get; set; }
        public DateTimeOffset CreatedDate { get; set; }        
        public DateTimeOffset? DeativatedDate { get; set; }
        public string DeativateNote { get; set; }                
    }
}
