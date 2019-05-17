namespace SAC.Stock.Front.Models.Budget
{
    using BranchOffice;
    using Customer;
    using System;
    using System.Collections.Generic;

    public class BudgetDpo
    {
        public int Id { get; set; }
        public DateTimeOffset BudgetDate { get; set; }
        public string NonCustomerName { get; set; }
        public string Name { get; set; }
        public Guid BranchOfficeId { get; set; }
        public BranchOfficeDpo BranchOffice { get; set; }
        public Guid? CustomerId { get; set; }
        public CustomerDpo Customer { get; set; }
        public Guid BranchOfficeStaffId { get; set; }
        public BranchOfficeStaffDpo BranchOfficeStaff { get; set; }
        public string PaymentTypeCode { get; set; }
        public ICollection<BudgetDetailDpo> Detail { get; set; }
    }
}