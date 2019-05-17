namespace SAC.Stock.Domain.LocationContext
{
    using System.Globalization;
    using System;
    using System.Linq;    
    using Seed.NLayer.Data;
    using Seed.NLayer.Domain;
    using Seed.NLayer.ExceptionHandling;
    using Code;
    using Service.BaseDto;
    using SAC.Seed.NLayer.Domain.Specification;
    using System.Collections.Generic;
    using SAC.Seed.NLayer.Data.Ordering;

    internal class LocationService
    {
        private readonly IDataContext context;

        public LocationService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Location, int> ViewLocations
        {
            get
            {
                return context.GetView<Location, int>();
            }
        }

        private IDataView<Address, int> ViewAddress
        {
            get
            {
                return this.context.GetView<Address, int>();
            }
        }

        public Location AddLocation(LocationDto locationInfo)
        {
            if ((locationInfo.Code != null) && ViewLocations.Exists(x => x.Code.Equals(locationInfo.Code, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw BusinessRulesCode.LocationExists.NewBusinessException();
            }

            TrimLocation(locationInfo);
            var location = locationInfo.AdaptToLocation();
            location.Code = locationInfo.Code ?? Guid.NewGuid().ToString();
            ViewLocations.Add(location);

            try
            {
                this.context.ApplyChanges();
            }
            catch
            {
                var locationCode = CodeConst.LocationType.Values().FirstOrDefault(l => l.Code.Equals(location.LocationTypeCode));
                var strLocationCodeName = (locationCode == null) ? "Ubicación" : locationCode.Name;
                throw new BusinessRulesException(string.Format("Falló la carga {0}", strLocationCodeName));
            }

            return location;
        }

        public Location ModifyLocation(LocationDto locationInfo)
        {
            var location = GetLocation(locationInfo.Id);
            UpdateLocation(locationInfo, location);

            try
            {
                context.ApplyChanges();
            }
            catch
            {
                var locationCode = CodeConst.LocationType.Values().FirstOrDefault(l => l.Code.Equals(location.LocationTypeCode));
                var strLocationCodeName = (locationCode == null) ? "Ubicación" : locationCode.Name;
                throw new BusinessRulesException(string.Format("Falló la carga {0}", strLocationCodeName));
            }

            return location;
        }

        public Location UpdateLocation(LocationDto locationInfo, Location location = null)
        {
            TrimLocation(locationInfo);
            location = location ?? GetLocation(locationInfo.Id);
            if (location == null)
            {
                throw BusinessRulesCode.LocationNotExists.NewBusinessException();
            }

            location.Description = locationInfo.Description;
            location.LocationTypeCode = locationInfo.LocationTypeCode;
            location.Name = locationInfo.Name;
            location.ParentLocationId = locationInfo.ParentLocationId;            
            ViewLocations.Modify(location);
            return location;
        }

        public IEnumerable<Location> QueryChildrenLocation(int? locationId, bool sorted)
        {
            return sorted
                     ? ViewLocations.Query(new IOrderByExpression<Location>[] { new OrderByExpression<Location, string>(r => r.Name) }, LocationsByParent(locationId))
                     : ViewLocations.Query(LocationsByParent(locationId));
        }

        public IEnumerable<Location> QueryLocation(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null)
        {
            var orderByExpressions = new List<IOrderByExpression<Location>>();

            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Location.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Location, string>(r => r.Name, info.Direction));
                }
            }

            return
              ViewLocations.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetLocationSpecification(filterInfo));
        }

        public IEnumerable<Location> QueryLocationCountry()
        {
            return ViewLocations.Query(l => l.LocationTypeCode.Equals(CodeConst.LocationType.Country.Code));
        }

        public Location GetLocation(int locationId)
        {
            return ViewLocations.Query(l => l.Id == locationId).FirstOrDefault();
        }

        public Location GetLocation(string code)
        {
            return ViewLocations.GetFirst(l => l.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));
        }

        public Location GetLocation(string name, string locTypeCode)
        {
            return ViewLocations.GetFirst(l => l.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && l.LocationTypeCode.Equals(locTypeCode, StringComparison.InvariantCultureIgnoreCase));
        }

        public Address UpdateOrNewAddress(AddressDto addressInfo, Address addressEntity = null)
        {
            Address address = null;
            if (addressInfo == null)
            {
                if (addressEntity != null)
                {
                    ViewAddress.Remove(addressEntity);
                }
            }
            else
            {
                address = addressEntity ?? new Address();

                address.LocationId = addressInfo.LocationId;
                address.Apartment = string.IsNullOrWhiteSpace(addressInfo.Apartment) ? null : addressInfo.Apartment.Trim();
                address.Floor = string.IsNullOrWhiteSpace(addressInfo.Floor) ? null : addressInfo.Floor.Trim();
                address.Neighborhood = string.IsNullOrWhiteSpace(addressInfo.Neighborhood) ? null : addressInfo.Neighborhood.Trim();
                address.Street = string.IsNullOrWhiteSpace(addressInfo.Street) ? null : addressInfo.Street.Trim();
                address.StreetNumber = string.IsNullOrWhiteSpace(addressInfo.StreetNumber) ? null : addressInfo.StreetNumber.Trim();
                address.ZipCode = string.IsNullOrWhiteSpace(addressInfo.ZipCode) ? null : addressInfo.ZipCode.Trim();
                if (address.Id > 0)
                {
                    if (!ViewLocations.Exists(x => x.Id == addressInfo.LocationId))
                    {
                        throw BusinessRulesCode.LocationNotExists.NewBusinessException();
                    }

                    if (ViewLocations.Get(addressInfo.LocationId).LocationTypeCode != "City")
                    {
                        throw BusinessRulesCode.LocationNotCity.NewBusinessException();
                    }

                    ViewAddress.Modify(address);
                }
                else
                {
                    ViewAddress.Add(address);
                }
            }

            return address;
        }

        private static Specification<Location> GetLocationSpecification(IEnumerable<FilterInfo> filterInfo)
        {
            Specification<Location> spec = new TrueSpecification<Location>();
            if (filterInfo == null)
            {
                return spec;
            }

            foreach (var info in filterInfo)
            {
                if (info.Spec.Equals(SpecFilter.Location.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecLocationByFullSearch(info.Value);
                }

                if (info.Spec.Equals(SpecFilter.Location.State, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecLocationByState(int.Parse(info.Value));
                }
            }

            return spec;
        }

        private static Specification<Location> SpecLocationByFullSearch(string value)
        {
            return new OrSpecification<Location>(
              new DirectSpecification<Location>(c => c.Name.Contains(value)),
              new DirectSpecification<Location>(c => c.Description.Contains(value)));
        }

        private static Specification<Location> SpecLocationByState(int stateId)
        {
            return new DirectSpecification<Location>(c => c.ParentLocationId == stateId);
        }

        private static ISpecification<Location> LocationsByParent(int? parentLocationId = null)
        {
            var result = parentLocationId == null
                           ? new DirectSpecification<Location>(r => r.ParentLocationId.Equals(null))
                           : new DirectSpecification<Location>(r => r.ParentLocationId == parentLocationId);
            return result;
        }

        private static void TrimLocation(LocationDto locationInfo)
        {
            locationInfo.Code = string.IsNullOrWhiteSpace(locationInfo.Code) ? null : locationInfo.Code.Trim();
            locationInfo.Name = string.IsNullOrWhiteSpace(locationInfo.Name) ? null : FirstToCapital(locationInfo.Name.Trim());
            locationInfo.Description = string.IsNullOrWhiteSpace(locationInfo.Description) ? null : FirstToCapital(locationInfo.Description.Trim());
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
    }
}
