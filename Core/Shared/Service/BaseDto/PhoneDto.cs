namespace SAC.Stock.Service.BaseDto
{
    using SAC.Seed.NLayer.Application;
    internal class PhoneDto : EntityDto<int>
    {
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string Number { get; set; }
        public bool? Mobile { get; set; }
        public string Name { get; set; }
        public int? TelcoId { get; set; }
    }
}
