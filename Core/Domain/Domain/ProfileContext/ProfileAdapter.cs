namespace SAC.Stock.Domain.ProfileContext
{
    using SAC.Stock.Service.ProfileContext;
    internal static class ProfileAdapter
    {
        public static void AdaptToProfile(this ProfileDto dto, Profile to)
        {
            if ((dto == null) || (to == null))
            {
                return;
            }

            to.Name = dto.Name;
            to.Description = dto.Description;
            to.Code = dto.Code;
            to.Hierarchy = dto.Hierarchy;
            to.Scope = dto.Scope;
        }

        public static Profile AdaptToProfile(this ProfileDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            // No se tienen en cuenta los roles.
            return new Profile
            {
                Id = dto.Id,
                Description = dto.Description,
                Name = dto.Name,
                Code = dto.Code,
                Hierarchy = dto.Hierarchy,
                Scope = dto.Scope
            };
        }

        public static ProfileDto AdaptToProfileDto(this Profile entity)
        {
            if (entity == null)
            {
                return null;
            }

            // No se tienen en cuenta los roles.
            return new ProfileDto
            {
                Id = entity.Id,
                Description = entity.Description,
                Name = entity.Name,
                Code = entity.Code,
                Hierarchy = entity.Hierarchy,
                Scope = entity.Scope
            };
        }

        public static RolesComposition AdaptToRolesComposition(this RolesCompositionDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            // No se aplica el profile. Pro regla de negocio se aplica en el metodo: NewRoleComposition. 
            // Esto hay que revisarlo.
            return new RolesComposition
            {
                Id = dto.Id,
                Hierarchy = dto.Hierarchy,
                CriticalRole = dto.CriticalRole,
                RoleCode = dto.RoleCode
            };
        }

        public static RolesCompositionDto AdaptToRolesCompositionDto(this RolesComposition dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new RolesCompositionDto
            {
                Id = dto.Id,
                Hierarchy = dto.Hierarchy,
                CriticalRole = dto.CriticalRole,
                RoleCode = dto.RoleCode,
                Profile = dto.Profile.AdaptToProfileDto()
            };
        }
    }
}
