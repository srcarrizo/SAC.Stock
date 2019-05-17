namespace SAC.Stock.Front.Models.Budget
{
    using Product;

    public class BudgetDetailDpo
    {
        public int Id { get; set; }        
        public int Amount { get; set; }                
        public double Price { get; set; }
        public int ProductId { get; set; }
        public ProductDpo Product { get; set; }
    }
}