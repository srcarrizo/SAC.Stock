namespace SAC.Stock.Front.Models.BranchOffice
{
    using Buy;
    using Sale;
    using Shared;
    using System;
    using System.Collections.Generic;    
    public class BranchOfficeDpo
    {
        public BranchOfficeDpo()
        {
            Phones = new List<PhoneDpo>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AddressId { get; set; }
        public AddressDpo Address { get; set; }
        public ICollection<PhoneDpo> Phones { get; set; }
        public DateTimeOffset? ActivatedDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivateNote { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public ICollection<SaleDpo> Sales { get; set; }
        public ICollection<BuyDpo> Buys { get; set; }
    }
}