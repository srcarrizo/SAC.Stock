namespace SAC.Stock.Domain.BoxContext
{
    using SAC.Seed.NLayer.Domain;
    using SAC.Stock.Domain.BillContext;    
    using System.ComponentModel.DataAnnotations;    

    public class BoxDetail : EntityAutoInc
    {
        public int Amount { get; set; }
        public int BillId { get; set; }
        public virtual Bill Bill { get; set; }

        [Required]
        public int BoxId { get; set; }
        public virtual Box Box { get; set; }   
    }
}