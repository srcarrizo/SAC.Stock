namespace SAC.Stock.Domain.TransactionContext
{
    using Seed.NLayer.Domain;
    using BranchOfficeContext;
    using BuyContext;
    using SaleContext;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using SAC.Stock.Domain.BoxContext;

    public class Transaction : EntityGuid
    {
        [Required]
        public string Name { get; set; }        
        public string Description { get; set; }
        [Required]
        public DateTimeOffset TransactionDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivatedNote { get; set; }

        [Required]
        public Guid BranchOfficeId { get; set; }
        public virtual BranchOffice BranchOffice { get; set; }

        [Required]
        public Guid BranchOfficeStaffId { get; set; }
        public virtual BranchOfficeStaff BranchOfficeStaff { get; set; }

        [Required]
        public bool TransactionTypeInOut { get; set; }        
        public virtual ICollection<TransactionDetail> Detail { get; set; }
        public int? BuyId { get; set; }        
        public virtual Buy Buy { get; set; }
        public int? SaleId { get; set; }
        public virtual Sale Sale { get; set; }
        public int? BoxId { get; set; }
        public virtual Box Box { get; set; }
    }
}