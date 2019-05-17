namespace SAC.Stock.Front.Models.Transaction
{
    using SAC.Stock.Front.Models.BranchOffice;
    using System;
    using System.Collections.Generic;    
    public class TransactionDpo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivatedNote { get; set; }    
        public BranchOfficeDpo BranchOffice { get; set; }
        public BranchOfficeStaffDpo BranchOfficeStaff { get; set; }
        public Guid BranchOfficeId { get; set; }               
        public Guid BranchOfficeStaffId { get; set; }                
        public bool TransactionTypeInOut { get; set; }
        public virtual ICollection<TransactionDetailDpo> Detail { get; set; }
        public int? BuyId { get; set; }        
        public int? SaleId { get; set; }
        public int? BoxId { get; set; }
    }
}