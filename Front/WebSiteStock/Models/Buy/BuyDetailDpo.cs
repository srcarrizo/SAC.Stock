namespace SAC.Stock.Front.Models.Buy
{
    using SAC.Stock.Front.Models.Product;    
    public class BuyDetailDpo
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }        
        public ProductDpo Product { get; set; }
    }
}