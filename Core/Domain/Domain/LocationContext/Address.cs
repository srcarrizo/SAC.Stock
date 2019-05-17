namespace SAC.Stock.Domain.LocationContext
{
    using System.ComponentModel.DataAnnotations;
    using Seed.NLayer.Domain;
    public class Address : EntityAutoInc
    {           
        [Required]
        public string Street { get; set; }
        [Required]
        public string StreetNumber { get; set; }
        public string Floor { get; set; }
        public string Apartment { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
    }
}
