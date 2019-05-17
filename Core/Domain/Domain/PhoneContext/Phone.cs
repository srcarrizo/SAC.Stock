namespace SAC.Stock.Domain.PhoneContext
{
    using System.ComponentModel.DataAnnotations;
    using Seed.NLayer.Domain;

    public class Phone : EntityAutoInc
    {        
        [Required(ErrorMessage = "El [Código de País] es requerido.")]
        public string CountryCode { get; set; }
       
        [Required(ErrorMessage = "El [Código de Area] es requerido.")]
        public string AreaCode { get; set; }
        
        [Required(ErrorMessage = "El [Número telefónico] es requerido.")]
        public string Number { get; set; }
        public bool? Mobile { get; set; }        
        public string Name { get; set; }        
        public virtual Telco Telco { get; set; }        
        public int? TelcoId { get; set; }
    }
}
