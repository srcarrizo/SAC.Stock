namespace SAC.Stock.Service.BaseDto
{
    using SAC.Seed.NLayer.Application;
    internal class TelcoDto: EntityDto<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}