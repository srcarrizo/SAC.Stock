namespace SAC.Stock.Front.Models.Sale
{
    using SAC.Stock.Front.Models.BranchOffice;
    using SAC.Stock.Front.Models.Customer;
    using SAC.Stock.Front.Models.Transaction;
    using System;
    using System.Collections.Generic;    

    public class SaleDpo
    {
        public int Id { get; set; }
        public DateTimeOffset SaleDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivatedNote { get; set; }
        public BranchOfficeDpo BranchOffice { get; set; }
        public CustomerDpo Customer { get; set; }
        public BranchOfficeStaffDpo BranchOfficeStaff { get; set; }
        public string PaymentTypeCode { get; set; }
        public bool PreSale { get; set; }
        public bool MayorMinorSale { get; set; }
        public int? StockId { get; set; }
        public int? BoxId { get; set; }
        public ICollection<SaleDetailDpo> Detail { get; set; }                            
        public ICollection<TransactionDpo> Transactions { get; set; }
    }
}