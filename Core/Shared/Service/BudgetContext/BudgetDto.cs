namespace SAC.Stock.Service.BudgetContext
{
    using Seed.NLayer.Application;
    using System;
    using System.Collections.Generic;
    using BranchOfficeContext;
    using CustomerContext;

    internal class BudgetDto : EntityDto<int>
    {
        public DateTimeOffset BudgetDate { get; set; }        
        public string NonCustomerName { get; set; }
        public Guid BranchOfficeId { get; set; }
        public BranchOfficeDto BranchOffice { get; set; }
        public Guid? CustomerId { get; set; }
        public CustomerDto Customer { get; set; }
        public Guid BranchOfficeStaffId { get; set; }
        public BranchOfficeStaffDto BranchOfficeStaff { get; set; }
        public string PaymentTypeCode { get; set; }
        public ICollection<BudgetDetailDto> Detail { get; set; }
    }
}
