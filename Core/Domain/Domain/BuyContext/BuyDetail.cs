namespace SAC.Stock.Domain.BuyContext
{
    using Seed.NLayer.Domain;
    using ProductContext;
    public class BuyDetail : EntityAutoInc
    {
        public int Amount { get; set; }
        public double Price { get; set; }
        public int BuyId { get; set; }
        public virtual Buy Buy { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
