namespace SAC.Stock.Service.LocationContext
{
    using Seed.NLayer.Data;
    using Domain.LocationContext;
    using BaseDto;
    using System.Collections.Generic;
    using System.Data;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using System.Linq;

    internal class LocationApplicationService : ILocationApplicationService
    {
        public IDataContext StockCtx { get; set; }

        public LocationDto AddLocation(LocationDto locationInfo)
        {
            var svc = new LocationService(StockCtx);
            return LocationToDto(svc.AddLocation(locationInfo));
        }

        public LocationDto ModifyLocation(LocationDto locationInfo)
        {
            var svc = new LocationService(StockCtx);
            return LocationToDto(svc.ModifyLocation(locationInfo));
        }

        public ICollection<LocationDto> QueryChildrenLocation(int? locationId = null, bool sorted = false)
        {
            var svc = new LocationService(StockCtx);
            return LocationsToDto(svc.QueryChildrenLocation(locationId, sorted));
        }

        public ICollection<LocationDto> QueryLocationCountry()
        {
            var svc = new LocationService(StockCtx);
            return LocationsToDto(svc.QueryLocationCountry());
        }

        public ICollection<LocationDto> QueryLocation(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null)
        {
            try
            {
                var svc = new LocationService(StockCtx);
                return LocationsToDto(svc.QueryLocation(pageIndex, pageSize, sortInfo, filterInfo));
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        public LocationDto GetLocation(int locationId)
        {
            var svc = new LocationService(StockCtx);
            return LocationToDto(svc.GetLocation(locationId));
        }

        public LocationDto GetLocation(string code)
        {
            var svc = new LocationService(StockCtx);
            return LocationToDto(svc.GetLocation(code));
        }

        public LocationDto GetLocation(string name, string locTypeCode)
        {
            var svc = new LocationService(StockCtx);
            return LocationToDto(svc.GetLocation(name, locTypeCode));
        }

        private static LocationDto LocationToDto(Location location)
        {
            return ConstructLocation(location);
        }

        private static ICollection<LocationDto> LocationsToDto(IEnumerable<Location> locations)
        {
            var result = locations.Select(ConstructLocation).ToList();
            return result;
        }

        private static LocationDto ConstructLocation(Location location)
        {
            return location == null
                ? null
                : new LocationDto
                {                                        
                    Code = location.Code,
                    Description = location.Description,
                    Id = location.Id,
                    LocationTypeCode = location.LocationTypeCode,
                    Name = location.Name,
                    ParentLocationId = location.ParentLocationId,
                    ParentLocation = ConstructLocation(location.ParentLocation)                    
                };
        }
    }
}