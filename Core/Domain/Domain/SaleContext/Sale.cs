namespace SAC.Stock.Domain.SaleContext
{
    using BranchOfficeContext;
    using CustomerContext;
    using TransactionContext;
    using Seed.NLayer.Domain;    
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using SAC.Stock.Domain.BoxContext;
    using SAC.Stock.Domain.StockContext;
    using SAC.Stock.Domain.ProductContext;

    public class Sale : EntityAutoInc
    {
        [Required]
        public DateTimeOffset SaleDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivatedNote { get; set; }

        [Required]
        public Guid BranchOfficeId { get; set; }
        public virtual BranchOffice BranchOffice { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Required]
        public Guid BranchOfficeStaffId { get; set; }
        public virtual BranchOfficeStaff BranchOfficeStaff { get; set; }
        public string PaymentTypeCode { get; set; }

        public int? BoxId { get; set; }
        public virtual Box Box { get; set; }

        public int? StockId { get; set; }
        public virtual Stock Stock { get; set; }

        public bool PreSale { get; set; }
        public bool MayorMinorSale { get; set; }

        public virtual ICollection<SaleDetail> Detail { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }        
    }
}
