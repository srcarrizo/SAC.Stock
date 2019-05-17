namespace SAC.Stock.Front.Models.Shared
{
    public class AddressDpo
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string Floor { get; set; }
        public string Apartment { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public int LocationId { get; set; }
        public LocationDpo Location { get; set; }
    }
}