namespace SAC.Stock.Front.Models.Shared
{
    using System;
    using System.Collections.Generic;

    public class PersonDpo
    {
        public PersonDpo()
        {
            UidSerieCode = UidSerie + ": " + UidCode;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }                
        public string Email { get; set; }   
        public string UidSerieCode { get; set; }
        public string UidSerie { get; set; }
        public string UidCode { get; set; }
        public DateTime BirthDate { get; set; }
        public List<PhoneDpo> Phones { get; set; }
        public AddressDpo Address { get; set; }
    }
}