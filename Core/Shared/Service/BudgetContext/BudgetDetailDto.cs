using SAC.Seed.NLayer.Application;

namespace SAC.Stock.Service.BudgetContext
{
    using ProductContext;

    internal class BudgetDetailDto : EntityDto<int>
    {
        public int Amount { get; set; }
        public int BudgetId { get; set; }
        public BudgetDto Budget { get; set; }
        public double Price { get; set; }
        public ProductDto Product { get; set; }
        public int ProductId { get; set; }
    }
}