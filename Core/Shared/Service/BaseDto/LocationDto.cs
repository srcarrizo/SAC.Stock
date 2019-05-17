namespace SAC.Stock.Service.BaseDto
{
    using Seed.NLayer.Application;
    internal class LocationDto : EntityDto<int>
    {
        public string Code { get; set; }       
        public string Name { get; set; }
        public string Description { get; set; }
        public string LocationTypeCode { get; set; }        
        public int? ParentLocationId { get; set; }
        public LocationDto ParentLocation { get; set; }
    }
}
