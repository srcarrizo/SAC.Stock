namespace SAC.Stock.Service.SaleContext
{
    using System.Collections.Generic; 
    internal class SalesTotalCountDto
    {
        public List<SaleDto> Sales { get; set; }        
        public List<SaleDto> PreSales { get; set; }
        public decimal Total { get; set; }
        public decimal PreSaleTotal { get; set; }
    }
}
