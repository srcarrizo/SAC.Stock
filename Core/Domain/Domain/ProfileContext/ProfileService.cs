namespace SAC.Stock.Domain.ProfileContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.Data.Ordering;
    using SAC.Seed.NLayer.Domain;
    using SAC.Seed.NLayer.Domain.Specification;
    using SAC.Stock.Code;
    using SAC.Stock.Service.ProfileContext;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    internal class ProfileService
    {
        private readonly IDataContext context;
        public ProfileService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<RolesComposition, int> ViewRolesComposition
        {
            get
            {
                return context.GetView<RolesComposition, int>();
            }
        }

        private IDataView<Profile, int> ViewProfile
        {
            get
            {
                return context.GetView<Profile, int>();
            }
        }

        public Profile AddProfile(ProfileDto profileInfo)
        {
            var profile = NewProfile(profileInfo);
            ViewProfile.Add(profile);

            foreach (var rolesCompositionDto in profileInfo.Roles)
            {
                AddRoleComposition(rolesCompositionDto, profile);
            }

            context.ApplyChanges();

            return profile;
        }

        public RolesComposition AddRoleComposition(RolesCompositionDto rolesCompositionInfo, Profile profileInfo)
        {
            var roleComposition = NewRoleComposition(rolesCompositionInfo, profileInfo);
            ViewRolesComposition.Add(roleComposition);

            return roleComposition;
        }

        public Profile ModifyProfile(ProfileDto profileInfo)
        {
            var profile = UpdateProfile(profileInfo);
            ViewProfile.Modify(profile);
            context.ApplyChanges();

            return profile;
        }
  
        public Profile UpdateProfile(ProfileDto profileInfo)
        {
            var profile = GetProfile(profileInfo.Id);
            if (profile == null)
            {
                throw BusinessRulesCode.ProfileNotExists.NewBusinessException();
            }

            profileInfo.AdaptToProfile(profile);

            foreach (var rolesCompositionDto in profileInfo.Roles)
            {
                var existingRole = profile.RolesComposition.FirstOrDefault(r => r.RoleCode.Equals(rolesCompositionDto.RoleCode, StringComparison.CurrentCultureIgnoreCase));
                if (existingRole != null)
                {
                    continue;
                }

                AddRoleComposition(rolesCompositionDto, profile);
            }

            var rolesCompositionToDelete = new List<RolesComposition>();
            foreach (var rolesComposition in profile.RolesComposition)
            {
                var deletingRole = profileInfo.Roles.FirstOrDefault(r => r.RoleCode.Equals(rolesComposition.RoleCode, StringComparison.CurrentCultureIgnoreCase));
                if (deletingRole != null)
                {
                    continue;
                }

                rolesCompositionToDelete.Add(rolesComposition);
            }

            if (rolesCompositionToDelete.Count > 0)
            {
                foreach (var rolesComposition in rolesCompositionToDelete)
                {
                    RemoveRolesComposition(rolesComposition);
                }
            }

            return profile;
        }

        public Profile GetProfile(int profileid)
        {
            return ViewProfile.Get(profileid);
        }

        public void RemoveProfile(int profileId)
        {
            var profile = DeleteProfile(profileId);
            ViewProfile.Remove(profile);
            context.ApplyChanges();
        }

        public Profile DeleteProfile(int profileId)
        {
            var profile = GetProfile(profileId);

            var roles = QueryRolesComposition(
              1,
              1000,
              new Collection<SortInfo> { new SortInfo { Direction = OrderDirection.Asc, Field = SortQuery.RolesComposition.Id } },
              new FilterInfo { Spec = SpecFilter.RolesComposition.ProfileId, Value = profile.Id.ToString(CultureInfo.InvariantCulture) });

            foreach (var rolesComposition in roles)
            {
                RemoveRolesComposition(rolesComposition);
            }

            return profile;
        }

        public void RemoveRolesComposition(RolesComposition rolesComposition)
        {
            this.ViewRolesComposition.Remove(rolesComposition);
        }

        public IEnumerable<Profile> QueryProfile(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<Profile>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.Profile.Id, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Profile, int>(pt => pt.Id, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Profile.Code, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Profile, string>(pt => pt.Code, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Profile.Scope, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Profile, string>(pt => pt.Scope, info.Direction));
                }

                if (info.Field.Equals(SortQuery.Profile.Hierarchy, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<Profile, int>(pt => pt.Hierarchy, info.Direction));
                }
            }

            return
                ViewProfile.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetProfileSpecification(filterInfo));
        }

        public IEnumerable<RolesComposition> QueryRolesComposition(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, FilterInfo filterInfo)
        {
            var orderByExpressions = new List<IOrderByExpression<RolesComposition>>();
            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.RolesComposition.Id, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<RolesComposition, int>(pt => pt.Id, info.Direction));
                }

                if (info.Field.Equals(SortQuery.RolesComposition.ProfileId, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<RolesComposition, int>(pt => pt.ProfileId, info.Direction));
                }

                if (info.Field.Equals(SortQuery.RolesComposition.RoleCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<RolesComposition, string>(pt => pt.RoleCode, info.Direction));
                }

                if (info.Field.Equals(SortQuery.RolesComposition.Hierarchy, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<RolesComposition, int>(pt => pt.Hierarchy, info.Direction));
                }

                if (info.Field.Equals(SortQuery.RolesComposition.CriticalRole, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<RolesComposition, bool>(pt => pt.CriticalRole, info.Direction));
                }
            }

            return
              ViewRolesComposition.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetRolesCompositionSpecification(filterInfo));
        }

        public IEnumerable<Profile> QueryProfile(FilterInfo filterInfo = null)
        {
            return context.GetView<Profile, int>().Query(GetProfileSpecification(filterInfo));
        }

        public IEnumerable<RolesComposition> QueryRolesComposition(FilterInfo filterInfo = null)
        {
            return context.GetView<RolesComposition, int>().Query(GetRolesCompositionSpecification(filterInfo));
        }

        private static Specification<RolesComposition> GetRolesCompositionSpecification(FilterInfo filterInfo)
        {
            Specification<RolesComposition> spec = new TrueSpecification<RolesComposition>();
            if (filterInfo != null)
            {
                if (filterInfo.Spec.Equals(SpecFilter.RolesComposition.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecRolesCompositionByFullSearch(filterInfo.Value);
                }

                if (filterInfo.Spec.Equals(SpecFilter.RolesComposition.ProfileId, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecRolesCompositionByProfileId(filterInfo.Value);
                }

                if (filterInfo.Spec.Equals(SpecFilter.RolesComposition.RoleCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecRolesCompositionByRoleCode(filterInfo.Value);
                }

                if (filterInfo.Spec.Equals(SpecFilter.RolesComposition.CriticalRole, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecRolesCompositionByCriticalRole(filterInfo.Value);
                }
            }

            return spec;
        }

        private static Specification<RolesComposition> SpecRolesCompositionByProfileId(string value)
        {
            int valueInt;
            return int.TryParse(value, out valueInt) ? new DirectSpecification<RolesComposition>(e => e.ProfileId.Equals(valueInt)) : null;
        }

        private static Specification<RolesComposition> SpecRolesCompositionByCriticalRole(string value)
        {
            bool valueBool;
            return bool.TryParse(value, out valueBool) ? new DirectSpecification<RolesComposition>(e => e.CriticalRole.Equals(valueBool)) : null;
        }

        private static Specification<RolesComposition> SpecRolesCompositionByRoleCode(string value)
        {
            return new DirectSpecification<RolesComposition>(e => e.RoleCode.ToLower().Contains(value.ToLower()));
        }

        private static Specification<RolesComposition> SpecRolesCompositionByFullSearch(string value)
        {
            int valueId;
            bool valueBool;

            Specification<RolesComposition> result = new DirectSpecification<RolesComposition>(e => e.RoleCode.ToLower().Contains(value.ToLower()));
            if (int.TryParse(value, out valueId))
            {
                result |= new DirectSpecification<RolesComposition>(e => e.ProfileId.Equals(valueId));
            }

            if (bool.TryParse(value, out valueBool))
            {
                result |= new DirectSpecification<RolesComposition>(e => e.CriticalRole.Equals(valueBool));
            }

            return result;
        }

        private static Specification<Profile> GetProfileSpecification(FilterInfo filterInfo)
        {
            Specification<Profile> spec = new TrueSpecification<Profile>();
            if (filterInfo != null)
            {
                if (filterInfo.Spec.Equals(SpecFilter.Profile.FullSearch, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecProfileByFullSearch(filterInfo.Value);
                }

                if (filterInfo.Spec.Equals(SpecFilter.Profile.Code, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecProfileByCode(filterInfo.Value);
                }

                if (filterInfo.Spec.Equals(SpecFilter.Profile.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecProfileByName(filterInfo.Value);
                }

                if (filterInfo.Spec.Equals(SpecFilter.Profile.Scope, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecProfileByScope(filterInfo.Value);
                }
            }

            return spec;
        }

        private static Specification<Profile> SpecProfileByFullSearch(string value)
        {
            int valueId;
            Specification<Profile> result = new DirectSpecification<Profile>(e => e.Name.ToLower().Contains(value.ToLower()));
            result |= new DirectSpecification<Profile>(e => e.Description.Contains(value.ToLower()));
            result |= new DirectSpecification<Profile>(e => e.Scope.Contains(value.ToLower()));
            if (int.TryParse(value, out valueId))
            {
                result |= new DirectSpecification<Profile>(e => e.Id.Equals(valueId));
                result |= new DirectSpecification<Profile>(e => e.Hierarchy.Equals(valueId));
            }

            return result;
        }

        private static Specification<Profile> SpecProfileByCode(string value)
        {
            return new DirectSpecification<Profile>(e => e.Code.ToLower().Contains(value.ToLower()));
        }

        private static Specification<Profile> SpecProfileByName(string value)
        {
            return new DirectSpecification<Profile>(e => e.Name.ToLower().Contains(value.ToLower()));
        }

        private static Specification<Profile> SpecProfileByScope(string value)
        {
            return new DirectSpecification<Profile>(e => e.Scope.ToLower().Contains(value.ToLower()));
        }

        private static Profile NewProfile(ProfileDto profileInfo)
        {
            var profile = profileInfo.AdaptToProfile();
            profile.GenerateNewIdentity();

            return profile;
        }
 
        private RolesComposition NewRoleComposition(RolesCompositionDto rolesCompositionInfo, Profile profile)
        {
            var roleComposition = rolesCompositionInfo.AdaptToRolesComposition();
            roleComposition.Profile = profile;
            roleComposition.GenerateNewIdentity();

            return roleComposition;
        }
    }
}