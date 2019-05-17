namespace SAC.Stock.Domain.CustomerContext
{    
    using Seed.NLayer.Data;
    using Seed.NLayer.Data.Ordering;
    using Seed.NLayer.Domain;
    using Seed.NLayer.Domain.Specification;    
    using Code;    
    using PersonContext;    
    using Service.CustomerContext;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
        
    internal class CustomerService
    {
        private readonly IDataContext context;

        internal CustomerService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Customer, Guid> ViewCustomer
        {
            get
            {
                return context.GetView<Customer, Guid>();
            }
        }

        internal Customer AddCustomer(CustomerDto customerInfo)
        {
            var customer = NewCustomer(customerInfo);
            context.ApplyChanges();

            return customer;
        }

        internal Customer GetCustomer(Guid id)
        {
            return ViewCustomer.Get(id);
        }

        internal Customer GetCustomer(string uidCode, string uidSerie)
        {
            return ViewCustomer.GetFirst(SpecCustomerFilterByUid(uidCode, uidSerie));
        }

        internal IEnumerable<Customer> GetCustomerForMailing(ICollection<FilterInfo> filter)
        {
            return ViewCustomer.Query(GetCustomerSpecification(filter));
        }

        internal Customer ModifyCustomer(CustomerDto customerInfo)
        {
            var customer = GetCustomer(customerInfo.Id);

            if (customer == null)
            {
                throw BusinessRulesCode.CustomerNotExists.NewBusinessException();
            }
         
            PrepareCustomer(customerInfo);            
            var personSvc = new PersonService(context);
            personSvc.UpdatePerson(customerInfo.Person, customer.Person);          
            ViewCustomer.Modify(customer);
            context.ApplyChanges();

            return customer;
        }

        internal IEnumerable<Customer> QueryCustomer(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<Customer>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Customer, string>(pt => pt.Person.LastName, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Customer.LastName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Customer, string>(pt => pt.Person.LastName, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Customer.FirstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Customer, string>(pt => pt.Person.FirstName, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Customer.Uid, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Customer, string>(pt => pt.Person.UidCode, info.Direction));
                    orderByExpressions.Add(new OrderByExpression<Customer, string>(pt => pt.Person.UidSerie, info.Direction));
                }
               
                if (info.Field.Equals(SortQuery.Customer.Email, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Customer, string>(pt => pt.Person.Email, info.Direction));
                }               
            }

            return ViewCustomer.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetCustomerSpecification(filterInfo));
        }

        private static void PrepareCustomer(CustomerDto customerInfo)
        {
            customerInfo.Person.Email = string.IsNullOrWhiteSpace(customerInfo.Person.Email) ? null : customerInfo.Person.Email.ToLowerInvariant().Trim();
            customerInfo.Person.FirstName = string.IsNullOrWhiteSpace(customerInfo.Person.FirstName) ? null : FirstToCamelCase(customerInfo.Person.FirstName.Trim());
            customerInfo.Person.LastName = string.IsNullOrWhiteSpace(customerInfo.Person.LastName) ? null : FirstToCamelCase(customerInfo.Person.LastName.Trim());
            customerInfo.Person.UidSerie = string.IsNullOrWhiteSpace(customerInfo.Person.UidSerie) ? null : customerInfo.Person.UidSerie.Trim();

            if (customerInfo.Person.Address != null)
            {
                customerInfo.Person.Address.Apartment = string.IsNullOrWhiteSpace(customerInfo.Person.Address.Apartment) ? null : FirstToCapital(customerInfo.Person.Address.Apartment.Trim());
                customerInfo.Person.Address.Floor = string.IsNullOrWhiteSpace(customerInfo.Person.Address.Floor) ? null : customerInfo.Person.Address.Floor.Trim();
                customerInfo.Person.Address.Neighborhood = string.IsNullOrWhiteSpace(customerInfo.Person.Address.Neighborhood) ? null : FirstToCapital(customerInfo.Person.Address.Neighborhood.Trim());
                customerInfo.Person.Address.Street = string.IsNullOrWhiteSpace(customerInfo.Person.Address.Street) ? null : FirstToCapital(customerInfo.Person.Address.Street.Trim());
                customerInfo.Person.Address.StreetNumber = string.IsNullOrWhiteSpace(customerInfo.Person.Address.StreetNumber) ? null : customerInfo.Person.Address.StreetNumber.Trim();
                customerInfo.Person.Address.ZipCode = string.IsNullOrWhiteSpace(customerInfo.Person.Address.ZipCode) ? null : customerInfo.Person.Address.ZipCode.Trim();
            }

            if (customerInfo.Person.Phones.Count <= 0)
            {
                return;
            }

            foreach (var phoneDto in customerInfo.Person.Phones)
            {
                phoneDto.AreaCode = string.IsNullOrWhiteSpace(phoneDto.AreaCode) ? null : phoneDto.AreaCode.Trim();
                phoneDto.CountryCode = string.IsNullOrWhiteSpace(phoneDto.CountryCode) ? null : phoneDto.CountryCode.Trim();
                phoneDto.Name = string.IsNullOrWhiteSpace(phoneDto.Name) ? null : FirstToCapital(phoneDto.Name.Trim());
                phoneDto.Number = string.IsNullOrWhiteSpace(phoneDto.Number) ? null : phoneDto.Number.Trim();
            }
        }

        private static string FirstToCapital(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return data;
            }

            var chars = data.ToCharArray();
            chars[0] = chars[0].ToString(CultureInfo.InvariantCulture).ToUpperInvariant().ToCharArray()[0];
            return new string(chars);
        }

        private static string FirstToCamelCase(string data)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.ToLowerInvariant());
        }

        private static Specification<Customer> GetCustomerSpecification(IEnumerable<FilterInfo> filterInfo)
        {
            Specification<Customer> spec = new TrueSpecification<Customer>();
            if (filterInfo != null)
            {
                foreach (var info in filterInfo)
                {
                    if (info.Spec.Equals(SpecFilter.Customer.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                    {
                        spec &= SpecCustomerByFullSearch(info.Value);
                    }

                    if (info.Spec.Equals(SpecFilter.Customer.Location, StringComparison.InvariantCultureIgnoreCase))
                    {
                        spec &= SpecCustomerByLocation(info.Value);
                    }

                    if (info.Spec.Equals(SpecFilter.Customer.Uid, StringComparison.InvariantCultureIgnoreCase))
                    {
                        spec &= SpecCustomerByUidSearch(info.Value);
                    }
                }
            }

            return spec;
        }

        private static Specification<Customer> SpecCustomerFilterByUid(string uidCode, string uidSerie)
        {
            return
              new DirectSpecification<Customer>(
                c =>
                c.Person.UidSerie.Equals(uidSerie, StringComparison.InvariantCultureIgnoreCase)
                && c.Person.UidCode.Equals(uidCode, StringComparison.InvariantCultureIgnoreCase));
        }

        private static Specification<Customer> SpecCustomerByFullSearch(string value)
        {
            Specification<Customer> result = new DirectSpecification<Customer>(c => c.Person.LastName.ToLower().Contains(value.ToLower()));
            result |= new DirectSpecification<Customer>(c => c.Person.FirstName.ToLower().Contains(value.ToLower()));            
            result |= new DirectSpecification<Customer>(c => c.Person.Email.ToLower().Contains(value.ToLower()));
            return result;
        }

        private static Specification<Customer> SpecCustomerByLocation(string locationId)
        {
            var locationIdValue = int.Parse(locationId);
            return new AndSpecification<Customer>(
              new DirectSpecification<Customer>(c => c.Person.AddressId.HasValue),
              new OrSpecification<Customer>(
                new DirectSpecification<Customer>(c => c.Person.Address.LocationId.Equals(locationIdValue)),
                new AndSpecification<Customer>(
                  new DirectSpecification<Customer>(c => c.Person.Address.Location.ParentLocationId.HasValue),
                  new DirectSpecification<Customer>(c => ((int)c.Person.Address.Location.ParentLocationId).Equals(locationIdValue)))));
        }

        private static Specification<Customer> SpecCustomerByUidSearch(string value)
        {
            Specification<Customer> result = new DirectSpecification<Customer>(c => c.Person.UidSerie.ToLower().Contains(value.ToLower()));
            return result;
        }

        private Customer NewCustomer(CustomerDto customerInfo)
        {
            var existingCustomer = GetCustomer(customerInfo.Person.UidCode, customerInfo.Person.UidSerie);
            if (existingCustomer != null)
            {
                throw BusinessRulesCode.CustomerExists.NewBusinessException();
            }           

            PrepareCustomer(customerInfo);           

            var personSvc = new PersonService(context);
            var person = personSvc.GetOrNewPerson(customerInfo.Person, true);
            var now = DateTimeOffset.UtcNow;
            var customer = new Customer
            {
                Id = person.Id,
                Name = customerInfo.Name,
                Person = person,
                CreatedDate = now                
            };
            
            ViewCustomer.Add(customer);

            return customer;
        }
    }
}
