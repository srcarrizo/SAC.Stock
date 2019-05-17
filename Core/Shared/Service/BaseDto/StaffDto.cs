namespace SAC.Stock.Service.BaseDto
{
    using SAC.Seed.NLayer.Application;
    using System;
    internal class StaffDto : EntityDto<Guid>
    {        
        public PersonDto Person { get; set; }
    }
}
