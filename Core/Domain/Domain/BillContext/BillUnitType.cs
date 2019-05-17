namespace SAC.Stock.Domain.BillContext
{
    using Seed.NLayer.Domain;
    using System.ComponentModel.DataAnnotations;
    public class BillUnitType : EntityAutoInc
    {
        public BillUnitType()
        {
            IsDecimal = false;
        }

        [Required]
        [StringLength(200)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]        
        public bool IsDecimal { get; set; }
    }
}