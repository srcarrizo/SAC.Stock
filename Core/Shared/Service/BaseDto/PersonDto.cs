namespace SAC.Stock.Service.BaseDto
{
    using SAC.Seed.NLayer.Application;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal class PersonDto : EntityDto<Guid>
    {
        public PersonDto()
        {
            Phones = new List<PhoneDto>();
            Attributes = new Collection<AttributeValueDto>();
            ChangingPassword = false;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GenderCode { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string UidSerie { get; set; }
        public string UidCode { get; set; }
        public ICollection<PhoneDto> Phones { get; set; }
        public AddressDto Address { get; set; }
        public ICollection<AttributeValueDto> Attributes { get; set; }
        public bool ChangingPassword { get; set; }
    }
}