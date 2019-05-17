namespace SAC.Stock.Domain.StockContext
{
    using Seed.NLayer.Domain;    
    using ProductContext;
    using System.ComponentModel.DataAnnotations;

    public class StockDetail : EntityAutoInc
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Amount { get; set; }
        public int StockId { get; set; }

        public virtual Stock Stock { get; set; }
        public virtual Product Product { get; set; }
    }
}
