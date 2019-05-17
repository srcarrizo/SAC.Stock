namespace SAC.Stock.Service.ProviderContext
{
    using Seed.NLayer.Application;
    using BaseDto;
    using System;
    internal class ProviderDto : EntityDto<Guid>
    {
        public ProviderDto()
        {
            Person = new PersonDto();
        }

        public string Name { get; set; }

        public PersonDto Person { get; set; }        
        
        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? DeactivateDate { get; set; }

        public string DeativateNote { get; set; }
    }
}
