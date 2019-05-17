namespace SAC.Stock.Service.TransactionContext
{
    using SAC.Seed.NLayer.Application;
    using SAC.Stock.Service.BoxContext;
    using SAC.Stock.Service.BranchOfficeContext;
    using SAC.Stock.Service.BuyContext;
    using SAC.Stock.Service.SaleContext;
    using System;
    using System.Collections.Generic;
    internal class TransactionDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivatedNote { get; set; }       
        public BranchOfficeDto BranchOffice { get; set; }
        public BranchOfficeStaffDto BranchOfficeStaff { get; set; }
        public Guid BranchOfficeId { get; set; }
        public Guid BranchOfficeStaffId { get; set; }
        public bool TransactionTypeInOut { get; set; }        
        public ICollection<TransactionDetailDto> Detail { get; set; }        
        public int? BuyId { get; set; }        
        public int? SaleId { get; set; }        
        public int? BoxId { get; set; }
        public BuyDto Buy { get; set; }
        public SaleDto Sale { get; set; }
        public BoxDto Box { get; set; }
    }
}
