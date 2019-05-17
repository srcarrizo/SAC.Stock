namespace SAC.Stock.Service.BuyContext
{    
    using System.Collections.Generic;
    
    internal class BuysTotalCountDto
    {
        public List<BuyDto> Buys { get; set; }        
        public decimal Total { get; set; }
    }
}
