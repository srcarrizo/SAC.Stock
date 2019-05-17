namespace SAC.Stock.Domain.ProviderContext
{    
    using Seed.NLayer.Data;
    using Seed.NLayer.Data.Ordering;
    using Seed.NLayer.Domain;
    using Seed.NLayer.Domain.Specification;
    using Code;    
    using PersonContext;
    using Service.ProviderContext;
    using System;
    using System.Collections.Generic;
    using System.Globalization;    

    internal class ProviderService
    {
        private readonly IDataContext context;

        internal ProviderService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Provider, Guid> ViewProvider
        {
            get
            {
                return context.GetView<Provider, Guid>();
            }
        }

        internal Provider AddProvider(ProviderDto providerInfo)
        {
            var provider = NewProvider(providerInfo);
            context.ApplyChanges();

            return provider;
        }

        internal Provider GetProvider(Guid id)
        {
            return ViewProvider.Get(id);
        }

        internal Provider GetProvider(string uidCode, string uidSerie)
        {
            return ViewProvider.GetFirst(SpecProviderFilterByUid(uidCode, uidSerie));
        }

        internal IEnumerable<Provider> GetProviderForMailing(ICollection<FilterInfo> filter)
        {
            return ViewProvider.Query(GetProviderSpecification(filter));
        }

        internal Provider ModifyProvider(ProviderDto ProviderInfo)
        {
            var provider = this.GetProvider(ProviderInfo.Id);

            if (provider == null)
            {
                throw BusinessRulesCode.ProviderNotExists.NewBusinessException();
            }

            PrepareProvider(ProviderInfo);
           
            var personSvc = new PersonService(context);
            personSvc.UpdatePerson(ProviderInfo.Person, provider.Person);
          
            ViewProvider.Modify(provider);
            context.ApplyChanges();

            return provider;
        }

        internal IEnumerable<Provider> QueryProvider(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<Provider>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Provider.LastName, StringComparison.InvariantCultureIgnoreCase) || info.Field.Equals(SortQuery.Common.Default, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Provider, string>(pt => pt.Person.LastName, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Provider.FirstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Provider, string>(pt => pt.Person.FirstName, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Provider.Uid, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Provider, string>(pt => pt.Person.UidCode, info.Direction));
                    orderByExpressions.Add(new OrderByExpression<Provider, string>(pt => pt.Person.UidSerie, info.Direction));
                }              

                if (info.Field.Equals(SortQuery.Provider.Email, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Provider, string>(pt => pt.Person.Email, info.Direction));
                }              
            }

            return ViewProvider.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetProviderSpecification(filterInfo));
        }

        private static void PrepareProvider(ProviderDto providerInfo)
        {
            providerInfo.Person.Email = string.IsNullOrWhiteSpace(providerInfo.Person.Email) ? null : providerInfo.Person.Email.ToLowerInvariant().Trim();
            providerInfo.Person.FirstName = string.IsNullOrWhiteSpace(providerInfo.Person.FirstName) ? null : FirstToCamelCase(providerInfo.Person.FirstName.Trim());
            providerInfo.Person.LastName = string.IsNullOrWhiteSpace(providerInfo.Person.LastName) ? null : FirstToCamelCase(providerInfo.Person.LastName.Trim());
            providerInfo.Person.UidSerie = string.IsNullOrWhiteSpace(providerInfo.Person.UidSerie) ? null : providerInfo.Person.UidSerie.Trim();

            if (providerInfo.Person.Address != null)
            {
                providerInfo.Person.Address.Apartment = string.IsNullOrWhiteSpace(providerInfo.Person.Address.Apartment) ? null : FirstToCapital(providerInfo.Person.Address.Apartment.Trim());
                providerInfo.Person.Address.Floor = string.IsNullOrWhiteSpace(providerInfo.Person.Address.Floor) ? null : providerInfo.Person.Address.Floor.Trim();
                providerInfo.Person.Address.Neighborhood = string.IsNullOrWhiteSpace(providerInfo.Person.Address.Neighborhood) ? null : FirstToCapital(providerInfo.Person.Address.Neighborhood.Trim());
                providerInfo.Person.Address.Street = string.IsNullOrWhiteSpace(providerInfo.Person.Address.Street) ? null : FirstToCapital(providerInfo.Person.Address.Street.Trim());
                providerInfo.Person.Address.StreetNumber = string.IsNullOrWhiteSpace(providerInfo.Person.Address.StreetNumber) ? null : providerInfo.Person.Address.StreetNumber.Trim();
                providerInfo.Person.Address.ZipCode = string.IsNullOrWhiteSpace(providerInfo.Person.Address.ZipCode) ? null : providerInfo.Person.Address.ZipCode.Trim();
            }

            if (providerInfo.Person.Phones.Count <= 0)
            {
                return;
            }

            foreach (var phoneDto in providerInfo.Person.Phones)
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

        private static Specification<Provider> GetProviderSpecification(IEnumerable<FilterInfo> filterInfo)
        {
            Specification<Provider> spec = new TrueSpecification<Provider>();
            if (filterInfo != null)
            {
                foreach (var info in filterInfo)
                {
                    if (info.Spec.Equals(SpecFilter.Provider.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                    {
                        spec &= SpecProviderByFullSearch(info.Value);
                    }

                    if (info.Spec.Equals(SpecFilter.Provider.Location, StringComparison.InvariantCultureIgnoreCase))
                    {
                        spec &= SpecProviderByLocation(info.Value);
                    }

                    if (info.Spec.Equals(SpecFilter.Provider.Uid, StringComparison.InvariantCultureIgnoreCase))
                    {
                        spec &= SpecProviderByUidSearch(info.Value);
                    }
                }
            }

            return spec;
        }

        private static Specification<Provider> SpecProviderFilterByUid(string uidCode, string uidSerie)
        {
            return
              new DirectSpecification<Provider>(
                c =>
                c.Person.UidSerie.Equals(uidSerie, StringComparison.InvariantCultureIgnoreCase)
                && c.Person.UidCode.Equals(uidCode, StringComparison.InvariantCultureIgnoreCase));
        }

        private static Specification<Provider> SpecProviderByFullSearch(string value)
        {
            Specification<Provider> result = new DirectSpecification<Provider>(c => c.Person.LastName.ToLower().Contains(value.ToLower()));
            result |= new DirectSpecification<Provider>(c => c.Person.FirstName.ToLower().Contains(value.ToLower()));            
            result |= new DirectSpecification<Provider>(c => c.Person.Email.ToLower().Contains(value.ToLower()));
            return result;
        }

        private static Specification<Provider> SpecProviderByLocation(string locationId)
        {
            var locationIdValue = int.Parse(locationId);
            return new AndSpecification<Provider>(
              new DirectSpecification<Provider>(c => c.Person.AddressId.HasValue),
              new OrSpecification<Provider>(
                new DirectSpecification<Provider>(c => c.Person.Address.LocationId.Equals(locationIdValue)),
                new AndSpecification<Provider>(
                  new DirectSpecification<Provider>(c => c.Person.Address.Location.ParentLocationId.HasValue),
                  new DirectSpecification<Provider>(c => ((int)c.Person.Address.Location.ParentLocationId).Equals(locationIdValue)))));
        }

        private static Specification<Provider> SpecProviderByUidSearch(string value)
        {
            Specification<Provider> result = new DirectSpecification<Provider>(c => c.Person.UidSerie.ToLower().Contains(value.ToLower()));
            return result;
        }

        private Provider NewProvider(ProviderDto providerInfo)
        {
            var existingProvider = GetProvider(providerInfo.Person.UidCode, providerInfo.Person.UidSerie);
            if (existingProvider != null)
            {
                throw BusinessRulesCode.ProviderExists.NewBusinessException();
            }

            PrepareProvider(providerInfo);                              

            var personSvc = new PersonService(context);
            var person = personSvc.GetOrNewPerson(providerInfo.Person, true);
            var now = DateTimeOffset.UtcNow;
            var provider = new Provider
            {
                Id = person.Id,
                Name = providerInfo.Name,                
                Person = person,
                CreatedDate = now              
            };
                                          
            ViewProvider.Add(provider);

            return provider;
        }
    }
}
