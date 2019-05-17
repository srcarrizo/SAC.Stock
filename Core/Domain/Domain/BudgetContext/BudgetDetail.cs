namespace SAC.Stock.Domain.BudgetContext
{
    using Seed.NLayer.Domain;
    using ProductContext;
    public class BudgetDetail : EntityAutoInc
    {
        public int Amount { get; set; }
        public double Price { get; set; }
        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}