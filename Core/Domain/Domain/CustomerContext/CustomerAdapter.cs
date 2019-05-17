namespace SAC.Stock.Domain.CustomerContext
{
    using PersonContext;
    using Service.CustomerContext;
    internal static class CustomerAdapter
    {
        public static CustomerDto AdaptToCustomerDto(this Customer customer)
        {
            if (customer == null)
            {
                return null;
            }

            return new CustomerDto
            {
                Person = customer.Person.AdaptPersonDto(),
                Id = customer.Id,
                Name = customer.Name,
                CreatedDate = customer.CreatedDate,
                DeactivateDate = customer.DeativatedDate,
                DeativateNote = customer.DeativateNote
            };
        }

        public static Customer AdaptToCustomer(this CustomerDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Customer
            {
                CreatedDate = dto.CreatedDate,
                DeativatedDate = dto.DeactivateDate,
                DeativateNote = dto.DeativateNote,
                Id = dto.Id,
                Name = dto.Name,
                Person = dto.Person.AdaptPerson()
            };
        }
    }
}
