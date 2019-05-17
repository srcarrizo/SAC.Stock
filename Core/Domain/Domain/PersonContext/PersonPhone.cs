namespace SAC.Stock.Domain.PersonContext
{
    using System;
    using PhoneContext;
    public class PersonPhone : Phone
    {
        public Guid PersonId { get; set; }
    }
}
