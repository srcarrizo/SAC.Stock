namespace SAC.Stock.Domain.BuyContext
{
    using Seed.NLayer.Domain;
    using BranchOfficeContext;
    using ProviderContext;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TransactionContext;
    using SAC.Stock.Domain.BoxContext;
    using SAC.Stock.Domain.StockContext;

    public class Buy : EntityAutoInc
    {
        [Required]
        public DateTimeOffset BuyDate { get; set; }

        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivatedNote { get; set; }

        [Required]
        public Guid BranchOfficeId { get; set; }
        public virtual BranchOffice BranchOffice { get; set; }

        [Required]
        public Guid ProviderId { get; set; }
        public virtual Provider Provider { get; set; }

        [Required]
        public Guid BranchOfficeStaffId { get; set; }
        public virtual BranchOfficeStaff BranchOfficeStaff { get; set; }
        public string PaymentTypeCode { get; set; }

        public int? BoxId { get; set; }
        public virtual Box Box { get; set; }

        public int? StockId { get; set; }
        public virtual Stock Stock { get; set; }

        public virtual ICollection<BuyDetail> Detail { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}