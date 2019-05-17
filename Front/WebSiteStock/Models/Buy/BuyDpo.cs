namespace SAC.Stock.Front.Models.Buy
{
    using SAC.Stock.Front.Models.BranchOffice;
    using SAC.Stock.Front.Models.Provinder;
    using SAC.Stock.Front.Models.Transaction;
    using System;
    using System.Collections.Generic;    

    public class BuyDpo
    {
        public int Id { get; set; }
        public DateTimeOffset BuyDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivatedNote { get; set; }
        public BranchOfficeDpo BranchOffice { get; set; }
        public ProviderDpo Provider { get; set; }
        public BranchOfficeStaffDpo BranchOfficeStaff { get; set; }
        public string PaymentTypeCode { get; set; }
        public int? BoxId { get; set; }
        public int? StockId { get; set; }
        public ICollection<BuyDetailDpo> Detail { get; set; }                            
        public ICollection<TransactionDpo> Transactions { get; set; }
    }
}