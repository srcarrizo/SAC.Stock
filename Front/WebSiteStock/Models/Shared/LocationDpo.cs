namespace SAC.Stock.Front.Models.Shared
{
    public class LocationDpo
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LocationTypeCode { get; set; }
        public int ParentLocationId { get; set; }
    }
}