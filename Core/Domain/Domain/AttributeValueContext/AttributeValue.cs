namespace SAC.Stock.Domain.AttributeValueContext
{
    using SAC.Seed.NLayer.Domain;
    public abstract class AttributeValue : EntityAutoInc
    {
        public string AttributeCode { get; set; }
        public string Value { get; set; }
    }
}
