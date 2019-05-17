namespace SAC.Stock.Domain.BillContext
{
    using Seed.NLayer.Domain;    
    using System.ComponentModel.DataAnnotations;
    public class Bill : EntityAutoInc
    {
        [Required]
        [StringLength(200)]
        public string Code { get; set; }

        public string Name { get; set; }

        [Required]
        public int Value { get; set; }

        [Required]
        public int BillUnitTypeId { get; set; }
        public virtual BillUnitType BillUnitType { get; set; }
    }
}