namespace SAC.Stock.Domain.BudgetContext
{
    using System.ComponentModel.DataAnnotations;
    using Seed.NLayer.Domain;
    using BranchOfficeContext;
    using CustomerContext;
    using System;
    using System.Collections.Generic;
    public class Budget : EntityAutoInc
    {
        public DateTimeOffset BudgetDate { get; set; }
        [MaxLength(250)]
        public string NonCustomerName { get; set; }
        public Guid BranchOfficeId { get; set; }
        public virtual BranchOffice BranchOffice { get; set; }
        public Guid? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public Guid BranchOfficeStaffId { get; set; }
        public virtual BranchOfficeStaff BranchOfficeStaff { get; set; }
        public string PaymentTypeCode { get; set; }
        public virtual ICollection<BudgetDetail> Detail { get; set; }
    }
}
