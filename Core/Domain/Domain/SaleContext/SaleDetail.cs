namespace SAC.Stock.Domain.SaleContext
{
    using Seed.NLayer.Domain;
    using ProductContext;    

    public class SaleDetail : EntityAutoInc
    {
        public int Amount { get; set; }
        public double Price { get; set; }
        public int SaleId { get; set; }       
        public virtual Sale Sale { get; set; }        
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductPriceId { get; set; }
        public virtual ProductPrice ProductPrice { get; set; }
    }
}
