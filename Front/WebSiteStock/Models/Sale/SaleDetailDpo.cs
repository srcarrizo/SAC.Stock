namespace SAC.Stock.Front.Models.Sale
{
    using SAC.Stock.Front.Models.Product;    
    public class SaleDetailDpo
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }        
        public ProductDpo Product { get; set; }
    }
}