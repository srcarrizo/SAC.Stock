namespace SAC.Stock.Domain.PersonContext
{
    using SAC.Stock.Domain.AttributeValueContext;
    using System;
    public class PersonAttributeValue : AttributeValue
    {
        public Guid PersonId { get; set; }
    }
}
