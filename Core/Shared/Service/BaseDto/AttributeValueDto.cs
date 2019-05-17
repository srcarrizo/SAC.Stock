namespace SAC.Stock.Service.BaseDto
{
    using SAC.Seed.NLayer.Application;
    internal class AttributeValueDto : EntityDto<int>
    {
        public string AttributeCode { get; set; }
        public string Value { get; set; }
    }
}