namespace SAC.Stock.Domain.BranchOfficeContext
{
    using PhoneContext;
    using System;
    public class BranchOfficePhone : Phone
    {
        public Guid BranchOfficeId { get; set; }
    }
}
