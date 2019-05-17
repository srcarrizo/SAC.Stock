namespace SAC.Stock.Service.CustomerContext
{
    using Seed.NLayer.Application;
    using BaseDto;
    using System;
    internal class CustomerDto : EntityDto<Guid>
    {
        public CustomerDto()
        {
            Person = new PersonDto();
        }

        public string Name { get; set; }

        public PersonDto Person { get; set; }

        public Guid UserId { get; set; }
        
        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? DeactivateDate { get; set; }       

        public string DeativateNote { get; set; }
    }
}
