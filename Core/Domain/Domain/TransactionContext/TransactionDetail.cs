namespace SAC.Stock.Domain.TransactionContext
{
    using BillContext;
    using Seed.NLayer.Domain;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TransactionDetail : EntityGuid
    {                
        public int Amount { get; set; }

        public int BillId { get; set; }
        public virtual Bill Bill { get; set; }

        [Required]
        public Guid TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }        
    }
}
