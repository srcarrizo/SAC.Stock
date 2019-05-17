namespace SAC.Stock.Front.Models
{
    using SAC.Seed.NLayer.Data;
    public class QueryInfo
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public SortInfo[] SortInfo { get; set; }
        
        public FilterInfo FilterInfo { get; set; }
    }
}