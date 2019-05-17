namespace SAC.Stock.Front.Models.Provinder
{
    using SAC.Stock.Front.Models.Shared;
    using System;
    using System.Collections.Generic;

    public class ProviderDpo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? DeativatedDate { get; set; }
        public string DeativateNote { get; set; }
        public string UidCode { get; set; }
        public string UidSerie { get; set; }
        public Guid PersonId { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public string GenderCode { get; set; }        
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string UidCodeSerie { get; set; }        
        public virtual ICollection<PhoneDpo> Phones { get; set; }
        public int? AddressId { get; set; }
        public virtual AddressDpo Address { get; set; }        
    }
}