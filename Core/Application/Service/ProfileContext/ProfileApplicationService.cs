namespace SAC.Stock.Service.ProfileContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Domain.ProfileContext;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    internal class ProfileApplicationService : IProfileApplicationService
    {
        public IDataContext StockCtx { get; set; }

        public ProfileDto AddProfile(ProfileDto profileInfo)
        {
            var svc = new ProfileService(StockCtx);

            return svc.AddProfile(profileInfo).AdaptToProfileDto();
        }

        public ProfileDto ModifyProfile(ProfileDto profileInfo)
        {
            var svc = new ProfileService(StockCtx);
            return svc.ModifyProfile(profileInfo).AdaptToProfileDto();
        }

        public ProfileDto GetProfile(int profileId)
        {
            var svc = new ProfileService(StockCtx);
            return svc.GetProfile(profileId).AdaptToProfileDto();
        }

        public void RemoveProfile(int profileId)
        {
            var svc = new ProfileService(StockCtx);
            svc.RemoveProfile(profileId);
        }

        public ICollection<ProfileDto> QueryProfile(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo = null)
        {
            try
            {
                var svc = new ProfileService(StockCtx);
                return this.QueryToDto(svc.QueryProfile(pageIndex, pageSize, sortInfo, filterInfo).ToList());
            }
            catch (DataException)
            {
                throw new DistributedServiceException(BusinessRulesCode.FailedConnectDatabase.Message, BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        public ICollection<ProfileDto> QueryProfile(FilterInfo filterInfo = null)
        {
            try
            {
                var svc = new ProfileService(StockCtx);
                return this.QueryToDto(svc.QueryProfile(filterInfo).ToList());
            }
            catch (DataException)
            {
                throw new DistributedServiceException(
                  BusinessRulesCode.FailedConnectDatabase.Message,
                  BusinessRulesCode.FailedConnectDatabase.Code);
            }
        }

        private ICollection<ProfileDto> QueryToDto(ICollection<Profile> profiles)
        {
            var profilesInfo = profiles.Select(r => r.AdaptToProfileDto()).ToList();

            foreach (var profile in profiles)
            {
                foreach (var profileDto in profilesInfo)
                {
                    if (profile.Id == profileDto.Id)
                    {
                        profileDto.Roles = profile.RolesComposition.Select(r => r.AdaptToRolesCompositionDto()).ToList();
                    }
                }
            }

            return profilesInfo.ToList();
        }
    }
}
