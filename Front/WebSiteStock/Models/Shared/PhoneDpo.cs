namespace SAC.Stock.Front.Models.Shared
{
    public class PhoneDpo
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string Number { get; set; }
        public bool? Mobile { get; set; }
        public string Name { get; set; }
        public int? TelcoId { get; set; }
    }
}