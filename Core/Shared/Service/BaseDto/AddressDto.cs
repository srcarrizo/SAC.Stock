namespace SAC.Stock.Service.BaseDto
{
    using Seed.NLayer.Application;
    internal class AddressDto : EntityDto<int>
    {
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string Floor { get; set; }
        public string Apartment { get; set; }    
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public int LocationId { get; set; }
        public LocationDto Location { get; set; }
    }
}
